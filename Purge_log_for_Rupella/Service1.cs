using System;
using System.IO;
using System.ServiceProcess;
using System.Configuration;

namespace Purge_log_for_Rupella
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Purge Log Service is started at " + DateTime.Now);
            Deletefile(); // This Method for Deleting files in DeleteData after 7 days.
        }
        protected void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Deletefile();
        }
        public void Deletefile()
        {
            try
            {
                string[] orgFiles = Directory.GetFiles(ConfigurationManager.AppSettings["OrganizationLogs"]);

                for (int i = 0; i < orgFiles.Length; i++)
                {
                    //Here we will find wheter the file is 7 days old
                    if (DateTime.Now.Subtract(File.GetCreationTime(orgFiles[i])).TotalDays > 7)
                    {
                        File.Delete(orgFiles[i]);
                    }
                    WriteToFile("Organization Logs Deleted Successfully for Past 7 Days");
                }
                WriteToFile("OrgService is stopped at " + DateTime.Now);
                string[] contactFiles = Directory.GetFiles(ConfigurationManager.AppSettings["ContactLogs"]);

                for (int i = 0; i < contactFiles.Length; i++)
                {
                    //Here we will find wheter the file is 7 days old
                    if (DateTime.Now.Subtract(File.GetCreationTime(contactFiles[i])).TotalDays > 7)
                    {
                        File.Delete(contactFiles[i]);
                    }
                    WriteToFile("Contact Logs Deleted Successfully for Past 7 Days");
                }
                WriteToFile("ContactService is stopped at " + DateTime.Now);
                string[] logsFiles = Directory.GetFiles(ConfigurationManager.AppSettings["Logs"]);

                for (int i = 0; i < logsFiles.Length; i++)
                {
                    //Here we will find wheter the file is 7 days old
                    if (DateTime.Now.Subtract(File.GetCreationTime(logsFiles[i])).TotalDays > 7)
                    {
                        File.Delete(logsFiles[i]);
                    }
                    WriteToFile("Logs Deleted Successfully for Past 7 Days");
                }
                WriteToFile("LogsService is stopped at " + DateTime.Now);
            }
            catch (Exception ex)
            {
                WriteToFile("Exception occurred " + ex);
                WriteToFile("Exception caught at " + DateTime.Now);
            }
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
