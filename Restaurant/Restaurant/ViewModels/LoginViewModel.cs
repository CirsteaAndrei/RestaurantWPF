using Restaurant.Helpers;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Restaurant.Models.Enums;

namespace Restaurant.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
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

        public LoginViewModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                    if (employee.EmployeeRole == EmployeeRole.Admin)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
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
