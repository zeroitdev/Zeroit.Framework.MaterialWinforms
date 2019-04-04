// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="Animations.cs" company="Zeroit Dev Technlologies">
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

namespace Zeroit.Framework.MaterialWinforms.Animations
{
    /// <summary>
    /// Enum AnimationType
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        /// The linear
        /// </summary>
        Linear,
        /// <summary>
        /// The ease in out
        /// </summary>
        EaseInOut,
        /// <summary>
        /// The ease out
        /// </summary>
        EaseOut,
        /// <summary>
        /// The custom quadratic
        /// </summary>
        CustomQuadratic
    }

    /// <summary>
    /// Class AnimationLinear.
    /// </summary>
    static class AnimationLinear
    {
        /// <summary>
        /// Calculates the progress.
        /// </summary>
        /// <param name="progress">The progress.</param>
        /// <returns>System.Double.</returns>
        public static double CalculateProgress(double progress)
        {
            return progress;
        }
    }

    /// <summary>
    /// Class AnimationEaseInOut.
    /// </summary>
    static class AnimationEaseInOut
    {
        /// <summary>
        /// The pi
        /// </summary>
        public static double PI = Math.PI;
        /// <summary>
        /// The pi half
        /// </summary>
        public static double PI_HALF = Math.PI / 2;

        /// <summary>
        /// Calculates the progress.
        /// </summary>
        /// <param name="progress">The progress.</param>
        /// <returns>System.Double.</returns>
        public static double CalculateProgress(double progress)
        {
            return EaseInOut(progress);
        }

        /// <summary>
        /// Eases the in out.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>System.Double.</returns>
        private static double EaseInOut(double s)
        {
            return s - Math.Sin(s * 2 * PI) / (2 * PI);
        }
    }

    /// <summary>
    /// Class AnimationEaseOut.
    /// </summary>
    public static class AnimationEaseOut
    {
        /// <summary>
        /// Calculates the progress.
        /// </summary>
        /// <param name="progress">The progress.</param>
        /// <returns>System.Double.</returns>
        public static double CalculateProgress(double progress)
        {
            return -1 * progress * (progress - 2);
        }
    }

    /// <summary>
    /// Class AnimationCustomQuadratic.
    /// </summary>
    public static class AnimationCustomQuadratic
    {
        /// <summary>
        /// Calculates the progress.
        /// </summary>
        /// <param name="progress">The progress.</param>
        /// <returns>System.Double.</returns>
        public static double CalculateProgress(double progress)
        {
            double kickoff = 0.6;
            return 1 - Math.Cos((Math.Max(progress, kickoff) - kickoff) * Math.PI / (2 - (2 * kickoff)));
        }
    }
}
