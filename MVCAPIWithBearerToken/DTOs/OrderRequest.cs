using MVCAPIWithBearerToken.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCAPIWithBearerToken.DTOs
{
    public class OrderRequest
    {
        public Order Order { get; set; }
        public byte[] ImageFile { get; set; }
        public string ImageFilename { get; set; }
    }
}