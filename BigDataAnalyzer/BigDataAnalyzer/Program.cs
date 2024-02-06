using BigDataAnalyzer;

var analyzer = new Analyzer();


analyzer.StartPackageAnalyze(new[] {@"C:\\Users\\evvlk\\Downloads\\Яндекс Браузер\\archive\\2019-Oct.csv"});

var amountOfIncome = analyzer.AmountOfIncome;
var mostPopularBrand = analyzer.MostPopularBrands;
var mostPopularCategoryIDs = analyzer.MostPopularCategoryIDs;
var mostPopularProductIDs = analyzer.MostPopularProductIDs;

Console.WriteLine();