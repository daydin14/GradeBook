using System;

namespace GradeBook {
    public class Statistics {
        public string? Name;
        public int Count;
        public double Sum;
        public double Average {
            get {
                return Sum/Count;
            }
        }
        public double High;
        public double Low;
        public char Letter {
            get {
                switch(Average) {
                    case var d when d >= 90.5:
                        return 'A';
                    case var d when d >= 80.8:
                        return 'B';
                    case var d when d >= 70:
                        return 'C';
                    case var d when d >= 60:
                        return 'D';
                    default:
                        return 'F';
                }
            }
        }

        public void Add(double number) {
            Sum += number;
            Count++;

            Low = Math.Min(number, Low);
            High = Math.Max(number, High);
        }

        public Statistics() {
            Count = 0;
            Sum = 0.0;
            High = double.MinValue;
            Low = double.MaxValue;
        }
    }
}