// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="IShadowedMaterialControl.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
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
