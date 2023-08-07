using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Helpers
{
    public static class ServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Initialize(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
