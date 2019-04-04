// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialContextMenuStrip.cs" company="Zeroit Dev Technlologies">
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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Zeroit.Framework.MaterialWinforms.Animations;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialContextMenuStrip.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ContextMenuStrip" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public class MaterialContextMenuStrip : ContextMenuStrip, IMaterialControl
    {
        //Properties for managing the material design properties
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
        /// The animation manager
        /// </summary>
        internal AnimationManager animationManager;
        /// <summary>
        /// The animation source
        /// </summary>
        internal Point animationSource;

        /// <summary>
        /// Delegate ItemClickStart
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        public delegate void ItemClickStart(object sender, ToolStripItemClickedEventArgs e);
        /// <summary>
        /// Occurs when [on item click start].
        /// </summary>
        public event ItemClickStart OnItemClickStart;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialContextMenuStrip"/> class.
        /// </summary>
        public MaterialContextMenuStrip()
        {
            Renderer = new MaterialToolStripRender();
            animationManager = new AnimationManager(false)
            {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
            animationManager.OnAnimationFinished += sender => OnItemClicked(delayesArgs);

            BackColor = SkinManager.GetApplicationBackgroundColor();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseUp" /> event.
        /// </summary>
        /// <param name="mea">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs mea)
        {
            base.OnMouseUp(mea);

            animationSource = mea.Location;
        }

        /// <summary>
        /// The delayes arguments
        /// </summary>
        private ToolStripItemClickedEventArgs delayesArgs;
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStrip.ItemClicked" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemClickedEventArgs" /> that contains the event data.</param>
        protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null && !(e.ClickedItem is ToolStripSeparator))
            {
                if (e == delayesArgs)
                {
                    //The event has been fired manualy because the args are the ones we saved for delay
                    base.OnItemClicked(e);
                }
                else
                {
                    //Interrupt the default on click, saving the args for the delay which is needed to display the animaton
                    delayesArgs = e;

                    //Fire custom event to trigger actions directly but keep cms open
                    if (OnItemClickStart != null) OnItemClickStart(this, e);

                    //Start animation
                    animationManager.StartNewAnimation(AnimationDirection.In);
                }
            }
        }
    }

    /// <summary>
    /// Class MaterialToolStripMenuItem.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ToolStripMenuItem" />
    public class MaterialToolStripMenuItem : ToolStripMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialToolStripMenuItem"/> class.
        /// </summary>
        public MaterialToolStripMenuItem()
        {
            Size = new Size(500, 30);
        }

        /// <summary>
        /// Creates a generic <see cref="T:System.Windows.Forms.ToolStripDropDown" /> for which events can be defined.
        /// </summary>
        /// <returns>A <see cref="T:System.Windows.Forms.ToolStripDropDown" />.</returns>
        protected override ToolStripDropDown CreateDefaultDropDown()
        {
            var baseDropDown = base.CreateDefaultDropDown();
            if (DesignMode) return baseDropDown;

            var defaultDropDown = new MaterialContextMenuStrip();
            defaultDropDown.Items.AddRange(baseDropDown.Items);

            return defaultDropDown;
        }
    }

    /// <summary>
    /// Class MaterialToolStripRender.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ToolStripProfessionalRenderer" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    internal class MaterialToolStripRender : ToolStripProfessionalRenderer, IMaterialControl
    {
        //Properties for managing the material design properties
        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth { get; set; }
        /// <summary>
        /// Gets the skin manager.
        /// </summary>
        /// <value>The skin manager.</value>
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        /// <summary>
        /// Gets or sets the state of the mouse.
        /// </summary>
        /// <value>The state of the mouse.</value>
        public MouseState MouseState { get; set; }

        /// <summary>
        /// Gets the color of the back.
        /// </summary>
        /// <value>The color of the back.</value>
        public Color BackColor { get { return SkinManager.GetCardsColor(); } }

        /// <summary>
        /// Handles the <see cref="E:RenderItemText" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemTextRenderEventArgs" /> that contains the event data.</param>
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            var itemRect = GetItemRect(e.Item);
            var textRect = new Rectangle(0, itemRect.Y, itemRect.Width, itemRect.Height);
            g.DrawString(
                e.Text,
                SkinManager.ROBOTO_MEDIUM_10,
                e.Item.Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush(),
                textRect,
                new StringFormat { LineAlignment = StringAlignment.Center });
        }



        /// <summary>
        /// Handles the <see cref="E:RenderMenuItemBackground" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(SkinManager.GetCardsColor());

            //Draw background
            var itemRect = GetItemRect(e.Item);
            g.FillRectangle(e.Item.Selected && e.Item.Enabled ? SkinManager.GetCmsSelectedItemBrush() : new SolidBrush(SkinManager.GetCardsColor()), itemRect);

            //Ripple animation
            var toolStrip = e.ToolStrip as MaterialContextMenuStrip;
            if (toolStrip != null)
            {
                var animationManager = toolStrip.animationManager;
                var animationSource = toolStrip.animationSource;
                if (toolStrip.animationManager.IsAnimating() && e.Item.Bounds.Contains(animationSource))
                {
                    for (int i = 0; i < animationManager.GetAnimationCount(); i++)
                    {
                        var animationValue = animationManager.GetProgress(i);
                        var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationValue * 50)), Color.Black));
                        var rippleSize = (int)(animationValue * itemRect.Width * 2.5);
                        g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, itemRect.Y - itemRect.Height, rippleSize, itemRect.Height * 3));
                    }
                }
            }
        }


        /// <summary>
        /// Handles the <see cref="E:RenderImageMargin" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> that contains the event data.</param>
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {

        }

        /// <summary>
        /// Handles the <see cref="E:RenderSeparator" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripSeparatorRenderEventArgs" /> that contains the event data.</param>
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            var g = e.Graphics;

            g.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), e.Item.Bounds);
            g.DrawLine(
                new Pen(SkinManager.GetDividersColor()),
                new Point(e.Item.Bounds.Left, e.Item.Bounds.Height / 2),
                new Point(e.Item.Bounds.Right, e.Item.Bounds.Height / 2));
        }

        /// <summary>
        /// Handles the <see cref="E:RenderToolStripBorder" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> that contains the event data.</param>
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            e.ToolStrip.BackColor = SkinManager.GetCardsColor();   

        }



        /// <summary>
        /// Handles the <see cref="E:RenderArrow" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripArrowRenderEventArgs" /> that contains the event data.</param>
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            var g = e.Graphics;
            const int ARROW_SIZE = 4;

            var arrowMiddle = new Point(e.ArrowRectangle.X + e.ArrowRectangle.Width / 2, e.ArrowRectangle.Y + e.ArrowRectangle.Height / 2);
            var arrowBrush = e.Item.Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush();
            using (var arrowPath = new GraphicsPath())
            {
                arrowPath.AddLines(
                    new[] { 
                        new Point(arrowMiddle.X - ARROW_SIZE, arrowMiddle.Y - ARROW_SIZE), 
                        new Point(arrowMiddle.X, arrowMiddle.Y), 
                        new Point(arrowMiddle.X - ARROW_SIZE, arrowMiddle.Y + ARROW_SIZE) });
                arrowPath.CloseFigure();

                g.FillPath(arrowBrush, arrowPath);
            }
        }

        /// <summary>
        /// Gets the item rect.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Rectangle.</returns>
        private Rectangle GetItemRect(ToolStripItem item)
        {
            return item.ContentRectangle;
            //return new Rectangle(0, item.ContentRectangle.Y, item.ContentRectangle.Width, item.ContentRectangle.Height);
        }

    }
}
