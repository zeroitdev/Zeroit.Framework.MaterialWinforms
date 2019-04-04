// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 07-04-2018
// ***********************************************************************
// <copyright file="MaterialBreadCrumbToolbar.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections.ObjectModel;
using System;

using Zeroit.Framework.MaterialWinforms.Animations;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialBreadCrumbToolbar.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public partial class MaterialBreadCrumbToolbar : Control, IMaterialControl
    {

        /// <summary>
        /// The teile
        /// </summary>
        private ObservableCollection<BreadCrumbItem> _Teile;

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
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
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public Color BackColor { get { return SkinManager.GetCardsColor() ; } }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public  ObservableCollection<BreadCrumbItem> Items
        {
            get { return _Teile; }
        }

        /// <summary>
        /// Delegate BreadCrumbItemClicked
        /// </summary>
        /// <param name="pTitel">The p titel.</param>
        /// <param name="pTag">The p tag.</param>
        public delegate void BreadCrumbItemClicked(String pTitel,Object pTag);
        /// <summary>
        /// Occurs when [on bread crumb item clicked].
        /// </summary>
        public event BreadCrumbItemClicked onBreadCrumbItemClicked;

        /// <summary>
        /// The item lengt
        /// </summary>
        private int ItemLengt;
        /// <summary>
        /// The trennzeichen
        /// </summary>
        private String _Trennzeichen = "  >";
        /// <summary>
        /// The hovered item
        /// </summary>
        private int HoveredItem = -1;
        /// <summary>
        /// The selected item index
        /// </summary>
        private int SelectedItemIndex = -1;
        /// <summary>
        /// The animation source
        /// </summary>
        private Point animationSource;
        /// <summary>
        /// The mouse down
        /// </summary>
        private bool mouseDown = false;
        /// <summary>
        /// The offset
        /// </summary>
        private int offset = 0;
        /// <summary>
        /// The tab offset
        /// </summary>
        private int TabOffset = 0;
        /// <summary>
        /// The old x location
        /// </summary>
        private int oldXLocation = -1;
        /// <summary>
        /// The tab length
        /// </summary>
        private int TabLength = 0;
        /// <summary>
        /// The animation manager
        /// </summary>
        private readonly AnimationManager animationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialBreadCrumbToolbar"/> class.
        /// </summary>
        public MaterialBreadCrumbToolbar()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Height = 1;
            Padding = new Padding(5, 5, 5, 5);
            _Teile = new ObservableCollection<BreadCrumbItem>();
            _Teile.CollectionChanged += RecalculateTabRects;
            ParentChanged += new System.EventHandler(Redraw);
            DoubleBuffered = true;
            animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseOut,
                Increment = 0.03
            };
            animationManager.OnAnimationProgress += sender => Invalidate();

        }

        /// <summary>
        /// Redraws the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Redraw(object sender, System.EventArgs e)
        {
            Invalidate();
            if (Parent != null)
            {
                Parent.BackColorChanged += new System.EventHandler(Redraw);
            }

        }

        /// <summary>
        /// Recalculates the tab rects.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void RecalculateTabRects(object sender, EventArgs e)
        {
            UpdateTabRects();
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseDown && _Teile.Count>0)
            {
                bool move = false;


                if (oldXLocation > 0)
                {

                    int off = offset;
                    off -= oldXLocation - e.X;
                    if (_Teile[0].ItemRect.X + off < 0)
                    {
                        if (_Teile[_Teile.Count - 1].ItemRect.Right + off > Width)
                        {
                            move = true;
                        }
                    }
                    else
                    {
                        if (_Teile[_Teile.Count - 1].ItemRect.Right + off < Width)
                        {
                            move = true;
                        }
                    }

                    if (move)
                    {
                        offset -= oldXLocation - e.X;
                        oldXLocation = e.X;
                        Invalidate();
                    }
                }
                else
                {
                    oldXLocation = e.X;
                    Invalidate();
                }


                return;
            }

            foreach (BreadCrumbItem objItem in _Teile)
            {
                if (objItem.ItemRect.Contains(e.Location))
                {
                    int newItem = _Teile.IndexOf(objItem);
                    if (HoveredItem != newItem)
                    {
                        HoveredItem = newItem;
                        Invalidate();
                    }
                    return;
                }
            }
            if (HoveredItem != -1)
            {
                HoveredItem = -1;
                Invalidate();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                mouseDown = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (HoveredItem != -1)
            {
                HoveredItem = -1;
                Invalidate();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);


            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mouseDown = false;
                oldXLocation = -1;
                bool ignoreClick = false;
                if (Math.Abs(offset) > 5)
                {
                    TabOffset += offset;
                    ignoreClick = true;
                }

                offset = 0;
                if (!ignoreClick && HoveredItem > -1)
                {
                    SelectedItemIndex = HoveredItem;
                    animationSource = e.Location;
                    UpdateTabRects();
                    animationManager.SetProgress(0);
                    animationManager.StartNewAnimation(AnimationDirection.In);
                    Invalidate();
                    if (onBreadCrumbItemClicked != null && SelectedItemIndex > -1)
                    {
                        onBreadCrumbItemClicked(_Teile[SelectedItemIndex].Text, _Teile[SelectedItemIndex].Tag);
                    }
                }
                UpdateTabRects();
                Invalidate();
            }

        }

        /// <summary>
        /// Handles the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="pevent">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            int iCropping = ClientRectangle.Width / 3;
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(Parent.BackColor);

            this.Region = new Region(DrawHelper.CreateRoundRect(ClientRectangle.X + 3,
                                    ClientRectangle.Y + 3,
                                    ClientRectangle.Width - 3, ClientRectangle.Height - 3, 10));

            using (var backgroundPath = DrawHelper.CreateRoundRect(ClientRectangle.X,
                    ClientRectangle.Y,
                    ClientRectangle.Width, ClientRectangle.Height, 3))
            {
                g.FillPath(SkinManager.getCardsBrush(), backgroundPath);

            }
            if (_Teile.Count > 0)
            {
                if (HoveredItem >= 0)
                {
                    g.FillRectangle(SkinManager.GetFlatButtonHoverBackgroundBrush(),
                        new Rectangle(_Teile[HoveredItem].ItemRect.X + offset, _Teile[HoveredItem].ItemRect.Y, _Teile[HoveredItem].ItemRect.Width, _Teile[HoveredItem].ItemRect.Height));
                }
                //Click feedback
                if (animationManager.IsAnimating())
                {
                    double animationProgress = animationManager.GetProgress();

                    var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationProgress * 50)), Color.White));
                    var rippleSize = (int)(animationProgress * _Teile[SelectedItemIndex].ItemRect.Width * 1.75);

                    g.SetClip(_Teile[SelectedItemIndex].ItemRect);
                    g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                    g.ResetClip();
                    rippleBrush.Dispose();
                }
                for (int i = 0; i < _Teile.Count; i++)
                {
                    g.DrawString(
                        _Teile[i].Text + (i == _Teile.Count - 1 ? "" : _Trennzeichen),
                        SkinManager.ROBOTO_MEDIUM_10,
                        SkinManager.GetPrimaryTextBrush(),
                        new Rectangle(_Teile[i].ItemRect.X + offset, _Teile[i].ItemRect.Y, _Teile[i].ItemRect.Width, _Teile[i].ItemRect.Height),
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
            }



        }

        /// <summary>
        /// Updates the tab rects.
        /// </summary>
        private void UpdateTabRects()
        {
            ItemLengt = 0;
            if (_Teile.Count == 0) return;

            using (var b = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(b))
                {

                    _Teile[0].ItemRect = new Rectangle(10, Convert.ToInt32((Height - g.MeasureString("T", SkinManager.ROBOTO_MEDIUM_10).Height) / 2), (int)g.MeasureString(_Teile[0].Text + (0 == _Teile.Count - 1 ? "" : _Trennzeichen) + 5, SkinManager.ROBOTO_MEDIUM_10).Width + 2, Height);
                    ItemLengt += _Teile[0].ItemRect.Width;
                    for (int i = 1; i < _Teile.Count; i++)
                    {

                        _Teile[i].ItemRect = new Rectangle(_Teile[i - 1].ItemRect.Right, _Teile[i - 1].ItemRect.Y, (int)g.MeasureString(_Teile[i].Text + (i == _Teile.Count - 1 ? "" : _Trennzeichen) + 5, SkinManager.ROBOTO_MEDIUM_10).Width + 2, Height);
                        ItemLengt += _Teile[i].ItemRect.Width;
                    }

                    if (TabOffset != 0)
                    {
                        Rectangle CurrentTab = _Teile[0].ItemRect;
                        CurrentTab = new Rectangle(CurrentTab.X + TabOffset, CurrentTab.Y, CurrentTab.Width, CurrentTab.Height);

                        _Teile[0].ItemRect = CurrentTab;
                        for (int i = 1; i < _Teile.Count; i++)
                        {
                            CurrentTab = _Teile[i].ItemRect;
                            CurrentTab = new Rectangle(CurrentTab.X + TabOffset, CurrentTab.Y, CurrentTab.Width, CurrentTab.Height);
                            _Teile[i].ItemRect = CurrentTab;
                        }
                    }
                }
            }
            Invalidate();
        }

    }

    /// <summary>
    /// Class BreadCrumbItem.
    /// </summary>
    [Serializable]
    public class BreadCrumbItem
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text{get;set;}
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public Object Tag { get; set; }
        /// <summary>
        /// The item rect
        /// </summary>
        public Rectangle ItemRect;
        /// <summary>
        /// Initializes a new instance of the <see cref="BreadCrumbItem"/> class.
        /// </summary>
        public BreadCrumbItem()
        {
            ItemRect = new Rectangle();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BreadCrumbItem"/> class.
        /// </summary>
        /// <param name="pText">The p text.</param>
        public BreadCrumbItem(string pText)
        {
            Text = pText;
            ItemRect = new Rectangle();
        }


    }

}
