using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Tracker.Models
{
    public class JobAppConsole
    {

        // koden neddan är en lista som lagrar alla jobbsökningsposter om det inte finns någon lista skapas en ny tom lista
        // new() är en förenklad syntax för att skapa en ny instans av en lista utan att behöva specificera typen igen
        public List<JobApplication> Applications { get; set; } = new();


        // Metod för att lägga till en ny jobbsökningspost i listan
        public void AddJob(JobApplication application) => Applications.Add(application);


        // logic för att uppdatera statusen för en jobbsökningspost baserat på företagsnamnet
         public bool Updatestatus(string companyname, ApplicationStatus newStatus)
         {

            // hitta jobbsökningsposten baserat på företagsnamnet (case-insensitive jämförelse)
            var app = Applications.FirstOrDefault(a =>
            a.CompanyName.Equals(companyname, StringComparison.OrdinalIgnoreCase));

            if (app != null)
                return false;

            app.Status = newStatus;
            app.ResponseDate = DateTime.Now;
            return true;    


         }


        // Metod för att visa alla jobbsökningsposter i konsolen
        public void ShowAll()
        {
            // Kontrollera om det finns några ansökningar i listan
            if (Applications.Count == 0)
            {
                Console.WriteLine("Inga ansökningar finns");
                return;
            }

            // Loop genom varje ansökan och skriv ut dess sammanfattning
            foreach (var app in Applications)
             Console.WriteLine(app.GetSummary());

        }

        // Metod för att hämta alla jobbsökningsposter med en specifik status
        public List<JobApplication> GetByStatus(ApplicationStatus status) =>
            Applications.Where(a => a.Status == status).ToList();

        // Metod för att hämta alla jobbsökningsposter sorterade efter löneförväntan i fallande ordning
        public List<JobApplication> getSortedBySalary() =>
            Applications.OrderByDescending(a => a.SalaryExpectation).ToList();


        // Metod för att visa statistik om löneförväntningar
        public void ShowSalaryStatistics()
        {
            // Kontrollera om det finns några ansökningar i listan
            if (Applications.Count == 0)
            {
                Console.WriteLine("Inga ansökningar finns för att visa statistik.");
                return;
            }

            // visa statistik om löneförväntningar och antal ansökningar
            Console.WriteLine($"Totalt antal ansökingar: {Applications.Count}");
            Console.WriteLine($"Genomsnittlig löneförväntan: {Applications.Average(a => a.SalaryExpectation):F0} kr");

        }

    }
}
