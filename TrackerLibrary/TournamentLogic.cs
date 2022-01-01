using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;


namespace TrackerLibrary
{

    /*
     *  -I will first order my team list randomly.
     *  
     *  -Check if it is 2^n, if not, I will pad the teams until 2^n. Added teams will represent 'bye' rounds
     *  
     *  -Create the first round matchups.
     *  
     *  -Create every round after that.
     *  
     */
    public static class TournamentLogic
    {
        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

            CreateOtherRounds(model, rounds);
        }
        
        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;

            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();

            //foreach round of the tournament
            while(round <= rounds)
            {
                //for each matchup model in the previous round
                foreach(MatchupModel match in previousRound)
                {
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });
                    
                    //if we have more than 1 entry
                    if(currMatchup.Entries.Count > 1)
                    {
                        //set the current round of member
                        currMatchup.MatchupRound = round;
                        //add it to round
                        currRound.Add(currMatchup);
                        //reset the currMatchup
                        currMatchup = new MatchupModel();
                    }
                }
                model.Rounds.Add(currRound);
                previousRound = currRound;
                currRound = new List<MatchupModel>();
                round += 1;
            }


        }

        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel curr = new MatchupModel();

            foreach (TeamModel team in teams)
            {
                curr.Entries.Add(new MatchupEntryModel{TeamCompeting = team});

                //if we have a bye available. OR we have no byes but we have 2 teams. meaning
                //we are done with this matchup.
                if(byes > 0 || curr.Entries.Count > 1)
                {
                    //I am creating the first round so I set the round informatin
                    curr.MatchupRound = 1;
                    //add the matchup we created to our list of match up models which is our output.
                    output.Add(curr);
                    
                    //reset our current matchup so we dont need to create a new instance..
                    curr = new MatchupModel();
                    //if it was a bye that got us into this if statement, we 
                    if (byes > 0) byes -= 1;
                }
            }
            return output;
        }


        /// <summary>
        /// How many byes we ll have.
        /// </summary>
        /// <param name="rounds"></param>
        /// <param name="numberOfTeams"></param>
        /// <returns></returns>
        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            return (int)Math.Pow(2, rounds) - numberOfTeams;

        }

        /// <summary>
        /// finds the number of Rounds by applying ceiling and log2.
        /// </summary>
        /// <param name="teamCount"></param>
        /// <returns></returns>
        private static int FindNumberOfRounds(int teamCount)
        {
            return (int)Math.Ceiling(Math.Log2(teamCount));

        }
        
        /// <summary>
        /// Randomizes the teamList by ordering it on their Guid. Guid is 
        /// really a pseudorandom tag on the items of the list so. 
        /// it's actually a fast and clever way to order items pseudorandomly.
        /// Awesome
        /// LMAO
        /// </summary>
        /// <param name="teams"></param>
        /// <returns></returns>
        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            
            //Guid is sort of pseudorandom so. we can use this! lol
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
