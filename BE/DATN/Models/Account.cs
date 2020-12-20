using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
