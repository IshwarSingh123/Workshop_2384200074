using BusinessLayer.Interface;
using DataAccessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

namespace Address_Book_Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserBL _userBL;
        private readonly IEmailService _emailService;
        public UserAuthenticationController(IUserBL userBL,IEmailService emailService)
        {
            _userBL = userBL;
            _emailService = emailService;
        }

        /// <summary>
        /// Post Method for Login User
        /// </summary>
        /// <param name="userLoginModel"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login(LoginUserModel userLoginModel)
        {
            try
            {
                var response = new ResponseModel<string>();
                var user = _userBL.Login(userLoginModel);
                if (user != null)
                {
                    response.Success = true;
                    response.Message = "User Login Successfully.";
                    response.Data = user;
                    return Ok(response);
                }
                return BadRequest("Invalid Credentials!");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Postmethod to register the user
        /// </summary>
        /// <param name="registrationUserModel"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public IActionResult Registration(RegistrationUserModel registrationUserModel)
        {
            try
            {
                var response = new ResponseModel<string>();
                var data = _userBL.Registration(registrationUserModel);
                if (data == null)
                {
                    response.Success = false;
                    response.Message = "User Already registered Successfully.";
                    response.Data = data.Email;
                    
                    return Ok(response);
                }


                response.Success = true;
                response.Message = "User registered Successfully.";
                response.Data = data.Email;
                _emailService.SendEmail(data.Email, "Registration successfully", "You Are Registered Successfully");
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }
               
        }


           


        
        /// <summary>
        /// Post method to Forget password
        /// </summary>
        /// <param name="forgetPasswordModel"></param>
        /// <returns></returns>
        [HttpPost("forget-password")]
        public IActionResult ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            try
            {
                var result = _userBL.ForgetPassword(forgetPasswordModel);
                if (!result)
                {
                    return BadRequest("Email not found!");
                }

                return Ok("Reset password email sent successfully.");
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        /// <summary>
        /// post method for Reset password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var result = _userBL.ResetPassword(model);
                if (!result)
                {
                    return BadRequest("Invalid or expired token.");
                }
                return Ok("Password reset successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

    }
}

