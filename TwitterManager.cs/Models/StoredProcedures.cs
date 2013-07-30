
namespace TwitterManager.Models
{
    public class StoredProcedures
    {
        public const string SqlInsertTwitterItems = "usp_TService_insert_twitter_item";
        public const string SqlGetMinMessageIdForUser = "select convert(nvarchar(18), min(messageid)) minId from TService_rowdata where ScreenName = '{0}'";
        public const string SqlGetMaxMessageIdForUser = "select convert(nvarchar(18), max(messageid)) maxId from TService_rowdata where ScreenName = '{0}'";
        //public const string SqlGetScreenNames = "usp_TService_get_screen_names";
        public const string SqlGetScreenNames= "select top 100 screenName from TService_ScreenNamesToLoad where screenName not in (select screenName from [TService_UserInfo])";
        public const string SqlUpdateScreenNames_LastUpdated = "update TService_ScreenNamesToLoad set lastupdated = getdate() where screenname = '{0}'";
        public const string SqlUpdateScreenNames_Deactivate = "update TService_ScreenNamesToLoad set lastupdated = getdate(), active = 0 where screenname = '{0}'";
        public const string SqlInsertRowsUpdateLog = "insert into tservice_rowsupdatelog(hostName,updatedate,numofrowes)values('{0}',GETDATE( ),{1})";
        public const string SqlInsertTwitterUser = "insert into TService_UserInfo values({0},	'{1}',	'{2}',	'{3}',	'{4}',	'{5}',	'{6}',	'{7}',	'{8}',	{9},"
                                                                                        + "{10},	{11},	'{12: yyyy/MM/dd hh:mm:ss}',	{13},	'{14}',	'{15}',	'{16}',	'{17}',	{18},	'{19}',	"
                                                                                        +"'{20}',	'{21}',	'{22}',	'{23}',	'{24}',	'{25}',	'{26}',	'{27}',	'{28}',	'{29}',"
                                                                                        +"'{30}',	'{31}',	'{32}',	'{33}',	'{34}',	'{35}',	'{36}',	'{37}')";
    }
}
