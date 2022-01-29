using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Newtonsoft.Json;

namespace CA1_Hospital
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Setup a observable collection

        ObservableCollection<Ward> wardsInfo = new ObservableCollection<Ward>();

        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
            //Creates a ward object hard coded
            Ward ward1 = new Ward { Name = "Marx Brothers Ward", Capacity = 3 };
            Ward ward2 = new Ward { Name = "Adams Family Ward", Capacity = 4 };

            //Creates patient objects hard coded
            Patient p1 = new Patient { Name = "Chico", DateOfBirth = 69, Blood = "A" };
            Patient p2 = new Patient { Name = "Harpo", DateOfBirth = 75, Blood = "AB" };
            Patient p3 = new Patient { Name = "Groucho", DateOfBirth = 69, Blood = "O" };

            Patient p4 = new Patient { Name = "Kevin", DateOfBirth = 32, Blood = "B" };
            Patient p5 = new Patient { Name = "James", DateOfBirth = 35, Blood = "AB" };
            Patient p6 = new Patient { Name = "John", DateOfBirth = 71, Blood = "O" };
            Patient p7 = new Patient { Name = "Alex", DateOfBirth = 55, Blood = "A" };

            ward1.patientsInfo = new ObservableCollection<Patient> { p1, p2, p3 };
            ward2.patientsInfo = new ObservableCollection<Patient> { p4, p5, p6, p7 };

            //adds the wards to the list collection
            wardsInfo.Add(ward1);
            wardsInfo.Add(ward2);

            //displays the collection 
            lbWardList.ItemsSource = wardsInfo;

            //automatically displays the first ward
            lbWardList.SelectedIndex = 0;

            //Displays Ward Count
             tblWardList.Text = string.Format($"Ward List ({wardsInfo.Count})");

        }//Startup code 


        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)//Creates a Slider
        {
            tblkSlider.Text = string.Format($"{slider.Value:F0}");
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)//Adds patient
        {

            //Read info from screen
            string name = tbxPatientName.Text;
            int age = Convert.ToInt32(DateTime.Now.Year - dateTimePicker.SelectedDate.Value.Year);
            string bType;

           

            //If statements for which radio button picked and assigns the blood type of patient 
            if (rbtnA.IsChecked == true)
            {
               //Converts enum to a string
                bType = Convert.ToString(Patient.BloodType.A);
               

            }
            else if (rbtnB.IsChecked == true)
            {
                bType = Convert.ToString(Patient.BloodType.B);

            }
            else if (rbtnAB.IsChecked == true)
            {
                bType = Convert.ToString(Patient.BloodType.AB);

            }
            else
            {
                bType = Convert.ToString(Patient.BloodType.O);

            }

            //Create Patient object
            Patient p1 = new Patient(name, age, bType);

            Ward w1 = lbWardList.SelectedItem as Ward; 

            
            //Add to collection
            w1.patientsInfo = new ObservableCollection<Patient> {p1};
 
            //Add to wards collection
            w1.patientsInfo.Add(p1);

            //displays the collection 
            lbPatient.ItemsSource = w1.patientsInfo;



        }

        private void btnAddWard_Click(object sender, RoutedEventArgs e) //Adds ward
        {
           

            //Reads values input by user
            string wardName = tbxWardName.Text;
            int wardCapacity = Convert.ToInt32(slider.Value);

            //Create Ward Object

            Ward w1 = new Ward(wardName, wardCapacity);

            //Add to collection
            wardsInfo.Add(w1);

           

        }

        private void btnWardSave_Click(object sender, RoutedEventArgs e) //Saves ward objects to JSON
        {
            //get stringof objects - JSON formatted

            string wardJSON = JsonConvert.SerializeObject(wardsInfo,Formatting.Indented);

            //Write to file
            
            using (StreamWriter sw = new StreamWriter(@"c:\temp\wardInfo.json"))
            {
                sw.Write(wardJSON); 
            }
        }

        private void btnWardLoad_Click(object sender, RoutedEventArgs e)//Code to load JSON for ward
        {
            //Connect to file
            using(StreamReader sr = new StreamReader(@"c:\temp\wardInfo.json"))
            {
                //Read text

                string jsonWard = sr.ReadToEnd();

                //Convert to json
                wardsInfo = JsonConvert.DeserializeObject<ObservableCollection<Ward>>(jsonWard);

                //refresh display
                lbWardList.ItemsSource = wardsInfo;
            }


        }

        private void btnPatientSave_Click(object sender, RoutedEventArgs e)//Saves patient objects to JSON file
        {
            Ward p1 = new Ward();
            //get stringof objects - JSON formatted

            string patientJSON = JsonConvert.SerializeObject(p1.patientsInfo, Formatting.Indented);

            //Write to file

            using (StreamWriter sw = new StreamWriter(@"c:\temp\patientsInfo.json"))
            {
                sw.Write(patientJSON);
            }
        }

        private void btnPatientLoad_Click(object sender, RoutedEventArgs e)//Loads patient objects from JSON file
        {
            Ward w1 = new Ward();
            //Connect to file
            using (StreamReader sr = new StreamReader(@"c:\temp\patientsInfo.json"))
            {
                //Read text

                string jsonPatient = sr.ReadToEnd();

                //Convert to json
                w1.patientsInfo = JsonConvert.DeserializeObject<ObservableCollection<Patient>>(jsonPatient);

                //refresh display
                lbPatient.ItemsSource = w1.patientsInfo;
            }
        }

        private void lbWardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //selects item from the listbox
            Ward w1 = (Ward)lbWardList.SelectedItem;

            if (lbWardList.SelectedItem != null)
            {
                lbPatient.ItemsSource = w1.patientsInfo;
                //picks first paient in the patients listbox
                lbPatient.SelectedIndex = 0;
            }
           
           
        }//Selects a ward that displays patients in the selected ward

        private void lbPatient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Patient p1 = (Patient)lbPatient.SelectedItem;

            //Allows for selection to be null if nothing selected
           
            if (p1 != null)
            {
                //Outputs Patient name into a text box

                tblPatientDetails.Text = p1.Name;

                //If statement to decide which picture to use with each patient

                if (p1.Blood == "A")
                {
                    imgBloodType.Source = new BitmapImage(new Uri("/Images/a.png", UriKind.Relative));
                }

                else if (p1.Blood == "B")
                {
                    imgBloodType.Source = new BitmapImage(new Uri("/Images/b.png", UriKind.Relative));
                }
                else if (p1.Blood == "AB")
                {
                    imgBloodType.Source = new BitmapImage(new Uri("/Images/ab.png", UriKind.Relative));
                }
                else
                {
                    imgBloodType.Source = new BitmapImage(new Uri("/Images/o.png", UriKind.Relative));
                }
            }


        }//Selects an object in the patient listbox and outputs patient details with a name and picture
    }
}
