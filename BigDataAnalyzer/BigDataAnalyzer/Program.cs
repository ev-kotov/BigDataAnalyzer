using System.Text;
using BigDataAnalyzer;

var analyzer = new Analyzer();

analyzer.StartPackageAnalyze(Convert.ToInt32(args.First()), args.Skip(1).ToArray());

var builder = new StringBuilder("***");

builder.AppendLine();
builder.AppendLine("Общая сумма выручки:");
builder.AppendLine($"{analyzer.AmountOfIncome}");

builder.AppendLine();
builder.AppendLine("Самые популярные бренды:");
for (var i = 0; i < 3; i++)
{
    var name = analyzer.MostPopularBrands[i].Name;

    if (name == string.Empty)
        name = "Неопределённый бренд";

    builder.AppendLine($"{i + 1} место - {name} (количество событий - {analyzer.MostPopularBrands[i].Count})");
}

builder.AppendLine();
builder.AppendLine("Самая популярная категория (ID)");
builder.AppendLine(
    $"{analyzer.MostPopularCategoryIDs[0].Id} (количество событий -{analyzer.MostPopularCategoryIDs[0].Count})");

builder.AppendLine();
builder.AppendLine("Самый популярный товар (ID)");
builder.AppendLine(
    $"{analyzer.MostPopularProductIDs[0].Id} (количество событий -{analyzer.MostPopularProductIDs[0].Count})");
builder.AppendLine();

Console.WriteLine(builder);

Console.ReadKey();