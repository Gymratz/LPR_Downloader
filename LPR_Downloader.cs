using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Configuration;
using System.Globalization;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LPR_Downloader
{
    public partial class frm_LPR_Downloader : Form
    {
        WebClient wc = new WebClient();
        private DateTime dtWbLastLoad;
        private DateTime dt_LastFullCSV;
        private long LastEpoch;
        private int MissedLocal = 0;

        private Image img_Full;

        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private SqlCommand sql_Command = new SqlCommand();
        private SqlConnection sql_Connection = new SqlConnection();

        private BindingSource bs_ImportHistory = new BindingSource();

        public frm_LPR_Downloader()
        {
            InitializeComponent();
        }

        private void Frm_LPR_Downloader_Load(object sender, EventArgs e) // Form Load, set constants and log into ALPR.
        {
            Settings_Populate();

            System.Threading.Tasks.Task blah = LoginALPR();
            HistoryGrid_Configure();

            // Starts the local ALPR Webpage Scraping on load
            if (Constants.StartOnLoad_Local == "True")
            {
                StartTimer_Local();
                Thread.Sleep(2000);
            }
            
            // Start the Cloud CSV Scraping on load
            if (Constants.StartOnLoad == "True")
            {
                StartTimer();
            }
        }

        private void Settings_Populate()
        {
            //Read in "Constants" from Configuration File (Not true constants...)
            Constants.emailDefaultTo = ConfigurationManager.AppSettings["emailDefaultTo"];
            txt_emailDefaultTo.Text = Constants.emailDefaultTo;

            Constants.emailSignIn = ConfigurationManager.AppSettings["emailSignIn"];
            txt_emailSignIn.Text = Constants.emailSignIn;

            Constants.emailPassword = ConfigurationManager.AppSettings["emailPassword"];
            txt_emailPassword.Text = Constants.emailPassword;

            Constants.emailServer = ConfigurationManager.AppSettings["emailServer"];
            txt_emailServer.Text = Constants.emailServer;

            Constants.emailPort = ConfigurationManager.AppSettings["emailPort"];
            txt_emailPort.Text = Constants.emailPort;  

            Constants.alprUser = ConfigurationManager.AppSettings["alprUser"];
            txt_alprUser.Text = Constants.alprUser;

            Constants.alprPassword = ConfigurationManager.AppSettings["alprPassword"];
            txt_alprPassword.Text = Constants.alprPassword;

            Constants.str_SqlCon = ConfigurationManager.AppSettings["SqlCon"];
            txt_SqlCon.Text = Constants.str_SqlCon;

            Constants.str_WebServer = ConfigurationManager.AppSettings["WebServer"];
            txt_WebServer.Text = Constants.str_WebServer;

            Constants.csvArchiveLocation = ConfigurationManager.AppSettings["csvArchiveLocation"];
            txt_csvArchiveLocation.Text = Constants.csvArchiveLocation;

            Constants.pushToken = ConfigurationManager.AppSettings["pushToken"];
            txt_pushToken.Text = Constants.pushToken;

            Constants.pushUser = ConfigurationManager.AppSettings["pushUser"];
            txt_pushUser.Text = Constants.pushUser;

            Constants.TimerMinutes = ConfigurationManager.AppSettings["TimerMinutes"];
            txt_TimerMinutes.Text = Constants.TimerMinutes;

            Constants.TimerMS = ConfigurationManager.AppSettings["TimerMS"];
            txt_TimerMS.Text = Constants.TimerMS;

            Constants.Image_Backup_Location = ConfigurationManager.AppSettings["Image_Backup_Location"];
            txt_Image_Backup_Location.Text = Constants.Image_Backup_Location;

            Constants.emailUseSSL = ConfigurationManager.AppSettings["emailUseSSL"];
            if (Constants.emailUseSSL == "True")
                chk_emailUseSSL.Checked = true;
            else
                chk_emailUseSSL.Checked = false;

            Constants.StartOnLoad = ConfigurationManager.AppSettings["StartOnLoad"];
            if (Constants.StartOnLoad == "True")
                chk_StartOnLoad.Checked = true;
            else
                chk_StartOnLoad.Checked = false;

            Constants.StartOnLoad_Local = ConfigurationManager.AppSettings["StartOnLoad_Local"];
            if (Constants.StartOnLoad_Local == "True")
                chk_StartOnLoad_Local.Checked = true;
            else
                chk_StartOnLoad_Local.Checked = false;
        }

        private void Settings_Save()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["emailDefaultTo"].Value = txt_emailDefaultTo.Text;
            config.AppSettings.Settings["emailSignIn"].Value = txt_emailSignIn.Text;
            config.AppSettings.Settings["emailPassword"].Value = txt_emailPassword.Text;
            config.AppSettings.Settings["emailServer"].Value = txt_emailServer.Text;
            config.AppSettings.Settings["emailPort"].Value = txt_emailPort.Text;
            config.AppSettings.Settings["emailUseSSL"].Value = chk_emailUseSSL.Checked.ToString();
            config.AppSettings.Settings["alprUser"].Value = txt_alprUser.Text;
            config.AppSettings.Settings["alprPassword"].Value = txt_alprPassword.Text;
            config.AppSettings.Settings["SqlCon"].Value = txt_SqlCon.Text;
            config.AppSettings.Settings["WebServer"].Value = txt_WebServer.Text;
            config.AppSettings.Settings["csvArchiveLocation"].Value = txt_csvArchiveLocation.Text;
            config.AppSettings.Settings["pushToken"].Value = txt_pushToken.Text;
            config.AppSettings.Settings["pushUser"].Value = txt_pushUser.Text;
            config.AppSettings.Settings["StartOnLoad"].Value = chk_StartOnLoad.Checked.ToString();
            config.AppSettings.Settings["StartOnLoad_Local"].Value = chk_StartOnLoad_Local.Checked.ToString();
            config.AppSettings.Settings["Image_Backup_Location"].Value = txt_Image_Backup_Location.Text;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            Settings_Populate();
        }

        #region Timer Related
        private void Btn_TimerStart_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["TimerMinutes"].Value = txt_TimerMinutes.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            StartTimer();
        }
        private void Btn_TimerStart_Local_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["TimerMS"].Value = txt_TimerMS.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            StartTimer_Local();
        }
        private void StartTimer()
        {
            int minutes;
            if (int.TryParse(txt_TimerMinutes.Text, out minutes))
            {
                btn_TimerStart.Enabled = false;
                btn_TimerStop.Enabled = true;
                txt_TimerMinutes.Enabled = false;

                timer_Download.Interval = minutes * 60 * 1000;
                timer_Download.Enabled = true;

                //Get a first load
                LoadCSV(true);
            }
        }
        private void StartTimer_Local()
        {      
            try
            {
                // Get the newest Epoch Time to ensure we're only looking at the new entries
                using (SqlConnection db_connection = new SqlConnection(Constants.str_SqlCon))
                {
                    using (SqlCommand db_command = new SqlCommand("select max(right(best_uuid, charindex('-', reverse(best_uuid)) - 1)) from LPR_PlateHits", db_connection))
                    {
                        db_connection.Open();

                        using (SqlDataReader db_reader = db_command.ExecuteReader())
                        {
                            while (db_reader.Read())
                            {
                                LastEpoch = long.Parse(db_reader[0].ToString());
                            }
                        }
                        db_connection.Close();
                    }
                }

                int ms;
                if (int.TryParse(txt_TimerMS.Text, out ms))
                {
                    btn_TimerStart_Local.Enabled = false;
                    btn_TimerStop_Local.Enabled = true;
                    txt_TimerMS.Enabled = false;

                    timer_Local_Download.Interval = ms;
                    timer_Local_Download.Enabled = true;

                    lbl_LastDownload_Local.Text = "Starting - No Cars Yet";
                }
            }
            catch (Exception e)
            {
                write_event(e.Message.ToString(), EventLogEntryType.Error);
            }
        }
        private void Btn_TimerStop_Click(object sender, EventArgs e)
        {
            timer_Download.Enabled = false;
            lbl_NextDownload.Text = "Timer Not Running...";

            btn_TimerStart.Enabled = true;
            btn_TimerStop.Enabled = false;
            txt_TimerMinutes.Enabled = true;
        }
        private void Btn_TimerStop_Local_Click(object sender, EventArgs e)
        {
            timer_Local_Download.Enabled = false;
            lbl_LastDownload_Local.Text = "Timer Not Running...";

            btn_TimerStart_Local.Enabled = true;
            btn_TimerStop_Local.Enabled = false;
            txt_TimerMS.Enabled = true;
        }

        private void Timer_Download_Tick(object sender, EventArgs e)
        {
            LoadCSV(false);
        }
        private void Timer_Local_Download_Tick(object sender, EventArgs e)
        {
            // Stop timer during execution so they don't overlap
            timer_Local_Download.Stop();
            GetLocal_NewHits();
            timer_Local_Download.Start();
        }
        private void GetLocal_NewHits()
        {
            try
            {
                var wc = new System.Net.WebClient();
                var stream_reader = new StreamReader(wc.OpenRead(Constants.str_WebServer));
                String Str = stream_reader.ReadToEnd();
                stream_reader.Close();

                DataTable UUIDs = ConvertHTMLTablesToDataTable(Str);

                foreach (DataRow lclRow in UUIDs.Rows)
                {
                    long CurrentCheck = long.Parse(lclRow[0].ToString().Split('-').Last());

                    if (CurrentCheck > LastEpoch)
                    {
                        string ALPR_json = "";

                        try
                        {
                            ALPR_json = (new WebClient()).DownloadString(Constants.str_WebServer + "/meta/" + lclRow[0].ToString());

                            // They don't have it wrapped in brackets...
                            string ALPR_json_2 = "[" + ALPR_json + "]";

                            var ALPR_Data_List = JsonSerializer.Deserialize<List<ALPR_Root>>(ALPR_json_2);

                            ALPR_Root ALPR_Data = new ALPR_Root();
                            ALPR_Data = ALPR_Data_List.First();

                            Process_Local_NewHit(ALPR_Data);
                        }
                        catch (Exception e)
                        {
                            write_event(e.Message.ToString(), EventLogEntryType.Error);
                        }

                        LastEpoch = CurrentCheck;
                    }
                }

                if (UUIDs.Rows.Count >= 1)
                {
                    CleanUpDB();
                }

                MissedLocal = 0;
            }
            catch (Exception e)
            {
                MissedLocal += 1;
                write_event(e.Message.ToString(), EventLogEntryType.Error);
            }


            if (MissedLocal != 0 && MissedLocal % 300 == 0)
            {
                if (Constants.emailDefaultTo != "")
                    Alert_Email_Generic(Constants.emailDefaultTo, "ALPR Agent Issue", "Errors the last " + MissedLocal + " attempts to access", "True");
            }
        }

        private void Process_Local_NewHit(ALPR_Root ALPR_Data)
        {
            if (ALPR_Data.is_parked == false)
            {
                string vehicle_crop_jpeg = ALPR_Data.vehicle_crop_jpeg;

                string agent_type = ALPR_Data.agent_type;
                string agent_uid = ALPR_Data.agent_uid;
                string best_plate = ALPR_Data.best_plate.plate;
                string best_uuid = ALPR_Data.best_uuid;

                double direction_of_travel_degrees = ALPR_Data.travel_direction;
                string epoch_time_start = DateTimeOffset.FromUnixTimeMilliseconds(ALPR_Data.epoch_start).ToString();
                string epoch_time_end = DateTimeOffset.FromUnixTimeMilliseconds(ALPR_Data.epoch_end).ToString();

                int plate_x1 = ALPR_Data.best_plate.coordinates[0].x;
                int plate_x2 = ALPR_Data.best_plate.coordinates[1].x;
                int plate_x3 = ALPR_Data.best_plate.coordinates[2].x;
                int plate_x4 = ALPR_Data.best_plate.coordinates[3].x;

                int plate_y1 = ALPR_Data.best_plate.coordinates[0].y;
                int plate_y2 = ALPR_Data.best_plate.coordinates[1].y;
                int plate_y3 = ALPR_Data.best_plate.coordinates[2].y;
                int plate_y4 = ALPR_Data.best_plate.coordinates[3].y;

                int vehicle_region_x = ALPR_Data.best_plate.vehicle_region.x;
                int vehicle_region_y = ALPR_Data.best_plate.vehicle_region.y;

                int vehicle_region_height = ALPR_Data.best_plate.vehicle_region.height;
                int vehicle_region_width = ALPR_Data.best_plate.vehicle_region.width;

                string region = ALPR_Data.best_region;

                string str_FieldNames = "pk, agent_type, agent_uid, best_plate, best_uuid, direction_of_travel_degrees, epoch_time_start, epoch_time_end,";
                str_FieldNames += "plate_x1, plate_x2, plate_x3, plate_x4, plate_y1, plate_y2, plate_y3, plate_y4, vehicle_region_x, vehicle_region_y, vehicle_region_height, vehicle_region_width, region";

                string str_ValueNames = "@pk, @agent_type, @agent_uid, @best_plate, @best_uuid, @direction_of_travel_degrees, @epoch_time_start, @epoch_time_end,";
                str_ValueNames += "@plate_x1, @plate_x2, @plate_x3, @plate_x4, @plate_y1, @plate_y2, @plate_y3, @plate_y4, @vehicle_region_x, @vehicle_region_y, @vehicle_region_height, @vehicle_region_width, @region";


                using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
                {
                    using (sql_Command = new SqlCommand("Insert Into LPR_PlateHits (" + str_FieldNames + ") VALUES (" + str_ValueNames + ")", sql_Connection))
                    {
                        sql_Command.Parameters.AddWithValue("@pk", "");
                        sql_Command.Parameters.AddWithValue("@agent_type", agent_type);
                        sql_Command.Parameters.AddWithValue("@agent_uid", agent_uid);
                        sql_Command.Parameters.AddWithValue("@best_plate", best_plate);
                        sql_Command.Parameters.AddWithValue("@best_uuid", best_uuid);
                        sql_Command.Parameters.AddWithValue("@direction_of_travel_degrees", direction_of_travel_degrees);
                        sql_Command.Parameters.AddWithValue("@epoch_time_start", epoch_time_start);
                        sql_Command.Parameters.AddWithValue("@epoch_time_end", epoch_time_end);
                        sql_Command.Parameters.AddWithValue("@plate_x1", plate_x1);
                        sql_Command.Parameters.AddWithValue("@plate_x2", plate_x2);
                        sql_Command.Parameters.AddWithValue("@plate_x3", plate_x3);
                        sql_Command.Parameters.AddWithValue("@plate_x4", plate_x4);
                        sql_Command.Parameters.AddWithValue("@plate_y1", plate_y1);
                        sql_Command.Parameters.AddWithValue("@plate_y2", plate_y2);
                        sql_Command.Parameters.AddWithValue("@plate_y3", plate_y3);
                        sql_Command.Parameters.AddWithValue("@plate_y4", plate_y4);
                        sql_Command.Parameters.AddWithValue("@vehicle_region_x", vehicle_region_x);
                        sql_Command.Parameters.AddWithValue("@vehicle_region_y", vehicle_region_y);
                        sql_Command.Parameters.AddWithValue("@vehicle_region_height", vehicle_region_height);
                        sql_Command.Parameters.AddWithValue("@vehicle_region_width", vehicle_region_width);
                        sql_Command.Parameters.AddWithValue("@region", region);

                        sql_Connection.Open();
                        sql_Command.ExecuteNonQuery();
                        sql_Connection.Close();
                    }
                }


                // Save the Color info to the separate table
                string myColor = ALPR_Data.vehicle.color[0].name;
                string myMake = ALPR_Data.vehicle.make[0].name;
                string myModel = ALPR_Data.vehicle.make_model[0].name;

                using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
                {
                    //Add new entry
                    using (sql_Command = new SqlCommand("Insert Into LPR_LocalInfo (UUID, best_color, best_make, best_model) VALUES (@UUID, @best_color, @best_make, @best_model)", sql_Connection))
                    {
                        sql_Command.Parameters.AddWithValue("@UUID", best_uuid);
                        sql_Command.Parameters.AddWithValue("@best_color", myColor);
                        sql_Command.Parameters.AddWithValue("@best_make", myMake);
                        sql_Command.Parameters.AddWithValue("@best_model", myModel);
                        sql_Connection.Open();
                        try
                        {
                            sql_Command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            write_event(e.Message.ToString(), EventLogEntryType.Error);
                        }
                        sql_Connection.Close();
                    }
                }

                // Save the image backup
                try
                {
                    //Gets Image and saves the full image
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(Constants.str_WebServer + "/img/" + best_uuid + ".jpg");
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    img_Full = Image.FromStream(ms);

                    // Save the Image to a Network Location
                    img_Full.Save(Constants.Image_Backup_Location + best_uuid + ".jpg");
                }
                catch (Exception e)
                {
                    write_event(e.Message.ToString(), EventLogEntryType.Error);
                }

                lbl_LastDownload_Local.Text = "Last Car: " + DateTime.Now.ToString("h:mm:ss tt");

                // Spin up a new thread for generating alerts since these will have a slight delay and shouldn't impact additional downloads.
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    GenerateAlerts_Step2(best_plate, best_uuid, vehicle_crop_jpeg);
                }).Start();
            }
        }
        #endregion


        private void LoadCSV(bool FullLoad = false)
        {
            int minutes;
            int.TryParse(txt_TimerMinutes.Text, out minutes);

            lbl_LastDownload.Text = "Last Download: " + DateTime.Now.ToString("h:mm:ss tt");
            lbl_NextDownload.Text = "Next Download: " + DateTime.Now.AddMinutes(minutes).ToString(("h:mm:ss tt"));

            if (FullLoad == true) // Pull in 2 full days of data.
            {
                minutes = 60 * 24 * 2;
                dt_LastFullCSV = DateTime.Now; // Keep track of last full load
            }
            else
            {
                if (dt_LastFullCSV.Day < DateTime.Now.Day) // Just after midnight
                {
                    FullLoad = true;
                }
            }
            DownloadAndImport(minutes);
        }

        public void DownloadAndImport(int minutes = 15)
        {
            DownloadFile(minutes);
        }

        private async void DownloadFile(int minutes = 15)
        {
            //See if already on the OpenALPR Page, load if not.  Also re-navigate to page if haven't done so within the past eight-hours to refresh cookie.
            if (wb_OpenALPR.Url == null || ((TimeSpan)(DateTime.Now - dtWbLastLoad)).Hours > 8)
            {
                await LoginALPR();
            }

            //Download last xx minutes (timer count + 1)
            string str_StartDate = DateTime.Now.AddMinutes(0 - (minutes * 2) - 1).ToString("yyyy-MM-dd");
            string str_StartHours = DateTime.Now.AddMinutes(0 - (minutes * 2) - 1).ToString("HH");
            string str_StartMinutes = DateTime.Now.AddMinutes(0 - (minutes * 2) - 1).ToString("mm");

            string str_EndDate = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd");
            string str_EndHours = DateTime.Now.AddMinutes(1).ToString("HH");
            string str_EndMinutes = DateTime.Now.AddMinutes(1).ToString("mm");

            string str_UTC_Offset = DateTime.Now.ToString("zz");
            if (!str_UTC_Offset.Contains("-") & str_UTC_Offset != "0" & str_UTC_Offset != "+0")
            {
                str_UTC_Offset = "%2b" + str_UTC_Offset;
            }


            string _url = "https://cloud.openalpr.com/api/search/group?topn=20000&start=" + str_StartDate + "T" + str_StartHours + "%3A" + str_StartMinutes + "%3A00" + str_UTC_Offset + "%3A00&end=" + str_EndDate + "T" + str_EndHours + "%3A" + str_EndMinutes + "%3A59" + str_UTC_Offset + "%3A00&order=desc&format=csv";

            try
            {
                using (MemoryStream streamCSV = new MemoryStream(wc.DownloadData(_url)))
                {
                    UploadStream(streamCSV);
                }
            }
            catch (Exception e) // Currently the only time I've had a failure is if logged out.
            {
                write_event(e.Message.ToString(), EventLogEntryType.Error);
                await LoginALPR(); // Logs in again, file should import next timer tick.
            }
        }

        private void UploadStream(MemoryStream streamCSV)
        {
            // Load CSV File into Data Table
            int TotalCSVCount = 0;
            DataTable dt_CSVData = new DataTable();
            DataTable dt_CSVData_ToUpdate = new DataTable();
            if (streamCSV.Length > 500)  // Ensure stream contains something before bothering to try a full import.
            {
                dt_CSVData = GetDataFromStream(streamCSV);
                dt_CSVData_ToUpdate = dt_CSVData.Copy(); // Make a copy for items to update that came across in the "Local" pickup
                TotalCSVCount = dt_CSVData.Rows.Count;

                // Load entire LPR Database into Data Table
                DataTable dt_SQLData = new DataTable();
                //dataAdapter = new SqlDataAdapter("Select * from LPR_PlateHits", Constants.str_SqlCon);
                dataAdapter = new SqlDataAdapter("Select * from LPR_PlateHits Where Cast(Cast(epoch_time_end as datetimeoffset) as datetime) > DateAdd(day, -10, GetDate())", Constants.str_SqlCon);
                dataAdapter.Fill(dt_SQLData);

                // Loop through CSV File and save the row id for any rows already in the DB (based on PK).
                List<DataRow> rows_to_remove = new List<DataRow>(); // Removes from primary import
                foreach (DataRow csvRow in dt_CSVData.Rows)
                {
                    foreach (DataRow sqlRow in dt_SQLData.Rows)
                    {
                        //if (csvRow["pk"].ToString() == sqlRow["pk"].ToString())
                        if (csvRow["best_uuid"].ToString() == sqlRow["best_uuid"].ToString())
                        {
                            rows_to_remove.Add(csvRow);
                        }
                    }
                }

                // Loop through CSV File and save the row id for any rows already in the DB (based on PK).
                List<DataRow> rows_to_remove_update = new List<DataRow>(); // Removes from update import
                foreach (DataRow csvRow in dt_CSVData_ToUpdate.Rows)
                {
                    foreach (DataRow sqlRow in dt_SQLData.Rows)
                    {
                        if (csvRow["best_uuid"].ToString() == sqlRow["best_uuid"].ToString())
                        {
                            if (csvRow["pk"].ToString() == sqlRow["pk"].ToString())
                            {
                                rows_to_remove_update.Add(csvRow);
                            }
                        }
                    }
                }

                // Loop through rows to remove, and remove them.
                foreach (DataRow row in rows_to_remove)
                {
                    dt_CSVData.Rows.Remove(row);
                    dt_CSVData.AcceptChanges();
                }

                // Loop through rows to remove, and remove them. (For Updates...)
                foreach (DataRow row in rows_to_remove_update)
                {
                    dt_CSVData_ToUpdate.Rows.Remove(row);
                    dt_CSVData_ToUpdate.AcceptChanges();
                }

                // Loop through Inserts and remove from Update
                List<DataRow> rows_to_remove_update_2 = new List<DataRow>(); // Removes from update import
                foreach (DataRow updateRow in dt_CSVData_ToUpdate.Rows)
                {
                    foreach (DataRow insertRow in dt_CSVData.Rows)
                    {
                        if (updateRow["best_uuid"].ToString() == insertRow["best_uuid"].ToString())
                        {
                            rows_to_remove_update_2.Add(updateRow);
                        }
                    }
                }

                // Loop through rows to remove, and remove them. (For Updates...)
                foreach (DataRow row in rows_to_remove_update_2)
                {
                    dt_CSVData_ToUpdate.Rows.Remove(row);
                    dt_CSVData_ToUpdate.AcceptChanges();
                }

                // If there is data to import...
                if (dt_CSVData.Rows.Count > 0)
                {
                    // Remove columns from CSVData that do not exist in SQLData, Save the column names that do for SQL Insert
                    string str_FieldNames = "";
                    string str_ValueNames = "";
                    foreach (DataColumn csvColumn in dt_CSVData.Columns)
                    {
                        if (!dt_SQLData.Columns.Contains(csvColumn.ColumnName))
                        {
                            dt_CSVData.Columns.Remove(csvColumn);
                        }
                        else
                        {
                            str_FieldNames = str_FieldNames + csvColumn.ColumnName + ", ";
                            str_ValueNames = str_ValueNames + "@" + csvColumn.ColumnName + ", ";
                        }
                    }

                    // Remove final ", "
                    str_FieldNames = str_FieldNames.Substring(0, str_FieldNames.Length - 2);
                    str_ValueNames = str_ValueNames.Substring(0, str_ValueNames.Length - 2);

                    using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
                    {
                        sql_Connection.Open();
                        foreach (DataRow csvRow in dt_CSVData.Rows) // For each record, we're going to do a separate insert.
                        {
                            using (sql_Command = new SqlCommand("Insert Into LPR_PlateHits (" + str_FieldNames + ") VALUES (" + str_ValueNames + ")", sql_Connection))
                            {
                                foreach (DataColumn csvColumn in dt_CSVData.Columns) // For each available column, add the parameter.
                                {
                                    sql_Command.Parameters.AddWithValue("@" + csvColumn.ColumnName, csvRow[csvColumn.ColumnName].ToString());
                                }

                                //sql_Connection.Open();
                                try
                                {
                                    sql_Command.ExecuteNonQuery();
                                }
                                catch (Exception e)
                                {
                                    write_event(e.Message.ToString(), EventLogEntryType.Error);
                                }
                                //sql_Connection.Close();
                            }
                        }
                        sql_Connection.Close();
                    }

                    // Spin up a new thread for generating alerts since these will have a slight delay and shouldn't impact additional downloads.
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        GenerateAlerts(dt_CSVData);
                    }).Start();

                    // If Archive Folder, Archive.  (Only archives CSVs with new information)
                    try
                    {
                        if (Constants.csvArchiveLocation != "")
                        {
                            File.WriteAllBytes(Constants.csvArchiveLocation + DateTime.Now.ToString("yyyy.MM.dd_hhmmss") + ".csv", streamCSV.ToArray());
                        }
                    }
                    catch (Exception e)
                    {
                        write_event(e.Message.ToString(), EventLogEntryType.Error);
                    }
                   

                    // Run a command to clean up the DB.
                    CleanUpDB();
                }

                // If there are rows to update
                if (dt_CSVData_ToUpdate.Rows.Count > 0)
                {
                    using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
                    {
                        foreach (DataRow csvRow in dt_CSVData_ToUpdate.Rows) // For each record, we're going to do a separate Update.
                        {
                            using (sql_Command = new SqlCommand("Update LPR_PlateHits Set pk = @pk, Camera = @Camera, Camera_ID = @Camera_ID Where best_uuid = @best_uuid", sql_Connection))
                            {
                                sql_Command.Parameters.AddWithValue("@pk", csvRow["pk"].ToString());
                                sql_Command.Parameters.AddWithValue("@Camera", csvRow["camera"].ToString());
                                sql_Command.Parameters.AddWithValue("@Camera_ID", csvRow["camera_id"].ToString());
                                sql_Command.Parameters.AddWithValue("@best_uuid", csvRow["best_uuid"].ToString());

                                sql_Connection.Open();
                                sql_Command.ExecuteNonQuery();
                                sql_Connection.Close();
                            }
                        }
                    }
                }
            }

            // Save Import History
            using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (sql_Command = new SqlCommand("Insert Into LPR_ImportHistory (Import_Time, Count_Imported, Count_Skipped, Count_Updated) VALUES (GetDate(), @Count_Imported, @Count_Skipped, @Count_Updated)", sql_Connection))
                {
                    sql_Command.Parameters.AddWithValue("@Count_Imported", dt_CSVData.Rows.Count);
                    sql_Command.Parameters.AddWithValue("@Count_Skipped", TotalCSVCount - dt_CSVData.Rows.Count);
                    sql_Command.Parameters.AddWithValue("@Count_Updated", dt_CSVData_ToUpdate.Rows.Count);
                    sql_Connection.Open();
                    sql_Command.ExecuteNonQuery();
                    sql_Connection.Close();
                }
            }
            HistoryGrid_Load();
        }

        private void CleanUpDB()
        {
            using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (sql_Command = new SqlCommand("Exec sp_LPR_FixPlates", sql_Connection))
                {
                    sql_Connection.Open();
                    sql_Command.ExecuteNonQuery();
                    sql_Connection.Close();
                }
            }
        }

        private void GenerateAlerts_Step2(string searchPlate, string imagePlate, string vehicle_crop_jpeg = "")
        {
            try
            {
                String alertAddress = "";
                String Description = "";
                String Status = "";
                String Year = "";
                String Make = "";
                String Model = "";

                String Pushover = "False";
                String Priority = "False";

                using (SqlConnection sql_Connection_Alert = new SqlConnection(Constants.str_SqlCon))
                {
                    using (SqlCommand sql_Command_Alert = new SqlCommand("Exec sp_LPR_PlateAlerts @Plate", sql_Connection_Alert))
                    {
                        sql_Command_Alert.Parameters.AddWithValue("@Plate", searchPlate);
                        sql_Connection_Alert.Open();

                        using (SqlDataReader db_reader_Alert = sql_Command_Alert.ExecuteReader())
                        {
                            while (db_reader_Alert.Read())
                            {
                                alertAddress = db_reader_Alert[0].ToString();
                                Description = db_reader_Alert[1].ToString();
                                Status = db_reader_Alert[2].ToString();
                                Year = db_reader_Alert[3].ToString();
                                Make = db_reader_Alert[4].ToString();
                                Model = db_reader_Alert[5].ToString();
                                Pushover = db_reader_Alert[6].ToString();
                                Priority = db_reader_Alert[7].ToString();
                            }
                        }
                        sql_Connection_Alert.Close();
                    }
                }

                string AlertMessage = "A flagged license plate has been spotted!";
                AlertMessage += "<br />Status: " + Status;
                AlertMessage += "<br />Description: " + Description;
                AlertMessage += "<br />Year: " + Year;
                AlertMessage += "<br />Make: " + Make;
                AlertMessage += "<br />Model: " + Model;

                if (Pushover == "True" && Constants.pushToken != "" && Constants.pushUser != "")
                {
                    try
                    {
                        var DontCare = Pushover_PlusImage("Spotted: " + searchPlate, AlertMessage, vehicle_crop_jpeg, Priority);
                    }
                    catch (Exception e)
                    {
                        write_event(e.Message.ToString(), EventLogEntryType.Error);
                    }
                }

                if (alertAddress != "" && Constants.emailSignIn != "")
                {
                    Image img_Full;
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(Constants.str_WebServer + "/img/" + imagePlate + ".jpg");
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    img_Full = Image.FromStream(ms);

                    Alert_Email(img_Full, alertAddress, searchPlate, AlertMessage, Priority);
                }
            }
            catch (Exception e)
            {
                write_event(e.Message.ToString(), EventLogEntryType.Error);
            }
        }
        private void GenerateAlerts(DataTable dt_NewEntries) // Takes the data just uploaded and checks to see if an e-mail alert is needed.
        {
            try
            {
                //For each Row...
                foreach (DataRow row1 in dt_NewEntries.Rows)
                {
                    String searchPlate = row1["best_plate"].ToString();
                    String imagePlate = row1["best_uuid"].ToString();

                    GenerateAlerts_Step2(searchPlate, imagePlate);
                }
            }
            catch (Exception e)
            {
                write_event(e.Message.ToString(), EventLogEntryType.Error);
            }     
        }

        private void Alert_Email_Generic(string str_EmailToAddress, string AlertSubject, string AlertMessage, string Priority)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress(Constants.emailSignIn);

                char[] delimiterChars = { ',', ';' };
                string[] ToEmails = str_EmailToAddress.Split(delimiterChars);
                foreach (var ToEmail in ToEmails)
                {
                    mail.To.Add(new MailAddress(ToEmail));
                }

                if (Priority == "True")
                    mail.Priority = MailPriority.High;

                mail.IsBodyHtml = true;
                mail.Subject = AlertSubject;
                mail.Body = AlertMessage;

                using (SmtpClient smtp = new SmtpClient()) // Information for Gmail, change if you use another provider.
                {
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(Constants.emailSignIn, Constants.emailPassword);
                    smtp.Host = Constants.emailServer;
                    smtp.Send(mail);
                }
            }
            catch (Exception e)
            {
                write_event(e.Message.ToString(), EventLogEntryType.Error);
            }
        }

        private void Alert_Email(Image img_ToEmail, string str_EmailToAddress, string str_Plate, string AlertMessage, string Priority = "False")
        {
            MailMessage mail = new MailMessage();
            mail.From = new System.Net.Mail.MailAddress(Constants.emailSignIn);

            char[] delimiterChars = { ',', ';' };
            string[] ToEmails = str_EmailToAddress.Split(delimiterChars);
            foreach (var ToEmail in ToEmails)
            {
                mail.To.Add(new MailAddress(ToEmail));
            }

            if (Priority == "True")
                mail.Priority = MailPriority.High;
                        
            mail.IsBodyHtml = true;
            mail.Subject = "License Plate Alert: " + str_Plate;
            string st = AlertMessage;
            st += "<br /><br /><img src = \"$LPIMAGE$\" />";

            //Update all the Image Placeholders to code that will allow the images to show up inline in the email
            string contentIDLP = Guid.NewGuid().ToString().Replace("-", "");
            st = st.Replace("$LPIMAGE$", "cid:" + contentIDLP);

            //Add to Alternate View
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(st, null, "text/html");

            //Add the actual images
            var imageStreamLP = GetImageStream(img_ToEmail);
            LinkedResource imagelinkLP = new LinkedResource(imageStreamLP, "image/jpeg");
            imagelinkLP.ContentId = contentIDLP;
            imagelinkLP.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            htmlView.LinkedResources.Add(imagelinkLP);

            mail.AlternateViews.Add(htmlView);
            mail.Body = st;

            using (SmtpClient smtp = new SmtpClient()) // Information for Gmail, change if you use another provider.
            {
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Constants.emailSignIn, Constants.emailPassword);
                smtp.Host = Constants.emailServer;
                smtp.Send(mail);
            }
        }

        private void Btn_Test_Pushover_Click(object sender, EventArgs e)
        {
            // Get the most recent entry for selected plate
            string best_plate = txt_PushoverTestPlate.Text;
            string best_uuid = "";
            using (SqlConnection db_connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (SqlCommand db_command = new SqlCommand("Select Top 1 best_uuid From LPR_PLateHits Where best_plate = '" + best_plate + "' Order By Cast(Cast(epoch_time_end as datetimeoffset) as datetime) Desc", db_connection))
                {
                    db_connection.Open();

                    using (SqlDataReader db_reader = db_command.ExecuteReader())
                    {
                        while (db_reader.Read())
                        {
                            best_uuid = db_reader[0].ToString();
                        }
                    }
                    db_connection.Close();
                }
            }

            // This section just to get the crop JPEG for testing...
            string ALPR_json = "";
            ALPR_json = (new WebClient()).DownloadString(Constants.str_WebServer + "/meta/" + best_uuid);

            // They don't have it wrapped in brackets...
            string ALPR_json_2 = "[" + ALPR_json + "]";

            var ALPR_Data_List = JsonSerializer.Deserialize<List<ALPR_Root>>(ALPR_json_2);
            ALPR_Root ALPR_Data = new ALPR_Root();
            ALPR_Data = ALPR_Data_List.First();

            string vehicle_crop_jpeg = ALPR_Data.vehicle_crop_jpeg;

            GenerateAlerts_Step2(best_plate, best_uuid, vehicle_crop_jpeg);
        }

        public async Task Pushover_PlusImage(string title, string message, string vehicle_crop_jpeg = "", string priority = "false")
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //specify to use TLS 1.2 as default connection
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var boundary = Guid.NewGuid().ToString();
                MultipartFormDataContent form = new MultipartFormDataContent(boundary);

                form.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data; boundary=" + boundary);

                form.Add(new StringContent(Constants.pushUser), "user");
                form.Add(new StringContent(Constants.pushToken), "token");
                form.Add(new StringContent(message), "message");
                form.Add(new StringContent(title), "title");
                form.Add(new StringContent("1"), "html");
                form.Add(new ByteArrayContent(Convert.FromBase64String(vehicle_crop_jpeg)), "attachment", "attachment.jpg");
                if (priority == "True")
                    form.Add(new StringContent("1"), "priority");

                HttpResponseMessage responseMessage = await httpClient.PostAsync("https://api.pushover.net/1/messages.json", form);
            }
        }

        private void HistoryGrid_Load()
        {
            dataAdapter = new SqlDataAdapter("Select Top 200 Import_Time as [Time], Count_Imported as [Imported], Count_Skipped as [Skipped], Count_Updated as [Updated] from LPR_ImportHistory Order By Import_Time Desc", Constants.str_SqlCon);
            DataTable table = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
            dataAdapter.Fill(table);

            dgv_ImportHistory.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv_ImportHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv_ImportHistory.RowHeadersVisible = false;
            bs_ImportHistory.DataSource = table;
            dgv_ImportHistory.RowHeadersVisible = true;
            dgv_ImportHistory.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private async Task LoginALPR()
        {
            wb_OpenALPR.Navigate("https://cloud.openalpr.com");
            await Wait_WBLoad(10);//await for page to load, timeout 10 seconds.

            //See if on login page, log in if so.
            var element = wb_OpenALPR.Document.GetElementById("username");
            if (element != null) //on login page
            {
                wb_OpenALPR.Document.GetElementById("username").SetAttribute("value", Constants.alprUser);
                wb_OpenALPR.Document.GetElementById("password").SetAttribute("value", Constants.alprPassword);

                //Updated 2020.10.14 to handle new login method.
                wb_OpenALPR.Document.GetElementById("login-btn").InvokeMember("click");

                //HtmlElementCollection elc = wb_OpenALPR.Document.GetElementsByTagName("input");
                //foreach (HtmlElement el in elc)
                //{
                //    if (el.GetAttribute("id").Equals("login-btn"))
                //    {
                //        el.InvokeMember("Click");
                //    }
               // }
                await Wait_WBLoad(10);//wait for page to login, timeout 10 seconds.
            }

            //This gets the cookies from the WebBrowser, which is logged in, and adds them to the WebClient, which can do an easy background file save.
            var cookies = FullWebBrowserCookie.GetCookieInternal(wb_OpenALPR.Url, false);
            wc.Headers.Clear();
            wc.Headers.Add("Cookie: " + cookies);
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            dtWbLastLoad = DateTime.Now;
        }

        private static Stream GetImageStream(Image image) // Convert the image to a memory stream and return.
        {
            var imageConverter = new ImageConverter();
            var imgaBytes = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
            var memoryStream = new MemoryStream(imgaBytes);

            return memoryStream;
        }

        private async Task Wait_WBLoad(int TimeOut)
        {
            TaskCompletionSource<bool> PageLoaded = null;
            PageLoaded = new TaskCompletionSource<bool>();
            int TimeElapsed = 0;
            wb_OpenALPR.DocumentCompleted += (s, e) =>
            {
                if (wb_OpenALPR.ReadyState != WebBrowserReadyState.Complete) return;
                if (PageLoaded.Task.IsCompleted) return; PageLoaded.SetResult(true);
            };
            //
            while (PageLoaded.Task.Status != TaskStatus.RanToCompletion)
            {
                await Task.Delay(10);//interval of 10 ms worked good for me
                TimeElapsed++;
                if (TimeElapsed >= TimeOut * 100) PageLoaded.TrySetResult(true);
            }
        }

        private void HistoryGrid_Configure()
        {
            //Set formatting of Main GridView
            {
                var withBlock = dgv_ImportHistory;
                withBlock.RowsDefaultCellStyle.BackColor = Color.LightSteelBlue;
                withBlock.AlternatingRowsDefaultCellStyle.BackColor = Color.PowderBlue;
                withBlock.Font = new Font("Courier New", 10, FontStyle.Regular);

                withBlock.DataSource = bs_ImportHistory;
            }
        }

        private DataTable GetDataFromStream(MemoryStream streamCSV)
        {
            DataTable importedData = new DataTable();
            string header = "";

            try
            {
                using (StreamReader sr = new StreamReader(streamCSV))
                {
                    if (string.IsNullOrEmpty(header))
                    {
                        header = sr.ReadLine();
                    }

                    string[] headerColumns = header.Split(',');
                    foreach (string headerColumn in headerColumns)
                    {
                        importedData.Columns.Add(headerColumn);
                    }

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        string[] fields = line.Split(',');
                        DataRow importedRow = importedData.NewRow();

                        for (int i = 0; i < fields.Count(); i++)
                        {
                            importedRow[i] = fields[i];
                        }
                        importedData.Rows.Add(importedRow);
                    }
                }
            }
            catch
            {
            }
            return importedData;
        }

        private void Btn_ManuallyUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd_ManualUpload;
            ofd_ManualUpload = new OpenFileDialog
            {
                Filter = "csv files (*.csv)|*.csv"
            };
            
            if (ofd_ManualUpload.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (MemoryStream streamCSV = new MemoryStream(File.ReadAllBytes(ofd_ManualUpload.FileName)))
                    {
                        UploadStream(streamCSV);
                    }
                }
                catch 
                {

                }
            }
        }

        private void Btn_SettingsSave_Click(object sender, EventArgs e)
        {
            Settings_Save();
        }

        private void Btn_emailTestSend_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress(txt_emailSignIn.Text);
                mail.To.Add(new MailAddress(txt_emailDefaultTo.Text));
                mail.IsBodyHtml = true;
                mail.Subject = "This is a test email";
                string st = "Success!";
                mail.Body = st;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Port = Convert.ToInt32(txt_emailPort.Text);
                    smtp.EnableSsl = chk_emailUseSSL.Checked;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(txt_emailSignIn.Text, txt_emailPassword.Text);
                    smtp.Host = txt_emailServer.Text;
                    smtp.Send(mail);
                }
                MessageBox.Show("Successful E-mail Test!", "Test Success");
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message.ToString(), "Test Failed");
            }
        }

        private void write_event(string myMessage, EventLogEntryType myType)
        {
            try
            {
                string source = ".NET Runtime";
                EventLog systemEventLog = new EventLog("Application");
                systemEventLog.Source = source;
                systemEventLog.WriteEntry(myMessage, myType, 1001);
            }
            catch { }    
        }

        public static DataTable ConvertHTMLTablesToDataTable(string HTML)
        {
            DataTable dt = null;
            DataRow dr = null;
            //DataColumn dc = null;
            string TableExpression = "<table[^>]*>(.*?)</table>";
            string HeaderExpression = "<th[^>]*>(.*?)</th>";
            string RowExpression = "<tr[^>]*>(.*?)</tr>";
            string ColumnExpression = "<td[^>]*>(.*?)</td>";
            bool HeadersExist = false;
            int iCurrentColumn = 0;
            int iCurrentRow = 0;

            // Get a match for all the tables in the HTML    
            MatchCollection Tables = Regex.Matches(HTML, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Loop through each table element    
            foreach (Match Table in Tables)
            {

                // Reset the current row counter and the header flag    
                iCurrentRow = 0;
                HeadersExist = false;

                // Add a new table to the DataSet    
                dt = new DataTable();

                // Create the relevant amount of columns for this table (use the headers if they exist, otherwise use default names)    
                if (Table.Value.Contains("<th"))
                {
                    // Set the HeadersExist flag    
                    HeadersExist = true;

                    // Get a match for all the rows in the table    
                    MatchCollection Headers = Regex.Matches(Table.Value, HeaderExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    // Loop through each header element    
                    foreach (Match Header in Headers)
                    {
                        //dt.Columns.Add(Header.Groups(1).ToString);  
                        dt.Columns.Add(Header.Groups[1].ToString());

                    }
                }
                else
                {
                    for (int iColumns = 1; iColumns <= Regex.Matches(Regex.Matches(Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Count; iColumns++)
                    {
                        dt.Columns.Add("Column " + iColumns);
                    }
                }

                // Get a match for all the rows in the table    
                MatchCollection Rows = Regex.Matches(Table.Value, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                // Loop through each row element    
                foreach (Match Row in Rows)
                {

                    // Only loop through the row if it isn't a header row    
                    if (!(iCurrentRow == 0 & HeadersExist == true))
                    {

                        // Create a new row and reset the current column counter    
                        dr = dt.NewRow();
                        iCurrentColumn = 0;

                        // Get a match for all the columns in the row    
                        MatchCollection Columns = Regex.Matches(Row.Value, ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                        // Loop through each column element    
                        foreach (Match Column in Columns)
                        {

                            DataColumnCollection columns = dt.Columns;

                            if (!columns.Contains("Column " + iCurrentColumn))
                            {
                                //Add Columns  
                                dt.Columns.Add("Column " + iCurrentColumn);
                            }
                            // Add the value to the DataRow    
                            dr[iCurrentColumn] = Column.Groups[1].ToString();
                            // Increase the current column    
                            iCurrentColumn += 1;

                        }

                        // Add the DataRow to the DataTable    
                        dt.Rows.Add(dr);

                    }

                    // Increase the current row counter    
                    iCurrentRow += 1;
                }
            }
            return (dt);
        }

        #region "ALPR Local Classes"
        public class ALPR_Candidate
        {
            public string plate { get; set; }
            public double confidence { get; set; }
            public int matches_template { get; set; }
        }
        public class ALPR_Coordinate
        {
            public int x { get; set; }
            public int y { get; set; }
        }
        public class ALPR_VehicleRegion
        {
            public int x { get; set; }
            public int y { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }
        public class ALPR_BestPlate
        {
            public string plate { get; set; }
            public double confidence { get; set; }
            public int matches_template { get; set; }
            public int plate_index { get; set; }
            public string region { get; set; }
            public int region_confidence { get; set; }
            public double processing_time_ms { get; set; }
            public int requested_topn { get; set; }
            public List<ALPR_Coordinate> coordinates { get; set; }
            public string plate_crop_jpeg { get; set; }
            public ALPR_VehicleRegion vehicle_region { get; set; }
            public List<ALPR_Candidate> candidates { get; set; }
        }
        public class ALPR_Color
        {
            public string name { get; set; }
            public double confidence { get; set; }
        }
        public class ALPR_Make
        {
            public string name { get; set; }
            public double confidence { get; set; }
        }
        public class ALPR_MakeModel
        {
            public string name { get; set; }
            public double confidence { get; set; }
        }
        public class ALPR_BodyType
        {
            public string name { get; set; }
            public double confidence { get; set; }
        }
        public class ALPR_Year
        {
            public string name { get; set; }
            public double confidence { get; set; }
        }
        public class ALPR_Orientation
        {
            public string name { get; set; }
            public double confidence { get; set; }
        }
        public class ALPR_Vehicle
        {
            public List<ALPR_Color> color { get; set; }
            public List<ALPR_Make> make { get; set; }
            public List<ALPR_MakeModel> make_model { get; set; }
            public List<ALPR_BodyType> body_type { get; set; }
            public List<ALPR_Year> year { get; set; }
            public List<ALPR_Orientation> orientation { get; set; }
        }
        public class ALPR_Root
        {
            public string data_type { get; set; }
            public int version { get; set; }
            public long epoch_start { get; set; }
            public long epoch_end { get; set; }
            public int frame_start { get; set; }
            public int frame_end { get; set; }
            public string company_id { get; set; }
            public string agent_uid { get; set; }
            public string agent_version { get; set; }
            public string agent_type { get; set; }
            public int camera_id { get; set; }
            public string country { get; set; }
            public List<string> uuids { get; set; }
            public List<int> plate_indexes { get; set; }
            public List<ALPR_Candidate> candidates { get; set; }
            public string vehicle_crop_jpeg { get; set; }
            public ALPR_BestPlate best_plate { get; set; }
            public double best_confidence { get; set; }
            public string best_uuid { get; set; }
            public string best_plate_number { get; set; }
            public string best_region { get; set; }
            public double best_region_confidence { get; set; }
            public bool matches_template { get; set; }
            public int best_image_width { get; set; }
            public int best_image_height { get; set; }
            public double travel_direction { get; set; }
            public bool is_parked { get; set; }
            public bool is_preview { get; set; }
            public ALPR_Vehicle vehicle { get; set; }
        }

        #endregion


    }
}
