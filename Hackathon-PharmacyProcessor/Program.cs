using System;
using System.Collections.Generic;
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
                if (context.PharmacyEvents.Any())
                {
                    Console.WriteLine("Connected to pharmacy data.");
                    Console.WriteLine("Listening for pharmacy events");
                }
            }

            while (true)
            {
                Console.WriteLine("Checking for changes...");

                using (var context = new HackathonEntities())
                {
                    var filledPharmacyEvents = (from pe in context.PharmacyEvents
                        join s in context.Scripts on pe.ScriptId equals s.ScriptId
                        where s.DateFilled == null && pe.DateFilled != null
                        select pe.ScriptId).ToList();
                    var pickedUpPharmacyEvents = (from pe in context.PharmacyEvents
                        join s in context.Scripts on pe.ScriptId equals s.ScriptId
                        where s.DateFilled != null && s.DatePickedUp == null && pe.DateFilled != null &&
                              pe.DatePickedUp != null
                        select pe.ScriptId).ToList();

                    if (filledPharmacyEvents.Count == 0 && pickedUpPharmacyEvents.Count == 0)
                    {
                        Console.WriteLine("Found 0 events to process.  Continuing...");
                        Thread.Sleep(5000);
                        continue;
                    }

                    Console.WriteLine($"Found {filledPharmacyEvents.Count} filled prescriptions to process.");

                    foreach (var s in context.Scripts.Where(s =>
                        filledPharmacyEvents.Contains(s.ScriptId)).ToList())
                    {
                        s.DateFilled = DateTime.Now;
                    }

                    if (filledPharmacyEvents.Count > 0)
                    {
                        Console.WriteLine($"Marked {filledPharmacyEvents.Count} prescriptions as filled.");
                    }

                    Console.WriteLine($"Found {pickedUpPharmacyEvents.Count} picked up prescriptions to process.");

                    foreach (var s in context.Scripts.Where(s =>
                        pickedUpPharmacyEvents.Contains(s.ScriptId)).ToList())
                    {
                        if (s.DateFilled == null)
                        {
                            s.DateFilled = DateTime.Now;
                        }

                        s.DatePickedUp = DateTime.Now;
                    }

                    if (pickedUpPharmacyEvents.Count > 0)
                    {
                        Console.WriteLine($"Marked {pickedUpPharmacyEvents.Count} prescriptions as picked up.");
                    }

                    context.SaveChanges();
                }

                Thread.Sleep(5000);
            }
        }
    }
}