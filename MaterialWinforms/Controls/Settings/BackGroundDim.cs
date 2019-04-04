// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="BackGroundDim.cs" company="Zeroit Dev Technlologies">
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
using System.Windows.Forms;

namespace Zeroit.Framework.MaterialWinforms.Controls.Settings
{
    /// <summary>
    /// Class BackGroundDim.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.Controls.MaterialForm" />
    public partial class BackGroundDim : MaterialForm
    {

        /// <summary>
        /// Gets a value indicating whether the control can receive focus.
        /// </summary>
        /// <value><c>true</c> if this instance can focus; otherwise, <c>false</c>.</value>
        public new bool CanFocus { get { return false; } }
        /// <summary>
        /// The is visible
        /// </summary>
        public bool IsVisible = false;

        /// <summary>
        /// The final size
        /// </summary>
        private Size finalSize;
        /// <summary>
        /// The final location
        /// </summary>
        private Point finalLocation;
        /// <summary>
        /// The form to dim
        /// </summary>
        private Form _FormToDim;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackGroundDim"/> class.
        /// </summary>
        /// <param name="FormToDim">The form to dim.</param>
        public BackGroundDim(MaterialForm FormToDim)
        {
            _FormToDim = FormToDim;
            InitializeComponent();
            SkinManager.AddFormToManage(this);
            StartPosition = FormStartPosition.Manual;
            Size = FormToDim.Size;
            MinimumSize = Size;
            Location = FormToDim.Location;
            BackColor = FormToDim.SkinManager.GetApplicationBackgroundColor();
            Opacity = 0;

            SetStyle(ControlStyles.StandardDoubleClick, false);
            //Enabled = false;
            finalLocation = FormToDim.Location; ;
            finalSize = FormToDim.Size;

            FormToDim.LocationChanged += FormToDim_LocationChanged;
        }

        /// <summary>
        /// Handles the LocationChanged event of the FormToDim control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void FormToDim_LocationChanged(object sender, EventArgs e)
        {
            Location = ((Form)sender).Location;
            Application.DoEvents();
        }


        /// <summary>
        /// Handles the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(SkinManager.GetApplicationBackgroundColor());
        }

        /// <summary>
        /// Gets a value indicating whether the window will be activated when it is shown.
        /// </summary>
        /// <value><c>true</c> if [show without activation]; otherwise, <c>false</c>.</value>
        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:Resize" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Size = _FormToDim.Size;
            Location = _FormToDim.Location;
        }


        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LocationChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLocationChanged(EventArgs e)
        {
            Location = _FormToDim.Location;
        }

        /// <summary>
        /// The wm mouseactivate
        /// </summary>
        private const int WM_MOUSEACTIVATE = 0x0021, MA_NOACTIVATE = 0x0003;

        /// <summary>
        /// WNDs the proc.
        /// </summary>
        /// <param name="m">The m.</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = (IntPtr)MA_NOACTIVATE;
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Shown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            for (int i = 0; i < 40; i++)
            {
                Opacity = Opacity + 0.02;
                Application.DoEvents();
                System.Threading.Thread.Sleep(1);
            }
            IsVisible = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            for (int i = 0; i < 40; i++)
            {
                Opacity = Opacity - 0.02;
                Application.DoEvents();
                System.Threading.Thread.Sleep(1);
            }
            IsVisible = false;
        }
    }
}
