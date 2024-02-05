using BigDataAnalyzer;

var analyzer = new Analyzer();


analyzer.StartPackageAnalyze(@"C:\Users\evvlk\Downloads\Яндекс Браузер\archive\2019-Oct.csv", 1000000);

var amountOfIncome = analyzer.AmountOfIncome;

var mostPopularBrand = analyzer.MostPopularBrands;

Console.WriteLine();