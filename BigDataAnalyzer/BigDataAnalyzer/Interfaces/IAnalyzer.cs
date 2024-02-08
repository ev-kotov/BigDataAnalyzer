namespace BigDataAnalyzer.Interfaces;

/// <summary>
///     Анализатор данных
/// </summary>
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
    /// <returns>Бренд и количество</returns>
    (string? Name, int Count)[] MostPopularBrands { get; }

    /// <summary>
    ///     Самые популярные категории
    /// </summary>
    /// <returns>ID категории и количество</returns>
    (long Id, int Count)[] MostPopularCategoryIDs { get; }

    /// <summary>
    ///     Самые популярные товары
    /// </summary>
    /// <returns>ID продукта и количество</returns>
    (long Id, int Count)[] MostPopularProductIDs { get; }

    /// <summary>
    ///     Запустить пакетный анализа данных
    /// </summary>
    /// <param name="packageSize">Размер пакета</param>
    /// <param name="filePaths">Пути к файлам</param>
    public void StartPackageAnalyze(int packageSize, string[] filePaths);
}