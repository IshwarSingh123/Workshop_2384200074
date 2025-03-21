﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using DataAccessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using StackExchange.Redis;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        AddressBookContext _dbContext;

        public UserRL(AddressBookContext dbContext)
        {
            _dbContext = dbContext;
        }


        public UserEntity GetEmail(string email)
        {
            var user = _dbContext.UserData.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    return null;
                }

                return user;
        }

        public UserEntity Login(LoginUserModel userLoginModel)
        {
            try
            {
                //string cacheKey = $"User:{userLoginModel.Email}";

                //var cachedToken = _cache.StringGet(cacheKey);
                //if (!cachedToken.IsNullOrEmpty)
                //{
                //    return cachedToken; // Return cached token
                //}
                var data = _dbContext.UserData.FirstOrDefault(e => e.Email == userLoginModel.Email);
                if (data != null)
                {
                    return data;
                }
                throw new NullReferenceException();
            }
            catch(NullReferenceException)
            {
                throw;
            }
        }

        public UserEntity Registration(UserEntity userEntity)
        {
            try
            {
                var data = _dbContext.UserData.FirstOrDefault(e => e.Email == userEntity.Email);
                if (data != null)
                {
                    throw new Exception("User Already registered!");
                }
                _dbContext.UserData.Add(userEntity);
                _dbContext.SaveChanges();
                return userEntity;
            }
            catch(Exception)
            {
                throw;
            }
                
        }
           
        


        public bool UpdatePassword(UserEntity userEntity)
        {

            _dbContext.UserData.Update(userEntity);
            _dbContext.SaveChanges();
            return true;
        }

       
    }
}
