namespace CA1_Hospital
{
    public class Patient
    {
        #region Enum
        //Enum with diffrent blood types
        public enum BloodType { A, B, AB, O }
        #endregion

        #region Properties
        //Properties
        public string Name { get; set; }

        public double DateOfBirth { get; set; }
        public BloodType Blood { get; set; }
        #endregion

        #region Constructors

        //Constructors
        public Patient()
        {

        }

        public Patient(string name, double dateOfBirth)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
        }

        public Patient(string name, double dateOfBirth, BloodType blood)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Blood = blood;
        }



        #endregion

        #region ToString method
        //ToString Method

        public override string ToString()
        {
            return string.Format($"{Name} ({DateOfBirth} years) Type: {Blood}");
        }
        #endregion
    }
}