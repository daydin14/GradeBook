using System.Collections.Generic;

namespace GradeBook {

    public delegate void GradeAddedDelegate(object sender, EventArgs args);

    public class NamedObject {

        public NamedObject(string name) {Name = name;}
        public string Name {get; set;}
    }

    public interface IBook {
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name {get;}
        event GradeAddedDelegate GradeAdded;
    }

    public abstract class Book : NamedObject, IBook {
        public Book(string name) : base(name) {}
        public abstract event GradeAddedDelegate GradeAdded;
        public abstract void AddGrade(double grade);
        public abstract Statistics GetStatistics();
    }

    public class DiskBook : Book {
        public DiskBook(string name) : base(name) {}

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            using(var writer = File.AppendText($"{Name}.txt")) {
                writer.WriteLine(grade);
                if(GradeAdded != null) {
                    GradeAdded(this, new EventArgs());
                }
            }
        }

        public override Statistics GetStatistics() {
            var result = new Statistics();
            using(var reader = File.OpenText($"{Name}.txt")) {
                var line = reader.ReadLine();
                while(line != null) {
                    var number = double.Parse(line);
                    result.Add(number);
                    line = reader.ReadLine();
                }
            }
            return result;
        }
    }

    public class InMemoryBook : Book {

        private new string Name;
        private List<double> grades;

        public InMemoryBook(string name) : base(name) {
            this.Name = name;
            grades = new List<double>();
        }

        public void SetName(string name) {
            this.Name = name;
        }

        public string GetName() {
            return this.Name;
        }

        // Property of Name, can take place of the above 2 methods
        // public string BookName {
        //     // get{
        //     //     return this.Name;
        //     // }
        //     // set{
        //     //     if(string.IsNullOrEmpty(value)) {
        //     //         this.Name = value;
        //     //     }
        //     // }
        //     // Or can be done like this:
        //     get; set;
        // }

        public const string CATEGORY = "Science";

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade) {
            using(var writer = File.AppendText($"{Name}.txt")) {
                if(grade >= 0 && grade <= 100) {
                    this.grades.Add(grade);
                    writer.WriteLine(grade);
                    if(GradeAdded != null) {
                        GradeAdded(this, new EventArgs());
                    }
                } else {
                    //Console.WriteLine("Invalid value, please enter between range of 0-100.");
                    throw new ArgumentException($"Invalid {nameof(grade)}");
                }
            }
        }

        public void AddGrade(char letter) {
            switch(letter) {
                case 'A': 
                    this.AddGrade(90);
                    break;

                case 'B':
                    this.AddGrade(80);
                    break;

                case 'C':
                    this.AddGrade(70);
                    break;

                case 'D':
                    this.AddGrade(60);
                    break;

                default:
                    this.AddGrade(0);
                    break;
            }
        }

        // public double SumOfGrades() {
        //     var sum = 0.0;

        //     // Using foreach loop: ----------------------------------------------------------
        //     // foreach(double grade in this.grades) {
        //     //     sum += grade;
        //     // }
        //     // return sum;

        //     // Using do-while loop: ----------------------------------------------------------
        //     // if(this.grades.Count > 0) {
        //     //     var index = 0;
        //     //     do {
        //     //         sum += this.grades[index];
        //     //         index++;
        //     //     } while (index < this.grades.Count);

        //     //     return sum;
        //     // } else {
        //     //     Console.WriteLine("Cannot calculate Sum of Grades.");
        //     //     return 0.0;
        //     // }


        //     // Using for loop: ----------------------------------------------------------
        //     for(int i = 0; i < this.grades.Count; i++) {
        //         // if(this.grades[i] == 42.1) {
        //         //     //break; // Demonstrating a jumping statment to break out of the loop
        //         //     //continue; // Skip this time and go to the next iteration
        //         //     //goto done; // Stops loop and goes-to the label, the line before the return statment for this program.
        //         // }
        //         sum += this.grades[i];
        //     }
        //     // done: 
        //     return sum;


        //     // Using while loop: ----------------------------------------------------------
        //     // var index = 0;
        //     // while(index < this.grades.Count) {
        //     //     sum += this.grades[index];
        //     //     index++;
        //     // }
        //     // return sum;
        // }

        // public double AverageGrade() {
        //     var avg = this.SumOfGrades()/(this.grades.Count);
        //     return avg;
        // }

        // public double HighestGrade() {
        //     double highGrade = double.MinValue;
        //     foreach(double grade in this.grades) {
        //         highGrade = Math.Max(grade, highGrade);
        //     }
        //     return highGrade;
        // }

        // public double LowestGrade() {
        //     double lowGrade = double.MaxValue;
        //     foreach(double grade in this.grades) {
        //         lowGrade = Math.Min(grade, lowGrade);
        //     }
        //     return lowGrade;
        // }

        // public void ShowStatistics() {
        //     Console.WriteLine($"{this.Name}");
        //     Console.WriteLine($"Sum of grades: {this.SumOfGrades()}");
        //     Console.WriteLine($"Average of grades: {this.AverageGrade()}");
        //     Console.WriteLine($"Highest grade: {this.HighestGrade()}");
        //     Console.WriteLine($"Lowest grade: {this.LowestGrade()}");
        // }

        public override Statistics GetStatistics() {
            var bookStats = new Statistics();
            for(var index = 0; index < this.grades.Count; index++) {
                bookStats.Add(this.grades[index]);
            }
            return bookStats;
        }
    }
}