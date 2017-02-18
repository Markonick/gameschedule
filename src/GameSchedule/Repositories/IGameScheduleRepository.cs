using System.Threading.Tasks;
using GameSchedule.Models;
using MongoDB.Driver;

namespace GameSchedule.Repositories
{
    public interface IGameScheduleRepository
    {
        void StoreGameSchedule(dynamic response);
        Task<dynamic> GetAllGamesAsync();
        Task<dynamic> GetTodaysGamesAsync();
    }
}