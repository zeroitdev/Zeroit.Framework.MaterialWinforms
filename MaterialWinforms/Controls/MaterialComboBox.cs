// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialComboBox.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Zeroit.Framework.MaterialWinforms.Controls
{

    /// <summary>
    /// Class MaterialComboBox.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ComboBox" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    [DefaultEvent("TextChanged")]
    public class MaterialComboBox : ComboBox, IMaterialControl
    {

        #region  Variables

        /// <summary>
        /// The drop down check
        /// </summary>
        private Timer _dropDownCheck = new Timer();
        /// <summary>
        /// The font
        /// </summary>
        FontManager font = new FontManager();

        /// <summary>
        /// The button area
        /// </summary>
        private Rectangle _ButtonArea;



        /// <summary>
        /// Gets the skin manager.
        /// </summary>
        /// <value>The skin manager.</value>
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }

        #endregion

        #region  Properties

        //Properties for managing the material design properties
        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        [Browsable(false)]
        public int Depth { get; set; }
        /// <summary>
        /// Gets or sets the state of the mouse.
        /// </summary>
        /// <value>The state of the mouse.</value>
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        /// <summary>
        /// Gets the color of the back.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public override Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : typeof(IShadowedMaterialControl).IsAssignableFrom(Parent.GetType()) ? ((IMaterialControl)Parent).BackColor : Parent.BackColor; } }


        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialComboBox"/> class.
        /// </summary>
        public MaterialComboBox()
        {
            SetStyle(ControlStyles.UserPaint, true);
            DropDownStyle = ComboBoxStyle.DropDownList;
            _ButtonArea = new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, ClientRectangle.Width + 2, ClientRectangle.Height + 2);
            _dropDownCheck.Interval = 10;
            _dropDownCheck.Tick += new EventHandler(dropDownCheck_Tick);
           MeasureItem += new MeasureItemEventHandler(CustMeasureItem);
        }
        /// <summary>
        /// Enum PenStyles
        /// </summary>
        public enum PenStyles
        {
            /// <summary>
            /// The ps solid
            /// </summary>
            PS_SOLID = 0,
            /// <summary>
            /// The ps dash
            /// </summary>
            PS_DASH = 1,
            /// <summary>
            /// The ps dot
            /// </summary>
            PS_DOT = 2,
            /// <summary>
            /// The ps dashdot
            /// </summary>
            PS_DASHDOT = 3,
            /// <summary>
            /// The ps dashdotdot
            /// </summary>
            PS_DASHDOTDOT = 4
        }

        /// <summary>
        /// Enum ComboBoxButtonState
        /// </summary>
        public enum ComboBoxButtonState
        {
            /// <summary>
            /// The state system none
            /// </summary>
            STATE_SYSTEM_NONE = 0,
            /// <summary>
            /// The state system invisible
            /// </summary>
            STATE_SYSTEM_INVISIBLE = 0x00008000,
            /// <summary>
            /// The state system pressed
            /// </summary>
            STATE_SYSTEM_PRESSED = 0x00000008
        }

        /// <summary>
        /// Struct COMBOBOXINFO
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct COMBOBOXINFO
        {
            /// <summary>
            /// The cb size
            /// </summary>
            public Int32 cbSize;
            /// <summary>
            /// The rc item
            /// </summary>
            public RECT rcItem;
            /// <summary>
            /// The rc button
            /// </summary>
            public RECT rcButton;
            /// <summary>
            /// The button state
            /// </summary>
            public ComboBoxButtonState buttonState;
            /// <summary>
            /// The HWND combo
            /// </summary>
            public IntPtr hwndCombo;
            /// <summary>
            /// The HWND edit
            /// </summary>
            public IntPtr hwndEdit;
            /// <summary>
            /// The HWND list
            /// </summary>
            public IntPtr hwndList;
        }

        /// <summary>
        /// Struct RECT
        /// </summary>
        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// The left
            /// </summary>
            public int Left;
            /// <summary>
            /// The top
            /// </summary>
            public int Top;
            /// <summary>
            /// The right
            /// </summary>
            public int Right;
            /// <summary>
            /// The bottom
            /// </summary>
            public int Bottom;

            /// <summary>
            /// Initializes a new instance of the <see cref="RECT"/> struct.
            /// </summary>
            /// <param name="left_">The left.</param>
            /// <param name="top_">The top.</param>
            /// <param name="right_">The right.</param>
            /// <param name="bottom_">The bottom.</param>
            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
            public override bool Equals(object obj)
            {
                if (obj == null || !(obj is RECT))
                {
                    return false;
                }
                return this.Equals((RECT)obj);
            }

            /// <summary>
            /// Equalses the specified value.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
            public bool Equals(RECT value)
            {
                return this.Left == value.Left &&
                       this.Top == value.Top &&
                       this.Right == value.Right &&
                       this.Bottom == value.Bottom;
            }

            /// <summary>
            /// Gets the height.
            /// </summary>
            /// <value>The height.</value>
            public int Height
            {
                get
                {
                    return Bottom - Top + 1;
                }
            }

            /// <summary>
            /// Gets the width.
            /// </summary>
            /// <value>The width.</value>
            public int Width
            {
                get
                {
                    return Right - Left + 1;
                }
            }

            /// <summary>
            /// Gets the size.
            /// </summary>
            /// <value>The size.</value>
            public Size Size { get { return new Size(Width, Height); } }

            /// <summary>
            /// Gets the location.
            /// </summary>
            /// <value>The location.</value>
            public Point Location { get { return new Point(Left, Top); } }

            // Handy method for converting to a System.Drawing.Rectangle
            /// <summary>
            /// To the rectangle.
            /// </summary>
            /// <returns>System.Drawing.Rectangle.</returns>
            public System.Drawing.Rectangle ToRectangle()
            {
                return System.Drawing.Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }

            /// <summary>
            /// Froms the rectangle.
            /// </summary>
            /// <param name="rectangle">The rectangle.</param>
            /// <returns>RECT.</returns>
            public static RECT FromRectangle(Rectangle rectangle)
            {
                return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }

            /// <summary>
            /// Inflates the specified width.
            /// </summary>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            public void Inflate(int width, int height)
            {
                this.Left -= width;
                this.Top -= height;
                this.Right += width;
                this.Bottom += height;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode()
            {
                return Left ^ ((Top << 13) | (Top >> 0x13))
                    ^ ((Width << 0x1a) | (Width >> 6))
                    ^ ((Height << 7) | (Height >> 0x19));
            }

            /// <summary>
            /// Performs an implicit conversion from <see cref="RECT"/> to <see cref="Rectangle"/>.
            /// </summary>
            /// <param name="rect">The rect.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator Rectangle(RECT rect)
            {
                return System.Drawing.Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }

            /// <summary>
            /// Performs an implicit conversion from <see cref="Rectangle"/> to <see cref="RECT"/>.
            /// </summary>
            /// <param name="rect">The rect.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator RECT(Rectangle rect)
            {
                return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }


        /// <summary>
        /// Override window messages
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        protected override void WndProc(ref Message m)
        {
            // Filter window messages
            switch (m.Msg)
            {
                // Draw a custom color border around the drop down list cintaining popup
                case WM_CTLCOLORLISTBOX:
                    base.WndProc(ref m);
                    DrawNativeBorder(m.LParam);
                    break;

                default: base.WndProc(ref m); break;
            }
        }

        /// <summary>
        /// The wm ctlcolorlistbox
        /// </summary>
        public const int WM_CTLCOLORLISTBOX = 0x0134;

        /// <summary>
        /// Gets the window rect.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="lpRect">The lp rect.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        /// <summary>
        /// Gets the window dc.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        /// <summary>
        /// Releases the dc.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="hDC">The h dc.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// Sets the focus.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        /// <summary>
        /// Gets the ComboBox information.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="pcbi">The pcbi.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool GetComboBoxInfo(IntPtr hWnd, ref COMBOBOXINFO pcbi);

        /// <summary>
        /// Excludes the clip rect.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        /// <param name="nLeftRect">The n left rect.</param>
        /// <param name="nTopRect">The n top rect.</param>
        /// <param name="nRightRect">The n right rect.</param>
        /// <param name="nBottomRect">The n bottom rect.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("gdi32.dll")]
        public static extern int ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        /// <summary>
        /// Creates the pen.
        /// </summary>
        /// <param name="enPenStyle">The en pen style.</param>
        /// <param name="nWidth">Width of the n.</param>
        /// <param name="crColor">Color of the cr.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(PenStyles enPenStyle, int nWidth, int crColor);

        /// <summary>
        /// Selects the object.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        /// <param name="hObject">The h object.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        /// <summary>
        /// Deletes the object.
        /// </summary>
        /// <param name="hObject">The h object.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Rectangles the specified HDC.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        /// <param name="X1">The x1.</param>
        /// <param name="Y1">The y1.</param>
        /// <param name="X2">The x2.</param>
        /// <param name="Y2">The y2.</param>
        [DllImport("gdi32.dll")]
        public static extern void Rectangle(IntPtr hdc, int X1, int Y1, int X2, int Y2);

        /// <summary>
        /// RGBs the specified r.
        /// </summary>
        /// <param name="R">The r.</param>
        /// <param name="G">The g.</param>
        /// <param name="B">The b.</param>
        /// <returns>System.Int32.</returns>
        public static int RGB(int R, int G, int B)
        {
            return (R | (G << 8) | (B << 16));
        }

        /// <summary>
        /// On drop down
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);

            // Start checking for the dropdown visibility
            _dropDownCheck.Start();
        }

        /// <summary>
        /// Checks when the drop down is fully visible
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void dropDownCheck_Tick(object sender, EventArgs e)
        {
            // If the drop down has been fully dropped
            if (DroppedDown)
            {
                // Stop the time, send a listbox update
                _dropDownCheck.Stop();
                Message m = GetControlListBoxMessage(this.Handle);
                WndProc(ref m);
            }
        }

        /// <summary>
        /// Non client area border drawing
        /// </summary>
        /// <param name="handle">The handle to the control</param>
        public void DrawNativeBorder(IntPtr handle)
        {
            // Define the windows frame rectangle of the control
            RECT controlRect;
            GetWindowRect(handle, out controlRect);
            controlRect.Right -= controlRect.Left; controlRect.Bottom -= controlRect.Top;
            controlRect.Top = controlRect.Left = 0;

            // Get the device context of the control
            IntPtr dc = GetWindowDC(handle);

            // Define the client area inside the control rect
            RECT clientRect = controlRect;
            clientRect.Left += 1;
            clientRect.Top += 1;
            clientRect.Right -= 1;
            clientRect.Bottom -= 1;
            ExcludeClipRect(dc, clientRect.Left, clientRect.Top, clientRect.Right, clientRect.Bottom);

            // Create a pen and select it
            Color borderColor = SkinManager.GetCardsColor();
            IntPtr border = CreatePen(PenStyles.PS_SOLID, 1, RGB(borderColor.R, borderColor.G, borderColor.B));

            // Draw the border rectangle
            IntPtr borderPen = SelectObject(dc, border);
            Rectangle(dc, controlRect.Left, controlRect.Top, controlRect.Right, controlRect.Bottom);
            SelectObject(dc, borderPen);
            DeleteObject(border);

            // Release the device context
            ReleaseDC(handle, dc);
            SetFocus(handle);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Pen objPen = new Pen(DroppedDown ? SkinManager.ColorScheme.AccentBrush : SkinManager.GetDividersBrush(), 1);
            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.Clear(BackColor);
            g.DrawLine(objPen, new Point(0, Height - 2), new Point(Width, Height - 2));
            if (SelectedIndex >= 0) { 
            e.Graphics.DrawString(Items[SelectedIndex].ToString(), font.Roboto_Medium10, SkinManager.ColorScheme.TextBrush, 0, 0);
            }
            Point[] objTriangle = new Point[3];

            objTriangle[0] = new Point(Width - 15, Convert.ToInt32(Height * (DroppedDown ? 0.6 : 0.3)));
            objTriangle[1] = new Point(Width - 10, Convert.ToInt32(Height * (DroppedDown ? 0.3 : 0.6)));
            objTriangle[2] = new Point(Width - 5, Convert.ToInt32(Height * (DroppedDown ? 0.6 : 0.3)));

            e.Graphics.FillPolygon(DroppedDown ? SkinManager.ColorScheme.AccentBrush : SkinManager.GetDividersBrush(), objTriangle);

            base.OnPaint(e);

        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ComboBox.DrawItem" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs" /> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {

            if (e.Index < 0)
                return;




            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SkinManager.GetFlatButtonHoverBackgroundBrush(), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(SkinManager.getCardsBrush(), e.Bounds);
            }

            Rectangle rectangle = new Rectangle(2, e.Bounds.Top + 2,
                e.Bounds.Height, e.Bounds.Height - 4);
            // Draw each string in the array, using a different size, color,
            // and font for each item.
            e.Graphics.DrawString(Items[e.Index].ToString(), font.Roboto_Medium10, SkinManager.ColorScheme.TextBrush, new RectangleF(e.Bounds.X + rectangle.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));


        }

        /// <summary>
        /// Customers the measure item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MeasureItemEventArgs"/> instance containing the event data.</param>
        private void CustMeasureItem(object sender,
    System.Windows.Forms.MeasureItemEventArgs e)
        {

            switch (e.Index)
            {
                case 0:
                    e.ItemHeight = 45;
                    break;
                case 1:
                    e.ItemHeight = 20;
                    break;
                case 2:
                    e.ItemHeight = 35;
                    break;
            }
            e.ItemWidth = 260;

        }

        /// <summary>
        /// Creates a default WM_CTLCOLORLISTBOX message
        /// </summary>
        /// <param name="handle">The drop down handle</param>
        /// <returns>A WM_CTLCOLORLISTBOX message</returns>
        public Message GetControlListBoxMessage(IntPtr handle)
        {
            // Force non-client redraw for focus border
            Message m = new Message();
            m.HWnd = handle;
            m.LParam = GetListHandle(handle);
            m.WParam = IntPtr.Zero;
            m.Msg = WM_CTLCOLORLISTBOX;
            m.Result = IntPtr.Zero;
            return m;
        }

        /// <summary>
        /// Gets the list control of a combo box
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr GetListHandle(IntPtr handle)
        {
            COMBOBOXINFO info;
            info = new COMBOBOXINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            return GetComboBoxInfo(handle, ref info) ? info.hwndList : IntPtr.Zero;
        }
    }
}


