using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Tracker.Models
{

    // koden för ApplicationStatus enum visar olika statusar för en jobbsökningsprocess
    public enum ApplicationStatus
    {
        Applied,
        Interview,
        Offer,
        Rejected,
        
    }

    // Klassen JobApplication representerar en jobbsökningspost med olika egenskaper
    public class JobApplication
    {
        // Olika egenskaper för en jobbsökningspost 
        public string CompanyName { get; set; }
        public string PositionTitle { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime ApplicationDate { get; set; }

        // Varför använder vi en nullable DateTime för ResponseDate?
        // svar: För att hantera situationer där det kanske inte finns något svar än från arbetsgivaren
        public DateTime? ResponseDate { get; set; } 

        public int SalaryExpectation { get; set; }


        // Konstruktor för att skapa en ny jobbsökningspost 
        // Varför gör vi en konstruktor här?
        // Svar: För att säkerställa att alla nödvändiga egenskaper är satta när en ny instans av JobApplication skapas
        public JobApplication(
           string companyName,
           string positionTitle,
           ApplicationStatus status,
           DateTime applicationDate,
           int salaryExpectation)
       {
            CompanyName = companyName;
            PositionTitle = positionTitle;
            Status = status;
            ApplicationDate = applicationDate;
            SalaryExpectation = salaryExpectation;
            // varför lägger vi ResponseDate till som null här?
            // Svar: Eftersom det är möjligt att vi inte har fått något svar än när vi skapar en ny jobbsökningspost
            ResponseDate = null;
        }

        // Metod för att beräkna antalet dagar sedan ansökan skickades
        // Varför behöver vi denna metod?
        // Svar: För att kunna spåra hur länge det har gått sedan ansökan skickades, vilket kan vara viktigt för uppföljning
        public int GetDaysSinceApplication()
        {
            return (DateTime.Now - ApplicationDate).Days;
        }

        // Metod för att få en sammanfattning av jobbsökningsposten
        public string GetSummary()
        {
            // Hantera nullable ResponseDate för att visa rätt information
            string responseInfo = ResponseDate.HasValue
            ? $"Svar: {ResponseDate.Value.ToShortDateString()}"
            : "Svar: Inget svar än";

            // Returnera en formaterad sträng med all relevant information
            return $"{CompanyName} - {PositionTitle} | Status: {Status} | " +
                   $"Ansökt för {GetDaysSinceApplication()} dagar sedan | {responseInfo} | " +
                   $"Lönekrav: {SalaryExpectation} kr";


        }

    }
}
