using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Users;
using DataStorage;

namespace WalletApp.WalletAppWPF.Services
{
    public class AuthenticationService
    {
        private FileDataStorage<DBUser> _storage = new FileDataStorage<DBUser>();

        public async Task<User> AuthenticateAsync(AuthenticationUser authUser)
        {
            if (String.IsNullOrWhiteSpace(authUser.Login) || String.IsNullOrWhiteSpace(authUser.Password))
                throw new ArgumentException("Login or Password is Empty");
            var users = await _storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Login == authUser.Login && user.Password == authUser.Password);
            if (dbUser == null)
                throw new Exception("Wrong Login or Password");
            return new User(dbUser.Guid, dbUser.FirstName, dbUser.LastName, dbUser.Email, dbUser.Login);
        }

        public async Task<bool> RegisterUserAsync(RegistrationUser regUser)
        {
            Thread.Sleep(2000);
            var users = await _storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
            if (dbUser != null)
                throw new Exception("User already exists");
            if (String.IsNullOrWhiteSpace(regUser.Login) || String.IsNullOrWhiteSpace(regUser.Password)
                || String.IsNullOrWhiteSpace(regUser.LastName) || String.IsNullOrWhiteSpace(regUser.FirstName)
                || String.IsNullOrWhiteSpace(regUser.Email))
                throw new ArgumentException("Login, Password or personal information is Empty");
            if (!IsValidEmail(regUser.Email))
                throw new ArgumentException("Email is Invalid");
            if (2 <= regUser.FirstName.Length && regUser.FirstName.Length <= 18)
                throw new ArgumentException("First Name length is Invalid");
            if (2 <= regUser.LastName.Length && regUser.LastName.Length <= 20)
                throw new ArgumentException("Last Name length is Invalid");
            dbUser = new DBUser(regUser.FirstName, regUser.LastName, regUser.Email,
                regUser.Login, regUser.Password);
            await _storage.AddOrUpdateAsync(dbUser);
            return true;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}