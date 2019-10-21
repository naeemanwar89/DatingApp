using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data.Repositry
{
    public class AuthRepositry : IAuthtRepositry
    {
        public readonly DataContext DataContext;
        public AuthRepositry( DataContext dataContext)
        {
            DataContext = dataContext;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await DataContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
                return null;
            if (!VerifyUserHashPassword(password,user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }

        private bool VerifyUserHashPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac=new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
               var hash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               for(int i=0; i<hash.Length; i++)
                {
                    if (hash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password,out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await DataContext.Users.AddAsync(user);
            await DataContext.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac =new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await DataContext.Users.AnyAsync(x => x.UserName == username))
                return true;
            return false;
        }
    }
}
