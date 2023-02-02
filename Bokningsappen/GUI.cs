using Bokningsappen.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bokningsappen
{
    internal class GUI
    {



        //internal static void PrintDaysMenu(string header, int positionX, int PositionY, int optionsPerLine,int spacingPerLine, int index, List<DateTime> day)
        //{
        //    int newLine = 0;
        //    for (int i = 0; i < day.Count; i++)
        //    {
        //        if (day[i].DayOfWeek == DayOfWeek.Monday) Console.SetCursorPosition(positionX + (0 % optionsPerLine) * spacingPerLine, PositionY + newLine);
        //        else if (day[i].DayOfWeek == DayOfWeek.Tuesday) Console.SetCursorPosition(positionX + (1 % optionsPerLine) * spacingPerLine, PositionY + newLine);
        //        else if (day[i].DayOfWeek == DayOfWeek.Wednesday) Console.SetCursorPosition(positionX + (2 % optionsPerLine) * spacingPerLine, PositionY + newLine);
        //        else if (day[i].DayOfWeek == DayOfWeek.Thursday) Console.SetCursorPosition(positionX + (3 % optionsPerLine) * spacingPerLine, PositionY + newLine);
        //        else if (day[i].DayOfWeek == DayOfWeek.Friday) { Console.SetCursorPosition(positionX + (4 % optionsPerLine) * spacingPerLine, PositionY + newLine); newLine ++; }
        //        if (i == index)
        //            Console.ForegroundColor = ConsoleColor.Blue;
        //        Console.Write(day[i].ToString("yy/MM/dd"));
        //        Console.ResetColor();


        //        //Console.WriteLine(day[i].ToString("yy/MM/dd") + " " + day[i].DayOfWeek);
        //    }
        //}

        internal static void PrintVerticalMenu(int index, string menuName, List<string> options)
        {
            string spaceOrArrow = "";
            Console.WriteLine("\n      Meny: " + menuName);
            for (int i = 0; i < options.Count; i++)
            {
                spaceOrArrow = "".PadRight(7);
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    spaceOrArrow = ("".PadRight(5) + "->");
                }
                Console.WriteLine(spaceOrArrow + "\t" + (i+1).ToString().PadRight(2) + ": " + options[i]);
                Console.ResetColor();
            }
        }

        internal static void PrintDayMenu(int index, string menuName, List<DateTime> days, List <DateTime> bookedRoomDates)
        {
            Console.WriteLine(menuName.PadRight(25) + "Enter = Select".PadRight(18) + "ESC = Cancel".PadRight(18) + "N = Next Month".PadRight(18) + "P = Previous Month");
            for (int i = 0; i < days.Count; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("".PadRight(5) + "->");
                }
                else Console.Write("".PadRight(7));
                if (bookedRoomDates.Contains(days[i])) Console.ForegroundColor = ConsoleColor.Red;
                else Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\t" + days[i].DayOfWeek.ToString().PadRight(10) + ": " + days[i].ToString("dd/MM/yy"));
                if (days[i].DayOfWeek == DayOfWeek.Friday) Console.WriteLine();
                Console.ResetColor();
            }
        }
    }
}
