using System;
class Person
{
    public string Name;
    public int Age;
    public Person(string name , int age)
    {
         this.Name = name;
        this.Age = age;
    }
    public void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }
}

class Student : Person
{
   
    public int StudentID;
    public Student(string name , int age ,int StudentID) : base(name , age)
    {
            this .StudentID = StudentID;
    }

    public void Study()
    {
        Console.WriteLine($"{Name} is studying");
    }
}

class Program
{
    static void Main()
    {
        Student s1 = new Student("Mostafa" , 20 , 1620776443);

        Console.WriteLine(s1.Name);
        Console.WriteLine(s1.Age);
        Console.WriteLine(s1.StudentID);

        Console.WriteLine();

        s1.Eat();
        s1.Study();
    }
}