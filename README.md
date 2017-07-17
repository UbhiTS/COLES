# COLES
Class Objects Logic Equations and Serializer (COLES) allows you to create "and, or" expression equations using custom class objects and then serialize them to a string representation

## How to use
Inherit the classes you want to objectify and serialize from the GenericCriteria class (feel free to rename the classes) and override the ToString() method as per your serialization requirements
```c#
// Name class containing FirstName and LastName properties
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

// Gender class containing GenderType property
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
```

Now you can include these classes in an equation as below

```C#

// John Doe who is a Male and lives in Zip 94587 or 94338
var search1 = new Name("John", "Doe") & new Gender(Gender.GenderType.Male) & (new ZipCode("94587") | new ZipCode("94338"));
Console.WriteLine(search1.ToString());
//OUTPUT: ((NAME:Doe,John && GENDER:Male) && (ZIPCODE:94587 || ZIPCODE:94338))

// John Doe who is a Male or Unknown Gender and lives in Zip 94587 or 94338
var search2 = new Name("John", "Doe") & (new Gender(Gender.GenderType.Male) | new Gender(Gender.GenderType.Unspecified)) & (new ZipCode("94587") | new ZipCode("94338"));
Console.WriteLine(search2.ToString());
//OUTPUT: ((NAME:Doe,John && (GENDER:Male || GENDER:Unspecified)) && (ZIPCODE:94587 || ZIPCODE:94338))

// John Doe who is a Male and lives in Zip 94587 and his Age is 35, 40 or 45
var search3 = new Name("John", "Doe") & (new Gender(Gender.GenderType.Male) & (new ZipCode("94587") & (new Age(35) | new Age(40) | new Age(45) )));
Console.WriteLine(search3.ToString());
//OUTPUT: (NAME:Doe,John && GENDER:Male && ZIPCODE:94587 && (AGE:35 || AGE:40 || AGE:45))

```

