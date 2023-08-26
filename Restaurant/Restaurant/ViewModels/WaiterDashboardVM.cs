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
    public class WaiterDashboardVM : ViewModelBase
    {
        private readonly UnitOfWork unitOfWork;
        public int EmployeeId { get; private set; }

        public WaiterDashboardVM(int employeeId)
        {
            EmployeeId = employeeId;
            unitOfWork = ServiceLocator.ServiceProvider.GetService<UnitOfWork>();
            AssignedTables = unitOfWork.Tables.ListToObsCollection(unitOfWork.Tables.GetTablesForEmployee(EmployeeId));
        }

        private ObservableCollection<Table> assignedTables;
        public ObservableCollection<Table> AssignedTables
        {
            get { return assignedTables; }
            set { assignedTables = value; OnPropertyChanged();}
        }
        public WaiterDashboardVM() { }
    }
}
