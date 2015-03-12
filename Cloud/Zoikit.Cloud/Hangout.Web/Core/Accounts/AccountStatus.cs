using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Accounts
{
    public enum AccountStatus
    {
        Blocked,Updated,AccountCreated,Error,Activated,LoggedIn,LogInFailed,AlreadyRegistered,UsernameInvalid,AppInvalid
    }
}