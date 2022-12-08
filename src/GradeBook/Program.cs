using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program {
        static void Main(String[] args)
        {
            Book book = new InMemoryBook("David's Grade Book");
            book.GradeAdded += OnGradeAdded;

            EnterGrades(book);

            var stats = book.GetStatistics();
            //Console.WriteLine($"Book Category: {InMemoryBook.CATEGORY}");
            Console.WriteLine($"GradeBook Name: {stats.Name}");
            Console.WriteLine($"The Average Grade is: {stats.Average:N1}");
            Console.WriteLine($"The Highest Grade is: {stats.High}");
            Console.WriteLine($"The Lowest Grade is: {stats.Low}");
            Console.WriteLine($"The Letter Grade is: {stats.Letter}");
        }

        private static void EnterGrades(IBook book)
        {
            while (true)
            {
                Console.WriteLine("Please enter a grade or 'q' to quit.");
                var input = Console.ReadLine();
                if (input == "q")
                {
                    // Or can do: ---------------------
                    // done = true;
                    // continue;
                    break;
                }
                try
                {
                    var grade = double.Parse(input);
                    book.AddGrade(grade);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    //Console.WriteLine("**"); // Always executes no matter the chaos above
                }
            }
        }

        static void OnGradeAdded(object sender, EventArgs e) {
            Console.WriteLine("A grade was added.");
        }
    }
}