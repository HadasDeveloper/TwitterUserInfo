using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TwitterManager.Models;
using Logger;
namespace TwitterManager.Helper
{
    public class DataHelper
    {
        //private const string ConnectionString = " Data Source=tcp:esql2k801.discountasp.net;Initial Catalog=SQL2008_856748_ntrlabs;User ID=SQL2008_856748_ntrlabs_user;Password=bbking;connection timeout=3600";
        //private const string DefaultDB = "SQL2008_856748_ntrlabs";
        private const string ConnectionString = " Data Source=SERVER02\\SQLSERVER;Initial Catalog=Dev;User ID=DevUser;Password=m4ffCr113P3vqOGGtuTW;connection timeout=3600";
        private const string DefaultDB = "Dev";

        [ThreadStatic]
        private static SqlConnection _connection;
        private static bool _isConnected;

        private static EventLogWriter logWriter = new EventLogWriter("TwitterManager");

        public static bool IsConnected
        {
            get { return _isConnected; }
        }

        public static void Connect(string initialCatalog)
        {
            if (_connection != null)
                if (_connection.State == ConnectionState.Open) return;

            if (_connection != null && _connection.State == ConnectionState.Connecting)
            {
                return;
            }

            lock (new object())
            {
                _connection = new SqlConnection { ConnectionString = ConnectionString };

                if (_connection.State != ConnectionState.Open)
                {
                    try
                    {
                        _connection.Open();
                        _isConnected = true;
                    }
                    catch (Exception e)
                    {
                        logWriter.WriteErrorToEventLog(string.Format("DataHelper.Connect: {0}", e.Message));           
                        if (_connection.State != ConnectionState.Open)
                            _isConnected = false;
                    }
                }
            }
        }

        public static void Disconnect()
        {
            if (_isConnected)
            {
                try
                {
                    _connection.Close();
                    _isConnected = false;
                }
                catch (Exception e)
                {
                    logWriter.WriteErrorToEventLog(string.Format("DataHelper.Disconnect: {0}", e.Message));   
                    if (_connection.State != ConnectionState.Open)
                        _isConnected = false;
                }
                finally
                {
                    _connection = null;
                }
            }
        }

        private static SqlConnection GetConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
                Connect(DefaultDB);

            return _connection;
        }

        public static void InsertTwitterUser(List<TwitterUserInfo> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                Console.WriteLine(++i + "). insert to db:" + item.Name);
                string executeSQLString = string.Format(StoredProcedures.SqlInsertTwitterUser, item.Id, item.IdStr,
                                                        item.Name, item.ScreenName, item.Location,
                                                        item.Description, item.Url, item.Urls, item.Protected,
                                                        item.FollowersCount, item.FriendsCount,
                                                        item.ListedCount, item.CreatedAt, item.FavouritesCount,
                                                        item.UtcOffset, item.TimeZone, item.GeoEnabled,
                                                        item.Verified, item.StatusesCount, item.Lang,
                                                        item.ContributorsEnabled, item.IsTranslator,
                                                        item.ProfileBackgroundColor,
                                                        item.ProfileBackgroundImageUrl,
                                                        item.ProfileBackgroundImageUrlHttps, item.ProfileBackgroundTile,
                                                        item.ProfileImageUrl, item.ProfileImageUrlHttps,
                                                        item.ProfileLinkColor, item.ProfileSidebarBorderColor,
                                                        item.ProfileSidebarFillColor, item.ProfileTextColor,
                                                        item.ProfileUseBackgroundImage, item.DefaultProfile,
                                                        item.DefaultProfileImage, item.Following, item.FollowRequestSent,
                                                        item.Notifications);
                executeSQLString.Replace("\'\'", "null");

                ExecuteSQL(executeSQLString);

                //set finish
                executeSQLString = string.Format(StoredProcedures.SqlUpdateScreenNames_finished, item.ScreenName);
                ExecuteSQL(executeSQLString);

            }
        }

        public static void InsertTwitterItems(List<TwitterUserInfo> items)
        {
            DataTable table = new DataTable();
            table.Columns.Add("id");
            table.Columns.Add("id_str");
            table.Columns.Add("name");
            table.Columns.Add("screen_name");
            table.Columns.Add("location");
            table.Columns.Add("description");
            table.Columns.Add("url");
            table.Columns.Add("Protected");
            table.Columns.Add("followers_count");
            table.Columns.Add("friends_count");
            table.Columns.Add("listed_count");
            table.Columns.Add("created_at", typeof(DateTime));
            table.Columns.Add("favourites_count");
            table.Columns.Add("utc_offset");
            table.Columns.Add("time_zone");
            table.Columns.Add("geo_enabled");
            table.Columns.Add("verified");
            table.Columns.Add("statuses_count");
            table.Columns.Add("lang");
            table.Columns.Add("contributors_enabled");
            table.Columns.Add("is_translator");
            table.Columns.Add("profile_background_color");
            table.Columns.Add("profile_background_image_url");
            table.Columns.Add("profile_background_image_url_https");
            table.Columns.Add("profile_background_tile");
            table.Columns.Add("profile_image_url");
            table.Columns.Add("profile_image_url_https");
            table.Columns.Add("profile_link_color");
            table.Columns.Add("profile_sidebar_border_color");
            table.Columns.Add("profile_sidebar_fill_color");
            table.Columns.Add("profile_text_color");
            table.Columns.Add("profile_use_background_image");
            table.Columns.Add("default_profile");
            table.Columns.Add("default_profile_image");
            table.Columns.Add("following");
            table.Columns.Add("follow_request_sent");
            table.Columns.Add("notifications");

            //table.Columns.Add("UploadedAt", typeof(DateTime));

            if (items.Count == 0)
                return;

            foreach (var item in items)
                table.Rows.Add(item.Id,
                                item.IdStr,
                                item.Name,
                                item.ScreenName,
                                item.Location,
                                item.Description,
                                item.Url,
                                item.Urls,
                                item.Protected,
                                item.FollowersCount,
                                item.FriendsCount,
                                item.ListedCount,
                                item.CreatedAt,
                                item.FavouritesCount,
                                item.UtcOffset,
                                item.TimeZone,
                                item.GeoEnabled,
                                item.Verified,
                                item.StatusesCount,
                                item.Lang,
                                item.ContributorsEnabled,
                                item.IsTranslator,
                                item.ProfileBackgroundColor,
                                item.ProfileBackgroundImageUrl,
                                item.ProfileBackgroundImageUrlHttps,
                                item.ProfileBackgroundTile,
                                item.ProfileImageUrl,
                                item.ProfileImageUrlHttps,
                                item.ProfileLinkColor,
                                item.ProfileSidebarBorderColor,
                                item.ProfileSidebarFillColor,
                                item.ProfileTextColor,
                                item.ProfileUseBackgroundImage,
                                item.DefaultProfile,
                                item.DefaultProfileImage,
                                item.Following,
                                item.FollowRequestSent,
                                item.Notifications);


            // Configure the SqlCommand and SqlParameter.
            SqlCommand insertCommand = new SqlCommand(StoredProcedures.SqlInsertTwitterItems, GetConnection()) { CommandType = CommandType.StoredProcedure, CommandTimeout = 3600};

            SqlParameter tvpParam = insertCommand.Parameters.AddWithValue("@TPV_TwitterItem", table);
            tvpParam.SqlDbType = SqlDbType.Structured;

            // Execute the command.
            try
            {
                insertCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logWriter.WriteErrorToEventLog(string.Format("DataHelper.InsertTwitterItems: {0}", e.Message));    
            }
        }

        public static void InsertRowsUpdateLog(string hostName, int numOfRowes)
        {
            ExecuteSQL(string.Format(StoredProcedures.SqlInsertRowsUpdateLog,  hostName, numOfRowes));
        }

        public static void UpdateScreenNames_finished(string screenName)
        {
            ExecuteSQL(string.Format(StoredProcedures.SqlUpdateScreenNames_finished, screenName));
        }
        
        public static void UpdateScreenNames_LastUpdated(string screenName)
        {
            ExecuteSQL(string.Format(StoredProcedures.SqlUpdateScreenNames_LastUpdated, screenName));
        }
       
        public static void UpdateScreenNames_Deactivate(string screenName)
        {
            ExecuteSQL(string.Format(StoredProcedures.SqlUpdateScreenNames_Deactivate, screenName));
        }
        
        public static DataTable GetMinMessageIdForUser(string screenName)
        {
            return ExecuteSqlForData(string.Format(StoredProcedures.SqlGetMinMessageIdForUser, screenName)) ?? new DataTable();
        }
        
        public static DataTable GetMaxMessageIdForUser(string screenName)
        {
            return ExecuteSqlForData(string.Format(StoredProcedures.SqlGetMaxMessageIdForUser, screenName)) ?? new DataTable();
        }
          
        public static DataTable GetScreenNames()
        {
            return ExecuteSqlForData(string.Format(StoredProcedures.SqlGetScreenNames)) ?? new DataTable();
        }
        
        public static bool ExecuteSQL(string sql)
        {
            return ExecuteSQL(sql, CommandType.Text, null);
        }

        public static bool ExecuteSQL(string sql, CommandType commandType, List<SqlParameter> parameters)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand(sql, GetConnection()) { CommandType = commandType };

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var sqlParameter in parameters)
                    {
                        command.Parameters.Add(sqlParameter);
                    }
                }

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logWriter.WriteErrorToEventLog(string.Format("DataHelper.ExecuteSQL: {0}", e.Message));   
                return false;
            }
            return true;
        }

        public static DataTable ExecuteSqlForData(string sql)
        {
            return ExecuteSqlForData(sql, CommandType.Text, null);
        }

        public static DataTable ExecuteSqlForData(string sql, CommandType commandType, List<SqlParameter> parameters)
        {
            DataTable result = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            if (IsConnected && _connection != null)
                _connection.Close();

            try
            {
                SqlConnection con = GetConnection();
                if (con.State != ConnectionState.Open)
                    return new DataTable();

                command = new SqlCommand(sql, con) { CommandType = commandType };


                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var sqlParameter in parameters)
                    {
                        command.Parameters.Add(sqlParameter);
                    }
                }

                reader = command.ExecuteReader();
                if (reader != null)
                    while (reader.Read())
                    {
                        if (result == null)
                        {
                            result = CreateResultTable(reader);
                        }
                        DataRow row = result.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            // if it is null and is not of type string, then, initialize to zero
                            if (reader.IsDBNull(i)
                                 && reader.GetFieldType(i) != typeof(string)
                                 && reader.GetFieldType(i) != typeof(DateTime))
                            {
                                row[i] = 0;
                            }
                            else
                            {
                                row[i] = reader.GetValue(i);
                            }
                        }
                        result.Rows.Add(row);
                    }

                return result;
            }
            catch (SqlException e)
            {
                logWriter.WriteErrorToEventLog(string.Format("DataHelper.ExecuteSqlForData: {0}", e.Message));  
                return result;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (command != null)
                    command.Dispose();
            }
        }

        public static List<DataTable> ExecuteSqlMultipleForData(string sql, CommandType commandType, List<SqlParameter> parameters)
        {
            List<DataTable> results = new List<DataTable>();
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                if (IsConnected)
                    _connection.Close();

                command = new SqlCommand(sql, GetConnection()) { CommandType = commandType };

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var sqlParameter in parameters)
                    {
                        command.Parameters.Add(sqlParameter);
                    }
                }

                reader = command.ExecuteReader();

                if (reader != null)
                {
                    do
                    {
                        DataTable result = new DataTable();

                        while (reader.Read())
                        {
                            if (result.Rows.Count == 0)
                                result = CreateResultTable(reader);

                            DataRow row = result.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                // if it is null and is not of type string, then, initialize to zero
                                if (reader.IsDBNull(i)
                                    && reader.GetFieldType(i) != typeof(string)
                                    && reader.GetFieldType(i) != typeof(DateTime))
                                {
                                    row[i] = 0;
                                }
                                else
                                {
                                    row[i] = reader.GetValue(i);
                                }
                            }
                            result.Rows.Add(row);
                        }

                        results.Add(result);

                    } while (reader.NextResult());
                }
                return results;
            }
            catch (SqlException e)
            {
                logWriter.WriteErrorToEventLog(string.Format("DataHelper.ExecuteSqlMultipleForData: {0}", e.Message));
                
                for (int i = 0; i < 4; i++)
                    if (results.Count < i + 1)
                        results.Add(new DataTable());

                return results;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (command != null)
                    command.Dispose();
            }
        }

        private static DataTable CreateResultTable(IDataRecord reader)
        {
            DataTable dataTable = new DataTable();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                DataColumn dataColumn = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
                dataTable.Columns.Add(dataColumn);
            }

            return dataTable;
        }
    }
}
