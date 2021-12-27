using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
using System.Linq;

namespace TrackerLibrary.DataAccess
{

    //When implementing from an interface, we can have multiple that we implement
    //ex. public class example : IDataConnection, IDisposible

    //Remember tho, when using inheriting, we bring code down with us. we can NOT inherit from two different classes


    /*
     * The plan is as follows
     * -load the test file
     * -find the id of the last record
     * -add new record with new id
     * -convert the prizes to a List<string>
     * -save List<string> to the text file.
     */
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PeopleFile = "PersonModels.csv";

        /*
        decided to use 1 file per model so each of them will have its own txt file, just as each model has its own 
        table in the SQL.

        all information will be stored in the same spot. same path.

         */
        public PrizeModel CreatePrize(PrizeModel model)
        {
            /*
            * -observe I don't pass anything as parameter. this is an extension method, so it takes the
            *   PrizesFile as parameter.
            *   Thanks to (this string file) in the methods prototype.

            * -Same thing for LoadFile method exactly.
            * 
            *
            * -Absolutely the same thing as the others.
            * 
            * - brought you by... the courtesy of extension methods.
            */
            //Loads and converts text file int List<PrizeModel>
            List<PrizeModel> prizes =  PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            //order the list as descending by the Id, then get its id and increment by 1 to find the current id which is what
            //I will use to write the new record.

            int currentId = 1;
            if(prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;

            prizes.Add(model);

            prizes.SaveToPrizeFile(PrizesFile);

            return model;

        }

        /*
         * Completely same sort of logic as the CreatePrize method above. 
         */
        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentId = 1;
            if(people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;

            }
            //set the id of the model that was passed in as the currentId
            model.Id = currentId;

            people.Add(model);

            people.SaveToPeopleFile(PeopleFile);

            return model;

        }





    }
}
