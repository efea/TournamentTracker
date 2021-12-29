using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        IPrizeRequester callingForm;
        public CreatePrizeForm(IPrizeRequester caller)
        {
            InitializeComponent();
            /*
             *  create a variable at the class level to store what is passed to the Ctor.
             *  We need to know the caller outside of the scope of the Ctor.
             */
            callingForm = caller;
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PrizeModel model = new PrizeModel(
                    placeNameValue.Text, 
                    placeNumberValue.Text, 
                    prizeAmountValue.Text,
                    prizePercentageValue.Text);
                /*
                foreach(IDataConnection db in GlobalConfig.Connections)
                {
                    db.CreatePrize(model);
                }*/
                GlobalConfig.Connection.CreatePrize(model);

                /*
                 * I say now, to whoever called this, I created the prize
                 * and here is the fully created model.
                 */
                callingForm.PrizeComplete(model);

                this.Close();
                //placeNameValue.Text = "";
                //placeNumberValue.Text =  "";
                //prizeAmountValue.Text = "0";
                //prizePercentageValue.Text = "0";

            }
            else
            {
                MessageBox.Show("This form has invalid information. Please check and try again.");
            }
        }

        private bool ValidateForm()
        {
            bool output = true;
            int placeNumber = 0;
            decimal prizeAmount = 0;
            double prizePercentage = 0;

            if(!int.TryParse(placeNumberValue.Text, out placeNumber) || placeNumber < 1)
            {
                output = false;
            }

            if (placeNameValue.Text.Length == 0) output = false;

            if (!decimal.TryParse(prizeAmountValue.Text, out prizeAmount)
                || !double.TryParse(prizePercentageValue.Text, out prizePercentage)) output = false;

            if (prizeAmount <= 0 && prizePercentage <= 0) output = false;

            if (prizePercentage < 0 || prizePercentage > 100) output = false;

            return output;
        }
    }
}
