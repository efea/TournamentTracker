using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{

    //When implementing from an interface, we can have multiple that we implement
    //ex. public class example : IDataConnection, IDisposible

    //Remember tho, when using inheriting, we bring code down with us. we can NOT inherit from two different classes

    public class TextConnector : IDataConnection
    {
        //TODO - Implement the CreatePrize for text files.
        public PrizeModel CreatePrize(PrizeModel model)
        {
            model.Id = 1;
            return model;
        }
    }
}
