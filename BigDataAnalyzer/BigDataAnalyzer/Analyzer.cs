using System.Text;
using BigDataAnalyzer.Interfaces;

namespace BigDataAnalyzer;

public class Analyzer : IAnalyzer
{
    private readonly object _amountOfIncomeLocker;
    private readonly object _brandLocker;
    private (string? Name, int Count)[] _tempBrands;

    public Analyzer()
    {
        _amountOfIncomeLocker = new object();
        _brandLocker = new object();

        _tempBrands = Array.Empty<(string? Name, int Count)>();

        MostPopularBrands = Array.Empty<(string? Name, int Count)>();
    }

    public decimal? AmountOfIncome { get; private set; }
    public (string? Name, int Count)[] MostPopularBrands { get; private set; }
    public long MostPopularCategoryCode { get; private set; }
    public long MostPopularProduct { get; private set; }

    public void StartPackageAnalyze(string path, int packageSize)
    {
        using var streamReader = new StreamReader(path, Encoding.Default);

        string? line;

        var t1 = 0;
        var t2 = 0;

        streamReader.ReadLine(); // выкидываю первую строку с заголовками

        var events = new List<Event>();

        while ((line = streamReader.ReadLine()) != null)
        {
            var strings = line.Split(",");

            events.Add(new Event(strings));

            if (events.Count <= packageSize) continue; //TODO  надо асинхронно отдавать не верное условие

            var eventsCopy = events.ToArray();
            events.Clear();

            /*var amountOfIncome = new Thread(SetAmountOfIncome)
            {
                Name = $"Поток - Пакет № {++t1} (установка суммы выручки)"
            };

            amountOfIncome.Start(eventsCopy);*/

            var mostPopularBrand = new Thread(SetMostPopularBrands)
            {
                Name = $"Поток - Пакет № {++t2} (популярность бренда)"
            };
            mostPopularBrand.Start(eventsCopy);
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