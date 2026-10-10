using System.Globalization;

namespace HseKat.ConsoleApp;

public sealed class ConsoleInput
{
    public string ReadText(string prompt)
    {
        while (true)
        {
            var line = ReadLine(prompt).Trim();
            if (line.Length > 0)
                return line;

            Console.WriteLine("Значение не может быть пустым.");
        }
    }

    public int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            var line = ReadLine(prompt);
            if (int.TryParse(line, out var value)
                && value >= min && value <= max)
                return value;

            Console.WriteLine($"Введите целое число от {min} до {max}.");
        }
    }

    public double ReadDouble(string prompt, double min)
    {
        while (true)
        {
            var line = ReadLine(prompt).Replace(',', '.');
            if (double.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                && double.IsFinite(value) && value >= min)
                return value;

            Console.WriteLine($"Введите число не меньше {min}.");
        }
    }

    private static string ReadLine(string prompt)
    {
        Console.Write($"{prompt}: ");
        return Console.ReadLine() ?? throw new EndOfStreamException();
    }
}
