using AmigueInvisible.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AmigueInvisible
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AmigueInvisibleSelection : Window
    {
        List<Participant> participants;
        List<Assignment> assignments;

        Participant selectedParticipant;

        int lastId;
        int deletedId;

        public AmigueInvisibleSelection()
        {
            InitializeComponent();
            this.lastId = 0;
            this.deletedId = 0;
            participants = new List<Participant>();
            assignments = new List<Assignment>();
            addedPeopleListView.ItemsSource = participants;
        }

        private void IntroNameButton_Click(object sender, RoutedEventArgs e)
        {
            string newPersonName = introNameTextBox.Text;
            int newPersonId;

            if (this.deletedId > 0)
            {
                newPersonId = this.deletedId;
                this.deletedId = 0;
            }
            else
            {
                this.lastId++;
                newPersonId = this.lastId;
            }

            participants.Add(new Participant()
            {
                Id = newPersonId,
                Name = newPersonName
            });

            this.refreshParticipantsList();
            introNameTextBox.Text = "";

            this.changeAddListButtonsEnable(true);

            this.assignments = new List<Assignment>();
            this.refreshAssignmentList();
        }

        private void addedPeopleListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedParticipant = e.AddedItems != null && e.AddedItems.Count > 0 
                                       ? (Participant) e.AddedItems[0]
                                       : null;
            this.changeButtonsVisibility(true);
            this.changeAddListButtonsEnable(false);
        }

        private void deletePeopleButton_Click(object sender, RoutedEventArgs e)
        {
            this.deletedId = this.selectedParticipant.Id;

            participants.Remove(this.selectedParticipant);
            this.refreshParticipantsList();

            this.changeButtonsVisibility(false);
            this.changeAddListButtonsEnable(true);
        }

        private void cancelPeopleButton_Click(object sender, RoutedEventArgs e)
        {
            this.selectedParticipant = null;
            this.changeButtonsVisibility(false);
            this.changeAddListButtonsEnable(true);
        }

        private void changeButtonsVisibility(bool visibilityToDisplay)
        {
            deletePeopleButton.Visibility = cancelPeopleButton.Visibility = visibilityToDisplay ? Visibility.Visible : Visibility.Hidden;
        }

        private void introNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            assignPeopleButton.IsEnabled = false;
        }

        private void refreshParticipantsList()
        {
            addedPeopleListView.ItemsSource = participants;
            addedPeopleListView.Items.Refresh();
        }

        private void refreshAssignmentList()
        {
            assignationsListView.ItemsSource = assignments;
            assignationsListView.Items.Refresh();
        }

        private void changeAddListButtonsEnable(bool isEnabled)
        {
            assignPeopleButton.IsEnabled = isEnabled;
            IntroNameButton.IsEnabled = isEnabled;
        }

        private void assignPeopleButton_Click(object sender, RoutedEventArgs e)
        {
            this.changeAddListButtonsEnable(false);

            List<Participant> possibleOptions = new List<Participant>(participants);

            for (int i=1; i< participants.Count; i++)
            {
                Participant assignedPerson = null;
                Participant currentParticipant = participants[i];
                bool wasRemoved = possibleOptions.Remove(currentParticipant);

                if(possibleOptions.Count > 1)
                {
                    Random rnd = new Random();
                    int assigneePosition = rnd.Next(1, possibleOptions.Count);
                    assignedPerson = possibleOptions[assigneePosition];
                }
                else
                {
                    assignedPerson = possibleOptions[0];
                }
                

                Assignment currentAssignment = new Assignment()
                {
                    Text = assignedPerson.Name + " ha sido asignado a " + currentParticipant.Name,
                    AssignedTo = currentParticipant,
                    Assignee = assignedPerson
                };
                this.assignments.Add(currentAssignment);
                this.refreshAssignmentList();

                if (wasRemoved)
                {
                    possibleOptions.Add(currentParticipant);
                }
                possibleOptions.Remove(assignedPerson);
            }

            this.changeAddListButtonsEnable(true);
        }

        private void newDrawbutton_Click(object sender, RoutedEventArgs e)
        {
            this.participants = new List<Participant>();
            this.refreshParticipantsList();

            this.assignments = new List<Assignment>();
            this.refreshAssignmentList();
        }
    }
}
