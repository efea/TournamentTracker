using System;
using System.Collections.Generic;
using System.Text;


//I really add model to the name of this class to differentiate this type of
//object from anything else.
namespace TrackerLibrary.Models
{
    public class TeamModel
    {
        //prop + tab + tab
        //built in visual studio snippet that creates an auto property
        //        public int MyProperty { get; set; }

        //ctor + tab + tab
        //creates constructor.
        /* public TeamModel()
        {

        }
        */

        //i make sure that i initialize the TeamMembers list before anything else.

        /// <summary>
        /// Represents the members of the team.
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
        /// <summary>
        /// Represents the name of the team.
        /// </summary>
        public string TeamName { get; set; }

        public int Id { get; set; }


    }
}
