using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Tracker.Models
{
    public class MenuUI
    {
        // --------------------------
        //      PILMENY
        // --------------------------

        // metod för att skriva ut titeln med formatering
        public static int ShowMenu(string title, string[] options)
        {
            int selectedIndex = 0;
            ConsoleKey key;

            Console.CursorVisible = false;

            // Koden inuti try-finally blocket säkerställer att konsolens markör alltid blir synlig igen när menyn är klar
            try
            {
                /// inne i do-while loopen hanteras användar input för att navigera genom menyalternativen
                do
                {
                    Console.Clear();
                    Console.Write("\u001b[3J");
                    Console.SetCursorPosition(0, 0);

                    WriteTitle(title);

                    Console.WriteLine("Använd ↑ / ↓ och tryck Enter för att välja.\n");

                    // Skriv ut menyalternativen med markering för det valda alternativet
                    for (int i = 0; i < options.Length; i++)
                    {
                        bool selected = i == selectedIndex;

                        // Formatera det valda alternativet
                        if (selected)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write(" > ");
                        }
                        // Formatera icke-valda alternativ
                        else
                        {
                            Console.Write("   ");
                        }

                        // Skriv ut alternativet som antingen valt eller icke valt
                        if (i == options.Length - 1)
                            Console.Write(options[i]);   
                        else
                            Console.WriteLine(options[i]);

                        Console.ResetColor();
                    }

                    // Läs användarens tangenttryckning utan att visa den i konsolen
                    key = Console.ReadKey(true).Key;

                    // Uppdatera det valda indexet baserat på användarens inmatning
                    if (key == ConsoleKey.UpArrow)
                    {
                        // Flytta markeringen uppåt i menyn 
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = options.Length - 1;
                    }
                    // Flytta markeringen nedåt i menyn
                    else if (key == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (selectedIndex >= options.Length)
                            selectedIndex = 0;
                    }
                    // Fortsätt loopen tills användaren trycker på Enter
                } while (key != ConsoleKey.Enter);
            }

            // Säkerställ att markören blir synlig igen när menyn är klar
            finally
            {
                Console.CursorVisible = true;
            }

            return selectedIndex;
        }


        // ----------------------------
        //     TITEL / RUBRIK
        // ----------------------------
        public static void WriteTitle(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine($"   {text}");
            Console.WriteLine("==================================================");
            Console.ResetColor();
        }


        // ----------------------------
        //         TABELL
        // ----------------------------


        // visar en tabbell med jobbansökningar
        public static void PrintTable(List<JobApplication> apps)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(
                "COMPANY".PadRight(15) +
                "POSITION".PadRight(20) +
                "STATUS".PadRight(12) +
                "DATE".PadRight(12) +
                "SALARY"
            );
            Console.ResetColor();

            Console.WriteLine(new string('-', 75));

            foreach (var app in apps)
            {
                SetStatusColor(app.Status);

                Console.Write(app.CompanyName.PadRight(15));
                Console.Write(app.PositionTitle.PadRight(20));
                Console.Write(app.Status.ToString().PadRight(12));

                Console.ResetColor();

                Console.Write(app.ApplicationDate.ToShortDateString().PadRight(12));
                Console.WriteLine($"{app.SalaryExpectation} kr");
            }
        }


        // --------------------------
        //    FÄRGLOGIK FÖR STATUS
        // --------------------------

        // sätter färg baserat på status på jobbansökan har 
        public static void SetStatusColor(ApplicationStatus status)
        {
            switch (status)
            {
                case ApplicationStatus.Offer:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ApplicationStatus.Rejected:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ApplicationStatus.Interview:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ApplicationStatus.Applied:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
            }
        }


        // --------------------------
        //        PAUS
        // --------------------------

        // pausmetod för att vänta på användar
        public static void Pause()
        {
            Console.WriteLine("\nTryck valfri tangent för att gå tillbaka till menyn...");
            Console.ReadKey();
        }
    }
}