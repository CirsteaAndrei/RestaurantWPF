using Restaurant.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class UnitOfWork
    {
        public EmployeesRepository Employees{ get; }
        public OrdersRepository Orders { get; }
        public OrderDetailsRepository OrderDetails { get; }
        public ProductsRepository Products { get; }
        public TablesRepository Tables { get; }
        public UsersRepository Users { get; }

        private readonly AppDbContext _dbContext;

        public UnitOfWork
        (
            AppDbContext dbContext,
            EmployeesRepository employeesRepository,
            OrdersRepository ordersRepository,
            OrderDetailsRepository orderDetailsRepository,
            ProductsRepository productsRepository,
            TablesRepository tablesRepository,
            UsersRepository usersRepository
        )
        {
            _dbContext = dbContext;
            Employees = employeesRepository;
            Orders = ordersRepository;  
            OrderDetails = orderDetailsRepository;
            Products = productsRepository;
            Tables = tablesRepository;
            Users = usersRepository;
        }

        public void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                var errorMessage = "Error when saving to the database: "
                    + $"{exception.Message}\n\n"
                    + $"{exception.InnerException}\n\n"
                    + $"{exception.StackTrace}\n\n";

                Console.WriteLine(errorMessage);
            }
        }
    }
}
