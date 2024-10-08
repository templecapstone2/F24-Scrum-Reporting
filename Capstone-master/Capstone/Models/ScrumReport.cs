using System;
using System.Collections.Generic;

namespace Capstone.Models
{
    public class ScrumItem
    {
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsSubmitted { get; set; }
    }

    public class ScrumReport
    {
        public List<ScrumItem> ScrumItems { get; set; } = new List<ScrumItem>();
    }
}
