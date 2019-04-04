// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialRadioButton.cs" company="Zeroit Dev Technlologies">
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
using System.Drawing.Text;
using System.Windows.Forms;
using Zeroit.Framework.MaterialWinforms.Animations;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialRadioButton.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.RadioButton" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public class MaterialRadioButton : RadioButton, IMaterialControl
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
        /// Gets or sets the mouse location.
        /// </summary>
        /// <value>The mouse location.</value>
        [Browsable(false)]
        public Point MouseLocation { get; set; }
        /// <summary>
        /// Gets or sets the background color of the control.
        /// </summary>
        /// <value>The color of the back.</value>
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : typeof(IShadowedMaterialControl).IsAssignableFrom(Parent.GetType()) ? ((IMaterialControl)Parent).BackColor : Parent.BackColor; } }

        /// <summary>
        /// The ripple
        /// </summary>
        private bool ripple;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MaterialRadioButton"/> is ripple.
        /// </summary>
        /// <value><c>true</c> if ripple; otherwise, <c>false</c>.</value>
        [Category("Behavior")]
        public bool Ripple
        {
            get { return ripple; }
            set
            {
                ripple = value;
                AutoSize = AutoSize; //Make AutoSize directly set the bounds.

                if (value)
                {
                    Margin = new Padding(0);
                }

                Invalidate();
            }
        }

        // animation managers
        /// <summary>
        /// The animation manager
        /// </summary>
        private readonly AnimationManager animationManager;
        /// <summary>
        /// The ripple animation manager
        /// </summary>
        private readonly AnimationManager rippleAnimationManager;

        // size related variables which should be recalculated onsizechanged
        /// <summary>
        /// The radio button bounds
        /// </summary>
        private Rectangle radioButtonBounds;
        /// <summary>
        /// The box offset
        /// </summary>
        private int boxOffset;

        // size constants
        /// <summary>
        /// The radiobutton size
        /// </summary>
        private const int RADIOBUTTON_SIZE = 19;
        /// <summary>
        /// The radiobutton size half
        /// </summary>
        private const int RADIOBUTTON_SIZE_HALF = RADIOBUTTON_SIZE / 2;
        /// <summary>
        /// The radiobutton outer circle width
        /// </summary>
        private const int RADIOBUTTON_OUTER_CIRCLE_WIDTH = 2;
        /// <summary>
        /// The radiobutton inner circle size
        /// </summary>
        private const int RADIOBUTTON_INNER_CIRCLE_SIZE = RADIOBUTTON_SIZE - (2 * RADIOBUTTON_OUTER_CIRCLE_WIDTH);

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialRadioButton"/> class.
        /// </summary>
        public MaterialRadioButton()
		{
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseInOut,
                Increment = 0.06
            };
            rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
            rippleAnimationManager.OnAnimationProgress += sender => Invalidate();

            CheckedChanged += (sender, args) => animationManager.StartNewAnimation(Checked ? AnimationDirection.In : AnimationDirection.Out);
			
            SizeChanged += OnSizeChanged;

            Ripple = true;
            MouseLocation = new Point(-1, -1);
        }
        /// <summary>
        /// Handles the <see cref="E:SizeChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            boxOffset = Height / 2 - (int)Math.Ceiling(RADIOBUTTON_SIZE / 2d);
            radioButtonBounds = new Rectangle(boxOffset, boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE);
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="proposedSize">The custom-sized area for a control.</param>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            int width = boxOffset + 20 + (int)CreateGraphics().MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10).Width;
            return Ripple ? new Size(width, 30) : new Size(width, 20);
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

            // clear the control
            g.Clear(BackColor);

            var RADIOBUTTON_CENTER = boxOffset + RADIOBUTTON_SIZE_HALF;

            var animationProgress = animationManager.GetProgress();

            int colorAlpha = Enabled ? (int)(animationProgress * 255.0) : SkinManager.GetCheckBoxOffDisabledColor().A;
            int backgroundAlpha = Enabled ? (int)(SkinManager.GetCheckboxOffColor().A * (1.0 - animationProgress)) : SkinManager.GetCheckBoxOffDisabledColor().A;
            float animationSize = (float)(animationProgress * 8f);
            float animationSizeHalf = animationSize / 2;
            animationSize = (float)(animationProgress * 9f);

			var brush = new SolidBrush(Color.FromArgb(colorAlpha, Enabled ? SkinManager.ColorScheme.AccentColor : SkinManager.GetCheckBoxOffDisabledColor()));
            var pen = new Pen(brush.Color);

            // draw ripple animation
            if (Ripple && rippleAnimationManager.IsAnimating())
            {
                for (int i = 0; i < rippleAnimationManager.GetAnimationCount(); i++)
                {
                    var animationValue = rippleAnimationManager.GetProgress(i);
                    var animationSource = new Point(RADIOBUTTON_CENTER, RADIOBUTTON_CENTER);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int)((animationValue * 40)), ((bool)rippleAnimationManager.GetData(i)[0]) ? Color.Black : brush.Color));
                    var rippleHeight = (Height % 2 == 0) ? Height - 3 : Height - 2;
                    var rippleSize = (rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn) ? (int)(rippleHeight * (0.8d + (0.2d * animationValue))) : rippleHeight;
                    using (var path = DrawHelper.CreateRoundRect(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize, rippleSize / 2))
                    {
                        g.FillPath(rippleBrush, path);
                    }

                    rippleBrush.Dispose();
                }
            }

            // draw radiobutton circle
            Color uncheckedColor = DrawHelper.BlendColor(Parent.BackColor, Enabled ? SkinManager.GetCheckboxOffColor() : SkinManager.GetCheckBoxOffDisabledColor(), backgroundAlpha);

            using (var path = DrawHelper.CreateRoundRect(boxOffset, boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE, 9f))
            {
                g.FillPath(new SolidBrush(uncheckedColor), path);

	            if (Enabled)
	            {
                    g.FillPath(brush, path);
	            }
            }

            g.FillEllipse(
                new SolidBrush(Parent.BackColor), 
                RADIOBUTTON_OUTER_CIRCLE_WIDTH + boxOffset, 
                RADIOBUTTON_OUTER_CIRCLE_WIDTH + boxOffset, 
                RADIOBUTTON_INNER_CIRCLE_SIZE,
                RADIOBUTTON_INNER_CIRCLE_SIZE);

            if (Checked)
            {
                using (var path = DrawHelper.CreateRoundRect(RADIOBUTTON_CENTER - animationSizeHalf, RADIOBUTTON_CENTER - animationSizeHalf, animationSize, animationSize, 4f))
                {
                    g.FillPath(brush, path);
                }
            }
            SizeF stringSize = g.MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10);
            g.DrawString(Text, SkinManager.ROBOTO_MEDIUM_10, Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush(), boxOffset + 22, Height / 2 - stringSize.Height / 2);

            brush.Dispose();  
            pen.Dispose();
        }

        /// <summary>
        /// Determines whether [is mouse in check area].
        /// </summary>
        /// <returns><c>true</c> if [is mouse in check area]; otherwise, <c>false</c>.</returns>
        private bool IsMouseInCheckArea()
        {
            return radioButtonBounds.Contains(MouseLocation);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl" /> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Font = SkinManager.ROBOTO_MEDIUM_10;

            if (DesignMode) return;

            MouseState = MouseState.OUT;
            MouseEnter += (sender, args) =>
            {
                MouseState = MouseState.HOVER;
            };
            MouseLeave += (sender, args) =>
            {
                MouseLocation = new Point(-1, -1);
                MouseState = MouseState.OUT;
            };
            MouseDown += (sender, args) =>
            {
                MouseState = MouseState.DOWN;

                if (Ripple && args.Button == MouseButtons.Left && IsMouseInCheckArea())
                {
                    rippleAnimationManager.SecondaryIncrement = 0;
                    rippleAnimationManager.StartNewAnimation(AnimationDirection.InOutIn, new object[] { Checked });
                }
            };
            MouseUp += (sender, args) =>
            {
                MouseState = MouseState.HOVER;
                rippleAnimationManager.SecondaryIncrement = 0.08;
            };
            MouseMove += (sender, args) =>
            {
                MouseLocation = args.Location;
                Cursor = IsMouseInCheckArea() ? Cursors.Hand : Cursors.Default;
            };
        }
    }
}
