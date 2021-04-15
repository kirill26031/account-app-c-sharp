using DataStorage;
using System;

namespace WalletApp.WalletAppWPF.Models.Categories
{
    public class Category : IStorable
    {
        public Guid Guid { get; }
        string _name;
        string _description;
        string _color;
        string _iconRef;

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public string Color
        {
            get => _color;
            set => _color = value;
        }
        public string IconRef
        {
            get => _iconRef;
            set => _iconRef = value;
        }

        public Category(string name, string description, string iconRef, string color = "#000000")
        {
            Guid = Guid.NewGuid();
            _name = name;
            _description = description;
            _iconRef = iconRef;
            _color = color;
        }
    }
}