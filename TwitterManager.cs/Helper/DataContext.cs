using System.Collections.Generic;
using System.Data;
using TwitterManager.Models;

namespace TwitterManager.Helper
{
    public class DataContext
    {
        public static bool FinishedFlag = false;

        public static void InsertTwitterUser(List<TwitterUserInfo> items)
        {
            DataHelper.InsertTwitterUser(items);
            FinishedFlag = true;
        }

        public static void UpdateScreenNames_finished(string screenName)
        {
            DataHelper.UpdateScreenNames_finished(screenName);
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

