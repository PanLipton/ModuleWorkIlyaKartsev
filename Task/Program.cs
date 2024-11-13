using System;
using System.IO;
using System.Numerics;

namespace Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SolveStringComputer();
        }

        public static void SolveStringComputer()
        {
            // Шлях до директорії Task у кореневій директорії проєкту
            string projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Task"));
            string inputFilePath = Path.Combine(projectDirectory, "INPUT.txt");
            string outputFilePath = Path.Combine(projectDirectory, "OUTPUT.TXT");

            // Перевіряємо наявність файлу INPUT.txt та виводимо діагностичну інформацію
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("Поточна директорія: " + Directory.GetCurrentDirectory());
                Console.WriteLine("Очікуваний шлях для INPUT.txt: " + inputFilePath);
                throw new Exception("Файл INPUT.txt не знайдено за шляхом: " + inputFilePath);
            }

            // Зчитуємо вміст INPUT.txt
            string[] input;
            try
            {
                input = File.ReadAllText(inputFilePath).Split();
            }
            catch (Exception)
            {
                throw new Exception("Не вдалося прочитати файл INPUT.txt");
            }

            // Перевіряємо, що у файлі два числа
            if (input.Length != 2)
                throw new Exception("У файлі INPUT.txt повинно бути два числа!");

            // Пытаємося перетворити рядки на числа
            if (!int.TryParse(input[0], out int N) || !int.TryParse(input[1], out int K))
                throw new Exception("Вхідні дані повинні бути цілими числами!");

            // Перевіряємо діапазон чисел
            if (N < 1 || N > 100 || K < 1 || K > 100)
                throw new Exception("Числа N і K повинні бути в діапазоні від 1 до 100 включно!");

            // Обчислюємо суму розміщень з повторенням для довжини від 2 до N
            BigInteger totalStrings = 0;
            for (int i = 2; i <= N; i++)
            {
                totalStrings += BigInteger.Pow(K, i);
            }

            // Записуємо результат у OUTPUT.TXT
            File.WriteAllText(outputFilePath, $"{totalStrings}\n1");
            Console.WriteLine($"Результат із вхідними значеннями INPUT: {totalStrings}");
        }
    }
}