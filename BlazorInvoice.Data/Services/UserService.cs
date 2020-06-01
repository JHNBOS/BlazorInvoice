using Blazored.LocalStorage;
using BlazorInvoice.Data.Services.Interfaces;
using BlazorInvoice.Infrastructure.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInvoice.Data.Services
{
    public class UserService : IUserService
    {
        public Task<User> GetUser(int id) => throw new NotImplementedException();
        public Task<User> GetUserByEmail(string email) => throw new NotImplementedException();
        public Task<User> GetUserByToken(string token) => throw new NotImplementedException();
    }
}