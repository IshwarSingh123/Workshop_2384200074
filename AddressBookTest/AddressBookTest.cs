using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Address_Book_Application.Controllers;
using BusinessLayer.Interface;
using DataAccessLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using Moq;
using NUnit.Framework;


namespace AddressBookTest
{
    [TestFixture]
    public class AddressBookControllerTest
    {
        private AddressBookController _controller;
        private Mock<IAddressBookBL> _mockAddressBookBL;

        [SetUp]
        public void SetUp()
        {
            _mockAddressBookBL = new Mock<IAddressBookBL>();
            _controller = new AddressBookController(_mockAddressBookBL.Object);
        }
        [Test]
        public void AddNewContact_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var addressBookModel = new AddressBookModel
            {
                FirstName = "Ishwar",
                LastName = "Singh",
                Email = "ishwarmars@gmail.com",
                PhoneNumber = "1234567890",
                Address = "Mathura",
                Country = "India",
                UserId = 2
            };

            var expectedEntity = new AddressBookEntity
            {
                FirstName = addressBookModel.FirstName,
                LastName = addressBookModel.LastName,
                Email = addressBookModel.Email,
                PhoneNumber = addressBookModel.PhoneNumber,
                Address = addressBookModel.Address,
                Country = addressBookModel.Country,
                UserId = addressBookModel.UserId
            };

            _mockAddressBookBL
                .Setup(bl => bl.AddNewContact(It.IsAny<AddressBookModel>()))
                .Returns(expectedEntity); // ✅ Fix: Return entity instead of string

            // Act
            var result = _controller.AddNewContact(addressBookModel) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var response = result.Value as ResponseModel<AddressBookEntity>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Success, Is.True);
            Assert.That(response.Message, Is.EqualTo("Contact Added Successfully."));
            Assert.That(response.Data.Email, Is.EqualTo(expectedEntity.Email)); 
        }


        [Test]
        public void AddNewContact_NullModel_ReturnsBadRequest()
        {
            // Act
            var result = _controller.AddNewContact(null) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400)); 

            var response = result.Value as ResponseModel<string>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Success, Is.False); //
            Assert.That(response.Message, Is.EqualTo("Invalid contact details"));
        }


        [Test]
        public void AddNewContact_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var addressBookModel = new AddressBookModel
            {
                FirstName = "Ishwar",
                LastName = "Singh",
                Email = "ishwarmars@gmail.com",
                PhoneNumber = "1234567890",
                Address = "Mathura",
                Country = "India",
                UserId = 2
            };

            _mockAddressBookBL
                .Setup(bl => bl.AddNewContact(It.IsAny<AddressBookModel>()))
                .Throws(new Exception("Database error"));

            // Act
            var result = _controller.AddNewContact(addressBookModel) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(500)); // ✅ Ensure correct status code

            var response = result.Value as ResponseModel<string>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Success, Is.False);  // ✅ Ensure failure response
            Assert.That(response.Message, Is.EqualTo("Internal Server Error"));  // ✅ Correct error message
            Assert.That(response.Data, Is.EqualTo("Database error"));  // ✅ Ensure correct exception message
        }


    }
}
