using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Helpers;
using Restaurant.Models;
using Restaurant.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    class AdminManageTablesVM : ViewModelBase
    {
        private UnitOfWork UnitOfWork;
        public ObservableCollection<Restaurant.Models.Entities.Table> tablesList { get; set; }
        public ObservableCollection<Employee> employeesList { get; set; }

        public AdminManageTablesVM() { 
            UnitOfWork = ServiceLocator.ServiceProvider.GetService<UnitOfWork>();
            tablesList = UnitOfWork.Tables.ListToObsCollection(UnitOfWork.Tables.GetAllWithEmployeeData());
            employeesList = UnitOfWork.Employees.ListToObsCollection(UnitOfWork.Employees.GetAllWaiters());
        }

        public ObservableCollection<Restaurant.Models.Entities.Table> TablesList {
            get
            {
                return tablesList;
            }
            set { tablesList = value; }
        }

        public ObservableCollection<Employee> EmployeesList
        {
            get
            {
                return employeesList;
            }
            set { employeesList = value; }
        }


    }
}
