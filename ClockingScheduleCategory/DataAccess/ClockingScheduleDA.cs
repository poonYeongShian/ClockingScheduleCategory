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

        internal int InsertDefaultScheduleCategory()
        {
            string query = $@"INSERT INTO schedule_category (`company_id`, `name`, `code`, `is_default`, `created_on`, `updated_on`)
                                SELECT 
                                    `Company_ID`, 
                                    `name`, 
                                    `code`,
                                    is_default, 
                                    @createdOn, 
                                    @createdOn
                                FROM (
                                    SELECT DISTINCT s.Company_ID
                                    FROM `schedule` s
                                    WHERE NOT EXISTS (
                                        SELECT 1
                                        FROM schedule_category sc
                                        WHERE s.Company_ID = sc.company_id
                                    )
                                ) AS sch
                                CROSS JOIN default_schedule_category dsc
                                ORDER BY `Company_ID`, `auto_no`;";

            return _DBHelper.CreateRecord(query, new List<MySqlParameter>()
            {
                new MySqlParameter("@createdOn", DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"))
            });
        }

        internal bool UpdateScheduleCategoryId()
        {
            string query = $@"UPDATE `schedule` s
                                JOIN schedule_category sc ON s.Company_ID = sc.company_id 
                                     AND s.ScheCategory = sc.code
                              SET s.ScheCategory = sc.auto_no
                              WHERE s.ScheCategory != 0;";

            return _DBHelper.UpdateRecord(query, new List<MySqlParameter>());
        }
    }
}
