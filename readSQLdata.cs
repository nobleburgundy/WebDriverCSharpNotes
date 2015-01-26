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
