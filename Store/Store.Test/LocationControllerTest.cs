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
    public class LocationControllerTest
    {
        [Fact]
        public void Index_ReturnsAView_WithAListOfLocations()
        {
            // Arrange
            var mockRepo = new Mock<ILocationRepository>();
            mockRepo.Setup(repo => repo.GetAll())
                .Returns(ListOfTwoLocations());
            var controller = new LocationsController(mockRepo.Object, null, null);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Location>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Details_ReturnsAView_WithInventoryAndOrders()
        {
            // Arrange
            int id = 1;
            var mockLocationRepo = new Mock<ILocationRepository>();
            mockLocationRepo.Setup(repo => repo.GetWithInventory(id))
                .Returns(LocationWithInventory());
            var mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderRepository.Setup(repo => repo.GetByLocationId(id))
                .Returns(ListOfTwoOrders());
            var controller = new LocationsController(mockLocationRepo.Object, mockOrderRepository.Object, null);

            // Act
            var result = controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<LocationWithOrderAndInventoryViewModel>(viewResult.ViewData.Model);
            Assert.Equal("Walmart", model.Name);
            var inventory = Assert.IsAssignableFrom<IEnumerable<InventoryViewModel>>(model.Inventory);
            Assert.Equal(2, inventory.Count());
            var orders = Assert.IsAssignableFrom<IEnumerable<OrderViewModel>>(model.Orders);
            Assert.Equal(2, orders.Count());
        }

        [Fact]
        public void Create_PostRequest_EnsureRedirectionToIndex()
        {
            var mockLocationRepo = new Mock<ILocationRepository>();
            mockLocationRepo.Setup(repo => repo.Create(null));
            var controller = new LocationsController(mockLocationRepo.Object, null, null);
            var locationViewModel = new LocationViewModel() { Name = "Walmart" };

            // Act
            var result = controller.Create(locationViewModel, null);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        private static IEnumerable<Order> ListOfTwoOrders()
        {
            return new List<Order>
            {
                new Order(1, 1, DateTime.Now, 1, 20.00m),
                new Order(1, 1, DateTime.Now, 2, 20.00m)
            };
        }

        private static Location LocationWithInventory()
        {
            var productOne = new Product("Test", 1, 1.99m, "Test", 100);
            var productTwo = new Product("TestTwo", 1, 1.99m, "Test", 100);
            var inventory = new List<Inventory>
            {
                new Inventory(productOne, 100),
                new Inventory(productTwo, 200)
            };
            return new Location("Walmart", 1, inventory);
        }

        private static IEnumerable<Location> ListOfTwoLocations()
        {
            var locations = new List<Location>
            {
                new Location("Walmart", 1),
                new Location("Waffle House", 2)
            };
            return locations;
        }
    }
}
