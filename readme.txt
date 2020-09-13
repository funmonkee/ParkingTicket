Parking Ticket Test

    * See spec as per email.
    * Do NOT show DI
    * DO show clean code (simple + extendable), some SOLID, unit tests (TDD)
    * Do NOT build complex user interface
    * limit time to ±3 hours

Approach

    * VSCode using .Net core 3.1 on a macbook
    * TDD for library functionality + used example data  
    * Algorithm: simple and includes reference for short stay Algorithm (re. calc working days)
    * Program to interfaces and prefer composition over inheritance

Running the test

    Ensure .Net Core 3.1 is available on your computer
    Pull the code from GITHUB https://github.com/funmonkee/ParkingTicket

    From the solution folder (where Parking.sln is located)

    a. build the code  
        dotnet build

    b. run the unit tests
        dotnet test
        
    c. run the command line interface
        dotnet run -p Parking.Console "long" "01/09/2020 09:00" "14/09/2020 13:30"


TODO:

    * DI/service provider
    * FluentValidation
    * Logging
    * Unit tests for console project
