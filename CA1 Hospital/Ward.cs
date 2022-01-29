using System.Collections.ObjectModel;

namespace CA1_Hospital
{
    public class Ward
    {
        #region Properties

        //Properties
        public int WardID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public ObservableCollection<Patient> PatientsInfo { get; set; }
        public static int NumberOfWards { get; set; }
        #endregion

        #region Constructors

        //Constructors
        public Ward()
        {

        }

        public Ward(string name)
        {
            Name = name;
           
        }

        public Ward(string name, int capacity)
        {
           Name = name;
           Capacity = capacity;
        }

        #endregion

        #region ToString methdod

        public override string ToString()
        {
            return string.Format($"{Name}\t(Limit: {Capacity})");
        }
        #endregion
    }
}