using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTA_Tracker_Client
{
    internal static class Helpers
    {

        public static string FullRouteName(string RouteName)
        {
            switch (RouteName)
            {
                case "Red":
                case "Blue":
                case "Pink":
                    return RouteName;
                case "Brn":
                    return "Brown";
                case "G":
                    return "Green";
                case "Org":
                    return "Orange";
                case "P":
                    return "Purple";
                case "Pexp":
                    return "PurpleExp";
                case "Y":
                    return "Yellow";
                default:
                    return "UNK";
            }
        }

        public static RouteColor RouteColor(string routeColor)
        {
            if (Enum.TryParse(typeof(RouteColor), routeColor, out var ret))
            {
                return (RouteColor)ret;
            }
            else
                throw new ArgumentException("Invalid Route Color Input");
        }

        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
