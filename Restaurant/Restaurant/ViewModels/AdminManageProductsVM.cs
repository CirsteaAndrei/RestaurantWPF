using Microsoft.Extensions.DependencyInjection;
using Restaurant.Helpers;
using Restaurant.Models.Entities;
using Restaurant.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    class AdminManageProductsVM : ViewModelBase
    {
        private UnitOfWork unitOfWork;
        private ObservableCollection<Product> productsList;

        public AdminManageProductsVM()
        {
            unitOfWork = ServiceLocator.ServiceProvider.GetService<UnitOfWork>();
            productsList = new ObservableCollection<Product>(unitOfWork.Products.GetAll());
            AddProductCommand = new RelayCommand(AddProduct);
            UpdateProductCommand = new RelayCommand(UpdateProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
        }

        public ObservableCollection<Product> ProductsList
        {
            get { return productsList; }
            set { productsList = value; }
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

        public ICommand AddProductCommand { get; private set; }
        public ICommand UpdateProductCommand { get; private set; }
        public ICommand DeleteProductCommand { get; private set; }
        public void AddProduct(object obj)
        {
            Product product = obj as Product;
            if (string.IsNullOrEmpty(product.Name))
            {
                ErrorMessage = "The name of the product must be given";
            }
            else
            {
                unitOfWork.Products.Insert(product);
                unitOfWork.SaveChanges();
                productsList.Add(product);
                ErrorMessage = "";
            }
        }

        public void UpdateProduct(object obj)
        {
            Product product = obj as Product;
            if (product == null)
            {
                ErrorMessage = "You must select a product";
            }
            else if (string.IsNullOrEmpty(product.Name))
            {
                ErrorMessage = "The name of the product must be given";
            }
            else
            {
                unitOfWork.Products.Update(product);
                unitOfWork.SaveChanges();
                ErrorMessage = "";
            }
        }

        public void DeleteProduct(object obj)
        {
            Product product = obj as Product;
            if (product == null)
            {
                ErrorMessage = "You must select a product";
            }
            else
            {
                Product p = unitOfWork.Products.GetById(product.Id);
                unitOfWork.Products.Remove(p);
                unitOfWork.SaveChanges();
                productsList.Remove(product);
                ErrorMessage = "";
            }
        }
    }
}