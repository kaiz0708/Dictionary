using MySql.Data.MySqlClient;

namespace Dictionary.Database
{
    public class ConnectDB
    {
        
            public MySqlConnection Connect()
            {
                MySqlConnection connect;
                string connectString = "server=localhost;database=dictionary;uid=root;password=kyanh0708";
                connect = new MySqlConnection(connectString);
                try
                {
                    connect.Open();
                    Console.WriteLine("Success");
                    connect.Close();
                }
                catch
                {
                    Console.WriteLine("Fail");
                }
                return connect;
            }
        }
    
}
