using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Job_Tracker.Models
{
    public class ApplicationController
    {
        // Instans av JobManager för att hantera jobbsökningslogiken
        private readonly JobManager _manager;
        private readonly string[] _menuOptions =
        {
            // Menyval för användargränssnittet
            "➕ Lägg till ny ansökan",
            "📋 Visa alla ansökningar",
            "🔍 Filtrera ansökningar efter status",
            "📅 Sortera ansökningar efter datum",
            "📊 Visa statistik",
            "✏️ Uppdatera status på en ansökan",
            "🗑️ Ta bort en ansökan",
            "💾 Avsluta programmet"
        };


        // Konstruktor för ApplicationController
        public ApplicationController()
        {
            _manager = new JobManager();
            TestData();
        }


        public void Run()
        {
            bool running = true;

            while (running)
            {

                int chocie = MenuUI.ShowMenu("🧾 JOB APPLICATION MANAGER", _menuOptions);

                switch (chocie)
                {
                    case 0:
                        AddApplication();
                        break;
                    case 1:
                        ShowAll();
                        break;
                    case 2:
                        FilterByStatus();
                        break;
                    case 3:
                        SortByDate();
                        break;
                    case 4:
                        ShowStatistics();
                        break;
                    case 5:
                        Updatestatus();
                        break;
                    case 6:
                        RemoveApplication();
                        break;
                    case 7:
                        running = false;
                        break;
                }

            }
        }

        // ---------------------------------------------
        // LOGIC/METODER FÖR MENYALTERNATIVEN KOMMER HÄR
        // ---------------------------------------------

        private void AddApplication()
        {
           
            Console.Clear();
            MenuUI.WriteTitle("➕ LÄGG TILL NY ANSÖKAN");

            // Hjälpmetod för att läsa in en obligatorisk sträng från användaren
            string companyName = ReadRequiredString("Företagsnamn: ");

            // Hjälpmetod för att läsa in en obligatorisk sträng från användaren
            string positionTitle = ReadRequiredString("Tjänst / Position: ");

            // Hjälpmetod för att läsa in en giltig status från användaren
            Console.WriteLine("\nVälj status:");
            var status = ReadStatusFromUser();


            // koden för att läsa in ansökningsdatum
            Console.Write("Ansökningsdatum (t.ex. Year-Month-Day): ");
            DateTime applicationDate;

            // om inmatningen inte är ett giltigt datum, upprepa tills giltigt datum anges
            while (!DateTime.TryParse(Console.ReadLine(), out applicationDate))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Ogiltigt datum. Försök igen: ");
                Console.ResetColor();
            }

            // koden för att läsa in löneförväntan
            Console.Write("Löneförväntan (kr): ");
            int salary;
            while (!int.TryParse(Console.ReadLine(), out salary))
            {
                // samma här, upprepa tills giltigt heltal anges
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Ogiltigt tal. Ange heltal (kr): ");
                Console.ResetColor();
            }

            // Skapa en ny JobApplication-instans med de insamlade uppgifterna
            var app = new JobApplication(
                companyName,
                positionTitle,
                status,
                applicationDate,
                salary
            );

            // Lägg till den nya ansökan i JobManager
            _manager.AddJob(app);

            // Bekräftelsemeddelande till användaren som visar att ansökan har lagts till i grön text
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAnsökan tillagd!");
            Console.ResetColor();
            MenuUI.Pause();
        }

        // Visar alla ansökningar i en tabell
        private void ShowAll()
        {
            // Rensa konsolen och skriv ut titeln
            Console.Clear();
            MenuUI.WriteTitle("📋 ALLA ANSÖKNINGAR");

            // Kontrollera om det finns några ansökningar att visa
            if (_manager.Applications.Count == 0)
                Console.WriteLine("Inga ansökningar finns.");
            else
                MenuUI.PrintTable(_manager.Applications);

            // Vänta på användarens input innan du fortsätter
            MenuUI.Pause();
        }

        // Filtrerar ansökningar baserat på status
        private void FilterByStatus()
        {
            Console.Clear();
            MenuUI.WriteTitle("🔍 FILTRERA ANSÖKNINGAR EFTER STATUS");

            // Be användaren att välja en status
            Console.WriteLine("Välj status:");
            var status = ReadStatusFromUser();

            // Hämta ansökningar med den valda statusen
            var filtered = _manager.GetByStatus(status);

            // Visa filtrerade ansökningar eller ett meddelande om inga hittades
            if (filtered.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nInga ansökningar hittades med status: {status}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"\nAnsökningar med status {status}:\n");
                MenuUI.PrintTable(filtered);
            }

            MenuUI.Pause();
        }

        // Sorterar ansökningar efter ansökningsdatum lite som en tidigare metod
        private void SortByDate()
        {
            Console.Clear();
            MenuUI.WriteTitle("📅 SORTERA ANSÖKNINGAR EFTER DATUM");

            var sorted = _manager.Applications
                .OrderBy(a => a.ApplicationDate)
                .ToList();

            if (sorted.Count == 0)
                Console.WriteLine("Inga ansökningar finns.");
            else
                MenuUI.PrintTable(sorted);

            MenuUI.Pause();
        }


        // Visar statistik om ansökningarna
        private void ShowStatistics()
        {
            Console.Clear();
            MenuUI.WriteTitle("📊 STATISTIK");

            // Hämta alla ansökningar från JobManager
            var apps = _manager.Applications;

            // Visar totalt antal ansökningar
            int total = apps.Count;
            Console.WriteLine($"Totalt antal ansökningar: {total}");

            // Visar antal ansökningar per status
            Console.WriteLine("\nAntal per status:");
            foreach (ApplicationStatus status in Enum.GetValues(typeof(ApplicationStatus)))
            {
                // Räkna antal ansökningar med aktuell status
                int countByStatus = apps.Count(a => a.Status == status);
                Console.WriteLine($"- {status}: {countByStatus}");
            }

            // Beräkna och visa genomsnittlig svarstid för ansökningar som har fått svar
            var respondedApps = apps
                .Where(a => a.ResponseDate.HasValue)
                .ToList();

            // Beräkna genomsnittlig tid i dagar mellan ansökningsdatum och svarsdatum
            if (respondedApps.Count > 0)
            {
                // Beräkna genomsnittlig svarstid i dagar
                var avgDays = respondedApps
                    .Average(a => (a.ResponseDate.Value - a.ApplicationDate).TotalDays);

                Console.WriteLine($"\nGenomsnittlig svarstid: {avgDays:F1} dagar");
            }
            // Om inga ansökningar har fått svar ännu
            else
            {
                Console.WriteLine("\nGenomsnittlig svarstid: Ingen ansökan har fått svar ännu.");
            }

            MenuUI.Pause();
        }


        // Uppdaterar statusen för en ansökan baserat på företagsnamn
        private void Updatestatus()
        {
 
            Console.Clear();
            MenuUI.WriteTitle("✏️ UPPDATERA STATUS PÅ EN ANSÖKAN");

            Console.Write("Ange företagsnamn för ansökan du vill uppdatera: ");
            string companyName = Console.ReadLine();

            Console.WriteLine("\nVälj ny status:");
            var newStatus = ReadStatusFromUser();

            bool updated = _manager.Updatestatus(companyName, newStatus);   

            // Visa resultatmeddelande baserat på om uppdateringen lyckades eller inte
            if (updated)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nStatus uppdaterad!");
            }

            // Om ingen ansökan hittades med det angivna företagsnamnet
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nIngen ansökan hittades med det företagsnamnet.");
            }
            Console.ResetColor();

            MenuUI.Pause();
        }


        // Tar bort en ansökan baserat på företagsnamn
        private void RemoveApplication()
        {
            Console.Clear();
            MenuUI.WriteTitle("🗑️ TA BORT EN ANSÖKAN");

            // Kontrollera om det finns några ansökningar att ta bort
            if (_manager.Applications.Count == 0)
            {
                Console.WriteLine("Det finns inga ansökningar att ta bort.");
                MenuUI.Pause();
                return;
            }

            // Be användaren att ange företagsnamnet för den ansökan som ska tas bort
            Console.Write("Ange företagsnamn för ansökan du vill ta bort: ");
            string companyName = Console.ReadLine();

            // Hitta ansökan baserat på företagsnamnet (case-insensitive jämförelse)
            var appToRemove = _manager.Applications
                .FirstOrDefault(a => a.CompanyName.Equals(companyName, StringComparison.OrdinalIgnoreCase));

            // Om ingen ansökan hittades med det angivna företagsnamnet
            if (appToRemove == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nIngen ansökan hittades med det företagsnamnet.");
            }

            // Om ansökan hittades, ta bort den från listan
            else
            {
                _manager.Applications.Remove(appToRemove);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nAnsökan borttagen.");
            }
            Console.ResetColor();

            MenuUI.Pause();
        }




        // --------------------------
        // HJÄLPMETODER KOMMER HÄR
        // --------------------------


        // Hjälpmetod för att läsa in en giltig ApplicationStatus från användaren
        private ApplicationStatus ReadStatusFromUser()
        {
            // Hämta alla värden i ApplicationStatus enum och konvertera till lista
            var values = Enum.GetValues(typeof(ApplicationStatus)).Cast<ApplicationStatus>().ToList();

            // Visa alla statusalternativ för användaren
            for (int i = 0; i < values.Count; i++)
                Console.WriteLine($"{i + 1}. {values[i]}");

            Console.Write("Val: ");
            int choice;
            // Läs in användarens val och validera inmatningen
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > values.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Ogiltigt val. Försök igen: ");
                Console.ResetColor();
            }

            return values[choice - 1];
        }


        // Metod för att lägga till testdata i JobManager för demonstration och testning
        private void TestData()
        {
            _manager.AddJob(new JobApplication(
                "Spotify",
                "Backend Developer",
                ApplicationStatus.Applied,
                DateTime.Now.AddDays(-10),
                55000));

            var klarna = new JobApplication(
                "Klarna",
                "QA Engineer",
                ApplicationStatus.Interview,
                DateTime.Now.AddDays(-20),
                48000)
            {
                ResponseDate = DateTime.Now.AddDays(-5)
            };
            _manager.AddJob(klarna);

            var ikea = new JobApplication(
                "IKEA",
                "Project Manager",
                ApplicationStatus.Offer,
                DateTime.Now.AddDays(-30),
                60000)
            {
                ResponseDate = DateTime.Now.AddDays(-2)
            };
            _manager.AddJob(ikea);
        }


        // Hjälpmetod för att läsa in en obligatorisk sträng från användaren
        private string ReadRequiredString(string prompt)
        {
            // Variabel för att lagra användarens inmatning
            string input;

            // Upprepa tills användaren anger en icke-tom sträng
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim();

                // Om inmatningen är tom, visa ett felmeddelande
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fältet får inte vara tomt. Försök igen.\n");
                    Console.ResetColor();
                }

            } while (string.IsNullOrEmpty(input));

            return input;
        }


    }

    
}
