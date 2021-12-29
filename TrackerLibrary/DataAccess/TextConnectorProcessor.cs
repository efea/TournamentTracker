using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;

/*
 * I manually added the .TextConnector to this namespace
 * Did this edit so that, only the using statements[meaning namespaces] will receive this clutter
 * that this will create.
 */

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        /*
         *  this string filename means that this is an extension method.
         *  
         *  
         */
        public static string FullFilePath(this string fileName) //PrizeModels.csv
        {
            /*
             * I use the $"" string formatting. 
             * This is more intuitive and performent.
             * 
             * {ConfigurationManager.AppSettings["filePath"]}  -> get the filePath from the app.config, app settings.
             * 
             * C:\Users\efea\OneDrive - RRI\Documents\tests_and_learning\c_#\Tournament_Data\PrizeModels.csv
             * 
             */
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
        }
        /*
         * Takes in full file path, returns a list of string, which represents all lines of the file.
         */
        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                //entries are seperated by commmas so we ll put it in a string called cols
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                output.Add(p);
            }
            return output;
        }
        /*
         * Basically same as the ConvertToPrizeModels method.
         */
        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PersonModel p = new PersonModel();

                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.CellphoneNumber = cols[4];
                output.Add(p);

            }
            return output;
        
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName) {

            //id,teamName,list of ids separated by the | 
            //3,Team c7, 1|3|5 ----> id of team, name of team, each of id of each person of the team.
            //we then need to pull the information on the members by querying by their id numbers.

            List<TeamModel> output = new List<TeamModel>();

            //list of people.
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();


            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TeamModel t = new TeamModel();

                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] personIds = cols[2].Split('|');

                foreach(string id in personIds)
                {

                   /*
                    *   take the list of people, 
                    *   search the list to find the id matching the id in my personIds array.
                    *   if everything is fine, we should return a single id.
                    *   however, c# obviously does NOT know that so it tries to return a list.
                    *   Therefore, I take the First() item in it, which should be the only item anyways.
                    *   Here i am deliberately not using FirstOrDefault to return null if the list is empty
                    *   I WANT my application to crash(for right now) if the List is empty.
                    */
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }
                output.Add(t);
            }

            return output;


        
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach(PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);

        }

        public static void SaveToPeopleFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            //creates a line in the form
            //id,firstname,lastname,email,cellnmber
            //adds it to the list<string> lines where each element of the list is a line that represents a person.
            foreach(PersonModel p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.EmailAddress},{p.CellphoneNumber}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);

        }

        public static void SaveToTeamFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach(TeamModel t in models)
            {
                lines.Add($"{t.Id},{t.TeamName},{ConvertPeopleListToString(t.TeamMembers)}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        /// <summary>
        /// helper function to convert a list of Personmodel to string where each person id is 
        /// delimited by | character
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";

            if (people.Count == 0) return "";

            foreach(PersonModel p in people)
            {
                output += $"{p.Id}|";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }
    
    
    

    }
}
