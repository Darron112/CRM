using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM1._2.Models
{
    public class RCViewModel
    {
        public RequestTable RT { get; set; }
        public ClientTable CT { get; set; }

        public IEnumerable<SelectListItem> DropDownItems { get; set; }
    }
}