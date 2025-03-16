using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entity;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {

        string Login(LoginUserModel userLoginModel);
        UserEntity Registration(RegistrationUserModel registrationUserModel);
        bool ForgetPassword(ForgetPasswordModel model);
        bool ResetPassword(ResetPasswordModel model);
        bool Logout(string email);
    }
}
