// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="ColorSchemePresetCollection.cs" company="Zeroit Dev Technlologies">
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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Zeroit.Framework.MaterialWinforms
{
    /// <summary>
    /// Class ColorSchemePresetCollection.
    /// </summary>
    public class ColorSchemePresetCollection
    {
        /// <summary>
        /// The object schemes
        /// </summary>
        private List<ColorSchemePreset> objSchemes;
        /// <summary>
        /// The user schemes
        /// </summary>
        private List<ColorSchemePreset> UserSchemes;
        /// <summary>
        /// The file path
        /// </summary>
        private String FilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSchemePresetCollection"/> class.
        /// </summary>
        public ColorSchemePresetCollection()
        {
            FilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "UserSchemes.xml");
            objSchemes = new List<ColorSchemePreset>();
            AddBaseSchemes();
            LoadUserSchemes();
        }

        /// <summary>
        /// Loads the user schemes.
        /// </summary>
        private void LoadUserSchemes()
        {
            UserSchemes = new List<ColorSchemePreset>();
            if (File.Exists(FilePath))
            {
                XmlSerializer objSerializer = new XmlSerializer(typeof(List<ColorSchemePreset>));
                using (StreamReader objReader = new StreamReader(FilePath))
                {
                    UserSchemes =  (List<ColorSchemePreset>) objSerializer.Deserialize(objReader);
                }

            }

            objSchemes.AddRange(UserSchemes);
        }

        /// <summary>
        /// Adds the base schemes.
        /// </summary>
        private void AddBaseSchemes()
        {
            objSchemes.Add(new ColorSchemePreset("Indigo Pink")
            {
                PrimaryColor = Primary.Indigo500,
                DarkPrimaryColor = Primary.Indigo700,
                LightPrimaryColor = Primary.Indigo100,
                AccentColor = Accent.Pink200,
                TextShade = TextShade.WHITE
            });

            objSchemes.Add(new ColorSchemePreset("BlueGrey LightBlue")
            {
                PrimaryColor = Primary.BlueGrey800,
                DarkPrimaryColor = Primary.BlueGrey900,
                LightPrimaryColor = Primary.BlueGrey500,
                AccentColor = Accent.LightBlue200,
                TextShade = TextShade.WHITE
            });

            objSchemes.Add(new ColorSchemePreset("Green Red")
            {
                PrimaryColor = Primary.Green600,
                DarkPrimaryColor = Primary.Green700,
                LightPrimaryColor = Primary.Green200,
                AccentColor = Accent.Red100,
                TextShade = TextShade.WHITE
            });

            objSchemes.Add(new ColorSchemePreset("Purple Green")
            {
                PrimaryColor = Primary.Purple500,
                DarkPrimaryColor = Primary.Purple700,
                LightPrimaryColor = Primary.Purple200,
                AccentColor = Accent.Green200,
                TextShade = TextShade.WHITE
            });

            objSchemes.Add(new ColorSchemePreset("Black White")
            {
                PrimaryColor = Primary.Black,
                DarkPrimaryColor = Primary.Black,
                LightPrimaryColor = Primary.Grey900,
                AccentColor = Accent.White,
                TextShade = TextShade.WHITE
            });

            objSchemes.Add(new ColorSchemePreset("White Black")
            {
                PrimaryColor = Primary.White,
                DarkPrimaryColor = Primary.Grey100,
                LightPrimaryColor = Primary.White,
                AccentColor = Accent.Black,
                TextShade = TextShade.BLACK
            });

            objSchemes.Add(new ColorSchemePreset("Black Red")
            {
                PrimaryColor = Primary.Black,
                DarkPrimaryColor = Primary.Black,
                LightPrimaryColor = Primary.Grey900,
                AccentColor = Accent.Red200,
                TextShade = TextShade.WHITE
            });

            objSchemes.Add(new ColorSchemePreset("Brown Cyan")
            {
                PrimaryColor = Primary.Brown500,
                DarkPrimaryColor = Primary.Brown700,
                LightPrimaryColor = Primary.Brown200,
                AccentColor = Accent.Green200,
                TextShade = TextShade.WHITE
            });

            objSchemes.Add(new ColorSchemePreset("Amber Red")
            {
                PrimaryColor = Primary.Amber500,
                DarkPrimaryColor = Primary.Amber700,
                LightPrimaryColor = Primary.Amber200,
                AccentColor = Accent.Red100,
                TextShade = TextShade.WHITE
            });

            objSchemes.Add(new ColorSchemePreset("Purple LightBlue")
            {
                PrimaryColor = Primary.Purple500,
                DarkPrimaryColor = Primary.Purple700,
                LightPrimaryColor = Primary.Purple200,
                AccentColor = Accent.LightBlue200,
                TextShade = TextShade.WHITE
            });
        }

        /// <summary>
        /// Gets the specified index.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <returns>ColorSchemePreset.</returns>
        public ColorSchemePreset get(int Index)
        {
            return objSchemes[Index];
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns>ColorSchemePreset.</returns>
        public ColorSchemePreset get(String Name)
        {
            return objSchemes.FirstOrDefault(Scheme => Scheme.Name == Name);
        }

        /// <summary>
        /// Adds the specified color scheme preset.
        /// </summary>
        /// <param name="ColorSchemePreset">The color scheme preset.</param>
        /// <returns>System.Int32.</returns>
        public int add(ColorSchemePreset ColorSchemePreset)
        {

            objSchemes.Add(ColorSchemePreset);
            UserSchemes.Add(ColorSchemePreset);
            SaveUserSchemes();
            return objSchemes.Count - 1;
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns>List&lt;ColorSchemePreset&gt;.</returns>
        public List<ColorSchemePreset> List()
        {
            return objSchemes;
        }

        /// <summary>
        /// Saves the user schemes.
        /// </summary>
        private void SaveUserSchemes()
        {
            XmlSerializer objSerializer = new XmlSerializer(typeof(List<ColorSchemePreset>));
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    objSerializer.Serialize(writer, UserSchemes);
                    String xml = sww.ToString(); // Your XML
                    File.WriteAllText(FilePath, xml);
                }

            }
        }

    }
}
