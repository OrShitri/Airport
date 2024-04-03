using AirportSimulator.Models;
using System.Net.Http.Json;
using T = System.Timers;

namespace AirportSimulator
{
    internal class Program
    {
        static readonly Random rndTime = new Random();
        static readonly Random rndPaggengersCount = new Random();
        static readonly Random rndBrand = new Random();
        static readonly Random rndCharFlightNumber = new Random();
        static readonly Random rndNumFlightNumber = new Random();

        static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:7253") };
        static readonly T.Timer timer = new T.Timer(5000);
        static readonly Dictionary<int, string> dicBrand = new Dictionary<int, string>
        {
            { 1, "Air Lingus" } ,
            { 2, "Singapore Airlines" },
            { 3, "Lufthansa" },
            { 4, "Air France" },
            { 5, "Delta" },
            { 6, "Iberia" },
            { 7, "El Al" },
            { 8, "Emirates" },
            { 9, "Swiss Air" },
            { 10, "Turkish Airlines" },
            { 11, "Aeroflot" },
            { 12, "British Airways" },
            { 13, "Cubana" },
            { 14, "Cathay Pacific" },
            { 15, "Air Canada" }
        };
        static void Main(string[] args)
        {
            timer.Elapsed += (s, e) => Timer_Elapsed();
            timer.Start();
            Console.ReadLine();
        }

        private static async void Timer_Elapsed()
        {
            timer.Interval = rndTime.Next(4000, 15000); //optional (5000,15000)

            string flightNumber = ((char)rndCharFlightNumber.Next(65, 91)).ToString() + ((char)rndCharFlightNumber.Next(65, 91)).ToString() + rndNumFlightNumber.Next(100, 1000).ToString();
            FlightDto flightDto = new FlightDto
            {
                FlightNumber = flightNumber,
                FlightStatus = "Arrival",
                Brand = dicBrand[rndBrand.Next(1, 16)],
                PaggengersCount = (short)rndPaggengersCount.Next(30, 301)
            };

            await client.PostAsJsonAsync("api/Flights", flightDto);
            await Console.Out.WriteLineAsync($"Flight Number: {flightDto.FlightNumber}, Flight Status: {flightDto.FlightStatus}, Brand: {flightDto.Brand}, Passengers Count: {flightDto.PaggengersCount}, Date: {DateTime.Now}");
        }
    }
}