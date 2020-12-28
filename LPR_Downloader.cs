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
using Microsoft.Win32;

namespace LPR_Downloader
{
    public partial class frm_LPR_Downloader : Form
    {
        WebClient wc = new WebClient();
        private DateTime dtWbLastLoad;
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

            //Added 2020.10.14 - Start downloading on form load if setting is configured
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

            Constants.emailUseSSL = ConfigurationManager.AppSettings["emailUseSSL"];
            if (Constants.emailUseSSL == "True")
            {
                chk_emailUseSSL.Checked = true;
            }
            else
            {
                chk_emailUseSSL.Checked = false;
            }

            //Added 2020.10.14 - New Setting
            Constants.StartOnLoad = ConfigurationManager.AppSettings["StartOnLoad"];
            if (Constants.StartOnLoad == "True")
            {
                chk_StartOnLoad.Checked = true;
            }
            else
            {
                chk_StartOnLoad.Checked = false;
            }

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

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            Settings_Populate();
        }

        #region Timer Related
        private void Btn_TimerStart_Click(object sender, EventArgs e)
        {
            //Added 2020.10.14 - Save the Timer Value
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["TimerMinutes"].Value = txt_TimerMinutes.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            //Added 2020.10.14 - Moved to Function so I can call it when starting the application
            StartTimer();
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

        private void Btn_TimerStop_Click(object sender, EventArgs e)
        {
            timer_Download.Enabled = false;
            lbl_NextDownload.Text = "Timer Not Running...";

            btn_TimerStart.Enabled = true;
            btn_TimerStop.Enabled = false;
            txt_TimerMinutes.Enabled = true;
        }

        private void Timer_Download_Tick(object sender, EventArgs e)
        {
            LoadCSV(false);
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
            string str_StartDate = DateTime.Now.AddMinutes(0 - minutes - 1).ToString("yyyy-MM-dd");
            string str_StartHours = DateTime.Now.AddMinutes(0 - minutes - 1).ToString("HH");
            string str_StartMinutes = DateTime.Now.AddMinutes(0 - minutes - 1).ToString("mm");

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
            if (streamCSV.Length > 500)  // Ensure stream contains something before bothering to try a full import.
            {
                dt_CSVData = GetDataFromStream(streamCSV);
                TotalCSVCount = dt_CSVData.Rows.Count;

                // Load entire LPR Database into Data Table
                DataTable dt_SQLData = new DataTable();
                dataAdapter = new SqlDataAdapter("Select * from LPR_PlateHits", Constants.str_SqlCon);
                dataAdapter.Fill(dt_SQLData);

                // Loop through CSV File and save the row id for any rows already in the DB (based on PK).
                List<DataRow> rows_to_remove = new List<DataRow>();
                foreach (DataRow csvRow in dt_CSVData.Rows)
                {
                    foreach (DataRow sqlRow in dt_SQLData.Rows)
                    {
                        if (csvRow["pk"].ToString() == sqlRow["pk"].ToString())
                        {
                            rows_to_remove.Add(csvRow);
                        }
                    }
                }

                // Loop through rows to remove, and remove them.
                foreach (DataRow row in rows_to_remove)
                {
                    dt_CSVData.Rows.Remove(row);
                    dt_CSVData.AcceptChanges();
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
                        foreach (DataRow csvRow in dt_CSVData.Rows) // For each record, we're going to do a separate insert.
                        {
                            using (sql_Command = new SqlCommand("Insert Into LPR_PlateHits (" + str_FieldNames + ") VALUES (" + str_ValueNames + ")", sql_Connection))
                            {
                                foreach (DataColumn csvColumn in dt_CSVData.Columns) // For each available column, add the parameter.
                                {
                                    sql_Command.Parameters.AddWithValue("@" + csvColumn.ColumnName, csvRow[csvColumn.ColumnName].ToString());
                                }

                                sql_Connection.Open();
                                sql_Command.ExecuteNonQuery();
                                sql_Connection.Close();
                            }
                        }
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
            }

            // Save Import History
            using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (sql_Command = new SqlCommand("Insert Into LPR_ImportHistory (Import_Time, Count_Imported, Count_Skipped) VALUES (GetDate(), @Count_Imported, @Count_Skipped)", sql_Connection))
                {
                    sql_Command.Parameters.AddWithValue("@Count_Imported", dt_CSVData.Rows.Count);
                    sql_Command.Parameters.AddWithValue("@Count_Skipped", TotalCSVCount - dt_CSVData.Rows.Count);
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

        private void GenerateAlerts(DataTable dt_NewEntries) // Takes the data just uploaded and checks to see if an e-mail alert is needed.
        {
            try
            {
                //For each Row...
                foreach (DataRow row1 in dt_NewEntries.Rows)
                {
                    String searchPlate = row1["best_plate"].ToString();
                    String imagePlate = row1["best_uuid"].ToString();
                    String alertAddress = "";
                    String Description = "";
                    String Status = "";
                    String Year = "";
                    String Make = "";
                    String Model = "";

                    using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
                    {
                        using (sql_Command = new SqlCommand("Exec sp_LPR_PlateAlerts @Plate", sql_Connection))
                        {
                            sql_Command.Parameters.AddWithValue("@Plate", searchPlate);
                            sql_Connection.Open();

                            using (SqlDataReader db_reader = sql_Command.ExecuteReader())
                            {
                                while (db_reader.Read())
                                {
                                    alertAddress = db_reader[0].ToString();
                                    Description = db_reader[1].ToString();
                                    Status = db_reader[2].ToString();
                                    Year = db_reader[3].ToString();
                                    Make = db_reader[4].ToString();
                                    Model = db_reader[5].ToString();
                                }
                            }
                            sql_Connection.Close();
                        }
                    }

                    if (alertAddress != "")
                    {
                        if (Constants.pushToken != "" && Constants.pushUser != "")
                        {
                            Alert_Push("Watched Plate", "You may be interested in this plate that just went by the house...");
                        }

                        if (Constants.emailSignIn != "")
                        {
                            Image img_Full;
                            //Gets Image and saves the full image
                            WebClient wc = new WebClient();
                            byte[] bytes = wc.DownloadData(Constants.str_WebServer + "/img/" + imagePlate + ".jpg");
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                            img_Full = Image.FromStream(ms);
                            Alert_Email(img_Full, alertAddress, searchPlate, Description, Status, Year, Make, Model);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                write_event(e.Message.ToString(), EventLogEntryType.Error);
            }     
        }

        private void Alert_Email(Image img_ToEmail, string str_EmailToAddress, string str_Plate, string D, String S, String Y, String Ma, String Mo)
        {
            MailMessage mail = new MailMessage();
            mail.From = new System.Net.Mail.MailAddress(Constants.emailSignIn);
            mail.To.Add(new MailAddress(str_EmailToAddress));
            mail.IsBodyHtml = true;
            mail.Subject = "License Plate Alert: " + str_Plate;
            string st = "A flagged license plate has been spotted!";
            st += "<br />Status: " + S;
            st += "<br />Description: " + D;
            st += "<br />Year: " + Y;
            st += "<br />Make: " + Ma;
            st += "<br />Model: " + Mo;
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

        public void Alert_Push (string thisTitle, string thisMessage, string thisDevice = "DefaultDevice")
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("token", Constants.pushToken);
                reqparm.Add("user", Constants.pushUser);
                reqparm.Add("device", thisDevice);
                reqparm.Add("title", thisTitle);
                reqparm.Add("message", thisMessage);
                reqparm.Add("sound", "pushover");

                var responsebytes = client.UploadValues("https://api.pushover.net/1/messages.json", "POST", reqparm);
                var responsebody = (new System.Text.UTF8Encoding()).GetString(responsebytes);
            }
        }

        private void HistoryGrid_Load()
        {
            dataAdapter = new SqlDataAdapter("Select Top 200 Import_Time as [Time], Count_Imported as [Imported], Count_Skipped as [Skipped] from LPR_ImportHistory Order By Import_Time Desc", Constants.str_SqlCon);
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
    }
}
