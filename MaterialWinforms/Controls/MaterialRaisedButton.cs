// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialRaisedButton.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Zeroit.Framework.MaterialWinforms.Animations;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialRaisedButton.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Button" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IShadowedMaterialControl" />
    public class MaterialRaisedButton : Button, IShadowedMaterialControl
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
        /// Gets or sets a value indicating whether this <see cref="MaterialRaisedButton"/> is primary.
        /// </summary>
        /// <value><c>true</c> if primary; otherwise, <c>false</c>.</value>
        public bool Primary { get; set; }
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
        public Color BackColor { get { return Primary ? SkinManager.ColorScheme.PrimaryColor : SkinManager.getRaisedButtonBackroundColor(); } }

        /// <summary>
        /// The animation manager
        /// </summary>
        private readonly AnimationManager animationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialRaisedButton"/> class.
        /// </summary>
        public MaterialRaisedButton()
        {
            Primary = true;

            animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            Elevation = 5;
            animationManager.OnAnimationProgress += sender => Invalidate();
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
        /// Raises the <see cref="E:System.Windows.Forms.Control.LocationChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLocationChanged(System.EventArgs e)
        {
            base.OnLocationChanged(e);
            ShadowBorder = new GraphicsPath();
            ShadowBorder.AddRectangle(new Rectangle(this.Location.X,this.Location.Y,Width,Height));
        }

        /// <summary>
        /// Handles the MouseLeave event of the MaterialCard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void MaterialCard_MouseLeave(object sender, System.EventArgs e)
        {
            Elevation /= 2;
            Refresh();
        }

        /// <summary>
        /// Handles the MouseEnter event of the MaterialCard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void MaterialCard_MouseEnter(object sender, System.EventArgs e)
        {
            Elevation *= 2;
            Refresh();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            ShadowBorder = new GraphicsPath();
            ShadowBorder.AddRectangle(new Rectangle(this.Location.X, this.Location.Y, Width, Height));
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(Parent.BackColor);
            
            using (var backgroundPath = DrawHelper.CreateRoundRect(ClientRectangle.X,
                ClientRectangle.Y,
                ClientRectangle.Width ,
                ClientRectangle.Height ,
                1f))
            {
                g.FillPath(Primary ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetRaisedButtonBackgroundBrush(), backgroundPath);
            }

            if (animationManager.IsAnimating())
            {
                for (int i = 0; i < animationManager.GetAnimationCount(); i++)
                {
                    var animationValue = animationManager.GetProgress(i);
                    var animationSource = animationManager.GetSource(i);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationValue * 50)), Color.White));
                    var rippleSize = (int)(animationValue * Width * 2);
                    g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                }
            }

            g.DrawString(
                Text.ToUpper(),
                SkinManager.ROBOTO_MEDIUM_10, 
                SkinManager.GetRaisedButtonTextBrush(Primary),
                ClientRectangle,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
    }
}
