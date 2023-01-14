using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBulkCopySample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataTable dtable = new DataTable();
            dtable.Columns.Add("Id", typeof(int));
            dtable.Columns.Add("Name", typeof(string));
            dtable.Columns.Add("Description", typeof(string));
            var samples = Samples();
            foreach (var item in samples)
            {
                dtable.Rows.Add(null, item.Name, item.Description);
            }
            var statu = BulkCopy(dtable);
            Console.WriteLine(statu);
            Console.ReadLine();
        }
        public static string BulkCopy(DataTable dtable)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=Sample;Integrated Security=True";
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = "SampleTable";
                    // How many records to send to the database in one go (all of them)
                    bulkCopy.BatchSize = dtable.Rows.Count;

                    // Load the data to the database
                    bulkCopy.WriteToServer(dtable);

                    // Close up          
                    bulkCopy.Close();
                }
            }
            return "successful - " + dtable.Rows.Count;
        }
        public static List<SampleEntity> Samples()
        {
            var samples = new  List<SampleEntity>();
            for (int i = 1; i < 10000; i++)
            {
                var sample = new SampleEntity();
                sample.Name=i.ToString();
                sample.Description=i.ToString();
                samples.Add(sample);
            }
            return samples;
        }

            //        USE[Sample]
            //GO

            ///****** Object:  Table [dbo].[SampleTable]    Script Date: 14.01.2023 22:36:07 ******/
            //SET ANSI_NULLS ON
            //GO

            //SET QUOTED_IDENTIFIER ON
            //GO

            //CREATE TABLE[dbo].[SampleTable]
            //        (

            //    [Id][int] IDENTITY(1,1) NOT NULL,

            //    [Name] [nvarchar] (50) NULL,
            //	[Description][nvarchar] (50) NULL,
            // CONSTRAINT[PK_SampleTable] PRIMARY KEY CLUSTERED
            //(
            //    [Id] ASC
            //)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY]
            //) ON[PRIMARY]
            //GO
    }
}
