using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangout.Web.Core.Text
{
    class UserDistint : IEqualityComparer<Model.Text>
    {
        public bool Equals(Model.Text x, Model.Text y)
        {
            

            if (x.UserFrom == y.UserFrom&&x.UserTo==y.UserTo)
            {
                return true;
            }

            if (x.UserFrom == y.UserTo && x.UserTo == y.UserFrom)
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(Model.Text obj)
        {
            return obj.UserFrom.GetHashCode() + obj.UserTo.GetHashCode();
        }
    }
}
