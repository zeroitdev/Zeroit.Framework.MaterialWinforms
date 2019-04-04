// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialToggle.cs" company="Zeroit Dev Technlologies">
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Zeroit.Framework.MaterialWinforms.Controls
{


    /// <summary>
    /// Class MaterialToggle.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.CheckBox" />
    public class MaterialToggle : CheckBox
    {
        #region Variables

        /// <summary>
        /// The animation timer
        /// </summary>
        Timer AnimationTimer = new Timer { Interval = 1 };
        /// <summary>
        /// The rounded rectangle
        /// </summary>
        GraphicsPath RoundedRectangle;

        /// <summary>
        /// The ellipse bg
        /// </summary>
        string EllipseBG = "#508ef5";
        /// <summary>
        /// The ellipse border
        /// </summary>
        string EllipseBorder = "#3b73d1";

        /// <summary>
        /// The ellipse back color
        /// </summary>
        Color EllipseBackColor;
        /// <summary>
        /// The ellipse border back color
        /// </summary>
        Color EllipseBorderBackColor;

        /// <summary>
        /// The enabled un checked color
        /// </summary>
        Color EnabledUnCheckedColor = ColorTranslator.FromHtml("#bcbfc4");
        /// <summary>
        /// The enabled un checked ellipse border color
        /// </summary>
        Color EnabledUnCheckedEllipseBorderColor = ColorTranslator.FromHtml("#a9acb0");

        /// <summary>
        /// The disabled ellipse back color
        /// </summary>
        Color DisabledEllipseBackColor = ColorTranslator.FromHtml("#c3c4c6");
        /// <summary>
        /// The disabled ellipse border back color
        /// </summary>
        Color DisabledEllipseBorderBackColor = ColorTranslator.FromHtml("#90949a");

        /// <summary>
        /// The point animation number
        /// </summary>
        int PointAnimationNum = 4;
        /// <summary>
        /// The animation running
        /// </summary>
        bool AnimationRunning = false;

        #endregion
        #region  Properties

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
        /// Gets or sets the background color of the control.
        /// </summary>
        /// <value>The color of the back.</value>
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : typeof(IShadowedMaterialControl).IsAssignableFrom(Parent.GetType()) ? ((IMaterialControl)Parent).BackColor : Parent.BackColor; } }

        /// <summary>
        /// Gets or sets the color of the ellipse.
        /// </summary>
        /// <value>The color of the ellipse.</value>
        [Category("Appearance")]
        public string EllipseColor
        {
            get { return EllipseBG; }
            set
            {
                EllipseBG = value;
                Invalidate();
            }
        }
        /// <summary>
        /// Gets or sets the color of the ellipse border.
        /// </summary>
        /// <value>The color of the ellipse border.</value>
        [Category("Appearance")]
        public string EllipseBorderColor
        {
            get { return EllipseBorder; }
            set
            {
                EllipseBorder = value;
                Invalidate();
            }
        }

        #endregion
        #region Events

        /// <summary>
        /// Handles the <see cref="E:HandleCreated" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AnimationTimer.Start();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            Height = 19; Width = 47;

            RoundedRectangle = new GraphicsPath();
            int radius = 10;

            RoundedRectangle.AddArc(11, 4, radius - 1, radius, 180, 90);
            RoundedRectangle.AddArc(Width - 21, 4, radius - 1, radius, -90, 90);
            RoundedRectangle.AddArc(Width - 21, Height - 15, radius - 1, radius, 0, 90);
            RoundedRectangle.AddArc(11, Height - 15, radius - 1, radius, 90, 90);

            RoundedRectangle.CloseAllFigures();
            Invalidate();
        }

        /// <summary>
        /// Delegate AnimationFinished
        /// </summary>
        public delegate void AnimationFinished();
        /// <summary>
        /// Occurs when [on animation finished].
        /// </summary>
        public event AnimationFinished onAnimationFinished;

        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialToggle"/> class.
        /// </summary>
        public MaterialToggle()
        {
            Height = 19; Width = 47; DoubleBuffered = true;
            AnimationTimer.Tick += new EventHandler(AnimationTick);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var G = pevent.Graphics;
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(BackColor);

            EllipseBackColor = ColorTranslator.FromHtml(EllipseBG);
            EllipseBorderBackColor = ColorTranslator.FromHtml(EllipseBorder);

            G.FillPath(new SolidBrush(Color.FromArgb(115, Enabled ? Checked ? SkinManager.ColorScheme.AccentColor : EnabledUnCheckedColor : EnabledUnCheckedColor)), RoundedRectangle);
            G.DrawPath(new Pen(Color.FromArgb(50, Enabled ? Checked ? SkinManager.ColorScheme.AccentColor : EnabledUnCheckedColor : EnabledUnCheckedColor)), RoundedRectangle);

            G.FillEllipse(new SolidBrush(Enabled ? Checked ? SkinManager.ColorScheme.AccentColor : Color.White : DisabledEllipseBackColor), PointAnimationNum, 0, 18, 18);
            G.DrawEllipse(new Pen(Enabled ? Checked ? SkinManager.ColorScheme.AccentColor : EnabledUnCheckedEllipseBorderColor : DisabledEllipseBorderBackColor), PointAnimationNum, 0, 18, 18);
        }

        /// <summary>
        /// Animations the tick.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void AnimationTick(object sender, EventArgs e)
        {
            if (Checked)
            {
                if (PointAnimationNum < 24)
                {
                    PointAnimationNum += 1;
                    AnimationRunning = true;
                    this.Invalidate();
                }
                else if (AnimationRunning)
                {
                    if (onAnimationFinished != null)
                    {
                        onAnimationFinished();
                    }
                    AnimationRunning = false;
                }
            }
            else
            {


                if (PointAnimationNum > 4)
                {
                    PointAnimationNum -= 1;
                    AnimationRunning = true;
                    this.Invalidate();
                }
                else if(AnimationRunning)
                {
                    if (onAnimationFinished != null)
                    {
                        onAnimationFinished();
                    }
                    AnimationRunning = false;
                }
            }
        }
    }
}

