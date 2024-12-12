using System;
using System.Collections.Generic;
using System.Data;
using App_General;
using MySql.Data.MySqlClient;

namespace ClockingScheduleCategory.DataAccess
{
    internal class ClockingScheduleDA
    {
        DBHelper _DBHelper;

        internal ClockingScheduleDA(string connectionString)
        {
            _DBHelper = new DBHelper(connectionString);
        }

        internal DataTable GetDistinctCompanyIds()
        {
            DataTable dt = new DataTable();
            string query = $@"SELECT DISTINCT s.Company_ID
                              FROM schedule s
                              WHERE NOT EXISTS (
                                  SELECT 1
                                  FROM schedule_category sc
                                  WHERE s.Company_ID = sc.Company_ID
                              );";

            return _DBHelper.RetrieveRecord(query, new List<MySqlParameter>());
        }
    }
}
