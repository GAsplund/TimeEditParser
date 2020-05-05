using System;
using System.Collections;
using Xamarin.Forms;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using TimeEditParser;
using System.Threading.Tasks;
using TimeEditParser.Models;

namespace TimeEditParser
{
    public class ScheduleParser
    {
        public static List<Week> ListWeek;


        // Gets the schedule, depending on day
        public static List<Week> GetSchedule()
        {
            // Initiate variables
            dynamic doc;
            var web = new HtmlWeb();
            // Get the web url asyncronously
            doc =  Task.Run(() => web.Load(Utilities.ScheduleSearch.ScheduleLink(ApplicationSettings.LinkBase))).GetAwaiter().GetResult();
            // Return the ParseSchedule function asyncronously
            return Task.Run(() => ParseSchedule(doc)).GetAwaiter().GetResult();
        }

        // Use the HtmlDocument to get the schedule data
        public static List<Week> ParseSchedule(dynamic doc)
        {
            // Get all HTML div nodes with the class "weekDiv"
            doc = doc.DocumentNode.SelectNodes(".//div[@class='weekDay']");

            // Create a 2 dimensional list of dictionaries (1st is weekday, 2nd is lesson)
            List<Day> weekSchedule = new List<Day>();

            foreach (HtmlNode weekNode in doc)
            {
                string dayDateString = weekNode.SelectSingleNode(".//div[contains(@class, 'headlinebottom2')]").InnerText.Replace("&nbsp;", " ").Trim().Substring(3);
                DateTime dayDateTime = DateTime.Parse(dayDateString);

                Day currentDaySchedule = new Day(dayDateTime);
                // Check if the weekNode contains a bookingDiv, add empty currentDaySchedule if null and continue.
                if(weekNode.SelectSingleNode(".//div[contains(@class, 'bookingDiv')]") == null)
                {
                    weekSchedule.Add(currentDaySchedule);
                    continue;
                }

                foreach (HtmlNode node in weekNode.SelectNodes(".//div[contains(@class, 'bookingDiv')]"))
                {
                    string lessonId = node.Attributes["data-id"].Value;

                    // Initiate empty variables
                    ArrayList cols = new ArrayList();
                    List<List<string>> teachers = new List<List<string>>();

                    // Add the booking div nodes to a list of bookings
                    foreach (HtmlNode BookingNode in node.SelectNodes(".//div[contains(@class, 'col')]"))
                    {
                        cols.Add(BookingNode.InnerText);
                    }
                    // The amount of entries are incomplete. Do not add it.
                    if (cols.Count < 5) { continue; }

                    // Check if lesson is filtered
                    if (ApplicationSettings.FilterNames.Contains(cols[1])) continue;
                    else if (ApplicationSettings.FilterIDs.Contains(lessonId)) continue;

                    // Create a list of teachers in the lesson
                    List<string> teacherslistsplit = Convert.ToString(cols[2]).Split(',').ToList();
                    for (int i = 0; i < teacherslistsplit.Count(); i += 2)
                    {
                        teachers.Add(teacherslistsplit.GetRange(i, Math.Min(2, teacherslistsplit.Count - i)));
                    }

                    string[] timesplit = node.Attributes["title"].Value.Split(',')[0].Split(' ');
                    string startTime = timesplit[2], endTime = timesplit[4];

                    // Create a formatted dictionary with the lesson information
                    Booking booking = new Booking
                    {
                        classes =   cols[0].ToString(),
                        name =      cols[1].ToString(),
                        Location =  cols[3].ToString(),
                        group =     cols[4].ToString(),
                        teachers =  teachers,
                        StartTime = startTime,
                        EndTime =   endTime,
                        Id =        lessonId
                    };
                    currentDaySchedule.Add(booking);
                }
                // Finally, add the list of lessons to the weekSchedule before next iteration
                weekSchedule.Add(currentDaySchedule);
            }

            // Return the list, **split with weeks**
            List<Week> weekList = new List<Week>();
            for (int i = 0; i < weekSchedule.Count; i += 5)
            {
                Week currentWeek = new Week();
                foreach(Day currentday in weekSchedule.GetRange(i, Math.Min(5, weekSchedule.Count - i)))
                    currentWeek.Add(currentday);

                weekList.Add(currentWeek);
            }

            return weekList;
            //return SplitList(weekSchedule, 5);
        }

        public static List<Week> SetSchedule()
        {
            List<Week> scheduleWeeks = GetSchedule();
            ListWeek = scheduleWeeks;
            return ListWeek;
        }

        public static int TodayIndex()
        {
            return ((int)DateTime.Now.DayOfWeek == 0) ? 7 : (int)DateTime.Now.DayOfWeek;
            //return 1;
        }

    }
}
