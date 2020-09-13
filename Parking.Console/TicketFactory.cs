using System;
using System.Collections.Generic;
using Parking.Library;

namespace Parking.Console
{
    public static class TicketFactory
    {
        public static Ticket CreateTicket(string ticketType, DateTime startDateTime)
        {
            var validTickets = new Dictionary<string, Ticket>
            {
                { "long", 
                    new LongStayTicket(startDateTime, Constants.LongStayPrice) },
                    
                { "short", 
                    new ShortStayTicket(
                        startDateTime,
                        Constants.ShortStayPrice,
                        new HourlyCalculator(
                            Constants.BusinessHoursStart,
                            Constants.BusinessHoursEnd)) }
            };

            var ticket = validTickets.GetValueOrDefault(ticketType);
            return ticket;
        }
    }

}
