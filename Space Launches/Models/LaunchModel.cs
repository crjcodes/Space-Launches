using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// see https://launchlibrary.net/docs/1.4.1/api.html#launch

namespace Space_Launches.Models
{
    public class LaunchModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Net { get; set; }
        public int Tbdtime { get; set; }
        public int Tbddate { get; set; }
    }
}