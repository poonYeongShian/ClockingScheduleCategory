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

                // SQL check the company id not in schedule category and insert default schedule category
                if (InsertDefaultScheduleCategory(_replicaConnectionString))
                {
                    Log.Information("Default schedule category inserted successfully");
                }
                // SQL update schedule's category id, change 1,2,3,4 to its corresponding category id in schedule_category table
                if (UpdateScheduleCategoryId(_replicaConnectionString))
                {
                    Log.Information("Schedule category id updated successfully");
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while running the service");
            }
            finally
            {
                Log.Information("Service stopped");
            }
        }

        static bool InsertDefaultScheduleCategory(string replicaConnectionString)
        {
            _clockingScheduleDA = new ClockingScheduleDA(_replicaConnectionString);

            bool isSuccess = _clockingScheduleDA.InsertDefaultScheduleCategory() > 0;

            return isSuccess;
        }        
        static bool UpdateScheduleCategoryId(string replicaConnectionString)
        {
            _clockingScheduleDA = new ClockingScheduleDA(_replicaConnectionString);

            bool isSuccess = _clockingScheduleDA.UpdateScheduleCategoryId();

            return isSuccess;
        }
    }
}
