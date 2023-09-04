using Restaurant.Models.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurant.ViewModels
{
    class AdminGenerateReportsVM
    {
        private UnitOfWork unitOfWork;
        private ObservableCollection<User> usersList;

        public AdminGenerateReportsVM() {

            unitOfWork = ServiceLocator.ServiceProvider.GetService<UnitOfWork>();
            usersList = unitOfWork.Users.ListToObsCollection(unitOfWork.Users.GetAllWithEmployeeData());
        }

        public ObservableCollection<User> UsersList
        {
            get
            { return usersList; }
            set { usersList = value; }
        }
    }
}
