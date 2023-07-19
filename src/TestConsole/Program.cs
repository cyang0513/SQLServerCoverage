using Microsoft.Data.SqlClient;
using System.Data;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test started...");
            var cover = new SQLServerCoverage.CodeCoverage("Data Source=.;Initial Catalog=MRE_RO_DEV;Integrated Security=True;TrustServerCertificate=True", "MRE_RO_DEV");
            cover.Start(30000);

            using (var sqlCon = new SqlConnection("Data Source=.;Initial Catalog=MRE_RO_DEV;Integrated Security=True;TrustServerCertificate=True"))
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("[TestHelpers].[p_TruncateDerivativeData]", sqlCon);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.ExecuteNonQuery();

                Console.WriteLine($"Done!");

                sqlCon.Close();
            }

            var result = cover.Stop();

            result.SaveSourceFiles(".\\SqlSource", true);
            result.ToOpenCoverXml("opencover.xml");
            result.ToHtml("coverage.html", ".\\SqlSource", "opencover.xml");

            Console.WriteLine("Test done...");

            Console.ReadKey();
        }
    }
}
