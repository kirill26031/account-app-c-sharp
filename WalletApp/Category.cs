using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp
{
    public class Category
    {
        Guid _Id;
        string _Name;
        string _Description;
        string _Color;
        string _IconRef;

        public Guid Id { 
            get => _Id; 
            private set => _Id = value; 
        }
        public string Name { 
            get => _Name; 
            private set => _Name = value; 
        }
        public string Description { 
            get => _Description; 
            private set => _Description = value; 
        }
        public string Color { 
            get => _Color; 
            private set => _Color = value; 
        }
        public string IconRef { 
            get => _IconRef; 
            private set => _IconRef = value; 
        }

        public Category(string name, string desciption, string iconRef, string color = "#000000")
        {
            _Id = Guid.NewGuid();
            _Name = name;
            _Description = desciption;
            _IconRef = iconRef;
            _Color = color;
        }
    }
}
