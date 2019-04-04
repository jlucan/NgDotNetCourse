using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Lname { get; set; }
        public string Fname { get; set; }
        public string Mname { get; set; }
        public byte[] PswdHash { get; set; }
        public byte[] PswdSalt { get; set; }
    }
}
