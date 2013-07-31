using System;
using System.Collections.Generic;
using System.Threading;
using Logger;
using TwitterManager.Helper;
using TwitterManager.Models;

namespace TwitterManager
{
    public class Reader
    {
        private readonly List<TwitterUserInfo> users = new List<TwitterUserInfo>();
 
        private int pageCounter;
        private int authenticateMessageCounter;
        private bool done;
        private string showUserUrl;
        private const string ShowUserFormat = "https://api.twitter.com/1.1/users/show.json?screen_name={0}";

        private EventLogWriter logWriter = new EventLogWriter("TwitterManager");

        public void Read()
        {
            authenticateMessageCounter = 1;
          
            //This function will get the twitter accounts from the db
            List<ScreenNameToLoad> screenNamesToLoad = DataContext.GetScreenNames();

            //-----------for debuging
            //List<ScreenNameToLoad> screenNamesToLoad = new List<ScreenNameToLoad>();
            //ScreenNameToLoad tempName = new ScreenNameToLoad();
            //tempName.ScreenName = "a_bh_a";
            //screenNamesToLoad.Add(tempName);
            //-----------

            Console.WriteLine(string.Format("Number of screen Names To Load = {0}", screenNamesToLoad.Count));

            if (screenNamesToLoad.Count == 0)
                return;

            int i=0;
            foreach (var screenNameToLoad in screenNamesToLoad)
            {
                Console.WriteLine( ++i +"). reading info for : "+screenNameToLoad.ScreenName);
                if (done)
                    break;

                GetUserInfo(screenNameToLoad.ScreenName);
                pageCounter = 0;
            }
            Console.WriteLine("Inserting to DB");
            DataContext.InsertTwitterUser(users);

            while (!DataContext.finishedFlag)
                Thread.Sleep(500);

            Console.WriteLine("Done");
        }

        public void GetUserInfo(string screenName)
        {
                pageCounter++;
 
                OAuthTwitterWrapper.OAuthTwitterWrapper oAuthT = new OAuthTwitterWrapper.OAuthTwitterWrapper();

                showUserUrl = string.Format(ShowUserFormat, screenName);

                dynamic info = oAuthT.GetMyTimeline(showUserUrl);

                if(info == null)
                    return;
                users.Add(new TwitterUserInfo(info));

                authenticateMessageCounter++;

                if (authenticateMessageCounter > 1)
                {
                    Console.WriteLine(" authenticate Message Counter = " + authenticateMessageCounter);
                    done = true;
                }
        }

    }
}
