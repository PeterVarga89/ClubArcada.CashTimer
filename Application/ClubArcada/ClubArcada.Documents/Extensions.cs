using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubArcada.Documents
{
    internal static class Extensions
    {
        public static MemoryStream ToMemoryStream(this byte[] byteArray)
        {
            return new MemoryStream(byteArray);
        }
    }
}
