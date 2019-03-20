using System;
using System.Data;
using System.Data.SqlClient;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Test.DataSimulation.Models
{
    internal class DatabaseBuilder
    {
        private static string InitDatabaseName
        {
            get
            {
                var connectionStringItems = ConfigHelper.Conn.Split(';');
                foreach (var connectionStringItem in connectionStringItems)
                {
                    if (!connectionStringItem.Contains("Catalog") && !connectionStringItem.Contains("catalog"))
                    {
                        continue;
                    }
                    return connectionStringItem.Split('=')[1].Trim();
                }
                throw new NotFoundException("Not found InitDatabaseName.");
            }
        }

        public static string GetDbDefaultFilePath()
        {
            var cmdText = "declare @DbDefaultFile nvarchar(1000) " +
                          "exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\\Microsoft\\Microsoft SQL Server\\MSSQLServer', N'DefaultData', @DbDefaultFile output " +
                          "select @DbDefaultFile as 'DbDefaultFileName'";
            using (var conn = new SqlConnection(ConfigHelper.MasterConn))
            {
                var scalar = SqlHelper.ExecuteScalar(conn, cmdText, CommandType.Text, null);
                if (string.IsNullOrEmpty(scalar.ToString()))
                {
                    cmdText = "select [fileName] from [sysaltfiles] where [name]='master'";
                    scalar = SqlHelper.ExecuteScalar(conn, cmdText, CommandType.Text, null);
                }
                if (string.IsNullOrEmpty(scalar.ToString()))
                {
                    throw new ArgumentNullException(ExMessage.MustNotBeNull("scalar"));
                }
                scalar = scalar.ToString().Replace("\\master.mdf", "");
                return scalar.ToString();
            }
        }

        public static void CreateInitDatabase()
        {
            var dbDefaultFilePath = GetDbDefaultFilePath();
            var cmdText =
                $"create database {InitDatabaseName} on primary(name={InitDatabaseName}_mdf,filename='{dbDefaultFilePath}\\{InitDatabaseName}.mdf') " +
                $"log on(name={InitDatabaseName}_log,filename='{dbDefaultFilePath}\\{InitDatabaseName}_log.ldf')";
            using (var conn = new SqlConnection(ConfigHelper.MasterConn))
            {
                SqlHelper.ExecuteNonQuery(conn, cmdText, CommandType.Text, null);
            }
        }
    }
}