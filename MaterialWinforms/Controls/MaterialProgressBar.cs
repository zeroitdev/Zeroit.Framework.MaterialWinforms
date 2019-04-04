// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialProgressBar.cs" company="Zeroit Dev Technlologies">
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
using System.Windows.Forms;
using System.Drawing;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Material design-like progress bar
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public class MaterialProgressBar : Control, IMaterialControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialProgressBar" /> class.
        /// </summary>
        public MaterialProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Maximum = 100;
            AnimationManagerStart = new Animations.AnimationManager();
            AnimationManagerStart.AnimationType = Animations.AnimationType.EaseInOut;
            AnimationManagerStart.Increment = 0.025;
            AnimationManagerStart.SetProgress(0);
            AnimationManagerStart.OnAnimationFinished += AnimationManager_OnAnimationFinished;
            AnimationManagerStart.OnAnimationProgress += AnimationManager_OnAnimationProgress;
            AnimationManagerEnd = new Animations.AnimationManager();
            AnimationManagerEnd.AnimationType = Animations.AnimationType.EaseInOut;
            AnimationManagerEnd.Increment = 0.025;
            AnimationManagerEnd.OnAnimationProgress += AnimationManager_OnAnimationProgress;
        }


        /// <summary>
        /// Animations the manager on animation progress.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void AnimationManager_OnAnimationProgress(object sender)
        {
            if (AnimationManagerEnd.GetProgress() >= 0.4 && !AnimationManagerStart.IsAnimating())
            {
                AnimationManagerStart.StartNewAnimation(Animations.AnimationDirection.In);
            }
            Invalidate();
        }

        /// <summary>
        /// Animations the manager on animation finished.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void AnimationManager_OnAnimationFinished(object sender)
        {
            Invalidate();
            if (AnimationManagerStart.Increment == 0.02)
            {
                AnimationManagerStart.Increment = 0.025;
                AnimationManagerEnd.Increment = 0.025;
            }
            else
            {
                AnimationManagerStart.Increment = 0.02;
                AnimationManagerEnd.Increment = 0.02;
            }

            AnimationManagerStart.SetProgress(0);
            AnimationManagerEnd.SetProgress(0);
            AnimationManagerEnd.StartNewAnimation(Animations.AnimationDirection.In);
        }

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
        public MaterialSkinManager SkinManager
        {
            get { return MaterialSkinManager.Instance; }
        }

        /// <summary>
        /// Gets or sets the state of the mouse.
        /// </summary>
        /// <value>The state of the mouse.</value>
        [Browsable(false)]
        public MouseState MouseState { get; set; }


        /// <summary>
        /// Sets the Orientation of The Progessbar
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get { return _Orientation; }
            set
            {
                if (value != _Orientation)
                {
                    _Orientation = value;

                    int tmp = _Orientation == System.Windows.Forms.Orientation.Horizontal ? Height : Width;

                    SetBoundsCore(Location.X, Location.Y, Height, Width, BoundsSpecified.All);

                    if (_Orientation == System.Windows.Forms.Orientation.Horizontal)
                    {
                        Width = tmp;
                    }
                    else
                    {
                        Height = tmp;
                    }

                }
            }
        }
        /// <summary>
        /// The orientation
        /// </summary>
        private Orientation _Orientation = Orientation.Horizontal;

        /// <summary>
        /// Inverts the Progressbar
        /// </summary>
        /// <value><c>true</c> if [inverted progress bar]; otherwise, <c>false</c>.</value>
        public bool InvertedProgressBar { get { return _InvertedProgressBar; } set { _InvertedProgressBar = value; Invalidate(); } }
        /// <summary>
        /// The inverted progress bar
        /// </summary>
        private bool _InvertedProgressBar;

        /// <summary>
        /// Enum ProgressStyle
        /// </summary>
        public enum ProgressStyle
        {
            /// <summary>
            /// The determinate
            /// </summary>
            Determinate,
            /// <summary>
            /// The indeterminate
            /// </summary>
            Indeterminate
        }

        /// <summary>
        /// The animation manager start
        /// </summary>
        private Animations.AnimationManager AnimationManagerStart, AnimationManagerEnd;

        /// <summary>
        /// Gets or sets the maxium Progress Value
        /// </summary>
        /// <value>The maximum.</value>
        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                if (value <= Minimum)
                {
                    _Maximum = Minimum + 1;
                }
                else
                {
                    _Maximum = value;
                }
            }
        }
        /// <summary>
        /// The maximum
        /// </summary>
        private int _Maximum;

        /// <summary>
        /// Gets or sets the Minimum Progress Value;
        /// </summary>
        /// <value>The minimum.</value>
        public int Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                if (value >= Maximum)
                {
                    _Minimum = Maximum - 1;
                }
                else
                {
                    _Minimum = value;
                }
            }
        }
        /// <summary>
        /// The minimum
        /// </summary>
        private int _Minimum;
        /// <summary>
        /// Gets or Sets The Amount of Progress on Step consists off
        /// </summary>
        /// <value>The step.</value>
        public int Step
        {
            get
            {
                return _Step;
            }
            set
            {
                if (value >= Maximum-Minimum )
                {
                    _Step = Maximum - Minimum;
                }
                else if(value <= Minimum-Maximum)
                {
                    _Step = Minimum - Maximum;
                }
                else
                {
                    _Step = value;
                }
            }
        }
        /// <summary>
        /// The step
        /// </summary>
        private int _Step;

        /// <summary>
        /// Gets or sets the Current Value
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                if (_Value > Maximum)
                {
                    _Value = Maximum;
                }
                else if (_Value < Minimum)
                {
                    _Value = Minimum;
                }
            }
        }
        /// <summary>
        /// The value
        /// </summary>
        private int _Value;

        /// <summary>
        /// The style
        /// </summary>
        private ProgressStyle _Style;
        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>The style.</value>
        public ProgressStyle Style
        {
            get
            {
                return _Style;
            }
            set
            {
                if (_Style == ProgressStyle.Determinate && value == ProgressStyle.Indeterminate)
                {
                    _Style = value;
                    AnimationManagerEnd.StartNewAnimation(Animations.AnimationDirection.In);
                }
                else if (_Style == ProgressStyle.Indeterminate && value == ProgressStyle.Determinate)
                {
                    _Style = value;
                    if (AnimationManagerEnd.IsAnimating() || AnimationManagerStart.IsAnimating())
                    {
                        AnimationManagerEnd.SetProgress(1);
                        AnimationManagerStart.SetProgress(1);
                    }
                }
            }
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
            if (Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                base.SetBoundsCore(x, y, width, 5, specified);
            }
            else
            {
                base.SetBoundsCore(x, y, 5, height, specified);
            }

        }

        /// <summary>
        /// Performs the step.
        /// </summary>
        public void PerformStep()
        {
            Value += Step;
            Invalidate();
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new Color BackColor { get { return SkinManager.GetDisabledOrHintColor(); } }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_Style == ProgressStyle.Determinate)
            {
                if (Orientation == System.Windows.Forms.Orientation.Horizontal)
                {
                    var doneProgress = (int)(e.ClipRectangle.Width * ((double)Value / (Maximum-Minimum)));
                    if (InvertedProgressBar)
                    {
                        doneProgress = Width - doneProgress;
                        e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), 0, 0, doneProgress, e.ClipRectangle.Height);
                        e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, doneProgress, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, 0, 0, doneProgress, e.ClipRectangle.Height);
                        e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), doneProgress, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
                    }
                }
                else
                {
                    var doneProgress = (int)(e.ClipRectangle.Height * ((double)Value / Maximum));
                    if (InvertedProgressBar)
                    {
                        doneProgress = Height - doneProgress;

                        e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), 0, 0, e.ClipRectangle.Width, doneProgress);
                        e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, 0, doneProgress, e.ClipRectangle.Height, e.ClipRectangle.Height);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, 0, 0, e.ClipRectangle.Height, doneProgress);
                        e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), 0, doneProgress, e.ClipRectangle.Width, e.ClipRectangle.Height);
                    }
                }
            }
            else
            {
                double startProgress = AnimationManagerStart.GetProgress();
                double EndProgress = AnimationManagerEnd.GetProgress();
                if (Orientation == System.Windows.Forms.Orientation.Horizontal)
                {
                    var doneLocation = (int)(e.ClipRectangle.Width * EndProgress);
                    var StartLocation = (int)(e.ClipRectangle.Width * startProgress);

                    if (InvertedProgressBar)
                    {
                        doneLocation = Width - doneLocation;
                        StartLocation = Width - StartLocation;
                        e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
                        e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, doneLocation, 0, StartLocation - doneLocation, e.ClipRectangle.Height);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
                        e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, StartLocation, 0, doneLocation - StartLocation, e.ClipRectangle.Height);

                    }
                }
                else
                {
                    var doneLocation = (int)(e.ClipRectangle.Height * EndProgress);
                    var StartLocation = (int)(e.ClipRectangle.Height * startProgress);

                    if (InvertedProgressBar)
                    {
                        doneLocation = Height - doneLocation;
                        StartLocation = Height - StartLocation;

                        e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
                        e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, 0, doneLocation, e.ClipRectangle.Height, StartLocation - doneLocation);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
                        e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, 0, StartLocation, e.ClipRectangle.Width, doneLocation - StartLocation);
                    }
                }
            }
        }
    }
}
