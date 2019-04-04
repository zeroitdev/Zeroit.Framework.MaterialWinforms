// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialForm.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Zeroit.Framework.MaterialWinforms.Animations;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialForm.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public class MaterialForm : Form, IMaterialControl
    {



        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        [Browsable(false)]
        public int Depth { get; set; }
        /// <summary>
        /// Gets the skin manager.
        /// </summary>
        /// <value>The skin manager.</value>
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        /// <summary>
        /// Gets or sets the state of the mouse.
        /// </summary>
        /// <value>The state of the mouse.</value>
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        /// <summary>
        /// Gets or sets the border style of the form.
        /// </summary>
        /// <value>The form border style.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new FormBorderStyle FormBorderStyle { get { return base.FormBorderStyle; } set { base.FormBorderStyle = value; } }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MaterialForm"/> is sizable.
        /// </summary>
        /// <value><c>true</c> if sizable; otherwise, <c>false</c>.</value>
        public bool Sizable { get; set; }
        /// <summary>
        /// The state changing
        /// </summary>
        private bool _StateChanging;

        /// <summary>
        /// The m last state
        /// </summary>
        private FormWindowState mLastState;


        /// <summary>
        /// The side drawer
        /// </summary>
        private MaterialSideDrawer _SideDrawer;
        /// <summary>
        /// Gets or sets the side drawer.
        /// </summary>
        /// <value>The side drawer.</value>
        public MaterialSideDrawer SideDrawer
        {
            get
            {
                return _SideDrawer;
            }
            set
            {
                _SideDrawer = value;
                _SideDrawer.onHiddenOnStartChanged += _SideDrawer_onHiddenOnStartChanged;
                _SideDrawer_onHiddenOnStartChanged(_SideDrawer.HiddenOnStart);
            }
        }

        /// <summary>
        /// Sides the drawer on hidden on start changed.
        /// </summary>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        void _SideDrawer_onHiddenOnStartChanged(bool newValue)
        {
            DrawerAnimationTimer.SetProgress(newValue ? 0 : 1);
            Invalidate();
        }

        /// <summary>
        /// The action bar
        /// </summary>
        private MaterialActionBar _ActionBar;
        /// <summary>
        /// Gets or sets the action bar.
        /// </summary>
        /// <value>The action bar.</value>
        public MaterialActionBar ActionBar
        {
            get
            {
                return _ActionBar;
            }
            set
            {
                _ActionBar = value;
                _ActionBar.onSideDrawerButtonClicked += _ActionBar_onSideDrawerButtonClicked;
            }
        }

        /// <summary>
        /// Actions the bar on side drawer button clicked.
        /// </summary>
        public virtual void _ActionBar_onSideDrawerButtonClicked()
        {

            DrawerAnimationTimer.StartNewAnimation(DrawerAnimationTimer.GetProgress() == 0 ? AnimationDirection.In : AnimationDirection.Out);
        }


        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="Msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// Releases the capture.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// Tracks the popup menu ex.
        /// </summary>
        /// <param name="hmenu">The hmenu.</param>
        /// <param name="fuFlags">The fu flags.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="lptpm">The LPTPM.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        /// <summary>
        /// Gets the system menu.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="bRevert">if set to <c>true</c> [b revert].</param>
        /// <returns>IntPtr.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// Monitors from window.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="dwFlags">The dw flags.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        /// <summary>
        /// Gets the monitor information.
        /// </summary>
        /// <param name="hmonitor">The hmonitor.</param>
        /// <param name="info">The information.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX info);

        /// <summary>
        /// The wm nclbuttondown
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;
        /// <summary>
        /// The ht caption
        /// </summary>
        public const int HT_CAPTION = 0x2;
        /// <summary>
        /// The wm mousemove
        /// </summary>
        public const int WM_MOUSEMOVE = 0x0200;
        /// <summary>
        /// The wm lbuttondown
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x0201;
        /// <summary>
        /// The wm lbuttonup
        /// </summary>
        public const int WM_LBUTTONUP = 0x0202;
        /// <summary>
        /// The wm lbuttondblclk
        /// </summary>
        public const int WM_LBUTTONDBLCLK = 0x0203;
        /// <summary>
        /// The wm rbuttondown
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x0204;
        /// <summary>
        /// The wm nccalcsize
        /// </summary>
        public const int WM_NCCALCSIZE = 0x83;
        /// <summary>
        /// The htbottomleft
        /// </summary>
        private const int HTBOTTOMLEFT = 16;
        /// <summary>
        /// The htbottomright
        /// </summary>
        private const int HTBOTTOMRIGHT = 17;
        /// <summary>
        /// The htleft
        /// </summary>
        private const int HTLEFT = 10;
        /// <summary>
        /// The htright
        /// </summary>
        private const int HTRIGHT = 11;
        /// <summary>
        /// The htbottom
        /// </summary>
        private const int HTBOTTOM = 15;
        /// <summary>
        /// The httop
        /// </summary>
        private const int HTTOP = 12;
        /// <summary>
        /// The httopleft
        /// </summary>
        private const int HTTOPLEFT = 13;
        /// <summary>
        /// The httopright
        /// </summary>
        private const int HTTOPRIGHT = 14;
        /// <summary>
        /// The border width
        /// </summary>
        private const int BORDER_WIDTH = 7;
        /// <summary>
        /// The resize dir
        /// </summary>
        private ResizeDirection resizeDir;
        /// <summary>
        /// The button state
        /// </summary>
        protected ButtonState buttonState = ButtonState.None;

        /// <summary>
        /// The WMSZ top
        /// </summary>
        private const int WMSZ_TOP = 3;
        /// <summary>
        /// The WMSZ topleft
        /// </summary>
        private const int WMSZ_TOPLEFT = 4;
        /// <summary>
        /// The WMSZ topright
        /// </summary>
        private const int WMSZ_TOPRIGHT = 5;
        /// <summary>
        /// The WMSZ left
        /// </summary>
        private const int WMSZ_LEFT = 1;
        /// <summary>
        /// The WMSZ right
        /// </summary>
        private const int WMSZ_RIGHT = 2;
        /// <summary>
        /// The WMSZ bottom
        /// </summary>
        private const int WMSZ_BOTTOM = 6;
        /// <summary>
        /// The WMSZ bottomleft
        /// </summary>
        private const int WMSZ_BOTTOMLEFT = 7;
        /// <summary>
        /// The WMSZ bottomright
        /// </summary>
        private const int WMSZ_BOTTOMRIGHT = 8;

        /// <summary>
        /// The aw ver positive
        /// </summary>
        private const int AW_VER_POSITIVE = 0x00000004;
        /// <summary>
        /// The aw ver negative
        /// </summary>
        private const int AW_VER_NEGATIVE = 0x00000008;
        /// <summary>
        /// The aw slide
        /// </summary>
        private const int AW_SLIDE = 0x00040000;
        /// <summary>
        /// The aw hide
        /// </summary>
        private const int AW_HIDE = 0x00010000;
        /// <summary>
        /// The aw activae
        /// </summary>
        private const int AW_ACTIVAE = 0x00020000;
        /// <summary>
        /// The aw blend
        /// </summary>
        private const int AW_BLEND = 0x00080000;
        /// <summary>
        /// The aw center
        /// </summary>
        private const int AW_CENTER = 0x00000010;
        /// <summary>
        /// The aw hor positive
        /// </summary>
        private const int AW_HOR_POSITIVE = 0x00000001;
        /// <summary>
        /// The aw hor negative
        /// </summary>
        private const int AW_HOR_NEGATIVE = 0x00000002;

        /// <summary>
        /// The resizing locations to command
        /// </summary>
        private readonly Dictionary<int, int> resizingLocationsToCmd = new Dictionary<int, int>
        {
            {HTTOP,         WMSZ_TOP},
            {HTTOPLEFT,     WMSZ_TOPLEFT},
            {HTTOPRIGHT,    WMSZ_TOPRIGHT},
            {HTLEFT,        WMSZ_LEFT},
            {HTRIGHT,       WMSZ_RIGHT},
            {HTBOTTOM,      WMSZ_BOTTOM},
            {HTBOTTOMLEFT,  WMSZ_BOTTOMLEFT},
            {HTBOTTOMRIGHT, WMSZ_BOTTOMRIGHT}
        };

        /// <summary>
        /// The status bar button width
        /// </summary>
        protected const int STATUS_BAR_BUTTON_WIDTH = STATUS_BAR_HEIGHT;
        /// <summary>
        /// The status bar height
        /// </summary>
        public const int STATUS_BAR_HEIGHT = 24;

        /// <summary>
        /// The TPM leftalign
        /// </summary>
        private const uint TPM_LEFTALIGN = 0x0000;
        /// <summary>
        /// The TPM returncmd
        /// </summary>
        private const uint TPM_RETURNCMD = 0x0100;

        /// <summary>
        /// The wm syscommand
        /// </summary>
        private const int WM_SYSCOMMAND = 0x0112;
        /// <summary>
        /// The ws minimizebox
        /// </summary>
        private const int WS_MINIMIZEBOX = 0x20000;
        /// <summary>
        /// The ws sysmenu
        /// </summary>
        private const int WS_SYSMENU = 0x00080000;

        /// <summary>
        /// The monitor defaulttonearest
        /// </summary>
        private const int MONITOR_DEFAULTTONEAREST = 2;

        /// <summary>
        /// The sc minimize
        /// </summary>
        private const int SC_MINIMIZE = 0xF020;
        /// <summary>
        /// The sc maximize
        /// </summary>
        private const int SC_MAXIMIZE = 0xF030;
        /// <summary>
        /// The sc restore
        /// </summary>
        private const int SC_RESTORE = 0xf120;

        /// <summary>
        /// Class MONITORINFOEX.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        public class MONITORINFOEX
        {
            /// <summary>
            /// The cb size
            /// </summary>
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
            /// <summary>
            /// The rc monitor
            /// </summary>
            public RECT rcMonitor = new RECT();
            /// <summary>
            /// The rc work
            /// </summary>
            public RECT rcWork = new RECT();
            /// <summary>
            /// The dw flags
            /// </summary>
            public int dwFlags = 0;
            /// <summary>
            /// The sz device
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szDevice = new char[32];
        }

        /// <summary>
        /// Struct RECT
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// The left
            /// </summary>
            public int left;
            /// <summary>
            /// The top
            /// </summary>
            public int top;
            /// <summary>
            /// The right
            /// </summary>
            public int right;
            /// <summary>
            /// The bottom
            /// </summary>
            public int bottom;

            /// <summary>
            /// Widthes this instance.
            /// </summary>
            /// <returns>System.Int32.</returns>
            public int Width()
            {
                return right - left;
            }

            /// <summary>
            /// Heights this instance.
            /// </summary>
            /// <returns>System.Int32.</returns>
            public int Height()
            {
                return bottom - top;
            }
        }

        /// <summary>
        /// Enum ResizeDirection
        /// </summary>
        private enum ResizeDirection
        {
            /// <summary>
            /// The bottom left
            /// </summary>
            BottomLeft,
            /// <summary>
            /// The left
            /// </summary>
            Left,
            /// <summary>
            /// The right
            /// </summary>
            Right,
            /// <summary>
            /// The bottom right
            /// </summary>
            BottomRight,
            /// <summary>
            /// The bottom
            /// </summary>
            Bottom,
            /// <summary>
            /// The none
            /// </summary>
            None
        }

        /// <summary>
        /// Enum ButtonState
        /// </summary>
        protected enum ButtonState
        {
            /// <summary>
            /// The x over
            /// </summary>
            XOver,
            /// <summary>
            /// The maximum over
            /// </summary>
            MaxOver,
            /// <summary>
            /// The minimum over
            /// </summary>
            MinOver,
            /// <summary>
            /// The drawer over
            /// </summary>
            DrawerOver,
            /// <summary>
            /// The menu over
            /// </summary>
            MenuOver,
            /// <summary>
            /// The x down
            /// </summary>
            XDown,
            /// <summary>
            /// The maximum down
            /// </summary>
            MaxDown,
            /// <summary>
            /// The minimum down
            /// </summary>
            MinDown,
            /// <summary>
            /// The drawer down
            /// </summary>
            DrawerDown,
            /// <summary>
            /// The none
            /// </summary>
            None
        }

        /// <summary>
        /// The resize cursors
        /// </summary>
        private readonly Cursor[] resizeCursors = { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };

        /// <summary>
        /// The minimum button bounds
        /// </summary>
        protected Rectangle minButtonBounds;
        /// <summary>
        /// The maximum button bounds
        /// </summary>
        protected Rectangle maxButtonBounds;
        /// <summary>
        /// The x button bounds
        /// </summary>
        protected Rectangle xButtonBounds;
        /// <summary>
        /// The status bar bounds
        /// </summary>
        protected Rectangle statusBarBounds;

        /// <summary>
        /// The maximized
        /// </summary>
        private bool Maximized;
        /// <summary>
        /// The previous size
        /// </summary>
        private Size previousSize;
        /// <summary>
        /// The previous location
        /// </summary>
        private Point previousLocation;
        /// <summary>
        /// The header mouse down
        /// </summary>
        private bool headerMouseDown;
        /// <summary>
        /// The drawer animation timer
        /// </summary>
        private Animations.AnimationManager DrawerAnimationTimer;
        /// <summary>
        /// The last location
        /// </summary>
        private Point LastLocation;
        /// <summary>
        /// The last state
        /// </summary>
        private FormWindowState LastState;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialForm"/> class.
        /// </summary>
        public MaterialForm()
        {
            _StateChanging = false;
            FormBorderStyle = FormBorderStyle.None;
            Sizable = true;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Padding = new Padding(0, STATUS_BAR_HEIGHT, 0, 0);

            // This enables the form to trigger the MouseMove event even when mouse is over another control
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;
            DrawerAnimationTimer = new Animations.AnimationManager()
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };

            DrawerAnimationTimer.OnAnimationProgress += sender => Invalidate();

        }


        /// <summary>
        /// WNDs the proc.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                int command = m.WParam.ToInt32() & 0xfff0;
                if (command == SC_MINIMIZE)
                {
                    if (!_StateChanging)
                    {
                        _StateChanging = true;
                        HideForm();
                        base.WndProc(ref m);
                        _StateChanging = false;
                    }
                }
                else if (command == SC_RESTORE)
                {
                    if (!_StateChanging)
                    {
                        Location = new Point(Location.X, Screen.FromHandle(Handle).Bounds.Height + 35);
                        base.WndProc(ref m);
                        Location = new Point(Location.X, Screen.FromHandle(Handle).Bounds.Height + 35);
                        WindowState = FormWindowState.Minimized;
                        _StateChanging = true;
                        RestoreForm();
                        _StateChanging = false;
                    }
                }
                else
                {

                    base.WndProc(ref m);
                }
            }
            else
            {

                base.WndProc(ref m);
            }

            if (DesignMode || IsDisposed) return;

            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                MaximizeWindow(!Maximized);
            }
            else if (m.Msg == WM_MOUSEMOVE && Maximized &&
                statusBarBounds.Contains(PointToClient(Cursor.Position)) &&
                !(minButtonBounds.Contains(PointToClient(Cursor.Position)) || maxButtonBounds.Contains(PointToClient(Cursor.Position)) || xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (headerMouseDown)
                {
                    Maximized = false;
                    headerMouseDown = false;

                    Point mousePoint = PointToClient(Cursor.Position);
                    if (mousePoint.X < Width / 2)
                        Location = mousePoint.X < previousSize.Width / 2 ?
                            new Point(Cursor.Position.X - mousePoint.X, Cursor.Position.Y - mousePoint.Y) :
                            new Point(Cursor.Position.X - previousSize.Width / 2, Cursor.Position.Y - mousePoint.Y);
                    else
                        Location = Width - mousePoint.X < previousSize.Width / 2 ?
                            new Point(Cursor.Position.X - previousSize.Width + Width - mousePoint.X, Cursor.Position.Y - mousePoint.Y) :
                            new Point(Cursor.Position.X - previousSize.Width / 2, Cursor.Position.Y - mousePoint.Y);

                    Size = previousSize;
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            else if (m.Msg == WM_LBUTTONDOWN &&
                statusBarBounds.Contains(PointToClient(Cursor.Position)) &&
                !(minButtonBounds.Contains(PointToClient(Cursor.Position)) || maxButtonBounds.Contains(PointToClient(Cursor.Position)) || xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (!Maximized)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
                else
                {
                    headerMouseDown = true;
                }
            }
            else if (m.Msg == WM_RBUTTONDOWN)
            {
                Point cursorPos = PointToClient(Cursor.Position);

                if (statusBarBounds.Contains(cursorPos) && !minButtonBounds.Contains(cursorPos) &&
                    !maxButtonBounds.Contains(cursorPos) && !xButtonBounds.Contains(cursorPos))
                {
                    // Show default system menu when right clicking titlebar
                    int id = TrackPopupMenuEx(
                        GetSystemMenu(Handle, false),
                        TPM_LEFTALIGN | TPM_RETURNCMD,
                        Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero);

                    // Pass the command as a WM_SYSCOMMAND message
                    SendMessage(Handle, WM_SYSCOMMAND, id, 0);
                }
            }
            else if (m.Msg == WM_NCLBUTTONDOWN)
            {
                // This re-enables resizing by letting the application know when the
                // user is trying to resize a side. This is disabled by default when using WS_SYSMENU.
                if (!Sizable) return;

                byte bFlag = 0;

                // Get which side to resize from
                if (resizingLocationsToCmd.ContainsKey((int)m.WParam))
                    bFlag = (byte)resizingLocationsToCmd[(int)m.WParam];

                if (bFlag != 0)
                    SendMessage(Handle, WM_SYSCOMMAND, 0xF000 | bFlag, (int)m.LParam);
            }
            else if (m.Msg == WM_LBUTTONUP)
            {
                headerMouseDown = false;
            }

        }

        /// <summary>
        /// Hides the form.
        /// </summary>
        private void HideForm()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                return;
            }
            if (Location.Y > Screen.FromHandle(Handle).Bounds.Height)
            {
                WindowState = FormWindowState.Minimized;
                return;
            }

            LastLocation = Location;
            LastState = WindowState;



            Double Duration = 0.5;
            Application.DoEvents();
            DateTime objTimeFin = DateTime.Now.AddSeconds(Duration);
            Point CurrentLocation = Location;
            Double Diff = Screen.FromHandle(Handle).Bounds.Height - Location.Y + 35;

            while (DateTime.Now < objTimeFin)
            {
                DateTime Curr = DateTime.Now;
                double Progress = ((Duration * 1000) - Math.Abs((objTimeFin - Curr).TotalMilliseconds)) / (Duration * 1000);
                Location = new Point(LastLocation.X, (int)(CurrentLocation.Y + Diff * Progress));
                Application.DoEvents();
            }


            WindowState = FormWindowState.Minimized;
            Location = LastLocation;


        }

        /// <summary>
        /// Restores the form.
        /// </summary>
        private void RestoreForm()
        {
            if (WindowState != FormWindowState.Minimized)
            {
                return;
            }
            Double Duration = 0.5;
            WindowState = LastState;
            Location =new Point(LastLocation.X,Screen.FromHandle(Handle).Bounds.Height - LastLocation.Y + 35);
            BringToFront();
            TopMost = true;
            Application.DoEvents();
            DateTime objTimeFin = DateTime.Now.AddSeconds(Duration);
            Point CurrentLocation = Location;
            Double Diff = Math.Abs(LastLocation.Y- CurrentLocation.Y);

            while(DateTime.Now < objTimeFin)
            {
                DateTime Curr = DateTime.Now;
                double Progress =((Duration * 1000)- Math.Abs((objTimeFin - Curr).TotalMilliseconds)) / (Duration * 1000);
                Location = new Point(LastLocation.X, (int)(CurrentLocation.Y - Diff * Progress));
                Application.DoEvents();
            }
            Location = LastLocation;
            TopMost = false;
            BringToFront();
            Application.DoEvents();
        }

        /// <summary>
        /// Gets the create parameters.
        /// </summary>
        /// <value>The create parameters.</value>
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams par = base.CreateParams;
                // WS_SYSMENU: Trigger the creation of the system menu
                // WS_MINIMIZEBOX: Allow minimizing from taskbar
                par.Style = par.Style | WS_MINIMIZEBOX | WS_SYSMENU; // Turn on the WS_MINIMIZEBOX style flag

                par.ClassStyle |= CS_DROPSHADOW;
                return par;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (DesignMode) return;
            UpdateButtons(e);

            if (e.Button == MouseButtons.Left && !Maximized)
                ResizeForm(resizeDir);
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (DesignMode) return;
            buttonState = ButtonState.None;
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (DesignMode) return;

            if (Sizable)
            {
                //True if the mouse is hovering over a child control
                bool isChildUnderMouse = GetChildAtPoint(e.Location) != null;

                if (e.Location.X < BORDER_WIDTH && e.Location.Y > Height - BORDER_WIDTH && !isChildUnderMouse && !Maximized)
                {
                    resizeDir = ResizeDirection.BottomLeft;
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.Location.X < BORDER_WIDTH && !isChildUnderMouse && !Maximized)
                {
                    resizeDir = ResizeDirection.Left;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Location.X > Width - BORDER_WIDTH && e.Location.Y > Height - BORDER_WIDTH && !isChildUnderMouse && !Maximized)
                {
                    resizeDir = ResizeDirection.BottomRight;
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.Location.X > Width - BORDER_WIDTH && !isChildUnderMouse && !Maximized)
                {
                    resizeDir = ResizeDirection.Right;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Location.Y > Height - BORDER_WIDTH && !isChildUnderMouse && !Maximized)
                {
                    resizeDir = ResizeDirection.Bottom;
                    Cursor = Cursors.SizeNS;
                }
                else
                {
                    resizeDir = ResizeDirection.None;

                    //Only reset the cursor when needed, this prevents it from flickering when a child control changes the cursor to its own needs
                    if (resizeCursors.Contains(Cursor))
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }

            UpdateButtons(e);
        }

        /// <summary>
        /// Handles the <see cref="E:GlobalMouseMove" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDisposed)
            {
                // Convert to client position and pass to Form.MouseMove
                Point clientCursorPos = PointToClient(e.Location);
                MouseEventArgs new_e = new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0);
                OnMouseMove(new_e);
            }
        }

        /// <summary>
        /// Updates the buttons.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <param name="up">if set to <c>true</c> [up].</param>
        protected virtual void UpdateButtons(MouseEventArgs e, bool up = false)
        {
            if (DesignMode) return;
            ButtonState oldState = buttonState;
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;

            if (e.Button == MouseButtons.Left && !up)
            {
                if (showMin && !showMax && maxButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.MinDown;
                else if (showMin && showMax && minButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.MinDown;
                else if (showMax && maxButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.MaxDown;
                else if (ControlBox && xButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.XDown;
                else
                    buttonState = ButtonState.None;
            }
            else
            {
                if (showMin && !showMax && maxButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown)
                        WindowState = FormWindowState.Minimized;
                }
                else if (showMin && showMax && minButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown)
                    {
                        HideForm();
                        //AnimateWindow(Handle, 300, AW_HIDE | AW_SLIDE | AW_VER_POSITIVE);
                        WindowState = FormWindowState.Minimized;
                    }
                }
                else if (MaximizeBox && ControlBox && maxButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.MaxOver;

                    if (oldState == ButtonState.MaxDown)
                        MaximizeWindow(!Maximized);

                }
                else if (ControlBox && xButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.XOver;

                    if (oldState == ButtonState.XDown)
                        Close();
                }

                else buttonState = ButtonState.None;
            }

            if (oldState != buttonState) Invalidate();
        }

        /// <summary>
        /// Maximizes the window.
        /// </summary>
        /// <param name="maximize">if set to <c>true</c> [maximize].</param>
        private void MaximizeWindow(bool maximize)
        {
            if (!MaximizeBox || !ControlBox) return;

            Maximized = maximize;

            if (maximize)
            {
                IntPtr monitorHandle = MonitorFromWindow(Handle, MONITOR_DEFAULTTONEAREST);
                MONITORINFOEX monitorInfo = new MONITORINFOEX();
                GetMonitorInfo(new HandleRef(null, monitorHandle), monitorInfo);
                previousSize = Size;
                previousLocation = Location;
                Size = new Size(monitorInfo.rcWork.Width(), monitorInfo.rcWork.Height());
                Location = new Point(monitorInfo.rcWork.left, monitorInfo.rcWork.top);
            }
            else
            {
                Size = previousSize;
                Location = previousLocation;
            }

        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DesignMode) return;
            UpdateButtons(e, true);

            base.OnMouseUp(e);
            ReleaseCapture();
        }

        /// <summary>
        /// Resizes the form.
        /// </summary>
        /// <param name="direction">The direction.</param>
        private void ResizeForm(ResizeDirection direction)
        {
            if (DesignMode) return;
            int dir = -1;
            switch (direction)
            {
                case ResizeDirection.BottomLeft:
                    dir = HTBOTTOMLEFT;
                    break;
                case ResizeDirection.Left:
                    dir = HTLEFT;
                    break;
                case ResizeDirection.Right:
                    dir = HTRIGHT;
                    break;
                case ResizeDirection.BottomRight:
                    dir = HTBOTTOMRIGHT;
                    break;
                case ResizeDirection.Bottom:
                    dir = HTBOTTOM;
                    break;
            }

            ReleaseCapture();
            if (dir != -1)
            {
                SendMessage(Handle, WM_NCLBUTTONDOWN, dir, 0);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            statusBarBounds = new Rectangle(0, 0, Width, STATUS_BAR_HEIGHT);
            minButtonBounds = new Rectangle((Width - SkinManager.FORM_PADDING / 2) - 3 * STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT);
            maxButtonBounds = new Rectangle((Width - SkinManager.FORM_PADDING / 2) - 2 * STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT);
            xButtonBounds = new Rectangle((Width - SkinManager.FORM_PADDING / 2) - STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT);
        }

        /// <summary>
        /// Handles the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(SkinManager.GetApplicationBackgroundColor());
            g.FillRectangle(SkinManager.ColorScheme.DarkPrimaryBrush, statusBarBounds);

            if (_ActionBar == null)
            {
                //Draw border
                using (var borderPen = new Pen(SkinManager.GetDividersColor(), 1))
                {
                    g.DrawLine(borderPen, new Point(0, statusBarBounds.Bottom), new Point(0, Height - 2));
                    g.DrawLine(borderPen, new Point(Width - 1, statusBarBounds.Bottom), new Point(Width - 1, Height - 2));
                    g.DrawLine(borderPen, new Point(0, Height - 1), new Point(Width - 1, Height - 1));
                }
            }

            // Determine whether or not we even should be drawing the buttons.
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;
            var hoverBrush = SkinManager.GetFlatButtonHoverBackgroundBrush();
            var downBrush = SkinManager.GetFlatButtonPressedBackgroundBrush();

            // When MaximizeButton == false, the minimize button will be painted in its place
            if (buttonState == ButtonState.MinOver && showMin)
                g.FillRectangle(hoverBrush, showMax ? minButtonBounds : maxButtonBounds);

            if (buttonState == ButtonState.MinDown && showMin)
                g.FillRectangle(downBrush, showMax ? minButtonBounds : maxButtonBounds);

            if (buttonState == ButtonState.MaxOver && showMax)
                g.FillRectangle(hoverBrush, maxButtonBounds);

            if (buttonState == ButtonState.MaxDown && showMax)
                g.FillRectangle(downBrush, maxButtonBounds);

            if (buttonState == ButtonState.XOver && ControlBox)
                g.FillRectangle(hoverBrush, xButtonBounds);

            if (buttonState == ButtonState.XDown && ControlBox)
                g.FillRectangle(downBrush, xButtonBounds);

            using (var formButtonsPen = new Pen(SkinManager.ACTION_BAR_TEXT_SECONDARY(), 2))
            {
                // Minimize button.
                if (showMin)
                {

                    int x = showMax ? minButtonBounds.X : maxButtonBounds.X;
                    int y = showMax ? minButtonBounds.Y : maxButtonBounds.Y;

                    g.DrawLine(
                        formButtonsPen,
                        x + (int)(minButtonBounds.Width * 0.33),
                        y + (int)(minButtonBounds.Height * 0.66),
                        x + (int)(minButtonBounds.Width * 0.66),
                        y + (int)(minButtonBounds.Height * 0.66)
                   );
                }

                // Maximize button
                if (showMax)
                {
                    g.DrawRectangle(
                        formButtonsPen,
                        maxButtonBounds.X + (int)(maxButtonBounds.Width * 0.33),
                        maxButtonBounds.Y + (int)(maxButtonBounds.Height * 0.36),
                        (int)(maxButtonBounds.Width * 0.39),
                        (int)(maxButtonBounds.Height * 0.31)
                   );
                }

                // Close button
                if (ControlBox)
                {
                    g.DrawLine(
                        formButtonsPen,
                        xButtonBounds.X + (int)(xButtonBounds.Width * 0.33),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.33),
                        xButtonBounds.X + (int)(xButtonBounds.Width * 0.66),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.66)
                   );

                    g.DrawLine(
                        formButtonsPen,
                        xButtonBounds.X + (int)(xButtonBounds.Width * 0.66),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.33),
                        xButtonBounds.X + (int)(xButtonBounds.Width * 0.33),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.66));
                }
            }

            //Form title
            if (_ActionBar == null)
                g.DrawString(Text, SkinManager.ROBOTO_REGULAR_11, SkinManager.ColorScheme.TextBrush, new Rectangle(SkinManager.FORM_PADDING, 0, Width, STATUS_BAR_HEIGHT), new StringFormat { LineAlignment = StringAlignment.Center });

            if (_SideDrawer != null)
            {
                if (!_SideDrawer.SideDrawerFixiert)
                {

                    _SideDrawer.Width = (int)(_SideDrawer.MaximumSize.Width * DrawerAnimationTimer.GetProgress());
                    if (_ActionBar != null)
                    {
                        _ActionBar.setDrawerAnimationProgress((int)(DrawerAnimationTimer.GetProgress() * 100));
                    }

                }

            }

            //Schatten Zeichnen
            GraphicsPath ActionBarShadow = new GraphicsPath();
            ActionBarShadow.AddLine(new Point(0, STATUS_BAR_HEIGHT), new Point(Width, STATUS_BAR_HEIGHT));
            DrawHelper.drawShadow(g, ActionBarShadow, 10, SkinManager.GetApplicationBackgroundColor());

            foreach (Control objChild in Controls)
            {
                if (typeof(IShadowedMaterialControl).IsAssignableFrom(objChild.GetType()))
                {
                    IShadowedMaterialControl objCurrent = (IShadowedMaterialControl)objChild;
                    DrawHelper.drawShadow(g, objCurrent.ShadowBorder, objCurrent.Elevation, SkinManager.GetApplicationBackgroundColor());
                }

            }
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MaterialForm
            // 
            mLastState = this.WindowState;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "MaterialForm";
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Animates the window.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="dwTime">The dw time.</param>
        /// <param name="dwFlags">The dw flags.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

    }

    /// <summary>
    /// Class MouseMessageFilter.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.IMessageFilter" />
    public class MouseMessageFilter : IMessageFilter
    {
        /// <summary>
        /// The wm mousemove
        /// </summary>
        private const int WM_MOUSEMOVE = 0x0200;

        /// <summary>
        /// Occurs when [mouse move].
        /// </summary>
        public static event MouseEventHandler MouseMove;

        /// <summary>
        /// Filters out a message before it is dispatched.
        /// </summary>
        /// <param name="m">The message to be dispatched. You cannot modify this message.</param>
        /// <returns>true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.</returns>
        public bool PreFilterMessage(ref Message m)
        {

            if (m.Msg == WM_MOUSEMOVE)
            {
                if (MouseMove != null)
                {
                    int x = Control.MousePosition.X, y = Control.MousePosition.Y;

                    MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                }
            }
            return false;
        }
    }

}
