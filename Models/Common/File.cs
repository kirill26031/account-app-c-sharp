using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.WalletAppWPF.Models.Common
{
    public enum fileType
    {
        Image,
        Text
    }

    public class File
    {
        public File(fileType typeOfFile, string fileLocation)
        {
            FileType = typeOfFile;
            FileLocation = fileLocation;
        }
        public File(File Other)
        {
            FileType = Other.FileType;
            FileLocation = Other.FileLocation;
        }

        public fileType FileType { get; private set; }
        public string FileLocation { get; private set; }

    }
}