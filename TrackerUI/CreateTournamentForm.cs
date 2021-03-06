using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> selectedTeams = new List<TeamModel>();

        List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        private void WireUpLists()
        {

            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            //The TeamName here is actually a variable. This is a c# thing where
            //the list will show the TeamName property in the drop down box.
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";

        }
        public CreateTournamentForm()
        {
            InitializeComponent();
            WireUpLists();
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            /**
             * 
             PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;

            if (p != null)
            {
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);
                WireUpLists(); 
            }
             */

            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;

            if(t != null)
            {
                availableTeams.Remove(t);
                selectedTeams.Add(t);
                WireUpLists();
            }

        }

        private void createPrize_Click(object sender, EventArgs e)
        {
            //Call the create prize form


            //Call the create prize form
            //this keyword here represents 'this' specific instance, being an instance of CreateTournamentForm class
            // but more importantly, this is an instance of a class which IMPLEMENTS IPrizeRequester!!!!


            //There we can pass the instance of CreateTournamentForm class as a parameter to CreatePrizeForm constructor
            //even though, the CreatePrizeForm constructor expects an IPrizeRequester. Essentially, CreateTournmanentForm is
            //implementing 'at least' the IPrizeRequester, so, it can be passed successfully!

            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();

            //get back from the form a PrizeModel.
  


        }

        public void PrizeComplete(PrizeModel model)
        {
            //get back from the form a PrizeModel.
            //well the model we pass as parameter IS the PrizeModel we want so this step is already complete.

            //Take the prize model and put it into the list of selected prizes.
            selectedPrizes.Add(model);
            WireUpLists();



        }
        //Absolutely the same thing with the interface as the prize.
        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void removeSelectedPlayerButton_Click(object sender, EventArgs e)
        {

            TeamModel t = (TeamModel)tournamentTeamsListBox.SelectedItem;
            if(t != null)
            {
                selectedTeams.Remove(t);
                availableTeams.Add(t);

                WireUpLists();
            }
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel p = (PrizeModel)prizesListBox.SelectedItem;
            if (p != null)
            {
                selectedPrizes.Remove(p);
                WireUpLists();
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            //Actually create the tournament itself, creating the record for it.

            //Create the prizes entries
            //create all the teams entries.
            //Create the matchups.

            //I absolutely need to create the tournament first, because I need the tournament Id
            //and Prize Id as an entry to my database table TournamentPrizes

            //validate data
            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out decimal fee);

            if (!feeAcceptable)
            {
                MessageBox.Show("Need to enter a valid entry fee.", 
                    "Invalid Fee", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }
            //Create my Tournamentmodel
            TournamentModel tm = new TournamentModel();

            tm.TournamentName = tournamentNameValue.Text;
            tm.EntryFee = fee;

            //Add all the selected prizes to the prizes of our tournament.
            tm.Prizes = selectedPrizes;
            //As with the prizes, add selectedteams to our tournament.
            tm.EnteredTeams = selectedTeams;

            //TODO - Wire up matchups
            //I moved the logic to CreateRounds to a seperate class.
            //Note that, as I don't think I will need to store anything in the TournamentLogic class, I made it static
            //Static classes cannot be instantiated with new() operator. 
            // TournamentLogic will do the work on the TournamentModel, meaning it will just modify it.
            TournamentLogic.CreateRounds(tm);

            GlobalConfig.Connection.CreateTournament(tm);

        }
    }
}
