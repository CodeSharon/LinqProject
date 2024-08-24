using LinqProject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Set the culture to ensure proper currency formatting
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

        var carService = new CarService();

        // Load data from both JSON and XML
        var carsFromJson = DataLoader.LoadFromJson("DataSource/cars.json");
        var carsFromXml = DataLoader.LoadFromXml("DataSource/cars.xml");

        // Combine both data sources
        var combinedCars = carsFromJson.Concat(carsFromXml).ToList();

        Console.WriteLine("Welcome to the Car Finder, an easy way to find your dream car!");

        int userAge = GetUserAge();

        if (userAge < 18)
        {
            Console.WriteLine("\nSorry, you must be at least 18 years old to use this application.");
            return;
        }

        var availableCars = carService.FilterCarsByAge(combinedCars, userAge);

        // If no cars are available, inform the user and exit
        if (availableCars.Count == 0)
        {
            Console.WriteLine("\nNo cars are available for your age.");
            return;
        }

        carService.ShowAvailableCarMakes(availableCars);

        // Enter a search term and filter based on text input
        Console.WriteLine("\nWhich model, year, price of car are you looking for?");
        string searchTerm = Console.ReadLine();
        var searchedCars = carService.SearchCars(availableCars, searchTerm);

        // Sort cars by a specified criterion
        Console.WriteLine("\nEnter a sort criterion (price, year) or type 'skip' to skip:");
        string sortBy = Console.ReadLine();

        var sortedCars = sortBy.ToLower() != "skip"
            ? carService.SortCars(searchedCars, sortBy)
            : searchedCars;

        carService.DisplayCars(sortedCars);

        Console.WriteLine("\nThank you for using the Car Finder. Press any key to exit.");
        Console.ReadKey();
    }

    private static int GetUserAge()
    {
        int userAge;
        Console.WriteLine("\nPlease enter your age to see the available cars:");
        while (!int.TryParse(Console.ReadLine(), out userAge) || userAge <= 0)
        {
            Console.WriteLine("Please enter a valid age.");
        }
        return userAge;
    }
}
