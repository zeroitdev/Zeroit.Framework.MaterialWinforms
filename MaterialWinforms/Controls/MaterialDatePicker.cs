// ***********************************************************************
// Assembly         : Zeroit.Framework.MaterialWinforms
// Author           : ZEROIT
// Created          : 11-22-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-18-2018
// ***********************************************************************
// <copyright file="MaterialDatePicker.cs" company="Zeroit Dev Technlologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
namespace Zeroit.Framework.MaterialWinforms.Controls
{
    /// <summary>
    /// Class MaterialDatePicker.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="Zeroit.Framework.MaterialWinforms.IMaterialControl" />
    public partial class MaterialDatePicker : Control, IMaterialControl
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
        public new Color BackColor { get { return SkinManager.GetCardsColor(); } set { } }

        /// <summary>
        /// The top day rect
        /// </summary>
        private RectangleF TopDayRect;
        /// <summary>
        /// The top date rect
        /// </summary>
        private RectangleF TopDateRect;
        /// <summary>
        /// The month rect
        /// </summary>
        private RectangleF MonthRect;
        /// <summary>
        /// The day rect
        /// </summary>
        private RectangleF DayRect;
        /// <summary>
        /// The year rect
        /// </summary>
        private RectangleF YearRect;

        /// <summary>
        /// The current cal header
        /// </summary>
        private RectangleF CurrentCal_Header;

        /// <summary>
        /// The top day font
        /// </summary>
        private Font TopDayFont, MonthFont, DayFont, YeahrFont;

        /// <summary>
        /// The current cal
        /// </summary>
        private RectangleF CurrentCal;
        /// <summary>
        /// The previous cal
        /// </summary>
        private RectangleF PreviousCal;
        /// <summary>
        /// The next cal
        /// </summary>
        private RectangleF NextCal;
        /// <summary>
        /// The shadow path
        /// </summary>
        private GraphicsPath ShadowPath;
        /// <summary>
        /// The current date
        /// </summary>
        private DateTime CurrentDate;
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get { return CurrentDate; } set { CurrentDate = value; Invalidate(); } }
        /// <summary>
        /// The date rectangles
        /// </summary>
        private List<List<DateRect>> DateRectangles;

        /// <summary>
        /// The date rect default size
        /// </summary>
        private int DateRectDefaultSize;
        /// <summary>
        /// The hover x
        /// </summary>
        private int HoverX;
        /// <summary>
        /// The hover y
        /// </summary>
        private int HoverY;
        /// <summary>
        /// The selected x
        /// </summary>
        private int SelectedX;
        /// <summary>
        /// The selected y
        /// </summary>
        private int SelectedY;
        /// <summary>
        /// The recent hovered
        /// </summary>
        private bool recentHovered;
        /// <summary>
        /// The next hovered
        /// </summary>
        private bool nextHovered;

        /// <summary>
        /// Delegate DateChanged
        /// </summary>
        /// <param name="newDateTime">The new date time.</param>
        public delegate void DateChanged(DateTime newDateTime);
        /// <summary>
        /// Occurs when [on date changed].
        /// </summary>
        public event DateChanged onDateChanged;


        /// <summary>
        /// The hover brush
        /// </summary>
        private Brush HoverBrush;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDatePicker"/> class.
        /// </summary>
        public MaterialDatePicker()
        {
            InitializeComponent();
            Width = 250;
            Height = 425;
            TopDayRect = new RectangleF(0f, 0f, Width, 20f);
            TopDateRect = new RectangleF(0f, TopDayRect.Bottom, Width, (float)(Height * 0.3));
            MonthRect = new RectangleF(0f, TopDayRect.Bottom, Width, (float)(TopDateRect.Height * 0.3));
            DayRect = new RectangleF(0f, MonthRect.Bottom, Width, (float)(TopDateRect.Height * 0.4));
            YearRect = new RectangleF(0f, DayRect.Bottom, Width, (float)(TopDateRect.Height * 0.3));
            CurrentCal = new RectangleF(0f, TopDateRect.Bottom, Width, (float)(Height * 0.75));
            CurrentCal_Header = new RectangleF(0f, TopDateRect.Bottom + 3, Width, (float)(CurrentCal.Height * 0.1));
            PreviousCal = new RectangleF(0f, CurrentCal_Header.Y, CurrentCal_Header.Height, CurrentCal_Header.Height);
            NextCal = new RectangleF(Width - CurrentCal_Header.Height, CurrentCal_Header.Y, CurrentCal_Header.Height, CurrentCal_Header.Height);
            ShadowPath = new GraphicsPath();
            ShadowPath.AddLine(-5, TopDateRect.Bottom, Width, TopDateRect.Bottom);
            TopDayFont = SkinManager.ROBOTO_MEDIUM_10;
            MonthFont = new Font(SkinManager.ROBOTO_MEDIUM_10.Name, 16, FontStyle.Bold);
            DayFont = new Font(SkinManager.ROBOTO_MEDIUM_10.Name, 30, FontStyle.Bold);
            YeahrFont = new Font(SkinManager.ROBOTO_MEDIUM_10.Name, 15, FontStyle.Bold);
            DoubleBuffered = true;
            DateRectDefaultSize = (Width - 10) / 7;
            CurrentDate = DateTime.Now;

            HoverX = -1;
            HoverY = -1;
            CalculateRectangles();

        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            for (int i = 1; i < 7; i++)
            {
                DateRectangles.Add(new List<DateRect>());
                for (int j = 0; j < 7; j++)
                {
                    if (DateRectangles[i][j].Drawn)
                    {
                        if (DateRectangles[i][j].Rect.Contains(e.Location))
                        {
                            if (HoverX != i || HoverY != j)
                            {
                                HoverX = i;
                                recentHovered = false;
                                nextHovered = false;
                                HoverY = j;
                                Invalidate();
                            }
                            return;
                        }
                    }
                }
            }
            
            if (PreviousCal.Contains(e.Location))
            {
                recentHovered = true;
                HoverX = -1;
                Invalidate();
                return;
            }
            if (NextCal.Contains(e.Location))
            {
                nextHovered = true;
                HoverX = -1;
                Invalidate();
                return;
            }
            if (HoverX >= 0 || recentHovered || nextHovered)
            {
                HoverX = -1;
                recentHovered = false;
                nextHovered = false;
                Invalidate();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (HoverX >= 0) { 
            SelectedX = HoverX;
            SelectedY = HoverY;
            CurrentDate = DateRectangles[SelectedX][SelectedY].Date;
            Invalidate();
            if (onDateChanged != null)
            {
                onDateChanged(CurrentDate);
            }
            return;
            }
            if (recentHovered)
            {
                CurrentDate = FirstDayOfMonth(CurrentDate.AddMonths(-1));
                CalculateRectangles();
                Invalidate();
                if (onDateChanged != null)
                {
                    onDateChanged(CurrentDate);
                }
                return;
            }
            if (nextHovered)
            {
                CurrentDate = FirstDayOfMonth(CurrentDate.AddMonths(1));
                CalculateRectangles();
                Invalidate();
                if (onDateChanged != null)
                {
                    onDateChanged(CurrentDate);
                }
                return;
            }
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            HoverX = -1;
            HoverY = -1;
            nextHovered = false;
            recentHovered = false;
            Invalidate();
            base.OnMouseLeave(e);
        }


        /// <summary>
        /// Calculates the rectangles.
        /// </summary>
        private void CalculateRectangles()
        {
            DateRectangles = new List<List<DateRect>>();
            for (int i = 0; i < 7; i++)
            {
                DateRectangles.Add(new List<DateRect>());
                for (int j = 0; j < 7; j++)
                {
                    DateRectangles[i].Add(new DateRect(new Rectangle(5 + (j * DateRectDefaultSize), (int)(CurrentCal_Header.Bottom + (i * DateRectDefaultSize)), DateRectDefaultSize, DateRectDefaultSize)));
                }
            }
            DateTime FirstDay = FirstDayOfMonth(CurrentDate);
            for (DateTime date = FirstDay; date <= LastDayOfMonth(CurrentDate); date = date.AddDays(1))
            {
                int WeekOfMonth = GetWeekNumber(date, FirstDay);
                int DayOfWeek = (int)date.DayOfWeek - 1;
                if (DayOfWeek < 0) DayOfWeek = 6;
                if (date.DayOfYear == CurrentDate.DayOfYear && date.Year == CurrentDate.Year)
                {
                    SelectedX = WeekOfMonth;
                    SelectedY = DayOfWeek;
                }
                DateRectangles[WeekOfMonth][DayOfWeek].Drawn = true;
                DateRectangles[WeekOfMonth][DayOfWeek].Date = date;
            }

        }


        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            Width = 250;
            Height = 425;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            HoverBrush = new SolidBrush(Color.FromArgb(100, SkinManager.ColorScheme.PrimaryColor));

            g.Clear(SkinManager.GetCardsColor());

            DrawHelper.drawShadow(g, ShadowPath, 8, SkinManager.GetCardsColor());

            g.FillRectangle(SkinManager.ColorScheme.DarkPrimaryBrush, TopDayRect);
            g.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, TopDateRect);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;


            g.DrawString(CurrentDate.ToString("dddd"), TopDayFont, SkinManager.ACTION_BAR_TEXT_BRUSH(), TopDayRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            g.DrawString(CurrentDate.ToString("MMMM"), MonthFont, SkinManager.ACTION_BAR_TEXT_BRUSH(), MonthRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far });
            g.DrawString(CurrentDate.ToString("dd"), DayFont, SkinManager.ACTION_BAR_TEXT_BRUSH(), DayRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            g.DrawString(CurrentDate.ToString("yyyy"), YeahrFont, new SolidBrush(Color.FromArgb(80, SkinManager.ACTION_BAR_TEXT())), YearRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            g.DrawString(CurrentDate.ToString("MMMM"), SkinManager.ROBOTO_REGULAR_11, SkinManager.GetPrimaryTextBrush(), CurrentCal_Header, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            if (HoverX >= 0)
            {
                g.FillEllipse(HoverBrush, DateRectangles[HoverX][HoverY].Rect);
            }

            g.FillEllipse(SkinManager.ColorScheme.PrimaryBrush, DateRectangles[SelectedX][SelectedY].Rect);
            if (recentHovered) g.FillEllipse(HoverBrush, PreviousCal);
            
            if (nextHovered) g.FillEllipse(HoverBrush, NextCal);

            using (var ButtonPen = new Pen(SkinManager.GetPrimaryTextBrush(), 2))
            {

                g.DrawLine(ButtonPen,
                        (int)(PreviousCal.X + PreviousCal.Width * 0.6),
                        (int)(PreviousCal.Y + PreviousCal.Height * 0.4),
                        (int)(PreviousCal.X + PreviousCal.Width * 0.4),
                        (int)(PreviousCal.Y + PreviousCal.Height * 0.5));

                g.DrawLine(ButtonPen,
                        (int)(PreviousCal.X + PreviousCal.Width * 0.6),
                        (int)(PreviousCal.Y + PreviousCal.Height * 0.6),
                        (int)(PreviousCal.X + PreviousCal.Width * 0.4),
                        (int)(PreviousCal.Y + PreviousCal.Height * 0.5));

                g.DrawLine(ButtonPen,
                       (int)(NextCal.X + NextCal.Width * 0.4),
                       (int)(NextCal.Y + NextCal.Height * 0.4),
                       (int)(NextCal.X + NextCal.Width * 0.6),
                       (int)(NextCal.Y + NextCal.Height * 0.5));

                g.DrawLine(ButtonPen,
                        (int)(NextCal.X + NextCal.Width * 0.4),
                        (int)(NextCal.Y + NextCal.Height * 0.6),
                        (int)(NextCal.X + NextCal.Width * 0.6),
                        (int)(NextCal.Y + NextCal.Height * 0.5));
            }

            DateTime FirstDay = FirstDayOfMonth(CurrentDate);
            for (int i = 0; i < 7; i++)
            {
                string strName;
                int DayOfWeek = (int)DateTime.Now.DayOfWeek - 1;
                if (DayOfWeek < 0) DayOfWeek = 6;

                strName = DateTime.Now.AddDays(-DayOfWeek+i).ToString("ddd");
                g.DrawString(strName, SkinManager.ROBOTO_MEDIUM_11, SkinManager.GetSecondaryTextBrush(), DateRectangles[0][i].Rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
                for (DateTime date = FirstDay; date <= LastDayOfMonth(CurrentDate); date = date.AddDays(1))
                {
                    int WeekOfMonth = GetWeekNumber(date, FirstDay);
                    int DayOfWeek = (int)date.DayOfWeek - 1;
                    if (DayOfWeek < 0) DayOfWeek = 6;

                    g.DrawString(date.Day.ToString(), SkinManager.ROBOTO_MEDIUM_11, SkinManager.GetPrimaryTextBrush(), DateRectangles[WeekOfMonth][DayOfWeek].Rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                }

        }

        /// <summary>
        /// Firsts the day of month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DateTime.</returns>
        public DateTime FirstDayOfMonth(DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        /// <summary>
        /// Lasts the day of month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DateTime.</returns>
        public DateTime LastDayOfMonth(DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        /// <summary>
        /// Gets the week number.
        /// </summary>
        /// <param name="CurrentDate">The current date.</param>
        /// <param name="FirstDayOfMonth">The first day of month.</param>
        /// <returns>System.Int32.</returns>
        public static int GetWeekNumber(DateTime CurrentDate, DateTime FirstDayOfMonth)
        {

            while (CurrentDate.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                CurrentDate = CurrentDate.AddDays(1);

            return (int)Math.Truncate((double)CurrentDate.Subtract(FirstDayOfMonth).TotalDays / 7f) + 1;
        }

        /// <summary>
        /// Class DateRect.
        /// </summary>
        private class DateRect
        {
            /// <summary>
            /// The rect
            /// </summary>
            public Rectangle Rect;
            /// <summary>
            /// The drawn
            /// </summary>
            public bool Drawn = false;
            /// <summary>
            /// The date
            /// </summary>
            public DateTime Date;

            /// <summary>
            /// Initializes a new instance of the <see cref="DateRect"/> class.
            /// </summary>
            /// <param name="pRect">The p rect.</param>
            public DateRect(Rectangle pRect)
            {
                Rect = pRect;
            }
        }

    }
}
