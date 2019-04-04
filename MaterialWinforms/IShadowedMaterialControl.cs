// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="IShadowedMaterialControl.cs" company="Zeroit Dev Technlologies">
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
using System.Drawing.Drawing2D;

namespace Zeroit.Framework.MaterialWinforms
{
    /// <summary>
    /// Interface IShadowedMaterialControl
    /// </summary>
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    interface IShadowedMaterialControl : IMaterialControl
    {
        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        int Depth { get; set; }
        /// <summary>
        /// Gets or sets the elevation.
        /// </summary>
        /// <value>The elevation.</value>
        int Elevation { get; set; }

        /// <summary>
        /// Gets or sets the shadow border.
        /// </summary>
        /// <value>The shadow border.</value>
        GraphicsPath ShadowBorder { get; set; }
        /// <summary>
        /// Gets the skin manager.
        /// </summary>
        /// <value>The skin manager.</value>
        MaterialSkinManager SkinManager { get; }
        /// <summary>
        /// Gets or sets the state of the mouse.
        /// </summary>
        /// <value>The state of the mouse.</value>
        MouseState MouseState { get; set; }

    }

    /// <summary>
    /// Enum MouseState
    /// </summary>
    public enum MouseState
    {
        /// <summary>
        /// The hover
        /// </summary>
        HOVER,
        /// <summary>
        /// Down
        /// </summary>
        DOWN,
        /// <summary>
        /// The out
        /// </summary>
        OUT
    }
}
