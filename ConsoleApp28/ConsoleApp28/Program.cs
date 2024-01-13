using System;
using System.Collections.Generic;

namespace CarRentalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int remainingAttempts = 3;
            string userName, password;

            while (remainingAttempts > 0)
            {
                Console.WriteLine("Kullanıcı Adı:");
                userName = Console.ReadLine();

                Console.WriteLine("Şifre:");
                password = GetPasswordFromConsole();

                if (userName == "admin" && password == "3636")
                {
                    Console.WriteLine("Giriş başarılı! Hoş geldiniz!");
                    // Giriş başarılı ise devam eden kod buraya gelebilir
                    RentalService rentalService = new RentalService();
                    RunRentalService(rentalService); // Yeni metod

                    return; // Programı sonlandırma
                }
                else
                {
                    remainingAttempts--;
                    Console.WriteLine($"Kullanıcı adı veya şifre hatalı. Kalan deneme hakkı: {remainingAttempts}");

                    if (remainingAttempts == 0)
                    {
                        Console.WriteLine("Giriş hakkınız tükendi. Program sonlandırılıyor.");
                        return; // Programı sonlandırma
                    }
                }
            }
        }

        // Yeni metod
        static void RunRentalService(RentalService rentalService)
        {
            while (true)
            {
                Console.WriteLine("1. Araç Ekle");
                Console.WriteLine("2. Araç Kirala");
                Console.WriteLine("3. Kiralık Araçları Listele");
                Console.WriteLine("4. Çıkış");
                Console.Write("Seçiminiz: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Araç Plakası: ");
                        string plateNumber = Console.ReadLine();
                        Console.Write("Marka: ");
                        string make = Console.ReadLine();
                        Console.Write("Model: ");
                        string model = Console.ReadLine();
                        Console.Write("Yıl: ");
                        int year = int.Parse(Console.ReadLine());
                        rentalService.AddCar(plateNumber, make, model, year);
                        break;

                    case "2":
                        Console.Write("Müşteri Adı: ");
                        string customerName = Console.ReadLine();
                        Console.Write("Araç Plakası: ");
                        string rentedPlateNumber = Console.ReadLine();
                        Console.Write("Kiralama Tarihi (GG.AA.YYYY): ");
                        DateTime rentalDate = DateTime.Parse(Console.ReadLine());
                        rentalService.RentCar(customerName, rentedPlateNumber, rentalDate);
                        break;

                    case "3":
                        Console.WriteLine("Kiralık Araçlar:");
                        foreach (var car in rentalService.GetAvailableCars())
                        {
                            Console.WriteLine($"{car.PlateNumber} - {car.Make} {car.Model} ({car.Year})");
                        }
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        break;
                }
            }
        }

        // Diğer sınıflar burada olduğu gibi devam edecek
        // ...

        private static string GetPasswordFromConsole()
        {
            string password = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            return password;
        }
    }

    class Car
    {
        public string PlateNumber { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }
        public int Year { get; private set; }
        public bool IsRented { get; set; }

        public Car(string plateNumber, string make, string model, int year)
        {
            PlateNumber = plateNumber;
            Make = make;
            Model = model;
            Year = year;
            IsRented = false;
        }
    }

    class RentalService
    {
        private List<Car> cars;
        private List<(string, string, DateTime)> reservations;

        public RentalService()
        {
            cars = new List<Car>();
            reservations = new List<(string, string, DateTime)>();
        }

        public void AddCar(string plateNumber, string make, string model, int year)
        {
            cars.Add(new Car(plateNumber, make, model, year));
        }

        public void RentCar(string customerName, string plateNumber, DateTime rentalDate)
        {
            Car car = cars.Find(c => c.PlateNumber == plateNumber && !c.IsRented);
            if (car != null)
            {
                car.IsRented = true;
                reservations.Add((customerName, plateNumber, rentalDate));
                Console.WriteLine($"Araç kiralama işlemi başarılı: {car.Make} {car.Model} - {rentalDate}");
            }
            else
            {
                Console.WriteLine("Araç kiralanamadı!");
            }
        }

        public List<Car> GetAvailableCars()
        {
            return cars.FindAll(c => !c.IsRented);
        }
    }
}