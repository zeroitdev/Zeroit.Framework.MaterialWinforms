// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialTabControl.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Drawing;

namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialTabControl.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.TabControl" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    [Designer(typeof(MaterialTabControlDesigner))]
    public class MaterialTabControl : TabControl, IMaterialControl
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
        /// This member is not meaningful for this control.
        /// </summary>
        /// <value>The color of the back.</value>
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : Parent.BackColor; } }

        /// <summary>
        /// The tabs are closable
        /// </summary>
        private bool _TabsAreClosable;
        /// <summary>
        /// Gets or sets a value indicating whether [tabs are closable].
        /// </summary>
        /// <value><c>true</c> if [tabs are closable]; otherwise, <c>false</c>.</value>
        public bool TabsAreClosable
        {
            get
            {
                return _TabsAreClosable;
            }
            set
            {
                _TabsAreClosable = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialTabControl"/> class.
        /// </summary>
        public MaterialTabControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer, true);
        }

        /// <summary>
        /// This member overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />.
        /// </summary>
        /// <param name="m">A Windows Message Object.</param>
        protected override void WndProc(ref Message m)
        {

            if (m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
        }


        /// <summary>
        /// Gets the collection of tab pages in this tab control.
        /// </summary>
        /// <value>The tab pages.</value>
        [Editor(typeof(MaterialTabPageCollectionEditor), typeof(UITypeEditor))]
        public new TabPageCollection TabPages
        {
            get
            {
                return base.TabPages;
            }
        }

        /// <summary>
        /// Class MaterialTabPageCollectionEditor.
        /// </summary>
        /// <seealso cref="System.ComponentModel.Design.CollectionEditor" />
        internal class MaterialTabPageCollectionEditor : CollectionEditor
        {
            /// <summary>
            /// Creates a new form to display and edit the current collection.
            /// </summary>
            /// <returns>A <see cref="T:System.ComponentModel.Design.CollectionEditor.CollectionForm" /> to provide as the user interface for editing the collection.</returns>
            protected override CollectionEditor.CollectionForm
            CreateCollectionForm()
            {
                CollectionForm baseForm = base.CreateCollectionForm();
                baseForm.Text = "TabPage Collection Editor";
                return baseForm;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MaterialTabPageCollectionEditor"/> class.
            /// </summary>
            /// <param name="type">The type of the collection for this editor to edit.</param>
            public MaterialTabPageCollectionEditor(System.Type type)
                : base(type)
            {
            }
            /// <summary>
            /// Gets the data type that this collection contains.
            /// </summary>
            /// <returns>The data type of the items in the collection, or an <see cref="T:System.Object" /> if no Item property can be located on the collection.</returns>
            protected override Type CreateCollectionItemType()
            {
                return typeof(MaterialTabPage);
            }
            /// <summary>
            /// Gets the data types that this collection editor can contain.
            /// </summary>
            /// <returns>An array of data types that this collection can contain.</returns>
            protected override Type[] CreateNewItemTypes()
            {
                return new Type[] { typeof(MaterialTabPage) };
            }

        }

    }

    /// <summary>
    /// Class MaterialTabPage.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.TabPage" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    [Designer(typeof(System.Windows.Forms.Design.ScrollableControlDesigner))]
    public class MaterialTabPage : TabPage, IMaterialControl
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
        /// Gets or sets the background color for the <see cref="T:System.Windows.Forms.TabPage" />.
        /// </summary>
        /// <value>The color of the back.</value>
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : typeof(IMaterialControl).IsAssignableFrom(Parent.GetType()) ? ((IMaterialControl)Parent).BackColor : Parent.BackColor; } }

        /// <summary>
        /// Gets or sets the closable.
        /// </summary>
        /// <value>The closable.</value>
        [Category("Behavior")]
        public Boolean Closable { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialTabPage"/> class.
        /// </summary>
        public MaterialTabPage()
        {

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialTabPage"/> class.
        /// </summary>
        /// <param name="pText">The p text.</param>
        public MaterialTabPage(string pText)
        {
            this.Name = pText;
            this.Text = pText;

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialTabPage"/> class.
        /// </summary>
        /// <param name="pText">The p text.</param>
        /// <param name="isClosable">if set to <c>true</c> [is closable].</param>
        public MaterialTabPage(string pText, bool isClosable)
        {
            this.Name = pText;
            this.Text = pText;

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Closable = isClosable;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(SkinManager.GetApplicationBackgroundColor());
            foreach (Control objChild in Controls)
            {
                if (typeof(IShadowedMaterialControl).IsAssignableFrom(objChild.GetType()))
                {
                    IShadowedMaterialControl objCurrent = (IShadowedMaterialControl)objChild;
                    DrawHelper.drawShadow(e.Graphics, objCurrent.ShadowBorder, objCurrent.Elevation, SkinManager.GetApplicationBackgroundColor());

                }
            }
        }
    }


    /// <summary>
    /// Class MaterialTabControlDesigner.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Design.ParentControlDesigner" />
    internal class MaterialTabControlDesigner :
    System.Windows.Forms.Design.ParentControlDesigner
    {

        #region Private Instance Variables

        /// <summary>
        /// The m verbs
        /// </summary>
        private DesignerVerbCollection m_verbs = new
        DesignerVerbCollection();
        /// <summary>
        /// The m designer host
        /// </summary>
        private IDesignerHost m_DesignerHost;
        /// <summary>
        /// The m selection service
        /// </summary>
        private ISelectionService m_SelectionService;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialTabControlDesigner"/> class.
        /// </summary>
        public MaterialTabControlDesigner()
            : base()
        {
            DesignerVerb verb1 = new DesignerVerb("Add Tab", new
            EventHandler(OnAddPage));
            DesignerVerb verb2 = new DesignerVerb("Remove Tab", new
            EventHandler(OnRemovePage));
            m_verbs.AddRange(new DesignerVerb[] { verb1, verb2 });
        }

        #region Properties

        /// <summary>
        /// Gets the design-time verbs supported by the component that is associated with the designer.
        /// </summary>
        /// <value>The verbs.</value>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (m_verbs.Count == 2)
                {
                    MaterialTabControl MyControl = (MaterialTabControl)Control;
                    if (MyControl.TabCount == 0)
                    {
                        m_verbs[1].Enabled = true;
                    }
                    else
                    {
                        m_verbs[1].Enabled = false;
                    }
                }
                return m_verbs;
            }
        }

        /// <summary>
        /// Gets the designer host.
        /// </summary>
        /// <value>The designer host.</value>
        public IDesignerHost DesignerHost
        {
            get
            {
                if (m_DesignerHost == null)
                    m_DesignerHost =
                    (IDesignerHost)(GetService(typeof(IDesignerHost)));

                return m_DesignerHost;
            }
        }

        /// <summary>
        /// Gets the selection service.
        /// </summary>
        /// <value>The selection service.</value>
        public ISelectionService SelectionService
        {
            get
            {
                if (m_SelectionService == null)
                    m_SelectionService =
                    (ISelectionService)(this.GetService(typeof(ISelectionService)));
                return m_SelectionService;
            }
        }

        #endregion

        /// <summary>
        /// Handles the <see cref="E:AddPage" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void OnAddPage(Object sender, EventArgs e)
        {
            MaterialTabControl ParentControl = (MaterialTabControl)Control;
            System.Windows.Forms.Control.ControlCollection oldTabs =
            ParentControl.Controls;

            RaiseComponentChanging(TypeDescriptor.GetProperties(ParentControl)["TabPages"]);

            System.Windows.Forms.TabPage P =
            (System.Windows.Forms.TabPage)(DesignerHost.CreateComponent(typeof(MaterialTabPage)));
            P.Text = P.Name;
            ParentControl.TabPages.Add(P);

            RaiseComponentChanged(TypeDescriptor.GetProperties(ParentControl)["TabPages"],
            oldTabs, ParentControl.TabPages);
            ParentControl.SelectedTab = P;

            SetVerbs();

        }

        /// <summary>
        /// Handles the <see cref="E:RemovePage" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void OnRemovePage(Object sender, EventArgs e)
        {
            MaterialTabControl ParentControl = (MaterialTabControl)Control;
            System.Windows.Forms.Control.ControlCollection oldTabs =
            ParentControl.Controls;

            if (ParentControl.SelectedIndex < 0) return;

            RaiseComponentChanging(TypeDescriptor.GetProperties(ParentControl)["TabPages"]);

            DesignerHost.DestroyComponent(ParentControl.TabPages[ParentControl.SelectedIndex]);

            RaiseComponentChanged(TypeDescriptor.GetProperties(ParentControl)["TabPages"],
            oldTabs, ParentControl.TabPages);

            SelectionService.SetSelectedComponents(new IComponent[] {
ParentControl }, SelectionTypes.Auto);

            SetVerbs();

        }

        /// <summary>
        /// Sets the verbs.
        /// </summary>
        private void SetVerbs()
        {
            MaterialTabControl ParentControl = (MaterialTabControl)Control;

            switch (ParentControl.TabPages.Count)
            {
                case 0:
                    Verbs[1].Enabled = false;
                    break;
                default:
                    Verbs[1].Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// The wm nchittest
        /// </summary>
        private const int WM_NCHITTEST = 0x84;

        /// <summary>
        /// The httransparent
        /// </summary>
        private const int HTTRANSPARENT = -1;
        /// <summary>
        /// The htclient
        /// </summary>
        private const int HTCLIENT = 1;

        /// <summary>
        /// Processes Windows messages and optionally routes them to the control.
        /// </summary>
        /// <param name="m">The <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
            {
                //select tabcontrol when Tabcontrol clicked outside of
                if (m.Result.ToInt32() == HTTRANSPARENT)
                    m.Result = (IntPtr)HTCLIENT;
            }

        }

        /// <summary>
        /// Enum TabControlHitTest
        /// </summary>
        private enum TabControlHitTest
        {
            /// <summary>
            /// The TCHT nowhere
            /// </summary>
            TCHT_NOWHERE = 1,
            /// <summary>
            /// The TCHT onitemicon
            /// </summary>
            TCHT_ONITEMICON = 2,
            /// <summary>
            /// The TCHT onitemlabel
            /// </summary>
            TCHT_ONITEMLABEL = 4,
            /// <summary>
            /// The TCHT onitem
            /// </summary>
            TCHT_ONITEM = TCHT_ONITEMICON | TCHT_ONITEMLABEL
        }

        /// <summary>
        /// The TCM hittest
        /// </summary>
        private const int TCM_HITTEST = 0x130D;

        /// <summary>
        /// Struct TCHITTESTINFO
        /// </summary>
        private struct TCHITTESTINFO
        {
            /// <summary>
            /// The pt
            /// </summary>
            public System.Drawing.Point pt;
            /// <summary>
            /// The flags
            /// </summary>
            public TabControlHitTest flags;
        }

        /// <summary>
        /// Indicates whether a mouse click at the specified point should be handled by the control.
        /// </summary>
        /// <param name="point">A <see cref="T:System.Drawing.Point" /> indicating the position at which the mouse was clicked, in screen coordinates.</param>
        /// <returns>true if a click at the specified point is to be handled by the control; otherwise, false.</returns>
        protected override bool GetHitTest(System.Drawing.Point point)
        {
            if (this.SelectionService.PrimarySelection == this.Control)
            {
                TCHITTESTINFO hti = new TCHITTESTINFO();

                hti.pt = this.Control.PointToClient(point);
                hti.flags = 0;

                System.Windows.Forms.Message m = new
                System.Windows.Forms.Message();
                m.HWnd = this.Control.Handle;
                m.Msg = TCM_HITTEST;

                IntPtr lparam =
                System.Runtime.InteropServices.Marshal.AllocHGlobal(System.Runtime.InteropServices.Marshal.SizeOf(hti));
                System.Runtime.InteropServices.Marshal.StructureToPtr(hti,
                lparam, false);
                m.LParam = lparam;

                base.WndProc(ref m);
                System.Runtime.InteropServices.Marshal.FreeHGlobal(lparam);

                if (m.Result.ToInt32() != -1)
                    return hti.flags != TabControlHitTest.TCHT_NOWHERE;

            }

            return false;
        }

        /// <summary>
        /// Called when the control that the designer is managing has painted its surface so the designer can paint any additional adornments on top of the control.
        /// </summary>
        /// <param name="pe">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that provides data for the event.</param>
        protected override void
        OnPaintAdornments(System.Windows.Forms.PaintEventArgs pe)
        {
            //Don't want DrawGrid dots.
        }

        //Fix the AllSizable selectionrule on DockStyle.Fill
        /// <summary>
        /// Gets the selection rules that indicate the movement capabilities of a component.
        /// </summary>
        /// <value>The selection rules.</value>
        public override System.Windows.Forms.Design.SelectionRules
        SelectionRules
        {
            get
            {
                if (Control.Dock == System.Windows.Forms.DockStyle.Fill)
                    return
                    System.Windows.Forms.Design.SelectionRules.Visible;
                return base.SelectionRules;
            }
        }
    }
}
