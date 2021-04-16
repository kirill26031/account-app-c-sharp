using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Users;
using DataStorage;
using WalletApp.WalletAppWPF.Models.Categories;
using WalletApp.WalletAppWPF.Models.Wallets;

namespace WalletApp.WalletAppWPF.Services
{
    public class AuthenticationService
    {
        private FileDataStorage<DBUser> _storage = new FileDataStorage<DBUser>();
        private WalletService _walletService = new WalletService();

        public async Task<User> AuthenticateAsync(AuthenticationUser authUser)
        {
            if (String.IsNullOrWhiteSpace(authUser.Login) || String.IsNullOrWhiteSpace(authUser.Password))
                throw new ArgumentException("Login or Password is Empty");
            var users = await _storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Login == authUser.Login && user.Hash == EncryptPassword(authUser.Password));
            if (dbUser == null)
                throw new Exception("Wrong Login or Password");
            return new User(dbUser.Guid, dbUser.FirstName, dbUser.LastName, dbUser.Email, dbUser.Login, dbUser.Categories,
                await _walletService.WalletsByGuids(dbUser.WalletGuids)) ;
        }



        public async Task<bool> RegisterUserAsync(RegistrationUser regUser)
        {
            var users = await _storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
            if (dbUser != null)
                throw new Exception("User already exists");
            if (String.IsNullOrWhiteSpace(regUser.Login) || String.IsNullOrWhiteSpace(regUser.Password)
                || String.IsNullOrWhiteSpace(regUser.LastName) || String.IsNullOrWhiteSpace(regUser.FirstName)
                || String.IsNullOrWhiteSpace(regUser.Email))
                throw new ArgumentException("Login, Password or personal information is Empty");
            if (regUser.FirstName.Any(char.IsDigit) || regUser.LastName.Any(char.IsDigit))
                throw new ArgumentException("First and Last Names cannot contain numbers");
            if (!(2 <= regUser.FirstName.Length && regUser.FirstName.Length <= 18))
                throw new ArgumentException("First Name length is Invalid");
            if (!(2 <= regUser.LastName.Length && regUser.LastName.Length <= 20))
                throw new ArgumentException("Last Name length is Invalid");
            if (!IsValidEmail(regUser.Email))
                throw new ArgumentException("Email is Invalid");
            if (regUser.Categories.Count == 0)
                throw new ArgumentException("At least one category must be chosen");
            dbUser = new DBUser(regUser.FirstName, regUser.LastName, regUser.Email,
                regUser.Login, EncryptPassword(regUser.Password), regUser.Categories, new List<Models.Wallets.Wallet>());
            await _storage.AddOrUpdateAsync(dbUser);
            return true;
        }

        public async Task<User> FindUser(Guid guid)
        {
            var users = await _storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Guid == guid);
            if (dbUser == null) return null;
            return new User(dbUser.Guid, dbUser.FirstName, dbUser.LastName, dbUser.Email, dbUser.Login, dbUser.Categories, new List<Models.Wallets.Wallet>());
        }

        public async Task<List<Category>> GetCategoriesForUser(Guid ownerId)
        {
            var user = await FindUser(ownerId);
            if (user == null || user.Categories.Count == 0) return new List<Category>();
            return user.Categories;
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

        private static String EncryptPassword(String inputString)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(inputString);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            return hash;
        }

        public async Task UpdateUser(User user)
        {
            var users = await _storage.GetAllAsync();
            var currentDBUser = users.FirstOrDefault(u => user.Guid == u.Guid);
            DBUser dbUser = new DBUser(user.FirstName, user.LastName, user.Email, user.Login, currentDBUser.Hash, user.Categories, user.Wallets);
            await _storage.AddOrUpdateAsync(dbUser);
        }
    }
}