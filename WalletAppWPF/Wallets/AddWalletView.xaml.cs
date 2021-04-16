using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WalletApp.WalletAppWPF.Models.Categories;

namespace WalletApp.WalletAppWPF.Wallets
{
    /// <summary>
    /// Interaction logic for AddWallet.xaml
    /// </summary>
    public partial class AddWalletView : UserControl
    {
        public AddWalletView()
        {
            InitializeComponent();
        }

        private void Checked_CategoryCheckBox(object sender, System.EventArgs e)
        {
            List<Category> categories = new List<Category>();
            foreach(Category category in CategoriesListBox.SelectedItems)
            {
                categories.Add(category);
            }
            ((AddWalletViewModel)DataContext).Categories = categories;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+(?:\\.[0-9]*)?");
            var possibleNext = Balance.Text + e.Text;
            e.Handled = !(regex.IsMatch(possibleNext) && 
                regex.Match(possibleNext).Value.Count<char>() == possibleNext.Length && 
                regex.Matches(possibleNext).Count == 1);
        }
    }
}
