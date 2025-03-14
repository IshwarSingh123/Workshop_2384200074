using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelLayer.Model;
using DataAccessLayer.Entity;

namespace DataAccessLayer.Interface
{
    public interface IAddressBookRL
    {
        List<AddressBookEntity> FetchAllContancts();
        AddressBookEntity FindContactById(int id);
        AddressBookEntity AddNewContact(AddressBookEntity addressBookEntity);
        AddressBookEntity GetEmail(string email);
        AddressBookEntity UpdateContact(AddressBookEntity addressBookEntity);
        bool DeleteContact(int id);
    }
}
