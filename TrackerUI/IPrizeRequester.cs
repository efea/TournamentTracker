using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerUI
{
    /*
     * I need to have a loose-coupling between the CreateTournamentForm and CreatePrize form
     * Since we need to be able to Create a prize from the CreateTournamentform.
     * The two forms should NOT be know about each other.
     * Therefore, the choice of using interface was obvious for this loose-coupling
     */
    public interface IPrizeRequester
    {

        void PrizeComplete(PrizeModel model);

    }
}
