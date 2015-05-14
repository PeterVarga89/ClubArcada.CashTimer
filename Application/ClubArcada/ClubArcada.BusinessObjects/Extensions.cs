using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClubArcada.BusinessObjects
{
    public static class Extensions
    {
        public static void Raise<T>(this PropertyChangedEventHandler handler, Expression<Func<T>> propertyExpression)
        {
            if (handler != null)
            {
                var body = propertyExpression.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("'propertyExpression' should be a member expression");

                var expression = body.Expression as ConstantExpression;
                if (expression == null)
                    throw new ArgumentException("'propertyExpression' body should be a constant expression");

                object target = Expression.Lambda(expression).Compile().DynamicInvoke();

                var e = new PropertyChangedEventArgs(body.Member.Name);
                handler(target, e);
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

        public static double ToAbs(this double val)
        {
            return Math.Abs(val);
        }

        public static double ToAbs(this double? val)
        {
            if (!val.HasValue)
                return 0;

            return Math.Abs(val.Value);
        }

        public static double ToAbs(this int val)
        {
            return Math.Abs(val);
        }

        public static double ToAbs(this int? val)
        {
            if (!val.HasValue)
                return 0;

            return Math.Abs(val.Value);
        }

        public static eTransactionType GetOposite(this eTransactionType value)
        {
            if (value == eTransactionType.Bar)
            {
                return eTransactionType.BarReturned;
            }
            else if (value == eTransactionType.CashGame)
            {
                return eTransactionType.CashGameReturned;
            }
            else if (value == eTransactionType.NotSet)
            {
                return eTransactionType.Returned;
            }
            else if (value == eTransactionType.Tournament)
            {
                return eTransactionType.TournamentReturned;
            }
            else if (value == eTransactionType.Bonus)
            {
                return eTransactionType.BonusReturned;
            }
            else
            {
                throw new NotImplementedException("Must be (-)");
            }
        }
    }
}
