using System;
using System.IO;
using System.Numerics;
using Xunit;
using Xunit.Abstractions;

namespace Task.Tests
{
    /// <summary>
    /// Тестовий клас для перевірки функціональності обчислення кількості можливих рядків.
    /// N - довжина рядка
    /// K - кількість символів в алфавіті
    /// Тести перевіряють різні комбінації вхідних даних та граничні випадки.
    /// </summary>
    public class ProgramTests : IDisposable
    {
        private readonly string projectDirectory;
        private readonly string inputFilePath;
        private readonly string outputFilePath;
        private readonly ITestOutputHelper _testOutput;

        public ProgramTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
            
            // Отримуємо шлях до директорії Task
            projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Task"));
            inputFilePath = Path.Combine(projectDirectory, "INPUT.txt");
            outputFilePath = Path.Combine(projectDirectory, "OUTPUT.TXT");
            
            _testOutput.WriteLine($"Шлях до проекту: {projectDirectory}");
            _testOutput.WriteLine($"Шлях до INPUT.txt: {inputFilePath}");
            _testOutput.WriteLine($"Шлях до OUTPUT.TXT: {outputFilePath}");
            
            // Створюємо директорію Task, якщо вона не існує
            Directory.CreateDirectory(projectDirectory);
            
            // Очищаємо файли перед кожним тестом
            CleanupFiles();
        }

        private void CleanupFiles()
        {
            if (File.Exists(inputFilePath))
            {
                File.Delete(inputFilePath);
                _testOutput.WriteLine("Видалено існуючий INPUT.txt");
            }
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
                _testOutput.WriteLine("Видалено існуючий OUTPUT.TXT");
            }
        }

        /// <summary>
        /// Тест перевіряє стандартний випадок: N=2, K=3
        /// Очікуваний результат: 9 (3² = 9 можливих комбінацій)
        /// </summary>
        [Fact]
        public void SolveStringComputer_ValidInput_CorrectOutput()
        {
            _testOutput.WriteLine("Тест: Перевірка стандартного випадку N=2, K=3");
            
            // Arrange
            File.WriteAllText(inputFilePath, "2 3");
            _testOutput.WriteLine("Створено INPUT.txt з вмістом: '2 3'");

            // Act
            Program.SolveStringComputer();
            _testOutput.WriteLine("Виконано SolveStringComputer()");

            // Assert
            Assert.True(File.Exists(outputFilePath), "OUTPUT.TXT файл не був створений");
            string[] result = File.ReadAllLines(outputFilePath);
            
            _testOutput.WriteLine($"Отримано результат: {string.Join(", ", result)}");
            Assert.Equal(2, result.Length);
            Assert.Equal("9", result[0]);
            Assert.Equal("1", result[1]);
        }

        /// <summary>
        /// Тест перевіряє максимально допустимі значення: N=100, K=100
        /// </summary>
        [Fact]
        public void SolveStringComputer_MaximumInput_CorrectOutput()
        {
            _testOutput.WriteLine("Тест: Перевірка максимальних значень N=100, K=100");
            
            File.WriteAllText(inputFilePath, "100 100");
            _testOutput.WriteLine("Створено INPUT.txt з вмістом: '100 100'");

            Program.SolveStringComputer();
            _testOutput.WriteLine("Виконано SolveStringComputer()");

            Assert.True(File.Exists(outputFilePath), "OUTPUT.TXT файл не був створений");
            string[] result = File.ReadAllLines(outputFilePath);
            
            _testOutput.WriteLine($"Отримано результат: {string.Join(", ", result)}");
            Assert.Equal(2, result.Length);
            Assert.True(BigInteger.TryParse(result[0], out _), "Результат має бути числом");
            Assert.Equal("1", result[1]);
        }

        /// <summary>
        /// Тест перевіряє мінімально допустимі значення: N=1, K=1
        /// Очікуваний результат: 0 (оскільки довжина має бути від 2 до N)
        /// </summary>
        [Fact]
        public void SolveStringComputer_MinimumInput_CorrectOutput()
        {
            _testOutput.WriteLine("Тест: Перевірка мінімальних значень N=1, K=1");
            
            File.WriteAllText(inputFilePath, "1 1");
            _testOutput.WriteLine("Створено INPUT.txt з вмістом: '1 1'");

            Program.SolveStringComputer();
            _testOutput.WriteLine("Виконано SolveStringComputer()");

            Assert.True(File.Exists(outputFilePath), "OUTPUT.TXT файл не був створений");
            string[] result = File.ReadAllLines(outputFilePath);
            
            _testOutput.WriteLine($"Отримано результат: {string.Join(", ", result)}");
            Assert.Equal(2, result.Length);
            Assert.Equal("0", result[0]);
            Assert.Equal("1", result[1]);
        }

        /// <summary>
        /// Тест перевіряє реакцію на некоректні вхідні дані (не числа)
        /// </summary>
        [Fact]
        public void SolveStringComputer_InvalidInput_ThrowsException()
        {
            _testOutput.WriteLine("Тест: Перевірка некоректних вхідних даних (не числа)");
            
            File.WriteAllText(inputFilePath, "x y");
            _testOutput.WriteLine("Створено INPUT.txt з вмістом: 'x y'");

            var exception = Assert.Throws<Exception>(() => Program.SolveStringComputer());
            _testOutput.WriteLine($"Отримано виняток: {exception.Message}");
            
            Assert.Contains("Вхідні дані повинні бути цілими числами!", exception.Message);
        }

        /// <summary>
        /// Тест перевіряє реакцію на відсутність другого числа
        /// </summary>
        [Fact]
        public void SolveStringComputer_MissingInput_ThrowsException()
        {
            _testOutput.WriteLine("Тест: Перевірка відсутності другого числа");
            
            File.WriteAllText(inputFilePath, "5");
            _testOutput.WriteLine("Створено INPUT.txt з вмістом: '5'");

            var exception = Assert.Throws<Exception>(() => Program.SolveStringComputer());
            _testOutput.WriteLine($"Отримано виняток: {exception.Message}");
            
            Assert.Contains("У файлі INPUT.txt повинно бути два числа!", exception.Message);
        }

        [Fact]
        public void SolveStringComputer_InputOutOfRange_ThrowsException()
        {
            _testOutput.WriteLine("Тест: Перевірка чисел поза допустимим діапазоном");
            
            File.WriteAllText(inputFilePath, "101 2");
            _testOutput.WriteLine("Створено INPUT.txt з вмістом: '101 2'");

            var exception = Assert.Throws<Exception>(() => Program.SolveStringComputer());
            _testOutput.WriteLine($"Отримано виняток: {exception.Message}");
            
            Assert.Contains("Числа N і K повинні бути в діапазоні від 1 до 100 включно!", exception.Message);
        }

        [Fact]
        public void SolveStringComputer_LargeKSmallN_CorrectOutput()
        {
            _testOutput.WriteLine("Тест: Перевірка великого K і малого N (K=100, N=3)");
            
            File.WriteAllText(inputFilePath, "3 100");
            _testOutput.WriteLine("Створено INPUT.txt з вмістом: '3 100'");

            Program.SolveStringComputer();
            _testOutput.WriteLine("Виконано SolveStringComputer()");

            Assert.True(File.Exists(outputFilePath), "OUTPUT.TXT файл не був створений");
            string[] result = File.ReadAllLines(outputFilePath);
            
            _testOutput.WriteLine($"Отримано результат: {string.Join(", ", result)}");
            Assert.Equal(2, result.Length);
            Assert.True(BigInteger.TryParse(result[0], out _), "Результат має бути числом");
            Assert.Equal("1", result[1]);
        }

        [Fact]
        public void SolveStringComputer_LargeNSmallK_CorrectOutput()
        {
            _testOutput.WriteLine("Тест: Перевірка N=3 і K=2");
            
            File.WriteAllText(inputFilePath, "3 2");
            _testOutput.WriteLine("Створено INPUT.txt з вмістом: '3 2'");

            Program.SolveStringComputer();
            _testOutput.WriteLine("Виконано SolveStringComputer()");

            Assert.True(File.Exists(outputFilePath), "OUTPUT.TXT файл не був створений");
            string[] result = File.ReadAllLines(outputFilePath);
            
            _testOutput.WriteLine($"Отримано результат: {string.Join(", ", result)}");
            Assert.Equal(2, result.Length);
            Assert.Equal("12", result[0]);
            Assert.Equal("1", result[1]);
        }

        [Fact]
        public void SolveStringComputer_NEqualToTwo_CorrectOutput()
        {
            _testOutput.WriteLine("Тест: Перевірка N=2 і K=2");
            
            File.WriteAllText(inputFilePath, "2 2");
            _testOutput.WriteLine("Створено INPUT.txt з вмістом: '2 2'");

            Program.SolveStringComputer();
            _testOutput.WriteLine("Виконано SolveStringComputer()");

            Assert.True(File.Exists(outputFilePath), "OUTPUT.TXT файл не був створений");
            string[] result = File.ReadAllLines(outputFilePath);
            
            _testOutput.WriteLine($"Отримано результат: {string.Join(", ", result)}");
            Assert.Equal(2, result.Length);
            Assert.Equal("4", result[0]);
            Assert.Equal("1", result[1]);
        }

        [Fact]
        public void SolveStringComputer_KEqualToOne_CorrectOutput()
        {
            _testOutput.WriteLine("Тест: Перевірка K=1 (мінімальний алфавіт)");
            
            File.WriteAllText(inputFilePath, "3 1");
            _testOutput.WriteLine("Створено INPUT.txt з вмістом: '3 1'");

            Program.SolveStringComputer();
            _testOutput.WriteLine("Виконано SolveStringComputer()");

            Assert.True(File.Exists(outputFilePath), "OUTPUT.TXT файл не був створений");
            string[] result = File.ReadAllLines(outputFilePath);
            
            _testOutput.WriteLine($"Отримано результат: {string.Join(", ", result)}");
            Assert.Equal(2, result.Length);
            Assert.Equal("2", result[0]);
            Assert.Equal("1", result[1]);
        }

        public void Dispose()
        {
            _testOutput.WriteLine("Очищення тестових файлів");
            CleanupFiles();
        }
    }
}