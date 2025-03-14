using BusinessLayer.Interface;
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
        [HttpPost("addNewContact")]
        public IActionResult AddNewContact(AddressBookModel addressBookModel)
        {
            try
            {
                if (addressBookModel == null)
                {
                    return BadRequest("AddressBookModel is null.");
                }

                var response = new ResponseModel<string>();
                var result = _addressBookBL.AddNewContact(addressBookModel);

                response.Success = true;
                response.Message = "Contact Added Successfully.";
                response.Data = result.Email;
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

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
