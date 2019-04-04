// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialDropDownDatePicker.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Drawing;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialDropDownDatePicker.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.Controls.DropDownControl" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public partial class MaterialDropDownDatePicker : DropDownControl, IMaterialControl
    {
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
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : Parent.BackColor; } set { } }


        /// <summary>
        /// The object date control
        /// </summary>
        private MaterialDatePicker objDateControl;
        /// <summary>
        /// The date
        /// </summary>
        private DateTime _Date;
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date {get { return _Date;}
                set { _Date = value; objDateControl.Date = _Date;
                Text = _Date.ToShortDateString();
                }
            }
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDropDownDatePicker"/> class.
        /// </summary>
        public MaterialDropDownDatePicker()
        {
            InitializeComponent();
            objDateControl = new MaterialDatePicker();
            Date = DateTime.Now;
            objDateControl.onDateChanged += objDateControl_onDateChanged;
            InitializeDropDown(objDateControl);
        }

        /// <summary>
        /// Objects the date control on date changed.
        /// </summary>
        /// <param name="newDateTime">The new date time.</param>
        void objDateControl_onDateChanged(DateTime newDateTime)
        {
            _Date = newDateTime;
            Text = newDateTime.ToShortDateString();

        }

    }
}
