namespace QuickGraph.Tests.Serialization
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using QuickGraph.Algorithms;
    using QuickGraph.Serialization;

    using Xunit;

    #region Enumerations

    /// Enumeration of the person's gender
    public enum Gender
    {
        /// Male gender.
        Male,

        /// Female gender.
        Female
    }

    /// Enumeration of the person's age group
    public enum AgeGroup
    {
        /// Unknown age group.
        Unknown,

        /// 0 to 20 age group.
        Youth,

        /// 20 to 40 age group.
        Adult,

        /// 40 to 65 age group.
        MiddleAge,

        /// Over 65 age group.
        Senior
    }

    #endregion

    /// Representation for a single serializable Person.
    /// INotifyPropertyChanged allows properties of the Person class to
    /// participate as source in data bindings.
    public class Person : INotifyPropertyChanged,
                          IEquatable<Person>
    {
        #region IEquatable Members

        /// Determine equality between two person classes
        /// <param name="other" />
        /// An object to compare with this object.
        /// true if the current object is equal to the other parameter; otherwise, false.
        public bool Equals(Person other)
        {
            return Id == other.Id;
        }

        #endregion

        #region Methods

        /// Returns a String that represents the current Object.
        /// 
        /// A String that represents the current Object.
        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Fields and Constants

        private const string DefaultFirstName = "Unknown";

        private string _id;

        private string _firstName;

        private string _lastName;

        private string _middleName;

        private string _suffix;

        private string _nickName;

        private string _maidenName;

        private Gender _gender;

        private DateTime? _birthDate;

        private string _birthPlace;

        private DateTime? _deathDate;

        private string _deathPlace;

        private bool _isLiving;

        #endregion

        #region Constructors

        /// Initializes a new instance of the Person class.
        /// Each new instance will be given a unique identifier.
        /// This parameterless constructor is also required for serialization.
        public Person()
        {
            _id = Guid.NewGuid().ToString();
            _firstName = DefaultFirstName;
            _isLiving = true;
        }

        /// Initializes a new instance of the person class with the firstname and the lastname.
        /// <param name="firstName" />
        /// First name.
        /// <param name="lastName" />
        /// Last name.
        public Person(string firstName, string lastName)
            : this()
        {
            // Use the first name if specified, if not, the default first name is used.
            if (!string.IsNullOrEmpty(firstName))
            {
                _firstName = firstName;
            }

            _lastName = lastName;
        }

        /// Initializes a new instance of the person class with the firstname, the lastname, and gender
        /// <param name="firstName" />
        /// First name.
        /// <param name="lastName" />
        /// Last name.
        /// <param name="gender" />
        /// Gender of the person.
        public Person(string firstName, string lastName, Gender gender)
            : this(firstName, lastName)
        {
            _gender = gender;
        }

        #endregion

        #region Properties

        /// Gets or sets the unique identifier for each person.
        [XmlAttribute]
        public string Id
        {
            get { return _id; }

            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        /// Gets or sets the name that occurs first in a given name
        [XmlElement]
        public string FirstName
        {
            get { return _firstName; }

            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged("FirstName");
                    OnPropertyChanged("Name");
                    OnPropertyChanged("FullName");
                }
            }
        }

        /// Gets or sets the part of a given name that indicates what family the person belongs to.
        [XmlElement]
        public string LastName
        {
            get { return _lastName; }

            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged("LastName");
                    OnPropertyChanged("Name");
                    OnPropertyChanged("FullName");
                }
            }
        }

        /// Gets or sets the name that occurs between the first and last name.
        [XmlElement]
        public string MiddleName
        {
            get { return _middleName; }

            set
            {
                if (_middleName != value)
                {
                    _middleName = value;
                    OnPropertyChanged("MiddleName");
                    OnPropertyChanged("FullName");
                }
            }
        }

        /// Gets the person's name in the format FirstName LastName.
        [XmlIgnore]
        public string Name
        {
            get
            {
                var name = string.Empty;
                if (!string.IsNullOrEmpty(_firstName))
                {
                    name += _firstName;
                }

                if (!string.IsNullOrEmpty(_lastName))
                {
                    name += " " + _lastName;
                }

                return name;
            }
        }

        /// Gets the person's fully qualified name: Firstname MiddleName LastName Suffix
        [XmlIgnore]
        public string FullName
        {
            get
            {
                var fullName = string.Empty;
                if (!string.IsNullOrEmpty(_firstName))
                {
                    fullName += _firstName;
                }

                if (!string.IsNullOrEmpty(_middleName))
                {
                    fullName += " " + _middleName;
                }

                if (!string.IsNullOrEmpty(_lastName))
                {
                    fullName += " " + _lastName;
                }

                if (!string.IsNullOrEmpty(_suffix))
                {
                    fullName += " " + _suffix;
                }

                return fullName;
            }
        }

        /// Gets or sets the text that appear behind the last name providing additional information about the person.
        [XmlElement]
        public string Suffix
        {
            get { return _suffix; }

            set
            {
                if (_suffix != value)
                {
                    _suffix = value;
                    OnPropertyChanged("Suffix");
                    OnPropertyChanged("FullName");
                }
            }
        }

        /// Gets or sets the person's familiar or shortened name
        [XmlElement]
        public string NickName
        {
            get { return _nickName; }

            set
            {
                if (_nickName != value)
                {
                    _nickName = value;
                    OnPropertyChanged("NickName");
                }
            }
        }

        /// Gets or sets the person's name carried before marriage
        [XmlElement]
        public string MaidenName
        {
            get { return _maidenName; }

            set
            {
                if (_maidenName != value)
                {
                    _maidenName = value;
                    OnPropertyChanged("MaidenName");
                }
            }
        }

        /// Gets or sets the person's gender
        [XmlElement]
        public Gender Gender
        {
            get { return _gender; }

            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        /// Gets the age of the person.
        [XmlIgnore]
        public int? Age
        {
            get
            {
                if (BirthDate == null)
                {
                    return null;
                }

                // Determine the age of the person based on just the year.
                var startDate = BirthDate.Value;
                var endDate = IsLiving || DeathDate == null
                                  ? DateTime.Now
                                  : DeathDate.Value;
                var age = endDate.Year - startDate.Year;

                // Compensate for the month and day of month (if they have not had a birthday this year).
                if (endDate.Month < startDate.Month || endDate.Month == startDate.Month && endDate.Day < startDate.Day)
                {
                    age--;
                }

                return Math.Max(0, age);
            }
        }

        /// Gets the age of the person.
        [XmlIgnore]
        public AgeGroup AgeGroup
        {
            get
            {
                var ageGroup = AgeGroup.Unknown;

                if (Age.HasValue)
                {
                    // The AgeGroup enumeration is defined later in this file. It is up to the Person
                    // class to define the ages that fall into the particular age groups
                    if (Age >= 0 && Age < 20)
                    {
                        ageGroup = AgeGroup.Youth;
                    }
                    else if (Age >= 20 && Age < 40)
                    {
                        ageGroup = AgeGroup.Adult;
                    }
                    else if (Age >= 40 && Age < 65)
                    {
                        ageGroup = AgeGroup.MiddleAge;
                    }
                    else
                    {
                        ageGroup = AgeGroup.Senior;
                    }
                }

                return ageGroup;
            }
        }

        /// Gets the year the person was born
        [XmlIgnore]
        public string YearOfBirth
        {
            get
            {
                if (_birthDate.HasValue)
                {
                    return _birthDate.Value.Year.ToString(CultureInfo.CurrentCulture);
                }
                return "-";
            }
        }

        /// Gets the year the person died
        [XmlIgnore]
        public string YearOfDeath
        {
            get
            {
                if (_deathDate.HasValue && !_isLiving)
                {
                    return _deathDate.Value.Year.ToString(CultureInfo.CurrentCulture);
                }
                return "-";
            }
        }

        /// Gets or sets the person's birth date.  This property can be null.
        [XmlElement]
        public DateTime? BirthDate
        {
            get { return _birthDate; }

            set
            {
                if (_birthDate == null || _birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged("BirthDate");
                    OnPropertyChanged("Age");
                    OnPropertyChanged("AgeGroup");
                    OnPropertyChanged("YearOfBirth");
                    OnPropertyChanged("BirthMonthAndDay");
                    OnPropertyChanged("BirthDateAndPlace");
                }
            }
        }

        /// Gets or sets the person's place of birth
        [XmlElement]
        public string BirthPlace
        {
            get { return _birthPlace; }

            set
            {
                if (_birthPlace != value)
                {
                    _birthPlace = value;
                    OnPropertyChanged("BirthPlace");
                    OnPropertyChanged("BirthDateAndPlace");
                }
            }
        }

        /// Gets the month and day the person was born in. This property can be null.
        [XmlIgnore]
        public string BirthMonthAndDay => _birthDate?.ToString(DateTimeFormatInfo.CurrentInfo.MonthDayPattern, CultureInfo.CurrentCulture);

        /// Gets a friendly string for BirthDate and Place
        [XmlIgnore]
        public string BirthDateAndPlace
        {
            get
            {
                if (_birthDate == null)
                {
                    return null;
                }
                var returnValue = new StringBuilder();
                returnValue.Append("Born ");
                returnValue.Append(
                    _birthDate.Value.ToString(
                        DateTimeFormatInfo.CurrentInfo.ShortDatePattern,
                        CultureInfo.CurrentCulture));

                if (!string.IsNullOrEmpty(_birthPlace))
                {
                    returnValue.Append(", ");
                    returnValue.Append(_birthPlace);
                }

                return returnValue.ToString();
            }
        }

        /// Gets or sets the person's death of death.  This property can be null.
        [XmlElement]
        public DateTime? DeathDate
        {
            get { return _deathDate; }

            set
            {
                if (_deathDate == null || _deathDate != value)
                {
                    IsLiving = false;
                    _deathDate = value;
                    OnPropertyChanged("DeathDate");
                    OnPropertyChanged("Age");
                    OnPropertyChanged("YearOfDeath");
                }
            }
        }

        /// Gets or sets the person's place of death
        [XmlElement]
        public string DeathPlace
        {
            get { return _deathPlace; }

            set
            {
                if (_deathPlace != value)
                {
                    IsLiving = false;
                    _deathPlace = value;
                    OnPropertyChanged("DeathPlace");
                }
            }
        }

        /// Gets or sets a value indicating whether the person is still alive or deceased.
        [XmlElement]
        public bool IsLiving
        {
            get { return _isLiving; }

            set
            {
                if (_isLiving != value)
                {
                    _isLiving = value;
                    OnPropertyChanged("IsLiving");
                }
            }
        }

        /// Gets a string that describes this person to their parents.
        [XmlIgnore]
        public string ParentRelationshipText => _gender == Gender.Male
                                                    ? "Son"
                                                    : "Daughter";

        /// Gets a string that describes this person to their siblings.
        [XmlIgnore]
        public string SiblingRelationshipText => _gender == Gender.Male
                                                     ? "Brother"
                                                     : "Sister";

        /// Gets a string that describes this person to their spouses.
        [XmlIgnore]
        public string SpouseRelationshipText => _gender == Gender.Male
                                                    ? "Husband"
                                                    : "Wife";

        /// Gets a string that describes this person to their children.
        [XmlIgnore]
        public string ChildRelationshipText => _gender == Gender.Male
                                                   ? "Father"
                                                   : "Mother";

        #endregion

        #region INotifyPropertyChanged Members

        /// INotifyPropertyChanged requires a property called PropertyChanged.
        public event PropertyChangedEventHandler PropertyChanged;

        /// Fires the event for the property when it changes.
        /// <param name="propertyName" />
        /// Property name.
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfo Members

        /// Gets an error message indicating what is wrong with this object.
        public string Error => null;

        /// Gets the error message for the property with the given name.
        /// <param name="columnName" />
        /// The name of the property whose error message to get.
        /// The error message for the property. The default is an empty string ("").
        public string this[string columnName]
        {
            get
            {
                var result = string.Empty;

                if (columnName == "BirthDate")
                {
                    if (BirthDate == DateTime.MinValue)
                    {
                        result = "This does not appear to be a valid date.";
                    }
                }

                if (columnName == "DeathDate")
                {
                    if (DeathDate == DateTime.MinValue)
                    {
                        result = "This does not appear to be a valid date.";
                    }
                }

                return result;
            }
        }

        #endregion
    }

    public class Repro13482Test
    {
        [Fact]
        public void Repro13482()
        {
            var graph = new AdjacencyGraph<Person, TaggedEdge<Person, string>>();

            var jacob = new Person("Jacob", "Hochstetler")
                        {
                            BirthDate = new DateTime(1712, 01, 01),
                            BirthPlace = "Alsace, France",
                            DeathDate = new DateTime(1776, 01, 01),
                            DeathPlace = "Pennsylvania, USA",
                            Gender = Gender.Male
                        };

            var john = new Person("John", "Hochstetler")
                       {
                           BirthDate = new DateTime(1735, 01, 01),
                           BirthPlace = "Alsace, France",
                           DeathDate = new DateTime(1805, 04, 15),
                           DeathPlace = "Summit Mills, PA",
                           Gender = Gender.Male
                       };

            var jonathon = new Person("Jonathon", "Hochstetler")
                           {
                               BirthPlace = "Pennsylvania",
                               DeathDate = new DateTime(1823, 05, 08),
                               Gender = Gender.Male
                           };

            var emanuel = new Person("Emanuel", "Hochstedler")
                          {
                              BirthDate = new DateTime(1855, 01, 01),
                              DeathDate = new DateTime(1900, 01, 01),
                              Gender = Gender.Male
                          };

            graph.AddVerticesAndEdge(new TaggedEdge<Person, string>(jacob, john, jacob.ChildRelationshipText));
            graph.AddVerticesAndEdge(new TaggedEdge<Person, string>(john, jonathon, john.ChildRelationshipText));
            graph.AddVerticesAndEdge(new TaggedEdge<Person, string>(jonathon, emanuel, jonathon.ChildRelationshipText));

            var settings = new XmlWriterSettings
                           {
                               Indent = true,
                               IndentChars = @"    "
                           };
            using (var writer = XmlWriter.Create(Console.Out, settings))
            {
                graph.SerializeToXml(
                    writer,
                    v => v.Id,
                    AlgorithmExtensions.GetEdgeIdentity(graph),
                    "graph",
                    "person",
                    "relationship",
                    "");
            }
        }
    }
}