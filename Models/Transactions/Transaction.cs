using System;
using System.Collections.Generic;
using WalletApp.WalletAppWPF.Models.Common;
using WalletApp.WalletAppWPF.Models.Categories;
using DataStorage;

namespace WalletApp.WalletAppWPF.Models.Transactions
{
    public class Transaction : IStorable
    {
        public Guid Guid { get;}
        private decimal _sum;
        private Category _category;
        private Currency.currencyType _currencyType;
        private string _description;
        public DateTimeOffset dateTime { get; set; }
        private List<File> _files;
        private Guid _creatorId;

        public Guid CreatorId
        {
            get => _creatorId;
        }

        public Category Category
        {
            get => _category;
            private set => _category = value;
        }

        public Currency.currencyType CurrencyType
        {
            get => _currencyType;
            private set => _currencyType = value;
        }

        public Transaction(decimal sum, Category category, Currency.currencyType currencyType, string descriprion, DateTimeOffset dateTime, List<File> files, Guid userId)
        {
            Guid = Guid.NewGuid();
            Sum = sum;
            _category = category;
            _currencyType = currencyType;
            Description = descriprion;
            this.dateTime = dateTime;
            _files = new List<File>(files);
            _creatorId = userId;
        }

        public List<File> Files
        {
            get
            {
                List<File> copy = new List<File>();
                foreach (File File in _files)
                {
                    copy.Add(new File(File));
                }
                return copy;
            }
            set => _files = value;
        }

        public decimal Sum
        {
            get => _sum;
            private set => _sum = value;
        }
        public string Description
        {
            get => _description;
            private set => _description = value;
        }

        public bool UpdateTransaction(decimal sum, string description, DateTimeOffset dateTime, List<File> files)
        {
            _sum = sum;
            _description = description;
            this.dateTime = dateTime;
            _files = new List<File>(files);
            return true;
        }



        public override string ToString()
        {
            return $"Transaction ${Guid.ToString()} used ${Sum.ToString()} of ${_currencyType} at ${dateTime.ToString()}. Description: ${Description}";
        }
    }
}