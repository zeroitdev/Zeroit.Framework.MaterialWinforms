// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialFlowLayoutPanel.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

using System.Runtime.InteropServices;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;

namespace Zeroit.Framework.MaterialWinforms.Controls
{

    /// <summary>
    /// Class MaterialFlowLayoutPanel.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IShadowedMaterialControl" />
    [Designer(typeof(ParentControlDesigner))]
    public class MaterialFlowLayoutPanel : Control, IShadowedMaterialControl
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
        /// Gets or sets the automatic scroll.
        /// </summary>
        /// <value>The automatic scroll.</value>
        public Boolean AutoScroll
        {
            get
            {
                return MainPanel.AutoScroll;
            }
            set
            {
                MainPanel.AutoScroll = value;
                VerticalScrollbar.Visible = MainPanel.VerticalScroll.Visible;
                VerticalScrollbarAdded = VerticalScrollbar.Visible;
                HorizontalScrollbar.Visible = MainPanel.HorizontalScroll.Visible;
                HorizontalScrollbarAdded = HorizontalScrollbar.Visible;
            }
        }

        /// <summary>
        /// This property is not relevant for this class.
        /// </summary>
        /// <value><c>true</c> if [automatic size]; otherwise, <c>false</c>.</value>
        public new bool AutoSize
        {
            get
            {
                return MainPanel.AutoSize;
            }

            set
            {
                MainPanel.AutoSize = value;
                base.AutoSize = value;
            }
        }

        /// <summary>
        /// The vertical scrollbar
        /// </summary>
        private MaterialScrollBar VerticalScrollbar, HorizontalScrollbar;
        /// <summary>
        /// The vertical scrollbar added
        /// </summary>
        private Boolean VerticalScrollbarAdded, HorizontalScrollbarAdded;
        /// <summary>
        /// The main panel
        /// </summary>
        private MaterialFlowLayoutDisplayingPanel MainPanel;

        /// <summary>
        /// The ignore resize
        /// </summary>
        private bool ignoreResize = true;
        /// <summary>
        /// The ignore main panel resize
        /// </summary>
        private bool ignoreMainPanelResize = false;
        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public override Color BackColor { get { return SkinManager.GetCardsColor(); } }

        /// <summary>
        /// Gets the collection of controls contained within the control.
        /// </summary>
        /// <value>The controls.</value>
        public new ControlCollection Controls
        {
            get
            {
                return MainPanel.Controls;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialFlowLayoutPanel"/> class.
        /// </summary>
        public MaterialFlowLayoutPanel() : base()
        {
            
            DoubleBuffered = true;
            VerticalScrollbar = new MaterialScrollBar(MaterialScrollOrientation.Vertical);
            VerticalScrollbar.Scroll += Scrolled;
            VerticalScrollbar.Visible = false;
            VerticalScrollbarAdded = false;

            HorizontalScrollbar = new MaterialScrollBar(MaterialScrollOrientation.Horizontal);
            HorizontalScrollbar.Scroll += Scrolled;
            HorizontalScrollbar.Visible = false;
            HorizontalScrollbarAdded = false;

            MainPanel = new MaterialFlowLayoutDisplayingPanel();
            MainPanel.Resize += MainPanel_Resize;
            MainPanel.Location = new Point(0, 0);

            Size = new Size(90, 90);

            base.Controls.Add(MainPanel);
            base.Controls.Add(VerticalScrollbar);
            base.Controls.Add(HorizontalScrollbar);
            MainPanel.ControlAdded += MaterialPanel_ControlsChanged;
            MainPanel.ControlRemoved += MaterialPanel_ControlsChanged;
            MainPanel.onScrollBarChanged += MainPanel_onScrollBarChanged;
            AutoScroll = true;

            ignoreResize = false;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.ControlAdded" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs" /> that contains the event data.</param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);


            VerticalScrollbar.BringToFront();
            HorizontalScrollbar.BringToFront();
            
            MainPanel.BringToFront();
        }

        /// <summary>
        /// Mains the panel on scroll bar changed.
        /// </summary>
        /// <param name="pScrollOrientation">The p scroll orientation.</param>
        /// <param name="pVisible">if set to <c>true</c> [p visible].</param>
        void MainPanel_onScrollBarChanged(Orientation pScrollOrientation, bool pVisible)
        {
            UpdateScrollbars();
        }

        /// <summary>
        /// Scrolleds the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ScrollEventArgs"/> instance containing the event data.</param>
        void Scrolled(object sender, ScrollEventArgs e)
        {
            MainPanel.AutoScrollPosition = new Point(HorizontalScrollbar.Value, VerticalScrollbar.Value);
        }

        /// <summary>
        /// Handles the ControlsChanged event of the MaterialPanel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ControlEventArgs"/> instance containing the event data.</param>
        void MaterialPanel_ControlsChanged(object sender, ControlEventArgs e)
        {
            UpdateScrollbars();
            MainPanel.BringToFront();
            VerticalScrollbar.BringToFront();
            HorizontalScrollbar.BringToFront();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.
        /// </summary>
        /// <param name="levent">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data.</param>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            MainPanel.BringToFront();
            VerticalScrollbar.BringToFront();
            HorizontalScrollbar.BringToFront();
        }

        /// <summary>
        /// Handles the Resize event of the MainPanel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void MainPanel_Resize(object sender, EventArgs e)
        {
            if (!ignoreMainPanelResize)
                UpdateScrollbars();
            else
                ignoreMainPanelResize = false;
        }


        /// <summary>
        /// Handles the <see cref="E:Resize" /> event.
        /// </summary>
        /// <param name="eventargs">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnResize(EventArgs eventargs)
        {
            VerticalScrollbar.Location = new Point(Width - VerticalScrollbar.Width, 0);
            VerticalScrollbar.Size = new Size(VerticalScrollbar.Width, Height - HorizontalScrollbar.Height);
            VerticalScrollbar.Anchor = ((AnchorStyles)AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);
            HorizontalScrollbar.Location = new Point(0, Height - HorizontalScrollbar.Height);
            HorizontalScrollbar.Size = new Size(Width - VerticalScrollbar.Width, HorizontalScrollbar.Height);
            HorizontalScrollbar.Anchor = ((AnchorStyles)AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
            ShadowBorder = new GraphicsPath();
            ShadowBorder.AddRectangle(new Rectangle(Location,Size));
            base.OnResize(eventargs);
            UpdateScrollbars();

        }


        /// <summary>
        /// Updates the scrollbars.
        /// </summary>
        private void UpdateScrollbars()
        {
            if(ignoreResize)
            {
                return;
            }
            VerticalScrollbar.Minimum = MainPanel.VerticalScroll.Minimum;
            VerticalScrollbar.Maximum = MainPanel.VerticalScroll.Maximum;
            VerticalScrollbar.LargeChange = MainPanel.VerticalScroll.LargeChange;
            VerticalScrollbar.SmallChange = MainPanel.VerticalScroll.SmallChange;

            HorizontalScrollbar.Minimum = MainPanel.HorizontalScroll.Minimum;
            HorizontalScrollbar.Maximum = MainPanel.HorizontalScroll.Maximum;
            HorizontalScrollbar.LargeChange = MainPanel.HorizontalScroll.LargeChange;
            HorizontalScrollbar.SmallChange = MainPanel.HorizontalScroll.SmallChange;

            if (MainPanel.VerticalScroll.Visible && !VerticalScrollbarAdded)
            {
                VerticalScrollbarAdded = true;
                VerticalScrollbar.Visible = true;
            }
            else if (!MainPanel.VerticalScroll.Visible && VerticalScrollbarAdded)
            {
                VerticalScrollbarAdded = false;
                VerticalScrollbar.Visible = false;
            }
            if (MainPanel.HorizontalScroll.Visible && !HorizontalScrollbarAdded)
            {
                HorizontalScrollbarAdded = true;
                HorizontalScrollbar.Visible = true;
            }
            else if (!MainPanel.HorizontalScroll.Visible && HorizontalScrollbarAdded)
            {
                HorizontalScrollbarAdded = false;
                HorizontalScrollbar.Visible = false;
            }
            ignoreMainPanelResize = true;


            Size MainPanelSize = new Size(Width - (VerticalScrollbarAdded ? VerticalScrollbar.Width : 0), Height - (HorizontalScrollbarAdded ? HorizontalScrollbar.Height : 0));

            MainPanel.IgnoreResizing = true;
            ignoreMainPanelResize = true;
            MainPanel.Size = new Size(Width - (VerticalScrollbarAdded ? VerticalScrollbar.Width : 0), Height - (HorizontalScrollbarAdded ? HorizontalScrollbar.Height : 0));
            MainPanel.IgnoreResizing = false;
            ignoreMainPanelResize = false;
        }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

        }



        /// <summary>
        /// Gets or sets the shadow border.
        /// </summary>
        /// <value>The shadow border.</value>
        [Browsable(false)]
        public GraphicsPath ShadowBorder { get; set; }

        /// <summary>
        /// Gets or sets the elevation.
        /// </summary>
        /// <value>The elevation.</value>
        public int Elevation { get; set; }

    }


    /// <summary>
    /// Class MaterialFlowLayoutDisplayingPanel.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.FlowLayoutPanel" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    internal class MaterialFlowLayoutDisplayingPanel : FlowLayoutPanel, IMaterialControl
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
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public override Color BackColor { get { return SkinManager.GetApplicationBackgroundColor(); } }

        /// <summary>
        /// Delegate ScrollbarChanged
        /// </summary>
        /// <param name="pScrollOrientation">The p scroll orientation.</param>
        /// <param name="pVisible">The p visible.</param>
        public delegate void ScrollbarChanged(Orientation pScrollOrientation, Boolean pVisible);

        /// <summary>
        /// The ignore resizing
        /// </summary>
        public bool IgnoreResizing = false;

        /// <summary>
        /// Occurs when [on scroll bar changed].
        /// </summary>
        public event ScrollbarChanged onScrollBarChanged;
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialFlowLayoutDisplayingPanel"/> class.
        /// </summary>
        public MaterialFlowLayoutDisplayingPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Padding = new Padding(3, 3, 3, 3);
        }

        /// <summary>
        /// Shows the scroll bar.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="wBar">The w bar.</param>
        /// <param name="bShow">if set to <c>true</c> [b show].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        /// <summary>
        /// Enum ScrollBarDirection
        /// </summary>
        private enum ScrollBarDirection
        {
            /// <summary>
            /// The sb horz
            /// </summary>
            SB_HORZ = 0,
            /// <summary>
            /// The sb vert
            /// </summary>
            SB_VERT = 1,
            /// <summary>
            /// The sb control
            /// </summary>
            SB_CTL = 2,
            /// <summary>
            /// The sb both
            /// </summary>
            SB_BOTH = 3
        }

        /// <summary>
        /// Fires the event indicating that the panel has been resized. Inheriting controls should use this in favor of actually listening to the event, but should still call base.onResize to ensure that the event is fired for external listeners.
        /// </summary>
        /// <param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (onScrollBarChanged != null)
            {
                onScrollBarChanged(Orientation.Horizontal, HorizontalScroll.Visible);
                onScrollBarChanged(Orientation.Vertical, VerticalScroll.Visible);
            }
        }

        /// <summary>
        /// WNDs the proc.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (onScrollBarChanged != null && ! IgnoreResizing)
            {
                onScrollBarChanged(Orientation.Horizontal, HorizontalScroll.Visible);
                onScrollBarChanged(Orientation.Vertical, VerticalScroll.Visible);
            }
            ShowScrollBar(this.Handle, (int)ScrollBarDirection.SB_HORZ, false);
            ShowScrollBar(this.Handle, (int)ScrollBarDirection.SB_VERT, false);
            base.WndProc(ref m);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            foreach (Control objChild in Controls)
            {
                if (typeof(IShadowedMaterialControl).IsAssignableFrom(objChild.GetType()))
                {
                    IShadowedMaterialControl objCurrent = (IShadowedMaterialControl)objChild;
                    DrawHelper.drawShadow(e.Graphics, objCurrent.ShadowBorder, objCurrent.Elevation, SkinManager.GetApplicationBackgroundColor());
                }

            }
        }
    }
}

