using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using ClockingScheduleCategory.DataAccess;
using ClockingScheduleCategory.Dto;
using System.Data;
using System.Linq;

namespace ClockingScheduleCategory
{
    internal class Program
    {
        private static string _replicaConnectionString;
        private static ClockingScheduleDA _clockingScheduleDA;

        static void Main(string[] args)
        {
            Console.WriteLine("Clocking Category Start Running...");
            string logDirectory = "Log";
            Directory.CreateDirectory(logDirectory);

            Log.Logger = new LoggerConfiguration()
              .WriteTo.File($"Log/.txt", rollingInterval: RollingInterval.Day)
              .CreateLogger();

            Log.Information("Service started");

            try
            {
                _replicaConnectionString = ConfigurationManager.ConnectionStrings["LocalConnectionString"]?.ConnectionString.ToString();

                // Retrieve Company Id from schedule table
                var companyIds = InsertDefaultScheduleCategory(_replicaConnectionString); //3394 company id
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while running the service");
            }
            finally
            {
                Log.Information("Service stopped");

                // Loop through company, add default sche categories


                // Update schudule table with default schedule categories id


            }
        }

        static bool InsertDefaultScheduleCategory(string replicaConnectionString)
        {
            _clockingScheduleDA = new ClockingScheduleDA(_replicaConnectionString);

            bool isSuccess = _clockingScheduleDA.InsertDefaultScheduleCategory() > 0;

            return isSuccess;
            //    AutoNo = Convert.ToUInt64(dr["Auto_No"]),
            //    CompanyId = dr["Company_ID"].ToString(),
            //    UserID = Convert.ToUInt64(dr["UserID"]),
            //    UID = Convert.ToUInt64(dr["UID"]),
            //    CheckTime = Convert.ToDateTime(dr["CheckTime"]),
            //    TerminalSn = dr["SerialNo"].ToString(),
            //}).ToList();
            //return new List<int>();
        }
    }
}
