using AirportApiServer.Data.DbContexts;
using AirportApiServer.Models.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AirportApiServer.Services
{
    public class DataService : IDataService
    {
        private readonly AirportDbContext _myContext;
        private readonly List<int> _nextLegs = new List<int>();
        private readonly IServiceScopeFactory _scopeService;
        private static readonly Random _rnd = new Random();

        public DataService(IServiceScopeFactory scopeService)
        {
            _myContext = scopeService.CreateScope().ServiceProvider.GetRequiredService<AirportDbContext>();

            _nextLegs = _myContext.NextLegs.Select(x => x.Next).ToList();
            this._scopeService = scopeService;
        }
        Dispatcher? dispatcher;

        #region Data Service
        public async Task AddFlight(Flight flight)
        {
            await _myContext.Flights.AddAsync(flight);
            await _myContext.SaveChangesAsync();
            StartMove(flight);
        }

        public async Task<IEnumerable<Log>> GetLogs()
        {
            using (var s = _scopeService.CreateScope().ServiceProvider.GetRequiredService<AirportDbContext>())
            {
                var logs = await s.Logs
                    .Include(X => X.Flight)
                    .Where(l => (l.Flight.CurrentLegId != 9 && l.Flight.CurrentLegId != null) || l.Out > DateTime.Now.AddSeconds(-3))
                    .OrderByDescending(log => log.InLeg)
                    .Take(32)
                    .Select(l => new Log
                    {
                        Id = l.Id,
                        FlightId = l.FlightId,
                        LegId = l.LegId,
                        FlightStatus = l.FlightStatus,
                        PassengersCount = l.PassengersCount,
                        InLeg = l.InLeg,
                        Out = l.Out,
                        Flight = new Flight
                        {
                            Id = l.Flight.Id,
                            FlightNumber = l.Flight.FlightNumber,
                            Brand = l.Flight.Brand
                        }
                    })
                    .ToListAsync();

                return logs;
            }
        }

        public async Task AddLog(Log log)
        {
            await _myContext.Logs.AddAsync(log);
            await _myContext.SaveChangesAsync();
        }
        #endregion

        #region Logic - Private Methods
        private async void StartMove(Flight flight)
        {
            int countFlights = _myContext.Flights.Where(f => f.CurrentLegId >= 1 && f.CurrentLegId < 9).Count();

            if (countFlights < 4 && await TerminalIsFree(1))
                GoNextLeg(flight, new List<int> { 1 });
        }

        private async Task<bool> TerminalIsFree(int nextLeg)
        {
            return await _myContext.Flights.AllAsync(f => f.CurrentLegId != nextLeg);
        }

        private async void GoNextLeg(Flight flight, List<int> nextLegs)
        {
            flight.LegTimer.Change(Timeout.Infinite, Timeout.Infinite);
            if (nextLegs.Count > 1)
            {
                int nextLexOne = int.Parse(nextLegs[0].ToString()!);
                int nextLexTwo = int.Parse(nextLegs[1].ToString()!);

                if (flight.CurrentLeg!.LegNumber == 4 && flight.GetOut) { UpdateFlightDetails(flight, nextLexTwo); }

                else if (await TerminalIsFree(nextLexOne))
                {
                    flight.CallMyCallback -= ContinueGoNext;
                    UpdateFlightDetails(flight, nextLexOne);
                }
                else if (await TerminalIsFree(nextLexTwo) && flight.CurrentLeg!.LegNumber != 4)
                {
                    flight.CallMyCallback -= ContinueGoNext;
                    UpdateFlightDetails(flight, nextLexTwo);
                }
                else
                {
                    StayInLeg(flight);
                    flight.LegTimer.Change(0, flight.CurrentLeg.CrossingTime);

                }
            }
            else
            {
                if (await TerminalIsFree(int.Parse(nextLegs[0].ToString()!)))
                {
                    flight.CallMyCallback -= ContinueGoNext;
                    UpdateFlightDetails(flight, int.Parse(nextLegs[0].ToString()!));
                }
                else
                {
                    StayInLeg(flight);
                    flight.LegTimer.Change(0, flight.CurrentLeg!.CrossingTime);
                }
            }
        }

        private async void UpdateFlightDetails(Flight flight, int nextLeg)
        {
            var log = await GetLogByFligthId(flight.Id);
            if (log != null)
            {
                log.Out = DateTime.Now;
                await _myContext.SaveChangesAsync();//?? maybe comment
            }

            var currentLeg = await _myContext.Legs.SingleAsync(x => x.LegNumber == nextLeg);
            flight.CurrentLeg = currentLeg;

            if (flight.CurrentLeg != null && nextLeg == 9)
            {
                flight.LegTimer.Change(Timeout.Infinite, Timeout.Infinite);
                await _myContext.SaveChangesAsync();
                return;
            }

            if (flight.CurrentLeg!.LegNumber == 4)
                flight.FlightStatus = (flight.FlightStatus == "Arrival") ? "Departure" : "Arrival";

            if (flight.CurrentLeg.LegNumber == 8)
                flight.GetOut = true;

            if (flight.CurrentLeg.LegNumber == 6 || flight.CurrentLeg.LegNumber == 7)
                flight.PaggengersCount = (short)((_rnd.Next(0, 11)) * 30);

            var passengersCount = flight.PaggengersCount != null ? flight.PaggengersCount.Value : 0;
            await AddLog(new Log
            {
                Flight = flight,
                Leg = flight.CurrentLeg,
                FlightStatus = flight.FlightStatus,
                PassengersCount = (short)passengersCount,
                InLeg = DateTime.Now
            });

            WaitingInLeg(flight);
        }

        private async Task<Log> GetLogByFligthId(int id)
        {
            var lastUpdatedLogForFlightId = await _myContext.Logs
                .Where(log => log.Flight.Id == id)
                .OrderByDescending(log => log.InLeg)
                .FirstOrDefaultAsync();
            return lastUpdatedLogForFlightId!;
        }

        private void WaitingInLeg(Flight flight)
        {
            flight.CallMyCallback -= ContinueGoNext;
            flight.CallMyCallback += ContinueGoNext;
        }

        private async void ContinueGoNext(Flight flight)
        {
            var nextLegs = await OptionalLegs(flight.CurrentLeg!.LegNumber);
            GoNextLeg(flight, nextLegs);
        }

        private async Task<List<int>> OptionalLegs(int currentLeg)
        {
            var nextLegs = await _myContext.NextLegs.Where(x => x.Leg == currentLeg).ToListAsync();

            var optionalLegs = nextLegs.Select(x => x.Next).ToList();
            return optionalLegs;
        }

        private void StayInLeg(Flight flight)
        {
            Console.WriteLine("waiting...");
            Thread.Sleep(1000);
            WaitingInLeg(flight);
            return;
        }
        #endregion

    }
}
