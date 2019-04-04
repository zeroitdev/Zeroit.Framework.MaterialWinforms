// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialLoadingFloatingActionButton.cs" company="Zeroit Dev Technlologies">
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
    /// Class MaterialLoadingFloatingActionButton.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Button" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IShadowedMaterialControl" />
    public class MaterialLoadingFloatingActionButton : Button, IShadowedMaterialControl
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
        /// Gets or sets the background color of the control.
        /// </summary>
        /// <value>The color of the back.</value>
        public Color BackColor { get { return SkinManager.ColorScheme.AccentColor; } }

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
        /// Gets or sets the breite.
        /// </summary>
        /// <value>The breite.</value>
        [Browsable(false)]
        [DefaultValue(typeof(int), "48")]
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
        /// The progress timer
        /// </summary>
        private Timer ProgressTimer;
        /// <summary>
        /// The progress finished
        /// </summary>
        private bool _ProgressFinished;
        /// <summary>
        /// The show progress
        /// </summary>
        private bool showProgress;
        /// <summary>
        /// The farbe getauscht
        /// </summary>
        private bool farbeGetauscht = false;
        /// <summary>
        /// The progress background pen
        /// </summary>
        private Pen ProgressBackgroundPen;
        /// <summary>
        /// The progress pen
        /// </summary>
        private Pen ProgressPen;
        /// <summary>
        /// The current progress position
        /// </summary>
        private Point CurrentProgressPosition;
        /// <summary>
        /// The current progress value
        /// </summary>
        private float CurrentProgressValue;
        /// <summary>
        /// The progress background width
        /// </summary>
        private int ProgressBackgroundWidth;
        /// <summary>
        /// The maximum progress background width
        /// </summary>
        private const int MaxProgressBackgroundWidth = 5;


        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialLoadingFloatingActionButton"/> class.
        /// </summary>
        public MaterialLoadingFloatingActionButton()
        {

            for (float i = 0; i < 21; i++)
            {
                getAngleInterpolator(i / 20);
            }
            Height = 48;
            Width = 48;
            Elevation = 5;
            ProgressBackgroundPen = new Pen(MaterialSkinManager.Instance.ColorScheme.LightPrimaryBrush);
            ProgressBackgroundPen.Width = 0;
            ProgressPen = new Pen(MaterialSkinManager.Instance.ColorScheme.PrimaryBrush);
            ProgressPen.Width = 0;
            ProgressBackgroundWidth = 0;
            CurrentProgressValue = 0;
            ProgressTimer = new Timer();
            ProgressTimer.Interval = 33;
            ProgressTimer.Tick += ProgressTimer_Tick;

            CurrentProgressPosition = new Point(270, 5);

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
            ShadowBorder = new GraphicsPath();
            ShadowBorder = DrawHelper.CreateCircle(Location.X+5,
                                    Location.Y+5,
                                    ClientRectangle.Width / 2 -6);

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
        /// Handles the Tick event of the ProgressTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void ProgressTimer_Tick(object sender, System.EventArgs e)
        {
            if (_ProgressFinished)
            {
                if (ProgressBackgroundWidth > 0)
                {
                    ProgressBackgroundWidth--;
                }
                else
                {
                    ProgressTimer.Stop();
                }
            }
            else
            {
                if (ProgressBackgroundWidth < MaxProgressBackgroundWidth)
                {
                    ProgressBackgroundWidth++;
                }
                CurrentProgressValue++;
                if (CurrentProgressValue > 20)
                {
                    CurrentProgressValue = 0;
                }
                calculateProgressPositions();
            }
            ProgressBackgroundPen.Width = ProgressBackgroundWidth;
            ProgressPen.Width = ProgressBackgroundWidth;


            Invalidate();
        }

        /// <summary>
        /// Calculates the progress positions.
        /// </summary>
        private void calculateProgressPositions()
        {
            int Angle = 0;
            int Position = 0;

            Angle = getAngleInterpolator(CurrentProgressValue / 20);
            if (Angle > 360)
            {
                Angle -= 360;
            }

            ;
            Position = getPositionInterpolator(CurrentProgressValue / 20);

            if (Position > 360)
            {
                Position -= 360;
            }

            CurrentProgressPosition.X = Position;
            CurrentProgressPosition.Y = Angle;
            if (CurrentProgressValue == 0)
            {

                farbeGetauscht = !farbeGetauscht;
                Color save = ProgressBackgroundPen.Color;
                ProgressBackgroundPen.Color = ProgressPen.Color;
                ProgressPen.Color = save;
            }
        }

        /// <summary>
        /// Gets the position interpolator.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>System.Int32.</returns>
        public int getPositionInterpolator(float t)
        {
            double x = 2.0f * t - 1.0f;
            x = 0.5f * (x * x * x + 1.0f);

            x = x * 360 + 270;
            x = Math.Pow(t, 1.5);
            x = x * 360 + 270;
            return Convert.ToInt32(x);
        }

        /// <summary>
        /// Gets the angle interpolator.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>System.Int32.</returns>
        public int getAngleInterpolator(float t)
        {
            t++;
            double res = t * Math.PI;
            res = Math.Cos(res);
            res = res / 2;
            res += 0.5;

            res = res * 360;

            t--;
            res = 1 - Math.Pow((1 - t), 1.5);
            res = res * 360;
            return Convert.ToInt32(res);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            int ShadowDepth = 5;
            int iCropping = ClientRectangle.Width / 3;
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            ProgressBackgroundPen.Color = SkinManager.Theme == MaterialSkinManager.Themes.LIGHT ? farbeGetauscht ? SkinManager.ColorScheme.PrimaryColor : SkinManager.ColorScheme.LightPrimaryColor : farbeGetauscht ? SkinManager.ColorScheme.PrimaryColor : SkinManager.ColorScheme.DarkPrimaryColor;
            ProgressPen.Color = SkinManager.Theme == MaterialSkinManager.Themes.LIGHT ? farbeGetauscht ? SkinManager.ColorScheme.LightPrimaryColor : SkinManager.ColorScheme.PrimaryColor : farbeGetauscht ? SkinManager.ColorScheme.DarkPrimaryColor : SkinManager.ColorScheme.PrimaryColor;


            g.Clear(Parent.BackColor);
          
            using (var backgroundPath = DrawHelper.CreateCircle(ClientRectangle.X + ShadowDepth,
                    ClientRectangle.Y + ShadowDepth,
                    ClientRectangle.Width / 2 - ShadowDepth))
            {
                g.FillPath(_ProgressFinished ? SkinManager.ColorScheme.DarkPrimaryBrush : SkinManager.ColorScheme.AccentBrush, backgroundPath);
            }

            if (_Icon != null && !_ProgressFinished)
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
                    var rippleSize = (int)(animationValue * Width * 2);
                    g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                }
            }

            if (showProgress)
            {

                if (_ProgressFinished)
                {
                    g.DrawLine(new Pen(SkinManager.ACTION_BAR_TEXT_BRUSH(), 2), new Point(Convert.ToInt32(Width / 2.4), Height / 2), new Point(Width / 2, Convert.ToInt32(Height / 1.8)));
                    g.DrawLine(new Pen(SkinManager.ACTION_BAR_TEXT_BRUSH(), 2), new Point(Convert.ToInt32(Width / 1.6), Convert.ToInt32(Height / 2.8)), new Point(Width / 2, Convert.ToInt32(Height / 1.8)));
                    Region = new Region(DrawHelper.CreateCircle(ClientRectangle.X + ShadowDepth,
                                ClientRectangle.Y + ShadowDepth,
                                ClientRectangle.Width / 2 - ShadowDepth));
                }
                else
                {
                    g.DrawArc(ProgressBackgroundPen, ShadowDepth - 1, ShadowDepth - 1, Width - (ShadowDepth * 2), Height - (ShadowDepth * 2), 0, 360);
                    g.DrawArc(ProgressPen, ShadowDepth - 1, ShadowDepth - 1, Width - (ShadowDepth * 2), Height - (ShadowDepth * 2), CurrentProgressPosition.X, CurrentProgressPosition.Y);
                    Region = new Region(DrawHelper.CreateCircle(ClientRectangle.X + 1,
                                  ClientRectangle.Y + 1,
                                  ClientRectangle.Width / 2 - 1));
                }
            }
            else
            {
                Region = new Region(DrawHelper.CreateCircle(ClientRectangle.X + ShadowDepth,
                                   ClientRectangle.Y + ShadowDepth,
                                   ClientRectangle.Width / 2 - ShadowDepth));
            }

        }

        /// <summary>
        /// Starts the progress animation.
        /// </summary>
        public void startProgressAnimation()
        {
            showProgress = true;
            ProgressTimer.Start();
        }

        /// <summary>
        /// Resets the progress animation.
        /// </summary>
        public void resetProgressAnimation()
        {
            ProgressTimer.Stop();
            showProgress = false;
            _ProgressFinished = false;
            Invalidate();

        }

        /// <summary>
        /// Progresses the finished.
        /// </summary>
        public void ProgressFinished()
        {
            _ProgressFinished = true;
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

