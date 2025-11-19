using System;
using System.Collections.Generic;

namespace LabWork16_Refactored
{
    // 1. Інтерфейс
    public interface IVectorSystem
    {
        void Input();
        void Show();
        bool IsLinearlyIndependent();
    }

    // 2. Абстрактний клас
    public abstract class VectorSystemBase : IVectorSystem
    {
        // Конструктор
        protected VectorSystemBase()
        {
            // Логіка ініціалізації (якщо потрібна)
        }

        // Деструктор (вимога методички: "В класах повинні бути присутні конструктори та деструктори")
        // В реальному C# коді деструктори використовуються рідко, тільки для Unmanaged Resources.
        ~VectorSystemBase()
        {
            // Порожній деструктор, щоб не порушувати Code Convention виводом в консоль
        }

        // Абстрактні методи
        public abstract void Input();
        public abstract void Show();
        public abstract bool IsLinearlyIndependent();

        // Допоміжний метод для безпечного введення чисел (DRY - Don't Repeat Yourself)
        protected double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double result))
                {
                    return result;
                }
                Console.WriteLine("Помилка! Введiть коректне число.");
            }
        }
    }

    // 3. Клас "Система двох векторів" (2D)
    // Вектори A(ax, ay), B(bx, by)
    public class TwoVectorSystem : VectorSystemBase
    {
        // Приватні поля (backing fields) - camelCase
        private double _ax, _ay;
        private double _bx, _by;

        // Публічні властивості - PascalCase
        public double Ax { get => _ax; set => _ax = value; }
        public double Ay { get => _ay; set => _ay = value; }
        public double Bx { get => _bx; set => _bx = value; }
        public double By { get => _by; set => _by = value; }

        public TwoVectorSystem() : base() { }

        public override void Input()
        {
            Console.WriteLine("\n--- Введення системи 2-х векторiв (2D) ---");
            Console.WriteLine("Вектор A:");
            Ax = ReadDouble("ax: ");
            Ay = ReadDouble("ay: ");

            Console.WriteLine("Вектор B:");
            Bx = ReadDouble("bx: ");
            By = ReadDouble("by: ");
        }

        public override void Show()
        {
            Console.WriteLine($"Система 2D: A({Ax}, {Ay}), B({Bx}, {By})");
        }

        public override bool IsLinearlyIndependent()
        {
            // Det = | ax ay |
            //       | bx by |
            double det = Ax * By - Ay * Bx;
            // Використовуємо мале число для порівняння double з нулем
            return Math.Abs(det) > 1e-9;
        }
    }

    // 4. Клас "Система трьох векторів" (3D)
    // Успадковуємось від 2D, додаємо Z координати та вектор C
    public class ThreeVectorSystem : TwoVectorSystem
    {
        // Додаткові приватні поля
        private double _az, _bz;       // Z-координати для A і B
        private double _cx, _cy, _cz;  // Новий вектор C

        // Властивості
        public double Az { get => _az; set => _az = value; }
        public double Bz { get => _bz; set => _bz = value; }
        
        public double Cx { get => _cx; set => _cx = value; }
        public double Cy { get => _cy; set => _cy = value; }
        public double Cz { get => _cz; set => _cz = value; }

        public ThreeVectorSystem() : base() { }

        public override void Input()
        {
            Console.WriteLine("\n--- Введення системи 3-х векторiв (3D) ---");
            // Перевизначаємо логіку повністю, щоб зберегти порядок введення
            Console.WriteLine("Вектор A:");
            Ax = ReadDouble("ax: ");
            Ay = ReadDouble("ay: ");
            Az = ReadDouble("az: ");

            Console.WriteLine("Вектор B:");
            Bx = ReadDouble("bx: ");
            By = ReadDouble("by: ");
            Bz = ReadDouble("bz: ");

            Console.WriteLine("Вектор C:");
            Cx = ReadDouble("cx: ");
            Cy = ReadDouble("cy: ");
            Cz = ReadDouble("cz: ");
        }

        public override void Show()
        {
            Console.WriteLine($"Система 3D: A({Ax}, {Ay}, {Az}), B({Bx}, {By}, {Bz}), C({Cx}, {Cy}, {Cz})");
        }

        public override bool IsLinearlyIndependent()
        {
            // Визначник 3-го порядку (Правило Саррюса / Трикутника)
            // | Ax Ay Az |
            // | Bx By Bz |
            // | Cx Cy Cz |
            
            double det = Ax * By * Cz + Ay * Bz * Cx + Az * Bx * Cy 
                       - Az * By * Cx - Ay * Bx * Cz - Ax * Bz * Cy;

            return Math.Abs(det) > 1e-9;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Демонстрація поліморфізму через список (Collection)
            // Це виконує рекомендацію "Покажіть приклад колекції"
            List<IVectorSystem> systems = new List<IVectorSystem>();

            Console.WriteLine("Створення об'єктів...");
            
            // Додаємо об'єкти різних типів в один список
            // (В реальній програмі тут міг би бути цикл із запитом до користувача)
            systems.Add(new TwoVectorSystem());
            systems.Add(new ThreeVectorSystem());

            // Проходимо по списку і працюємо з об'єктами універсально
            foreach (var system in systems)
            {
                system.Input();
                system.Show();

                if (system.IsLinearlyIndependent())
                {
                    Console.WriteLine("-> Вектори ЛІНІЙНО НЕЗАЛЕЖНІ (утворюють базис).");
                }
                else
                {
                    Console.WriteLine("-> Вектори ЛІНІЙНО ЗАЛЕЖНІ.");
                }
                Console.WriteLine("------------------------------------------------");
            }

            Console.WriteLine("Роботу завершено. Натисніть Enter.");
            Console.ReadLine();
        }
    }
}
