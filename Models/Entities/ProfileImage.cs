using System;
using System.Collections.Generic;

namespace ChatApp.Models.Entities
{
    public partial class ProfileImage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public byte[] ProfileImage1 { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
