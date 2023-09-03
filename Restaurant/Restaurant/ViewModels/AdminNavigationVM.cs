using Microsoft.Extensions.DependencyInjection;
using Restaurant.Helpers;
using Restaurant.Models.Entities;
using Restaurant.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    class AdminNavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand ManageUsersCommand { get; set; }
        public ICommand ManageTablesCommand { get; set; }
        public ICommand GenerateReportsCommand { get; set; }
        public ICommand ManageProductsCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        private void ManageUsers(object obj) => CurrentView = new AdminManageUsersVM();
        private void ManageTables(object obj) => CurrentView = new AdminManageTablesVM();
        private void ManageProducts(object obj)=> CurrentView = new AdminManageProductsVM();
        private void GenerateReports(object obj) => CurrentView = new AdminGenerateReportsVM();

        private void Logout(object obj)
        {
            var mainNavVM = ServiceLocator.ServiceProvider.GetService<MainNavigationVM>();
            var loginVM = new LoginViewModel();
            var loginView = new Login();
            loginView.DataContext = loginVM;
            mainNavVM.CurrentView = loginVM;
        }
        public AdminNavigationVM()
        {
            ManageUsersCommand = new RelayCommand(ManageUsers);
            ManageTablesCommand = new RelayCommand(ManageTables);
            GenerateReportsCommand = new RelayCommand(GenerateReports);
            ManageProductsCommand = new RelayCommand(ManageProducts);
            LogoutCommand = new RelayCommand(Logout);

            // Startup Page
            CurrentView = new AdminManageUsersVM();
        }
    }
}
