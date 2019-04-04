// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="TabWindow.cs" company="Zeroit Dev Technlologies">
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

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class TabWindow.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.Controls.MaterialForm" />
    public class TabWindow: MaterialForm
    {
        /// <summary>
        /// The tab page
        /// </summary>
        private MaterialTabPage TabPage;
        /// <summary>
        /// The base tab control
        /// </summary>
        private MaterialTabSelector BaseTabControl;
        /// <summary>
        /// The root
        /// </summary>
        private MaterialTabControl Root;
        /// <summary>
        /// The return button bounds
        /// </summary>
        private Rectangle ReturnButtonBounds;
        /// <summary>
        /// The return button state
        /// </summary>
        private RetButtonState ReturnButtonState;
        /// <summary>
        /// The closable
        /// </summary>
        private bool Closable;
        /// <summary>
        /// The allow close
        /// </summary>
        private bool allowClose;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabWindow"/> class.
        /// </summary>
        /// <param name="tabPage">The tab page.</param>
        /// <param name="baseTab">The base tab.</param>
        public TabWindow(MaterialTabPage tabPage,ref MaterialTabSelector baseTab)
        {
            TabPage = tabPage;
            Text = TabPage.Text;
            Root = new MaterialTabControl();
            Root.TabPages.Add(TabPage);
            Root.Dock= System.Windows.Forms.DockStyle.Fill;
            BaseTabControl = baseTab;
            Closable = tabPage.Closable;
            Size = TabPage.Size;
            Controls.Add(Root);
            allowClose = false;
        }

        /// <summary>
        /// Enum RetButtonState
        /// </summary>
        protected enum RetButtonState
        {
            /// <summary>
            /// The return button down
            /// </summary>
            ReturnButtonDown,
            /// <summary>
            /// The return button over
            /// </summary>
            ReturnButtonOver,
            /// <summary>
            /// The none
            /// </summary>
            None
        }

        /// <summary>
        /// Updates the buttons.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <param name="up">if set to <c>true</c> [up].</param>
        protected override void UpdateButtons(MouseEventArgs e, bool up = false)
        {
            base.UpdateButtons(e, up);
            RetButtonState oldState = ReturnButtonState;
            if (e.Button == MouseButtons.Left && !up)
            {

                if (ReturnButtonBounds.Contains(e.Location))
                {
                    ReturnButtonState = RetButtonState.ReturnButtonDown;
                   
                }
                else
                    ReturnButtonState = RetButtonState.None;
            }
            else
            {
                if (ReturnButtonBounds.Contains(e.Location))
                {
                    ReturnButtonState = RetButtonState.ReturnButtonOver;

                    if (oldState == RetButtonState.ReturnButtonDown)
                        Return();
                }

                else ReturnButtonState = RetButtonState.None;
            }

            if (oldState != ReturnButtonState) Invalidate();
        }

        /// <summary>
        /// Handles the <see cref="E:Resize" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ReturnButtonBounds = new Rectangle((Width - SkinManager.FORM_PADDING / 2) -  4 * STATUS_BAR_BUTTON_WIDTH , 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT);

        }

        /// <summary>
        /// Returns this instance.
        /// </summary>
        private void Return()
        {
            Root.TabPages.Remove(TabPage);
            BaseTabControl.BaseTabControl.TabPages.Add(TabPage);
            BaseTabControl.Invalidate();
            allowClose = true;
            Close();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.FormClosingEventArgs" /> that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = !allowClose;
        }

        /// <summary>
        /// Handles the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (!Closable)
            {
                g.FillRectangle(SkinManager.ColorScheme.DarkPrimaryBrush, xButtonBounds);
            }

            var downBrush = SkinManager.GetFlatButtonPressedBackgroundBrush();
            if (ReturnButtonState == RetButtonState.ReturnButtonOver )
                g.FillRectangle(downBrush, ReturnButtonBounds);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var DrawerButtonPen = new Pen(SkinManager.ACTION_BAR_TEXT_SECONDARY(), 2))
            {
                g.DrawLine(
                   DrawerButtonPen,
                   ReturnButtonBounds.X + (int)(ReturnButtonBounds.Width * (0.75)),
                   ReturnButtonBounds.Y + (int)(ReturnButtonBounds.Height * (0.5)),
                   ReturnButtonBounds.X + (int)(ReturnButtonBounds.Width * (0.25)),
                   ReturnButtonBounds.Y + (int)(ReturnButtonBounds.Height * (0.5)));
                g.DrawLine(
                   DrawerButtonPen,
                   ReturnButtonBounds.X + (int)(ReturnButtonBounds.Width * (0.5)),
                   ReturnButtonBounds.Y + (int)(ReturnButtonBounds.Height * (0.3)),
                   ReturnButtonBounds.X + (int)(ReturnButtonBounds.Width * (0.25)),
                   ReturnButtonBounds.Y + (int)(ReturnButtonBounds.Height * (0.5)));
                g.DrawLine(
                  DrawerButtonPen,
                  ReturnButtonBounds.X + (int)(ReturnButtonBounds.Width * 0.5),
                  ReturnButtonBounds.Y + (int)(ReturnButtonBounds.Height * 0.7),
                  ReturnButtonBounds.X + (int)(ReturnButtonBounds.Width * 0.25),
                  ReturnButtonBounds.Y + (int)(ReturnButtonBounds.Height * 0.5));
            }
        }
    }
}
