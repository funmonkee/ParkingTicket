using System;
using System.Globalization;
using Parking.Library;

namespace Parking.Console
{
    partial class Program
    {
        static void Main(string[] args)
        {
            try 
            {
                var userInput = ParseUserInput(args);
                var parkingTicket = TicketFactory.CreateTicket(userInput.TicketType, userInput.StartDateTime);

                if(parkingTicket == null)
                {
                    ShowError("Ticket type not accepted. Please try long or short ticket types.");
                }

                var result = parkingTicket.CalculateCharge(userInput.EndDateTime);
                System.Console.WriteLine($"Total charge: {result.Value.ToString("C")}.  For stay between {args[1]} and {args[2]}");
            }
            catch(FormatException)
            {
                ShowError("Date format not accepted.  Please check.");
            }
            catch(IndexOutOfRangeException)
            {
                ShowError("Missing parameters.  Please check.");
            }
            catch(Exception e)
            {
                ShowError($"Something went wrong. Please contact support - please provide logs (TODO) - {e.Message}.");
            }
        }

        /// <summary>
        /// return a structure with the input parameters. basic validation on parameters
        /// </summary>
        /// <param name="args">TODO</param>
        /// <returns>TODO</returns>
        static InputParameters ParseUserInput(string[] args)
        {
            if(args.Length != 3)
            {
                throw new ArgumentException($"Expected 3 parameters, got {args.Length}");
            }

            return new InputParameters
            {
                TicketType = args[0].ToLowerInvariant(),
                StartDateTime = DateTime.ParseExact(args[1], "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture),
                EndDateTime = DateTime.ParseExact(args[2], "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture)
            };
        }

        /// <summary>
        /// display message to console and details on usage
        /// </summary>
        /// <param name="message">optional message</param>
        static void ShowError(string message = null) {
            if(message!=null)
            {
                System.Console.WriteLine($"\nError: {message}");
            }

            var exeName = System.AppDomain.CurrentDomain.FriendlyName;
            System.Console.WriteLine($"\nUsage: {exeName} <ticketType> <start> <end>");
            System.Console.WriteLine("\t<ticketType> is long or short" 
                + "\n\t<start> is datetime in \"dd/mm/yyyy hh:mm\""
                + "\n\t<end> is datetime in \"dd/mm/yyyy hh:mm\"\n");
            Environment.Exit(0);
        }
    }

}
