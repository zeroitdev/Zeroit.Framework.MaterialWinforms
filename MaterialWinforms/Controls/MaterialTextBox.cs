// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialTextBox.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Zeroit.Framework.MaterialWinforms.Animations;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialTextBox.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public class MaterialTextBox : Control, IMaterialControl
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
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>The color of the back.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : typeof(IMaterialControl).IsAssignableFrom(Parent.GetType()) ? ((IMaterialControl)Parent).BackColor : Parent.BackColor; } }

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        /// <value>The text.</value>
        public override string Text { get { return baseTextBox.Text; } set { baseTextBox.Text = value; } }
        /// <summary>
        /// Gets or sets the object that contains data about the control.
        /// </summary>
        /// <value>The tag.</value>
        public new object Tag { get { return baseTextBox.Tag; } set { baseTextBox.Tag = value; } }
        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        public new int MaxLength { get { return baseTextBox.MaxLength; } set { baseTextBox.MaxLength = value; } }

        /// <summary>
        /// Gets or sets the selected text.
        /// </summary>
        /// <value>The selected text.</value>
        public string SelectedText { get { return baseTextBox.SelectedText; } set { baseTextBox.SelectedText = value; } }
        /// <summary>
        /// Gets or sets the hint.
        /// </summary>
        /// <value>The hint.</value>
        public string Hint { get { return baseTextBox.Hint; } set { baseTextBox.Hint = value; } }

        /// <summary>
        /// Gets or sets the selection start.
        /// </summary>
        /// <value>The selection start.</value>
        public int SelectionStart { get { return baseTextBox.SelectionStart; } set { baseTextBox.SelectionStart = value; } }
        /// <summary>
        /// Gets or sets the length of the selection.
        /// </summary>
        /// <value>The length of the selection.</value>
        public int SelectionLength { get { return baseTextBox.SelectionLength; } set { baseTextBox.SelectionLength = value; } }
        /// <summary>
        /// Gets the length of the text.
        /// </summary>
        /// <value>The length of the text.</value>
        public int TextLength { get { return baseTextBox.TextLength; } }

        /// <summary>
        /// Selects all.
        /// </summary>
        public void SelectAll() { baseTextBox.SelectAll(); }
        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear() { baseTextBox.Clear(); }

        /// <summary>
        /// Copies this instance.
        /// </summary>
        public void Copy() { baseTextBox.Copy(); }

        /// <summary>
        /// Cuts this instance.
        /// </summary>
        public void Cut() { baseTextBox.Cut(); }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        public bool ReadOnly { get { return baseTextBox.ReadOnly; } set { baseTextBox.ReadOnly = value; } }


        # region Forwarding events to baseTextBox
        /// <summary>
        /// Occurs when [accepts tab changed].
        /// </summary>
        public event EventHandler AcceptsTabChanged
        {
            add
            {

                baseTextBox.AcceptsTabChanged += value;
            }
            remove
            {
                baseTextBox.AcceptsTabChanged -= value;
            }
        }

        /// <summary>
        /// This event is not relevant for this class.
        /// </summary>
        public new event EventHandler AutoSizeChanged
        {
            add
            {
                baseTextBox.AutoSizeChanged += value;
            }
            remove
            {
                baseTextBox.AutoSizeChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="P:System.Windows.Forms.Control.BackgroundImage" /> property changes.
        /// </summary>
        public new event EventHandler BackgroundImageChanged
        {
            add
            {
                baseTextBox.BackgroundImageChanged += value;
            }
            remove
            {
                baseTextBox.BackgroundImageChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.BackgroundImageLayout" /> property changes.
        /// </summary>
        public new event EventHandler BackgroundImageLayoutChanged
        {
            add
            {
                baseTextBox.BackgroundImageLayoutChanged += value;
            }
            remove
            {
                baseTextBox.BackgroundImageLayoutChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="T:System.Windows.Forms.BindingContext" /> property changes.
        /// </summary>
        public new event EventHandler BindingContextChanged
        {
            add
            {
                baseTextBox.BindingContextChanged += value;
            }
            remove
            {
                baseTextBox.BindingContextChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when [border style changed].
        /// </summary>
        public event EventHandler BorderStyleChanged
        {
            add
            {
                baseTextBox.BorderStyleChanged += value;
            }
            remove
            {
                baseTextBox.BorderStyleChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="P:System.Windows.Forms.Control.CausesValidation" /> property changes.
        /// </summary>
        public new event EventHandler CausesValidationChanged
        {
            add
            {
                baseTextBox.CausesValidationChanged += value;
            }
            remove
            {
                baseTextBox.CausesValidationChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the focus or keyboard user interface (UI) cues change.
        /// </summary>
        public new event UICuesEventHandler ChangeUICues
        {
            add
            {
                baseTextBox.ChangeUICues += value;
            }
            remove
            {
                baseTextBox.ChangeUICues -= value;
            }
        }

        /// <summary>
        /// Occurs when the control is clicked.
        /// </summary>
        public new event EventHandler Click
        {
            add
            {
                baseTextBox.Click += value;
            }
            remove
            {
                baseTextBox.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="P:System.Windows.Forms.Control.ClientSize" /> property changes.
        /// </summary>
        public new event EventHandler ClientSizeChanged
        {
            add
            {
                baseTextBox.ClientSizeChanged += value;
            }
            remove
            {
                baseTextBox.ClientSizeChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="P:System.Windows.Forms.Control.ContextMenu" /> property changes.
        /// </summary>
        public new event EventHandler ContextMenuChanged
        {
            add
            {
                baseTextBox.ContextMenuChanged += value;
            }
            remove
            {
                baseTextBox.ContextMenuChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="P:System.Windows.Forms.Control.ContextMenuStrip" /> property changes.
        /// </summary>
        public new event EventHandler ContextMenuStripChanged
        {
            add
            {
                baseTextBox.ContextMenuStripChanged += value;
            }
            remove
            {
                baseTextBox.ContextMenuStripChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when a new control is added to the <see cref="T:System.Windows.Forms.Control.ControlCollection" />.
        /// </summary>
        public new event ControlEventHandler ControlAdded
        {
            add
            {
                baseTextBox.ControlAdded += value;
            }
            remove
            {
                baseTextBox.ControlAdded -= value;
            }
        }

        /// <summary>
        /// Occurs when a control is removed from the <see cref="T:System.Windows.Forms.Control.ControlCollection" />.
        /// </summary>
        public new event ControlEventHandler ControlRemoved
        {
            add
            {
                baseTextBox.ControlRemoved += value;
            }
            remove
            {
                baseTextBox.ControlRemoved -= value;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="P:System.Windows.Forms.Control.Cursor" /> property changes.
        /// </summary>
        public new event EventHandler CursorChanged
        {
            add
            {
                baseTextBox.CursorChanged += value;
            }
            remove
            {
                baseTextBox.CursorChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the component is disposed by a call to the <see cref="M:System.ComponentModel.Component.Dispose" /> method.
        /// </summary>
        public new event EventHandler Disposed
        {
            add
            {
                baseTextBox.Disposed += value;
            }
            remove
            {
                baseTextBox.Disposed -= value;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="P:System.Windows.Forms.Control.Dock" /> property changes.
        /// </summary>
        public new event EventHandler DockChanged
        {
            add
            {
                baseTextBox.DockChanged += value;
            }
            remove
            {
                baseTextBox.DockChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the control is double-clicked.
        /// </summary>
        public new event EventHandler DoubleClick
        {
            add
            {
                baseTextBox.DoubleClick += value;
            }
            remove
            {
                baseTextBox.DoubleClick -= value;
            }
        }

        /// <summary>
        /// Occurs when a drag-and-drop operation is completed.
        /// </summary>
        public new event DragEventHandler DragDrop
        {
            add
            {
                baseTextBox.DragDrop += value;
            }
            remove
            {
                baseTextBox.DragDrop -= value;
            }
        }

        /// <summary>
        /// Occurs when an object is dragged into the control's bounds.
        /// </summary>
        public new event DragEventHandler DragEnter
        {
            add
            {
                baseTextBox.DragEnter += value;
            }
            remove
            {
                baseTextBox.DragEnter -= value;
            }
        }

        /// <summary>
        /// Occurs when an object is dragged out of the control's bounds.
        /// </summary>
        public new event EventHandler DragLeave
        {
            add
            {
                baseTextBox.DragLeave += value;
            }
            remove
            {
                baseTextBox.DragLeave -= value;
            }
        }

        /// <summary>
        /// Occurs when an object is dragged over the control's bounds.
        /// </summary>
        public new event DragEventHandler DragOver
        {
            add
            {
                baseTextBox.DragOver += value;
            }
            remove
            {
                baseTextBox.DragOver -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.Enabled" /> property value has changed.
        /// </summary>
        public new event EventHandler EnabledChanged
        {
            add
            {
                baseTextBox.EnabledChanged += value;
            }
            remove
            {
                baseTextBox.EnabledChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the control is entered.
        /// </summary>
        public new event EventHandler Enter
        {
            add
            {
                baseTextBox.Enter += value;
            }
            remove
            {
                baseTextBox.Enter -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.Font" /> property value changes.
        /// </summary>
        public new event EventHandler FontChanged
        {
            add
            {
                baseTextBox.FontChanged += value;
            }
            remove
            {
                baseTextBox.FontChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.ForeColor" /> property value changes.
        /// </summary>
        public new event EventHandler ForeColorChanged
        {
            add
            {
                baseTextBox.ForeColorChanged += value;
            }
            remove
            {
                baseTextBox.ForeColorChanged -= value;
            }
        }

        /// <summary>
        /// Occurs during a drag operation.
        /// </summary>
        public new event GiveFeedbackEventHandler GiveFeedback
        {
            add
            {
                baseTextBox.GiveFeedback += value;
            }
            remove
            {
                baseTextBox.GiveFeedback -= value;
            }
        }

        /// <summary>
        /// Occurs when the control receives focus.
        /// </summary>
        public new event EventHandler GotFocus
        {
            add
            {
                baseTextBox.GotFocus += value;
            }
            remove
            {
                baseTextBox.GotFocus -= value;
            }
        }

        /// <summary>
        /// Occurs when a handle is created for the control.
        /// </summary>
        public new event EventHandler HandleCreated
        {
            add
            {
                baseTextBox.HandleCreated += value;
            }
            remove
            {
                baseTextBox.HandleCreated -= value;
            }
        }

        /// <summary>
        /// Occurs when the control's handle is in the process of being destroyed.
        /// </summary>
        public new event EventHandler HandleDestroyed
        {
            add
            {
                baseTextBox.HandleDestroyed += value;
            }
            remove
            {
                baseTextBox.HandleDestroyed -= value;
            }
        }

        /// <summary>
        /// Occurs when the user requests help for a control.
        /// </summary>
        public new event HelpEventHandler HelpRequested
        {
            add
            {
                baseTextBox.HelpRequested += value;
            }
            remove
            {
                baseTextBox.HelpRequested -= value;
            }
        }

        /// <summary>
        /// Occurs when [hide selection changed].
        /// </summary>
        public event EventHandler HideSelectionChanged
        {
            add
            {
                baseTextBox.HideSelectionChanged += value;
            }
            remove
            {
                baseTextBox.HideSelectionChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.ImeMode" /> property has changed.
        /// </summary>
        public new event EventHandler ImeModeChanged
        {
            add
            {
                baseTextBox.ImeModeChanged += value;
            }
            remove
            {
                baseTextBox.ImeModeChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when a control's display requires redrawing.
        /// </summary>
        public new event InvalidateEventHandler Invalidated
        {
            add
            {
                baseTextBox.Invalidated += value;
            }
            remove
            {
                baseTextBox.Invalidated -= value;
            }
        }

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        public new event KeyEventHandler KeyDown
        {
            add
            {
                baseTextBox.KeyDown += value;
            }
            remove
            {
                baseTextBox.KeyDown -= value;
            }
        }

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        public new event KeyPressEventHandler KeyPress
        {
            add
            {
                baseTextBox.KeyPress += value;
            }
            remove
            {
                baseTextBox.KeyPress -= value;
            }
        }

        /// <summary>
        /// Occurs when a key is released while the control has focus.
        /// </summary>
        public new event KeyEventHandler KeyUp
        {
            add
            {
                baseTextBox.KeyUp += value;
            }
            remove
            {
                baseTextBox.KeyUp -= value;
            }
        }

        /// <summary>
        /// Occurs when a control should reposition its child controls.
        /// </summary>
        public new event LayoutEventHandler Layout
        {
            add
            {
                baseTextBox.Layout += value;
            }
            remove
            {
                baseTextBox.Layout -= value;
            }
        }

        /// <summary>
        /// Occurs when the input focus leaves the control.
        /// </summary>
        public new event EventHandler Leave
        {
            add
            {
                baseTextBox.Leave += value;
            }
            remove
            {
                baseTextBox.Leave -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.Location" /> property value has changed.
        /// </summary>
        public new event EventHandler LocationChanged
        {
            add
            {
                baseTextBox.LocationChanged += value;
            }
            remove
            {
                baseTextBox.LocationChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the control loses focus.
        /// </summary>
        public new event EventHandler LostFocus
        {
            add
            {
                baseTextBox.LostFocus += value;
            }
            remove
            {
                baseTextBox.LostFocus -= value;
            }
        }

        /// <summary>
        /// Occurs when the control's margin changes.
        /// </summary>
        public new event EventHandler MarginChanged
        {
            add
            {
                baseTextBox.MarginChanged += value;
            }
            remove
            {
                baseTextBox.MarginChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when [modified changed].
        /// </summary>
        public event EventHandler ModifiedChanged
        {
            add
            {
                baseTextBox.ModifiedChanged += value;
            }
            remove
            {
                baseTextBox.ModifiedChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the control loses mouse capture.
        /// </summary>
        public new event EventHandler MouseCaptureChanged
        {
            add
            {
                baseTextBox.MouseCaptureChanged += value;
            }
            remove
            {
                baseTextBox.MouseCaptureChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the control is clicked by the mouse.
        /// </summary>
        public new event MouseEventHandler MouseClick
        {
            add
            {
                baseTextBox.MouseClick += value;
            }
            remove
            {
                baseTextBox.MouseClick -= value;
            }
        }

        /// <summary>
        /// Occurs when the control is double clicked by the mouse.
        /// </summary>
        public new event MouseEventHandler MouseDoubleClick
        {
            add
            {
                baseTextBox.MouseDoubleClick += value;
            }
            remove
            {
                baseTextBox.MouseDoubleClick -= value;
            }
        }

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is pressed.
        /// </summary>
        public new event MouseEventHandler MouseDown
        {
            add
            {
                baseTextBox.MouseDown += value;
            }
            remove
            {
                baseTextBox.MouseDown -= value;
            }
        }

        /// <summary>
        /// Occurs when the mouse pointer enters the control.
        /// </summary>
        public new event EventHandler MouseEnter
        {
            add
            {
                baseTextBox.MouseEnter += value;
            }
            remove
            {
                baseTextBox.MouseEnter -= value;
            }
        }

        /// <summary>
        /// Occurs when the mouse pointer rests on the control.
        /// </summary>
        public new event EventHandler MouseHover
        {
            add
            {
                baseTextBox.MouseHover += value;
            }
            remove
            {
                baseTextBox.MouseHover -= value;
            }
        }

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        public new event EventHandler MouseLeave
        {
            add
            {
                baseTextBox.MouseLeave += value;
            }
            remove
            {
                baseTextBox.MouseLeave -= value;
            }
        }

        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        public new event MouseEventHandler MouseMove
        {
            add
            {
                baseTextBox.MouseMove += value;
            }
            remove
            {
                baseTextBox.MouseMove -= value;
            }
        }

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        public new event MouseEventHandler MouseUp
        {
            add
            {
                baseTextBox.MouseUp += value;
            }
            remove
            {
                baseTextBox.MouseUp -= value;
            }
        }

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        public new event MouseEventHandler MouseWheel
        {
            add
            {
                baseTextBox.MouseWheel += value;
            }
            remove
            {
                baseTextBox.MouseWheel -= value;
            }
        }

        /// <summary>
        /// Occurs when the control is moved.
        /// </summary>
        public new event EventHandler Move
        {
            add
            {
                baseTextBox.Move += value;
            }
            remove
            {
                baseTextBox.Move -= value;
            }
        }

        /// <summary>
        /// Occurs when [multiline changed].
        /// </summary>
        public event EventHandler MultilineChanged
        {
            add
            {
                baseTextBox.MultilineChanged += value;
            }
            remove
            {
                baseTextBox.MultilineChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the control's padding changes.
        /// </summary>
        public new event EventHandler PaddingChanged
        {
            add
            {
                baseTextBox.PaddingChanged += value;
            }
            remove
            {
                baseTextBox.PaddingChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the control is redrawn.
        /// </summary>
        public new event PaintEventHandler Paint
        {
            add
            {
                baseTextBox.Paint += value;
            }
            remove
            {
                baseTextBox.Paint -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.Parent" /> property value changes.
        /// </summary>
        public new event EventHandler ParentChanged
        {
            add
            {
                baseTextBox.ParentChanged += value;
            }
            remove
            {
                baseTextBox.ParentChanged -= value;
            }
        }

        /// <summary>
        /// Occurs before the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event when a key is pressed while focus is on this control.
        /// </summary>
        public new event PreviewKeyDownEventHandler PreviewKeyDown
        {
            add
            {
                baseTextBox.PreviewKeyDown += value;
            }
            remove
            {
                baseTextBox.PreviewKeyDown -= value;
            }
        }

        /// <summary>
        /// Occurs when <see cref="T:System.Windows.Forms.AccessibleObject" /> is providing help to accessibility applications.
        /// </summary>
        public new event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
        {
            add
            {
                baseTextBox.QueryAccessibilityHelp += value;
            }
            remove
            {
                baseTextBox.QueryAccessibilityHelp -= value;
            }
        }

        /// <summary>
        /// Occurs during a drag-and-drop operation and enables the drag source to determine whether the drag-and-drop operation should be canceled.
        /// </summary>
        public new event QueryContinueDragEventHandler QueryContinueDrag
        {
            add
            {
                baseTextBox.QueryContinueDrag += value;
            }
            remove
            {
                baseTextBox.QueryContinueDrag -= value;
            }
        }

        /// <summary>
        /// Occurs when [read only changed].
        /// </summary>
        public event EventHandler ReadOnlyChanged
        {
            add
            {
                baseTextBox.ReadOnlyChanged += value;
            }
            remove
            {
                baseTextBox.ReadOnlyChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="P:System.Windows.Forms.Control.Region" /> property changes.
        /// </summary>
        public new event EventHandler RegionChanged
        {
            add
            {
                baseTextBox.RegionChanged += value;
            }
            remove
            {
                baseTextBox.RegionChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the control is resized.
        /// </summary>
        public new event EventHandler Resize
        {
            add
            {
                baseTextBox.Resize += value;
            }
            remove
            {
                baseTextBox.Resize -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.RightToLeft" /> property value changes.
        /// </summary>
        public new event EventHandler RightToLeftChanged
        {
            add
            {
                baseTextBox.RightToLeftChanged += value;
            }
            remove
            {
                baseTextBox.RightToLeftChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.Size" /> property value changes.
        /// </summary>
        public new event EventHandler SizeChanged
        {
            add
            {
                baseTextBox.SizeChanged += value;
            }
            remove
            {
                baseTextBox.SizeChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the control style changes.
        /// </summary>
        public new event EventHandler StyleChanged
        {
            add
            {
                baseTextBox.StyleChanged += value;
            }
            remove
            {
                baseTextBox.StyleChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the system colors change.
        /// </summary>
        public new event EventHandler SystemColorsChanged
        {
            add
            {
                baseTextBox.SystemColorsChanged += value;
            }
            remove
            {
                baseTextBox.SystemColorsChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.TabIndex" /> property value changes.
        /// </summary>
        public new event EventHandler TabIndexChanged
        {
            add
            {
                baseTextBox.TabIndexChanged += value;
            }
            remove
            {
                baseTextBox.TabIndexChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.TabStop" /> property value changes.
        /// </summary>
        public new event EventHandler TabStopChanged
        {
            add
            {
                baseTextBox.TabStopChanged += value;
            }
            remove
            {
                baseTextBox.TabStopChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.Text" /> property value changes.
        /// </summary>
        public new event EventHandler TextChanged
        {
            add
            {
                baseTextBox.TextChanged += value;
            }
            remove
            {
                baseTextBox.TextChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the control is finished validating.
        /// </summary>
        public new event EventHandler Validated
        {
            add
            {
                baseTextBox.Validated += value;
            }
            remove
            {
                baseTextBox.Validated -= value;
            }
        }

        /// <summary>
        /// Occurs when the control is validating.
        /// </summary>
        public new event CancelEventHandler Validating
        {
            add
            {
                baseTextBox.Validating += value;
            }
            remove
            {
                baseTextBox.Validating -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.Visible" /> property value changes.
        /// </summary>
        public new event EventHandler VisibleChanged
        {
            add
            {
                baseTextBox.VisibleChanged += value;
            }
            remove
            {
                baseTextBox.VisibleChanged -= value;
            }
        }
        # endregion

        /// <summary>
        /// The animation manager
        /// </summary>
        private readonly AnimationManager animationManager;

        /// <summary>
        /// The base text box
        /// </summary>
        protected readonly BaseTextBox baseTextBox;
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialTextBox"/> class.
        /// </summary>
        public MaterialTextBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer, true);

            animationManager = new AnimationManager
            {
                Increment = 0.06,
                AnimationType = AnimationType.EaseInOut,
                InterruptAnimation = false
            };
            animationManager.OnAnimationProgress += sender => Invalidate();

            baseTextBox = new BaseTextBox
            {
                BorderStyle = BorderStyle.None,
                Font = SkinManager.ROBOTO_REGULAR_11,
                ForeColor = SkinManager.GetPrimaryTextColor(),
                Location = new Point(0, 18),
                Width = Width,
                Height = Height - 5,
                Multiline = true
            };
            baseTextBox.SizeChanged += baseTextBox_SizeChanged;

            if (!Controls.Contains(baseTextBox) && !DesignMode)
            {
                Controls.Add(baseTextBox);
            }

            baseTextBox.GotFocus += (sender, args) => animationManager.StartNewAnimation(AnimationDirection.In);
            baseTextBox.LostFocus += (sender, args) => animationManager.StartNewAnimation(AnimationDirection.Out);
            BackColorChanged += (sender, args) =>
            {
                baseTextBox.BackColor = BackColor;
                baseTextBox.ForeColor = SkinManager.GetPrimaryTextColor();
            };

            baseTextBox.TextChanged += new EventHandler(Redraw);

            //Fix for tabstop
            baseTextBox.TabStop = true;
            this.TabStop = false;
        }

        /// <summary>
        /// Handles the SizeChanged event of the baseTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void baseTextBox_SizeChanged(object sender, EventArgs e)
        {
           // Height = baseTextBox.Height + 20;
        }

        /// <summary>
        /// Redraws the specified sencer.
        /// </summary>
        /// <param name="sencer">The sencer.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Redraw(object sencer, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Handles the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="pevent">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.Clear(BackColor);
            baseTextBox.BackColor = BackColor;

            int lineY = baseTextBox.Bottom + 3;

            if (!animationManager.IsAnimating())
            {
                //No animation
                g.FillRectangle(baseTextBox.Focused ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetDividersBrush(), baseTextBox.Location.X, lineY, baseTextBox.Width, baseTextBox.Focused ? 2 : 1);
            }
            else
            {
                //Animate
                int animationWidth = (int)(baseTextBox.Width * animationManager.GetProgress());
                int halfAnimationWidth = animationWidth / 2;
                int animationStart = baseTextBox.Location.X + baseTextBox.Width / 2;

                //Unfocused background
                g.FillRectangle(SkinManager.GetDividersBrush(), baseTextBox.Location.X, lineY, baseTextBox.Width, 1);

                //Animated focus transition
                g.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, animationStart - halfAnimationWidth, lineY, animationWidth, 2);


            }
            if (!String.IsNullOrWhiteSpace(Hint))
            {
                g.DrawString(
                Hint,
                SkinManager.ROBOTO_MEDIUM_10,
                Focused() ? SkinManager.ColorScheme.AccentBrush: SkinManager.GetSecondaryTextBrush(),
                new Rectangle(ClientRectangle.X, 0, ClientRectangle.Width, ClientRectangle.Height),
                new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            }
        }

        /// <summary>
        /// Focuseds this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Focused()
        {
            return baseTextBox.Focused;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            baseTextBox.Location = new Point(0, 15);
            baseTextBox.Width = Width;
            baseTextBox.Height = Height-20;

        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl" /> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            baseTextBox.BackColor = Parent.BackColor;
            baseTextBox.ForeColor = SkinManager.GetPrimaryTextColor();
        }

        /// <summary>
        /// Class BaseTextBox.
        /// </summary>
        /// <seealso cref="System.Windows.Forms.RichTextBox" />
        protected class BaseTextBox : RichTextBox
        {
            /// <summary>
            /// Sends the message.
            /// </summary>
            /// <param name="hWnd">The h WND.</param>
            /// <param name="msg">The MSG.</param>
            /// <param name="wParam">The w parameter.</param>
            /// <param name="lParam">The l parameter.</param>
            /// <returns>IntPtr.</returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

            /// <summary>
            /// The em setcuebanner
            /// </summary>
            private const int EM_SETCUEBANNER = 0x1501;
            /// <summary>
            /// The empty character
            /// </summary>
            private const char EmptyChar = (char)0;
            /// <summary>
            /// The visual style password character
            /// </summary>
            private const char VisualStylePasswordChar = '\u25CF';
            /// <summary>
            /// The non visual style password character
            /// </summary>
            private const char NonVisualStylePasswordChar = '\u002A';

            /// <summary>
            /// The hint
            /// </summary>
            private string hint = string.Empty;
            /// <summary>
            /// Gets or sets the hint.
            /// </summary>
            /// <value>The hint.</value>
            public string Hint
            {
                get { return hint; }
                set
                {
                    hint = value;
                    SendMessage(Handle, EM_SETCUEBANNER, (int)IntPtr.Zero, Hint);
                }
            }

            /// <summary>
            /// Selects all text in the text box.
            /// </summary>
            /// <PermissionSet>
            ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
            ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
            ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
            ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
            /// </PermissionSet>
            public new void SelectAll()
            {
                BeginInvoke((MethodInvoker)delegate()
                {
                    base.Focus();
                    base.SelectAll();
                });
            }


            /// <summary>
            /// Initializes a new instance of the <see cref="BaseTextBox"/> class.
            /// </summary>
            public BaseTextBox()
            {
                MaterialContextMenuStrip cms = new TextBoxContextMenuStrip();
                cms.Opening += ContextMenuStripOnOpening;
                cms.OnItemClickStart += ContextMenuStripOnItemClickStart;
                ContextMenuStrip = cms;
            }

            /// <summary>
            /// Contexts the menu strip on item click start.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="toolStripItemClickedEventArgs">The <see cref="ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
            private void ContextMenuStripOnItemClickStart(object sender, ToolStripItemClickedEventArgs toolStripItemClickedEventArgs)
            {
                switch (toolStripItemClickedEventArgs.ClickedItem.Text)
                {
                    case "Undo":
                        Undo();
                        break;
                    case "Cut":
                        Cut();
                        break;
                    case "Copy":
                        Copy();
                        break;
                    case "Paste":
                        Paste();
                        break;
                    case "Delete":
                        SelectedText = string.Empty;
                        break;
                    case "Select All":
                        SelectAll();
                        break;
                }
            }

            /// <summary>
            /// Contexts the menu strip on opening.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="cancelEventArgs">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
            private void ContextMenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
            {
                var strip = sender as TextBoxContextMenuStrip;
                if (strip != null)
                {
                    strip.undo.Enabled = CanUndo;
                    strip.cut.Enabled = !string.IsNullOrEmpty(SelectedText);
                    strip.copy.Enabled = !string.IsNullOrEmpty(SelectedText);
                    strip.paste.Enabled = Clipboard.ContainsText();
                    strip.delete.Enabled = !string.IsNullOrEmpty(SelectedText);
                    strip.selectAll.Enabled = !string.IsNullOrEmpty(Text);
                }
            }
        }

        /// <summary>
        /// Class TextBoxContextMenuStrip.
        /// </summary>
        /// <seealso cref="Zeroit.Framework.MaterialWinforms.Controls.MaterialContextMenuStrip" />
        private class TextBoxContextMenuStrip : MaterialContextMenuStrip
        {
            /// <summary>
            /// The undo
            /// </summary>
            public readonly ToolStripItem undo = new MaterialToolStripMenuItem { Text = "Undo" };
            /// <summary>
            /// The seperator1
            /// </summary>
            public readonly ToolStripItem seperator1 = new ToolStripSeparator();
            /// <summary>
            /// The cut
            /// </summary>
            public readonly ToolStripItem cut = new MaterialToolStripMenuItem { Text = "Cut" };
            /// <summary>
            /// The copy
            /// </summary>
            public readonly ToolStripItem copy = new MaterialToolStripMenuItem { Text = "Copy" };
            /// <summary>
            /// The paste
            /// </summary>
            public readonly ToolStripItem paste = new MaterialToolStripMenuItem { Text = "Paste" };
            /// <summary>
            /// The delete
            /// </summary>
            public readonly ToolStripItem delete = new MaterialToolStripMenuItem { Text = "Delete" };
            /// <summary>
            /// The seperator2
            /// </summary>
            public readonly ToolStripItem seperator2 = new ToolStripSeparator();
            /// <summary>
            /// The select all
            /// </summary>
            public readonly ToolStripItem selectAll = new MaterialToolStripMenuItem { Text = "Select All" };

            /// <summary>
            /// Initializes a new instance of the <see cref="TextBoxContextMenuStrip"/> class.
            /// </summary>
            public TextBoxContextMenuStrip()
            {
                Items.AddRange(new[]
                {
                    undo,
                    seperator1,
                    cut,
                    copy,
                    paste,
                    delete,
                    seperator2,
                    selectAll
                });
            }
        }
    }
}
