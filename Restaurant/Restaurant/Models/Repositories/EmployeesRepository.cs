using Restaurant.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.Repositories
{
    public class EmployeesRepository : RepositoryBase<Employee>
    {
        private readonly AppDbContext dbContext;
        public EmployeesRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Employee> GetAllWaiters()
        {
            var employees = GetAll();
            return employees.Where(e => e.EmployeeRole == Enums.EmployeeRole.Waiter).ToList();
        }
    }
}
