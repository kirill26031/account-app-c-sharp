using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp
{
    public enum FileType
    {
        Image,
        Text
    }

    public class File
    {
        public File(File Other)
        { 
            FileType = Other.FileType;
            FileLocation = Other.FileLocation;
        }

        public FileType FileType { get; set; }
        public string FileLocation { get; set; }

    }
}
