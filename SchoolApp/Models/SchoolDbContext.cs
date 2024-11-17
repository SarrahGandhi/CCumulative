using MySql.Data.MySqlClient;

namespace SchoolApp.Models
{
    /// <summary>
    /// Provides database connection functionality for the School database.
    /// </summary>
    public class SchoolDbContext
    {
        // Input the details of the username, password, server, and port number to connect the server to the database
        private static string User { get { return "root"; } }
        private static string Password { get { return ""; } }
        private static string Database { get { return "school"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }
        /// <summary>
        /// Constructs the connection string using the provided credentials and database settings.
        /// </summary>
        protected static string ConnectionString
        {
            get
            {
                // The "convert zero datetime" option is used to handle dates like 0000-00-00 in MySQL.

                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }



        /// <summary>
        /// Creates and returns a connection to the database.
        /// </summary>
        /// <example>
        /// var dbContext = new SchoolDbContext();
        /// MySqlConnection connection = dbContext.AccessDatabase();
        /// </example>
        /// <returns>A <see cref="MySqlConnection"/> object representing the database connection.</returns>
        public MySqlConnection AccessDatabase()
        {
            // We are giving instance to SchoolDbContext class to create an object
            // The object is a specific connection to our school database on port 3306 of localhost
            return new MySqlConnection(ConnectionString);
        }
    }
}
