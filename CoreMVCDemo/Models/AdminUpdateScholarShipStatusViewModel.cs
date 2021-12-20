using System;
using System.Collections.Generic;

namespace CoreMVCDemo.Models
{
    public class AdminUpdateScholarShipStatusViewModel
    {
        public List<string> ApprovedStatusIds { get; set; }
        public List<string> RejectedStatusIds { get; set; }
    }
}
