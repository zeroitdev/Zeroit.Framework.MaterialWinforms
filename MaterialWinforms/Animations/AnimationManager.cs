// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="AnimationManager.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Zeroit.Framework.MaterialWinforms.Animations
{
    /// <summary>
    /// Class AnimationManager.
    /// </summary>
    public class AnimationManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether [interrupt animation].
        /// </summary>
        /// <value><c>true</c> if [interrupt animation]; otherwise, <c>false</c>.</value>
        public bool InterruptAnimation { get; set; }
        /// <summary>
        /// Gets or sets the increment.
        /// </summary>
        /// <value>The increment.</value>
        public double Increment { get; set; }
        /// <summary>
        /// Gets or sets the secondary increment.
        /// </summary>
        /// <value>The secondary increment.</value>
        public double SecondaryIncrement { get; set; }
        /// <summary>
        /// Gets or sets the type of the animation.
        /// </summary>
        /// <value>The type of the animation.</value>
        public AnimationType AnimationType { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AnimationManager"/> is singular.
        /// </summary>
        /// <value><c>true</c> if singular; otherwise, <c>false</c>.</value>
        public bool Singular { get; set; }

        /// <summary>
        /// Delegate AnimationFinished
        /// </summary>
        /// <param name="sender">The sender.</param>
        public delegate void AnimationFinished(object sender);
        /// <summary>
        /// Occurs when [on animation finished].
        /// </summary>
        public event AnimationFinished OnAnimationFinished;

        /// <summary>
        /// Delegate AnimationProgress
        /// </summary>
        /// <param name="sender">The sender.</param>
        public delegate void AnimationProgress(object sender);
        /// <summary>
        /// Occurs when [on animation progress].
        /// </summary>
        public event AnimationProgress OnAnimationProgress;

        /// <summary>
        /// The animation progresses
        /// </summary>
        private readonly List<double> animationProgresses;
        /// <summary>
        /// The animation sources
        /// </summary>
        private readonly List<Point> animationSources;
        /// <summary>
        /// The animation directions
        /// </summary>
        private readonly List<AnimationDirection> animationDirections;
        /// <summary>
        /// The animation datas
        /// </summary>
        private readonly List<object[]> animationDatas;

        /// <summary>
        /// The minimum value
        /// </summary>
        private const double MIN_VALUE = 0.00;
        /// <summary>
        /// The maximum value
        /// </summary>
        private const double MAX_VALUE = 1.00;

        /// <summary>
        /// The animation timer
        /// </summary>
        private readonly Timer animationTimer = new Timer { Interval = 5, Enabled = false };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="singular">If true, only one animation is supported. The current animation will be replaced with the new one. If false, a new animation is added to the list.</param>
        public AnimationManager(bool singular = true)
        {
            animationProgresses = new List<double>();
            animationSources = new List<Point>();
            animationDirections = new List<AnimationDirection>();
            animationDatas = new List<object[]>();

            Increment = 0.03;
            SecondaryIncrement = 0.03;
            AnimationType = AnimationType.Linear;
            InterruptAnimation = true;
            Singular = singular;

            if (Singular)
            {
                animationProgresses.Add(0);
                animationSources.Add(new Point(0, 0));
                animationDirections.Add(AnimationDirection.In);
            }

            animationTimer.Tick += AnimationTimerOnTick;
        }

        /// <summary>
        /// Animations the timer on tick.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AnimationTimerOnTick(object sender, EventArgs eventArgs)
        {
            for (int i = 0; i < animationProgresses.Count; i++)
            {
                UpdateProgress(i);

                if (!Singular)
                {
                    if ((animationDirections[i] == AnimationDirection.InOutIn && animationProgresses[i] == MAX_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutOut;
                    }
                    else if ((animationDirections[i] == AnimationDirection.InOutRepeatingIn && animationProgresses[i] == MIN_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingOut;
                    }
                    else if ((animationDirections[i] == AnimationDirection.InOutRepeatingOut && animationProgresses[i] == MIN_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingIn;
                    }
                    else if (
                        (animationDirections[i] == AnimationDirection.In && animationProgresses[i] == MAX_VALUE) ||
                        (animationDirections[i] == AnimationDirection.Out && animationProgresses[i] == MIN_VALUE) ||
                        (animationDirections[i] == AnimationDirection.InOutOut && animationProgresses[i] == MIN_VALUE))
                    {
                        animationProgresses.RemoveAt(i);
                        animationSources.RemoveAt(i);
                        animationDirections.RemoveAt(i);
                        animationDatas.RemoveAt(i);
                    }
                }
                else
                {
                    if ((animationDirections[i] == AnimationDirection.InOutIn && animationProgresses[i] == MAX_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutOut;
                    }
                    else if ((animationDirections[i] == AnimationDirection.InOutRepeatingIn && animationProgresses[i] == MAX_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingOut;
                    }
                    else if ((animationDirections[i] == AnimationDirection.InOutRepeatingOut && animationProgresses[i] == MIN_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingIn;
                    }
                }
            }

            if (OnAnimationProgress != null)
            {
                OnAnimationProgress(this);
            }
        }

        /// <summary>
        /// Determines whether this instance is animating.
        /// </summary>
        /// <returns><c>true</c> if this instance is animating; otherwise, <c>false</c>.</returns>
        public bool IsAnimating()
        {
            return animationTimer.Enabled;
        }

        /// <summary>
        /// Starts the new animation.
        /// </summary>
        /// <param name="animationDirection">The animation direction.</param>
        /// <param name="data">The data.</param>
        public void StartNewAnimation(AnimationDirection animationDirection, object[] data = null)
        {
            StartNewAnimation(animationDirection, new Point(0, 0), data);
        }

        /// <summary>
        /// Starts the new animation.
        /// </summary>
        /// <param name="animationDirection">The animation direction.</param>
        /// <param name="animationSource">The animation source.</param>
        /// <param name="data">The data.</param>
        /// <exception cref="System.Exception">Invalid AnimationDirection</exception>
        public void StartNewAnimation(AnimationDirection animationDirection, Point animationSource, object[] data = null)
        {
            if (!IsAnimating() || InterruptAnimation)
            {
                if (Singular && animationDirections.Count > 0)
                {
                    animationDirections[0] = animationDirection;
                }
                else
                {
                    animationDirections.Add(animationDirection);
                }

                if (Singular && animationSources.Count > 0)
                {
                    animationSources[0] = animationSource;
                }
                else
                {
                    animationSources.Add(animationSource);
                }

                if (!(Singular && animationProgresses.Count > 0))
                {
                    switch (animationDirections[animationDirections.Count - 1])
                    {
                        case AnimationDirection.InOutRepeatingIn:
                        case AnimationDirection.InOutIn:
                        case AnimationDirection.In:
                            animationProgresses.Add(MIN_VALUE);
                            break;
                        case AnimationDirection.InOutRepeatingOut:
                        case AnimationDirection.InOutOut:
                        case AnimationDirection.Out:
                            animationProgresses.Add(MAX_VALUE);
                            break;
                        default:
                            throw new Exception("Invalid AnimationDirection");
                    }
                }

                if (Singular && animationDatas.Count > 0)
                {
                    animationDatas[0] = data ?? new object[] { };
                }
                else
                {
                    animationDatas.Add(data ?? new object[] { });
                }

            }

            animationTimer.Start();
        }

        /// <summary>
        /// Updates the progress.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <exception cref="System.Exception">No AnimationDirection has been set</exception>
        public void UpdateProgress(int index)
        {
            switch (animationDirections[index])
            {
                case AnimationDirection.InOutRepeatingIn:
                case AnimationDirection.InOutIn:
                case AnimationDirection.In:
                    IncrementProgress(index);
                    break;
                case AnimationDirection.InOutRepeatingOut:
                case AnimationDirection.InOutOut:
                case AnimationDirection.Out:
                    DecrementProgress(index);
                    break;
                default:
                    throw new Exception("No AnimationDirection has been set");
            }
        }

        /// <summary>
        /// Increments the progress.
        /// </summary>
        /// <param name="index">The index.</param>
        private void IncrementProgress(int index)
        {
            animationProgresses[index] += Increment;
            if (animationProgresses[index] > MAX_VALUE)
            {
                animationProgresses[index] = MAX_VALUE;

                for (int i = 0; i < GetAnimationCount(); i++)
                {
                    if (animationDirections[i] == AnimationDirection.InOutIn) return;
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingIn) return;
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingOut) return;
                    if (animationDirections[i] == AnimationDirection.InOutOut && animationProgresses[i] != MAX_VALUE) return;
                    if (animationDirections[i] == AnimationDirection.In && animationProgresses[i] != MAX_VALUE) return;
                }
                
                animationTimer.Stop();
                if (OnAnimationFinished != null) OnAnimationFinished(this);
            }
        }

        /// <summary>
        /// Decrements the progress.
        /// </summary>
        /// <param name="index">The index.</param>
        private void DecrementProgress(int index)
        {
            animationProgresses[index] -= (animationDirections[index] == AnimationDirection.InOutOut || animationDirections[index] == AnimationDirection.InOutRepeatingOut) ? SecondaryIncrement : Increment;
            if (animationProgresses[index] < MIN_VALUE)
            {
                animationProgresses[index] = MIN_VALUE;

                for (int i = 0; i < GetAnimationCount(); i++)
                {
                    if (animationDirections[i] == AnimationDirection.InOutIn) return;
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingIn) return;
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingOut) return;
                    if (animationDirections[i] == AnimationDirection.InOutOut && animationProgresses[i] != MIN_VALUE) return;
                    if (animationDirections[i] == AnimationDirection.Out && animationProgresses[i] != MIN_VALUE) return;
                }

                animationTimer.Stop();
                if (OnAnimationFinished != null) OnAnimationFinished(this);
            }
        }

        /// <summary>
        /// Gets the progress.
        /// </summary>
        /// <returns>System.Double.</returns>
        /// <exception cref="System.Exception">
        /// Animation is not set to Singular.
        /// or
        /// Invalid animation
        /// </exception>
        public double GetProgress()
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationProgresses.Count == 0)
                throw new Exception("Invalid animation");

            return GetProgress(0);
        }

        /// <summary>
        /// Gets the progress.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>System.Double.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Invalid animation index</exception>
        /// <exception cref="System.NotImplementedException">The given AnimationType is not implemented</exception>
        public double GetProgress(int index)
        {
            if (!(index < GetAnimationCount()))
                throw new IndexOutOfRangeException("Invalid animation index");

            switch (AnimationType)
            {
                case AnimationType.Linear:
                    return AnimationLinear.CalculateProgress(animationProgresses[index]);
                case AnimationType.EaseInOut:
                    return AnimationEaseInOut.CalculateProgress(animationProgresses[index]);
                case AnimationType.EaseOut:
                    return AnimationEaseOut.CalculateProgress(animationProgresses[index]);
                case AnimationType.CustomQuadratic:
                    return AnimationCustomQuadratic.CalculateProgress(animationProgresses[index]);
                default:
                    throw new NotImplementedException("The given AnimationType is not implemented");
            }

        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Point.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Invalid animation index</exception>
        public Point GetSource(int index)
        {
            if (!(index < GetAnimationCount()))
                throw new IndexOutOfRangeException("Invalid animation index");

            return animationSources[index];
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <returns>Point.</returns>
        /// <exception cref="System.Exception">
        /// Animation is not set to Singular.
        /// or
        /// Invalid animation
        /// </exception>
        public Point GetSource()
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationSources.Count == 0)
                throw new Exception("Invalid animation");

            return animationSources[0];
        }

        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <returns>AnimationDirection.</returns>
        /// <exception cref="System.Exception">
        /// Animation is not set to Singular.
        /// or
        /// Invalid animation
        /// </exception>
        public AnimationDirection GetDirection()
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationDirections.Count == 0)
                throw new Exception("Invalid animation");

            return animationDirections[0];
        }

        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>AnimationDirection.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Invalid animation index</exception>
        public AnimationDirection GetDirection(int index)
        {
            if (!(index < animationDirections.Count))
                throw new IndexOutOfRangeException("Invalid animation index");

            return animationDirections[index];
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>System.Object[].</returns>
        /// <exception cref="System.Exception">
        /// Animation is not set to Singular.
        /// or
        /// Invalid animation
        /// </exception>
        public object[] GetData()
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationDatas.Count == 0)
                throw new Exception("Invalid animation");

            return animationDatas[0];
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>System.Object[].</returns>
        /// <exception cref="System.IndexOutOfRangeException">Invalid animation index</exception>
        public object[] GetData(int index)
        {
            if (!(index < animationDatas.Count))
                throw new IndexOutOfRangeException("Invalid animation index");

            return animationDatas[index];
        }

        /// <summary>
        /// Gets the animation count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetAnimationCount()
        {
            return animationProgresses.Count;
        }

        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="progress">The progress.</param>
        /// <exception cref="System.Exception">
        /// Animation is not set to Singular.
        /// or
        /// Invalid animation
        /// </exception>
        public void SetProgress(double progress)
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationProgresses.Count == 0)
                throw new Exception("Invalid animation");

            animationProgresses[0] = progress;
        }

        /// <summary>
        /// Sets the direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <exception cref="System.Exception">
        /// Animation is not set to Singular.
        /// or
        /// Invalid animation
        /// </exception>
        public void SetDirection(AnimationDirection direction)
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationProgresses.Count == 0)
                throw new Exception("Invalid animation");

            animationDirections[0] = direction;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="System.Exception">
        /// Animation is not set to Singular.
        /// or
        /// Invalid animation
        /// </exception>
        public void SetData(object[] data)
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationDatas.Count == 0)
                throw new Exception("Invalid animation");

            animationDatas[0] = data;
        }
    }
}
