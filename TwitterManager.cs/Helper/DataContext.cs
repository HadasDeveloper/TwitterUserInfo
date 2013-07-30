using System.Collections.Generic;
using System.Data;
using TwitterManager.Models;

namespace TwitterManager.Helper
{
    public class DataContext
    {
        public static bool finishedFlag = false;

        public static void InsertTwitterUser(List<TwitterUserInfo> items)
        {
            DataHelper.InsertTwitterUser(items);
            finishedFlag = true;
        }

        public static void InsertTwitterItems(List<TwitterUserInfo> items)
        {
            DataHelper.InsertTwitterItems(items);
        }
        
        public static void InsertRowsUpdateLog(string hostName, int numofrowes)
        {
            DataHelper.InsertRowsUpdateLog(hostName, numofrowes);
        }

        public static string GetMinMessageIdForUser(string screenName)
        {
            DataTable table = DataHelper.GetMinMessageIdForUser(screenName);
            string min = table.Rows.Count > 0 ? table.Rows[0].Field<string>("minId") : "";
            return min;
        }
        
        public static string GetMaxMessageIdForUser(string screenName)
        {
            DataTable table = DataHelper.GetMaxMessageIdForUser(screenName);
            string max = table.Rows.Count > 0 ? table.Rows[0].Field<string>("maxId") : "";
            return max;
        }
        
        public static void UpdateScreenNames_LastUpdated(string screenName)
        {
            DataHelper.UpdateScreenNames_LastUpdated(screenName);
        }
       
        public static void UpdateScreenNames_Deactivate(string screenName)
        {
            DataHelper.UpdateScreenNames_Deactivate(screenName);
        }
        
        public static List<ScreenNameToLoad> GetScreenNames()
        {
            DataTable table = DataHelper.GetScreenNames();
            List<ScreenNameToLoad> names = new List<ScreenNameToLoad>();

            foreach (DataRow row in table.Rows)
            {
                ScreenNameToLoad name = new ScreenNameToLoad
                                            {
                                                ScreenName = row.Field<string>("screenname")  
                                            };
                names.Add(name);
            }

            return names;
        }
       
    }
}

