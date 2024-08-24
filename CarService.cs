using LinqProject;

public class CarService
{
    public List<Car> FilterCarsByAge(List<Car> cars, int userAge)
    {
        return cars.Where(car => userAge >= car.AgeRequirement).ToList();
    }

    public List<Car> SearchCars(List<Car> cars, string searchTerm)
    {
        searchTerm = searchTerm.ToLower();
        List<Car> filteredCars = cars.Where(car =>
            car.Make.ToLower().Contains(searchTerm) ||
            car.Model.ToLower().Contains(searchTerm) ||
            car.Year.ToString().Contains(searchTerm) ||
            (int.TryParse(searchTerm, out int price) && car.Price == price) ||
            (searchTerm.Contains('-') && TryParsePriceRange(searchTerm, out int minPrice, out int maxPrice) && car.Price >= minPrice && car.Price <= maxPrice)
        ).ToList();

        return filteredCars;
    }

    private bool TryParsePriceRange(string searchTerm, out int minPrice, out int maxPrice)
    {
        minPrice = 0;
        maxPrice = int.MaxValue;
        var parts = searchTerm.Split('-');
        if (parts.Length == 2 && int.TryParse(parts[0], out minPrice) && int.TryParse(parts[1], out maxPrice))
        {
            return true;
        }
        return false;
    }

    public List<Car> SortCars(List<Car> cars, string sortBy)
    {
        return sortBy.ToLower() switch
        {
            "price" => cars.OrderByDescending(car => car.Price).ToList(),
            "year" => cars.OrderByDescending(car => car.Year).ToList(),
            "make" => cars.OrderBy(car => car.Make).ToList(),
            _ => cars
        };
    }

    public void ShowAvailableCarMakes(List<Car> cars)
    {
        Console.WriteLine("\nAvailable Cars for Your Age:\n");
        var makes = cars.Select(car => car.Make).Distinct();
        foreach (var make in makes)
        {
            Console.WriteLine(make);
        }
    }

    public void DisplayCars(List<Car> cars)
    {
        Console.WriteLine("\nFiltered and Sorted Cars for You:\n");
        foreach (var car in cars)
        {
            Console.WriteLine($"{car.Make} {car.Model} - Year: {car.Year}, Price: {car.Price:C}");
        }
    }
}
