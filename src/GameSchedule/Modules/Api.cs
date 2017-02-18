using GameSchedule.Repositories;
using Nancy;

namespace GameSchedule.Modules
{
    public class Api : NancyModule
    {
        public Api(IGameScheduleRepository repo)
        {
            Get("api/fullgameschedule", async args =>
            {
                 return await repo.GetAllGamesAsync();
            });

            Get("api/todaysgameschedule", async args =>
            {
                return await repo.GetTodaysGamesAsync();
            });
        }
    }
}
