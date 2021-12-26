using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;

/*
 * I manually added the .TextConnector to this namespace
 * Did this edit so that, only the using() statements will receive this clutter
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

        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach(PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);

        }
    }
}
