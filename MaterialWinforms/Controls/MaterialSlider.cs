// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialSlider.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialSlider.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public partial class MaterialSlider : Control, IMaterialControl
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
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : typeof(IShadowedMaterialControl).IsAssignableFrom(Parent.GetType()) ? ((IMaterialControl)Parent).BackColor : Parent.BackColor; } }
        /// <summary>
        /// Delegate ValueChanged
        /// </summary>
        /// <param name="newValue">The new value.</param>
        [Browsable(false)]

        public delegate void ValueChanged(int newValue);
        /// <summary>
        /// Occurs when [on value changed].
        /// </summary>
        public event ValueChanged onValueChanged;

        /// <summary>
        /// The value
        /// </summary>
        private int _Value;
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                MouseX = (int)((double)_Value / (double)(MaxValue - MinValue) * (double)(Width - IndicatorSize));
            }
        }
        /// <summary>
        /// The maximum value
        /// </summary>
        private int _MaxValue;
        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public int MaxValue
        {
            get { return _MaxValue; }
            set
            {
                _MaxValue = value;
                MouseX = (int)((double)_Value / (double)(MaxValue - MinValue) * (double)(Width - IndicatorSize));
            }
        }
        /// <summary>
        /// The minimum value
        /// </summary>
        private int _MinValue;
        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        public int MinValue
        {
            get { return _MinValue; }
            set
            {
                _MinValue = value;
                MouseX = (int)((double)_Value / (double)(MaxValue - MinValue) * (double)(Width - IndicatorSize));
            }
        }

        /// <summary>
        /// The mouse pressed
        /// </summary>
        private bool MousePressed;
        /// <summary>
        /// The mouse x
        /// </summary>
        private int MouseX;

        /// <summary>
        /// The indicator size
        /// </summary>
        private int IndicatorSize;
        /// <summary>
        /// The hovered
        /// </summary>
        private bool hovered = false;

        /// <summary>
        /// The indicator rectangle
        /// </summary>
        private Rectangle IndicatorRectangle;
        /// <summary>
        /// The indicator rectangle normal
        /// </summary>
        private Rectangle IndicatorRectangleNormal;
        /// <summary>
        /// The indicator rectangle pressed
        /// </summary>
        private Rectangle IndicatorRectanglePressed;
        /// <summary>
        /// The indicator rectangle disabled
        /// </summary>
        private Rectangle IndicatorRectangleDisabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialSlider"/> class.
        /// </summary>
        public MaterialSlider()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Selectable, true);
            IndicatorSize = 30;
            MaxValue = 100;
            Width = 80;
            MinValue = 0;
            Height = IndicatorSize + 10;

            Value = 50;

            IndicatorRectangle = new Rectangle(0, 10, IndicatorSize, IndicatorSize);
            IndicatorRectangleNormal = new Rectangle();
            IndicatorRectanglePressed = new Rectangle();

            EnabledChanged += MaterialSlider_EnabledChanged;

            DoubleBuffered = true;

        }

        /// <summary>
        /// Handles the EnabledChanged event of the MaterialSlider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void MaterialSlider_EnabledChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Height = IndicatorSize + 10;
            MouseX = (int)((double)_Value / (double)(MaxValue - MinValue) * (double)(Width - IndicatorSize));
            RecalcutlateIndicator();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            hovered = true;
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            hovered = false;
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Y > IndicatorRectanglePressed.Top && e.Y < IndicatorRectanglePressed.Bottom)
            {
                MousePressed = true;
                if (e.X >= IndicatorSize / 2 && e.X <= Width - IndicatorSize / 2)
                {
                    MouseX = e.X - IndicatorSize / 2;
                    double ValuePerPx = ((double)(MaxValue - MinValue)) / (Width - IndicatorSize);
                    int v = (int)(ValuePerPx * MouseX);
                    if (v != _Value)
                    {
                        _Value = v;
                        if (onValueChanged != null) onValueChanged(_Value);
                    }
                    RecalcutlateIndicator();
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            hovered = true;
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hovered = false;
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MousePressed = false;
            Invalidate();
        }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (MousePressed)
            {
                if (e.X >= IndicatorSize / 2 && e.X <= Width - IndicatorSize / 2)
                {
                    MouseX = e.X - IndicatorSize / 2;
                    double ValuePerPx = ((double)(MaxValue - MinValue)) / (Width - IndicatorSize);
                    int v = (int)(ValuePerPx * MouseX);
                    if (v != _Value)
                    {
                        _Value = v;
                        if (onValueChanged != null) onValueChanged(_Value);
                    }
                    RecalcutlateIndicator();
                }
            }
        }

        /// <summary>
        /// Recalcutlates the indicator.
        /// </summary>
        private void RecalcutlateIndicator()
        {
            int iWidht = Width - IndicatorSize;

            IndicatorRectangle = new Rectangle(MouseX, Height - IndicatorSize, IndicatorSize, IndicatorSize);
            IndicatorRectangleNormal = new Rectangle(IndicatorRectangle.X + (int)(IndicatorRectangle.Width * 0.25), IndicatorRectangle.Y + (int)(IndicatorRectangle.Height * 0.25), (int)(IndicatorRectangle.Width * 0.5), (int)(IndicatorRectangle.Height * 0.5));
            IndicatorRectanglePressed = new Rectangle(IndicatorRectangle.X + (int)(IndicatorRectangle.Width * 0.165), IndicatorRectangle.Y + (int)(IndicatorRectangle.Height * 0.165), (int)(IndicatorRectangle.Width * 0.66), (int)(IndicatorRectangle.Height * 0.66));
            IndicatorRectangleDisabled = new Rectangle(IndicatorRectangle.X + (int)(IndicatorRectangle.Width * 0.34), IndicatorRectangle.Y + (int)(IndicatorRectangle.Height * 0.34), (int)(IndicatorRectangle.Width * 0.33), (int)(IndicatorRectangle.Height * 0.33));
            Invalidate();
        }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(BackColor);
            Color LineColor;
            Brush DisabledBrush;
            Color BalloonColor;

            if (SkinManager.Theme == MaterialSkinManager.Themes.DARK)
            {
                LineColor = Color.FromArgb((int)(2.55 * 30), 255, 255, 255);
            }
            else
            {
                LineColor = Color.FromArgb((int)(2.55 * (hovered ? 38 : 26)), 0, 0, 0);
            }

            DisabledBrush = new SolidBrush(LineColor);
            BalloonColor = Color.FromArgb((int)(2.55 * 30), (Value == 0 ? Color.Gray : SkinManager.ColorScheme.AccentColor));

            Pen LinePen = new Pen(LineColor, 2);

            g.DrawLine(LinePen, IndicatorSize / 2, Height / 2 + (Height - IndicatorSize) / 2, Width - IndicatorSize / 2, Height / 2 + (Height - IndicatorSize) / 2);

            if (Enabled)
            {
                g.DrawLine(SkinManager.ColorScheme.AccentPen, IndicatorSize / 2, Height / 2 + (Height - IndicatorSize) / 2, IndicatorRectangleNormal.X, Height / 2 + (Height - IndicatorSize) / 2);

                if (MousePressed)
                {
                    if (Value > MinValue)
                    {
                        g.FillEllipse(SkinManager.ColorScheme.AccentBrush, IndicatorRectanglePressed);
                    }
                    else
                    {
                        g.FillEllipse(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), IndicatorRectanglePressed);
                        g.DrawEllipse(LinePen, IndicatorRectanglePressed);
                    }
                }
                else
                {
                    if (Value > MinValue)
                    {
                        g.FillEllipse(SkinManager.ColorScheme.AccentBrush, IndicatorRectangleNormal);
                    }
                    else
                    {
                        g.FillEllipse(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), IndicatorRectangleNormal);
                        g.DrawEllipse(LinePen, IndicatorRectangleNormal);
                    }


                    if (hovered)
                    {
                        g.FillEllipse(new SolidBrush(BalloonColor), IndicatorRectangle);
                    }
                }
            }
            else
            {
                if (Value > MinValue)
                {
                    g.FillEllipse(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), IndicatorRectangleNormal);
                    g.FillEllipse(DisabledBrush, IndicatorRectangleDisabled);
                }
                else
                {
                    g.FillEllipse(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), IndicatorRectangleNormal);
                    g.DrawEllipse(LinePen, IndicatorRectangleDisabled);
                }
            }


            g.DrawString(MinValue.ToString(), SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetPrimaryTextBrush(), new PointF(0, 0));
            g.DrawString(MaxValue.ToString(), SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetPrimaryTextBrush(), new PointF(Width - g.MeasureString(MaxValue.ToString(), SkinManager.ROBOTO_MEDIUM_10).Width, 0f));
            g.DrawString(Value.ToString(), SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetPrimaryTextBrush(), new PointF(Width / 2 - g.MeasureString(Value.ToString(), SkinManager.ROBOTO_MEDIUM_10).Width / 2, 0f));
            e.Graphics.DrawImage((Image)bmp.Clone(), 0, 0);
        }
    }
}
