using System;
using System.Collections.Generic;
using System.Xml.Linq;
abstract class person
{
    public string name { get; set; }
    public int age { get; set; }
    public int id { get; set; }
    protected person(string name , int age , int id )
    {
       this.name = name;  
        this.age = age;
        this.id = id;
    }
    public abstract void DisplayInfo ();
    public virtual void Work()
    {
        Console.WriteLine($"{name} is Working");
    }
    public void introducte() { 
        Console.WriteLine($"Hello , My name is {name} , and my id {id} ");
    }
}
class Student : person
{
    public string Major {  get; set; }
    public double Gpa { get; set; }
    public Student(string name , int age , int id , string Major , double Gpa) : base (name , age , id )
    {
         this.Major = Major;
         this.Gpa = Gpa;
        
    }
    public override void DisplayInfo()
    {
        Console.WriteLine("---- Informtion Student ---- ");
        Console.WriteLine($"Name : {name}");
        Console.WriteLine($"Age  : {age}");
        Console.WriteLine($"ID   : {id}");
        Console.WriteLine($"Major : {Major}");
        Console.WriteLine($"GPA {Gpa}");
    }
    public override void Work()
    {
        Console.WriteLine($"{name} is studying  ");
    }
    public void Study()
    {
        Console.WriteLine($"{name} is studying {Major} ");
    }
}
class Teacher : person
{
    public string subject { get; set; }
    public double Salary { get; set; }
    public Teacher(string name, int age, int id, string subject, double Salary) : base(name, age, id)
    {
        this.subject = subject;
        this.Salary = Salary;
    }
    public override void DisplayInfo()
    {
        Console.WriteLine("____ Informtion Teacher ____");
        Console.WriteLine($"Name : {name}");
        Console.WriteLine($"Age : {age}");
        Console.WriteLine($"iD :  {id}");
        Console.WriteLine($"Subject : {subject}");
        Console.WriteLine($"Salary : {Salary}");
    }
    public override void Work()
    {
        Console.WriteLine($"{name} is teaching {subject}");
    }
    public void teach()
    {
        Console.WriteLine($"{name} is explaning {subject}");
    }
}
class Employee : person
{
    public string Department { get; set; }
    public double salary { get; set; }
    public Employee(string name, int age, int id, string Department, double salary) : base(name, age, id)
    {
        this.salary = salary;
        this.Department = Department;
    }
    public override void DisplayInfo()
    {
        Console.WriteLine($"____ Informtion Employee ____");
        Console.WriteLine($"Name :{name}");
        Console.WriteLine($"Age : {age}");
        Console.WriteLine($"ID : {id}");
        Console.WriteLine($"Department : {Department}");
        Console.WriteLine($"Salary :  {salary}");
    }
    public override void Work()
    {

        Console.WriteLine($"{name} is working in {Department}.");
    }
}

class University
{
    public string UniversityName { get; set; }
    private List<person> people = new List<person>(); 
    public University(string universityName) 
    { 
        UniversityName = universityName; 
    }
    public void AddPerson(person person) { 
        people.Add(person); 
        Console.WriteLine($"{person.name} added to {UniversityName}."); }
    public void ShowAllPeople() 
    {
        Console.WriteLine($"\n========== {UniversityName} =========="); 
        foreach (person person in people)
        { 
            person.DisplayInfo(); person.Work();
        } 
    }
}
class Program
{
    static void Main (string[] args)
    {

        Console.WriteLine("UNIVERSITY MANAGEMENT SYSTEM");
        Student student1 = new Student("Mostafa", 20, 101, "Computer Science", 3.5);

        Teacher teacher1 = new Teacher("Ahmed", 40, 201, "C# Programming", 15000);

        Employee employee1 = new Employee("Mohamed", 35, 301, "Administration", 8000);

        University university = new University("Assiut University");

        university.AddPerson(student1); 
        university.AddPerson(teacher1); 
        university.AddPerson(employee1);

        Console.WriteLine("\n========== Introductions ==========");
        student1.introducte();
        teacher1.introducte(); 
        employee1.introducte();

        university.ShowAllPeople();

        Console.WriteLine("\n========== Activities =========="); 
        student1.Study(); 
        teacher1.teach();
        Console.WriteLine("\nProgram Finished.");

    }






}