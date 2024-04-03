using AirportApiServer.Models.Entities;

namespace AirportApiServer.Services
{
    public interface IDataService
    {
        Task AddFlight(Flight data);

        Task<IEnumerable<Log>> GetLogs();

        Task AddLog(Log data);
    }
}
