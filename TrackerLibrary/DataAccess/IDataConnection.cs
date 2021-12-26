using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    //remember thsi is a contract
    public interface IDataConnection
    {
        //remember, this is a contract. so we need no public keyword in front of the method
        //all classes that 'sign' this contract, has to obey AT LEAST the methods and properties mentioned
        //in this contract.

        //We add NO code into this method.
        //As, any class that implements from this interface might have radically
        //different functionality of the code.

        PrizeModel CreatePrize(PrizeModel model);

    }
}
