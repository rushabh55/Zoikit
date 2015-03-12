using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Objects.Category
{
    public class UserCategory
    {
        public Category Category { get; set; }
        public bool? Following { get; set; }
    }
}