using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class Statistics
    {
        public float Min { get; set; }
        public float Max { get; set; }
        public float Mean { get; set; }
        public float Median { get; set; }
        public float StdDev { get; set; }
    }
}
