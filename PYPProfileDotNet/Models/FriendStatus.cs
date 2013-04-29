using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
﻿using System.ComponentModel.DataAnnotations;

namespace PYPProfileDotNet.Models
{
    public class FriendStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string Status { get; set; }
    }
}
