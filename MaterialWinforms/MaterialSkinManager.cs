// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 06-22-2018
// ***********************************************************************
// <copyright file="MaterialSkinManager.cs" company="Zeroit Dev Technlologies">
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Zeroit.Framework.MaterialWinforms.Controls;
using Zeroit.Framework.MaterialWinforms.Properties;

namespace Zeroit.Framework.MaterialWinforms
{
    /// <summary>
    /// Class MaterialSkinManager.
    /// </summary>
    public class MaterialSkinManager
    {
        //Singleton instance
        /// <summary>
        /// The instance
        /// </summary>
        private static MaterialSkinManager instance;

        //Forms to control
        /// <summary>
        /// The forms to manage
        /// </summary>
        private readonly List<MaterialForm> formsToManage = new List<MaterialForm>();

        /// <summary>
        /// The theme name
        /// </summary>
        public String ThemeName = "";
        /// <summary>
        /// Delegate ThemeChanged
        /// </summary>
        public delegate void ThemeChanged();
        /// <summary>
        /// Occurs when [on theme changed].
        /// </summary>
        public event ThemeChanged onThemeChanged;

        /// <summary>
        /// The color schemes
        /// </summary>
        public ColorSchemePresetCollection ColorSchemes;

        //Theme
        /// <summary>
        /// The theme
        /// </summary>
        private Themes theme;
        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        /// <value>The theme.</value>
        public Themes Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                UpdateBackgrounds();
                if (onThemeChanged != null)
                {
                    onThemeChanged();
                }
            }
        }

        /// <summary>
        /// The color scheme
        /// </summary>
        private ColorScheme colorScheme;
        /// <summary>
        /// Gets or sets the color scheme.
        /// </summary>
        /// <value>The color scheme.</value>
        public ColorScheme ColorScheme
        {
			get { return colorScheme; }
            set
            {
				colorScheme = value;
                UpdateBackgrounds();
                if (onThemeChanged != null)
                {
                    onThemeChanged();
                }
            }
        }

        /// <summary>
        /// Loads the color scheme from preset.
        /// </summary>
        /// <param name="pPreset">The p preset.</param>
        public void LoadColorSchemeFromPreset(ColorSchemePreset pPreset)
        {
            ThemeName = pPreset.Name;
            ColorScheme = new ColorScheme(pPreset.PrimaryColor, pPreset.DarkPrimaryColor, pPreset.LightPrimaryColor, pPreset.AccentColor, pPreset.TextShade);
        }

        /// <summary>
        /// Enum Themes
        /// </summary>
        public enum Themes : byte
        {
            /// <summary>
            /// The light
            /// </summary>
            LIGHT,
            /// <summary>
            /// The dark
            /// </summary>
            DARK
        }

        //Constant color values
        /// <summary>
        /// The primary text black
        /// </summary>
        private static readonly Color PRIMARY_TEXT_BLACK = Color.FromArgb(222, 0, 0, 0);
        /// <summary>
        /// The primary text black brush
        /// </summary>
        private static readonly Brush PRIMARY_TEXT_BLACK_BRUSH = new SolidBrush(PRIMARY_TEXT_BLACK);
        /// <summary>
        /// The secondary text black
        /// </summary>
        public static Color SECONDARY_TEXT_BLACK = Color.FromArgb(138, 0, 0, 0);
        /// <summary>
        /// The secondary text black brush
        /// </summary>
        public static Brush SECONDARY_TEXT_BLACK_BRUSH = new SolidBrush(SECONDARY_TEXT_BLACK);
        /// <summary>
        /// The disabled or hint text black
        /// </summary>
        private static readonly Color DISABLED_OR_HINT_TEXT_BLACK = Color.FromArgb(66, 0, 0, 0);
        /// <summary>
        /// The disabled or hint text black brush
        /// </summary>
        private static readonly Brush DISABLED_OR_HINT_TEXT_BLACK_BRUSH = new SolidBrush(DISABLED_OR_HINT_TEXT_BLACK);
        /// <summary>
        /// The dividers black
        /// </summary>
        private static readonly Color DIVIDERS_BLACK = Color.FromArgb(31, 0, 0, 0);
        /// <summary>
        /// The dividers black brush
        /// </summary>
        private static readonly Brush DIVIDERS_BLACK_BRUSH = new SolidBrush(DIVIDERS_BLACK);
        /// <summary>
        /// The card black
        /// </summary>
        private static readonly Color CARD_BLACK = Color.FromArgb(255, 42, 42, 42);
        /// <summary>
        /// The card white
        /// </summary>
        private static readonly Color CARD_WHITE = Color.White;

        /// <summary>
        /// The primary text white
        /// </summary>
        private static readonly Color PRIMARY_TEXT_WHITE = Color.FromArgb(255, 255, 255, 255);
        /// <summary>
        /// The primary text white brush
        /// </summary>
        private static readonly Brush PRIMARY_TEXT_WHITE_BRUSH = new SolidBrush(PRIMARY_TEXT_WHITE);
        /// <summary>
        /// The secondary text white
        /// </summary>
        public static Color SECONDARY_TEXT_WHITE = Color.FromArgb(179, 255, 255, 255);
        /// <summary>
        /// The secondary text white brush
        /// </summary>
        public static Brush SECONDARY_TEXT_WHITE_BRUSH = new SolidBrush(SECONDARY_TEXT_WHITE);
        /// <summary>
        /// The disabled or hint text white
        /// </summary>
        private static readonly Color DISABLED_OR_HINT_TEXT_WHITE = Color.FromArgb(77, 255, 255, 255);
        /// <summary>
        /// The disabled or hint text white brush
        /// </summary>
        private static readonly Brush DISABLED_OR_HINT_TEXT_WHITE_BRUSH = new SolidBrush(DISABLED_OR_HINT_TEXT_WHITE);
        /// <summary>
        /// The dividers white
        /// </summary>
        private static readonly Color DIVIDERS_WHITE = Color.FromArgb(31, 255, 255, 255);
        /// <summary>
        /// The dividers white brush
        /// </summary>
        private static readonly Brush DIVIDERS_WHITE_BRUSH = new SolidBrush(DIVIDERS_WHITE);

        // Checkbox colors
        /// <summary>
        /// The checkbox off light
        /// </summary>
        private static readonly Color CHECKBOX_OFF_LIGHT = Color.FromArgb(138, 0, 0, 0);
        /// <summary>
        /// The checkbox off light brush
        /// </summary>
        private static readonly Brush CHECKBOX_OFF_LIGHT_BRUSH = new SolidBrush(CHECKBOX_OFF_LIGHT);
        /// <summary>
        /// The checkbox off disabled light
        /// </summary>
        private static readonly Color CHECKBOX_OFF_DISABLED_LIGHT = Color.FromArgb(66, 0, 0, 0);
        /// <summary>
        /// The checkbox off disabled light brush
        /// </summary>
        private static readonly Brush CHECKBOX_OFF_DISABLED_LIGHT_BRUSH = new SolidBrush(CHECKBOX_OFF_DISABLED_LIGHT);

        /// <summary>
        /// The checkbox off dark
        /// </summary>
        private static readonly Color CHECKBOX_OFF_DARK = Color.FromArgb(179, 255, 255, 255);
        /// <summary>
        /// The checkbox off dark brush
        /// </summary>
        private static readonly Brush CHECKBOX_OFF_DARK_BRUSH = new SolidBrush(CHECKBOX_OFF_DARK);
        /// <summary>
        /// The checkbox off disabled dark
        /// </summary>
        private static readonly Color CHECKBOX_OFF_DISABLED_DARK = Color.FromArgb(77, 255, 255, 255);
        /// <summary>
        /// The checkbox off disabled dark brush
        /// </summary>
        private static readonly Brush CHECKBOX_OFF_DISABLED_DARK_BRUSH = new SolidBrush(CHECKBOX_OFF_DISABLED_DARK);

        //Raised button
        /// <summary>
        /// The raised button background
        /// </summary>
        private static readonly Color RAISED_BUTTON_BACKGROUND = Color.FromArgb(255, 255, 255, 255);
        /// <summary>
        /// The raised button background brush
        /// </summary>
        private static readonly Brush RAISED_BUTTON_BACKGROUND_BRUSH = new SolidBrush(RAISED_BUTTON_BACKGROUND);
        /// <summary>
        /// The raised button text light
        /// </summary>
        private static readonly Color RAISED_BUTTON_TEXT_LIGHT = PRIMARY_TEXT_WHITE;
        /// <summary>
        /// The raised button text light brush
        /// </summary>
        private static readonly Brush RAISED_BUTTON_TEXT_LIGHT_BRUSH = new SolidBrush(RAISED_BUTTON_TEXT_LIGHT);
        /// <summary>
        /// The raised button text dark
        /// </summary>
        private static readonly Color RAISED_BUTTON_TEXT_DARK = PRIMARY_TEXT_BLACK;
        /// <summary>
        /// The raised button text dark brush
        /// </summary>
        private static readonly Brush RAISED_BUTTON_TEXT_DARK_BRUSH = new SolidBrush(RAISED_BUTTON_TEXT_DARK);

        //Flat button
        /// <summary>
        /// The flat button background hover light
        /// </summary>
        private static readonly Color FLAT_BUTTON_BACKGROUND_HOVER_LIGHT = Color.FromArgb(20.PercentageToColorComponent(), 0x999999.ToColor());
        /// <summary>
        /// The flat button background hover light brush
        /// </summary>
        private static readonly Brush FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_HOVER_LIGHT);
        /// <summary>
        /// The flat button background pressed light
        /// </summary>
        private static readonly Color FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT = Color.FromArgb(40.PercentageToColorComponent(), 0x999999.ToColor());
        /// <summary>
        /// The flat button background pressed light brush
        /// </summary>
        private static readonly Brush FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT);
        /// <summary>
        /// The flat button disabledtext light
        /// </summary>
        private static readonly Color FLAT_BUTTON_DISABLEDTEXT_LIGHT = Color.FromArgb(26.PercentageToColorComponent(), 0x000000.ToColor());
        /// <summary>
        /// The flat button disabledtext light brush
        /// </summary>
        private static readonly Brush FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH = new SolidBrush(FLAT_BUTTON_DISABLEDTEXT_LIGHT);

        /// <summary>
        /// The flat button background hover dark
        /// </summary>
        private static readonly Color FLAT_BUTTON_BACKGROUND_HOVER_DARK = Color.FromArgb(15.PercentageToColorComponent(), 0xCCCCCC.ToColor());
        /// <summary>
        /// The flat button background hover dark brush
        /// </summary>
        private static readonly Brush FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_HOVER_DARK);
        /// <summary>
        /// The flat button background pressed dark
        /// </summary>
        private static readonly Color FLAT_BUTTON_BACKGROUND_PRESSED_DARK = Color.FromArgb(25.PercentageToColorComponent(), 0xCCCCCC.ToColor());
        /// <summary>
        /// The flat button background pressed dark brush
        /// </summary>
        private static readonly Brush FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_PRESSED_DARK);
        /// <summary>
        /// The flat button disabledtext dark
        /// </summary>
        private static readonly Color FLAT_BUTTON_DISABLEDTEXT_DARK = Color.FromArgb(30.PercentageToColorComponent(), 0xFFFFFF.ToColor());
        /// <summary>
        /// The flat button disabledtext dark brush
        /// </summary>
        private static readonly Brush FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH = new SolidBrush(FLAT_BUTTON_DISABLEDTEXT_DARK);

        //ContextMenuStrip
        /// <summary>
        /// The CMS background light hover
        /// </summary>
        private static readonly Color CMS_BACKGROUND_LIGHT_HOVER = Color.FromArgb(255, 238, 238, 238);
        /// <summary>
        /// The CMS background hover light brush
        /// </summary>
        private static readonly Brush CMS_BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(CMS_BACKGROUND_LIGHT_HOVER);

        /// <summary>
        /// The CMS background dark hover
        /// </summary>
        private static readonly Color CMS_BACKGROUND_DARK_HOVER = Color.FromArgb(38, 204, 204, 204);
        /// <summary>
        /// The CMS background hover dark brush
        /// </summary>
        private static readonly Brush CMS_BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(CMS_BACKGROUND_DARK_HOVER);

        //Application background
        /// <summary>
        /// The background light
        /// </summary>
        private static readonly Color BACKGROUND_LIGHT = Color.FromArgb(255, 255, 255, 255);
        /// <summary>
        /// The background light brush
        /// </summary>
        private static Brush BACKGROUND_LIGHT_BRUSH = new SolidBrush(BACKGROUND_LIGHT);

        /// <summary>
        /// The background dark
        /// </summary>
        private static readonly Color BACKGROUND_DARK = Color.FromArgb(255, 51, 51, 51);
        /// <summary>
        /// The background dark brush
        /// </summary>
        private static Brush BACKGROUND_DARK_BRUSH = new SolidBrush(BACKGROUND_DARK);

        /// <summary>
        /// Actions the bar text.
        /// </summary>
        /// <returns>Color.</returns>
        public Color ACTION_BAR_TEXT()
        {
            return (ColorScheme.PrimaryColor.GetBrightness()<0.5? Color.White: Color.Black);
        }

        /// <summary>
        /// Actions the bar text brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush ACTION_BAR_TEXT_BRUSH()
        {
            return new SolidBrush((ColorScheme.PrimaryColor.GetBrightness() < 0.5 ? Color.White : Color.Black));
        }

        /// <summary>
        /// Actions the bar text secondary.
        /// </summary>
        /// <returns>Color.</returns>
        public Color ACTION_BAR_TEXT_SECONDARY()
        {
            return (ColorScheme.PrimaryColor.GetBrightness() < 0.5 ? Color.White : Color.DarkGray);
        }

        /// <summary>
        /// Actions the bar text secondary brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush ACTION_BAR_TEXT_SECONDARY_BRUSH()
        {
            return new SolidBrush((ColorScheme.PrimaryColor.GetBrightness() < 0.5 ? Color.White : Color.DarkGray));
        }
        /// <summary>
        /// Gets the color of the primary text.
        /// </summary>
        /// <returns>Color.</returns>
        public Color GetPrimaryTextColor()
        {
            return (Theme == Themes.LIGHT ? PRIMARY_TEXT_BLACK : PRIMARY_TEXT_WHITE);
        }

        /// <summary>
        /// Gets the primary text brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetPrimaryTextBrush()
        {
            return (Theme == Themes.LIGHT ? PRIMARY_TEXT_BLACK_BRUSH : PRIMARY_TEXT_WHITE_BRUSH);
		}

        /// <summary>
        /// Gets the color of the secondary text.
        /// </summary>
        /// <returns>Color.</returns>
        public Color GetSecondaryTextColor()
		{
			return (Theme == Themes.LIGHT ? SECONDARY_TEXT_BLACK : SECONDARY_TEXT_WHITE);
		}

        /// <summary>
        /// Gets the secondary text brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetSecondaryTextBrush()
		{
			return (Theme == Themes.LIGHT ? SECONDARY_TEXT_BLACK_BRUSH : SECONDARY_TEXT_WHITE_BRUSH);
		}

        /// <summary>
        /// Gets the color of the disabled or hint.
        /// </summary>
        /// <returns>Color.</returns>
        public Color GetDisabledOrHintColor()
        {
            return (Theme == Themes.LIGHT ? DISABLED_OR_HINT_TEXT_BLACK : DISABLED_OR_HINT_TEXT_WHITE);
        }

        /// <summary>
        /// Gets the disabled or hint brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetDisabledOrHintBrush()
        {
            return (Theme == Themes.LIGHT ? DISABLED_OR_HINT_TEXT_BLACK_BRUSH : DISABLED_OR_HINT_TEXT_WHITE_BRUSH);
        }

        /// <summary>
        /// Gets the color of the dividers.
        /// </summary>
        /// <returns>Color.</returns>
        public Color GetDividersColor()
        {
            return (Theme == Themes.LIGHT ? DIVIDERS_BLACK : DIVIDERS_WHITE);
        }

        /// <summary>
        /// Gets the color of the cards.
        /// </summary>
        /// <returns>Color.</returns>
        public Color GetCardsColor()
        {
            return (Theme == Themes.LIGHT ? CARD_WHITE : CARD_BLACK);
        }

        /// <summary>
        /// Gets the cards brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush getCardsBrush()
        {
            return new SolidBrush(Theme == Themes.LIGHT ? CARD_WHITE : CARD_BLACK);
        }

        /// <summary>
        /// Gets the dividers brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetDividersBrush()
        {
            return (Theme == Themes.LIGHT ? DIVIDERS_BLACK_BRUSH : DIVIDERS_WHITE_BRUSH);
        }

        /// <summary>
        /// Gets the color of the checkbox off.
        /// </summary>
        /// <returns>Color.</returns>
        public Color GetCheckboxOffColor()
        {
            return (Theme == Themes.LIGHT ? CHECKBOX_OFF_LIGHT : CHECKBOX_OFF_DARK);
        }

        /// <summary>
        /// Gets the checkbox off brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetCheckboxOffBrush()
        {
            return (Theme == Themes.LIGHT ? CHECKBOX_OFF_LIGHT_BRUSH : CHECKBOX_OFF_DARK_BRUSH);
        }

        /// <summary>
        /// Gets the color of the CheckBox off disabled.
        /// </summary>
        /// <returns>Color.</returns>
        public Color GetCheckBoxOffDisabledColor()
        {
            return (Theme == Themes.LIGHT ? CHECKBOX_OFF_DISABLED_LIGHT : CHECKBOX_OFF_DISABLED_DARK);
        }

        /// <summary>
        /// Gets the CheckBox off disabled brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetCheckBoxOffDisabledBrush()
        {
            return (Theme == Themes.LIGHT ? CHECKBOX_OFF_DISABLED_LIGHT_BRUSH : CHECKBOX_OFF_DISABLED_DARK_BRUSH);
        }

        /// <summary>
        /// Gets the color of the raised button backround.
        /// </summary>
        /// <returns>Color.</returns>
        public Color getRaisedButtonBackroundColor()
        {
            return RAISED_BUTTON_BACKGROUND;
        }

        /// <summary>
        /// Gets the raised button background brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetRaisedButtonBackgroundBrush()
        {
            return RAISED_BUTTON_BACKGROUND_BRUSH;
        }

        /// <summary>
        /// Gets the raised button text brush.
        /// </summary>
        /// <param name="primary">if set to <c>true</c> [primary].</param>
        /// <returns>Brush.</returns>
        public Brush GetRaisedButtonTextBrush(bool primary)
        {
            return (primary ? ACTION_BAR_TEXT_BRUSH() : RAISED_BUTTON_TEXT_DARK_BRUSH);
        }

        /// <summary>
        /// Gets the color of the flat button hover background.
        /// </summary>
        /// <returns>Color.</returns>
        public Color GetFlatButtonHoverBackgroundColor()
        {
            return (Theme == Themes.LIGHT ? FLAT_BUTTON_BACKGROUND_HOVER_LIGHT : FLAT_BUTTON_BACKGROUND_HOVER_DARK);
        }

        /// <summary>
        /// Gets the flat button hover background brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetFlatButtonHoverBackgroundBrush()
        {
            return (Theme == Themes.LIGHT ? FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH : FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH);
        }

        /// <summary>
        /// Gets the color of the flat button pressed background.
        /// </summary>
        /// <returns>Color.</returns>
        public Color GetFlatButtonPressedBackgroundColor()
        {
            return (Theme == Themes.LIGHT ? FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT : FLAT_BUTTON_BACKGROUND_PRESSED_DARK);
        }

        /// <summary>
        /// Gets the flat button pressed background brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetFlatButtonPressedBackgroundBrush()
        {
            return (Theme == Themes.LIGHT ? FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH : FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH);
        }

        /// <summary>
        /// Gets the flat button disabled text brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetFlatButtonDisabledTextBrush()
        {
            return (Theme == Themes.LIGHT ? FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH : FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH);
        }

        /// <summary>
        /// Gets the CMS selected item brush.
        /// </summary>
        /// <returns>Brush.</returns>
        public Brush GetCmsSelectedItemBrush()
        {
            return (Theme == Themes.LIGHT ? CMS_BACKGROUND_HOVER_LIGHT_BRUSH : CMS_BACKGROUND_HOVER_DARK_BRUSH);
        }

        /// <summary>
        /// Gets the color of the application background.
        /// </summary>
        /// <returns>Color.</returns>
        public Color GetApplicationBackgroundColor()
        {
            return (Theme == Themes.LIGHT ? BACKGROUND_LIGHT : BACKGROUND_DARK);
        }

        //Roboto font
        /// <summary>
        /// The roboto medium 12
        /// </summary>
        public Font ROBOTO_MEDIUM_12;
        /// <summary>
        /// The roboto regular 11
        /// </summary>
        public Font ROBOTO_REGULAR_11;
        /// <summary>
        /// The roboto medium 11
        /// </summary>
        public Font ROBOTO_MEDIUM_11;
        /// <summary>
        /// The roboto medium 10
        /// </summary>
        public Font ROBOTO_MEDIUM_10;

        //Other constants
        /// <summary>
        /// The form padding
        /// </summary>
        public int FORM_PADDING = 14;

        /// <summary>
        /// Adds the font memory resource ex.
        /// </summary>
        /// <param name="pbFont">The pb font.</param>
        /// <param name="cbFont">The cb font.</param>
        /// <param name="pvd">The PVD.</param>
        /// <param name="pcFonts">The pc fonts.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pvd, [In] ref uint pcFonts);

        /// <summary>
        /// Prevents a default instance of the <see cref="MaterialSkinManager"/> class from being created.
        /// </summary>
        private MaterialSkinManager()
        {
            ROBOTO_MEDIUM_12 = new Font(LoadFont(Resources.Roboto_Medium), 12f);
            ROBOTO_MEDIUM_10 = new Font(LoadFont(Resources.Roboto_Medium), 10f);
            ROBOTO_REGULAR_11 = new Font(LoadFont(Resources.Roboto_Regular), 11f);
            ROBOTO_MEDIUM_11 = new Font(LoadFont(Resources.Roboto_Medium), 11f);
			Theme = Themes.DARK;
            ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);
            ColorSchemes = new ColorSchemePresetCollection();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static MaterialSkinManager Instance
        {
            get { return instance ?? (instance = new MaterialSkinManager()); }
        }

        /// <summary>
        /// Adds the form to manage.
        /// </summary>
        /// <param name="materialForm">The material form.</param>
        public void AddFormToManage(MaterialForm materialForm)
        {
            formsToManage.Add(materialForm);
            UpdateBackgrounds();
        }

        /// <summary>
        /// Removes the form to manage.
        /// </summary>
        /// <param name="materialForm">The material form.</param>
        public void RemoveFormToManage(MaterialForm materialForm)
        {
            formsToManage.Remove(materialForm);
        }

        /// <summary>
        /// The private font collection
        /// </summary>
        private readonly PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        /// <summary>
        /// Loads the font.
        /// </summary>
        /// <param name="fontResource">The font resource.</param>
        /// <returns>FontFamily.</returns>
        private FontFamily LoadFont(byte[] fontResource)
        {
            int dataLength = fontResource.Length;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontResource, 0, fontPtr, dataLength);

            uint cFonts = 0;
            AddFontMemResourceEx(fontPtr, (uint)fontResource.Length, IntPtr.Zero, ref cFonts);
            privateFontCollection.AddMemoryFont(fontPtr, dataLength);

            return privateFontCollection.Families.Last();
        }

        /// <summary>
        /// Updates the backgrounds.
        /// </summary>
        private void UpdateBackgrounds()
        {
            var newBackColor = GetApplicationBackgroundColor();
            foreach (var materialForm in formsToManage)
            {
                materialForm.BackColor = newBackColor;
                UpdateControl(materialForm, newBackColor);
            }
        }

        /// <summary>
        /// Updates the tool strip.
        /// </summary>
        /// <param name="toolStrip">The tool strip.</param>
        /// <param name="newBackColor">New color of the back.</param>
        private void UpdateToolStrip(ToolStrip toolStrip, Color newBackColor)
        {
            if (toolStrip == null) return;

            toolStrip.BackColor = newBackColor;
            foreach (ToolStripItem control in toolStrip.Items)
            {
                control.BackColor = newBackColor;
                if (control is MaterialToolStripMenuItem && (control as MaterialToolStripMenuItem).HasDropDownItems)
                {

                    //recursive call
                    UpdateToolStrip((control as MaterialToolStripMenuItem).DropDown, newBackColor);
                }
            }
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        /// <param name="controlToUpdate">The control to update.</param>
        /// <param name="newBackColor">New color of the back.</param>
        private void UpdateControl(Control controlToUpdate, Color newBackColor)
        {
            if (controlToUpdate == null) return;

            if (controlToUpdate.ContextMenuStrip != null)
            {
                UpdateToolStrip(controlToUpdate.ContextMenuStrip, newBackColor);
            }
            var tabControl = controlToUpdate as MaterialTabControl;
            if (tabControl != null)
            {
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    tabPage.BackColor = newBackColor;
                }
            }

            if (controlToUpdate is MaterialDivider)
            {
                controlToUpdate.BackColor = GetDividersColor();
            }

	        if (controlToUpdate is MaterialListView)
	        {
		        controlToUpdate.BackColor = newBackColor;

	        }

            //recursive call
            foreach (Control control in controlToUpdate.Controls)
            {
                UpdateControl(control, newBackColor);
            }

            controlToUpdate.Invalidate();
        }
    }
}
