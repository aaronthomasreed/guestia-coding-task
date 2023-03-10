using GuestiaCodingTask.Data;
using GuestiaCodingTask.Helpers;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GuestiaCodingTask
{
    class Program
    {
        static void Main(string[] args)
        {
            DbInitialiser.CreateDb();

            GuestiaContext db = new GuestiaContext();

            List<Guest> guests = db.Guests.Include(n => n.GuestGroup).Where(n => n.RegistrationDate == null).ToList();

            foreach (var group in guests.GroupBy(g => g.GuestGroup))
            {
                Console.WriteLine("Unregistered " + group.Key.Name + " Guests");
                Console.WriteLine("========================================");

                IEnumerable<string> formattedGuestNames = group.Select(s => GetFormattedName(s));

                foreach (string name in formattedGuestNames.OrderBy(o => o))
                    Console.WriteLine(name);

                Console.WriteLine("========================================\n");
            }

            Console.ReadLine();
        }

        static string GetFormattedName(Guest guest)
        {
            switch (guest.GuestGroup.NameDisplayFormat)
            {
                case NameDisplayFormatType.UpperCaseLastNameSpaceFirstName:
                    return guest.LastName.ToUpper() + " " + guest.FirstName;
                case NameDisplayFormatType.LastNameCommaFirstNameInitial:
                default:
                    return guest.LastName + ", " + guest.FirstName[0];
            }
        }
    }
}
