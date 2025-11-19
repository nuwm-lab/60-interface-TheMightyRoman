using System;

namespace LabWork16
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
            Console.WriteLine("-> Створено об'єкт векторної системи (Constructor)");
        }

        // Деструктор (вимога методички)
        ~VectorSystemBase()
        {
            Console.WriteLine("-> Об'єкт векторної системи видалено (Destructor)");
        }

        // Абстрактні методи, які мають реалізувати нащадки
        public abstract void Input();
        public abstract void Show();
        public abstract bool IsLinearlyIndependent();
    }

    // 3. Клас "Система двох векторів"
    public class TwoVectorSystem : VectorSystemBase
    {
        // Використовуємо protected, щоб мати доступ у класі-спадкоємці
        protected double Ax, Ay;
        protected double Bx, By;

        public TwoVectorSystem() : base() { }

        public override void Input()
        {
            Console.WriteLine("\n--- Введення системи 2-х векторiв (2D) ---");
            Console.WriteLine("Введiть координати вектора A:");
            Console.Write("ax: "); Ax = GetDouble();
            Console.Write("ay: "); Ay = GetDouble();

            Console.WriteLine("Введiть координати вектора B:");
            Console.Write("bx: "); Bx = GetDouble();
            Console.Write("by: "); By = GetDouble();
        }

        public override void Show()
        {
            Console.WriteLine("\n--- Система 2-х векторiв ---");
            Console.WriteLine($"Вектор A = ({Ax}, {Ay})");
            Console.WriteLine($"Вектор B = ({Bx}, {By})");
        }

        public override bool IsLinearlyIndependent()
        {
            // Для 2D вектори незалежні, якщо визначник матриці не дорівнює 0
            // | ax ay |
            // | bx by |
            double det = Ax * By - Ay * Bx;
            Console.WriteLine($"Детермінант: {det:F2}");
            return Math.Abs(det) > 1e-9;
        }

        // Допоміжний метод для зчитування чисел
        protected double GetDouble()
        {
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out double result))
                    return result;
                Console.Write("Помилка. Введіть число: ");
            }
        }
    }

    // 4. Похідний клас "Система трьох векторів"
    public class ThreeVectorSystem : TwoVectorSystem
    {
        // Додаємо Z-координати для A і B, та повний вектор C
        private double _az, _bz;
        private double _cx, _cy, _cz;

        public ThreeVectorSystem() : base() { }

        public override void Input()
        {
            Console.WriteLine("\n--- Введення системи 3-х векторiв (3D) ---");
            Console.WriteLine("Введiть координати вектора A:");
            Console.Write("ax: "); Ax = GetDouble();
            Console.Write("ay: "); Ay = GetDouble();
            Console.Write("az: "); _az = GetDouble();

            Console.WriteLine("Введiть координати вектора B:");
            Console.Write("bx: "); Bx = GetDouble();
            Console.Write("by: "); By = GetDouble();
            Console.Write("bz: "); _bz = GetDouble();

            Console.WriteLine("Введiть координати вектора C:");
            Console.Write("cx: "); _cx = GetDouble();
            Console.Write("cy: "); _cy = GetDouble();
            Console.Write("cz: "); _cz = GetDouble();
        }

        public override void Show()
        {
            Console.WriteLine("\n--- Система 3-х векторiв ---");
            Console.WriteLine($"Вектор A = ({Ax}, {Ay}, {_az})");
            Console.WriteLine($"Вектор B = ({Bx}, {By}, {_bz})");
            Console.WriteLine($"Вектор C = ({_cx}, {_cy}, {_cz})");
        }

        public override bool IsLinearlyIndependent()
        {
            // Визначник матриці 3x3
            // | ax ay az |
            // | bx by bz |
            // | cx cy cz |
            
            // Правило трикутника (Саррюса)
            double det = Ax * By * _cz + Ay * _bz * _cx + _az * Bx * _cy
                       - _az * By * _cx - Ay * Bx * _cz - Ax * _bz * _cy;

            Console.WriteLine($"Детермінант: {det:F2}");
            return Math.Abs(det) > 1e-9;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // Поліморфізм: використовуємо інтерфейс або базовий клас як тип змінної
            IVectorSystem vectorSystem = null;

            while (true)
            {
                Console.WriteLine("\n============================================");
                Console.WriteLine("Оберiть тип системи векторів:");
                Console.WriteLine("1 - Система 2-х векторiв (2D)");
                Console.WriteLine("2 - Система 3-х векторiв (3D)");
                Console.WriteLine("0 - Вихiд");
                Console.Write("Ваш вибiр: ");

                string choice = Console.ReadLine();

                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        vectorSystem = new TwoVectorSystem();
                        break;
                    case "2":
                        vectorSystem = new ThreeVectorSystem();
                        break;
                    default:
                        Console.WriteLine("Некоректний вибір.");
                        continue;
                }

                // Робота через інтерфейс/базовий клас
                vectorSystem.Input();
                vectorSystem.Show();

                if (vectorSystem.IsLinearlyIndependent())
                {
                    Console.WriteLine("РЕЗУЛЬТАТ: Вектори ЛІНІЙНО НЕЗАЛЕЖНІ (утворюють базис).");
                }
                else
                {
                    Console.WriteLine("РЕЗУЛЬТАТ: Вектори ЛІНІЙНО ЗАЛЕЖНІ.");
                }

                // Примусовий виклик збирача сміття для демонстрації деструкторів (не обов'язково в реальному коді)
                vectorSystem = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
