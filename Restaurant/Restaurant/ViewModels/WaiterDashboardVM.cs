using Microsoft.Extensions.DependencyInjection;
using Restaurant.Helpers;
using Restaurant.Models;
using Restaurant.Models.Entities;
using Restaurant.Models.Enums;
using Restaurant.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Table = Restaurant.Models.Entities.Table;

namespace Restaurant.ViewModels
{
    class WaiterDashboardVM : ViewModelBase
    {
        private readonly UnitOfWork unitOfWork;

        public int EmployeeId { get; private set; }

        private ObservableCollection<Table> assignedTables;
        public ObservableCollection<Table> AssignedTables
        {
            get { return assignedTables; }
            set { assignedTables = value; OnPropertyChanged(); }
        }

        private int _seatsTaken;
        public int SeatsTaken
        {
            get { return _seatsTaken; }
            set
            {
                _seatsTaken = value;
                NotifyPropertyChanged("SeatsTaken");
            }
        }

        private bool _isSetSeatsTakenPopupOpen;
        public bool IsSetSeatsTakenPopupOpen
        {
            get { return _isSetSeatsTakenPopupOpen; }
            set
            {
                _isSetSeatsTakenPopupOpen = value;
                NotifyPropertyChanged("IsSetSeatsTakenPopupOpen");
            }
        }

        public ICommand SetSeatsTakenCommand { get; private set; }
        public ICommand ConfirmSetSeatsTakenCommand { get; private set; }

        
        // Add other commands if needed...

        public WaiterDashboardVM(int employeeId)
        {
            EmployeeId = employeeId;
            unitOfWork = ServiceLocator.ServiceProvider.GetService<UnitOfWork>();
            AssignedTables = unitOfWork.Tables.ListToObsCollection(unitOfWork.Tables.GetTablesForEmployee(EmployeeId));

            SetSeatsTakenCommand = new RelayCommand(SetSeatsTaken);
            ConfirmSetSeatsTakenCommand = new RelayCommand(ConfirmSetSeatsTaken);
            FreeTableCommand = new RelayCommand(FreeTable);
            ViewOrderCommand = new RelayCommand(ViewOrder);
            LogoutCommand = new RelayCommand(Logout);
            
            // Initialize other commands...
        }

        public WaiterDashboardVM() { }

        private void SetSeatsTaken(object obj)
        {
            // Reset the seats taken to a default
            // 
            var table = obj as Table;
            if(table != null)
                if(!table.OrderId.HasValue)
                    IsSetSeatsTakenPopupOpen = true;
        }

        private void ConfirmSetSeatsTaken(object obj)
        {
            // Assuming you have access to the selected table
            var selectedTable = obj as Table;
            if (selectedTable != null)
            {
                // Update the seats taken for the table
                selectedTable.SeatsTaken = SeatsTaken;
                // Create a new order
                var newOrder = new Order
                {
                    OrderStatusEnum = OrderStatusEnum.Unpaid,
                    TotalPrice = null, // or set an initial value if needed
                    EmployeeId = selectedTable.Employee.Id,
                };

                // Add the new order to the repository and save changes
                unitOfWork.Orders.Insert(newOrder);
                unitOfWork.SaveChanges();

                // Associate the order with the table
                selectedTable.OrderId = newOrder.Id;

                // Update the table in the repository and save changes
                unitOfWork.Tables.Update(selectedTable);
                unitOfWork.SaveChanges();
            }

            // Close the popup after confirming
            IsSetSeatsTakenPopupOpen = false;
        }

        public ICommand FreeTableCommand { get; set; }

        private void FreeTable(object obj)
        {
            if (obj is Table table)
            {
                if (table.SeatsTaken > 0 && table.OrderId.HasValue)
                {
                    // Show a popup with options: "Cancel Order", "Order Paid", and "Cancel".
                    var result = MessageBox.Show("The table has an Order.What would you like to do?\n\nYes = Cancel Order\nNo = Order Paid\nCancel = Do Nothing",
                             "Table Occupied",
                                                 MessageBoxButton.YesNoCancel,
                                                 MessageBoxImage.Question,
                                                 MessageBoxResult.Cancel,
                                                 MessageBoxOptions.DefaultDesktopOnly);

                    switch (result)
                    {
                        case MessageBoxResult.Yes: // Assuming this corresponds to "Cancel Order"
                            table.Order.OrderStatusEnum = OrderStatusEnum.Cancelled;
                            table.OrderId = null;
                            table.SeatsTaken = 0;
                            break;
                        case MessageBoxResult.No: // Assuming this corresponds to "Order Paid"
                            table.Order.OrderStatusEnum = OrderStatusEnum.Paid;
                            table.OrderId = null;
                            table.SeatsTaken = 0;
                            break;
                        case MessageBoxResult.Cancel:
                            // Do nothing.
                            break;
                    }
                    unitOfWork.SaveChanges();
                }
                else if (table.SeatsTaken > 0)
                {
                    // If the table is taken but there's no order, just free the table.
                    table.SeatsTaken = 0;
                    unitOfWork.SaveChanges();
                }
            }
        }

        public ICommand ViewOrderCommand { get; set; }
        private void ViewOrder(object obj)
        {
            Table table = obj as Table;
            if (table.OrderId.HasValue)
            {
                var mainNavVM = ServiceLocator.ServiceProvider.GetService<MainNavigationVM>();
                WaiterManageOrdersVM waiterManageOrdersVM = new WaiterManageOrdersVM(table.OrderId.Value);
                WaiterManageOrdersView view = new WaiterManageOrdersView();
                view.DataContext = waiterManageOrdersVM;
                mainNavVM.CurrentView = waiterManageOrdersVM;
            }
        }

        public ICommand LogoutCommand { get; }
        private void Logout(object obj)
        {
            var mainNavVM = ServiceLocator.ServiceProvider.GetService<MainNavigationVM>();
            var loginVM = new LoginViewModel();
            var loginView = new Login();
            loginView.DataContext = loginVM;
            mainNavVM.CurrentView = loginVM;
        }



    }


}

