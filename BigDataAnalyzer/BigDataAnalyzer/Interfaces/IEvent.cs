namespace BigDataAnalyzer.Interfaces;

/// <summary>
///     Событие
/// </summary>
public interface IEvent
{
    /// <summary>
    ///     Дата события
    /// </summary>
    DateTime EventTime { get; }

    /// <summary>
    ///     Тип события
    /// </summary>
    string? EventType { get; }

    /// <summary>
    ///     ID продукта
    /// </summary>
    long ProductId { get; }

    /// <summary>
    ///     ID категории
    /// </summary>
    long CategoryId { get; }

    /// <summary>
    ///     Шифр категории
    /// </summary>
    string? CategoryCode { get; }

    /// <summary>
    ///     Бренд
    /// </summary>
    string? Brand { get; }

    /// <summary>
    ///     Стоимость
    /// </summary>
    decimal? Price { get; }

    /// <summary>
    ///     ID пользователя
    /// </summary>
    long UserId { get; }

    /// <summary>
    ///     Сессия пользователя
    /// </summary>
    Guid UserSession { get; }
}