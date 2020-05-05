using System;
using Xamarin.Forms;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Collections.Generic;
using TimeEditParser.Models;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TimeEditParser.Utilities;
using MessagePack;
using System.Text;

namespace TimeEditParser
{
    public class ApplicationSettings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static bool UseActiveNotification
        {
            get => AppSettings.GetValueOrDefault(nameof(UseActiveNotification), true);
            set => AppSettings.AddOrUpdateValue(nameof(UseActiveNotification), value);
        }

        public static bool EnableDarkTheme
        {
            get => AppSettings.GetValueOrDefault(nameof(EnableDarkTheme), true);
            set => AppSettings.AddOrUpdateValue(nameof(EnableDarkTheme), value);
        }

        public static bool ForceSetTheme
        {
            get => AppSettings.GetValueOrDefault(nameof(ForceSetTheme), true);
            set => AppSettings.AddOrUpdateValue(nameof(ForceSetTheme), value);
        }

        public static string LinkBase
        {
            get => AppSettings.GetValueOrDefault(nameof(LinkBase), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(LinkBase), value);
        }

        // Notifications to send before lesson start
        public static bool SendNotificationAtStart
        {
            get => AppSettings.GetValueOrDefault(nameof(SendNotificationAtStart), true);
            set => AppSettings.AddOrUpdateValue(nameof(SendNotificationAtStart), value);
        }
        public static bool SendNotificationBefore
        {
            get => AppSettings.GetValueOrDefault(nameof(SendNotificationBefore), true);
            set => AppSettings.AddOrUpdateValue(nameof(SendNotificationBefore), value);
        }

        public static int MinutesBeforeNotification
        {
            get => AppSettings.GetValueOrDefault(nameof(MinutesBeforeNotification), 0);
            set => AppSettings.AddOrUpdateValue(nameof(MinutesBeforeNotification), value);
        }
        // Notifications to send related to lesson end
        public static bool SendNotificationAtEnd
        {
            get => AppSettings.GetValueOrDefault(nameof(SendNotificationAtEnd), true);
            set => AppSettings.AddOrUpdateValue(nameof(SendNotificationAtEnd), value);
        }
        public static bool SendNotificationAfter
        {
            get => AppSettings.GetValueOrDefault(nameof(SendNotificationAfter), true);
            set => AppSettings.AddOrUpdateValue(nameof(SendNotificationAfter), value);
        }

        public static int MinutesAfterNotification
        {
            get => AppSettings.GetValueOrDefault(nameof(MinutesAfterNotification), 0);
            set => AppSettings.AddOrUpdateValue(nameof(MinutesAfterNotification), value);
        }

        public static Dictionary<string, string> LinkGroups
        {
            get
            {
                string serializedData = AppSettings.GetValueOrDefault(nameof(LinkGroups), "");
                try
                {
                    return MessagePackSerializer.Deserialize <Dictionary<string,string>> (MessagePackSerializer.ConvertFromJson(serializedData));
                } catch (Exception e)
                {
                    return new Dictionary<string, string>();
                }
            }

            set
            {
                AppSettings.AddOrUpdateValue(nameof(LinkGroups), MessagePackSerializer.SerializeToJson(value));
            }
        }

        //
        // Filter variables
        //
        public static List<string> FilterIDs
        {
            get
            {
                string serializedData = AppSettings.GetValueOrDefault(nameof(FilterIDs), "");
                XmlSerializer deserializer = new XmlSerializer(typeof(List<string>));
                using (TextReader tr = new StringReader(serializedData))
                {
                    try
                    {
                        return (List<string>)deserializer.Deserialize(tr);
                    }
                    catch
                    {
                        return new List<string>();
                    }
                }
            }

            set
            {
                XmlSerializer serializer = new XmlSerializer(FilterIDs.GetType());
                using (StringWriter sw = new StringWriter())
                {
                    serializer.Serialize(sw, value);
                    AppSettings.AddOrUpdateValue(nameof(FilterIDs), sw.ToString());
                }
            }
        }

        public static List<string> FilterNames
        {
            get
            {
                string serializedData = AppSettings.GetValueOrDefault(nameof(FilterNames), "");
                XmlSerializer deserializer = new XmlSerializer(typeof(List<string>));
                using (TextReader tr = new StringReader(serializedData))
                {
                    try
                    {
                        return (List<string>)deserializer.Deserialize(tr);
                    }
                    catch
                    {
                        return new List<string>();
                    }
                }
            }

            set
            {
                XmlSerializer serializer = new XmlSerializer(FilterNames.GetType());
                using (StringWriter sw = new StringWriter())
                {
                    serializer.Serialize(sw, value);
                    AppSettings.AddOrUpdateValue(nameof(FilterNames), sw.ToString());
                }
            }
        }

        public static bool FirstTimeUse
        {
            get => AppSettings.GetValueOrDefault(nameof(FirstTimeUse), true);
            set => AppSettings.AddOrUpdateValue(nameof(FirstTimeUse), value);
        }

        public static void ClearAllData()
        {
            AppSettings.Clear();
        }
    }
}
