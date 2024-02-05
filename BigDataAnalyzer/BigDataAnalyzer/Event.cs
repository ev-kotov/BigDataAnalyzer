using System.Globalization;
using BigDataAnalyzer.Interfaces;

namespace BigDataAnalyzer;

public class Event : IEvent
{
    private readonly string _eventStringTime = string.Empty;
    private readonly DateTime _eventTime;
    private readonly string[] _formats = Array.Empty<string>();
    private readonly Guid _userSession;
    private readonly string _userSessionString = string.Empty;

    public Event(DateTime eventTime, string? eventType, int productId, int categoryId, string? categoryCode,
        string? brand,
        decimal? price, long userId, Guid userSession)
    {
        _eventTime = eventTime;
        EventType = eventType;
        ProductId = productId;
        CategoryId = categoryId;
        CategoryCode = categoryCode;
        Brand = brand;
        Price = price;
        UserId = userId;
        _userSession = userSession;
    }

    public Event(Span<string> strings)
    {
        _formats = new[] {"yyyy-MM-dd HH:mm:ss UTC"};
        _eventStringTime = strings[0];
        EventType = strings[1];
        ProductId = long.Parse(strings[2]);
        CategoryId = long.Parse(strings[3]);
        CategoryCode = strings[4];
        Brand = strings[5];
        Price = decimal.Parse(strings[6], new NumberFormatInfo {NumberDecimalSeparator = "."});
        UserId = long.Parse(strings[7]);
        _userSessionString = strings[0];
    }

    public DateTime EventTime
    {
        get
        {
            if (_eventStringTime != string.Empty)
                return DateTime.ParseExact(_eventStringTime, _formats, new CultureInfo("ru-RU"),
                    DateTimeStyles.AdjustToUniversal);

            return _eventTime;
        }
    }

    public string? EventType { get; }

    public long ProductId { get; }

    public long CategoryId { get; }

    public string? CategoryCode { get; }

    public string? Brand { get; }

    public decimal? Price { get; }

    public long UserId { get; }

    public Guid UserSession
    {
        get
        {
            if (_userSessionString != string.Empty)
                // TODO: Уточнить у заказчика
                return Guid.TryParse(_userSessionString, out var guid) ? guid : Guid.NewGuid();

            return _userSession;
        }
    }
}