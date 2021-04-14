using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using McFisher;
using Timer = System.Windows.Forms.Timer;

// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

namespace Fisher
{
    public partial class Form1 : Form
    {

        private void FetchReadMe()
        {
            try
            {
                WebClient client = new WebClient();
                var data = client.DownloadString("https://raw.githubusercontent.com/FairfieldTekLLC/McFishing/master/README.md");
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        ///     Used to hook the keyboard
        /// </summary>
        private readonly GlobalKeyboardHook _globalKeyboardHook;

        /// <summary>
        ///     Timer used to grab the screen every 100 ms
        /// </summary>
        private readonly Timer _pixelCheck;

        private object locker = new object();

        private void SetRunning(bool flag)
        {
            lock (locker)
            {
                __running = flag;
            }
        }

        private bool GetRunning()
        {
            lock (locker)
                return __running;
        }


        /// <summary>
        ///     Flag to determine if it is processing an image
        /// </summary>
        private bool __running;

        public Form1()
        {
            InitializeComponent();
            //Set keyboard hook
            _globalKeyboardHook = new GlobalKeyboardHook();
            //Hook to keypress
            _globalKeyboardHook.KeyboardPressed += _globalKeyboardHook_KeyboardPressed;
            //Setup timer for checking screen
            _pixelCheck = new Timer { Interval = (int)TimeSpan.FromMilliseconds(10).TotalMilliseconds };
            //Setup timer on tick event
            _pixelCheck.Tick += PixelCheckTick;
        }

        /// <summary>
        ///     Fires whenever there is keyboard input, regardless of what application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _globalKeyboardHook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            switch (e.KeyboardData.VirtualCode)
            {
                //Ctrl F2
                case 113:
                    {
                        if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
                        {
                            _pixelCheck.Start();
                            lblStatus.Text = @"Status: Running";
                        }

                        break;
                    }
                //Ctrl F3
                case 114:
                    {
                        if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
                        {
                            _pixelCheck.Stop();
                            lblStatus.Text = @"Status: Stopped";
                        }

                        break;
                    }
            }
        }



        public Bitmap CaptureRegion(Rectangle region)
        {
            Bitmap result;

            IntPtr desktophWnd = GetDesktopWindow();
            IntPtr desktopDc = GetWindowDC(desktophWnd);
            IntPtr memoryDc = CreateCompatibleDC(desktopDc);
            IntPtr bitmap = CreateCompatibleBitmap(desktopDc, region.Width, region.Height);
            IntPtr oldBitmap = SelectObject(memoryDc, bitmap);
            bool success = BitBlt(memoryDc, 0, 0, region.Width, region.Height, desktopDc, region.Left, region.Top, SRCCOPY | CAPTUREBLT);
            try
            {
                if (!success)
                {
                    throw new Exception("Failed to capture image!");
                }

                result = Image.FromHbitmap(bitmap);
            }
            finally
            {
                SelectObject(memoryDc, oldBitmap);
                DeleteObject(bitmap);
                DeleteDC(memoryDc);
                ReleaseDC(desktophWnd, desktopDc);
            }

            return result;
        }


        //Memory Leak!
        private Bitmap GetBitMap(Rectangle captureRectangle)
        {
            Bitmap captureBitmap = new Bitmap(captureRectangle.Width, captureRectangle.Height, PixelFormat.Format32bppArgb);
            Graphics captureGraphics = null;
            try
            {
                //Get a graphics adapter
                captureGraphics = Graphics.FromImage(captureBitmap);

                //Capture image from screen
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

                return captureBitmap;
            }
            catch (Exception e)
            {

            }
            finally
            {
                captureGraphics?.Dispose();
            }

            return null;
        }



        /// <summary>
        ///     Retrieves a bitmap for the portion of the screen which is in the captureRectangle.
        ///     Then looks for a red color of the bobber, if it doesn't find it, it clicks the mouse.
        /// </summary>
        /// <param name="captureRectangle"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        private void CheckImage(Rectangle captureRectangle)
        {
            bool foundBobber = false;
            SetRunning(true);
            using (Bitmap captureBitmap = CaptureRegion(captureRectangle))
            {
                if (captureBitmap == null)
                {
                    SetRunning(false);
                    return;
                }

                //For each pixel in the image
                for (int j = 0; j < captureRectangle.Height; j++)
                    for (int i = 0; i < captureRectangle.Width; i++)
                    {
                        //Get the pixel
                        Color color = captureBitmap.GetPixel(j, i);
                        //Is it red like the bobber?
                        if (color.R > 120 && color.B < 30 && color.G < 30)
                        {
                            foundBobber = true;
                            break;
                        }

                        if (foundBobber)
                            break;
                    }

                //Grab the old preview image.
                Image old = pbPreview.Image;
                //Clear the preview image
                pbPreview.Image = null;
                //Dispose of the old preview image
                old?.Dispose();
                //Create a new preview image from the screen grab
                pbPreview.Image = (Bitmap)captureBitmap.Clone();
                pbPreview.SizeMode = PictureBoxSizeMode.StretchImage;
                if (foundBobber)
                {
                    LogConsole("Found Bobber!");
                }
                else
                {
                    LogConsole("Bobber Missing!");
                    RighClickWindow();
                }

                //Clear the running flag.
                SetRunning(false);
            }
            //Force Garbage Collection
            GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        }



        public void RighClickWindow()
        {
            try
            {
                MouseEvent(MouseEventFlags.RightDown);
                Thread.Sleep(10);
                MouseEvent(MouseEventFlags.RightUp);
                Thread.Sleep(10);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public new void Dispose()
        {
            //Dispose of the keyboard hook
            _globalKeyboardHook.Dispose();
            //Stop the screen grab timer.
            _pixelCheck.Stop();
            _pixelCheck.Dispose();
            //Normal Dispose.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Draws a rectangle at the specified position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void DrawRectangle(int x, int y, int width, int height)
        {
            IntPtr desktop = GetDC(IntPtr.Zero);
            using (Graphics g = Graphics.FromHdc(desktop))
            {
                using (Pen pen = new Pen(Brushes.Chartreuse))
                    g.DrawRectangle(pen, x, y, width, height);
            }

            ReleaseDC(IntPtr.Zero, desktop);
        }

        /// <summary>
        ///     Gets the current mouse position
        /// </summary>
        /// <returns></returns>
        public static MousePoint GetCursorPosition()
        {
            bool gotPoint = GetCursorPos(out MousePoint currentMousePoint);
            if (!gotPoint)
                currentMousePoint = new MousePoint(0, 0);
            return currentMousePoint;
        }

        /// <summary>
        ///     Logs a comment to the console.
        /// </summary>
        /// <param name="msg"></param>
        public void LogConsole(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogConsole), msg);
                return;
            }

            txtConsole.Text = "\r\n" + msg + txtConsole.Text;
            string text = txtConsole.Text;
            if (text.Length > 1000)
                txtConsole.Text = txtConsole.Text.Substring(0, 1000);
        }

        /// <summary>
        ///     Fires a mouse event at the current mouse position.
        /// </summary>
        /// <param name="value"></param>
        public static void MouseEvent(MouseEventFlags value)
        {
            MousePoint position = GetCursorPosition();

            mouse_event((int)value, position.X, position.Y, 0, 0);
        }

        /// <summary>
        ///     This is fired from the timer, it gets the capture rectangle, draws a green rectangle on screen
        ///     then if it is not processing a image, it processes it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PixelCheckTick(object sender, EventArgs e)
        {
            int height = 200;
            int width = 200;
            Point cursor = new Point();
            GetCursorPos(ref cursor);
            Rectangle captureRectangle = new Rectangle();
            captureRectangle.X = cursor.X - width / 2;
            captureRectangle.Y = cursor.Y - height / 2;
            captureRectangle.Width = width;
            captureRectangle.Height = height;

            DrawRectangle(captureRectangle.X, captureRectangle.Y, captureRectangle.Width, captureRectangle.Height);

            if (!GetRunning())
                new Thread(() => { CheckImage(captureRectangle); }).Start();
        }

        #region DllImports

        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [DllImport("User32.dll")]
        // ReSharper disable once IdentifierTypo
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("User32.dll")]
        // ReSharper disable once IdentifierTypo
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public int X;
            public int Y;

            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

#pragma warning disable 649


        [StructLayout(LayoutKind.Explicit)]

        // ReSharper disable once IdentifierTypo
        // ReSharper disable once InconsistentNaming
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)] public MOUSEINPUT Mouse;
        }

        // ReSharper disable once IdentifierTypo
        // ReSharper disable once InconsistentNaming
        internal struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

#pragma warning restore 649
        const int SRCCOPY = 0xcc0020; // we want to copy an in memory image

        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int nxDest, int nyDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int width, int nHeight);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);


        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);



        const int CAPTUREBLT = 0x40000000;

        #endregion

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/FairfieldTekLLC/McFishing");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FetchReadMe();
        }
    }
}
