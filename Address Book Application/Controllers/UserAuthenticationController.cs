using BusinessLayer.Interface;
using BusinessLayer.Service;
using DataAccessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using Newtonsoft.Json;

namespace Address_Book_Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserBL _userBL;
        private readonly IEmailService _emailService;
        private readonly IRabbitMQProducer _rabbitMQProducer;
        public UserAuthenticationController(IUserBL userBL,IEmailService emailService, IRabbitMQProducer rabbitMQProducer)
        {
            _userBL = userBL;
            _emailService = emailService;
            _rabbitMQProducer = rabbitMQProducer;
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
        public IActionResult Registration([FromBody] RegistrationUserModel registrationUserModel)
        {
            try
            {
                var response = new ResponseModel<string>();
                var data = _userBL.Registration(registrationUserModel);

                if (data == null)
                {
                    response.Success = false;
                    response.Message = "User already registered!";
                    return BadRequest(response);
                }

                response.Success = true;
                response.Message = "User registered successfully.";
                response.Data = data.Email;

                // Prepare the message for RabbitMQ
                string jsonMessage = JsonConvert.SerializeObject(registrationUserModel);

                try
                {
                    _rabbitMQProducer.PublishMessage(jsonMessage);
                    _emailService.SendEmail(data.Email, "Registration Successful", "You are registered successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"RabbitMQ or Email Error: {ex.Message}");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal Server Error", error = ex.Message });
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
        /// <summary>
        /// Post method for logout
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Email is required for logout.");
                }

                bool isLoggedOut = _userBL.Logout(email);
                if (isLoggedOut)
                {
                    return Ok(new { Success = true, Message = "User logged out successfully." });
                }
                return BadRequest("User is not logged in or session expired.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }


    }
}

