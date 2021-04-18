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

namespace WalletApp.WalletAppWPF.Transactions
{
    /// <summary>
    /// Interaction logic for TransactionDetailsView.xaml
    /// </summary>
    public partial class AddTransactionView : UserControl
    {
        private bool IsLoaded { get; set; }
        public AddTransactionView()
        {
            IsLoaded = false;
            InitializeComponent();
        }

        void Window_Loaded(object sender, RoutedEventArgs e) {
            IsLoaded = true;
            SetCategory();
        }

        void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsLoaded = false;
        }

        void DataContextUpdated(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsLoaded)
            {
                SetCategory();
            }
        }

        private void SetCategory()
        {
            if (DataContext != null)
            {
                int index = ((AddTransactionViewModel)DataContext).Categories.FindIndex(
                        cat => cat.Name == ((AddTransactionViewModel)DataContext).Transaction.Category.Name);
                CategoriesListBox.SelectedItem = CategoriesListBox.Items[index];
            }
            else
            {
                //MessageBox.Show("Set category with null context");
            }
        }

        private void Checked_CategoryCheckBox(object sender, System.EventArgs e)
        {
            ((AddTransactionViewModel)DataContext).Categories = 
                new() { (Category)CategoriesListBox.SelectedItem };
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("-?(?:[0-9]+(?:\\.[0-9]*)?)?");
            var possibleNext = Sum.Text + e.Text;
            e.Handled = !(regex.IsMatch(possibleNext) &&
                regex.Match(possibleNext).Value.Count<char>() == possibleNext.Length);
        }
    }
}
