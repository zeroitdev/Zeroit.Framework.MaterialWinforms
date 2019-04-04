// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialDialog.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialDialog.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.Controls.MaterialForm" />
    public class MaterialDialog : MaterialForm
    {
        /// <summary>
        /// The cs dropshadow
        /// </summary>
        private const int CS_DROPSHADOW = 0x00020000;
        /// <summary>
        /// The font manager
        /// </summary>
        private FontManager _FontManager;
        /// <summary>
        /// The MSG box
        /// </summary>
        private static MaterialDialog _msgBox;
        /// <summary>
        /// The FLP buttons
        /// </summary>
        private FlowLayoutPanel _flpButtons = new FlowLayoutPanel();
        /// <summary>
        /// The button collection
        /// </summary>
        private List<MaterialFlatButton> _buttonCollection = new List<MaterialFlatButton>();
        /// <summary>
        /// The button result
        /// </summary>
        private static DialogResult _buttonResult = new DialogResult();
        /// <summary>
        /// The PNL top
        /// </summary>
        private Panel pnl_Top;
        /// <summary>
        /// The label message
        /// </summary>
        private MaterialLabel lbl_Message;
        /// <summary>
        /// The label title
        /// </summary>
        private Label lbl_Title;
        /// <summary>
        /// The PNL footer
        /// </summary>
        private Panel pnl_Footer;
        /// <summary>
        /// The PNL message
        /// </summary>
        private Panel pnl_Message;
        /// <summary>
        /// The timer
        /// </summary>
        private static Timer _timer;


        /// <summary>
        /// Messages the beep.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool MessageBeep(uint type);

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDialog"/> class.
        /// </summary>
        /// <param name="pColor">Color of the p.</param>
        private MaterialDialog(MaterialSkinManager pColor)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = pColor.GetCardsColor();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Padding = new System.Windows.Forms.Padding(0);
            this.Width = 400;
            this.SkinManager.ColorScheme = pColor.ColorScheme;
            this.SkinManager.Theme = pColor.Theme;
            TopMost = true;
            _FontManager = new FontManager();

            InitializeComponent();



            _flpButtons.FlowDirection = FlowDirection.RightToLeft;
            _flpButtons.Dock = DockStyle.Fill;
            pnl_Footer.Controls.Add(_flpButtons);
            lbl_Title.ForeColor = SkinManager.ColorScheme.AccentColor;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Visible = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {

            base.OnClosing(e);
        }

        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Show(string message)
        {
            _msgBox = new MaterialDialog(MaterialSkinManager.Instance);
            _msgBox.lbl_Message.Text = message;
            _msgBox.ShowDialog();
            MaterialDialog.InitButtons(MaterialDialog.Buttons.OK);
        }

        /// <summary>
        /// Shows the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="pContent">Content of the p.</param>
        /// <returns>DialogResult.</returns>
        public static DialogResult Show(string title, UserControl pContent)
        {
            _msgBox = new MaterialDialog(MaterialSkinManager.Instance);
            pContent.Location = new Point(0, 0);
            _msgBox.pnl_Message.Controls.Add(pContent);
            _msgBox.Width = pContent.Width;
            _msgBox.pnl_Message.Location = new Point(0, _msgBox.pnl_Message.Location.Y);
            _msgBox.Height = _msgBox.pnl_Footer.Height + 5 + pContent.Height;
            _msgBox.lbl_Title.Text = title;
            _msgBox.pnl_Message.Size = pContent.Size;
            _msgBox.pnl_Top.Size = new Size(pContent.Size.Width, pContent.Size.Height + _msgBox.lbl_Title.Height);
            _msgBox.Size = new Size(pContent.Width, _msgBox.pnl_Top.Height + _msgBox.pnl_Footer.Height);
            MaterialDialog.InitButtons(MaterialDialog.Buttons.OK);
            return _buttonResult;
        }

        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        public static void Show(string message, string title)
        {
            _msgBox = new MaterialDialog(MaterialSkinManager.Instance);
            _msgBox.SkinManager.ColorScheme = MaterialSkinManager.Instance.ColorScheme;
            _msgBox.lbl_Message.Text = message;
            _msgBox.lbl_Title.Text = title;
            _msgBox.ShowDialog();
        }

        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns>DialogResult.</returns>
        public static DialogResult Show(string message, string title, Buttons buttons)
        {
            _msgBox = new MaterialDialog(MaterialSkinManager.Instance);
            _msgBox.lbl_Message.Text = message;
            _msgBox.lbl_Title.Text = title;

            MaterialDialog.InitButtons(buttons);

            _msgBox.ShowDialog();
            MessageBeep(0);
            return _buttonResult;
        }

        /// <summary>
        /// Shows the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="pContent">Content of the p.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns>DialogResult.</returns>
        public static DialogResult Show(string title, UserControl pContent, Buttons buttons)
        {
            _msgBox = new MaterialDialog(MaterialSkinManager.Instance);
            pContent.Location = new Point(0, 0);
            _msgBox.pnl_Message.Controls.Add(pContent);
            _msgBox.Width = pContent.Width;
            _msgBox.pnl_Message.Location = new Point(0, _msgBox.pnl_Message.Location.Y);
            _msgBox.Height = _msgBox.pnl_Footer.Height + 5 + pContent.Height;
            _msgBox.lbl_Title.Text = title;
            _msgBox.pnl_Message.Size = pContent.Size;
            _msgBox.pnl_Top.Size = new Size(pContent.Size.Width, pContent.Size.Height + _msgBox.lbl_Title.Height);
            _msgBox.Size = new Size(pContent.Width, _msgBox.pnl_Top.Height + _msgBox.pnl_Footer.Height);

            MaterialDialog.InitButtons(buttons);
            _msgBox.ShowDialog();

            return _buttonResult;
        }

        /// <summary>
        /// Shows the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="pContent">Content of the p.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <returns>DialogResult.</returns>
        public static DialogResult Show(string title, UserControl pContent, Buttons buttons, Icon icon)
        {
            _msgBox = new MaterialDialog(MaterialSkinManager.Instance);
            pContent.Location = new Point(0, 0);
            _msgBox.pnl_Message.Controls.Add(pContent);
            _msgBox.Width = pContent.Width;
            _msgBox.pnl_Message.Location = new Point(0, _msgBox.pnl_Message.Location.Y);
            _msgBox.Height = _msgBox.pnl_Footer.Height + 5 + pContent.Height;
            _msgBox.lbl_Title.Text = title;
            _msgBox.pnl_Message.Size = pContent.Size;
            _msgBox.pnl_Top.Size = new Size(pContent.Size.Width, pContent.Size.Height + _msgBox.lbl_Title.Height);
            _msgBox.Size = new Size(pContent.Width, _msgBox.pnl_Top.Height + _msgBox.pnl_Footer.Height);

            MaterialDialog.InitButtons(buttons);
            MaterialDialog.InitIcon(icon);
            _msgBox.ShowDialog();
            return _buttonResult;
        }

        /// <summary>
        /// Shows the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="pContent">Content of the p.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="style">The style.</param>
        /// <returns>DialogResult.</returns>
        public static DialogResult Show(string title, UserControl pContent, Buttons buttons, Icon icon, AnimateStyle style)
        {
            _msgBox = new MaterialDialog(MaterialSkinManager.Instance);
            pContent.Location = new Point(0, 0);
            _msgBox.pnl_Message.Controls.Add(pContent);
            _msgBox.Width = pContent.Width;
            _msgBox.pnl_Message.Location = new Point(0, _msgBox.pnl_Message.Location.Y);
            _msgBox.Height = _msgBox.pnl_Footer.Height + 5 + pContent.Height;
            _msgBox.lbl_Title.Text = title;
            _msgBox.pnl_Message.Size = pContent.Size;
            _msgBox.pnl_Top.Size = new Size(pContent.Size.Width, pContent.Size.Height + _msgBox.lbl_Title.Height);
            _msgBox.Size = new Size(pContent.Width, _msgBox.pnl_Top.Height + _msgBox.pnl_Footer.Height);

            MaterialDialog.InitButtons(buttons);
            MaterialDialog.InitIcon(icon);


            _timer = new Timer();
            Size formSize = _msgBox.Size;

            switch (style)
            {
                case MaterialDialog.AnimateStyle.SlideDown:
                    _msgBox.Size = new Size(formSize.Width, 0);
                    _timer.Interval = 1;
                    _timer.Tag = new AnimateMsgBox(formSize, style);
                    break;

                case MaterialDialog.AnimateStyle.FadeIn:
                    _msgBox.Size = formSize;
                    _msgBox.Opacity = 0;
                    _timer.Interval = 20;
                    _timer.Tag = new AnimateMsgBox(formSize, style);
                    break;

                case MaterialDialog.AnimateStyle.ZoomIn:
                    _msgBox.Size = new Size(formSize.Width + 100, formSize.Height + 100);
                    _timer.Tag = new AnimateMsgBox(formSize, style);
                    _timer.Interval = 1;
                    break;
            }

            _timer.Tick += timer_Tick;
            _timer.Start();


            _msgBox.ShowDialog();
            return _buttonResult;
        }
        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <returns>DialogResult.</returns>
        public static DialogResult Show(string message, string title, Buttons buttons, Icon icon)
        {
            _msgBox = new MaterialDialog(MaterialSkinManager.Instance);
            _msgBox.lbl_Message.Text = message;
            _msgBox.lbl_Title.Text = title;

            MaterialDialog.InitButtons(buttons);
            MaterialDialog.InitIcon(icon);
            _msgBox.ShowDialog();
            return _buttonResult;
        }

        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="style">The style.</param>
        /// <returns>DialogResult.</returns>
        public static DialogResult Show(string message, string title, Buttons buttons, Icon icon, AnimateStyle style)
        {
            _msgBox = new MaterialDialog(MaterialSkinManager.Instance);
            _msgBox.lbl_Message.Text = message;
            _msgBox.lbl_Title.Text = title;
            _msgBox.Height = 0;

            MaterialDialog.InitButtons(buttons);
            MaterialDialog.InitIcon(icon);

            _timer = new Timer();
            Size formSize = _msgBox.Size;

            switch (style)
            {
                case MaterialDialog.AnimateStyle.SlideDown:
                    _msgBox.Size = new Size(formSize.Width, 0);
                    _timer.Interval = 1;
                    _timer.Tag = new AnimateMsgBox(formSize, style);
                    break;

                case MaterialDialog.AnimateStyle.FadeIn:
                    _msgBox.Size = formSize;
                    _msgBox.Opacity = 0;
                    _timer.Interval = 20;
                    _timer.Tag = new AnimateMsgBox(formSize, style);
                    break;

                case MaterialDialog.AnimateStyle.ZoomIn:
                    _msgBox.Size = new Size(formSize.Width + 100, formSize.Height + 100);
                    _timer.Tag = new AnimateMsgBox(formSize, style);
                    _timer.Interval = 1;
                    break;
            }

            _timer.Tick += timer_Tick;
            _timer.Start();

            _msgBox.ShowDialog();
            return _buttonResult;
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void timer_Tick(object sender, EventArgs e)
        {
            Timer timer = (Timer)sender;
            AnimateMsgBox animate = (AnimateMsgBox)timer.Tag;

            switch (animate.Style)
            {
                case MaterialDialog.AnimateStyle.SlideDown:
                    if (_msgBox.Height < animate.FormSize.Height)
                    {
                        _msgBox.Height += 17;
                        _msgBox.Invalidate();
                    }
                    else
                    {
                        _timer.Stop();
                        _timer.Dispose();
                    }
                    break;

                case MaterialDialog.AnimateStyle.FadeIn:
                    if (_msgBox.Opacity < 1)
                    {
                        _msgBox.Opacity += 0.1;
                        _msgBox.Invalidate();
                    }
                    else
                    {
                        _timer.Stop();
                        _timer.Dispose();
                    }
                    break;

                case MaterialDialog.AnimateStyle.ZoomIn:
                    if (_msgBox.Width > animate.FormSize.Width)
                    {
                        _msgBox.Width -= 17;
                        _msgBox.Invalidate();
                    }
                    if (_msgBox.Height > animate.FormSize.Height)
                    {
                        _msgBox.Height -= 17;
                        _msgBox.Invalidate();
                    }
                    break;
            }
        }

        /// <summary>
        /// Initializes the buttons.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        private static void InitButtons(Buttons buttons)
        {
            switch (buttons)
            {
                case MaterialDialog.Buttons.AbortRetryIgnore:
                    _msgBox.InitAbortRetryIgnoreButtons();
                    break;

                case MaterialDialog.Buttons.OK:
                    _msgBox.InitOKButton();
                    break;

                case MaterialDialog.Buttons.OKCancel:
                    _msgBox.InitOKCancelButtons();
                    break;

                case MaterialDialog.Buttons.RetryCancel:
                    _msgBox.InitRetryCancelButtons();
                    break;

                case MaterialDialog.Buttons.YesNo:
                    _msgBox.InitYesNoButtons();
                    break;

                case MaterialDialog.Buttons.YesNoCancel:
                    _msgBox.InitYesNoCancelButtons();
                    break;
            }

            foreach (Button btn in _msgBox._buttonCollection)
            {
                btn.Font = new FontManager().Roboto_Medium10;
                btn.Padding = new Padding(3);
                btn.FlatStyle = FlatStyle.Flat;
                btn.Height = 30;
                _msgBox._flpButtons.Controls.Add(btn);
            }
        }

        /// <summary>
        /// Initializes the icon.
        /// </summary>
        /// <param name="icon">The icon.</param>
        private static void InitIcon(Icon icon)
        {
            switch (icon)
            {
                case MaterialDialog.Icon.Application:
                    _msgBox.lbl_Title.ForeColor = ((int)Accent.Green200).ToColor();
                    break;

                case MaterialDialog.Icon.Exclamation:
                    _msgBox.lbl_Title.ForeColor = ((int)Accent.Red200).ToColor();
                    break;

                case MaterialDialog.Icon.Error:
                    _msgBox.lbl_Title.ForeColor = ((int)Accent.Red200).ToColor();
                    break;

                case MaterialDialog.Icon.Info:
                    _msgBox.lbl_Title.ForeColor = ((int)Accent.Blue200).ToColor();
                    break;

                case MaterialDialog.Icon.Question:
                    _msgBox.lbl_Title.ForeColor = ((int)Accent.Blue200).ToColor();
                    break;

                case MaterialDialog.Icon.Shield:
                    _msgBox.lbl_Title.ForeColor = ((int)Accent.Yellow200).ToColor();
                    break;

                case MaterialDialog.Icon.Warning:
                    _msgBox.lbl_Title.ForeColor = ((int)Accent.Orange200).ToColor();
                    break;
            }
        }

        /// <summary>
        /// Initializes the abort retry ignore buttons.
        /// </summary>
        private void InitAbortRetryIgnoreButtons()
        {
            MaterialFlatButton btnAbort = new MaterialFlatButton();
            btnAbort.Text = "Abbrechen";
            btnAbort.Accent = true;
            btnAbort.Tag = "Abort";
            btnAbort.Click += ButtonClick;

            MaterialFlatButton btnRetry = new MaterialFlatButton();
            btnRetry.Text = "Erneut versuchen";
            btnRetry.ForeColor = Color.DarkGray;
            btnRetry.Tag = "Retry";
            btnRetry.Click += ButtonClick;

            MaterialFlatButton btnIgnore = new MaterialFlatButton();
            btnIgnore.Text = "Ignorieren";
            btnIgnore.Tag = "Ignore";
            btnIgnore.ForeColor = Color.DarkGray;
            btnIgnore.Click += ButtonClick;

            this._buttonCollection.Add(btnIgnore);
            this._buttonCollection.Add(btnAbort);
            this._buttonCollection.Add(btnRetry);

        }

        /// <summary>
        /// Initializes the ok button.
        /// </summary>
        private void InitOKButton()
        {
            MaterialFlatButton btnOK = new MaterialFlatButton();
            btnOK.Text = "OK";
            btnOK.Tag = "Ok";
            btnOK.Accent = true;
            btnOK.Click += ButtonClick;

            this._buttonCollection.Add(btnOK);
        }

        /// <summary>
        /// Initializes the ok cancel buttons.
        /// </summary>
        private void InitOKCancelButtons()
        {
            MaterialFlatButton btnOK = new MaterialFlatButton();
            btnOK.Text = "OK";
            btnOK.Tag = "Ok";
            btnOK.Accent = true;
            btnOK.Click += ButtonClick;

            MaterialFlatButton btnCancel = new MaterialFlatButton();
            btnCancel.Text = "Abbrechen";
            btnCancel.Tag = "Cancel";
            btnCancel.ForeColor = Color.DarkGray;
            btnCancel.Click += ButtonClick;

            this._buttonCollection.Add(btnOK);
            this._buttonCollection.Add(btnCancel);

        }

        /// <summary>
        /// Initializes the retry cancel buttons.
        /// </summary>
        private void InitRetryCancelButtons()
        {
            MaterialFlatButton btnRetry = new MaterialFlatButton();
            btnRetry.Text = "Erneut versuchen";
            btnRetry.Tag = "Retry";
            btnRetry.Accent = true;
            btnRetry.Click += ButtonClick;

            MaterialFlatButton btnCancel = new MaterialFlatButton();
            btnCancel.Text = "Abbrechen";
            btnCancel.Tag = "Cancel";
            btnCancel.ForeColor = Color.DarkGray;
            btnCancel.Click += ButtonClick;

            this._buttonCollection.Add(btnRetry);
            this._buttonCollection.Add(btnCancel);

        }

        /// <summary>
        /// Initializes the yes no buttons.
        /// </summary>
        private void InitYesNoButtons()
        {
            MaterialFlatButton btnYes = new MaterialFlatButton();
            btnYes.Text = "Ja";
            btnYes.Tag = "Yes";
            btnYes.Accent = true;
            btnYes.Click += ButtonClick;

            MaterialFlatButton btnNo = new MaterialFlatButton();
            btnNo.Text = "Nein";
            btnNo.Tag = "No";
            btnNo.ForeColor = Color.DarkGray;
            btnNo.Click += ButtonClick;

            this._buttonCollection.Add(btnYes);
            this._buttonCollection.Add(btnNo);

        }

        /// <summary>
        /// Gets the create parameters.
        /// </summary>
        /// <value>The create parameters.</value>
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        /// <summary>
        /// Initializes the yes no cancel buttons.
        /// </summary>
        private void InitYesNoCancelButtons()
        {
            MaterialFlatButton btnYes = new MaterialFlatButton();
            btnYes.Text = "Ja";

            btnYes.Tag = "Yes";
            btnYes.Accent = true;
            btnYes.Click += ButtonClick;

            MaterialFlatButton btnNo = new MaterialFlatButton();
            btnNo.Text = "Nein";

            btnNo.Tag = "No";
            btnNo.ForeColor = Color.DarkGray;
            btnNo.Click += ButtonClick;

            MaterialFlatButton btnCancel = new MaterialFlatButton();
            btnCancel.Text = "Abbrechen";

            btnCancel.Tag = "Cancel";
            btnCancel.ForeColor = Color.DarkGray;
            btnCancel.Click += ButtonClick;

            this._buttonCollection.Add(btnYes);
            this._buttonCollection.Add(btnNo);
            this._buttonCollection.Add(btnCancel);

        }

        /// <summary>
        /// Buttons the click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void ButtonClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Tag.ToString())
            {
                case "Abort":
                    _buttonResult = DialogResult.Abort;
                    break;

                case "Retry":
                    _buttonResult = DialogResult.Retry;
                    break;

                case "Ignore":
                    _buttonResult = DialogResult.Ignore;
                    break;

                case "Ok":
                    _buttonResult = DialogResult.OK;
                    break;

                case "Cancel":
                    _buttonResult = DialogResult.Cancel;
                    break;

                case "Yes":
                    _buttonResult = DialogResult.Yes;
                    break;

                case "No":
                    _buttonResult = DialogResult.No;
                    break;
            }

            _msgBox.Dispose();
        }


        /// <summary>
        /// Enum Buttons
        /// </summary>
        public enum Buttons
        {
            /// <summary>
            /// The abort retry ignore
            /// </summary>
            AbortRetryIgnore = 1,
            /// <summary>
            /// The ok
            /// </summary>
            OK = 2,
            /// <summary>
            /// The ok cancel
            /// </summary>
            OKCancel = 3,
            /// <summary>
            /// The retry cancel
            /// </summary>
            RetryCancel = 4,
            /// <summary>
            /// The yes no
            /// </summary>
            YesNo = 5,
            /// <summary>
            /// The yes no cancel
            /// </summary>
            YesNoCancel = 6
        }

        /// <summary>
        /// Enum Icon
        /// </summary>
        public enum Icon
        {
            /// <summary>
            /// The application
            /// </summary>
            Application = 1,
            /// <summary>
            /// The exclamation
            /// </summary>
            Exclamation = 2,
            /// <summary>
            /// The error
            /// </summary>
            Error = 3,
            /// <summary>
            /// The warning
            /// </summary>
            Warning = 4,
            /// <summary>
            /// The information
            /// </summary>
            Info = 5,
            /// <summary>
            /// The question
            /// </summary>
            Question = 6,
            /// <summary>
            /// The shield
            /// </summary>
            Shield = 7,
            /// <summary>
            /// The search
            /// </summary>
            Search = 8
        }

        /// <summary>
        /// Enum AnimateStyle
        /// </summary>
        public enum AnimateStyle
        {
            /// <summary>
            /// The slide down
            /// </summary>
            SlideDown = 1,
            /// <summary>
            /// The fade in
            /// </summary>
            FadeIn = 2,
            /// <summary>
            /// The zoom in
            /// </summary>
            ZoomIn = 3
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_Top = new System.Windows.Forms.Panel();
            this.pnl_Message = new System.Windows.Forms.Panel();
            this.lbl_Message = new Zeroit.Framework.MaterialWinforms.Controls.MaterialLabel();
            this.lbl_Title = new System.Windows.Forms.Label();
            this.pnl_Footer = new System.Windows.Forms.Panel();
            this.pnl_Top.SuspendLayout();
            this.pnl_Message.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_Top
            // 
            this.pnl_Top.Controls.Add(this.pnl_Message);
            this.pnl_Top.Controls.Add(this.lbl_Title);
            this.pnl_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_Top.Location = new System.Drawing.Point(0, 0);
            this.pnl_Top.Name = "pnl_Top";
            this.pnl_Top.Padding = new System.Windows.Forms.Padding(10);
            this.pnl_Top.Size = new System.Drawing.Size(350, 250);
            this.pnl_Top.TabIndex = 1;
            this.pnl_Top.MouseDown += lbl_Title_MouseDown;
            // 
            // pnl_Message
            // 
            this.pnl_Message.AutoScroll = true;
            this.pnl_Message.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnl_Message.Controls.Add(this.lbl_Message);
            this.pnl_Message.Location = new System.Drawing.Point(12, 41);
            this.pnl_Message.MinimumSize = new System.Drawing.Size(330, 200);
            this.pnl_Message.Name = "pnl_Message";
            this.pnl_Message.Padding = new System.Windows.Forms.Padding(5);
            this.pnl_Message.Size = new System.Drawing.Size(330, 200);
            this.pnl_Message.TabIndex = 2;
            this.pnl_Message.Paint += new System.Windows.Forms.PaintEventHandler(this.pnl_Message_Paint);
            // 
            // lbl_Message
            // 
            this.lbl_Message.AutoSize = true;
            this.lbl_Message.Depth = 0;
            this.lbl_Message.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_Message.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_Message.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_Message.Location = new System.Drawing.Point(5, 5);
            this.lbl_Message.MaximumSize = new System.Drawing.Size(330, 5000);
            this.lbl_Message.MouseState = Zeroit.Framework.MaterialWinforms.MouseState.HOVER;
            this.lbl_Message.Name = "lbl_Message";
            this.lbl_Message.Size = new System.Drawing.Size(0, 19);
            this.lbl_Message.TabIndex = 1;
            // 
            // lbl_Title
            // 
            this.lbl_Title.AutoSize = true;
            this.lbl_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_Title.Font = new System.Drawing.Font("Roboto", 12F);
            this.lbl_Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_Title.Location = new System.Drawing.Point(10, 10);
            this.lbl_Title.Name = "lbl_Title";
            this.lbl_Title.Size = new System.Drawing.Size(115, 20);
            this.lbl_Title.TabIndex = 0;
            this.lbl_Title.Text = "materialLabel1";
            this.lbl_Title.MouseDown += lbl_Title_MouseDown;
            // 
            // pnl_Footer
            // 
            this.pnl_Footer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_Footer.Location = new System.Drawing.Point(0, 250);
            this.pnl_Footer.Name = "pnl_Footer";
            this.pnl_Footer.Size = new System.Drawing.Size(350, 50);
            this.pnl_Footer.TabIndex = 2;
            this.pnl_Footer.MouseDown += lbl_Title_MouseDown;
            // 
            // MaterialDialog
            // 
            this.ClientSize = new System.Drawing.Size(350, 300);
            this.Controls.Add(this.pnl_Footer);
            this.Controls.Add(this.pnl_Top);
            this.Name = "MaterialDialog";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.pnl_Top.ResumeLayout(false);
            this.pnl_Top.PerformLayout();
            this.pnl_Message.ResumeLayout(false);
            this.pnl_Message.PerformLayout();
            this.ResumeLayout(false);
            this.MouseDown += lbl_Title_MouseDown;

        }


        /// <summary>
        /// Handles the MouseDown event of the lbl_Title control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        void lbl_Title_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        /// <summary>
        /// Handles the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(SkinManager.GetCardsColor());
        }

        /// <summary>
        /// Handles the Paint event of the pnl_Message control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        private void pnl_Message_Paint(object sender, PaintEventArgs e)
        {

        }


    }

    /// <summary>
    /// Class AnimateMsgBox.
    /// </summary>
    class AnimateMsgBox
    {
        /// <summary>
        /// The form size
        /// </summary>
        public Size FormSize;
        /// <summary>
        /// The style
        /// </summary>
        public MaterialDialog.AnimateStyle Style;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimateMsgBox"/> class.
        /// </summary>
        /// <param name="formSize">Size of the form.</param>
        /// <param name="style">The style.</param>
        public AnimateMsgBox(Size formSize, MaterialDialog.AnimateStyle style)
        {
            this.FormSize = formSize;
            this.Style = style;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimateMsgBox"/> class.
        /// </summary>
        /// <param name="formSize">Size of the form.</param>
        public AnimateMsgBox(Size formSize)
        {
            this.FormSize = formSize;
        }
    }
}
