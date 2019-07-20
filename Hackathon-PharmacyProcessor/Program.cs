using System;
using System.Linq;
using System.Threading;
using Hackathon_DataAccess;

namespace Hackathon_PharmacyProcessor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Connecting to pharmacy data...");
            using (var context = new HackathonEntities())
            {
                if (context.users.Any())
                {
                    Console.WriteLine("Connected to pharmacy data.");
                    Console.WriteLine("Listening for pharmacy events");
                }
            }

            while (true)
            {
                Console.WriteLine("Checking for changes...");

                Thread.Sleep(2500);
            }
        }
    }
}