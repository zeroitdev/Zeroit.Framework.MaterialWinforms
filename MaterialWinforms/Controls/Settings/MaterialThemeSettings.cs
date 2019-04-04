// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialThemeSettings.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Zeroit.Framework.MaterialWinforms.Controls.Settings
{
    /// <summary>
    /// Class MaterialThemeSettings.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.Controls.MaterialUserControl" />
    public partial class MaterialThemeSettings : MaterialUserControl
    {
        /// <summary>
        /// The base form
        /// </summary>
        private MaterialForm _BaseForm;
        /// <summary>
        /// The parent
        /// </summary>
        private MaterialSettings _Parent;
        /// <summary>
        /// The ignore
        /// </summary>
        private bool Ignore;
        /// <summary>
        /// The presets
        /// </summary>
        private ColorSchemePresetCollection Presets;
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialThemeSettings"/> class.
        /// </summary>
        /// <param name="pBaseForm">The p base form.</param>
        /// <param name="pSettings">The p settings.</param>
        public MaterialThemeSettings(MaterialForm pBaseForm, MaterialSettings pSettings)
        {
            InitializeComponent();
            _Parent = pSettings;
            _BaseForm = pBaseForm;
            tgl_Theme.Checked = SkinManager.Theme == MaterialSkinManager.Themes.DARK;
            Ignore = tgl_Theme.Checked;
            foreach (ColorSchemePreset objPrest in SkinManager.ColorSchemes.List())
            {
                ThemePreview objPreview = new ThemePreview(objPrest);
                objPreview.Click += objPreview_Click;
                flowLayoutPanel1.Controls.Add(objPreview);
            }
            Bitmap bmp = new Bitmap(materialFloatingActionButton1.Width, materialFloatingActionButton1.Height);
            Graphics g = Graphics.FromImage(bmp);
            Pen p = new Pen(Brushes.White, 6);
            g.DrawLine(p, new Point(0, bmp.Height / 2), new Point(bmp.Width, bmp.Height / 2));
            g.DrawLine(p, new Point(bmp.Width / 2, 0), new Point(bmp.Width / 2, bmp.Height));
            materialFloatingActionButton1.Icon = bmp;
        }

        /// <summary>
        /// Handles the Click event of the objPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void objPreview_Click(object sender, EventArgs e)
        {
            ThemePreview objPreview =(ThemePreview) sender;

            Point OverlayOrigin = new Point();
            OverlayOrigin = Cursor.Position;
            ColorOverlay objOverlay = new ColorOverlay(OverlayOrigin,objPreview.getColorSchemePreset(), _BaseForm,_Parent);
            objOverlay.FormClosed += objOverlay_FormClosed;
            objOverlay.Show();

        }


        /// <summary>
        /// Handles the FormClosed event of the objOverlay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        void objOverlay_FormClosed(object sender, FormClosedEventArgs e)
        {
            tgl_Theme.Focus();
        }

        /// <summary>
        /// TGLs the theme on animation finished.
        /// </summary>
        private void tgl_Theme_onAnimationFinished()
        {
            if (Ignore)
            {
                Ignore = false;
                return;
            }
            Point OverlayOrigin = new Point();
            OverlayOrigin.X = tgl_Theme.Checked ? tgl_Theme.Right - tgl_Theme.Height / 2 : tgl_Theme.Left + tgl_Theme.Height / 2;
            OverlayOrigin.Y = tgl_Theme.Location.Y + tgl_Theme.Height / 3;
            ColorOverlay objOverlay = new ColorOverlay(PointToScreen(OverlayOrigin), (tgl_Theme.Checked ? MaterialSkinManager.Themes.DARK : MaterialSkinManager.Themes.LIGHT), _BaseForm,_Parent);
            objOverlay.FormClosed += objOverlay_FormClosed;
            objOverlay.Show();
        }

        /// <summary>
        /// Handles the Click event of the materialFloatingActionButton1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void materialFloatingActionButton1_Click(object sender, EventArgs e)
        {
            Point OverlayOrigin = new Point();
            OverlayOrigin = Cursor.Position;
            SchemeCreator objScheme = new SchemeCreator(OverlayOrigin, _BaseForm);
            objScheme.FormClosed += newColorScheme;
            objScheme.Show();
        }

        /// <summary>
        /// News the color scheme.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        private void newColorScheme(object sender, FormClosedEventArgs e)
        
        {
           
            flowLayoutPanel1.Controls.Clear();
            foreach (ColorSchemePreset objPrest in SkinManager.ColorSchemes.List())
            {
                ThemePreview objPreview = new ThemePreview(objPrest);
                objPreview.Click += objPreview_Click;
                flowLayoutPanel1.Controls.Add(objPreview);
            }
        }
    }

    /// <summary>
    /// Class ThemePreview.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    class ThemePreview : Control,IMaterialControl
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
        /// The preview preset
        /// </summary>
        private ColorSchemePreset PreviewPreset;
        /// <summary>
        /// The top dark
        /// </summary>
        private Rectangle TopDark,TopDefault,Fab;
        /// <summary>
        /// The primary dark
        /// </summary>
        private SolidBrush PrimaryDark, Primary, Accent,Text;
        /// <summary>
        /// Initializes a new instance of the <see cref="ThemePreview"/> class.
        /// </summary>
        /// <param name="SchemeToPreview">The scheme to preview.</param>
        public ThemePreview(ColorSchemePreset SchemeToPreview)
        {
            PreviewPreset = SchemeToPreview;

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Size = new Size(200, 110);
            TopDark = new Rectangle(0, 0, 200, 20);
            TopDefault = new Rectangle(0, TopDark.Bottom, 200,60);
            Fab = new Rectangle(Width - 60, TopDefault.Bottom - 20, 40, 40);
            PrimaryDark = new SolidBrush(((int)PreviewPreset.DarkPrimaryColor).ToColor());
            Primary = new SolidBrush(((int)PreviewPreset.PrimaryColor).ToColor());
            Accent = new SolidBrush(((int)PreviewPreset.AccentColor).ToColor());
            Text = new SolidBrush(((int)PreviewPreset.TextShade).ToColor());
        }

        /// <summary>
        /// Gets the color scheme preset.
        /// </summary>
        /// <returns>ColorSchemePreset.</returns>
        public ColorSchemePreset getColorSchemePreset()
        {
            return PreviewPreset;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            GraphicsPath objPath = new GraphicsPath();
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.Clear(SkinManager.GetApplicationBackgroundColor());
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.FillRectangle(PrimaryDark, TopDark);
            g.FillRectangle(Primary, TopDefault);
            DrawHelper.drawShadow(g, DrawHelper.CreateCircle(Fab.X-1, Fab.Y-1, 20), 2, Color.Black);
            g.FillEllipse(Accent, Fab);

            g.DrawString(
                PreviewPreset.Name,
                 SkinManager.ROBOTO_REGULAR_11,
                 Text, TopDefault);
            g.ResetClip();
            
        }

        
    }

}
