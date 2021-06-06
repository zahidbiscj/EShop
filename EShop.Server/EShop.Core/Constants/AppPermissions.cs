using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Constants
{
    public class AppPermissions
    {
        public const string ViewWeatherForcast = "ViewWeatherForcast";
        public const string ViewPermissions = "ViewPermissions";
        public const string AddRoles = "AddRoles";
        public const string ViewRoles = "ViewRoles";
        public const string AddCategories = "AddCategories";
        public const string ViewCategories = "ViewCategories";

        public static List<string> All()
        {
            Type t = typeof(AppPermissions);

            return t.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Select(x => x.GetValue(null).ToString())
                .ToList();
        }
    }

}
