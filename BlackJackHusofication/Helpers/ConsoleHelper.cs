namespace BlackJackHusofication.Helpers;

internal class ConsoleHelper
{
    public static void Write(string message){
        Console.Write(message);
    }

    public static void Write(string message, ConsoleColor foreColor)
    {
        Console.ForegroundColor = foreColor;
        Console.Write(message);
        Console.ResetColor(); 
    }

    public static void Write(string message, ConsoleColor foreColor, ConsoleColor backColor)
    {
        Console.ForegroundColor = foreColor;
        Console.BackgroundColor = backColor;
        Console.Write(message);
        Console.ResetColor();
    }

    public static void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public static void WriteLine(string message, ConsoleColor foreColor)
    {
        Console.ForegroundColor = foreColor;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void WriteLine(string message, ConsoleColor foreColor, ConsoleColor backColor)
    {
        Console.ForegroundColor = foreColor;
        Console.BackgroundColor = backColor;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    static void ClearLastConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor - 1);
    }
}
