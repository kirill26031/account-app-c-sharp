using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
