using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClubArcada.BusinessObjects
{
    internal static class Extensions
    {
        public static void Raise(this PropertyChangedEventHandler helper, object thing, string name)
        {
            if (helper != null)
            {
                helper(thing, new PropertyChangedEventArgs(name));
            }
        }

        public static string RemoveDiacritics(this string text)
        {
            text = text.Replace("á", "a");
            text = text.Replace("é", "e");
            text = text.Replace("í", "i");
            text = text.Replace("ó", "o");
            text = text.Replace("š", "s");
            text = text.Replace("ž", "z");
            text = text.Replace("č", "c");

            return text;
        }

        public static T Copy<T>(this T source)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)serializer.ReadObject(ms);
            }
        }

        
    }
}
