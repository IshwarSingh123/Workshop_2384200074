using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entity;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IAddressBookBL
    {
        List<AddressBookEntity> FetchAllContancts();
        AddressBookEntity FindContactById(int id);
        AddressBookEntity AddNewContact(AddressBookModel addressBookModel);
        AddressBookEntity UpdateContact(int id, AddressBookModel updatedContact);
        bool DeleteContact(int id);
    }
}
