using Restaurant.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.Repositories
{
    public class OrderDetailsRepository : RepositoryBase<OrderDetail>
    {
        private readonly AppDbContext dbContext;
        public OrderDetailsRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<OrderDetail> GetByOrderIdWithProductData(int orderId)
        {
            var orderDetailsList = dbContext.OrderDetails.Where(od => od.OrderId == orderId).ToList();
            foreach (var orderDetails in orderDetailsList)
            {
                orderDetails.Product = dbContext.Products.FirstOrDefault(p => p.Id == orderDetails.ProductId);
            }
            return orderDetailsList;
        }
    }
}
