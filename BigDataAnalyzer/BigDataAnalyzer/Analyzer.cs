using System.Text;
using BigDataAnalyzer.Interfaces;

namespace BigDataAnalyzer;

public class Analyzer : IAnalyzer
{
    private static readonly object AmountOfIncomeLocker = new();
    private static readonly object BrandLocker = new();
    private static readonly object CategoryIDsLocker = new();
    private static readonly object ProductIDsLocker = new();

    private (string? Name, int Count)[] _tempBrands = Array.Empty<(string? Name, int Count)>();
    private (long Id, int Count)[] _tempCategoryIDs = Array.Empty<(long Id, int Count)>();
    private (long Id, int Count)[] _tempProductIDs = Array.Empty<(long Id, int Count)>();

    public Analyzer()
    {
        MostPopularBrands = Array.Empty<(string? Name, int Count)>();
        MostPopularCategoryIDs = Array.Empty<(long Id, int Count)>();
        MostPopularProductIDs = Array.Empty<(long Id, int Count)>();
    }

    public decimal? AmountOfIncome { get; private set; }
    public (string? Name, int Count)[] MostPopularBrands { get; private set; }
    public (long Id, int Count)[] MostPopularCategoryIDs { get; private set; }
    public (long Id, int Count)[] MostPopularProductIDs { get; private set; }


    public void StartPackageAnalyze(int packageSize, string[] filePaths)
    {
        var tuples = new ValueTuple<ParameterizedThreadStart, string>[]
        {
            (SetAmountOfIncome, "Установка суммы выручки"),
            (SetMostPopularBrands, "Популярность бренда"),
            (SetMostPopularCategoryIDs, "Популярность категории"),
            (SetMostPopularProductIDs, "Популярность продукта")
        };
        
        var builder = new StringBuilder();
        builder.AppendLine($"Идёт обработка данных. Размер пакета - {packageSize}.");
        builder.AppendLine("Дождитесь результатов обработки всех пакетов...");
        Console.WriteLine(builder);

        foreach (var filePath in filePaths)
        {
            using var streamReader = new StreamReader(filePath);

            streamReader.ReadLine(); // выкидываю первую строку с заголовками
            
            string? line;
            
            var events = new List<Event>();

            var packageNumber = 1;

            while ((line = streamReader.ReadLine()) != null)
            {
                var strings = line.Split(",");

                events.Add(new Event(strings));

                var eventsCount = events.Count;

                if (eventsCount < packageSize)
                {
                    SimpleProgressBar.Show($"Анализ пакета № {packageNumber}", eventsCount, packageSize,
                        packageNumber+2);
                    continue;
                }

                packageNumber++;

                var eventsCopy = events.ToArray();

                events.Clear();

                foreach (var (parameterizedThreadStart, name) in tuples)
                {
                    var thread = new Thread(parameterizedThreadStart)
                    {
                        Name = name
                    };
                    thread.Start(eventsCopy);
                }
            }
        }
    }

    private void SetAmountOfIncome(object? argument)
    {
        if (argument is not Event[] events) return;

        lock (AmountOfIncomeLocker)
        {
            if (AmountOfIncome is null or 0)
                AmountOfIncome = Sum(events);
            else
                AmountOfIncome += Sum(events);
        }

        decimal? Sum(IEnumerable<Event> items)
        {
            return items.Select(x => x.Price).Sum();
        }
    }

    private void SetMostPopularBrands(object? argument)
    {
        if (argument is not Event[] events) return;

        var brands = events
            .GroupBy(x => x.Brand)
            .Select(g =>
                (Name: g.Key, Count: g.Count()))
            .ToArray();

        lock (BrandLocker)
        {
            MostPopularBrands = GetMostPopularParameters(brands, ref _tempBrands);
        }
    }

    private void SetMostPopularCategoryIDs(object? argument)
    {
        if (argument is not Event[] events) return;

        var ids = events
            .GroupBy(x => x.CategoryId)
            .Select(g =>
                (ID: g.Key, Count: g.Count()))
            .ToArray();

        lock (CategoryIDsLocker)
        {
            MostPopularCategoryIDs = GetMostPopularParameters(ids, ref _tempCategoryIDs);
        }
    }

    private void SetMostPopularProductIDs(object? argument)
    {
        if (argument is not Event[] events) return;

        var ids = events
            .GroupBy(x => x.ProductId)
            .Select(g =>
                (ID: g.Key, Count: g.Count()))
            .ToArray();

        lock (ProductIDsLocker)
        {
            MostPopularProductIDs = GetMostPopularParameters(ids, ref _tempProductIDs);
        }
    }

    private (long Number, int Count)[] GetMostPopularParameters(
        (long Number, int Count)[] parameters,
        ref (long Number, int Count)[] temp,
        int objCount = 5)
    {
        temp = temp.Length == 0
            ? parameters
            : temp.Concat(parameters).ToArray();

        return temp
            .GroupBy(x => x.Number)
            .Select(g =>
                (Number: g.Key, Count: g.Select(x => x.Count).Sum()))
            .OrderByDescending(x => x.Count)
            .Take(objCount)
            .ToArray();
    }

    private (string? Name, int Count)[] GetMostPopularParameters(
        (string? Name, int Count)[] parameters,
        ref (string? Name, int Count)[] temp,
        int objCount = 5)
    {
        temp = temp.Length == 0
            ? parameters
            : temp.Concat(parameters).ToArray();

        return temp
            .GroupBy(x => x.Name)
            .Select(g =>
                (Name: g.Key, Count: g.Select(x => x.Count).Sum()))
            .OrderByDescending(x => x.Count)
            .Take(objCount)
            .ToArray();
    }
}