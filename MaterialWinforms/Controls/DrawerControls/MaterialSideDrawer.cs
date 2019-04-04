// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialSideDrawer.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

using System.Runtime.InteropServices;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialSideDrawer.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.FlowLayoutPanel" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IShadowedMaterialControl" />
    public partial class MaterialSideDrawer : FlowLayoutPanel, IShadowedMaterialControl
    {
        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        [Browsable(false)]
        public int Depth { get; set; }

        /// <summary>
        /// Gets or sets the elevation.
        /// </summary>
        /// <value>The elevation.</value>
        [Browsable(false)]
        public int Elevation { get; set; }
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
        /// Gets or sets the shadow border.
        /// </summary>
        /// <value>The shadow border.</value>
        [Browsable(false)]
        public GraphicsPath ShadowBorder { get; set; }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public override Color BackColor { get { return SkinManager.GetApplicationBackgroundColor(); } }

        /// <summary>
        /// The side drawer
        /// </summary>
        private MaterialContextMenuStrip _SideDrawer;

        /// <summary>
        /// Gets or sets a value indicating whether [select on click].
        /// </summary>
        /// <value><c>true</c> if [select on click]; otherwise, <c>false</c>.</value>
        public bool SelectOnClick { get; set; }

        /// <summary>
        /// Delegate HiddenOnStartChanged
        /// </summary>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        public delegate void HiddenOnStartChanged(bool newValue);
        /// <summary>
        /// Occurs when [on hidden on start changed].
        /// </summary>
        public event HiddenOnStartChanged onHiddenOnStartChanged;
        /// <summary>
        /// The hidden on start
        /// </summary>
        private bool _HiddenOnStart;
        /// <summary>
        /// Gets or sets a value indicating whether [hidden on start].
        /// </summary>
        /// <value><c>true</c> if [hidden on start]; otherwise, <c>false</c>.</value>
        public bool HiddenOnStart
        {
            get
            {
                return _HiddenOnStart;
            }
            set
            {
                _HiddenOnStart = value;
                if (onHiddenOnStartChanged != null)
                {
                    onHiddenOnStartChanged(value);
                }
            }
        }

        /// <summary>
        /// The side drawer fixiert
        /// </summary>
        public bool _SideDrawerFixiert;
        /// <summary>
        /// The side drawer unter action bar
        /// </summary>
        public bool _SideDrawerUnterActionBar;
        /// <summary>
        /// Gets or sets a value indicating whether [side drawer fixiert].
        /// </summary>
        /// <value><c>true</c> if [side drawer fixiert]; otherwise, <c>false</c>.</value>
        public bool SideDrawerFixiert
        {
            get
            {
                return _SideDrawerFixiert;
            }
            set
            {
                _SideDrawerFixiert = value;
                if (_SideDrawerFixiert)
                {
                    Size = new Size(MaximumSize.Width, Height);
                }
                else
                {
                    Size = new Size(0, Height);
                }


            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [side drawer unter action bar].
        /// </summary>
        /// <value><c>true</c> if [side drawer unter action bar]; otherwise, <c>false</c>.</value>
        public bool SideDrawerUnterActionBar
        {
            get
            {
                return _SideDrawerUnterActionBar;
            }
            set
            {
                _SideDrawerUnterActionBar = value;
                Location = new Point(0, MaterialActionBar.ACTION_BAR_HEIGHT + MaterialForm.STATUS_BAR_HEIGHT + (_SideDrawerUnterActionBar ? 48 : 0));


            }
        }

        /// <summary>
        /// Redraws this instance.
        /// </summary>
        public void Redraw()
        {
            base.Invalidate();
            foreach (Control objItem in Controls)
            {
                objItem.Invalidate();
            }
        }

        /// <summary>
        /// The hide side drawer
        /// </summary>
        private bool _HideSideDrawer;
        /// <summary>
        /// Gets or sets a value indicating whether [hide side drawer].
        /// </summary>
        /// <value><c>true</c> if [hide side drawer]; otherwise, <c>false</c>.</value>
        public bool HideSideDrawer
        {
            get
            {
                return _HideSideDrawer;
            }
            set
            {
                _HideSideDrawer = value;
                Visible = !value;


            }
        }

        /// <summary>
        /// Gets or sets the side drawer.
        /// </summary>
        /// <value>The side drawer.</value>
        public MaterialContextMenuStrip SideDrawer
        {
            get
            {
                return _SideDrawer;
            }
            set
            {
                _SideDrawer = value;
                if (_SideDrawer != null)
                {
                    _SideDrawer.ItemAdded += ItemCollectionChanged;
                    _SideDrawer.ItemRemoved += ItemCollectionChanged;
                    initSideDrawer();
                }
            }
        }

        /// <summary>
        /// Delegate SideDrawerEventHandler
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SideDrawerEventArgs"/> instance containing the event data.</param>
        public delegate void SideDrawerEventHandler(object sender, SideDrawerEventArgs e);

        /// <summary>
        /// Class SideDrawerEventArgs.
        /// </summary>
        /// <seealso cref="System.EventArgs" />
        public class SideDrawerEventArgs : EventArgs
        {
            /// <summary>
            /// The clicked item
            /// </summary>
            private String ClickedItem;
            /// <summary>
            /// The tag
            /// </summary>
            private Object Tag;
            /// <summary>
            /// Initializes a new instance of the <see cref="SideDrawerEventArgs"/> class.
            /// </summary>
            /// <param name="pItem">The p item.</param>
            /// <param name="pTag">The p tag.</param>
            public SideDrawerEventArgs(String pItem, Object pTag)
            {
                ClickedItem = pItem;
                Tag = pTag;
            }

            /// <summary>
            /// Gets the clicked item.
            /// </summary>
            /// <returns>String.</returns>
            public String getClickedItem()
            {
                return ClickedItem;
            }

            /// <summary>
            /// Gets the tag.
            /// </summary>
            /// <returns>Object.</returns>
            public Object getTag()
            {
                return Tag;
            }
        }



        /// <summary>
        /// Occurs when [on side drawer item clicked].
        /// </summary>
        public event SideDrawerEventHandler onSideDrawerItemClicked;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialSideDrawer"/> class.
        /// </summary>
        public MaterialSideDrawer()
        {
            InitializeComponent();
            Dock = DockStyle.Left;
            AutoScroll = true;
            Elevation = 10;
            HiddenOnStart = true;
            MinimumSize = new Size(0, MaximumSize.Height);

        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            MaximumSize = new Size(Math.Min(Parent.Width * 80, MaterialActionBar.ACTION_BAR_HEIGHT * 5), 10000);
            Width = _SideDrawerFixiert ? MaximumSize.Width : 0;
        }

        /// <summary>
        /// Fires the event indicating that the panel has been resized. Inheriting controls should use this in favor of actually listening to the event, but should still call base.onResize to ensure that the event is fired for external listeners.
        /// </summary>
        /// <param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs eventargs)
        {
            if (Parent != null)
            {
                Width = _SideDrawerFixiert ? MaximumSize.Width : Width;
            }
            ShadowBorder = new GraphicsPath();
            if (Width == 0)
            {
                Elevation = 0;
            }
            else
            {
                Elevation = 10;
            }

            ShadowBorder.AddLine(new Point(Location.X + Width, Location.Y), new Point(Location.X + Width, Location.Y + Height));

        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(SkinManager.GetApplicationBackgroundColor());
        }

        /// <summary>
        /// Initializes the side drawer.
        /// </summary>
        private void initSideDrawer()
        {
            bool LastControlWasDivider = false;
            if (_SideDrawer != null)
            {

                Controls.Clear();

                foreach (ToolStripItem objMenuItem in _SideDrawer.Items)
                {
                    if (objMenuItem.GetType() == typeof(ToolStripSeparator))
                    {
                        MaterialDivider objDivider = new MaterialDivider();
                        objDivider.Size = new Size(MaximumSize.Width - Margin.Left - Margin.Right - SystemInformation.VerticalScrollBarWidth, 2);
                        Controls.Add(objDivider);
                        LastControlWasDivider = true;
                    }
                    else
                    {
                        bool Verarbeitet = false;

                        if (objMenuItem.GetType() == typeof(MaterialToolStripMenuItem))
                        {
                            MaterialToolStripMenuItem t = (MaterialToolStripMenuItem)objMenuItem;
                            if (t.DropDownItems.Count > 0)
                            {
                                Verarbeitet = true;
                                if (Controls.Count > 0 && !LastControlWasDivider)
                                {
                                    MaterialDivider objTopDivider = new MaterialDivider();
                                    objTopDivider.Size = new Size(MaximumSize.Width - Margin.Left - Margin.Right - SystemInformation.VerticalScrollBarWidth, 2);
                                    Controls.Add(objTopDivider);
                                    LastControlWasDivider = true;
                                }
                                MaterialLabel objLabel = new MaterialLabel();
                                objLabel.Text = objMenuItem.Text;
                                objLabel.Tag = objMenuItem.Tag;
                                objLabel.Margin = new Padding(0);
                                objLabel.Font = SkinManager.ROBOTO_MEDIUM_10;
                                LastControlWasDivider = false;
                                Controls.Add(objLabel);

                                foreach (ToolStripItem objSubMenuItem in t.DropDownItems)
                                {
                                    MaterialDrawerItem objSubItem = new MaterialDrawerItem();
                                    objSubItem.Text = objSubMenuItem.Text;
                                    objSubItem.Tag = objSubMenuItem.Tag;
                                    objSubItem.Enabled = objSubMenuItem.Enabled;
                                    objSubItem.AutoSize = false;
                                    objSubItem.Margin = new Padding(10, 0, 0, 0);
                                    if (objSubMenuItem.GetType() == typeof(MaterialToolStripMenuItem))
                                    {
                                        objSubItem.IconImage = ((MaterialToolStripMenuItem)objSubMenuItem).Image;
                                    }
                                    objSubItem.MouseClick += new MouseEventHandler(DrawerItemClicked);
                                    objSubItem.Size = new Size(MaximumSize.Width - Margin.Left - Margin.Right - SystemInformation.VerticalScrollBarWidth - 10, 40);
                                    objSubItem.MouseClick -= new MouseEventHandler(DrawerItemClicked);
                                    objSubItem.MouseClick += new MouseEventHandler(DrawerItemClicked);

                                    Controls.Add(objSubItem);
                                    LastControlWasDivider = false;
                                    objSubItem.Location = new Point(10, objSubItem.Location.Y);
                                }

                                MaterialDivider objBottomDivider = new MaterialDivider();
                                objBottomDivider.Size = new Size(MaximumSize.Width - Margin.Left - Margin.Right - SystemInformation.VerticalScrollBarWidth, 2);
                                Controls.Add(objBottomDivider);
                                LastControlWasDivider = true;
                            }
                        }
                        if (!Verarbeitet)
                        {
                            MaterialFlatButton objItem = new MaterialFlatButton();
                            objItem.Text = objMenuItem.Text;
                            objItem.Tag = objMenuItem.Tag;
                            objItem.Enabled = objMenuItem.Enabled;
                            objItem.AutoSize = false;
                            objItem.Margin = new Padding(0, 0, 0, 0);
                            objItem.Size = new Size(MaximumSize.Width - Margin.Left - Margin.Right - SystemInformation.VerticalScrollBarWidth, 40);
                            objItem.MouseClick -= new MouseEventHandler(DrawerItemClicked);
                            objItem.MouseClick += new MouseEventHandler(DrawerItemClicked);
                            LastControlWasDivider = false;
                            Controls.Add(objItem);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Selects the item.
        /// </summary>
        /// <param name="pPosition">The p position.</param>
        public void SelectItem(int pPosition)
        {
            if(Controls[pPosition].GetType() == typeof(MaterialFlatButton))
            {
                DrawerItemClicked((MaterialFlatButton)Controls[pPosition],EventArgs.Empty);
            }
        }

        /// <summary>
        /// Items the collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ItemCollectionChanged(object sender, EventArgs e)
        {
            initSideDrawer();
        }

        /// <summary>
        /// Drawers the item clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DrawerItemClicked(object sender, EventArgs e)
        {
            String strText;
            Object objTag;

            if (sender.GetType() == typeof(MaterialDrawerItem))
            {
                MaterialDrawerItem t = (MaterialDrawerItem)sender;
                if (t.Selected)
                {
                    return;
                }
            }
            else
            {
                MaterialFlatButton t = (MaterialFlatButton)sender;
                if (t.Selected)
                {
                    return;
                }
            }

            foreach (Control objSideControl in Controls)
            {
                if (objSideControl.GetType() == typeof(MaterialFlatButton))
                {
                    MaterialFlatButton objItem = (MaterialFlatButton)objSideControl;
                    objItem.Selected = false;
                    objItem.Invalidate();
                }
                else if (objSideControl.GetType() == typeof(MaterialDrawerItem))
                {
                    MaterialDrawerItem t = (MaterialDrawerItem)objSideControl;
                    t.Selected = false;
                    t.Invalidate();
                }
            }

            if (sender.GetType() == typeof(MaterialDrawerItem))
            {
                MaterialDrawerItem t = (MaterialDrawerItem)sender;
                t.Selected = true && SelectOnClick;
                strText = t.Text;
                objTag = t.Tag;
            }
            else
            {
                MaterialFlatButton t = (MaterialFlatButton)sender;
                t.Selected = true && SelectOnClick;
                strText = t.Text;
                objTag = t.Tag;
            }


            if (onSideDrawerItemClicked != null)
            {
                onSideDrawerItemClicked(sender, new SideDrawerEventArgs(strText, objTag));
            }
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
        /// WNDs the proc.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            ShowScrollBar(this.Handle, (int)ScrollBarDirection.SB_HORZ, false);
            base.WndProc(ref m);
        }
    }
}
