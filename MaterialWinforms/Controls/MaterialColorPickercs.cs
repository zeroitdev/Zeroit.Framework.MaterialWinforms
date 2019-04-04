// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialColorPickercs.cs" company="Zeroit Dev Technlologies">
//    This program is for creating Material Design controls.
//    Copyright ©  2017  Zeroit Dev Technologies
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
//    You can contact me at zeroitdevnet@gmail.com or zeroitdev@outlook.com
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Zeroit.Framework.MaterialWinforms.Animations;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialColorPicker.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public class MaterialColorPicker : Control, IMaterialControl
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
        /// The color rectangles
        /// </summary>
        private List<ColorRect> ColorRectangles;

        /// <summary>
        /// The object animation manager
        /// </summary>
        private AnimationManager objAnimationManager;

        /// <summary>
        /// Delegate ColorChanged
        /// </summary>
        /// <param name="newColor">The new color.</param>
        public delegate void ColorChanged(Color newColor);
        /// <summary>
        /// Occurs when [on color changed].
        /// </summary>
        public event ColorChanged onColorChanged;

        /// <summary>
        /// The red slider
        /// </summary>
        private MaterialColorSlider RedSlider, GreenSlider, BlueSlider;
        /// <summary>
        /// The p base color
        /// </summary>
        private Color pBaseColor;
        /// <summary>
        /// The object shadow path
        /// </summary>
        private GraphicsPath objShadowPath;

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public Color BackColor { get { return SkinManager.GetCardsColor(); } set{} }

        /// <summary>
        /// The current hovered path
        /// </summary>
        private GraphicsPath CurrentHoveredPath;
        /// <summary>
        /// The hovered index
        /// </summary>
        private int HoveredIndex;
        /// <summary>
        /// The temporary color
        /// </summary>
        private Color TempColor;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public Color Value
        {
            get { return pBaseColor; }
            set
            {
                pBaseColor = value;
                RedSlider.Value = pBaseColor.R;
                GreenSlider.Value = pBaseColor.G;
                BlueSlider.Value = pBaseColor.B;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            for (int i = 0; i < ColorRectangles.Count; i++)
            {
                if (ColorRectangles[i].Rect.Contains(e.Location))
                {
                    if (i != HoveredIndex)
                    {
                        HoveredIndex = i;
                        CurrentHoveredPath = new GraphicsPath();
                        CurrentHoveredPath.AddRectangle(new Rectangle(ColorRectangles[i].Rect.X-3,ColorRectangles[i].Rect.Y-3,ColorRectangles[i].Rect.Width,ColorRectangles[i].Rect.Height));
                        Invalidate();   
                    }
                    return;
                }
            }
            if(HoveredIndex>=0){
                HoveredIndex = -1;
                CurrentHoveredPath = new GraphicsPath();
                Invalidate();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (HoveredIndex >= 0)
                {
                    TempColor = ColorRectangles[HoveredIndex].Color;
                    objAnimationManager.SetProgress(0);
                    objAnimationManager.StartNewAnimation(AnimationDirection.In);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            HoveredIndex = -1;
            Invalidate();
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            objShadowPath = new GraphicsPath();
            objShadowPath.AddLine(0, (int)(Height * 0.6), Width, (int)(Height * 0.6));
            Invalidate();
        }

        /// <summary>
        /// Initializes the color hints.
        /// </summary>
        private void initColorHints()
        {
            ColorRectangles = new List<ColorRect>();
            int x = 0;
            int y = 0;
            
            int RectWidth = (Width / 8);
          
            int RectY = BlueSlider.Bottom + 5;
            int RectHeight = (Height - RectY) / 3;


            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Red200).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Red500).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Red700).ToColor()));
            y = 0;
            x++;

            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Purple200).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Purple500).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Purple700).ToColor()));
            y = 0;
            x++;

            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Indigo200).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Indigo500).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Indigo700).ToColor()));
            y = 0;
            x++;

            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.LightBlue200).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.LightBlue500).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.LightBlue700).ToColor()));
            y = 0;
            x++;

            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Teal200).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Teal500).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Teal700).ToColor()));
            y = 0;
            x++;

            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.LightGreen200).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.LightGreen500).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.LightGreen700).ToColor()));
            y = 0;
            x++;

            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Yellow200).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Yellow500).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Yellow700).ToColor()));
            y = 0;
            x++;

            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Orange200).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Orange500).ToColor()));
            y++;
            ColorRectangles.Add(new ColorRect(new Rectangle(RectWidth * x, RectHeight * y + RectY, RectWidth, RectHeight), ((int)Primary.Orange700).ToColor()));

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialColorPicker"/> class.
        /// </summary>
        public MaterialColorPicker()
        {
            pBaseColor = SkinManager.ColorScheme.AccentColor;
            Width = 248;
            Height = 425;

            RedSlider = new MaterialColorSlider("R", Color.Red, pBaseColor.R);
            GreenSlider = new MaterialColorSlider("G", Color.Green, pBaseColor.G);
            BlueSlider = new MaterialColorSlider("B", Color.Blue, pBaseColor.B);

            RedSlider.Width = Width;
            GreenSlider.Width = Width;
            BlueSlider.Width = Width;

            RedSlider.onValueChanged += RedSlider_onValueChanged;
            GreenSlider.onValueChanged += GreenSlider_onValueChanged;
            BlueSlider.onValueChanged += BlueSlider_onValueChanged;

            Controls.Add(RedSlider);
            Controls.Add(GreenSlider);
            Controls.Add(BlueSlider);

            RedSlider.Location = new Point(0, (int)(Height * 0.61));
            GreenSlider.Location = new Point(0, RedSlider.Location.Y + RedSlider.Height + 5);
            BlueSlider.Location = new Point(0, GreenSlider.Location.Y + GreenSlider.Height + 5);

            objShadowPath = new GraphicsPath();
            objShadowPath.AddLine(0, (int)(Height * 0.6), Width, (int)(Height * 0.6));

            HoveredIndex = -1;
            initColorHints();

            DoubleBuffered = true;

            objAnimationManager = new AnimationManager()
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseInOut
            };
            objAnimationManager.OnAnimationProgress += sender => Invalidate();
            objAnimationManager.OnAnimationFinished += objAnimationManager_OnAnimationFinished;
        }

        /// <summary>
        /// Objects the animation manager on animation finished.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void objAnimationManager_OnAnimationFinished(object sender)
        {
            Value = TempColor;
            if (onColorChanged != null) onColorChanged(pBaseColor);
        }

        /// <summary>
        /// Reds the slider on value changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        void RedSlider_onValueChanged(int newValue)
        {
            if (!objAnimationManager.IsAnimating()) { 
            pBaseColor = Color.FromArgb(newValue, pBaseColor.G, pBaseColor.B);
                if(onColorChanged!= null) onColorChanged(pBaseColor);
            Invalidate();
            }
        }

        /// <summary>
        /// Greens the slider on value changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        void GreenSlider_onValueChanged(int newValue)
        {
            if (!objAnimationManager.IsAnimating())
            {
                pBaseColor = Color.FromArgb(pBaseColor.R, newValue, pBaseColor.B);
                if(onColorChanged!= null) onColorChanged(pBaseColor);
                Invalidate();
            }
        }

        /// <summary>
        /// Blues the slider on value changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        void BlueSlider_onValueChanged(int newValue)
        {
            if (!objAnimationManager.IsAnimating()) { 
            pBaseColor = Color.FromArgb(pBaseColor.R, pBaseColor.G, newValue);
                if(onColorChanged!= null) onColorChanged(pBaseColor);
            Invalidate();
            }
        }



        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.Clear(SkinManager.GetCardsColor());
            g.FillRectangle(new SolidBrush(pBaseColor), 0, 0, Width, (int)(Height * 0.6));
            DrawHelper.drawShadow(g, objShadowPath, 20, SkinManager.GetCardsColor());

            foreach (ColorRect objColor in ColorRectangles)
            {
                g.FillRectangle(new SolidBrush(objColor.Color), objColor.Rect);

            }

            if (HoveredIndex >= 0)
            {
                DrawHelper.drawShadow(g, CurrentHoveredPath, 4, Color.Black);
                g.FillRectangle(new SolidBrush(ColorRectangles[HoveredIndex].Color), ColorRectangles[HoveredIndex].Rect);
            }

            if (objAnimationManager.IsAnimating())
            {
                RedSlider.Value =(int)(pBaseColor.R + (TempColor.R - pBaseColor.R) * objAnimationManager.GetProgress());
                GreenSlider.Value = (int)(pBaseColor.G + (TempColor.G - pBaseColor.G) * objAnimationManager.GetProgress());
                BlueSlider.Value = (int)(pBaseColor.B + (TempColor.B - pBaseColor.B) * objAnimationManager.GetProgress());
                Rectangle clip = new Rectangle(0, 0, Width, (int)(Height * 0.6));
                g.SetClip(clip);
                int xPos, yPos;
                xPos = (int)((clip.Width * 0.5) - (clip.Width * objAnimationManager.GetProgress()));
                yPos = (int)((clip.Height * 0.5) - (clip.Height * objAnimationManager.GetProgress()));
                g.FillEllipse(new SolidBrush(TempColor), new Rectangle(xPos, yPos, (int)(clip.Width * 2 * objAnimationManager.GetProgress()), (int)(clip.Height * 2 * objAnimationManager.GetProgress())));
                g.ResetClip();
            }

        }
    }

    /// <summary>
    /// Class ColorRect.
    /// </summary>
    class ColorRect
    {
        /// <summary>
        /// The rect
        /// </summary>
        public Rectangle Rect;
        /// <summary>
        /// The color
        /// </summary>
        public Color Color;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRect"/> class.
        /// </summary>
        /// <param name="pRect">The p rect.</param>
        /// <param name="pColor">Color of the p.</param>
        public ColorRect(Rectangle pRect, Color pColor)
        {
            Rect = pRect;
            Color = pColor;
        }
    }

    /// <summary>
    /// Class MaterialColorSlider.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    class MaterialColorSlider : Control, IMaterialControl
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
        /// The value
        /// </summary>
        [Browsable(false)]

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
                MouseX = SliderRectangle.X + ((int)((double)_Value / (double)(MaxValue - MinValue) * (double)(SliderRectangle.Width - IndicatorSize)));
                RecalcutlateIndicator();
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
                MouseX = SliderRectangle.X + ((int)((double)_Value / (double)(MaxValue - MinValue) * (double)(SliderRectangle.Width - IndicatorSize)));
                RecalcutlateIndicator();
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
                MouseX = SliderRectangle.X + ((int)((double)_Value / (double)(MaxValue - MinValue) * (double)(SliderRectangle.Width - IndicatorSize)));
                RecalcutlateIndicator();
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
        public static int IndicatorSize = 25;
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
        /// The descriptio rectangle
        /// </summary>
        private Rectangle DescriptioRectangle;
        /// <summary>
        /// The value rectangle
        /// </summary>
        private Rectangle ValueRectangle;
        /// <summary>
        /// The slider rectangle
        /// </summary>
        private Rectangle SliderRectangle;
        /// <summary>
        /// The beschreibung
        /// </summary>
        private String Beschreibung;
        /// <summary>
        /// The accent color
        /// </summary>
        private Color AccentColor;
        /// <summary>
        /// The accent brush
        /// </summary>
        private Brush AccentBrush;
        /// <summary>
        /// The border distance
        /// </summary>
        private int BorderDistance;

        /// <summary>
        /// Delegate ValueChanged
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public delegate void ValueChanged(int newValue);
        /// <summary>
        /// Occurs when [on value changed].
        /// </summary>
        public event ValueChanged onValueChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialColorSlider"/> class.
        /// </summary>
        /// <param name="pBeschreibung">The p beschreibung.</param>
        /// <param name="pAccent">The p accent.</param>
        /// <param name="pValue">The p value.</param>
        public MaterialColorSlider(string pBeschreibung, Color pAccent, int pValue)
        {

            SetStyle(ControlStyles.Selectable, true);
            Beschreibung = pBeschreibung;
            AccentColor = pAccent;
            AccentBrush = new SolidBrush(AccentColor);
            MaxValue = 255;
            MinValue = 0;
            Height = IndicatorSize;
            BorderDistance = Width / 4;
            Value = pValue;

            ValueRectangle = new Rectangle((int)(Width * 0.8), 0, (int)(Width * 0.2), IndicatorSize);
            DescriptioRectangle = new Rectangle(0, 0, (int)(Width * 0.2), IndicatorSize);
            SliderRectangle = new Rectangle(DescriptioRectangle.Right, 0, (int)(Width * 0.6), IndicatorSize);

            IndicatorRectangle = new Rectangle(0, 0, IndicatorSize, IndicatorSize);
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
            Height = IndicatorSize;
            ValueRectangle = new Rectangle((int)(Width * 0.8), 0, (int)(Width * 0.2), IndicatorSize);
            DescriptioRectangle = new Rectangle(0, 0, (int)(Width * 0.2), IndicatorSize);
            SliderRectangle = new Rectangle(DescriptioRectangle.Right, 0, (int)(Width * 0.6), IndicatorSize);
            MouseX = SliderRectangle.X + ((int)((double)_Value / (double)(MaxValue - MinValue) * (double)(SliderRectangle.Width - IndicatorSize)));
            BorderDistance = Width / 4;
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
                if (e.X >= SliderRectangle.X + (IndicatorSize / 2) && e.X <= SliderRectangle.Right - IndicatorSize / 2)
                {
                    MouseX = e.X - IndicatorSize / 2;
                    double ValuePerPx = ((double)(MaxValue - MinValue)) / (SliderRectangle.Width - IndicatorSize);
                    int v = (int)(ValuePerPx * (MouseX - SliderRectangle.X));
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
                if (e.X >= SliderRectangle.X + (IndicatorSize / 2) && e.X <= SliderRectangle.Right - IndicatorSize * 0.5)
                {
                    MouseX = e.X - IndicatorSize / 2;
                    double ValuePerPx = ((double)(MaxValue - MinValue)) / (SliderRectangle.Width - IndicatorSize);
                    int v = (int)(ValuePerPx * (MouseX - SliderRectangle.X));
                    if (v > MaxValue) v = MaxValue;
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
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.Clear(SkinManager.GetCardsColor());
            Color LineColor;
            Brush DisabledBrush;
            Color BalloonColor;
            Brush AccentBrush = new SolidBrush(AccentColor);
            Pen AccentPen = new Pen(AccentBrush, 2);

            if (SkinManager.Theme == MaterialSkinManager.Themes.DARK)
            {
                LineColor = Color.FromArgb((int)(2.55 * 30), 255, 255, 255);
            }
            else
            {
                LineColor = Color.FromArgb((int)(2.55 * (hovered ? 38 : 26)), 0, 0, 0);
            }

            DisabledBrush = new SolidBrush(LineColor);
            BalloonColor = Color.FromArgb((int)(2.55 * 30), (Value == 0 ? Color.Gray : AccentColor));

            Pen LinePen = new Pen(LineColor, 2);


            g.DrawLine(LinePen, SliderRectangle.X + (IndicatorSize / 2), Height / 2, SliderRectangle.Right - (IndicatorSize / 2), Height / 2);


            if (Enabled)
            {
                g.DrawLine(AccentPen, IndicatorSize / 2 + SliderRectangle.X, Height / 2, IndicatorRectangleNormal.X, Height / 2);

                if (MousePressed)
                {
                    if (Value > MinValue)
                    {
                        g.FillEllipse(AccentBrush, IndicatorRectanglePressed);
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
                        g.FillEllipse(AccentBrush, IndicatorRectangleNormal);
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


            g.DrawString(Beschreibung, SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetPrimaryTextBrush(), DescriptioRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            g.DrawString(Value.ToString(), SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetPrimaryTextBrush(), ValueRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            e.Graphics.DrawImage((Image)bmp.Clone(), 0, 0);
        }
    }

}
