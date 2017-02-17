using System.IO;
using Nancy;

namespace GameSchedule.Helpers
{
    public class RootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
