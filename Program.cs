using Job_Tracker.Models;
using System.Text;

namespace Job_Tracker
{
    public class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;

            var controller = new ApplicationController();
            controller.Run();


        }
    }
}
