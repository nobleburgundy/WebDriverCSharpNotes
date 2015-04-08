// quick sample of reading SQL data
// will need an ODBC connection made(in this case: ImperialQA)

using System.Data.Odbc;

        [TestMethod]
        public void ODBC_TEST()
        {
            OdbcConnection cmd = new OdbcConnection("DSN=ImperialQA");
            cmd.Open();
            OdbcCommand dbCommand = cmd.CreateCommand();
            dbCommand.CommandText = "select (sql stuff here)";
            OdbcDataReader dbReader = dbCommand.ExecuteReader();
            dbReader.Read(); // read it once
            Debug.WriteLine("field count {0}", dbReader.FieldCount); // works
            Debug.WriteLine("Data {0}", dbReader.GetString(0));
            dbReader.Close(); // close it nicely
        }


// better way of doing it.
// no ODBC connection needs to be set
// Windows credentials do the authentication for you

using System.Data.SqlClient;

        internal void SQLCheck(string myUser="thisuser")
        {
            string sqlcomommand = "SELECT * from WHATEVER " +
                                  " WHERE ";

            SqlConnection myConnection = new SqlConnection("server=pdentqa.pdental.com\\pattdent;" +
                " integrated security=true;" +
                " connection timeout=90;");

            try
            {
                myConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;  // give up
            }
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand(sqlcomommand, myConnection);
            myCommand.ExecuteNonQuery();  // run it
            myReader = myCommand.ExecuteReader();
            if (myReader.HasRows == true)
                Debug.WriteLine("SQL verification checks correctly.");
                while (myReader.Read())
                    {
                        Console.WriteLine(myReader["OrderNumber"].ToString());
                        Console.WriteLine(myReader["DateOrdered"].ToString());
                    }                
            else
                Debug.WriteLine("SQL verification failed. No rows returned.");




            try
            {
                myConnection.Close();  // close it
            }

            catch (Exception e)
            {
                Debug.WriteLine("failure on closing SQL connection.");
                Debug.WriteLine(e.ToString());
            }
        }
