using Microsoft.Extensions.DependencyInjection;
using Restaurant.Helpers;
using Restaurant.Models.Repositories;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Restaurant.ViewModels;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            services.AddSingleton<AppDbContext>();
            services.AddTransient<EmployeesRepository>();
            services.AddTransient<OrdersRepository>();
            services.AddTransient<OrderDetailsRepository>();
            services.AddTransient<ProductsRepository>();
            services.AddTransient<TablesRepository>();
            services.AddTransient<UsersRepository>();
            services.AddTransient<UnitOfWork>();
            services.AddSingleton<MainNavigationVM>();

            ServiceLocator.Initialize(services);
        }
    }
}
