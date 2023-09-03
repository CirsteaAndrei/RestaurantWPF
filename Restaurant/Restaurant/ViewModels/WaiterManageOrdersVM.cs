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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public class WaiterManageOrdersVM : ViewModelBase
    {
        private readonly UnitOfWork unitOfWork;
        private ObservableCollection<OrderDetail> _selectedProducts;
        private ObservableCollection<Product> _allProducts;
        private float _totalPrice;
        private int _orderId;

        public ObservableCollection<OrderDetail> SelectedProducts
        {
            get { return _selectedProducts; }
            set { _selectedProducts = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Product> AllProducts
        {
            get { return _allProducts; }
            set { _allProducts = value; OnPropertyChanged(); }
        }

        public float TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; OnPropertyChanged(); }
        }

        public ICommand SelectProductCommand { get; }
        public ICommand FinalizeOrderCommand { get; }
        public ICommand CancelOrderCommand { get; }
        public ICommand NavigateToDashboardCommand { get; private set; }

        public WaiterManageOrdersVM(int orderId)
        {
            // Initialize collections and commands
            // Load data based on the provided orderId
            unitOfWork = ServiceLocator.ServiceProvider.GetService<UnitOfWork>();
            _allProducts = new ObservableCollection<Product>(unitOfWork.Products.GetAll());
            _selectedProducts = new ObservableCollection<OrderDetail>(unitOfWork.OrderDetails.GetByOrderIdWithProductData(orderId));
            _orderId = orderId;
            SelectProductCommand = new RelayCommand(SelectProduct);
            TotalPrice = CalculateTotalPrice();
            NavigateToDashboardCommand = new RelayCommand(NavigateToDashboard);
            FinalizeOrderCommand = new RelayCommand(FinalizeOrder);
            CancelOrderCommand = new RelayCommand(CancelOrder);
        }

        public WaiterManageOrdersVM() { }

        private void SelectProduct(object obj)
        {
            var selectedProduct = obj as Product;
            if (selectedProduct != null && selectedProduct.isAvailable)
            {
                // Create a new OrderDetail
                var newOrderDetail = new OrderDetail
                {
                    ProductId = selectedProduct.Id,
                    OrderId = _orderId,
                    ProductName = selectedProduct.Name
                                       // Set other required properties of OrderDetail if needed
                };

                // Add the new OrderDetail to the repository and save changes
                unitOfWork.OrderDetails.Insert(newOrderDetail);
                unitOfWork.SaveChanges();

                // Update the SelectedProducts ObservableCollection
                SelectedProducts.Add(newOrderDetail);
                TotalPrice = CalculateTotalPrice();
                UpdateOrderTotalPrice();
            }
        }
        private float CalculateTotalPrice()
        {
            return SelectedProducts.Sum(od => od.Product.Price);
        }

        private void UpdateOrderTotalPrice()
        {
            var orderToUpdate = unitOfWork.Orders.GetById(_orderId);
            if (orderToUpdate != null)
            {
                orderToUpdate.TotalPrice = TotalPrice;
                unitOfWork.Orders.Update(orderToUpdate);
                unitOfWork.SaveChanges();
            }
        }

        private void NavigateToDashboard(object obj)
        {
            var currentOrder = unitOfWork.Orders.GetById(_orderId);
            var mainNavVM = ServiceLocator.ServiceProvider.GetService<MainNavigationVM>();
            var waiterDashboardVm = new WaiterDashboardVM(currentOrder.EmployeeId); // You might need to pass parameters if required by the constructor
            var view = new WaiterDashboard();
            view.DataContext = waiterDashboardVm;
            mainNavVM.CurrentView = waiterDashboardVm;
        }

        private void FinalizeOrder(object obj)
        {
            var result = MessageBox.Show("Was the order paid?", "Finalize Order", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                // Update the order status
                var currentOrder = unitOfWork.Orders.GetById(_orderId);
                currentOrder.OrderStatusEnum = OrderStatusEnum.Paid;
                unitOfWork.Orders.Update(currentOrder);

                // Find the table associated with the order and update its properties
                var table = unitOfWork.Tables.GetByOrderId(_orderId);
                if (table != null)
                {
                    table.SeatsTaken = 0;
                    table.OrderId = null;
                    unitOfWork.Tables.Update(table);
                }

                // Save changes to the database
                unitOfWork.SaveChanges();

                // Navigate back to the WaiterDashboard
                var mainNavVM = ServiceLocator.ServiceProvider.GetService<MainNavigationVM>();
                var waiterDashboardVm = new WaiterDashboardVM(currentOrder.EmployeeId);
                var view = new WaiterDashboard();
                view.DataContext = waiterDashboardVm;
                mainNavVM.CurrentView = waiterDashboardVm;
            }
        }
        private void CancelOrder(object obj)
        {
            var result = MessageBox.Show("Are you sure you want to cancel the order?", "Cancel Order", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                // Update the order status
                var currentOrder = unitOfWork.Orders.GetById(_orderId);
                currentOrder.OrderStatusEnum = OrderStatusEnum.Cancelled;
                unitOfWork.Orders.Update(currentOrder);

                // Find the table associated with the order and update its properties
                var table = unitOfWork.Tables.GetByOrderId(_orderId);
                if (table != null)
                {
                    table.SeatsTaken = 0;
                    table.OrderId = null;
                    unitOfWork.Tables.Update(table);
                }

                // Save changes to the database
                unitOfWork.SaveChanges();

                // Navigate back to the WaiterDashboard
                var mainNavVM = ServiceLocator.ServiceProvider.GetService<MainNavigationVM>();
                var waiterDashboardVm = new WaiterDashboardVM(currentOrder.EmployeeId); // Assuming you have the EmployeeId in the Order entity
                var view = new WaiterDashboard();
                view.DataContext = waiterDashboardVm;
                mainNavVM.CurrentView = waiterDashboardVm;
            }
        }
        // Implement command methods and other logic here
    }
}
