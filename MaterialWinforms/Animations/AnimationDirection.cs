// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="AnimationDirection.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Zeroit.Framework.MaterialWinforms.Animations
{
    /// <summary>
    /// Enum AnimationDirection
    /// </summary>
    public enum AnimationDirection
    {
        /// <summary>
        /// The in
        /// </summary>
        In, //In. Stops if finished.
        /// <summary>
        /// The out
        /// </summary>
        Out, //Out. Stops if finished.
        /// <summary>
        /// The in out in
        /// </summary>
        InOutIn, //Same as In, but changes to InOutOut if finished.
        /// <summary>
        /// The in out out
        /// </summary>
        InOutOut, //Same as Out.
        /// <summary>
        /// The in out repeating in
        /// </summary>
        InOutRepeatingIn, // Same as In, but changes to InOutRepeatingOut if finished.
        /// <summary>
        /// The in out repeating out
        /// </summary>
        InOutRepeatingOut // Same as Out, but changes to InOutRepeatingIn if finished.
    }
}
