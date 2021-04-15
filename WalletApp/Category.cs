using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp
{
    public class Category
    {
        Guid _id;
        string _name;
        string _description;
        string _color;
        string _iconRef;

        public Guid Id
        {
            get => _id;
            private set => _id = value;
        }
        public string Name
        {
            get => _name;
            private set => _name = value;
        }
        public string Description
        {
            get => _description;
            private set => _description = value;
        }
        public string Color
        {
            get => _color;
            private set => _color = value;
        }
        public string IconRef
        {
            get => _iconRef;
            private set => _iconRef = value;
        }

        public Category(string name, string desciption, string iconRef, string color = "#000000")
        {
            _id = Guid.NewGuid();
            _name = name;
            _description = desciption;
            _iconRef = iconRef;
            _color = color;
        }
    }
}