using COLES.Infrastructure;

namespace COLES
{
    #region Criterias

    public abstract class GenericCriteria : ExpressionElement
    {
        public string CriteriaType { get; set; }

        #region Interface Methods

        public override string ToString()
        {
            return "GENERIC";
        }

        #endregion
    }

    public class Name : GenericCriteria
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Name(string firstName, string lastName)
        {
            CriteriaType = "NAME";
            FirstName = firstName;
            LastName = lastName;
        }

        #region Interface Methods

        public override string ToString()
        {
            return CriteriaType + ":" + LastName + "," + FirstName;
        }

        #endregion
    }    

    public class Age : GenericCriteria
    {
        public int Years { get; set; }
        public Age(int years)
        {
            CriteriaType = "AGE";
            Years = years;
        }

        #region Interface Methods

        public override string ToString()
        {
            return CriteriaType + ":" + Years;
        }

        #endregion
    }

    public class Gender : GenericCriteria
    {
        public enum GenderType
        {
            Unspecified,
            Male,
            Female
        }

        public GenderType PersonGender { get; set; }
        public Gender(GenderType personGender)
        {
            CriteriaType = "GENDER";
            PersonGender = personGender;
        }

        #region Interface Methods

        public override string ToString()
        {
            return CriteriaType + ":" + PersonGender.GetDescription();
        }

        #endregion
    }

    public class ZipCode : GenericCriteria
    {
        public string Zip { get; set; }
        public ZipCode(string zip)
        {
            CriteriaType = "ZIPCODE";
            Zip = zip;
        }

        #region Interface Methods

        public override string ToString()
        {
            return CriteriaType + ":" + Zip;
        }

        #endregion
    }

    #endregion
}
