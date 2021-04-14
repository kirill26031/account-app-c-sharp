using System;
using System.Windows;
using System.Windows.Controls;
using WalletApp.WalletAppWPF.Models.Categories;
//using WalletApp.WalletAppWPF.Authentication;

namespace WalletApp.WalletAppWPF.Authentication
{
    /// <summary>
    /// Interaction logic for SignInView.xaml
    /// </summary>
    public partial class SignUpView : UserControl
    {
        public SignUpView()
        {
            InitializeComponent();

            //TODO: add categories to listbox
            //SignUpViewModel smth;
            //foreach (Category category in smth.InitializeCategories()) 
            //{
            //    string categoryName = category.Name + category.Description;
            //    categoriesListBox.Items.Add(categoryName);
            //}
        }

        private void TbPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((SignUpViewModel)DataContext).Password = TbPassword.Password;
        }
    }
}
