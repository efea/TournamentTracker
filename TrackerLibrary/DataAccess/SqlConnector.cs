using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrackerLibrary.Models;
using System.Data.SqlClient;
using Dapper;
using System.Linq;


/*
 
 
 	@PlaceNumber int,
	@PlaceName nvarchar(50),
	@PrizeAmount money,
	@PrizePercentage float,
 
 */
namespace TrackerLibrary.DataAccess
{
    //implements the IDataConnection
    public class SqlConnector : IDataConnection
    {
        private const string db = "Tournaments";
        /// <summary>
        /// Saves a new prize to the database.
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The prize information including the unique identifier.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            /*
            using statement
            we have this connection which we open. and we use the using statement so that
            whenever we hit the closing curly brackets of the using statement, whatever happens
            connection is closed and disposed.

            That's why the any type that is inside the using statement has to implement IDisposable.
             */
           using(IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);

                /*
                To the dynamicParameters p, we should add the @id variable.
                we set it to 0. it is of DbType int32
                direction is output since we will get it as an output of database.
                */
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                /*
                now this Execute method will execute the store procedure we defined, pass all the information we defined
                as p, which is of type dynamic parameters to the store procedure
                 */
                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);
                /*
                 * looks at the dynamic parameters list p 
                 * find the @id which we gave to it as parameter
                 * it says
                 * give me the value of @id and it is of type int
                 */
                model.Id = p.Get<int>("@id");

                return model;

            }
        }

        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@CellphoneNumber", model.CellphoneNumber);
                p.Add("@EmailAddress", model.EmailAddress);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);


                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                return model;

            }
        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;

            using(IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }  

            return output;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");


                foreach (PersonModel tm in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonId", tm.Id);

                    connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }
            }

            return model;

        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<TeamModel>("dbo.spTeams_GetAll").ToList();


                /*
                 *  For each team in the resulting list of TeamModel
                 *  I need to query their teammembers as the list I get does not provide that.
                 *  
                 *  For each team, I am calling my store procedure which 
                 *  Takes the TeamId as parameter and then
                 *  take the conjunction of the tables People and TeamMembers where People.Id = TeamMember.PersonId
                 *  where TeamId is the team we passed as the parameter.
                 */
                foreach(TeamModel team in output)
                {
                    var p = new DynamicParameters();

                    //note how I pass the TeamId as a parameter to my stored procedure.
                    p.Add("@TeamId", team.Id);

                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure ).ToList();
                }
            }

            return output;
        }

        public void CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                SaveTournament(model, connection);

                SaveTournamentPrizes(model, connection);

                SaveTournamentEntries(model, connection);

                SaveTournamentRounds(model, connection);
            }
        }

        private void SaveTournament(TournamentModel model, IDbConnection connection)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFee", model.EntryFee);

            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.spTournament_Insert", p, commandType: CommandType.StoredProcedure);

            model.Id = p.Get<int>("@id");
        }

        private void SaveTournamentPrizes(TournamentModel model, IDbConnection connection)
        {
            foreach (PrizeModel pz in model.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@PrizeId", pz.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.TournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);

            }
        }

        private void SaveTournamentEntries(TournamentModel model, IDbConnection connection)
        {

            foreach (TeamModel tm in model.EnteredTeams)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@TeamId", tm.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);

            }
        }

        private void SaveTournamentRounds(TournamentModel model, IDbConnection connection)
        {
            /*
             * What do I need to save? 
             * I have to have the order while saving. 
             * Each round depend on the information of the previous roundç
             */
            // List<List<MatchupModel>> Rounds
            //List<MatchupEntryModel> Entries

            //loop through the rounds
                //loop through the matchupsmodels
                //save the matchup
                //loop throught its entries(matchupentries) which are 2, and save them.
            
            
            foreach(List<MatchupModel> round in model.Rounds){

                foreach(MatchupModel matchup in round)
                {
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@MatchupRound", matchup.MatchupRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                   
                    connection.Execute("dbo.spMatchups_Insert", p, commandType: CommandType.StoredProcedure);
                    //now i ve saved the matchup to the db and got back its id. 
                    matchup.Id = p.Get<int>("@id");

                    //now I can go through the matchup entries of the matchup, and save them!
                    foreach(MatchupEntryModel me in matchup.Entries)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", matchup.Id);

                        if (me.ParentMatchup == null)
                        {
                            p.Add("@ParentMatchupId", null);
                        }
                        else
                        {
                            p.Add("@ParentMatchupId", me.ParentMatchup.Id);
                        }

                        if (me.TeamCompeting == null)
                        {
                            p.Add("@TeamCompetingId", null);
                        }
                        else
                        {
                            p.Add("@TeamCompetingId", me.TeamCompeting.Id);
                        }

                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);


                        connection.Execute("dbo.spMatchupEntries_Insert", p, commandType: CommandType.StoredProcedure);

                    }


                }

            }

        }

    }
}
