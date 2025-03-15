using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entity;
using ModelLayer.Model;

namespace DataAccessLayer.Interface
{
    public interface IUserRL
    {
        UserEntity Login(LoginUserModel userLoginModel);
        UserEntity Registration(UserEntity userEntity);
        bool UpdatePassword(UserEntity userEntity);
        UserEntity GetEmail(string email);

    }
}
