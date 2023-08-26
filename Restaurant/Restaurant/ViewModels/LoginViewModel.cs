using Restaurant.Helpers;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Restaurant.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Views;

namespace Restaurant.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private MainNavigationVM _mainNavigationVM;
        private readonly UnitOfWork _unitOfWork;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand CancelCommand { get; }

        public LoginViewModel()
        {
            _unitOfWork = ServiceLocator.ServiceProvider.GetService<UnitOfWork>();
            LoginCommand = new RelayCommand(Login, CanLogin);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private void Login(object parameter)
        {
            var users = _unitOfWork.Users.GetAll();
            var user = users.FirstOrDefault(u => u.UserName == Username && u.Password == Password);

            if (user != null)
            {
                var employee = _unitOfWork.Employees.GetById(user.EmployeeId);
                if (employee != null)
                {
                    if (employee != null)
                    {
                        if (employee.EmployeeRole == EmployeeRole.Admin)
                        {
                            // Navigate to Admin dashboard
                            var mainNavVM = ServiceLocator.ServiceProvider.GetService<MainNavigationVM>();
                            mainNavVM.CurrentView = new AdminNavigationVM();
                        }
                        else if (employee.EmployeeRole == EmployeeRole.Waiter)
                        {
                            // Navigate to Waiter dashboard
                            var mainNavVM = ServiceLocator.ServiceProvider.GetService<MainNavigationVM>();
                            WaiterDashboardVM waiterDashboardVm = new WaiterDashboardVM(employee.Id);
                            WaiterDashboard view = new WaiterDashboard();
                            view.DataContext = waiterDashboardVm;
                            mainNavVM.CurrentView = waiterDashboardVm;
                        }
                    }
                }
            }
            else
            {
                // Login failed
            }
        }

        private void Cancel(object parameter)
        {
            // Cancel logic goes here
        }
    }
}
