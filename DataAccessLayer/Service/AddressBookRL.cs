using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using DataAccessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;

namespace DataAccessLayer.Service
{
    public class AddressBookRL : IAddressBookRL
    {
        AddressBookContext _dbContext;

        public AddressBookRL(AddressBookContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<AddressBookEntity> FetchAllContancts()
        {
            return  _dbContext.AddressBook.ToList();
        }

        public AddressBookEntity FindContactById(int id)
        {
            try
            {
                var data = _dbContext.AddressBook.Find(id);
                if(data == null)
                {
                    throw new KeyNotFoundException();
                }
                return data;
            }
            catch (KeyNotFoundException key)
            {
                throw new KeyNotFoundException(key.Message);
            }
            
            
        }
        public AddressBookEntity AddNewContact(AddressBookEntity addressBookEntity)
        {
            try
            {
                var contact = _dbContext.AddressBook.FirstOrDefault(c => c.Email == addressBookEntity.Email);
                if(contact != null)
                {
                    throw new Exception("Contact already Exist");
                }
                _dbContext.AddressBook.Add(addressBookEntity);
                _dbContext.SaveChanges();
                return addressBookEntity;
            }
            catch (Exception)
            {
                throw;
            }
           

        }

        public AddressBookEntity GetEmail(string Email)
        {
              var email = _dbContext.AddressBook.FirstOrDefault(e => e.Email == Email);
                if(email == null)
                {
                    return null;
                }
                return email;
            
            
        }
        public AddressBookEntity UpdateContact(AddressBookEntity addressBookEntity)
        {
            _dbContext.AddressBook.Update(addressBookEntity);
            _dbContext.SaveChanges();
            return addressBookEntity;
        }
        public bool DeleteContact(int id)
        {
            try
            {
                var contact = _dbContext.AddressBook.FirstOrDefault(c => c.Id == id);

                if (contact == null)
                {
                    throw new KeyNotFoundException();
                }

                _dbContext.AddressBook.Remove(contact);
                _dbContext.SaveChanges();

                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }

        }


    }
}
