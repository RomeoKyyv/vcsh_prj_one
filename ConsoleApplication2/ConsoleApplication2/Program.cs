

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApplication2
{
    [Serializable]
    abstract class Employee
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public abstract decimal GetSalary { get; set; }
        protected Employee(string name, int age)
        {
            Name = name;
            Age = age;
        }

        protected abstract decimal Salary();
        public override string ToString()
        {
            return Name + " " + Age + " " + Salary();
        }
    }
    [Serializable]
    sealed class BadEmployee : Employee
    {
        public double Stavka { get; private set; }

        public BadEmployee(string name, int age, double stavka)
            : base(name, age)
        {
            Stavka = stavka;
            GetSalary = Salary();
        }

        public override decimal GetSalary { get; set; }

        protected override decimal Salary()
        {
            return (decimal)(20.8 * 8 * Stavka);
        }
    }
    [Serializable]
    sealed class GoodEmployee : Employee
    {
        public decimal Oklad { get; private set; }
        public GoodEmployee(string name, int age, decimal oklad)
            : base(name, age)
        {
            Oklad = oklad;
            GetSalary = Salary();
        }

        public override decimal GetSalary { get; set; }

        protected override decimal Salary()
        {
            return Oklad;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            List<Employee> emp = new List<Employee>()
            {
                new GoodEmployee("Dmitrenko",22,1000),
                new GoodEmployee("Pipipenko",25,1500),
                new GoodEmployee("Savchenko",14,800),
                new BadEmployee("Filipchuk",20,15),
                new BadEmployee("Konenko",20,10),
                new BadEmployee("Uzun",25,11)
            };
            var punktA = emp.OrderBy(a => a.GetSalary);

            foreach (var e in punktA)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine(new string('*', 20));

            var punktB = punktA.Take(5);
            foreach (var e in punktB)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine(new string('*', 20));

            foreach (var q in punktA.Take(3))
            {
                Console.WriteLine(q.Name);
            }

            var binary = new BinaryFormatter();

            using (var fs = new FileStream("data.dat", FileMode.Create))
            {
                binary.Serialize(fs, emp);
                Console.WriteLine("Даные записаны в data.dat");
            }

            try
            {
                var res = binary.Deserialize(File.Open("data.dat", FileMode.Open)) as List<Employee>;
                foreach (var a in res)
                {
                    Console.WriteLine(a);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            Console.ReadKey();
        }
    }
}

