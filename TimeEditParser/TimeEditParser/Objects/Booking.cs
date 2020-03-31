using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TimeEditParser.Models
{
    public class Booking
    {
        public string classes;
        public string name;
        public string Location { get; set; }
        public string group;
        public List<List<string>> teachers = new List<List<string>>();
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Id { get; set; }

        public string Heading;
        public string Text => name;
        public string Description => Location + ", " + teachers[0][1].ToString() + " " + teachers[0][0];
        /*
        "classes", 
        "name",    
        "location",
        "group",   
        "teachers",
        "startTime"
        "endTime", 
        "id",      
         */
    }
}
