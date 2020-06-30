using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Space_Launches.Models
{
    public class LaunchCollectionModel
    {
        // formerly "Launch[]"
        public LaunchModel[] Launches { get; set; }
        public int Total { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }

    }
}