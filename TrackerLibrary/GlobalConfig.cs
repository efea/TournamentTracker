using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    //public static means that we can NOT instantiate the class GlobalConfig
    //always visible for everyone.

    //usually it would be a bad idea to put any data into such global class. However, in this case
    //we actually need global data.
    public static class GlobalConfig
    {
        //private set means that only methods inside the GlobalConfig class can set the values of the variable Connections.
        //everyone can read still

        /*Decided to use a List of IDataConnection as we can have multiple sources to save or pull data from.
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();*/

        /*
         *Actually, after seeing the Id in the model will cause problem, for example, same prize can
         *have a different id in text and sql, i decided not to use a list of IDataConnection but a single one.
         *
         */

        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType db)
        {
            /*
            switch + tab + tab
            automatically creates the 
            switch on db 
             
            switch (db)
            {
                case DatabaseType.Sql:
                    break;
                case DatabaseType.TextFile:
                    break;
                default:
                    break;
            }
            */

            if (db == DatabaseType.Sql)
            {
                //TODO - set up the sql connector properly.
                /*
                 notice here that, we can add our SqlConnector instance to our Connections list.
                eventhough, it is a list of IDataConnection. 

                Our SqlConnector implements IDataConnection, thus, we can add it to the Connections list.
                In fact, this is the whole point why I am using interfaces.
                 */
                SqlConnector sql = new SqlConnector();
                Connection = sql;
            }
            else if (db == DatabaseType.TextFile)
            {
                //TODO - create the text connection.
                /*
                 same as SqlConnector.
                 */
                TextConnector text = new TextConnector();
                Connection = text;
            }
        }
        public static string CnnString(string name)
        {
           return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
