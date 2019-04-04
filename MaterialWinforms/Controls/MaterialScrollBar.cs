// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialScrollBar.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Security;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Zeroit.Framework.MaterialWinforms.Controls
{
    
    /// <summary>
    /// Enum MaterialScrollOrientation
    /// </summary>
    public enum MaterialScrollOrientation
    {
        /// <summary>
        /// The horizontal
        /// </summary>
        Horizontal,
        /// <summary>
        /// The vertical
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Class MaterialScrollBar.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    [DefaultEvent("Scroll")]
    [DefaultProperty("Value")]
    public class MaterialScrollBar : Control,IMaterialControl
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
        /// The scrollbar default size
        /// </summary>
        internal const int SCROLLBAR_DEFAULT_SIZE = 10;

        #region Events

        /// <summary>
        /// Occurs when [scroll].
        /// </summary>
        public event ScrollEventHandler Scroll;

        /// <summary>
        /// Called when [scroll].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="orientation">The orientation.</param>
        private void OnScroll(ScrollEventType type, int oldValue, int newValue, ScrollOrientation orientation)
        {
            if (Scroll == null) return;

            if (orientation == ScrollOrientation.HorizontalScroll)
            {
                if (type != ScrollEventType.EndScroll && isFirstScrollEventHorizontal)
                {
                    type = ScrollEventType.First;
                }
                else if (!isFirstScrollEventHorizontal && type == ScrollEventType.EndScroll)
                {
                    isFirstScrollEventHorizontal = true;
                }
            }
            else
            {
                if (type != ScrollEventType.EndScroll && isFirstScrollEventVertical)
                {
                    type = ScrollEventType.First;
                }
                else if (!isFirstScrollEventHorizontal && type == ScrollEventType.EndScroll)
                {
                    isFirstScrollEventVertical = true;
                }
            }

            Scroll(this, new ScrollEventArgs(type, oldValue, newValue, orientation));
        }

        #endregion

        #region Properties

        /// <summary>
        /// The is first scroll event vertical
        /// </summary>
        private bool isFirstScrollEventVertical = true;
        /// <summary>
        /// The is first scroll event horizontal
        /// </summary>
        private bool isFirstScrollEventHorizontal = true;

        /// <summary>
        /// The in update
        /// </summary>
        private bool inUpdate;

        /// <summary>
        /// The clicked bar rectangle
        /// </summary>
        private Rectangle clickedBarRectangle;
        /// <summary>
        /// The thumb rectangle
        /// </summary>
        private Rectangle thumbRectangle;

        /// <summary>
        /// The top bar clicked
        /// </summary>
        private bool topBarClicked;
        /// <summary>
        /// The bottom bar clicked
        /// </summary>
        private bool bottomBarClicked;
        /// <summary>
        /// The thumb clicked
        /// </summary>
        private bool thumbClicked;

        /// <summary>
        /// The thumb width
        /// </summary>
        private int thumbWidth = 6;
        /// <summary>
        /// The thumb height
        /// </summary>
        private int thumbHeight;

        /// <summary>
        /// The thumb bottom limit bottom
        /// </summary>
        private int thumbBottomLimitBottom;
        /// <summary>
        /// The thumb bottom limit top
        /// </summary>
        private int thumbBottomLimitTop;
        /// <summary>
        /// The thumb top limit
        /// </summary>
        private int thumbTopLimit;
        /// <summary>
        /// The thumb position
        /// </summary>
        private int thumbPosition;

        /// <summary>
        /// The wm setredraw
        /// </summary>
        public const int WM_SETREDRAW = 0xb;

        /// <summary>
        /// The track position
        /// </summary>
        private int trackPosition;

        /// <summary>
        /// The progress timer
        /// </summary>
        private readonly Timer progressTimer = new Timer();

        /// <summary>
        /// The mouse wheel bar partitions
        /// </summary>
        private int mouseWheelBarPartitions = 10;
        /// <summary>
        /// Gets or sets the mouse wheel bar partitions.
        /// </summary>
        /// <value>The mouse wheel bar partitions.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">value - MouseWheelBarPartitions has to be greather than zero</exception>
        [DefaultValue(10)]
        public int MouseWheelBarPartitions
        {
            get { return mouseWheelBarPartitions; }
            set
            {
                if (value > 0)
                {
                    mouseWheelBarPartitions = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("value", "MouseWheelBarPartitions has to be greather than zero");
                }
            }
        }

        /// <summary>
        /// The is hovered
        /// </summary>
        private bool isHovered;
        /// <summary>
        /// The is pressed
        /// </summary>
        private bool isPressed;

        /// <summary>
        /// The use bar color
        /// </summary>
        private bool useBarColor = false;
        /// <summary>
        /// Gets or sets a value indicating whether [use bar color].
        /// </summary>
        /// <value><c>true</c> if [use bar color]; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        public bool UseBarColor
        {
            get { return useBarColor; }
            set { useBarColor = value; }
        }

        /// <summary>
        /// Gets or sets the size of the scrollbar.
        /// </summary>
        /// <value>The size of the scrollbar.</value>
        [DefaultValue(SCROLLBAR_DEFAULT_SIZE)]
        public int ScrollbarSize
        {
            get { return Orientation == MaterialScrollOrientation.Vertical ? Width : Height; }
            set
            {
                if (Orientation == MaterialScrollOrientation.Vertical)
                    Width = value;
                else
                    Height = value;
            }
        }

        /// <summary>
        /// The highlight on wheel
        /// </summary>
        private bool highlightOnWheel = false;
        /// <summary>
        /// Gets or sets a value indicating whether [highlight on wheel].
        /// </summary>
        /// <value><c>true</c> if [highlight on wheel]; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        public bool HighlightOnWheel
        {
            get { return highlightOnWheel; }
            set { highlightOnWheel = value; }
        }

        /// <summary>
        /// The material orientation
        /// </summary>
        private MaterialScrollOrientation MaterialOrientation = MaterialScrollOrientation.Vertical;
        /// <summary>
        /// The scroll orientation
        /// </summary>
        private ScrollOrientation scrollOrientation = ScrollOrientation.VerticalScroll;

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public MaterialScrollOrientation Orientation
        {
            get { return MaterialOrientation; }
            set
            {
                if (value == MaterialOrientation) return;
                MaterialOrientation = value;
                scrollOrientation = value == MaterialScrollOrientation.Vertical ? ScrollOrientation.VerticalScroll : ScrollOrientation.HorizontalScroll;
                Size = new Size(Height, Width);
                SetupScrollBar();
            }
        }

        /// <summary>
        /// The minimum
        /// </summary>
        private int minimum = 0;
        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        [DefaultValue(0)]
        public int Minimum
        {
            get { return minimum; }
            set
            {
                if (minimum == value || value < 0 || value >= maximum)
                {
                    return;
                }

                minimum = value;
                if (curValue < value)
                {
                    curValue = value;
                }

                if (largeChange > (maximum - minimum))
                {
                    largeChange = maximum - minimum;
                }

                SetupScrollBar();

                if (curValue < value)
                {
                    dontUpdateColor = true;
                    Value = value;
                }
                else
                {
                    ChangeThumbPosition(GetThumbPosition());
                    Refresh();
                }
            }
        }

        /// <summary>
        /// The maximum
        /// </summary>
        private int maximum = 100;
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        [DefaultValue(100)]
        public int Maximum
        {
            get { return maximum; }
            set
            {
                if (value == maximum || value < 1 || value <= minimum)
                {
                    return;
                }

                maximum = value;
                if (largeChange > (maximum - minimum))
                {
                    largeChange = maximum - minimum;
                }

                SetupScrollBar();

                if (curValue > value)
                {
                    dontUpdateColor = true;
                    Value = maximum;
                }
                else
                {
                    ChangeThumbPosition(GetThumbPosition());
                    Refresh();
                }
            }
        }

        /// <summary>
        /// The small change
        /// </summary>
        private int smallChange = 1;
        /// <summary>
        /// Gets or sets the small change.
        /// </summary>
        /// <value>The small change.</value>
        [DefaultValue(1)]
        public int SmallChange
        {
            get { return smallChange; }
            set
            {
                if (value == smallChange || value < 1 || value >= largeChange)
                {
                    return;
                }

                smallChange = value;
                SetupScrollBar();
            }
        }

        /// <summary>
        /// The large change
        /// </summary>
        private int largeChange = 10;
        /// <summary>
        /// Gets or sets the large change.
        /// </summary>
        /// <value>The large change.</value>
        [DefaultValue(10)]
        public int LargeChange
        {
            get { return largeChange; }
            set
            {
                if (value == largeChange || value < smallChange || value < 2)
                {
                    return;
                }

                if (value > (maximum - minimum))
                {
                    largeChange = maximum - minimum;
                }
                else
                {
                    largeChange = value;
                }

                SetupScrollBar();
            }
        }

        /// <summary>
        /// The dont update color
        /// </summary>
        private bool dontUpdateColor = false;

        /// <summary>
        /// The current value
        /// </summary>
        private int curValue = 0;
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [DefaultValue(0)]
        [Browsable(false)]
        public int Value
        {
            get { return curValue; }

            set
            {
                if (curValue == value || value < minimum || value > maximum)
                {
                    return;
                }

                curValue = value;

                ChangeThumbPosition(GetThumbPosition());

                OnScroll(ScrollEventType.ThumbPosition, -1, value, scrollOrientation);

                if (!dontUpdateColor && highlightOnWheel)
                {
                    if (!isHovered)
                        isHovered = true;

                    if (autoHoverTimer == null)
                    {
                        autoHoverTimer = new Timer();
                        autoHoverTimer.Interval = 1000;
                        autoHoverTimer.Tick += new EventHandler(autoHoverTimer_Tick);
                        autoHoverTimer.Start();
                    }
                    else
                    {
                        autoHoverTimer.Stop();
                        autoHoverTimer.Start();
                    }
                }
                else
                {
                    dontUpdateColor = false;
                }

                Refresh();
            }
        }

        /// <summary>
        /// Handles the Tick event of the autoHoverTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void autoHoverTimer_Tick(object sender, EventArgs e)
        {
            isHovered = false;
            Invalidate();
            autoHoverTimer.Stop();
        }

        /// <summary>
        /// The automatic hover timer
        /// </summary>
        private Timer autoHoverTimer = null;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialScrollBar"/> class.
        /// </summary>
        public MaterialScrollBar()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            Width = SCROLLBAR_DEFAULT_SIZE;
            Height = 200;

            SetupScrollBar();

            progressTimer.Interval = 20;
            progressTimer.Tick += ProgressTimerTick;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialScrollBar"/> class.
        /// </summary>
        /// <param name="orientation">The orientation.</param>
        public MaterialScrollBar(MaterialScrollOrientation orientation)
            : this()
        {
            Orientation = orientation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialScrollBar"/> class.
        /// </summary>
        /// <param name="orientation">The orientation.</param>
        /// <param name="width">The width.</param>
        public MaterialScrollBar(MaterialScrollOrientation orientation, int width)
            : this(orientation)
        {
            Width = width;
        }

        #region Update Methods

        /// <summary>
        /// Begins the update.
        /// </summary>
        [SecuritySafeCritical]
        public void BeginUpdate()
        {
            SendMessage(Handle, WM_SETREDRAW, 0, 0);
            inUpdate = true;
        }

        /// <summary>
        /// Ends the update.
        /// </summary>
        [SecuritySafeCritical]
        public void EndUpdate()
        {
            SendMessage(Handle, WM_SETREDRAW, 1, 0);
            inUpdate = false;
            SetupScrollBar();
            Refresh();
        }

        #endregion

        #region Paint Methods


        /// <summary>
        /// Handles the <see cref="E:PaintBackground" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                e.Graphics.Clear(MaterialSkinManager.Instance.GetCardsColor());
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Invalidate();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawScrollBar(e.Graphics, MaterialSkinManager.Instance.GetCardsColor(), MaterialSkinManager.Instance.ColorScheme.PrimaryColor, MaterialSkinManager.Instance.ColorScheme.AccentColor);
        }

        /// <summary>
        /// Draws the scroll bar.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="backColor">Color of the back.</param>
        /// <param name="thumbColor">Color of the thumb.</param>
        /// <param name="barColor">Color of the bar.</param>
        private void DrawScrollBar(Graphics g, Color backColor, Color thumbColor, Color barColor)
        {
            if (useBarColor)
            {
                using (var b = new SolidBrush(barColor))
                {
                    g.FillRectangle(b, ClientRectangle);
                }
            }

            using (var b = new SolidBrush(backColor))
            {
                var thumbRect = new Rectangle(thumbRectangle.X - 1, thumbRectangle.Y - 1, thumbRectangle.Width + 2, thumbRectangle.Height + 2);
                g.FillRectangle(b, thumbRect);
            }

            using (var b = new SolidBrush(isHovered ?barColor:thumbColor))
            {
                g.FillRectangle(b, thumbRectangle);
            }
        }

        #endregion

        #region Focus Methods

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();

            base.OnGotFocus(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnEnter(EventArgs e)
        {
            Invalidate();

            base.OnEnter(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLeave(EventArgs e)
        {
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLeave(e);
        }

        #endregion

        #region Mouse Methods

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            int v = e.Delta / 120 * (maximum - minimum) / mouseWheelBarPartitions;

            if (Orientation == MaterialScrollOrientation.Vertical)
            {
                Value -= v;
            }
            else
            {
                Value += v;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);

            Focus();

            if (e.Button == MouseButtons.Left)
            {

                var mouseLocation = e.Location;

                if (thumbRectangle.Contains(mouseLocation))
                {
                    thumbClicked = true;
                    thumbPosition = MaterialOrientation == MaterialScrollOrientation.Vertical ? mouseLocation.Y - thumbRectangle.Y : mouseLocation.X - thumbRectangle.X;

                    Invalidate(thumbRectangle);
                }
                else
                {
                    trackPosition = MaterialOrientation == MaterialScrollOrientation.Vertical ? mouseLocation.Y : mouseLocation.X;

                    if (trackPosition < (MaterialOrientation == MaterialScrollOrientation.Vertical ? thumbRectangle.Y : thumbRectangle.X))
                    {
                        topBarClicked = true;
                    }
                    else
                    {
                        bottomBarClicked = true;
                    }

                    ProgressThumb(true);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                trackPosition = MaterialOrientation == MaterialScrollOrientation.Vertical ? e.Y : e.X;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;

            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                if (thumbClicked)
                {
                    thumbClicked = false;
                    OnScroll(ScrollEventType.EndScroll, -1, curValue, scrollOrientation);
                }
                else if (topBarClicked)
                {
                    topBarClicked = false;
                    StopTimer();
                }
                else if (bottomBarClicked)
                {
                    bottomBarClicked = false;
                    StopTimer();
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            Invalidate();

            base.OnMouseLeave(e);

            ResetScrollStatus();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left)
            {
                if (thumbClicked)
                {
                    int oldScrollValue = curValue;

                    int pos = MaterialOrientation == MaterialScrollOrientation.Vertical ? e.Location.Y : e.Location.X;
                    int thumbSize = MaterialOrientation == MaterialScrollOrientation.Vertical ? (pos / Height) / thumbHeight : (pos / Width) / thumbWidth;

                    if (pos <= (thumbTopLimit + thumbPosition))
                    {
                        ChangeThumbPosition(thumbTopLimit);
                        curValue = minimum;
                        Invalidate();
                    }
                    else if (pos >= (thumbBottomLimitTop + thumbPosition))
                    {
                        ChangeThumbPosition(thumbBottomLimitTop);
                        curValue = maximum;
                        Invalidate();
                    }
                    else
                    {
                        ChangeThumbPosition(pos - thumbPosition);

                        int pixelRange, thumbPos;

                        if (Orientation == MaterialScrollOrientation.Vertical)
                        {
                            pixelRange = Height - thumbSize;
                            thumbPos = thumbRectangle.Y;
                        }
                        else
                        {
                            pixelRange = Width - thumbSize;
                            thumbPos = thumbRectangle.X;
                        }

                        float perc = 0f;

                        if (pixelRange != 0)
                        {
                            perc = (thumbPos) / (float)pixelRange;
                        }

                        curValue = Convert.ToInt32((perc * (maximum - minimum)) + minimum);
                    }

                    if (oldScrollValue != curValue)
                    {
                        OnScroll(ScrollEventType.ThumbTrack, oldScrollValue, curValue, scrollOrientation);
                        Refresh();
                    }
                }
            }
            else if (!ClientRectangle.Contains(e.Location))
            {
                ResetScrollStatus();
            }
            else if (e.Button == MouseButtons.None)
            {
                if (thumbRectangle.Contains(e.Location))
                {
                    Invalidate(thumbRectangle);
                }
                else if (ClientRectangle.Contains(e.Location))
                {
                    Invalidate();
                }
            }
        }

        #endregion

        #region Keyboard Methods

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            isHovered = true;
            isPressed = true;
            Invalidate();

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnKeyUp(e);
        }

        #endregion

        #region Management Methods

        /// <summary>
        /// Hits the test.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool HitTest(Point point)
        {
            return thumbRectangle.Contains(point);
        }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        /// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
        /// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
        /// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
        /// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
        /// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);

            if (DesignMode)
            {
                SetupScrollBar();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetupScrollBar();
        }

        /// <summary>
        /// Processes a dialog key.
        /// </summary>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
        /// <returns>true if the key was processed by the control; otherwise, false.</returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            var keyUp = Keys.Up;
            var keyDown = Keys.Down;

            if (Orientation == MaterialScrollOrientation.Horizontal)
            {
                keyUp = Keys.Left;
                keyDown = Keys.Right;
            }

            if (keyData == keyUp)
            {
                Value -= smallChange;

                return true;
            }

            if (keyData == keyDown)
            {
                Value += smallChange;

                return true;
            }

            if (keyData == Keys.PageUp)
            {
                Value = GetValue(false, true);

                return true;
            }

            if (keyData == Keys.PageDown)
            {
                if (curValue + largeChange > maximum)
                {
                    Value = maximum;
                }
                else
                {
                    Value += largeChange;
                }

                return true;
            }

            if (keyData == Keys.Home)
            {
                Value = minimum;

                return true;
            }

            if (keyData == Keys.End)
            {
                Value = maximum;

                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        /// <summary>
        /// Setups the scroll bar.
        /// </summary>
        private void SetupScrollBar()
        {
            if (inUpdate) return;

            if (Orientation == MaterialScrollOrientation.Vertical)
            {
                thumbWidth = Width > 0 ? Width : 10;
                thumbHeight = GetThumbSize();

                clickedBarRectangle = ClientRectangle;
                clickedBarRectangle.Inflate(-1, -1);

                thumbRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, thumbWidth, thumbHeight);

                thumbPosition = thumbRectangle.Height / 2;
                thumbBottomLimitBottom = ClientRectangle.Bottom;
                thumbBottomLimitTop = thumbBottomLimitBottom - thumbRectangle.Height;
                thumbTopLimit = ClientRectangle.Y;
            }
            else
            {
                thumbHeight = Height > 0 ? Height : 10;
                thumbWidth = GetThumbSize();

                clickedBarRectangle = ClientRectangle;
                clickedBarRectangle.Inflate(-1, -1);

                thumbRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, thumbWidth, thumbHeight);

                thumbPosition = thumbRectangle.Width / 2;
                thumbBottomLimitBottom = ClientRectangle.Right;
                thumbBottomLimitTop = thumbBottomLimitBottom - thumbRectangle.Width;
                thumbTopLimit = ClientRectangle.X;
            }

            ChangeThumbPosition(GetThumbPosition());

            Refresh();
        }

        /// <summary>
        /// Resets the scroll status.
        /// </summary>
        private void ResetScrollStatus()
        {
            bottomBarClicked = topBarClicked = false;

            StopTimer();
            Refresh();
        }

        /// <summary>
        /// Progresses the timer tick.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ProgressTimerTick(object sender, EventArgs e)
        {
            ProgressThumb(true);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="smallIncrement">if set to <c>true</c> [small increment].</param>
        /// <param name="up">if set to <c>true</c> [up].</param>
        /// <returns>System.Int32.</returns>
        private int GetValue(bool smallIncrement, bool up)
        {
            int newValue;

            if (up)
            {
                newValue = curValue - (smallIncrement ? smallChange : largeChange);

                if (newValue < minimum)
                {
                    newValue = minimum;
                }
            }
            else
            {
                newValue = curValue + (smallIncrement ? smallChange : largeChange);

                if (newValue > maximum)
                {
                    newValue = maximum;
                }
            }

            return newValue;
        }

        /// <summary>
        /// Gets the thumb position.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetThumbPosition()
        {
            int pixelRange;

            if (thumbHeight == 0 || thumbWidth == 0)
            {
                return 0;
            }

            int thumbSize = MaterialOrientation == MaterialScrollOrientation.Vertical ? (thumbPosition / Height) / thumbHeight : (thumbPosition / Width) / thumbWidth;

            if (Orientation == MaterialScrollOrientation.Vertical)
            {
                pixelRange = Height - thumbSize;
            }
            else
            {
                pixelRange = Width - thumbSize;
            }

            int realRange = maximum - minimum;
            float perc = 0f;

            if (realRange != 0)
            {
                perc = (curValue - (float)minimum) / realRange;
            }

            return Math.Max(thumbTopLimit, Math.Min(thumbBottomLimitTop, Convert.ToInt32((perc * pixelRange))));
        }

        /// <summary>
        /// Gets the size of the thumb.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetThumbSize()
        {
            int trackSize =
                MaterialOrientation == MaterialScrollOrientation.Vertical ?
                    Height : Width;

            if (maximum == 0 || largeChange == 0)
            {
                return trackSize;
            }

            float newThumbSize = (largeChange * (float)trackSize) / maximum;

            return Convert.ToInt32(Math.Min(trackSize, Math.Max(newThumbSize, 10f)));
        }

        /// <summary>
        /// Enables the timer.
        /// </summary>
        private void EnableTimer()
        {
            if (!progressTimer.Enabled)
            {
                progressTimer.Interval = 600;
                progressTimer.Start();
            }
            else
            {
                progressTimer.Interval = 10;
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        private void StopTimer()
        {
            progressTimer.Stop();
        }

        /// <summary>
        /// Changes the thumb position.
        /// </summary>
        /// <param name="position">The position.</param>
        private void ChangeThumbPosition(int position)
        {
            if (Orientation == MaterialScrollOrientation.Vertical)
            {
                thumbRectangle.Y = position;
            }
            else
            {
                thumbRectangle.X = position;
            }
        }

        /// <summary>
        /// Progresses the thumb.
        /// </summary>
        /// <param name="enableTimer">if set to <c>true</c> [enable timer].</param>
        private void ProgressThumb(bool enableTimer)
        {
            int scrollOldValue = curValue;
            var type = ScrollEventType.First;
            int thumbSize, thumbPos;

            if (Orientation == MaterialScrollOrientation.Vertical)
            {
                thumbPos = thumbRectangle.Y;
                thumbSize = thumbRectangle.Height;
            }
            else
            {
                thumbPos = thumbRectangle.X;
                thumbSize = thumbRectangle.Width;
            }

            if ((bottomBarClicked && (thumbPos + thumbSize) < trackPosition))
            {
                type = ScrollEventType.LargeIncrement;

                curValue = GetValue(false, false);

                if (curValue == maximum)
                {
                    ChangeThumbPosition(thumbBottomLimitTop);

                    type = ScrollEventType.Last;
                }
                else
                {
                    ChangeThumbPosition(Math.Min(thumbBottomLimitTop, GetThumbPosition()));
                }
            }
            else if ((topBarClicked && thumbPos > trackPosition))
            {
                type = ScrollEventType.LargeDecrement;

                curValue = GetValue(false, true);

                if (curValue == minimum)
                {
                    ChangeThumbPosition(thumbTopLimit);

                    type = ScrollEventType.First;
                }
                else
                {
                    ChangeThumbPosition(Math.Max(thumbTopLimit, GetThumbPosition()));
                }
            }

            if (scrollOldValue != curValue)
            {
                OnScroll(type, scrollOldValue, curValue, scrollOrientation);

                Invalidate();

                if (enableTimer)
                {
                    EnableTimer();
                }
            }
        }

        #endregion
    }
}