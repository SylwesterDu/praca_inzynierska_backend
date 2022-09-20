using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.Entities;

namespace backend.Repositories.AccountRepository
{
    public interface IAccountRepository
    {
        Task<User> getUserByToken(string token);
    }
}