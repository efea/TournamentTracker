using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerUI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
             So this is our main. Once this method is done, application closes
             
            Application.Run(new TournamentDashboardForm());

            this line creates a new dashboardform instance. 
            What that line actually does is:
                - Pauses this thread, which is mentioned at line 14
                - says 'don't end this line until instance of the new tournamentdashboard closes'

            If we ever close that form, the application closes.
            
            OK Microsoft lmao
             */

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Initialize the database connections
            TrackerLibrary.GlobalConfig.InitializeConnections(TrackerLibrary.DatabaseType.Sql);

            Application.Run(new CreateTournamentForm());
            //Application.Run(new TournamentDashboardForm());
        }
    }
}