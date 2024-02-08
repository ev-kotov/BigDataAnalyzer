namespace BigDataAnalyzer;

public static class SimpleProgressBar
{
    public static void Show(string operation, int progress, int total, int cursorTop = 0)
    {
        Console.CursorLeft = 0;
        Console.CursorTop = cursorTop;
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.Green;

        var status = Math.Round(Convert.ToDecimal((progress + 1) / (total / 100)));

        Console.WriteLine($"{operation} - {status} %                                                           ");
    }
}