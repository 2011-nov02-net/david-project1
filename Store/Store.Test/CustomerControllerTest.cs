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
    public class CustomerControllerTest
    {
        [Fact]
        public void Index_ReturnsAView_WithAListOfCustomers()
        {
            // Arrange
            var mockRepo = new Mock<ICustomerRepository>();
            mockRepo.Setup(repo => repo.GetAll())
                .Returns(ListOfTwoCustomers());
            var controller = new CustomersController(mockRepo.Object, null);

            // Act
            var result = controller.Index(null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Customer>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Theory]
        [InlineData("Sherlock")]
        [InlineData("David")]
        public void Index_ReturnsAView_WithAListOfCustomers_SearchByFirstName_CustomerExists(string firstName)
        {
            // Arrange
            var mockRepo = new Mock<ICustomerRepository>();
            mockRepo.Setup(repo => repo.GetAll())
                .Returns(ListOfTwoCustomers());
            var controller = new CustomersController(mockRepo.Object, null);

            // Act
            var result = controller.Index(firstName, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Customer>>(viewResult.ViewData.Model);
            var customer = model.First();
            Assert.Single(model);
            Assert.Equal(firstName, customer.FirstName);
        }

        [Theory]
        [InlineData("Holmes")]
        [InlineData("Barnes")]
        public void Index_ReturnsAView_WithAListOfCustomers_SearchByLastName_CustomerExists(string lastName)
        {
            // Arrange
            var mockRepo = new Mock<ICustomerRepository>();
            mockRepo.Setup(repo => repo.GetAll())
                .Returns(ListOfTwoCustomers());
            var controller = new CustomersController(mockRepo.Object, null);

            // Act
            var result = controller.Index(null, lastName);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Customer>>(viewResult.ViewData.Model);
            var customer = model.First();
            Assert.Single(model);
            Assert.Equal(lastName, customer.LastName);
        }

        [Fact]
        public void Detail_ReturnsAView_CustomerDetailsWithListOfOrders()
        {
            // Arrange
            int id = 1;
            var mockCustRepo = new Mock<ICustomerRepository>();
            mockCustRepo.Setup(repo => repo.Get(id))
                .Returns(new Customer("Sherlock", "Holmes", id));
            var mockOrderRepo = new Mock<IOrderRepository>();
            mockOrderRepo.Setup(repo => repo.GetByCustomerId(id))
                .Returns(ListOfTwoOrders());
            var controller = new CustomersController(mockCustRepo.Object, mockOrderRepo.Object);

            // Act
            var result = controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CustomerWithOrderViewModel>(viewResult.ViewData.Model);
            var orders = Assert.IsAssignableFrom<IEnumerable<OrderViewModel>>(model.Orders);
            Assert.Equal("Sherlock", model.FirstName);
            Assert.Equal("Holmes", model.LastName);
            Assert.Equal(2, orders.Count());
        }

        private static IEnumerable<Customer> ListOfTwoCustomers()
        {
            var customers = new List<Customer>
            {
                new Customer("Sherlock", "Holmes", 1),
                new Customer("David", "Barnes", 2)
            };
            return customers;
        }

        private static IEnumerable<Order> ListOfTwoOrders()
        {
            return new List<Order>
            {
                new Order(1,99,DateTime.Now,1,1.99m),
                new Order(1,99,DateTime.Now,2,4.99m)
            };
        }
    }
}
