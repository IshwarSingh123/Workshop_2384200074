using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataAccessLayer.Entity;
using DataAccessLayer.Interface;
using ModelLayer.Model;

namespace BusinessLayer.Service
{
    public class AddressBookBL : IAddressBookBL
    {
        private readonly IAddressBookRL _addressBookRL;
        public AddressBookBL(IAddressBookRL addressBookRL)
        {
            _addressBookRL = addressBookRL;
        }

        public List<AddressBookEntity> FetchAllContancts()
        {
            return _addressBookRL.FetchAllContancts();
        }

        public AddressBookEntity FindContactById(int id)
        {
            try
            {
                var result = _addressBookRL.FindContactById(id);
                if(result == null)
                {
                    throw new NullReferenceException();
                }
                return result;
            }
            catch (NullReferenceException )
            {
                throw;
            }
        }
        public AddressBookEntity AddNewContact(AddressBookModel addressBookModel)
        {
            try
            {
                var contact = _addressBookRL.GetEmail(addressBookModel.Email);
                if (contact != null)
                {
                    throw new Exception("Contact already Exist!");
                }
                var newContact = new AddressBookEntity
                {
                    FirstName = addressBookModel.FirstName,
                    LastName = addressBookModel.LastName,
                    Email = addressBookModel.Email,
                    PhoneNumber = addressBookModel.PhoneNumber,
                    Address = addressBookModel.Address,
                    Country = addressBookModel.Country,
                    UserId= addressBookModel.UserId

                };
                return _addressBookRL.AddNewContact(newContact);
            }
            catch (Exception )
            {
                throw;
            }
            
        }
        public AddressBookEntity UpdateContact(int id, AddressBookModel updatedContact)
        {
            var existingContact = _addressBookRL.FindContactById(id);
            if (existingContact == null)
            {
                return null;
            }

            // Update contact details
            existingContact.FirstName = updatedContact.FirstName;
            existingContact.LastName = updatedContact.LastName;
            existingContact.Email = updatedContact.Email;
            existingContact.PhoneNumber = updatedContact.PhoneNumber;
            existingContact.Address = updatedContact.Address;
            existingContact.Country = updatedContact.Country;

            return _addressBookRL.UpdateContact(existingContact);
        }
        public bool DeleteContact(int id)
        {
            try
            {
                var contact = _addressBookRL.FindContactById(id);

                if (contact == null)
                {
                    throw new KeyNotFoundException();
                }

                return _addressBookRL.DeleteContact(id);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            
        }


    }
}
