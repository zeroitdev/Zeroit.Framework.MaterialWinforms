// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialListView.cs" company="Zeroit Dev Technlologies">
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
using System.Windows.Forms;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialListView.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ListView" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public class MaterialListView : ListView, IMaterialControl
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
        /// Gets or sets the background color.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : typeof(IShadowedMaterialControl).IsAssignableFrom(Parent.GetType()) ? ((IMaterialControl)Parent).BackColor : Parent.BackColor; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialListView"/> class.
        /// </summary>
        public MaterialListView()
		{
			GridLines = false;
			FullRowSelect = true;
			HeaderStyle = ColumnHeaderStyle.Nonclickable;
			View = View.Details;
			OwnerDraw = true;
			ResizeRedraw = true;
			BorderStyle = BorderStyle.None;
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

			//Fix for hovers, by default it doesn't redraw
			//TODO: should only redraw when the hovered line changed, this to reduce unnecessary redraws
			MouseLocation = new Point(-1, -1);
			MouseState = MouseState.OUT;
			MouseEnter += delegate
			{
				MouseState = MouseState.HOVER;
			}; 
			MouseLeave += delegate
			{
				MouseState = MouseState.OUT; 
				MouseLocation = new Point(-1, -1);
				Invalidate();
			};
			MouseDown += delegate { MouseState = MouseState.DOWN; };
			MouseUp += delegate{ MouseState = MouseState.HOVER; };
			MouseMove += delegate(object sender, MouseEventArgs args)
			{
				MouseLocation = args.Location;
				Invalidate();
			};
		}

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListView.DrawColumnHeader" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawListViewColumnHeaderEventArgs" /> that contains the event data.</param>
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			e.Graphics.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), new Rectangle(e.Bounds.X, e.Bounds.Y, Width, e.Bounds.Height));
			e.Graphics.DrawString(e.Header.Text, 
				SkinManager.ROBOTO_MEDIUM_10, 
				SkinManager.GetSecondaryTextBrush(),
				new Rectangle(e.Bounds.X + ITEM_PADDING, e.Bounds.Y + ITEM_PADDING, e.Bounds.Width - ITEM_PADDING * 2, e.Bounds.Height - ITEM_PADDING * 2), 
				getStringFormat());
		}

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains information about the control to paint.</param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

            pevent.Graphics.Clear(SkinManager.GetApplicationBackgroundColor());
        }

        /// <summary>
        /// The item padding
        /// </summary>
        private const int ITEM_PADDING = 12;
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListView.DrawItem" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawListViewItemEventArgs" /> that contains the event data.</param>
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			//We draw the current line of items (= item with subitems) on a temp bitmap, then draw the bitmap at once. This is to reduce flickering.
			var b = new Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height);
			var g = Graphics.FromImage(b);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			//always draw default background
			g.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			
			if (e.State.HasFlag(ListViewItemStates.Selected))
			{
				//selected background
				g.FillRectangle(SkinManager.GetFlatButtonPressedBackgroundBrush(), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			}
			else if (e.Bounds.Contains(MouseLocation) && MouseState == MouseState.HOVER)
			{
				//hover background
				g.FillRectangle(SkinManager.GetFlatButtonHoverBackgroundBrush(), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			}


			//Draw separator
			g.DrawLine(new Pen(SkinManager.GetDividersColor()), e.Bounds.Left, 0, e.Bounds.Right, 0);
			
			foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
			{
				//Draw text
				g.DrawString(subItem.Text, SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetPrimaryTextBrush(),
								 new Rectangle(subItem.Bounds.Location.X + ITEM_PADDING, ITEM_PADDING, subItem.Bounds.Width - 2 * ITEM_PADDING, subItem.Bounds.Height - 2 * ITEM_PADDING),
								 getStringFormat());
			}

			e.Graphics.DrawImage((Image) b.Clone(), e.Item.Bounds.Location);
			g.Dispose();
			b.Dispose();
		}

        /// <summary>
        /// Gets the string format.
        /// </summary>
        /// <returns>StringFormat.</returns>
        private StringFormat getStringFormat()
		{
			return new StringFormat
			{
				FormatFlags = StringFormatFlags.LineLimit,
				Trimming = StringTrimming.EllipsisCharacter,
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center
			};
		}

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl" /> method.
        /// </summary>
        protected override void OnCreateControl()
		{
			base.OnCreateControl();

			//This is a hax for the needed padding.
			//Another way would be intercepting all ListViewItems and changing the sizes, but really, that will be a lot of work
			//This will do for now.
			Font = new Font(SkinManager.ROBOTO_MEDIUM_12.FontFamily, 24);
		}
	}
}
