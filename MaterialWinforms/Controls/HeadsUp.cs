// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="HeadsUp.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Zeroit.Framework.MaterialWinforms.Animations;
using System.Drawing;
using System.Windows.Forms;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class HeadsUp.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.Controls.MaterialForm" />
    public class HeadsUp : MaterialForm
    {
        /// <summary>
        /// The animation manager
        /// </summary>
        private AnimationManager _AnimationManager;
        /// <summary>
        /// The schliessen
        /// </summary>
        private bool Schliessen = false;
        /// <summary>
        /// The start height
        /// </summary>
        private int StartHeight;
        /// <summary>
        /// The button panel
        /// </summary>
        private FlowLayoutPanel ButtonPanel;
        /// <summary>
        /// The close animation
        /// </summary>
        private bool CloseAnimation = false;
        /// <summary>
        /// The start x
        /// </summary>
        private int StartX;

        /// <summary>
        /// Gets the color of the back.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new Color BackColor
        {
            get
            {
                return SkinManager.GetApplicationBackgroundColor();
            }
        }

        /// <summary>
        /// The titel
        /// </summary>
        private String _Titel;
        /// <summary>
        /// The Titel of the Form
        /// </summary>
        /// <value>The titel.</value>
        public String Titel
        {
            get
            {
                return _Titel;
            }
            set
            {
                _Titel = value;
                TitelLabel.Text = _Titel;
                TextLabel.Location = new Point(TextLabel.Location.X, TitelLabel.Bottom + 8);
            }
        }


        /// <summary>
        /// The text
        /// </summary>
        private String _Text;
        /// <summary>
        /// The Text which gets displayed as the Content
        /// </summary>
        /// <value>The text.</value>
        public new String Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                TextLabel.Text = _Text;
            }
        }

        /// <summary>
        /// The Collection for the Buttons
        /// </summary>
        /// <value>The buttons.</value>
        public ObservableCollection<MaterialFlatButton> Buttons {get;set;}

        /// <summary>
        /// The Content Labels
        /// </summary>
        private MaterialLabel TitelLabel, TextLabel;

        /// <summary>
        /// Constructer Setting up the Layout
        /// </summary>
        public HeadsUp()
        {
            SkinManager.AddFormToManage(this);
            TopMost = true;
            ShowInTaskbar = false;
            Width = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.4);
            _AnimationManager = new AnimationManager();
            _AnimationManager.AnimationType = AnimationType.EaseOut;
            _AnimationManager.Increment = 0.03;
            _AnimationManager.OnAnimationProgress += _AnimationManager_OnAnimationProgress;
            TitelLabel = new MaterialLabel();
            TitelLabel.AutoSize = true;
            TextLabel = new MaterialLabel();
            ButtonPanel = new FlowLayoutPanel();
            ButtonPanel.FlowDirection = FlowDirection.RightToLeft;
            ButtonPanel.Height = 40;
            //ButtonPanel.AutoScroll = true;
            ButtonPanel.Dock = DockStyle.Bottom;
            //ButtonPanel.BackColor = BackColor;
            Controls.Add(ButtonPanel);
            TitelLabel.Location = new Point(20, 10);
            TextLabel.Location = new Point(20, TitelLabel.Bottom + 5);
            TextLabel.AutoSize = true;
            TextLabel.Resize += TextLabel_Resize;
            TextLabel.MaximumSize = new Size(Width, Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.2));
            Controls.Add(TitelLabel);
            Controls.Add(TextLabel);
            Buttons = new ObservableCollection<MaterialFlatButton>();
            Buttons.CollectionChanged += Buttons_CollectionChanged;
        }

        /// <summary>
        /// Sets up The Buttons
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        void Buttons_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ButtonPanel.SuspendLayout();
            ButtonPanel.Controls.Clear();
            if (Buttons.Count > 0) { 
            ButtonPanel.Controls.AddRange(Buttons.ToArray());
            //    ButtonPanel.Height = Buttons.First().Height;
            }
            ButtonPanel.ResumeLayout();
        }

        /// <summary>
        /// Corrects the Size and Location after the Content changes
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void TextLabel_Resize(object sender, EventArgs e)
        {
            Height = TextLabel.Bottom + 10 + ButtonPanel.Height;
            StartHeight = Height;
        }

        /// <summary>
        /// Sets up the Starting Location and starts the Animation
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Location = new Point(Convert.ToInt32((Screen.PrimaryScreen.Bounds.Width-Width)/2), -StartHeight);
            StartX = Location.X;
            _AnimationManager.StartNewAnimation(AnimationDirection.In);
        }

        /// <summary>
        /// Animates the Form slides
        /// </summary>
        /// <param name="sender">The sender.</param>
        void _AnimationManager_OnAnimationProgress(object sender)
        {
            if (CloseAnimation)
            {
                Opacity =  _AnimationManager.GetProgress();
                Location = new Point(StartX+Convert.ToInt32((Screen.PrimaryScreen.Bounds.Width-StartX-Width)*(1-_AnimationManager.GetProgress())), Location.Y);
            }
            else
            {
                Location = new Point(Location.X, -StartHeight + Convert.ToInt32((StartHeight * _AnimationManager.GetProgress())));
            }
        }

        /// <summary>
        /// Ovverides the Paint to create the solid colored backcolor
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
        }

        /// <summary>
        /// Overrides the Closing Event to Animate the Slide Out
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !Schliessen;
            if (!Schliessen)
            {
                CloseAnimation = true;
                _AnimationManager.Increment = 0.06;
                _AnimationManager.OnAnimationFinished += _AnimationManager_OnAnimationFinished;
                _AnimationManager.StartNewAnimation(AnimationDirection.Out);
            }
            base.OnClosing(e);
        }

        /// <summary>
        /// Closes the Form after the pull out animation
        /// </summary>
        /// <param name="sender">The sender.</param>
        void _AnimationManager_OnAnimationFinished(object sender)
        {
            Schliessen = true;
            Close();
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(347, 300);
            this.Name = "HeadsUp";
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Prevents the Form from beeing dragged
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }

    }
}
