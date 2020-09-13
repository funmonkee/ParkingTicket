using System;

namespace Parking.Console
{
    partial class Program
    {
        struct InputParameters
        {
            public string TicketType { get; set; }
            public DateTime StartDateTime { get; set;}
            public DateTime EndDateTime { get; set;}
        }
    }
}
