namespace HashSet.Lib
{
    public interface IHashSet<T> 
        where T : SPSStudent, IEquatable<T>
    {
        T Add(T value);
        bool IsPresent(T value);
        void Rebalance();
    }

    public interface SPSStudent : IEquatable<SPSStudent>
    {
        string Name { get; }
        string Year { get; }
        string Tutor { get; }
    }
    public class Student : SPSStudent
    {
        public string Name {get;}
        public string Year { get; }
        public string Tutor { get;}

        public Student(string name, string year, string tutor)
        {
            Name = name;
            Year = year;
            Tutor = tutor;
        }
        
        public override string ToString()
        {
            return $"{Name} + {Year} + {Tutor}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        


    }
}
