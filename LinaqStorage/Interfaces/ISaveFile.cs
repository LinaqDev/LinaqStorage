using System;
using System.Collections.Generic;
using System.Text;

namespace LinaqStorage.Interfaces
{
    public interface ISaveFile
    {
        void SaveFile(byte[] file, string fileName);
    }
}
