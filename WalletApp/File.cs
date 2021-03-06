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
        public File(FileType fileType, string fileLocation)
        {
            FileType = fileType;
            FileLocation = fileLocation;
        }
        public File(File Other)
        { 
            FileType = Other.FileType;
            FileLocation = Other.FileLocation;
        }

        public FileType FileType { get; private set; }
        public string FileLocation { get; private set; }

    }
}
