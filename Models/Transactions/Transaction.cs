using System;
using System.Collections.Generic;
using WalletApp.WalletAppWPF.Models.Common;
using WalletApp.WalletAppWPF.Models.Categories;
using DataStorage;
using System.Text.Json.Serialization;

namespace WalletApp.WalletAppWPF.Models.Transactions
{
    public class Transaction : IStorable
    {
        public Guid Guid { get;}
        decimal _sum;
        Category _category;
        Currency.currencyType _currencyType;
        string _description;
        List<File> _files;
        Guid _creatorId;

        public Guid CreatorId
        {
            get => _creatorId;
            set => _creatorId = value;
        }

        public Category Category
        {
            get => _category;
            set => _category = value;
        }

        public Currency.currencyType CurrencyType
        {
            get => _currencyType;
            set => _currencyType = value;
        }

        public Transaction(decimal sum, Category category, Currency.currencyType currencyType, string description, DateTimeOffset dateTime, List<File> files, Guid creatorId)
        {
            Guid = Guid.NewGuid();
            Sum = sum;
            Category = category;
            CurrencyType = currencyType;
            Description = description;
            DateTime = dateTime;
            Files = new List<File>(files);
            CreatorId = creatorId;
        }

        [JsonConstructor]
        public Transaction(Guid guid, decimal sum, Category category, Currency.currencyType currencyType, string description, DateTimeOffset dateTime, List<File> files, Guid creatorId)
        {
            Guid = guid;
            Sum = sum;
            Category = category;
            CurrencyType = currencyType;
            Description = description;
            DateTime = dateTime;
            Files = new List<File>(files);
            CreatorId = creatorId;
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
            set => _sum = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public DateTimeOffset DateTime { get; set; }

        public bool UpdateTransaction(decimal sum, Currency.currencyType currency, string description, DateTimeOffset dateTime, List<File> files)
        {
            _sum = sum;
            _description = description;
            _currencyType = currency;
            DateTime = dateTime;
            _files = new List<File>(files);
            return true;
        }



        public override string ToString()
        {
            return $"Transaction ${Guid.ToString()} used ${Sum.ToString()} of ${_currencyType} at ${DateTime.ToString()}. Description: ${Description}";
        }
    }
}