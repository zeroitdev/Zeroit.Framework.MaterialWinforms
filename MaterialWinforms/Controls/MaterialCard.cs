// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialCard.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialCard.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Panel" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IShadowedMaterialControl" />
    public class MaterialCard : Panel, IShadowedMaterialControl
    {

        /// <summary>
        /// The text
        /// </summary>
        private string _Text;
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
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public Color BackColor { get { return SkinManager.GetCardsColor(); } }

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
        /// The large title
        /// </summary>
        private bool _LargeTitle;
        /// <summary>
        /// Gets or sets a value indicating whether [large title].
        /// </summary>
        /// <value><c>true</c> if [large title]; otherwise, <c>false</c>.</value>
        public bool LargeTitle
        {
            get
            {
                return _LargeTitle;
            }
            set
            {
                _LargeTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Category("Appearance")]
        public string Title
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the size of the title.
        /// </summary>
        /// <value>The size of the title.</value>
        public SizeF TitleSize { get { return CreateGraphics().MeasureString(_Text, LargeTitle ? new FontManager().Roboto_Medium15 : SkinManager.ROBOTO_MEDIUM_10); } }
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialCard"/> class.
        /// </summary>
        public MaterialCard()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Height = 1;
            Padding = new Padding(5, 25, 5, 5);
            Elevation = 5;
            SizeChanged += Redraw;
            LocationChanged += Redraw;
            DoubleBuffered = true;
            ParentChanged += new System.EventHandler(Redraw);
        }


        /// <summary>
        /// Redraws the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Redraw(object sender, System.EventArgs e)
        {
            ShadowBorder = new GraphicsPath();
            ShadowBorder = DrawHelper.CreateRoundRect(Location.X,
                                    Location.Y,
                                    ClientRectangle.Width, ClientRectangle.Height, 10);
            this.Region = new Region(DrawHelper.CreateRoundRect(ClientRectangle.X,
                                    ClientRectangle.Y,
                                    ClientRectangle.Width, ClientRectangle.Height, 10));
            Invalidate();


        }

        /// <summary>
        /// Handles the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="pevent">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            int iCropping = ClientRectangle.Width / 3;
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(SkinManager.GetCardsColor());

            if (!string.IsNullOrWhiteSpace(_Text))
            {
                g.DrawString(
               _Text,
               LargeTitle?new FontManager().Roboto_Medium15: SkinManager.ROBOTO_MEDIUM_10,
               SkinManager.ColorScheme.PrimaryBrush,
               new Rectangle(ClientRectangle.X + 10, ClientRectangle.Y + 10, ClientRectangle.Width, (int)TitleSize.Height),
               new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            }
            g.ResetClip();
        }
    }
}
