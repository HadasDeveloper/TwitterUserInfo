using System;
using System.Collections.Generic;
using System.Data;
using oAuthTwitterWrapper;
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

        public static void InsertTwitterError(string error, string screenName)
        {
            DataHelper.InsertTwitterError(error, screenName);
        }

        public static void UpdateScreenNames_finished(string screenName)
        {
            DataHelper.UpdateScreenNames_finished(screenName);
        }

        public static OAuthData GetoAuthData()
        {
            DataTable table = DataHelper.GetoAuthData(Environment.MachineName);
            OAuthData data = new OAuthData();

            data.OAuthConsumerKey = table.Rows[0].Field<string>("ConsumerKey");
            data.OAuthConsumerSecret = table.Rows[0].Field<string>("ConsumerSecret");

            return data;

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

