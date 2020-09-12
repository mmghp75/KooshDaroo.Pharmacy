using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KooshDaroo.Models
{
    public class PrescribeItem
    {
        public int id { get; set; }
        public int PrescribeId { get; set; }
        public string DrugName { get; set; }
        public int CountOf { get; set; }
    }
}