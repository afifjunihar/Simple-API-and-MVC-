using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class MsUser
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool? IsActive { get; set; }
    }
}
