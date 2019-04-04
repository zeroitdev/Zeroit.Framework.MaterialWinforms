// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialFloatingActionButton.cs" company="Zeroit Dev Technlologies">
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Zeroit.Framework.MaterialWinforms.Animations;
using System;

namespace Zeroit.Framework.MaterialWinforms.Controls
{

    /// <summary>
    /// Class MaterialFloatingActionButton.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Button" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IShadowedMaterialControl" />
    public class MaterialFloatingActionButton : Button, IShadowedMaterialControl
    {

        /// <summary>
        /// The icon
        /// </summary>
        private Image _Icon;

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
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        [Category("Appearance")]
        public Image Icon
        {
            get { return _Icon; }
            set
            {
                _Icon = value;
            }
        }

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
        /// Gets or sets the background color of the control.
        /// </summary>
        /// <value>The color of the back.</value>
        public new Color BackColor { get { return SkinManager.ColorScheme.AccentColor; } }

        /// <summary>
        /// Gets or sets the breite.
        /// </summary>
        /// <value>The breite.</value>
        [Browsable(false)]
        [DefaultValue (typeof(int),"48")]
        public int Breite { get { return this.Width; } set { this.Width = value; } }

        /// <summary>
        /// Gets or sets the hoehe.
        /// </summary>
        /// <value>The hoehe.</value>
        [Browsable(false)]
        [DefaultValue(typeof(int), "48")]
        public int Hoehe { get { return this.Height; } set { this.Height = value; } }

        /// <summary>
        /// The animation manager
        /// </summary>
        private readonly AnimationManager animationManager;
        /// <summary>
        /// The hover animation manager
        /// </summary>
        private readonly AnimationManager hoverAnimationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialFloatingActionButton"/> class.
        /// </summary>
        public MaterialFloatingActionButton()
        {
            Height = 48;
            Width = 48;
            Elevation = 5;
            
            animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            hoverAnimationManager = new AnimationManager
            {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };
            hoverAnimationManager.OnAnimationProgress += sender => Invalidate();
            animationManager.OnAnimationProgress += sender => Invalidate();
            SizeChanged += Redraw;
            LocationChanged += Redraw;
            ParentChanged += new System.EventHandler(onParentChanged);
            MouseEnter += MaterialCard_MouseEnter;
            MouseLeave += MaterialCard_MouseLeave;
        }

        /// <summary>
        /// Handles the MouseLeave event of the MaterialCard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void MaterialCard_MouseLeave(object sender, System.EventArgs e)
        {
            Elevation /= 2;
            Redraw(null, null);
        }

        /// <summary>
        /// Handles the MouseEnter event of the MaterialCard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void MaterialCard_MouseEnter(object sender, System.EventArgs e)
        {
            Elevation *= 2;
            Redraw(null, null);
        }

        /// <summary>
        /// Redraws the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Redraw(object sender, System.EventArgs e)
        {
            if (ShadowBorder != null)
            {
                ShadowBorder.Dispose();
            }
            ShadowBorder = new GraphicsPath();
            ShadowBorder = DrawHelper.CreateCircle(Location.X ,
                                    Location.Y,
                                    ClientRectangle.Width/2 -1);
            if (Width != Height)
            {
                Width = Math.Min(Width, Height);
                Height = Math.Min(Width, Height);
            }

            Invalidate();

        }

        /// <summary>
        /// Ons the parent changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void onParentChanged(object sender, System.EventArgs e)
        {
            if (Parent != null)
            {
                Parent.BackColorChanged += new System.EventHandler(Redraw);
                Parent.Invalidate();
            }
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnMouseUp(System.Windows.Forms.MouseEventArgs)" /> event.
        /// </summary>
        /// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            animationManager.StartNewAnimation(AnimationDirection.In, mevent.Location);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            int iCropping = ClientRectangle.Width / 3;
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
           

            Region = new Region(DrawHelper.CreateCircle(ClientRectangle.X ,
                                    ClientRectangle.Y ,
                                    ClientRectangle.Width / 2));


            g.Clear(SkinManager.ColorScheme.AccentColor);


            if (_Icon != null)
            {
                g.DrawImage(_Icon, ClientRectangle.X + iCropping / 2, ClientRectangle.X + iCropping / 2, ClientRectangle.Width - iCropping, ClientRectangle.Width - iCropping);
            }

            Color c = SkinManager.GetFlatButtonHoverBackgroundColor();
            using (Brush b = new SolidBrush(Color.FromArgb((int)(hoverAnimationManager.GetProgress() * c.A), c.RemoveAlpha())))
                g.FillEllipse(b, ClientRectangle);

            if (animationManager.IsAnimating())
            {
                for (int i = 0; i < animationManager.GetAnimationCount(); i++)
                {
                    var animationValue = animationManager.GetProgress(i);
                    var animationSource = animationManager.GetSource(i);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationValue * 50)), Color.White));
                    var rippleSize = (int)(animationValue * Width *2);
                    g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                }
            }
           
        }
        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl" /> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DesignMode) return;

            MouseState = MouseState.OUT;
            MouseEnter += (sender, args) =>
            {
                MouseState = MouseState.HOVER;
                hoverAnimationManager.StartNewAnimation(AnimationDirection.In);
                Invalidate();
            };
            MouseLeave += (sender, args) =>
            {
                MouseState = MouseState.OUT;
                hoverAnimationManager.StartNewAnimation(AnimationDirection.Out);
                Invalidate();
            };
            MouseDown += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                {
                    MouseState = MouseState.DOWN;

                    animationManager.StartNewAnimation(AnimationDirection.In, args.Location);
                    Invalidate();
                }
            };
            MouseUp += (sender, args) =>
            {
                MouseState = MouseState.HOVER;

                Invalidate();
            };
        }

    }
}

