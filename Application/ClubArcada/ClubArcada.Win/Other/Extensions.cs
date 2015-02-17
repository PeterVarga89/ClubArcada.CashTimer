using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace ClubArcada.Win.Other
{
    public static class Extensions
    {
        public static T Clone<T>(this T source)
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