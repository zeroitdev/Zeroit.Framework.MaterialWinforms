// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="ColorOverlay.cs" company="Zeroit Dev Technlologies">
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
using Zeroit.Framework.MaterialWinforms.Animations;

namespace Zeroit.Framework.MaterialWinforms.Controls.Settings
{
    /// <summary>
    /// Class ColorOverlay.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class ColorOverlay : Form
    {

        /// <summary>
        /// The object animation manager
        /// </summary>
        private AnimationManager objAnimationManager;
        /// <summary>
        /// The origin
        /// </summary>
        private Point _Origin;
        /// <summary>
        /// The theme to apply
        /// </summary>
        private MaterialSkinManager.Themes _ThemeToApply;
        /// <summary>
        /// The color scheme to apply
        /// </summary>
        private ColorSchemePreset _ColorSchemeToApply;
        /// <summary>
        /// The fill brush
        /// </summary>
        private Brush FillBrush;
        /// <summary>
        /// The base form
        /// </summary>
        private MaterialForm _BaseForm;
        /// <summary>
        /// The apply theme
        /// </summary>
        private bool applyTheme;
        /// <summary>
        /// The original
        /// </summary>
        private Bitmap Original;
        /// <summary>
        /// The final
        /// </summary>
        private Bitmap Final;
        /// <summary>
        /// The settings dialog
        /// </summary>
        private MaterialSettings _SettingsDialog;
        /// <summary>
        /// The style wurde gesetzt
        /// </summary>
        private Boolean _StyleWurdeGesetzt = false;
        /// <summary>
        /// The color scheme pen
        /// </summary>
        private Pen _ColorSchemePen;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorOverlay"/> class.
        /// </summary>
        /// <param name="Origin">The origin.</param>
        /// <param name="Theme">The theme.</param>
        /// <param name="BaseFormToOverlay">The base form to overlay.</param>
        /// <param name="pSettingsDialog">The p settings dialog.</param>
        public ColorOverlay(Point Origin, MaterialSkinManager.Themes Theme, MaterialForm BaseFormToOverlay, MaterialSettings pSettingsDialog)
        {

            _SettingsDialog = pSettingsDialog;
            _BaseForm = BaseFormToOverlay;
            _ThemeToApply = Theme;
            _Origin = Origin;
            applyTheme = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            InitializeComponent();
            objAnimationManager = new AnimationManager()
            {
                Increment = 0.015,
                AnimationType = AnimationType.EaseInOut
            };
            DoubleBuffered = true;
            objAnimationManager.OnAnimationProgress += sender => Invalidate();
            objAnimationManager.OnAnimationFinished += objAnimationManager_OnAnimationFinished;
        }

        /// <summary>
        /// Generates the original bitmap.
        /// </summary>
        private void GenerateOriginalBitmap()
        {
            Original = _SettingsDialog.CreateImage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorOverlay"/> class.
        /// </summary>
        /// <param name="Origin">The origin.</param>
        /// <param name="Theme">The theme.</param>
        /// <param name="BaseFormToOverlay">The base form to overlay.</param>
        /// <param name="pSettingsDialog">The p settings dialog.</param>
        public ColorOverlay(Point Origin, ColorSchemePreset Theme, MaterialForm BaseFormToOverlay, MaterialSettings pSettingsDialog)
        {

            _SettingsDialog = pSettingsDialog;
            _ColorSchemeToApply = Theme;
            _Origin = Origin;
            _BaseForm = BaseFormToOverlay;
            GenerateOriginalBitmap();
            BackgroundImage = Original;
            applyTheme = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            InitializeComponent();
            objAnimationManager = new AnimationManager()
            {
                Increment = 0.015,
                AnimationType = AnimationType.EaseInOut
            };
            DoubleBuffered = true;
            objAnimationManager.OnAnimationProgress += sender => Invalidate();
            objAnimationManager.OnAnimationFinished += objAnimationManager_OnAnimationFinished;
            _ColorSchemePen = new Pen(new SolidBrush(((int)_ColorSchemeToApply.PrimaryColor).ToColor()), 25);
        }

        /// <summary>
        /// Objects the animation manager on animation finished.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void objAnimationManager_OnAnimationFinished(object sender)
        {
            Close();
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Opacity = 1;
            if(Final == null)
            {
                if(!_StyleWurdeGesetzt)
                {
                    if(applyTheme)
                    {
                        MaterialSkinManager.Instance.Theme = _ThemeToApply;
                    }
                    else
                    {
                        MaterialSkinManager.Instance.LoadColorSchemeFromPreset(_ColorSchemeToApply);
                    }
                    _StyleWurdeGesetzt = true;
                    return;
                }
                else
                {
                    Final = _SettingsDialog.CreateImage();
                    FillBrush = new TextureBrush(Final);
                }
                
            }
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle CurrentRect = CalculateCurrentRect();
            e.Graphics.FillEllipse(FillBrush,CurrentRect);

            if(!applyTheme)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawEllipse(_ColorSchemePen, CurrentRect);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            Opacity = 0;
            base.OnLoad(e);
            this.Location = _BaseForm.Location;
            this.Size = _BaseForm.Size;

            GenerateOriginalBitmap();
            BackgroundImage = Original;
            TopMost = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Shown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            objAnimationManager.SetProgress(0);
            _Origin = PointToClient(_Origin); ;
            objAnimationManager.StartNewAnimation(AnimationDirection.In);
        }

        /// <summary>
        /// Calculates the current rect.
        /// </summary>
        /// <returns>Rectangle.</returns>
        private Rectangle CalculateCurrentRect()
        {
            Rectangle objResult = new Rectangle();

            double xEdge = (Width / 2 >= _Origin.X ? Width : 0);
            double YEdge = (Height / 2 >= _Origin.Y ? Height : 0);


            double radiusMax = Math.Sqrt(Math.Pow(_Origin.X - xEdge, 2) + Math.Pow(_Origin.Y - YEdge, 2));
            radiusMax *= 2;
            double radius = radiusMax * objAnimationManager.GetProgress();
            double top = _Origin.Y - (radius / 2);
            double Left = _Origin.X - (radius / 2);

            objResult.Location = new Point((int)Left, (int)top);
            objResult.Size = new Size((int)radius, (int)radius);

            return objResult;
        }

    }
}
