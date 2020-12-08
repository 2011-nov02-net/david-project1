using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Library.Repository_Interfaces;
using Store.Library;
using Store.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Store.Library.Repository_Interfaces;
using Store.WebApp.ViewModels;

namespace Store.Test
{
    public class OrderControllerTest
    {
        [Fact]
        public void Order_Details_ReturnsAView()
        {
            // Arrange
            int id = 1;
            var mockOrderRepo = new Mock<IOrderRepository>();
            mockOrderRepo.Setup(repo => repo.GetOrderByOrderNumber(id))
                .Returns(OneOrderWithSale());
            var controller = new OrdersController(mockOrderRepo.Object, null, null);

            // Act
            var result = controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<OrderViewModel>(viewResult.ViewData.Model);
            Assert.Equal(1, model.CustomerId);
            Assert.Equal(1, model.LocationId);
            Assert.Equal(1, model.OrderNumber);
            Assert.Equal(40.00m, model.OrderTotal);

            var sales = Assert.IsAssignableFrom<IEnumerable<SaleViewModel>>(model.Sales);
            Assert.Equal(2, sales.Count());
        }
        private static Order OneOrderWithSale()
        {
            var sales = new List<Sale>
            {
                new Sale(1, "test", 20.00m, 1),
                new Sale(1, "test", 20.00m, 1)
            };

            return new Order(1, 1, sales, DateTime.Now, 1, 40.00m);
        }
        private static IEnumerable<Order> ListOfTwoOrders()
        {
            var sales = new List<Sale>
            {
                new Sale(1, "test", 20.00m, 1),
                new Sale(1, "test", 20.00m, 1)
            };
            var orders =  new List<Order>
            {
                new Order(1, 1, sales, DateTime.Now, 1, 20.00m),
                new Order(1, 1, sales, DateTime.Now, 2, 20.00m)
            };

            return orders;
        }
    }
}
