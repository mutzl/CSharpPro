using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CSharp_Pro
{
	class Program
	{
		static void Main(string[] args)
		{
			ImplicitlyTypedLocalVariables();
			Console.WriteLine();

			Initializers();
			Console.WriteLine();

			ExtensionMethods();
			Console.WriteLine();

			AnonymousTypes();
			Console.WriteLine();

			LambdaExpressions();
			Console.WriteLine();

			Linq();
			Console.WriteLine();


			Console.ReadLine();
		}


		static void ImplicitlyTypedLocalVariables()
		{

			//int i = 5;
			//string x = "Hello World";

			var i = 5;
			var x = "Hello World";

			if (x.StartsWith("Hello")) Console.WriteLine("Nice");

			var balance = 123.45m;  // decimal


			var student1 = new Student();
			student1.First = "Thomas";
			student1.Last = "Mutzl";

			var student2 = new Student();
			student2.First = "Chuck";
			student2.Last = "Norris";

			var students = new List<Student>();
			students.Add(student1);
			students.Add(student2);

			foreach (var student in students)
			{
				Console.WriteLine(student);
			}


		}

		static void Initializers()
		{
			// Object initializer
			var student1 = new Student
			{
				First = "Thomas",
				Last = "Mutzl",
				ID = 99,
			};

			var student2 = new Student("Chuck", "Norris", 0);

			var student3 = new Student(1)
			{
				First = "James",
				Last = "Bond",
			};

			// Collection initializer
			var students = new List<Student>
							   {
								   student1,
								   student2,
								   student3,
								   new Student
									   {
										   First = "Super",
										   Last = "Man",
										   Scores = new List<int> { 24, 24, 24, }
									   },
							   };
		}

		static void ExtensionMethods()
		{
			var name = "Thomas";

			var spacedName = StringHelper.ToSpaceStyle(name);
			Console.WriteLine(spacedName);

			Console.WriteLine(name.ToSpaceStyle());

			Console.WriteLine("Chuck Norris".ToSpaceStyle(3));
			Console.WriteLine(StringHelper.ToSpaceStyle("James Bond", 2));

			var student = new Student("Thomas", "Mutzl", 99);

			student.AddTestGrade(23);
		}

		static void AnonymousTypes()
		{
			var car1 = new
			{
				Brand = "Audi",
				Power = 115,
				Price = 43000m,
			};

			var car2 = new
			{
				Brand = "Ford",
				Power = 80,
				Price = 22222m,
			};

			car2 = car1;

			Console.WriteLine(car2.ToString());
		}


		private delegate int CalcOperation(int a, int b);
		private static int Add(int a, int b)
		{
			return a + b;
		}
		static void LambdaExpressions()
		{

			int result0 = Add(1, 2);
			Console.WriteLine(result0);

			// delegate
			CalcOperation calc1 = Add;
			int result1 = calc1(3, 4);
			Console.WriteLine(result1);

			// anonymous method C# 2.0 
			CalcOperation calc2 = delegate (int a, int b)
				{
					return a + b;
				};
			int result2 = calc2(5, 6);
			Console.WriteLine(result2);


			Func<int, int, int> calc3 = delegate (int a, int b)
				{
					return a + b;
				};
			int result3 = calc3(7, 8);
			Console.WriteLine(result3);



			// lambda expressions C# 3.0
			// with statement body
			Func<int, int, int> calc4 = (a, b) =>
				{
					return a + b;
				};
			int result4 = calc4(9, 10);
			Console.WriteLine(result4);

			// lambda expressions C# 3.0
			// with expression body
			Func<int, int, int> calc5 = (a, b) => a + b;
			int result5 = calc5(11, 12);
			Console.WriteLine(result5);

			Func<int> x1 = () => 42;
			Func<int, bool> x2 = a => a >= 0;

			Console.WriteLine(x1().ToString());
			Console.WriteLine(x2(1).ToString());
			Console.WriteLine(x2(-1).ToString());


			var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

			var filterdList = list.Filter(x => x > 7);
			filterdList.Dump();

			list.Filter(y => y <= 5).Dump();

			list.Filter(a => a < 6).Filter(a => a >= 2).Dump();
			list.Filter(a => a < 6 && a >= 2).Dump();



		}


		static void Linq()
		{
			var students = StudentHelper.GetStudents();

			var query = students.Where(student => student.Last == "Garcia");
			query.Dump();


			students.Where(s => s.Scores.Average() > 85)
					.OrderBy(s => s.Last)
					.ThenBy(s => s.First)
					.Dump();

			var s1 = students.SingleOrDefault(s => s.ID == 111);
			Console.WriteLine(s1);

			students.OrderByDescending(s => s.Scores.Average())
					.Skip(5)
					.Take(5)
					.Select(s => new { Firstname = s.First, Surname = s.Last, s.ID })
					.Dump();

			var query2 = from s in students
						 where s.Last.Contains("l")
						 orderby s.Last, s.First
						 select new
						 {
							 s.Last,
							 s.First
						 };
			query2.Dump();

		}
	}


	class Student
	{

		public Student()
		{
		}

		public Student(string first, string last, int id)
		{
			First = first;
			Last = last;
			ID = id;
		}

		public Student(int id)
		{
			ID = id;
		}

		public string First { get; set; }
		public string Last { get; set; }


		public int ID { get; set; }

		public List<int> Scores { get; set; } = new List<int>();

		public override string ToString()
		{
			var score = Scores.Count == 0 ? "-" : Scores.Average().ToString();
			return $"{Last}, {First} avg. Score {score}";
		}
	}

	static class StringHelper
	{

		public static string ToSpaceStyle(this string value)
		{
			return ToSpaceStyle(value, 1);
		}

		public static string ToSpaceStyle(this string value, int numberOfSpaces)
		{
			var output = new StringBuilder();
			foreach (var character in value)
			{
				output.Append(character);
				for (int i = 0; i < numberOfSpaces; i++)
				{
					output.Append(" ");
				}
			}

			return output.ToString().Substring(0, output.ToString().Length - 1);
		}
	}

	static class StudentHelper
	{
		public static void AddTestGrade(this Student student, int grade)
		{
			student.Scores.Add(grade);
		}

		public static List<Student> GetStudents()
		{
			return new List<Student>()
					   {
							new Student {First="Svetlana", Last="Omelchenko", ID=111, Scores= new List<int> {97, 92, 81, 60}},
							new Student {First="Claire", Last="O’Donnell", ID=112, Scores= new List<int> {75, 84, 91, 39}},
							new Student {First="Sven", Last="Mortensen", ID=113, Scores= new List<int>{88, 94, 65, 91}},
							new Student {First="Cesar", Last="Garcia", ID=114, Scores= new List<int>{97, 89, 85, 82}},
							new Student {First="Debra", Last="Garcia", ID=115, Scores= new List<int>{35, 72, 91, 70}},
							new Student {First="Fadi", Last="Fakhouri", ID=116, Scores= new List<int>{99, 86, 90, 94}},
							new Student {First="Hanying", Last="Feng", ID=117, Scores= new List<int>{93, 92, 80, 87}},
							new Student {First="Hugo", Last="Garcia", ID=118, Scores= new List<int> {92, 90, 83, 78}},
							new Student {First="Lance", Last="Tucker", ID=119, Scores= new List<int>{68, 79, 88, 92}},
							new Student {First="Terry", Last="Adams", ID=120, Scores= new List<int> {99, 82, 81, 79}},
							new Student {First="Eugene", Last="Zabokritski", ID=121, Scores= new List<int> {96, 85, 91, 60}},
							new Student {First="Michael", Last="Tucker", ID=122, Scores= new List<int>{94, 92, 91, 91}},

					   };
		}
	}

	static class ListHelper
	{
		public static List<int> Filter(this List<int> list, Func<int, bool> predicate)
		{
			var resultList = new List<int>();
			foreach (var item in list)
			{
				if (predicate(item))
				{
					resultList.Add(item);
				}
			}

			return resultList;
		}


		public static void Dump(this List<int> list)
		{
			foreach (var i in list)
			{
				Console.Write(i);
				Console.Write(" - ");
			}
			Console.WriteLine();
		}

		public static void Dump(this IEnumerable list)
		{
			foreach (var item in list)
			{
				Console.WriteLine(item);
			}
			Console.WriteLine();
		}
	}

}
