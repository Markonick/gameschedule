using GameSchedule.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Nancy.Owin;

namespace GameSchedule
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseOwin(buildFunc =>
            {
                buildFunc.UseNancy(opt => opt.Bootstrapper = new Bootstrapper(env));
            });
        }
    }
}
