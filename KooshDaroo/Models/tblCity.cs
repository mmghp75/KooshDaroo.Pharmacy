using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace KooshDaroo.Models
{
    public class tblCity
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
