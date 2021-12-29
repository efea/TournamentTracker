 using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models

{
    public class TournamentModel
    {
        /// <summary>
        /// Represents the tournament name
        /// </summary>
        public string TournamentName { get; set; }
        /// <summary>
        /// Represents the entry fee for the tournament
        /// </summary>
        public decimal EntryFee { get; set; }
        /// <summary>
        /// Represents the teams that will compete in the tournament.
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();
        /// <summary>
        /// Represents the prizes that will be handed out
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();
        /// <summary>
        /// Represents each round of the tournament.
        /// Each round of the tournament will consist several matchups.
        /// Hence a list of lists.
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();

        /// <summary>
        /// The unique identifier for the tournament.
        /// </summary>
        public int Id { get; set; }
    }
}
