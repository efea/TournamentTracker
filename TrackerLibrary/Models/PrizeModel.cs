using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        /// <summary>
        /// The unique identifier for the prize.
        /// </summary>
         public int Id { get; set; }
        /// <summary>
        /// Represents the the place number.
        /// </summary>
        public int PlaceNumber { get; set; }
        /// <summary>
        /// Represents the NAME of the place number.
        /// </summary>
        public string PlaceName { get; set; }
        /// <summary>
        /// Represents the prize amount given to the placenumber.
        /// </summary>
        public decimal PrizeAmount { get; set; }
        /// <summary>
        /// represents the prize percentage given to the placenumber.
        /// </summary>
        public double PrizePercentage { get; set;}

        public PrizeModel()
        {

        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;

            int.TryParse(placeNumber, out int placeNumberValue);
            PlaceNumber = placeNumberValue;

            decimal.TryParse(prizeAmount, out decimal prizeAmountValue);
            PrizeAmount = prizeAmountValue;

            double.TryParse(prizePercentage, out double prizePercentageValue);
            PrizePercentage = prizePercentageValue;
        }
    }
}
