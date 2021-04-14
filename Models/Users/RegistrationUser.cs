using System.Collections.Generic;
using WalletApp.WalletAppWPF.Models.Categories;

namespace WalletApp.WalletAppWPF.Models.Users
{
    public class RegistrationUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Category> Categories { get; set; }
    }
}