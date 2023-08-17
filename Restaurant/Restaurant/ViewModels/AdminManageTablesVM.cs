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
using System.Windows.Input;
using System.Windows.Media.Animation;
using Table = Restaurant.Models.Entities.Table;

namespace Restaurant.ViewModels
{
    class AdminManageTablesVM : ViewModelBase
    {
        private UnitOfWork UnitOfWork;
        private Table? currentSelectedTable;

        public AdminManageTablesVM()
        {
            UnitOfWork = ServiceLocator.ServiceProvider.GetService<UnitOfWork>();
            tablesList = UnitOfWork.Tables.ListToObsCollection(UnitOfWork.Tables.GetAllWithEmployeeData());
            employeesList = UnitOfWork.Employees.ListToObsCollection(UnitOfWork.Employees.GetAllWaiters());
            AddTableCommand = new RelayCommand(AddTable);
            AssignEmployeeCommand = new RelayCommand(AssignEmployee);
            DeleteTableCommand = new RelayCommand(DeleteTable);
            ConfirmAddTableCommand = new RelayCommand(ConfirmAddTable);
            ConfirmAssignEmployeeCommand = new RelayCommand(ConfirmAssignEmployee);
        }

        public ObservableCollection<Table> tablesList { get; set; }
        public ObservableCollection<Table> TablesList
        {
            get
            {
                return tablesList;
            }
            set { tablesList = value; }
        }

        public ObservableCollection<Employee> employeesList { get; set; }
        public ObservableCollection<Employee> WaitersList
        {
            get
            {
                return employeesList;
            }
            set { employeesList = value; }
        }

        private int _newTableSeatsTotal;
        public int NewTableSeatsTotal
        {
            get { return _newTableSeatsTotal; }
            set
            {
                _newTableSeatsTotal = value;
                NotifyPropertyChanged("NewTableSeatsTotal");
            }
        }

        private bool _isAddTablePopupOpen;
        public bool IsAddTablePopupOpen
        {
            get { return _isAddTablePopupOpen; }
            set
            {
                _isAddTablePopupOpen = value;
                NotifyPropertyChanged("IsAddTablePopupOpen");
            }
        }

        private Employee _selectedWaiter;
        public Employee SelectedWaiter
        {
            get { return _selectedWaiter; }
            set
            {
                _selectedWaiter = value;
                NotifyPropertyChanged("SelectedWaiter");
            }
        }

        private bool _isAssignEmployeePopupOpen;
        public bool IsAssignEmployeePopupOpen
        {
            get { return _isAssignEmployeePopupOpen; }
            set
            {
                _isAssignEmployeePopupOpen = value;
                NotifyPropertyChanged("IsAssignEmployeePopupOpen");
            }
        }


        public ICommand AddTableCommand { get; private set; }
        public ICommand AssignEmployeeCommand { get; private set; }
        public ICommand DeleteTableCommand { get; private set; }
        public ICommand ConfirmAddTableCommand { get; private set; }
        public ICommand ConfirmAssignEmployeeCommand { get; private set; }

        private void AddTable(object obj)
        {
            // Reset the seats total to a default value
            NewTableSeatsTotal = 0;

            // Open the popup
            IsAddTablePopupOpen = true;
        }

        private void AssignEmployee(object obj)
        {
            currentSelectedTable = obj as Table;
            if (currentSelectedTable != null)
            {
                // Open the popup
                IsAssignEmployeePopupOpen = true;
            }
        }

        private void DeleteTable(object obje)
        {
            Table table = obje as Table;
            UnitOfWork.Tables.Remove(table);
            UnitOfWork.SaveChanges();
            TablesList.Remove(table);
        }

        private void ConfirmAddTable(object obj)
        {
            var newTable = new Table { SeatsTotal = NewTableSeatsTotal, SeatsTaken = 0 };
            UnitOfWork.Tables.Insert(newTable);
            UnitOfWork.SaveChanges();
            TablesList.Add(newTable);

            // Close the popup after confirming
            IsAddTablePopupOpen = false;
        }

        private void ConfirmAssignEmployee(object obj)
        {
            if (currentSelectedTable != null)
            {
                currentSelectedTable.Employee = SelectedWaiter;
                currentSelectedTable.EmployeeId = SelectedWaiter.Id;
                UnitOfWork.Tables.Update(currentSelectedTable);
                UnitOfWork.SaveChanges();

                int index = TablesList.IndexOf(currentSelectedTable);
                if (index >= 0)
                {
                    TablesList[index] = currentSelectedTable;
                }
                // Reset the reference to ensure no unintended operations on it later
                currentSelectedTable = null;
            }

            // Close the popup after confirming
            IsAssignEmployeePopupOpen = false;
        }
    }
}
