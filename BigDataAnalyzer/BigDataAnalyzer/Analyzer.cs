using System.Data.Common;
using System.Text;
using BigDataAnalyzer.Interfaces;

namespace BigDataAnalyzer;

public class Analyzer : IAnalyzer
{
    private readonly object _amountOfIncomeLocker;
    private readonly object _brandLocker;
    private readonly object _categoryIDsLocker;
    private readonly object _productIDsLocker;
    
    private (string? Name, int Count)[] _tempBrands;
    private (long Id, int Count)[] _tempCategoryIDs;
    private (long Id, int Count)[] _tempProductIDs;

    public Analyzer()
    {
        _amountOfIncomeLocker = new object();
        _brandLocker = new object();
        _categoryIDsLocker = new object();
        _productIDsLocker = new object();

        _tempBrands = Array.Empty<(string? Name, int Count)>();
        _tempCategoryIDs = Array.Empty<(long Id, int Count)>();
        _tempProductIDs = Array.Empty<(long Id, int Count)>();

        MostPopularBrands = Array.Empty<(string? Name, int Count)>();
        MostPopularCategoryIDs = Array.Empty<(long Id, int Count)>();
        MostPopularProductIDs = Array.Empty<(long Id, int Count)>();
    }

    public decimal? AmountOfIncome { get; private set; }
    public (string? Name, int Count)[] MostPopularBrands { get; private set; }
    public (long Id, int Count)[] MostPopularCategoryIDs { get; private set; }
    
    public (long Id, int Count)[] MostPopularProductIDs { get; private set; }

    public void StartPackageAnalyze(string path, int packageSize)
    {
        using var streamReader = new StreamReader(path, Encoding.Default);

        string? line;

        var t1 = 0;
        var t2 = 0;
        var t3 = 0;
        var t4 = 0;

        streamReader.ReadLine(); // выкидываю первую строку с заголовками

        var events = new List<Event>();

        while ((line = streamReader.ReadLine()) != null)
        {
            var strings = line.Split(",");

            events.Add(new Event(strings));

            if (events.Count <= packageSize) continue; //TODO  надо асинхронно отдавать не верное условие

            var eventsCopy = events.ToArray();
            events.Clear();

            var amountOfIncome = new Thread(SetAmountOfIncome)
            {
                Name = $"Поток - Пакет № {++t1} (установка суммы выручки)"
            };
            amountOfIncome.Start(eventsCopy);

            var mostPopularBrands = new Thread(SetMostPopularBrands)
            {
                Name = $"Поток - Пакет № {++t2} (популярность бренда)"
            };
            mostPopularBrands.Start(eventsCopy);
            
            var mostPopularCategoryIDs = new Thread(SetMostPopularCategoryIDs)
            {
                Name = $"Поток - Пакет № {++t3} (популярность категории)"
            };
            mostPopularCategoryIDs.Start(eventsCopy);
            
            var mostPopularProductIds = new Thread(SetMostPopularProductIDs)
            {
                Name = $"Поток - Пакет № {++t4} (популярность продукта)"
            };
            mostPopularProductIds.Start(eventsCopy);
        }
    }

    private void SetAmountOfIncome(object? argument)
    {
        lock (_amountOfIncomeLocker)
        {
            if (argument is not Event[] events) return;

            if (AmountOfIncome is null or 0)
                AmountOfIncome = events.Select(x => x.Price).Sum();
            else
                AmountOfIncome += events.Select(x => x.Price).Sum();
        }
    }

    private void SetMostPopularBrands(object? argument)
    {
        lock (_brandLocker)
        {
            if (argument is not Event[] events) return;

            var brands = events
                .GroupBy(x => x.Brand)
                .Select(g =>
                    (Name: g.Key, Count: g.Count()))
                .ToArray();

            MostPopularBrands = GetMostPopularParameters(brands, ref _tempBrands);
        }
    }
    
    private void SetMostPopularCategoryIDs(object? argument)
    {
        lock (_categoryIDsLocker)
        {
            if (argument is not Event[] events) return;

            var ids = events
                .GroupBy(x => x.CategoryId)
                .Select(g =>
                    (ID: g.Key, Count: g.Count()))
                .ToArray();

            MostPopularCategoryIDs = GetMostPopularParameters(ids, ref _tempCategoryIDs);
        }
    }
    
    private void SetMostPopularProductIDs(object? argument)
    {
        lock (_productIDsLocker)
        {
            if (argument is not Event[] events) return;

            var ids = events
                .GroupBy(x => x.ProductId)
                .Select(g =>
                    (ID: g.Key, Count: g.Count()))
                .ToArray();

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