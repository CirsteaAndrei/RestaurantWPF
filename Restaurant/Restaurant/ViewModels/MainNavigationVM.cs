using Microsoft.Extensions.DependencyInjection;
using Restaurant.Helpers;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public class MainNavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; set; }




        private void Login(object obj) => CurrentView = new LoginViewModel();


        public MainNavigationVM()
        {
            LoginCommand = new RelayCommand(Login);

            CurrentView = new LoginViewModel();
        }
    }
}
