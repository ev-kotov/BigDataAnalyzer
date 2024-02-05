namespace BigDataAnalyzer.Interfaces;

public interface IAnalyzer
{
    /// <summary>
    ///     Сумма выручки
    /// </summary>
    /// <returns>Сумма выручки</returns>
    decimal? AmountOfIncome { get; }

    /// <summary>
    ///     Самые популярные бренды
    /// </summary>
    /// <returns>Бренды</returns>
    (string? Name, int Count)[] MostPopularBrands { get; }

    /// <summary>
    ///     Самая популярная категория
    /// </summary>
    /// <returns>ID категории</returns>
    long MostPopularCategoryCode { get; }

    /// <summary>
    ///     Самый популярный товар
    /// </summary>
    /// <returns>ID продукта</returns>
    long MostPopularProduct { get; }

    /// <summary>
    ///     Запустить пакетный анализа данных
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    /// <param name="packageSize">Размер пакета</param>
    public void StartPackageAnalyze(string path, int packageSize);
}