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

        public List<Table> GetTablesForEmployee(int employeeId)
        {
            var tables = GetAll();
            var resultTables = tables.Where(t=> t.EmployeeId==employeeId).ToList();
            foreach (var table in resultTables)
            {
                table.Employee = dbContext.Employees.FirstOrDefault(e => e.Id == table.EmployeeId);
                table.Order = dbContext.Orders.FirstOrDefault(o => o.Id == table.OrderId && o.OrderStatusEnum == OrderStatusEnum.Unpaid);
            }
            return resultTables;
        }

        public Table GetByOrderId(int orderId)
        {
            return dbContext.Tables.FirstOrDefault(t => t.OrderId == orderId);
        }
    }
}
