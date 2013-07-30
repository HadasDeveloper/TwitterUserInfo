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
        private List<TwitterUserInfo> users = new List<TwitterUserInfo>();
        private bool finishedFlag;
        private bool finishedToProcessScreenName;
        private bool finishedToProcessPage;
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
            //tempName.ScreenName = "Starbucks";
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

                //while (!finishedFlag)
                //    Thread.Sleep(500);

                finishedFlag = false;
                finishedToProcessScreenName = false;
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
                logWriter.WriteToEventLog(string.Format("screenName = {0}, finishedToProcessScreenName = {1}, PageCount = {2}, finishedToProcessPage = {3}", screenName, finishedToProcessScreenName, pageCounter, finishedToProcessPage));

                pageCounter++;
                finishedToProcessPage = true;

                OAuthTwitterWrapper.OAuthTwitterWrapper oAuthT = new OAuthTwitterWrapper.OAuthTwitterWrapper();

                showUserUrl = string.Format(ShowUserFormat, screenName);

                dynamic info = oAuthT.GetMyTimeline(showUserUrl);

                if(info == null)
                    return;
                //TwitterUserInfo items = new TwitterUserInfo(info);
                users.Add(new TwitterUserInfo(info));

                finishedToProcessScreenName = true;
                //DataContext.InsertTwitterItems(users);
                authenticateMessageCounter++;

                if (authenticateMessageCounter > 100)
                //if (authenticateMessageCounter > 10)
                {
                    Console.WriteLine(" authenticate Message Counter = " + authenticateMessageCounter);
                    done = true;
                }

            finishedFlag = true;
            logWriter.WriteToEventLog(string.Format("finished readind for {0}", screenName));
        }

    }
}
