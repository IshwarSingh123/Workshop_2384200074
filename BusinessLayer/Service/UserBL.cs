using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataAccessLayer.Entity;
using DataAccessLayer.Interface;
using ModelLayer.Model;
using BCrypt.Net;
using StackExchange.Redis;
using Newtonsoft.Json.Linq;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly JwtServices _jwtServices;
        private readonly IEmailService _emailService;
        private readonly IDatabase _redisDb;
        public UserBL(IUserRL userRL, JwtServices jwtServices, IEmailService emailService,IDatabase redis)
        {
            _userRL = userRL;
            _jwtServices = jwtServices;
            _emailService = emailService;
            _redisDb = redis;
            
        }


        public UserEntity Registration(RegistrationUserModel registrationUserModel)
        {
            try
            {
                var existingUser = _userRL.GetEmail(registrationUserModel.Email);
                if (existingUser != null)
                {
                    throw new Exception("User already Registered!");
                }


                var newUser = new UserEntity
                {
                    FirstName = registrationUserModel.FirstName,
                    LastName = registrationUserModel.LastName,
                    Email = registrationUserModel.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(registrationUserModel.Password)

                };

                return _userRL.Registration(newUser);
            }
            catch (Exception)
            {
                throw;
            }


        }



        public string Login(LoginUserModel userLoginModel)
        {
            try
            {
                if (userLoginModel == null)
                {
                    throw new ArgumentNullException(nameof(userLoginModel), "Login data cannot be null.");
                }

                string cacheKey = $"UserToken:{userLoginModel.Email}";

                // Check if token exists in Redis cache
                var cachedToken = _redisDb.StringGet(cacheKey);
                if (!cachedToken.IsNullOrEmpty)
                {
                    return cachedToken; // Return cached JWT token
                }

                var data = _userRL.Login(userLoginModel);

                if (data == null)
                {
                    throw new Exception("User not found! Please register first.");
                }

                if (!BCrypt.Net.BCrypt.Verify(userLoginModel.Password, data.Password))
                {
                    throw new Exception("Invalid credentials.");
                }
                else
                {
                    string token = _jwtServices.GenerateToken(data); // Return token on successful login
                    _redisDb.StringSet(cacheKey, token, TimeSpan.FromMinutes(30));
                    return token;
                }

                
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("Invalid login request: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed: " + ex.Message);
            }
        }

        public bool ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(forgetPasswordModel.Email))
                {
                    throw new ArgumentException();
                }
                var user = _userRL.GetEmail(forgetPasswordModel.Email);
                if (user == null)
                {
                    return false;
                }
                string token = _jwtServices.GenerateToken(user);
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new InvalidOperationException();
                }

                return _emailService.SendEmail(forgetPasswordModel.Email, "Reset Password", token);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (InvalidOperationException) 
            { 
                throw;
            }
            

            
        }
        public bool ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                string email = _jwtServices.ValidateToken(resetPasswordModel.Token);
                if (string.IsNullOrEmpty(email))
                {
                    return false;
                }

                var user = _userRL.GetEmail(email);
                if (user == null)
                {
                    return false;
                }

                // Hash the new password
                user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordModel.NewPassword);

                return _userRL.UpdatePassword(user);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool Logout(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
                }

                string cacheKey = $"UserToken:{email}";

                if (_redisDb.KeyExists(cacheKey))
                {
                    _redisDb.KeyDelete(cacheKey); 
                    return true; 
                }
                return false; 
            }
            catch (Exception ex)
            {
                throw new Exception("Logout failed: " + ex.Message);
            }
        }

    }
}
