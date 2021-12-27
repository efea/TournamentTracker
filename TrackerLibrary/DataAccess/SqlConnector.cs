using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrackerLibrary.Models;
using System.Data.SqlClient;
using Dapper;


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
           using(IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
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
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
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
    }
}
