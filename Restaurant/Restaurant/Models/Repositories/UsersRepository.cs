using Restaurant.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.Repositories
{
    public class UsersRepository : RepositoryBase<User>
    {
        private readonly AppDbContext dbContext;
        public UsersRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<User> GetAllWithEmployeeData()
        {
            var users = GetAll();
            foreach (var user in users)
            {
                user.Employee = dbContext.Employees.FirstOrDefault(e => e.Id == user.EmployeeId);
            }
            return users;
        }
    }
}
