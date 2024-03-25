using System.Diagnostics;

class Program
{
    static void Main()
    {
        const int numberOfIntegers = 10000000;
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string outputFile = desktopPath + "\\random_integers.txt";

        // Crear un generador de números aleatorios
        Random random = new Random();

        // Usar un HashSet para almacenar los enteros generados y garantizar que sean únicos
        HashSet<int> uniqueIntegers = new HashSet<int>();

        // Crear un cronómetro para medir el tiempo
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Generar números enteros aleatorios únicos
        while (uniqueIntegers.Count < numberOfIntegers)
        {
            int randomNumber = random.Next();
            uniqueIntegers.Add(randomNumber);
        }

        // Escribir los números enteros en el archivo de texto
        using (StreamWriter writer = new StreamWriter(outputFile))
        {
            foreach (int number in uniqueIntegers)
            {
                writer.WriteLine(number);
            }
        }

        stopwatch.Stop();

        Console.WriteLine($"Se han generado {uniqueIntegers.Count} números enteros aleatorios distintos en el archivo '{outputFile}'.");
        Console.WriteLine($"Tiempo transcurrido: {stopwatch.Elapsed.TotalSeconds} segundos.");
    }

}
