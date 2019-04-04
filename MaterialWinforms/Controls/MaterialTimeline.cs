// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialTimeline.cs" company="Zeroit Dev Technlologies">
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialTimeline.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    [Serializable]
    public partial class MaterialTimeline : Control, IMaterialControl
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
        /// Delegate TimeLineEntryClicked
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        public delegate void TimeLineEntryClicked(MaterialTimeLineEntry sender, MouseEventArgs e);
        /// <summary>
        /// Occurs when [on time line entry clicked].
        /// </summary>
        public event TimeLineEntryClicked onTimeLineEntryClicked;

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : typeof(IMaterialControl).IsAssignableFrom(Parent.GetType()) ? ((IMaterialControl)Parent).BackColor : Parent.BackColor; } }

        /// <summary>
        /// The aufsteigen sortieren
        /// </summary>
        private Boolean _AufsteigenSortieren;
        /// <summary>
        /// Gets or sets the aufsteigend sortieren.
        /// </summary>
        /// <value>The aufsteigend sortieren.</value>
        public Boolean AufsteigendSortieren { get { return _AufsteigenSortieren; } set { _AufsteigenSortieren = value; sortEntrys(); } }

        /// <summary>
        /// Gets or sets the entrys.
        /// </summary>
        /// <value>The entrys.</value>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObservableCollection<MaterialTimeLineEntry> Entrys { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialTimeline"/> class.
        /// </summary>
        public MaterialTimeline()
        {
            DoubleBuffered = true;
            InitializeComponent();
            Entrys = new ObservableCollection<MaterialTimeLineEntry>();
            Entrys.CollectionChanged += Entrys_CollectionChanged;
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Entrys control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        void Entrys_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            sortEntrys();
        }

        /// <summary>
        /// Sorts the entrys.
        /// </summary>
        private void sortEntrys()
        {
            Entrys.CollectionChanged -= Entrys_CollectionChanged;

            foreach (MaterialTimeLineEntry objEntry in Entrys)
            {
                objEntry.SizeChanged -= EntrySizeChanged;
                objEntry.MouseClick -= objEntry_Click;
            }

            List<MaterialTimeLineEntry> objSorted = Entrys.OrderByDescending(Entry => Entry, new TimeLineComparer(_AufsteigenSortieren)).ToList<MaterialTimeLineEntry>();
            Entrys.Clear();
            Controls.Clear();
            int h = 0;
            int w = 0;
            foreach (MaterialTimeLineEntry objEntry in objSorted)
            {
                objEntry.Dock = DockStyle.Top;
                Controls.Add(objEntry);
                Entrys.Add(objEntry);
                objEntry.SizeChanged += EntrySizeChanged;
                objEntry.MouseClick += objEntry_Click;
                h += objEntry.Height;
                if (objEntry.Width > w)
                {
                    w = objEntry.Width;
                }
            }

            Entrys.CollectionChanged += Entrys_CollectionChanged;

            Size = new Size(w, h);
        }

        /// <summary>
        /// Handles the Click event of the objEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        void objEntry_Click(object sender, MouseEventArgs e)
        {
            if(onTimeLineEntryClicked != null)
            {

                onTimeLineEntryClicked((MaterialTimeLineEntry)sender,e);
            }
        }


        /// <summary>
        /// Entries the size changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void EntrySizeChanged(object sender, EventArgs e)
        {

            int w = 0;
            int h = 0;
            foreach (MaterialTimeLineEntry objEntry in Entrys)
            {
                h += objEntry.Height;
                if (objEntry.Width > w)
                {
                    w = objEntry.Width;
                }
            }
            Size = new Size(w, h);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
        }
    }

    /// <summary>
    /// Class TimeLineComparer.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IComparer{Zeroit.Framework.MaterialWinforms.Controls.MaterialTimeLineEntry}" />
    internal class TimeLineComparer : IComparer<MaterialTimeLineEntry>
    {
        /// <summary>
        /// The aufsteigen sortieren
        /// </summary>
        private bool _AufsteigenSortieren;
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeLineComparer"/> class.
        /// </summary>
        /// <param name="AufsteigenSortieren">The aufsteigen sortieren.</param>
        public TimeLineComparer(Boolean AufsteigenSortieren)
        {
            _AufsteigenSortieren = AufsteigenSortieren;
        }

        /// <summary>
        /// Compares the specified t1.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>System.Int32.</returns>
        public int Compare(MaterialTimeLineEntry t1, MaterialTimeLineEntry t2)
        {
            if (t1.Time > t2.Time)
            {
                return _AufsteigenSortieren ? 1 : -1;
            }
            else if (t1.Time == t2.Time)
            {
                return 0;
            };
            return _AufsteigenSortieren ? -1 : 1;
        }
    }

}
