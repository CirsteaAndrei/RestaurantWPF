using Restaurant.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Models.Enums;

namespace Restaurant.Models.Repositories
{
    public class TablesRepository : RepositoryBase<Table>
    {
        private readonly AppDbContext dbContext;
        public TablesRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Table> GetAllWithEmployeeData()
        {
            var tables = GetAll();
            foreach (var table in tables)
            {
                table.Employee = dbContext.Employees.FirstOrDefault(e => e.Id == table.EmployeeId);
            }
            return tables;
        }
    }
}
