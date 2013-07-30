using System;
using System.Globalization;

namespace TwitterManager.Models
{
    public class TwitterUserInfo
    {
        public int Id { get; set; }		
        public string IdStr { get; set; }		
        public string Name { get; set; }		
        public string ScreenName { get; set; }		
        public string Location { get; set; }		
        public string Description { get; set; }		
        public string Url { get; set; }				
        public string Urls { get; set; }		
        public string Protected { get; set; }
        public int FollowersCount { get; set; }
        public int FriendsCount { get; set; }
        public int ListedCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int FavouritesCount { get; set; }
        public int UtcOffset { get; set; }
        public string TimeZone { get; set; }
        public string GeoEnabled { get; set; }
        public string Verified { get; set; }
        public int StatusesCount { get; set; }
        public string Lang { get; set; }
        public string ContributorsEnabled { get; set; }
        public string IsTranslator { get; set; }
        public string ProfileBackgroundColor { get; set; }
        public string ProfileBackgroundImageUrl { get; set; }
        public string ProfileBackgroundImageUrlHttps { get; set; }
        public string ProfileBackgroundTile { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ProfileImageUrlHttps { get; set; }
        public string ProfileLinkColor { get; set; }
        public string ProfileSidebarBorderColor { get; set; }
        public string ProfileSidebarFillColor { get; set; }
        public string ProfileTextColor { get; set; }
        public string ProfileUseBackgroundImage { get; set; }
        public string DefaultProfile { get; set; }
        public string DefaultProfileImage { get; set; }
        public string Following { get; set; }
        public string FollowRequestSent { get; set; }
        public string Notifications { get; set; }


        public TwitterUserInfo(dynamic user)
        {
            Console.WriteLine();

            Id = user.id;
            IdStr = user.id_str;
            Name = user.name;
            ScreenName = user.screen_name;
            Location = user.location;

            string description = user.description;
            Description = description.Replace('\'','"');

            Url = user.url;
//            Protected = user.protected ;
            FollowersCount = user.followers_count ;
            FriendsCount = user.friends_count ;
            ListedCount = user.listed_count ;
            try
            {
                string temp = user.created_at;
                string[] date = temp.Split(' ');// date[] = {ddd, MMM, dd, +0000 ,hh:mm:ss, yyyy}
                string dateToParse = string.Format("{0} {1}, {2} {3}",date[1], date[2] ,date[5] ,date[3]);
                CreatedAt = DateTime.Parse(dateToParse);
                Console.WriteLine(CreatedAt);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            FavouritesCount = user.favourites_count ;

            UtcOffset = user.utc_offset.Value == null ? 0 : user.utc_offset;

            TimeZone = user.time_zone ;
            GeoEnabled = user.geo_enabled ;
            Verified = user.verified ;
            StatusesCount = user.statuses_count ;
            Lang = user.lang ;
            ContributorsEnabled = user.contributors_enabled ;
            IsTranslator = user.is_translator ;
            ProfileBackgroundColor = user.profile_background_color ;
            ProfileBackgroundImageUrl = user.profile_background_image_url ;
            ProfileBackgroundImageUrlHttps = user.profile_background_image_url_https ;
            ProfileBackgroundTile = user.profile_background_tile ;
            ProfileImageUrl = user.profile_image_url ;
            ProfileImageUrlHttps = user.profile_image_url_https ;
            ProfileLinkColor = user.profile_link_color ;
            ProfileSidebarBorderColor = user.profile_sidebar_border_color ;
            ProfileSidebarFillColor = user.profile_sidebar_fill_color ;
            ProfileTextColor = user.profile_text_color ;
            ProfileUseBackgroundImage = user.profile_use_background_image ;
            DefaultProfile = user.default_profile ;
            DefaultProfileImage = user.default_profile_image ;
            Following = user.following ;
            FollowRequestSent = user.follow_request_sent ;
            Notifications = user.notifications ;

        }
    }
}
