﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp
{
    public class Transaction
    {
        private Guid _Id;
        private decimal _Sum;
        private Category _Category;
        private Currency.CurrencyType _CurrencyType;
        private string _Description;
        public DateTimeOffset DateTime { get; set; }
        private List<File> _Files;

        public Guid Id
        {
            get => _Id;
        }

        public Category Category
        {
            get => _Category; 
            private set => _Category = value;
        }

        public Currency.CurrencyType CurrencyType
        {
            get => _CurrencyType;
            private set => _CurrencyType = value;
        }

        public Transaction(decimal sum, Category category, Currency.CurrencyType currencyType, string descriprion, DateTimeOffset dateTime, List<File> files)
        {
            _Id = Guid.NewGuid();
            Sum = sum;
            _Category = category;
            _CurrencyType = currencyType;
            Description = descriprion;
            DateTime = dateTime;
            _Files = new List<File>(files);
        }

        public List<File> Files
        {
            get
            {
                List<File> copy = new List<File>();
                foreach (File File in _Files) {
                    copy.Add(new File(File));
                }
                return copy;
            }
            set => _Files = value;
        }

        public decimal Sum { 
            get => _Sum; 
            private set => _Sum = value; 
        }
        public string Description { 
            get => _Description; 
            private set => _Description = value; 
        }

        public bool UpdateTransaction(decimal sum, string description, DateTimeOffset dateTime, List<File> files)
        {
            _Sum = sum;
            _Description = description;
            DateTime = dateTime;
            _Files = new List<File>(files);
            return true;
        }



        public override string ToString()
        {
            return $"Transaction ${Id.ToString()} used ${Sum.ToString()} of ${_CurrencyType} at ${DateTime.ToString()}. Description: ${Description}";
        }
    }
}
