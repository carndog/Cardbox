using static System.Console;

namespace WordServices.Output;

public static class ConsoleColors
{
    public const string Reset = "\x1b[0m";
    public const string Black = "\x1b[30m";
    public const string Red = "\x1b[31m";
    public const string Green = "\x1b[32m";
    public const string Yellow = "\x1b[33m";
    public const string Blue = "\x1b[34m";
    public const string Magenta = "\x1b[35m";
    public const string Cyan = "\x1b[36m";
    public const string White = "\x1b[37m";
    public const string BrightBlack = "\x1b[90m";
    public const string BrightRed = "\x1b[91m";
    public const string BrightGreen = "\x1b[92m";
    public const string BrightYellow = "\x1b[93m";
    public const string BrightBlue = "\x1b[94m";
    public const string BrightMagenta = "\x1b[95m";
    public const string BrightCyan = "\x1b[96m";
    public const string BrightWhite = "\x1b[97m";

    public static string ColorText(string text, string color)
    {
        return $"{color}{text}{Reset}";
    }

    public static void WriteColored(string text, string color)
    {
        Write(ColorText(text, color));
    }

    public static void WriteLineColored(string text, string color)
    {
        WriteLine(ColorText(text, color));
    }

    public static void WriteSuccess(string text)
    {
        WriteLineColored(text, Green);
    }

    public static void WriteError(string text)
    {
        WriteLineColored(text, Red);
    }

    public static void WriteWarning(string text)
    {
        WriteLineColored(text, Yellow);
    }

    public static void WriteInfo(string text)
    {
        WriteLineColored(text, Cyan);
    }
}
