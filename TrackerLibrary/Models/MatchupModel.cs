using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        /// <summary>
        /// represents the unique identifier for a matchup
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents the matchups
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
        /// <summary>
        /// Represents the winner in of the matchup.
        /// </summary>
        public TeamModel Winner { get; set; }
        /// <summary>
        /// Represents at which round the matchup happened.
        /// </summary>
        public int MatchupRound { get; set; }
    }
}
