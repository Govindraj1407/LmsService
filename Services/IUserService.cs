using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface IUserService
    {
        public Task<string> CreateUser(User user);
    }
}
