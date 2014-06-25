using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubArcada.Win.Other
{
    public static class Extensions
    {
        public static void Raise(this PropertyChangedEventHandler helper, object thing, string name)
        {
            if(helper != null)
            {
                helper(thing, new PropertyChangedEventArgs(name));
            }
        }
    }
}
