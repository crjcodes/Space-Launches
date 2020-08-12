using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Space_Launches.Models
{
    public class LaunchCollectionModel
    {
        // formerly "Launch[]"
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public LaunchModel[] Results { get; set; }

        public int CurrentCount() {
            return Results.Length;                       
        }
    }
}