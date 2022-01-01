using System;
using System.Collections.Generic;
using System.Text;

/*
 When I dragged the Model .cs files into the Models folder, 

the namespace TrackerLibrary does NOT automatically change to TrackerLibrary.Models
since the rest of application use the TrackerLibrary namespace.

However, adding new classes in the Models folder, we can see the namespace of these new classes
would be
namespace TrackerLibrary.Models

I believe it is better to see in the development process that when we use these models,
we know they are models.

So, I manually changed the namespaces of these 6 models into

TrackerLibrary.Models


 */
namespace TrackerLibrary.Models
{
    /// a matchup entry is basically 1 team(half) of the matchup
    //cw + tab + tab, snippet for Console.WriteLine();
    public class MatchupEntryModel
    {

        /// <summary>
        /// represents the unique identifier for a matchup entry.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents one team in the matchup
        /// </summary>
        public TeamModel TeamCompeting { get; set; }
        /// <summary>
        /// Represents the score for this particular team
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// Represents the matchup that this team came from as the winner
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }
        
        
        /*
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="initialSCore"></param>
        public MatchupEntryModel(double initialSCore, TeamModel teamCompeting)
        {
            TeamCompeting = teamCompeting;

        }*/
    }
}
