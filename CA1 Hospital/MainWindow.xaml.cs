using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace CA1_Hospital
{
    //Student Name: Kevin Doherty
    //Student Number: S00125770


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Observable collections and patientFileLocation
        //Setup a observable collection
        private ObservableCollection<Ward> wardsInfo = new ObservableCollection<Ward>();

        public string patientFileLocation { get; set; }

        #endregion

        #region Startup code and Window Loaded
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Error handling if any issues observed in the app 
            try
            {
                // Prevent future dates and the maximum age is set to 130 as a maximum
                dateTimePicker.DisplayDateEnd = DateTime.Today;
                dateTimePicker.DisplayDateStart = DateTime.Today.AddYears(-130);

                //Creates a ward object hard coded
                Ward ward1 = new Ward { WardID = 1, Name = "Marx Brothers Ward", Capacity = 5 };
                Ward ward2 = new Ward { WardID = 2, Name = "Adams Family Ward", Capacity = 6 };

                
                Patient p1 = new Patient { Name = "Chico", DateOfBirth = 77, Blood = Patient.BloodType.A };
                Patient p2 = new Patient { Name = "Harpo", DateOfBirth = 78, Blood = Patient.BloodType.B };
                Patient p3 = new Patient { Name = "Groucho", DateOfBirth = 55, Blood = Patient.BloodType.O };

                Patient p4 = new Patient { Name = "Kevin", DateOfBirth = 27, Blood = Patient.BloodType.O };
                Patient p5 = new Patient { Name = "James", DateOfBirth = 25, Blood = Patient.BloodType.B };
                Patient p6 = new Patient { Name = "John", DateOfBirth = 26, Blood = Patient.BloodType.AB };
                Patient p7 = new Patient { Name = "Alex", DateOfBirth = 28, Blood = Patient.BloodType.B };

                ward1.PatientsInfo = new ObservableCollection<Patient> { p1, p2, p3 };
                ward2.PatientsInfo = new ObservableCollection<Patient> { p4, p5, p6, p7 };

                //adds the wards to the list collection
                wardsInfo.Add(ward1);
                wardsInfo.Add(ward2);

                //displays the collection
                lbWardList.ItemsSource = wardsInfo;

                //automatically displays the first ward and patient
                lbWardList.SelectedIndex = 0;
                lbPatient.SelectedIndex = 0;

                //Gets the count of wards and displays it
                Ward.NumberOfWards = GetTotalWards();
                tblWardList.Text = string.Format("Ward List ({0})", Ward.NumberOfWards);

                // Set patient file location
                patientFileLocation = Environment.CurrentDirectory + @"\PatientsInfo.json";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to load application correctly. Message: {ex.Message}");
            }
        }//Startup code
        #endregion

        #region Slider object
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)//Creates a Slider
        {
            tblkSlider.Text = string.Format($"{slider.Value:F0}");
        }
        #endregion

        #region click events
        private void btnAddWard_Click(object sender, RoutedEventArgs e) //Adds ward
        {
            //Reads values input by user
            string wardName = tbxWardName.Text;
            int wardCapacity = Convert.ToInt32(slider.Value);

            //Create Ward Object

            Ward w1 = new Ward(wardName, wardCapacity);

            //Add to collection
            wardsInfo.Add(w1);
            tblWardList.Text = string.Format("Ward List ({0})", GetTotalWards());
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)//Adds patient
        {
            //Selects ward as ward
            Ward selectedWard = lbWardList.SelectedItem as Ward;

            //Creates a new collection for patients if empty
            if (selectedWard.PatientsInfo == null)
            {
                selectedWard.PatientsInfo = new ObservableCollection<Patient>();
            }

            //If statement to check capacity is enough for patient numbers
            if (selectedWard.PatientsInfo.Count != selectedWard.Capacity)
            {
                //Reads info from screen
                string name = tbxPatientName.Text;
                double age = GetAge();
                Patient.BloodType bType;

                //If statements for which radio button picked and assigns the blood type of patient
                if (rbtnA.IsChecked == true)
                {
                    //Converts enum to a string
                    bType = Patient.BloodType.A;
                }
                else if (rbtnB.IsChecked == true)
                {
                    bType = Patient.BloodType.B;
                }
                else if (rbtnAB.IsChecked == true)
                {
                    bType = Patient.BloodType.AB;
                }
                else
                {
                    bType = Patient.BloodType.O;
                }

                //Creates new Patient
                Patient newPatient = new Patient(name, age, bType);

                //Adds patient to collection
                selectedWard.PatientsInfo.Add(newPatient);

                //Shows patient in listbox
                lbPatient.ItemsSource = selectedWard.PatientsInfo;
            }
            else
            {
                MessageBox.Show($"Not enough capacity in ward {selectedWard.Name}. Please add Paitent {tbxPatientName.Text} to another ward.");
            }
        }
        #endregion

        #region save/Load JSON file
        private void btnWardSave_Click(object sender, RoutedEventArgs e) 
        {
            //Error handling for saving file
            try
            {
                string wardJSON = JsonConvert.SerializeObject(wardsInfo, Formatting.Indented);

                //Write to file

                using (StreamWriter sw = new StreamWriter(patientFileLocation))
                {
                    sw.Write(wardJSON);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to save Patient data");
            }
        }//Saves to JSON file

        private void btnWardLoad_Click(object sender, RoutedEventArgs e)
        {
            //Using try catch for error handling if no save file is detected
            try
            {
                //Connect to file
                using (StreamReader sr = new StreamReader(patientFileLocation))
                {
                    //Read text
                    string jsonWard = sr.ReadToEnd();

                    //Convert to json
                    wardsInfo = JsonConvert.DeserializeObject<ObservableCollection<Ward>>(jsonWard);

                    //refresh display
                    lbWardList.ItemsSource = wardsInfo;

                    //automatically displays the first ward and patient
                    lbWardList.SelectedIndex = 0;
                    lbPatient.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to load patient data");
            }
        }//LoadS to JSON file
        #endregion

        #region Selection events
        private void lbWardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Selects Ward from list box as the ward
            Ward selectedWard = lbWardList.SelectedItem as Ward;

            //If statement to allow nulls
            if (selectedWard != null)
            {
                lbPatient.ItemsSource = selectedWard.PatientsInfo;
                lbPatient.SelectedIndex = 0;
            }
        }//Selects a ward that displays patients in the selected ward

        private void lbPatient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Selects patient in list box as patient
            Patient selectedPatient = lbPatient.SelectedItem as Patient;

            //Allows for selection to be null if nothing selected

            if (selectedPatient != null)
            {
                //Outputs Patient name into a text box
                tblPatientDetails.Text = selectedPatient.Name;

                //If statement to decide which picture to use with each patient
                if (selectedPatient.Blood == Patient.BloodType.A)
                {
                    imgBloodType.Source = new BitmapImage(new Uri("/Images/a.png", UriKind.Relative));
                }
                else if (selectedPatient.Blood == Patient.BloodType.B)
                {
                    imgBloodType.Source = new BitmapImage(new Uri("/Images/b.png", UriKind.Relative));
                }
                else if (selectedPatient.Blood == Patient.BloodType.AB)
                {
                    imgBloodType.Source = new BitmapImage(new Uri("/Images/ab.png", UriKind.Relative));
                }
                else
                {
                    imgBloodType.Source = new BitmapImage(new Uri("/Images/o.png", UriKind.Relative));
                }
            }
        }//Selects an object in the patient listbox and outputs patient details with a name and picture
        #endregion

        #region Methods
        private int GetTotalWards()
        {
            //initialise variable to 0
            int total = 0;

            //foreach searches how many wards are in the list
            foreach (Ward wardCount in wardsInfo)
            {
                //Adds that value to a variable
                total = wardsInfo.Count;
            }

            return total;
        }//Method to get Ward count

        private double GetAge()
        {
            //Initialises variable to 0
            double age = 0;

            //Gets the year from date picker and subtracts from todays date
            age = DateTime.Now.Year - dateTimePicker.SelectedDate.Value.Year;

            //If statement to account for days and take away a year to get an accurate date of birth
            if (DateTime.Now.DayOfYear < dateTimePicker.SelectedDate.Value.DayOfYear)
            {
                age -= 1;
            }
            return age;
        }//Metod to getAge
        #endregion
    }
}