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
}
