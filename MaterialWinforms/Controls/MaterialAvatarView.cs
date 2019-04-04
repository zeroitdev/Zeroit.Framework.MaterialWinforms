// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialAvatarView.cs" company="Zeroit Dev Technlologies">
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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialAvatarView.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IShadowedMaterialControl" />
    public class MaterialAvatarView : Control, IShadowedMaterialControl
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
        /// Gets or sets the elevation.
        /// </summary>
        /// <value>The elevation.</value>
        public int Elevation { get; set; }
        /// <summary>
        /// Gets or sets the shadow border.
        /// </summary>
        /// <value>The shadow border.</value>
        [Browsable(false)]
        public GraphicsPath ShadowBorder { get; set; }

        /// <summary>
        /// The avater
        /// </summary>
        private Image _Avater;
        /// <summary>
        /// The avatar scaled
        /// </summary>
        private Image _AvatarScaled;
        /// <summary>
        /// Gets or sets the avatar.
        /// </summary>
        /// <value>The avatar.</value>
        public Image Avatar
        {
            get
            {
                return _Avater;
            }
            set
            {
                _Avater = value;
                _AvatarScaled = DrawHelper.ResizeImage(_Avater, Width-1, Height-1);
            }

        }

        /// <summary>
        /// The avatar letter
        /// </summary>
        private String _AvatarLetter;
        /// <summary>
        /// Gets or sets the avatar letter.
        /// </summary>
        /// <value>The avatar letter.</value>
        public String AvatarLetter
        {
            get
            {
                return _AvatarLetter;
            }
            set
            {
                _AvatarLetter = value;
                CalculateAvatarFont();
            }
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public override Color BackColor { get { return Color.Transparent; } }

        /// <summary>
        /// The object font manager
        /// </summary>
        private FontManager objFontManager;
        /// <summary>
        /// The avatar font
        /// </summary>
        private Font AvatarFont;
        /// <summary>
        /// The object graphic
        /// </summary>
        private Graphics objGraphic;
        /// <summary>
        /// The text rect
        /// </summary>
        private Rectangle TextRect;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialAvatarView"/> class.
        /// </summary>
        public MaterialAvatarView()
        {
            objFontManager = new FontManager();
            DoubleBuffered = true;
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);

            Elevation = 5;
            objGraphic = CreateGraphics();
            Width = 80;
            Height = 80;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            if(Width>Height)
            {
                Height = Width;
            }
            else
            {
                Width = Height;
            }
            TextRect = new Rectangle(Convert.ToInt32(Width * 0.05), Convert.ToInt32(Height * 0.05),Convert.ToInt32(Width * 0.9), Convert.ToInt32(Height * 0.9));
            Region = new Region(DrawHelper.CreateCircle(0, 0, Width / 2));
            CalculateAvatarFont();
            if (ShadowBorder != null)
            {
                ShadowBorder.Dispose();
            }
            ShadowBorder = new GraphicsPath();
            ShadowBorder = DrawHelper.CreateCircle(Location.X,
                                    Location.Y,
                                    ClientRectangle.Width / 2 -1);
            if (_AvatarScaled != null) { 
            _AvatarScaled.Dispose();
            _AvatarScaled = DrawHelper.ResizeImage(_Avater, Width, Height);
            }
        }

        /// <summary>
        /// Calculates the avatar font.
        /// </summary>
        private void CalculateAvatarFont()
        {
            if (!String.IsNullOrEmpty(_AvatarLetter))
            {
                AvatarFont = objFontManager.ScaleTextToRectangle(objGraphic, AvatarLetter, TextRect);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

           
            if (Avatar == null)
            {
                g.FillPath(SkinManager.ColorScheme.PrimaryBrush, DrawHelper.CreateCircle(1, 1, Height / 2 - 1));
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.DrawString(AvatarLetter, AvatarFont, SkinManager.ACTION_BAR_TEXT_BRUSH(), TextRect, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                 
            }
            else
            {

                using (Brush brush = new TextureBrush(_AvatarScaled))
                {
                    g.FillEllipse(brush, new Rectangle(1, 1, Width-1, Height-1));
                }
            }
        }


        /// <summary>
        /// Gets the required creation parameters when the control handle is created.
        /// </summary>
        /// <value>The create parameters.</value>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parms = base.CreateParams;
                parms.ExStyle |= 0x20;
                return parms;
            }
        }
    }
}
