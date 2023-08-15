using Microsoft.Extensions.DependencyInjection;
using Restaurant.Helpers;
using Restaurant.Models;
using Restaurant.Models.Entities;
using Restaurant.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    class AdminManageUsersVM : ViewModelBase
    {
        private UnitOfWork unitOfWork;
        private ObservableCollection<User> usersList;
        public AdminManageUsersVM()
        {
            unitOfWork = ServiceLocator.ServiceProvider.GetService<UnitOfWork>();
            usersList = unitOfWork.Users.ListToObsCollection(unitOfWork.Users.GetAllWithEmployeeData());
        }

        

        public ObservableCollection<User> UsersList
        {
            get
            { return usersList; }
            set { usersList = value; }
        }
        public Array EmployeeRoles
        {
            get { return Enum.GetValues(typeof(EmployeeRole)); }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }

        public void AddUser(Object obj)
        {
            User user = obj as User;
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.Employee.FirstName) || string.IsNullOrEmpty(user.Employee.LastName))
                {
                    ErrorMessage = "The name of the employee must be given";
                }
                else if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                {
                    ErrorMessage = "The username and password must be given";
                }
                else
                {
                    unitOfWork.Employees.Insert(user.Employee);
                    unitOfWork.SaveChanges();
                    user.EmployeeId = unitOfWork.Employees.GetAll().FirstOrDefault(employee => employee.FirstName == user.Employee.FirstName && employee.LastName == user.Employee.LastName).Id;
                    unitOfWork.Users.Insert(user);
                    unitOfWork.SaveChanges();
                    usersList.Add(user);
                    ErrorMessage = "";
                }
            }
        }
        private ICommand addUserCommand;
        public ICommand AddUserCommand
        {
            get
            {
                if (addUserCommand == null)
                    addUserCommand = new RelayCommand(AddUser);
                return addUserCommand;
            }
        }

        public void UpdateUser(Object obj)
        {
            User user = obj as User;
            if (user == null)
            {
                ErrorMessage = "You must select a user";
            }
            else if (string.IsNullOrEmpty(user.Employee.FirstName) || string.IsNullOrEmpty(user.Employee.LastName))
            {
                ErrorMessage = "The name of the employee must be given";
            }
            else if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                ErrorMessage = "The username and password must be given";
            }
            else
            {
                unitOfWork.Employees.Update(user.Employee);

                unitOfWork.Users.Update(user);
                unitOfWork.SaveChanges();
                ErrorMessage = "";
            }
        }

        private ICommand updateUserCommand;
        public ICommand UpdateUserCommand
        {
            get
            {
                if (updateUserCommand == null)
                    updateUserCommand = new RelayCommand(UpdateUser);
                return updateUserCommand;
            }
        }

        public void DeleteUser(Object obj)
        {
            User user = obj as User;
            if (user == null)
            {
                ErrorMessage = "You must select a user";
            }
            else
            {
                User u = unitOfWork.Users.GetById(user.Id);
                Employee e = unitOfWork.Employees.GetById(user.EmployeeId);
                unitOfWork.Employees.Remove(e);
                unitOfWork.Users.Remove(u);
                unitOfWork.SaveChanges();
                usersList.Remove(user);
                ErrorMessage = "";
            }
        }

        private ICommand deleteUserCommand;
        public ICommand DeleteUserCommand
        {
            get
            {
                if (deleteUserCommand == null)
                    deleteUserCommand = new RelayCommand(DeleteUser);
                return deleteUserCommand;
            }
        }
    }
}
