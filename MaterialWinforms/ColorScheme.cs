// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="ColorScheme.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Drawing;

namespace Zeroit.Framework.MaterialWinforms
{
    /// <summary>
    /// Class ColorScheme.
    /// </summary>
    public class ColorScheme
    {
        /// <summary>
        /// The primary color
        /// </summary>
        public readonly Color PrimaryColor, DarkPrimaryColor, LightPrimaryColor, AccentColor, TextColor;
        /// <summary>
        /// The primary pen
        /// </summary>
        public readonly Pen PrimaryPen, DarkPrimaryPen, LightPrimaryPen, AccentPen, TextPen;
        /// <summary>
        /// The primary brush
        /// </summary>
        public readonly Brush PrimaryBrush, DarkPrimaryBrush, LightPrimaryBrush, AccentBrush, TextBrush;

        /// <summary>
        /// Defines the Color Scheme to be used for all forms.
        /// </summary>
        /// <param name="primary">The primary color, a -500 color is suggested here.</param>
        /// <param name="darkPrimary">A darker version of the primary color, a -700 color is suggested here.</param>
        /// <param name="lightPrimary">A lighter version of the primary color, a -100 color is suggested here.</param>
        /// <param name="accent">The accent color, a -200 color is suggested here.</param>
        /// <param name="textShade">The text color, the one with the highest contrast is suggested.</param>
        public ColorScheme(Primary primary, Primary darkPrimary, Primary lightPrimary, Accent accent, TextShade textShade)
        {
            //Color
            PrimaryColor = ((int)primary).ToColor();
            DarkPrimaryColor = ((int)darkPrimary).ToColor();
            LightPrimaryColor = ((int)lightPrimary).ToColor();
            AccentColor = ((int)accent).ToColor();
            TextColor = ((int)textShade).ToColor();

            //Pen
            PrimaryPen = new Pen(PrimaryColor);
            DarkPrimaryPen = new Pen(DarkPrimaryColor);
            LightPrimaryPen = new Pen(LightPrimaryColor);
            AccentPen = new Pen(AccentColor);
            TextPen = new Pen(TextColor);

            //Brush
            PrimaryBrush = new SolidBrush(PrimaryColor);
            DarkPrimaryBrush = new SolidBrush(DarkPrimaryColor);
            LightPrimaryBrush = new SolidBrush(LightPrimaryColor);
            AccentBrush = new SolidBrush(AccentColor);
            TextBrush = new SolidBrush(TextColor);
        }

    }


    /// <summary>
    /// Class ColorExtension.
    /// </summary>
    public static class ColorExtension
    {
        /// <summary>
        /// Convert an integer number to a Color.
        /// </summary>
        /// <param name="argb">The ARGB.</param>
        /// <returns>Color.</returns>
        public static Color ToColor(this int argb)
        {
            return Color.FromArgb(
                (argb & 0xff0000) >> 16,
                (argb & 0xff00) >> 8,
                 argb & 0xff);
        }

        /// <summary>
        /// Removes the alpha component of a color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>Color.</returns>
        public static Color RemoveAlpha(this Color color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts a 0-100 integer to a 0-255 color component.
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        /// <returns>System.Int32.</returns>
        public static int PercentageToColorComponent(this int percentage)
        {
            return (int)((percentage / 100d) * 255d);
        }
    }

    //Color constantes
    /// <summary>
    /// Enum TextShade
    /// </summary>
    public enum TextShade
    {
        /// <summary>
        /// The white
        /// </summary>
        WHITE = 0xFFFFFF,
        /// <summary>
        /// The black
        /// </summary>
        BLACK = 0x212121
    }

    /// <summary>
    /// Enum Primary
    /// </summary>
    public enum Primary
    {
        /// <summary>
        /// The red50
        /// </summary>
        Red50 = 0xFFEBEE,
        /// <summary>
        /// The red100
        /// </summary>
        Red100 = 0xFFCDD2,
        /// <summary>
        /// The red200
        /// </summary>
        Red200 = 0xEF9A9A,
        /// <summary>
        /// The red300
        /// </summary>
        Red300 = 0xE57373,
        /// <summary>
        /// The red400
        /// </summary>
        Red400 = 0xEF5350,
        /// <summary>
        /// The red500
        /// </summary>
        Red500 = 0xF44336,
        /// <summary>
        /// The red600
        /// </summary>
        Red600 = 0xE53935,
        /// <summary>
        /// The red700
        /// </summary>
        Red700 = 0xD32F2F,
        /// <summary>
        /// The red800
        /// </summary>
        Red800 = 0xC62828,
        /// <summary>
        /// The red900
        /// </summary>
        Red900 = 0xB71C1C,
        /// <summary>
        /// The pink50
        /// </summary>
        Pink50 = 0xFCE4EC,
        /// <summary>
        /// The pink100
        /// </summary>
        Pink100 = 0xF8BBD0,
        /// <summary>
        /// The pink200
        /// </summary>
        Pink200 = 0xF48FB1,
        /// <summary>
        /// The pink300
        /// </summary>
        Pink300 = 0xF06292,
        /// <summary>
        /// The pink400
        /// </summary>
        Pink400 = 0xEC407A,
        /// <summary>
        /// The pink500
        /// </summary>
        Pink500 = 0xE91E63,
        /// <summary>
        /// The pink600
        /// </summary>
        Pink600 = 0xD81B60,
        /// <summary>
        /// The pink700
        /// </summary>
        Pink700 = 0xC2185B,
        /// <summary>
        /// The pink800
        /// </summary>
        Pink800 = 0xAD1457,
        /// <summary>
        /// The pink900
        /// </summary>
        Pink900 = 0x880E4F,
        /// <summary>
        /// The purple50
        /// </summary>
        Purple50 = 0xF3E5F5,
        /// <summary>
        /// The purple100
        /// </summary>
        Purple100 = 0xE1BEE7,
        /// <summary>
        /// The purple200
        /// </summary>
        Purple200 = 0xCE93D8,
        /// <summary>
        /// The purple300
        /// </summary>
        Purple300 = 0xBA68C8,
        /// <summary>
        /// The purple400
        /// </summary>
        Purple400 = 0xAB47BC,
        /// <summary>
        /// The purple500
        /// </summary>
        Purple500 = 0x9C27B0,
        /// <summary>
        /// The purple600
        /// </summary>
        Purple600 = 0x8E24AA,
        /// <summary>
        /// The purple700
        /// </summary>
        Purple700 = 0x7B1FA2,
        /// <summary>
        /// The purple800
        /// </summary>
        Purple800 = 0x6A1B9A,
        /// <summary>
        /// The purple900
        /// </summary>
        Purple900 = 0x4A148C,
        /// <summary>
        /// The deep purple50
        /// </summary>
        DeepPurple50 = 0xEDE7F6,
        /// <summary>
        /// The deep purple100
        /// </summary>
        DeepPurple100 = 0xD1C4E9,
        /// <summary>
        /// The deep purple200
        /// </summary>
        DeepPurple200 = 0xB39DDB,
        /// <summary>
        /// The deep purple300
        /// </summary>
        DeepPurple300 = 0x9575CD,
        /// <summary>
        /// The deep purple400
        /// </summary>
        DeepPurple400 = 0x7E57C2,
        /// <summary>
        /// The deep purple500
        /// </summary>
        DeepPurple500 = 0x673AB7,
        /// <summary>
        /// The deep purple600
        /// </summary>
        DeepPurple600 = 0x5E35B1,
        /// <summary>
        /// The deep purple700
        /// </summary>
        DeepPurple700 = 0x512DA8,
        /// <summary>
        /// The deep purple800
        /// </summary>
        DeepPurple800 = 0x4527A0,
        /// <summary>
        /// The deep purple900
        /// </summary>
        DeepPurple900 = 0x311B92,
        /// <summary>
        /// The indigo50
        /// </summary>
        Indigo50 = 0xE8EAF6,
        /// <summary>
        /// The indigo100
        /// </summary>
        Indigo100 = 0xC5CAE9,
        /// <summary>
        /// The indigo200
        /// </summary>
        Indigo200 = 0x9FA8DA,
        /// <summary>
        /// The indigo300
        /// </summary>
        Indigo300 = 0x7986CB,
        /// <summary>
        /// The indigo400
        /// </summary>
        Indigo400 = 0x5C6BC0,
        /// <summary>
        /// The indigo500
        /// </summary>
        Indigo500 = 0x3F51B5,
        /// <summary>
        /// The indigo600
        /// </summary>
        Indigo600 = 0x3949AB,
        /// <summary>
        /// The indigo700
        /// </summary>
        Indigo700 = 0x303F9F,
        /// <summary>
        /// The indigo800
        /// </summary>
        Indigo800 = 0x283593,
        /// <summary>
        /// The indigo900
        /// </summary>
        Indigo900 = 0x1A237E,
        /// <summary>
        /// The blue50
        /// </summary>
        Blue50 = 0xE3F2FD,
        /// <summary>
        /// The blue100
        /// </summary>
        Blue100 = 0xBBDEFB,
        /// <summary>
        /// The blue200
        /// </summary>
        Blue200 = 0x90CAF9,
        /// <summary>
        /// The blue300
        /// </summary>
        Blue300 = 0x64B5F6,
        /// <summary>
        /// The blue400
        /// </summary>
        Blue400 = 0x42A5F5,
        /// <summary>
        /// The blue500
        /// </summary>
        Blue500 = 0x2196F3,
        /// <summary>
        /// The blue600
        /// </summary>
        Blue600 = 0x1E88E5,
        /// <summary>
        /// The blue700
        /// </summary>
        Blue700 = 0x1976D2,
        /// <summary>
        /// The blue800
        /// </summary>
        Blue800 = 0x1565C0,
        /// <summary>
        /// The blue900
        /// </summary>
        Blue900 = 0x0D47A1,
        /// <summary>
        /// The light blue50
        /// </summary>
        LightBlue50 = 0xE1F5FE,
        /// <summary>
        /// The light blue100
        /// </summary>
        LightBlue100 = 0xB3E5FC,
        /// <summary>
        /// The light blue200
        /// </summary>
        LightBlue200 = 0x81D4FA,
        /// <summary>
        /// The light blue300
        /// </summary>
        LightBlue300 = 0x4FC3F7,
        /// <summary>
        /// The light blue400
        /// </summary>
        LightBlue400 = 0x29B6F6,
        /// <summary>
        /// The light blue500
        /// </summary>
        LightBlue500 = 0x03A9F4,
        /// <summary>
        /// The light blue600
        /// </summary>
        LightBlue600 = 0x039BE5,
        /// <summary>
        /// The light blue700
        /// </summary>
        LightBlue700 = 0x0288D1,
        /// <summary>
        /// The light blue800
        /// </summary>
        LightBlue800 = 0x0277BD,
        /// <summary>
        /// The light blue900
        /// </summary>
        LightBlue900 = 0x01579B,
        /// <summary>
        /// The cyan50
        /// </summary>
        Cyan50 = 0xE0F7FA,
        /// <summary>
        /// The cyan100
        /// </summary>
        Cyan100 = 0xB2EBF2,
        /// <summary>
        /// The cyan200
        /// </summary>
        Cyan200 = 0x80DEEA,
        /// <summary>
        /// The cyan300
        /// </summary>
        Cyan300 = 0x4DD0E1,
        /// <summary>
        /// The cyan400
        /// </summary>
        Cyan400 = 0x26C6DA,
        /// <summary>
        /// The cyan500
        /// </summary>
        Cyan500 = 0x00BCD4,
        /// <summary>
        /// The cyan600
        /// </summary>
        Cyan600 = 0x00ACC1,
        /// <summary>
        /// The cyan700
        /// </summary>
        Cyan700 = 0x0097A7,
        /// <summary>
        /// The cyan800
        /// </summary>
        Cyan800 = 0x00838F,
        /// <summary>
        /// The cyan900
        /// </summary>
        Cyan900 = 0x006064,
        /// <summary>
        /// The teal50
        /// </summary>
        Teal50 = 0xE0F2F1,
        /// <summary>
        /// The teal100
        /// </summary>
        Teal100 = 0xB2DFDB,
        /// <summary>
        /// The teal200
        /// </summary>
        Teal200 = 0x80CBC4,
        /// <summary>
        /// The teal300
        /// </summary>
        Teal300 = 0x4DB6AC,
        /// <summary>
        /// The teal400
        /// </summary>
        Teal400 = 0x26A69A,
        /// <summary>
        /// The teal500
        /// </summary>
        Teal500 = 0x009688,
        /// <summary>
        /// The teal600
        /// </summary>
        Teal600 = 0x00897B,
        /// <summary>
        /// The teal700
        /// </summary>
        Teal700 = 0x00796B,
        /// <summary>
        /// The teal800
        /// </summary>
        Teal800 = 0x00695C,
        /// <summary>
        /// The teal900
        /// </summary>
        Teal900 = 0x004D40,
        /// <summary>
        /// The green50
        /// </summary>
        Green50 = 0xE8F5E9,
        /// <summary>
        /// The green100
        /// </summary>
        Green100 = 0xC8E6C9,
        /// <summary>
        /// The green200
        /// </summary>
        Green200 = 0xA5D6A7,
        /// <summary>
        /// The green300
        /// </summary>
        Green300 = 0x81C784,
        /// <summary>
        /// The green400
        /// </summary>
        Green400 = 0x66BB6A,
        /// <summary>
        /// The green500
        /// </summary>
        Green500 = 0x4CAF50,
        /// <summary>
        /// The green600
        /// </summary>
        Green600 = 0x43A047,
        /// <summary>
        /// The green700
        /// </summary>
        Green700 = 0x388E3C,
        /// <summary>
        /// The green800
        /// </summary>
        Green800 = 0x2E7D32,
        /// <summary>
        /// The green900
        /// </summary>
        Green900 = 0x1B5E20,
        /// <summary>
        /// The light green50
        /// </summary>
        LightGreen50 = 0xF1F8E9,
        /// <summary>
        /// The light green100
        /// </summary>
        LightGreen100 = 0xDCEDC8,
        /// <summary>
        /// The light green200
        /// </summary>
        LightGreen200 = 0xC5E1A5,
        /// <summary>
        /// The light green300
        /// </summary>
        LightGreen300 = 0xAED581,
        /// <summary>
        /// The light green400
        /// </summary>
        LightGreen400 = 0x9CCC65,
        /// <summary>
        /// The light green500
        /// </summary>
        LightGreen500 = 0x8BC34A,
        /// <summary>
        /// The light green600
        /// </summary>
        LightGreen600 = 0x7CB342,
        /// <summary>
        /// The light green700
        /// </summary>
        LightGreen700 = 0x689F38,
        /// <summary>
        /// The light green800
        /// </summary>
        LightGreen800 = 0x558B2F,
        /// <summary>
        /// The light green900
        /// </summary>
        LightGreen900 = 0x33691E,
        /// <summary>
        /// The lime50
        /// </summary>
        Lime50 = 0xF9FBE7,
        /// <summary>
        /// The lime100
        /// </summary>
        Lime100 = 0xF0F4C3,
        /// <summary>
        /// The lime200
        /// </summary>
        Lime200 = 0xE6EE9C,
        /// <summary>
        /// The lime300
        /// </summary>
        Lime300 = 0xDCE775,
        /// <summary>
        /// The lime400
        /// </summary>
        Lime400 = 0xD4E157,
        /// <summary>
        /// The lime500
        /// </summary>
        Lime500 = 0xCDDC39,
        /// <summary>
        /// The lime600
        /// </summary>
        Lime600 = 0xC0CA33,
        /// <summary>
        /// The lime700
        /// </summary>
        Lime700 = 0xAFB42B,
        /// <summary>
        /// The lime800
        /// </summary>
        Lime800 = 0x9E9D24,
        /// <summary>
        /// The lime900
        /// </summary>
        Lime900 = 0x827717,
        /// <summary>
        /// The yellow50
        /// </summary>
        Yellow50 = 0xFFFDE7,
        /// <summary>
        /// The yellow100
        /// </summary>
        Yellow100 = 0xFFF9C4,
        /// <summary>
        /// The yellow200
        /// </summary>
        Yellow200 = 0xFFF59D,
        /// <summary>
        /// The yellow300
        /// </summary>
        Yellow300 = 0xFFF176,
        /// <summary>
        /// The yellow400
        /// </summary>
        Yellow400 = 0xFFEE58,
        /// <summary>
        /// The yellow500
        /// </summary>
        Yellow500 = 0xFFEB3B,
        /// <summary>
        /// The yellow600
        /// </summary>
        Yellow600 = 0xFDD835,
        /// <summary>
        /// The yellow700
        /// </summary>
        Yellow700 = 0xFBC02D,
        /// <summary>
        /// The yellow800
        /// </summary>
        Yellow800 = 0xF9A825,
        /// <summary>
        /// The yellow900
        /// </summary>
        Yellow900 = 0xF57F17,
        /// <summary>
        /// The amber50
        /// </summary>
        Amber50 = 0xFFF8E1,
        /// <summary>
        /// The amber100
        /// </summary>
        Amber100 = 0xFFECB3,
        /// <summary>
        /// The amber200
        /// </summary>
        Amber200 = 0xFFE082,
        /// <summary>
        /// The amber300
        /// </summary>
        Amber300 = 0xFFD54F,
        /// <summary>
        /// The amber400
        /// </summary>
        Amber400 = 0xFFCA28,
        /// <summary>
        /// The amber500
        /// </summary>
        Amber500 = 0xFFC107,
        /// <summary>
        /// The amber600
        /// </summary>
        Amber600 = 0xFFB300,
        /// <summary>
        /// The amber700
        /// </summary>
        Amber700 = 0xFFA000,
        /// <summary>
        /// The amber800
        /// </summary>
        Amber800 = 0xFF8F00,
        /// <summary>
        /// The amber900
        /// </summary>
        Amber900 = 0xFF6F00,
        /// <summary>
        /// The orange50
        /// </summary>
        Orange50 = 0xFFF3E0,
        /// <summary>
        /// The orange100
        /// </summary>
        Orange100 = 0xFFE0B2,
        /// <summary>
        /// The orange200
        /// </summary>
        Orange200 = 0xFFCC80,
        /// <summary>
        /// The orange300
        /// </summary>
        Orange300 = 0xFFB74D,
        /// <summary>
        /// The orange400
        /// </summary>
        Orange400 = 0xFFA726,
        /// <summary>
        /// The orange500
        /// </summary>
        Orange500 = 0xFF9800,
        /// <summary>
        /// The orange600
        /// </summary>
        Orange600 = 0xFB8C00,
        /// <summary>
        /// The orange700
        /// </summary>
        Orange700 = 0xF57C00,
        /// <summary>
        /// The orange800
        /// </summary>
        Orange800 = 0xEF6C00,
        /// <summary>
        /// The orange900
        /// </summary>
        Orange900 = 0xE65100,
        /// <summary>
        /// The deep orange50
        /// </summary>
        DeepOrange50 = 0xFBE9E7,
        /// <summary>
        /// The deep orange100
        /// </summary>
        DeepOrange100 = 0xFFCCBC,
        /// <summary>
        /// The deep orange200
        /// </summary>
        DeepOrange200 = 0xFFAB91,
        /// <summary>
        /// The deep orange300
        /// </summary>
        DeepOrange300 = 0xFF8A65,
        /// <summary>
        /// The deep orange400
        /// </summary>
        DeepOrange400 = 0xFF7043,
        /// <summary>
        /// The deep orange500
        /// </summary>
        DeepOrange500 = 0xFF5722,
        /// <summary>
        /// The deep orange600
        /// </summary>
        DeepOrange600 = 0xF4511E,
        /// <summary>
        /// The deep orange700
        /// </summary>
        DeepOrange700 = 0xE64A19,
        /// <summary>
        /// The deep orange800
        /// </summary>
        DeepOrange800 = 0xD84315,
        /// <summary>
        /// The deep orange900
        /// </summary>
        DeepOrange900 = 0xBF360C,
        /// <summary>
        /// The brown50
        /// </summary>
        Brown50 = 0xEFEBE9,
        /// <summary>
        /// The brown100
        /// </summary>
        Brown100 = 0xD7CCC8,
        /// <summary>
        /// The brown200
        /// </summary>
        Brown200 = 0xBCAAA4,
        /// <summary>
        /// The brown300
        /// </summary>
        Brown300 = 0xA1887F,
        /// <summary>
        /// The brown400
        /// </summary>
        Brown400 = 0x8D6E63,
        /// <summary>
        /// The brown500
        /// </summary>
        Brown500 = 0x795548,
        /// <summary>
        /// The brown600
        /// </summary>
        Brown600 = 0x6D4C41,
        /// <summary>
        /// The brown700
        /// </summary>
        Brown700 = 0x5D4037,
        /// <summary>
        /// The brown800
        /// </summary>
        Brown800 = 0x4E342E,
        /// <summary>
        /// The brown900
        /// </summary>
        Brown900 = 0x3E2723,
        /// <summary>
        /// The grey50
        /// </summary>
        Grey50 = 0xFAFAFA,
        /// <summary>
        /// The grey100
        /// </summary>
        Grey100 = 0xF5F5F5,
        /// <summary>
        /// The grey200
        /// </summary>
        Grey200 = 0xEEEEEE,
        /// <summary>
        /// The grey300
        /// </summary>
        Grey300 = 0xE0E0E0,
        /// <summary>
        /// The grey400
        /// </summary>
        Grey400 = 0xBDBDBD,
        /// <summary>
        /// The grey500
        /// </summary>
        Grey500 = 0x9E9E9E,
        /// <summary>
        /// The grey600
        /// </summary>
        Grey600 = 0x757575,
        /// <summary>
        /// The grey700
        /// </summary>
        Grey700 = 0x616161,
        /// <summary>
        /// The grey800
        /// </summary>
        Grey800 = 0x424242,
        /// <summary>
        /// The grey900
        /// </summary>
        Grey900 = 0x212121,
        /// <summary>
        /// The blue grey50
        /// </summary>
        BlueGrey50 = 0xECEFF1,
        /// <summary>
        /// The blue grey100
        /// </summary>
        BlueGrey100 = 0xCFD8DC,
        /// <summary>
        /// The blue grey200
        /// </summary>
        BlueGrey200 = 0xB0BEC5,
        /// <summary>
        /// The blue grey300
        /// </summary>
        BlueGrey300 = 0x90A4AE,
        /// <summary>
        /// The blue grey400
        /// </summary>
        BlueGrey400 = 0x78909C,
        /// <summary>
        /// The blue grey500
        /// </summary>
        BlueGrey500 = 0x607D8B,
        /// <summary>
        /// The blue grey600
        /// </summary>
        BlueGrey600 = 0x546E7A,
        /// <summary>
        /// The blue grey700
        /// </summary>
        BlueGrey700 = 0x455A64,
        /// <summary>
        /// The blue grey800
        /// </summary>
        BlueGrey800 = 0x37474F,
        /// <summary>
        /// The blue grey900
        /// </summary>
        BlueGrey900 = 0x263238,
        /// <summary>
        /// The black
        /// </summary>
        Black = 0x000000,
        /// <summary>
        /// The white
        /// </summary>
        White = 0xffffff,
    }

    /// <summary>
    /// Enum Accent
    /// </summary>
    public enum Accent
    {
        /// <summary>
        /// The red100
        /// </summary>
        Red100 = 0xFF8A80,
        /// <summary>
        /// The red200
        /// </summary>
        Red200 = 0xFF5252,
        /// <summary>
        /// The red400
        /// </summary>
        Red400 = 0xFF1744,
        /// <summary>
        /// The red700
        /// </summary>
        Red700 = 0xD50000,
        /// <summary>
        /// The pink100
        /// </summary>
        Pink100 = 0xFF80AB,
        /// <summary>
        /// The pink200
        /// </summary>
        Pink200 = 0xFF4081,
        /// <summary>
        /// The pink400
        /// </summary>
        Pink400 = 0xF50057,
        /// <summary>
        /// The pink700
        /// </summary>
        Pink700 = 0xC51162,
        /// <summary>
        /// The purple100
        /// </summary>
        Purple100 = 0xEA80FC,
        /// <summary>
        /// The purple200
        /// </summary>
        Purple200 = 0xE040FB,
        /// <summary>
        /// The purple400
        /// </summary>
        Purple400 = 0xD500F9,
        /// <summary>
        /// The purple700
        /// </summary>
        Purple700 = 0xAA00FF,
        /// <summary>
        /// The deep purple100
        /// </summary>
        DeepPurple100 = 0xB388FF,
        /// <summary>
        /// The deep purple200
        /// </summary>
        DeepPurple200 = 0x7C4DFF,
        /// <summary>
        /// The deep purple400
        /// </summary>
        DeepPurple400 = 0x651FFF,
        /// <summary>
        /// The deep purple700
        /// </summary>
        DeepPurple700 = 0x6200EA,
        /// <summary>
        /// The indigo100
        /// </summary>
        Indigo100 = 0x8C9EFF,
        /// <summary>
        /// The indigo200
        /// </summary>
        Indigo200 = 0x536DFE,
        /// <summary>
        /// The indigo400
        /// </summary>
        Indigo400 = 0x3D5AFE,
        /// <summary>
        /// The indigo700
        /// </summary>
        Indigo700 = 0x304FFE,
        /// <summary>
        /// The blue100
        /// </summary>
        Blue100 = 0x82B1FF,
        /// <summary>
        /// The blue200
        /// </summary>
        Blue200 = 0x448AFF,
        /// <summary>
        /// The blue400
        /// </summary>
        Blue400 = 0x2979FF,
        /// <summary>
        /// The blue700
        /// </summary>
        Blue700 = 0x2962FF,
        /// <summary>
        /// The light blue100
        /// </summary>
        LightBlue100 = 0x80D8FF,
        /// <summary>
        /// The light blue200
        /// </summary>
        LightBlue200 = 0x40C4FF,
        /// <summary>
        /// The light blue400
        /// </summary>
        LightBlue400 = 0x00B0FF,
        /// <summary>
        /// The light blue700
        /// </summary>
        LightBlue700 = 0x0091EA,
        /// <summary>
        /// The cyan100
        /// </summary>
        Cyan100 = 0x84FFFF,
        /// <summary>
        /// The cyan200
        /// </summary>
        Cyan200 = 0x18FFFF,
        /// <summary>
        /// The cyan400
        /// </summary>
        Cyan400 = 0x00E5FF,
        /// <summary>
        /// The cyan700
        /// </summary>
        Cyan700 = 0x00B8D4,
        /// <summary>
        /// The teal100
        /// </summary>
        Teal100 = 0xA7FFEB,
        /// <summary>
        /// The teal200
        /// </summary>
        Teal200 = 0x64FFDA,
        /// <summary>
        /// The teal400
        /// </summary>
        Teal400 = 0x1DE9B6,
        /// <summary>
        /// The teal700
        /// </summary>
        Teal700 = 0x00BFA5,
        /// <summary>
        /// The green100
        /// </summary>
        Green100 = 0xB9F6CA,
        /// <summary>
        /// The green200
        /// </summary>
        Green200 = 0x69F0AE,
        /// <summary>
        /// The green400
        /// </summary>
        Green400 = 0x00E676,
        /// <summary>
        /// The green700
        /// </summary>
        Green700 = 0x00C853,
        /// <summary>
        /// The light green100
        /// </summary>
        LightGreen100 = 0xCCFF90,
        /// <summary>
        /// The light green200
        /// </summary>
        LightGreen200 = 0xB2FF59,
        /// <summary>
        /// The light green400
        /// </summary>
        LightGreen400 = 0x76FF03,
        /// <summary>
        /// The light green700
        /// </summary>
        LightGreen700 = 0x64DD17,
        /// <summary>
        /// The lime100
        /// </summary>
        Lime100 = 0xF4FF81,
        /// <summary>
        /// The lime200
        /// </summary>
        Lime200 = 0xEEFF41,
        /// <summary>
        /// The lime400
        /// </summary>
        Lime400 = 0xC6FF00,
        /// <summary>
        /// The lime700
        /// </summary>
        Lime700 = 0xAEEA00,
        /// <summary>
        /// The yellow100
        /// </summary>
        Yellow100 = 0xFFFF8D,
        /// <summary>
        /// The yellow200
        /// </summary>
        Yellow200 = 0xFFFF00,
        /// <summary>
        /// The yellow400
        /// </summary>
        Yellow400 = 0xFFEA00,
        /// <summary>
        /// The yellow700
        /// </summary>
        Yellow700 = 0xFFD600,
        /// <summary>
        /// The amber100
        /// </summary>
        Amber100 = 0xFFE57F,
        /// <summary>
        /// The amber200
        /// </summary>
        Amber200 = 0xFFD740,
        /// <summary>
        /// The amber400
        /// </summary>
        Amber400 = 0xFFC400,
        /// <summary>
        /// The amber700
        /// </summary>
        Amber700 = 0xFFAB00,
        /// <summary>
        /// The orange100
        /// </summary>
        Orange100 = 0xFFD180,
        /// <summary>
        /// The orange200
        /// </summary>
        Orange200 = 0xFFAB40,
        /// <summary>
        /// The orange400
        /// </summary>
        Orange400 = 0xFF9100,
        /// <summary>
        /// The orange700
        /// </summary>
        Orange700 = 0xFF6D00,
        /// <summary>
        /// The deep orange100
        /// </summary>
        DeepOrange100 = 0xFF9E80,
        /// <summary>
        /// The deep orange200
        /// </summary>
        DeepOrange200 = 0xFF6E40,
        /// <summary>
        /// The deep orange400
        /// </summary>
        DeepOrange400 = 0xFF3D00,
        /// <summary>
        /// The deep orange700
        /// </summary>
        DeepOrange700 = 0xDD2C00,
        /// <summary>
        /// The black
        /// </summary>
        Black = 0x000000,
        /// <summary>
        /// The white
        /// </summary>
        White = 0xffffff
    }

    /// <summary>
    /// Class ColorSchemePreset.
    /// </summary>
    public class ColorSchemePreset
    {
        /// <summary>
        /// The primary color
        /// </summary>
        public Primary PrimaryColor;
        /// <summary>
        /// The dark primary color
        /// </summary>
        public Primary DarkPrimaryColor;
        /// <summary>
        /// The light primary color
        /// </summary>
        public Primary LightPrimaryColor;
        /// <summary>
        /// The accent color
        /// </summary>
        public Accent AccentColor;
        /// <summary>
        /// The text shade
        /// </summary>
        public TextShade TextShade;

        /// <summary>
        /// The name
        /// </summary>
        public string Name;
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSchemePreset"/> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        public ColorSchemePreset(string Name)
        {
            this.Name = Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSchemePreset"/> class.
        /// </summary>
        public ColorSchemePreset()
        {

        }


    }
}
