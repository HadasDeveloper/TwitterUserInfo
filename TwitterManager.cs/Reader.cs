using System;
using System.Collections.Generic;
using System.Threading;
using Logger;
using oAuthTwitterWrapper;
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
        private OAuthData oAuthData = new OAuthData();
        private const string ShowUserFormat = "https://api.twitter.com/1.1/users/show.json?screen_name={0}";
        
        public void Read()
        {
            authenticateMessageCounter = 1;
          
            //This function will get the twitter accounts from the db
            List<ScreenNameToLoad> screenNamesToLoad = DataContext.GetScreenNames();

            //This function will get the twitter authentication data for this host name from the db
            oAuthData = DataContext.GetoAuthData();

            //-----------for debuging
            //List<ScreenNameToLoad> screenNamesToLoad = new List<ScreenNameToLoad>();
            //ScreenNameToLoad tempName = new ScreenNameToLoad();
            //tempName.ScreenName = "zite";
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
           Console.WriteLine(string.Format("Inserting to DB {0} users ",users.Count));
           
           DataContext.InsertTwitterUser(users);

            while (!DataContext.FinishedFlag)
                Thread.Sleep(500);

            Console.WriteLine("Done");
        }

        public void GetUserInfo(string screenName)
        {
                pageCounter++;
 
                OAuthTwitterWrapper.OAuthTwitterWrapper oAuthT = new OAuthTwitterWrapper.OAuthTwitterWrapper();

                showUserUrl = string.Format(ShowUserFormat, screenName);

                TwitterData info = oAuthT.GetMyTimeline(showUserUrl, oAuthData);
                
                if (info.error !=null)
                {
                    DataContext.InsertTwitterError(info.error, screenName);
                            return;
                }

                if(info.json == null)
                    return;
                users.Add(new TwitterUserInfo(info.json));

                authenticateMessageCounter++;

                if (authenticateMessageCounter > 100)
                {
                    Console.WriteLine(" authenticate Message Counter = " + authenticateMessageCounter);
                    done = true;
                }
        }

    }
}
