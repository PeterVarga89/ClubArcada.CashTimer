using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClubArcada.Win.Other
{
    public static class Extensions
    {
        public static void Raise(this PropertyChangedEventHandler helper, object thing, string name)
        {
            if (helper != null)
            {
                helper(thing, new PropertyChangedEventArgs(name));
            }
        }

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