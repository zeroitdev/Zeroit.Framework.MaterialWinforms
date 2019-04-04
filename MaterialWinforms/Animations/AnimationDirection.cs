// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="AnimationDirection.cs" company="Zeroit Dev Technlologies">
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
