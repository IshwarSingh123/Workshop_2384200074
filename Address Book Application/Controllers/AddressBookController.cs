using BusinessLayer.Interface;
using DataAccessLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

namespace Address_Book_Application.Controllers
{
    /// <summary>
    /// class providing API for Address Book
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AddressBookController : ControllerBase
    {

        private readonly IAddressBookBL _addressBookBL;

        /// <summary>
        /// Dependency inject 
        /// </summary>
        /// <param name="addressBookBL"></param>
        public AddressBookController(IAddressBookBL addressBookBL)
        {
            _addressBookBL = addressBookBL;
        }
        /// <summary>
        /// Get method to get all contacts
        /// </summary>
        /// <returns></returns>
      
        [HttpGet]
        public IActionResult GetAllContacts()
        {
            var result = _addressBookBL.FetchAllContancts();
            return Ok(result);
        }

        /// <summary>
        /// Get Method to find contacts by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetContactById(int id)
        {
            try
            {
                var response = new ResponseModel<object>();
                var result = _addressBookBL.FindContactById(id);
                if (result == null)
                {
                    return BadRequest("Id Not found");
                }
                response.Success = true;
                response.Message = "Contact found successfully!";
                response.Data = new
                {
                    Id = result.Id,
                    Name = $"{result.FirstName} {result.LastName}",
                    Email = result.Email,
                    Phone = result.PhoneNumber,
                    Address = result.Address,
                    Country = result.Country
                };
                return Ok(response);
            }
            catch(NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        /// <summary>
        /// Post method to add a new contact
        /// </summary>
        /// <param name="addressBookModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult AddNewContact([FromBody] AddressBookModel addressBookModel)
        {
            try
            {
                if (addressBookModel == null)
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid contact details",
                        Data = null
                    });
                }

                var result = _addressBookBL.AddNewContact(addressBookModel);
                return Ok(new ResponseModel<AddressBookEntity>
                {
                    Success = true,
                    Message = "Contact Added Successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = ex.Message  // Optional: You can remove this in production.
                });
            }
        }

        /// <summary>
        /// Put method to Update a contact
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedContact"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id, [FromBody] AddressBookModel updatedContact)
        {
            var response = new ResponseModel<object>();

            if (id <= 0 || updatedContact == null)
            {
                return BadRequest(new { Success = false, Message = "Invalid request data." });
            }

            var result = _addressBookBL.UpdateContact(id, updatedContact);

            if (result == null)
            {
                response.Success = false;
                response.Message = "Contact not found!";
                return NotFound(response);
            }

            response.Success = true;
            response.Message = "Contact updated successfully!";
            response.Data = new
            {
                
                Name = $"{result.FirstName} {result.LastName}",
                Email = result.Email,
                Phone = result.PhoneNumber,
                Address = result.Address,
                Country = result.Country
            };

            return Ok(response);
        }
        /// <summary>
        /// Delete method to delete a contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            try
            {
                var response = new ResponseModel<string>();

                if (id <= 0)
                {
                    response.Success = false;
                    response.Message = "Invalid contact ID.";
                    return BadRequest(response);
                }

                var isDeleted = _addressBookBL.DeleteContact(id);

                if (!isDeleted)
                {
                    response.Success = false;
                    response.Message = "Contact not found!";
                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Contact deleted successfully!";
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }


    }
}
