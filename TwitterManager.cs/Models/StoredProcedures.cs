
namespace TwitterManager.Models
{
    public class StoredProcedures
    {
        public const string SqlGetScreenNames = "usp_TService_get_screen_names_for_info_update";
        public const string SqlGetoAuthData = "usp_TService_getOAuthTokens '{0}'";
        public const string SqlInsertTwitterError = "update TService_UsersInfoTracking set TwitterError = '{0}' where screenname = '{1}'";        
        public const string SqlUpdateScreenNames_finished = "update TService_UsersInfoTracking set finish = 'true' where screenname = '{0}'";
        public const string SqlInsertTwitterUser = "insert into TService_UserInfo values({0},	'{1}',	'{2}',	'{3}',	'{4}',	'{5}',	'{6}',	'{7}',	'{8}',	{9},"
                                                                                        + "{10},	{11},	'{12: yyyy/MM/dd hh:mm:ss}',	{13},	'{14}',	'{15}',	'{16}',	'{17}',	{18},	'{19}',	"
                                                                                        +"'{20}',	'{21}',	'{22}',	'{23}',	'{24}',	'{25}',	'{26}',	'{27}',	'{28}',	'{29}',"
                                                                                        +"'{30}',	'{31}',	'{32}',	'{33}',	'{34}',	'{35}',	'{36}',	'{37}')";
        
    }
}
