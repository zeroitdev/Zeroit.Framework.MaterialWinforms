// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialDropDownColorPicker.cs" company="Zeroit Dev Technlologies">
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
    /// Class MaterialDropDownColorPicker.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.Controls.DropDownControl" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public partial class MaterialDropDownColorPicker : DropDownControl,IMaterialControl
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
        /// The object color control
        /// </summary>
        private MaterialColorPicker objColorControl;
        /// <summary>
        /// The color
        /// </summary>
        private Color _Color;
        /// <summary>
        /// The color rect
        /// </summary>
        private Rectangle ColorRect;
        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : Parent.BackColor; } set { } }
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value; objColorControl.Value = _Color;
                }
            }
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDropDownColorPicker"/> class.
        /// </summary>
        public MaterialDropDownColorPicker()
        {
            InitializeComponent();
            objColorControl = new MaterialColorPicker();
            Color = SkinManager.ColorScheme.AccentColor;
            objColorControl.onColorChanged += objDateControl_onDateChanged;
            InitializeDropDown(objColorControl);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                ColorRect = new Rectangle();
                ColorRect.Location = new Point(1, 1);
                ColorRect.Size = new Size((int)(Width - 18), (int)(Height * 0.8));

                e.Graphics.FillRectangle(new SolidBrush(Color), ColorRect);
            }

        /// <summary>
        /// Objects the date control on date changed.
        /// </summary>
        /// <param name="newColor">The new color.</param>
        void objDateControl_onDateChanged(Color newColor)
        {
            Color = newColor;
            Invalidate();
        }
    }
}
