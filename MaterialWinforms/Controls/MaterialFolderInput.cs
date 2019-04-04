// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialFolderInput.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Zeroit.Framework.MaterialWinforms.Controls
{

    /// <summary>
    /// Class MaterialFolderInput.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    [DefaultEvent("TextChanged")]
public class MaterialFolderInput : Control, IMaterialControl
{

        #region  Variables

        /// <summary>
        /// The in put BTN
        /// </summary>
        Button InPutBTN = new Button();
        /// <summary>
        /// The material tb
        /// </summary>
        MaterialSingleLineTextField MaterialTB = new MaterialSingleLineTextField();

        /// <summary>
        /// The aln type
        /// </summary>
        HorizontalAlignment ALNType;
        /// <summary>
        /// The maxchars
        /// </summary>
        int maxchars = 32767;
        /// <summary>
        /// The read only
        /// </summary>
        bool readOnly;
        /// <summary>
        /// The previous read only
        /// </summary>
        bool previousReadOnly;
        /// <summary>
        /// The is password masked
        /// </summary>
        bool isPasswordMasked = false;
        /// <summary>
        /// The enable
        /// </summary>
        bool Enable = true;

        /// <summary>
        /// The animation timer
        /// </summary>
        Timer AnimationTimer = new Timer { Interval = 1 };
        /// <summary>
        /// The font
        /// </summary>
        FontManager font = new FontManager();

        /// <summary>
        /// The dialog
        /// </summary>
        public FolderBrowserDialog Dialog;
        /// <summary>
        /// The filter
        /// </summary>
        string filter = @"All Files (*.*)|*.*";

        /// <summary>
        /// The focus
        /// </summary>
        bool Focus = false;
        /// <summary>
        /// The mouse over
        /// </summary>
        bool mouseOver = false;

        /// <summary>
        /// The size animation
        /// </summary>
        float SizeAnimation = 0;
        /// <summary>
        /// The size inc decimal
        /// </summary>
        float SizeInc_Dec;

        /// <summary>
        /// The point animation
        /// </summary>
        float PointAnimation;
        /// <summary>
        /// The point inc decimal
        /// </summary>
        float PointInc_Dec;

        /// <summary>
        /// The font color
        /// </summary>
        string fontColor = "#999999";
        /// <summary>
        /// The focus color
        /// </summary>
        string focusColor = "#508ef5";

        /// <summary>
        /// The enabled focused color
        /// </summary>
        Color EnabledFocusedColor;
        /// <summary>
        /// The enabled string color
        /// </summary>
        Color EnabledStringColor;

        /// <summary>
        /// The enabled in put color
        /// </summary>
        Color EnabledInPutColor = ColorTranslator.FromHtml("#acacac");
        /// <summary>
        /// The enabled un focused color
        /// </summary>
        Color EnabledUnFocusedColor = ColorTranslator.FromHtml("#dbdbdb");

        /// <summary>
        /// The disabled input color
        /// </summary>
        Color DisabledInputColor = ColorTranslator.FromHtml("#d1d2d4");
        /// <summary>
        /// The disabled un focused color
        /// </summary>
        Color DisabledUnFocusedColor = ColorTranslator.FromHtml("#e9ecee");
        /// <summary>
        /// The disabled string color
        /// </summary>
        Color DisabledStringColor = ColorTranslator.FromHtml("#babbbd");

        /// <summary>
        /// Gets the skin manager.
        /// </summary>
        /// <value>The skin manager.</value>
        [Browsable(false)]
    public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }

        #endregion

        #region  Properties

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value>The text alignment.</value>
        public HorizontalAlignment TextAlignment
    {
        get
        {
            return ALNType;
        }
        set
        {
            ALNType = value;
            Invalidate();
        }
    }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        [Browsable(false)]
    public int Depth { get; set; }
        /// <summary>
        /// Gets or sets the state of the mouse.
        /// </summary>
        /// <value>The state of the mouse.</value>
        [Browsable(false)]
    public MouseState MouseState { get; set; }
        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        [Browsable(false)]
    [Category("Behavior")]
    public int MaxLength
    {
        get
        {
            return maxchars;
        }
        set
        {
            maxchars = value;
            MaterialTB.MaxLength = MaxLength;
            Invalidate();
        }
    }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : typeof(IShadowedMaterialControl).IsAssignableFrom(Parent.GetType()) ? ((IMaterialControl)Parent).BackColor : Parent.BackColor; } }

        /// <summary>
        /// Gets or sets a value indicating whether [use system password character].
        /// </summary>
        /// <value><c>true</c> if [use system password character]; otherwise, <c>false</c>.</value>
        [Category("Behavior")]
    public bool UseSystemPasswordChar
    {
        get
        {
            return isPasswordMasked;
        }
        set
        {
            MaterialTB.UseSystemPasswordChar = UseSystemPasswordChar;
            isPasswordMasked = value;
            Invalidate();
        }
    }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        [Category("Behavior")]
    public bool ReadOnly
    {
        get
        {
            return readOnly;
        }
        set
        {
            readOnly = value;
            if (MaterialTB != null)
            {
                MaterialTB.ReadOnly = value;
            }
        }
    }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        [Category("Behavior")]
    public bool IsEnabled
    {
        get { return Enable; }
        set
        {
            Enable = value;

            if (IsEnabled)
            {
                readOnly = previousReadOnly;
                MaterialTB.ReadOnly = previousReadOnly;
                MaterialTB.ForeColor = EnabledStringColor;
                InPutBTN.Enabled = true;
            }
            else
            {
                previousReadOnly = ReadOnly;
                ReadOnly = true;
                MaterialTB.ForeColor = DisabledStringColor;
                InPutBTN.Enabled = false;
            }

            Invalidate();
        }
    }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        [Category("Behavior")]
    public string Filter
    {
        get { return filter; }
        set
        {
            filter = value;
            Invalidate();
        }
    }

        /// <summary>
        /// Gets or sets the color of the focused.
        /// </summary>
        /// <value>The color of the focused.</value>
        [Category("Appearance")]
    public string FocusedColor
    {
        get { return focusColor; }
        set
        {
            focusColor = value;
            Invalidate();
        }
    }

        /// <summary>
        /// Gets or sets the color of the font.
        /// </summary>
        /// <value>The color of the font.</value>
        [Category("Appearance")]
    public string FontColor
    {
        get { return fontColor; }
        set
        {
            fontColor = value;
            Invalidate();
        }
    }

        /// <summary>
        /// Gets or sets the hint.
        /// </summary>
        /// <value>The hint.</value>
        [Category("Appearance")]
    public string Hint
    {
        get { return MaterialTB.Hint; }
        set
        {
            MaterialTB.Hint = value;
            Invalidate();
        }
    }

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond to user interaction.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        [Browsable(false)]
    public bool Enabled
    {
        get { return base.Enabled; }
        set { base.Enabled = value; }
    }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value>The font.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        [Browsable(false)]
    public Font Font
    {
        get { return base.Font; }
        set { base.Font = value; }
    }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        /// <value>The color of the fore.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        [Browsable(false)]
    public Color ForeColor
    {
        get { return base.ForeColor; }
        set { base.ForeColor = value; }
    }

        #endregion

        #region  Events

        /// <summary>
        /// Handles the <see cref="E:KeyDown" /> event.
        /// </summary>
        /// <param name="Obj">The object.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        protected void OnKeyDown(object Obj, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.A)
        {
            MaterialTB.SelectAll();
            e.SuppressKeyPress = true;
        }
        if (e.Control && e.KeyCode == Keys.C)
        {
            MaterialTB.Copy();
            e.SuppressKeyPress = true;
        }
        if (e.Control && e.KeyCode == Keys.X)
        {
            MaterialTB.Cut();
            e.SuppressKeyPress = true;
        }
    }
        /// <summary>
        /// Browses down.
        /// </summary>
        /// <param name="Obj">The object.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BrowseDown(object Obj, EventArgs e)
    {      
        Dialog = new FolderBrowserDialog();
        DialogResult result = Dialog.ShowDialog();

        if (result == DialogResult.OK && Dialog.SelectedPath != null)
        {
            Text = Dialog.SelectedPath;
        }
        Focus();
    }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnTextChanged(System.EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(System.EventArgs e)
    {
        base.OnGotFocus(e);
        MaterialTB.Focus();
        MaterialTB.SelectionLength = 0;
    }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(System.EventArgs e)
    {
        base.OnResize(e);

        Height = MaterialTB.Height;

        PointAnimation = Width / 2;
        SizeInc_Dec = Width / 18;
        PointInc_Dec = Width / 36;

        MaterialTB.Width = Width - 21;
        InPutBTN.Location = new Point(Width - 21, 1);
        InPutBTN.Size = new Size(21, this.Height - 2);
    }

        #endregion
        /// <summary>
        /// Adds the button.
        /// </summary>
        public void AddButton()
    {
        InPutBTN.Location = new Point(Width - 21, 3);
        InPutBTN.Size = new Size(21, this.Height - 2);

        InPutBTN.ForeColor = Color.FromArgb(255, 255, 255);
        InPutBTN.TextAlign = ContentAlignment.MiddleCenter;
        InPutBTN.BackColor = Color.Transparent;
        
        InPutBTN.TabStop = false;
        InPutBTN.FlatStyle = FlatStyle.Flat;
        InPutBTN.FlatAppearance.MouseOverBackColor = Color.Transparent;
        InPutBTN.FlatAppearance.MouseDownBackColor = Color.Transparent;
        InPutBTN.FlatAppearance.BorderSize = 0;
        
        InPutBTN.MouseDown += BrowseDown;
        InPutBTN.MouseEnter += (sender, args) => mouseOver = true;
        InPutBTN.MouseLeave += (sender, args) => mouseOver = false;
    }
        /// <summary>
        /// Adds the text box.
        /// </summary>
        public void AddTextBox()
    {
        MaterialTB.Text = Text;
        MaterialTB.Location = new Point(0, 1);
        MaterialTB.Size = new Size(Width - 21, 20);

        MaterialTB.Font = font.Roboto_Regular10;
        MaterialTB.UseSystemPasswordChar = UseSystemPasswordChar;       
        
        MaterialTB.KeyDown += OnKeyDown;

        MaterialTB.GotFocus += (sender, args) => Focus = true; AnimationTimer.Start();
        MaterialTB.LostFocus += (sender, args) => Focus = false; AnimationTimer.Start();
    }
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialFolderInput"/> class.
        /// </summary>
        public MaterialFolderInput()
    {
        Width = 300;
        DoubleBuffered = true;       
        previousReadOnly = ReadOnly;

        AddTextBox();
        AddButton();
       
        Controls.Add(MaterialTB);
        Controls.Add(InPutBTN);

        MaterialTB.TextChanged += (sender, args) => Text = MaterialTB.Text;
        base.TextChanged += (sender, args) => MaterialTB.Text = Text;

        AnimationTimer.Tick += new EventHandler(AnimationTick);
    }


        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        base.OnPaint(e);
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        G.Clear(BackColor);

        EnabledStringColor = ColorTranslator.FromHtml(fontColor);
        EnabledFocusedColor = ColorTranslator.FromHtml(focusColor);

        MaterialTB.TextAlign = TextAlignment;
        MaterialTB.ForeColor = IsEnabled ? EnabledStringColor : DisabledStringColor;
        MaterialTB.UseSystemPasswordChar = UseSystemPasswordChar;


        G.DrawLine(new Pen(new SolidBrush(IsEnabled ? SkinManager.GetDividersColor() : DisabledUnFocusedColor)), new Point(0, Height - 1), new Point(Width, Height - 1));
        if (IsEnabled)
        { G.FillRectangle(MaterialTB.Focused() ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetDividersBrush(), PointAnimation, (float)Height - 1, SizeAnimation, MaterialTB.Focused() ? 2 : 1); }


        G.SmoothingMode = SmoothingMode.AntiAlias;
        G.FillEllipse(new SolidBrush(IsEnabled ? mouseOver ? SkinManager.ColorScheme.AccentColor : SkinManager.GetDividersColor() : DisabledInputColor), Width - 5, 24, 4, 4);
        G.FillEllipse(new SolidBrush(IsEnabled ? mouseOver ? SkinManager.ColorScheme.AccentColor : SkinManager.GetDividersColor() : DisabledInputColor), Width - 11, 24, 4, 4);
        G.FillEllipse(new SolidBrush(IsEnabled ? mouseOver ? SkinManager.ColorScheme.AccentColor : SkinManager.GetDividersColor() : DisabledInputColor), Width - 17, 24, 4, 4);

        e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
        G.Dispose();
        B.Dispose();
    }

        /// <summary>
        /// Animations the tick.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void AnimationTick(object sender, EventArgs e)
    {
        if (Focus)
        {
            if (SizeAnimation < Width)
            {
                SizeAnimation += SizeInc_Dec;
                this.Invalidate();
            }

            if (PointAnimation > 0)
            {
                PointAnimation -= PointInc_Dec;
                this.Invalidate();
            }
        }
        else
        {
            if (SizeAnimation > 0)
            {
                SizeAnimation -= SizeInc_Dec;
                this.Invalidate();
            }

            if (PointAnimation < Width / 2)
            {
                PointAnimation += PointInc_Dec;
                this.Invalidate();
            }
        }
    }

}
}


