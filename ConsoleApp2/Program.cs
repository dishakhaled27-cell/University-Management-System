using System;
using System.Collections.Generic;
using System.Linq;

// ==========================================
// 1. INTERFACES
// ==========================================

interface IPayable
{
    bool Pay(decimal amount);
}

interface IReportable
{
    void GenerateReport();
}

// ==========================================
// 2. ABSTRACT CLASS: PERSON
// ==========================================

abstract class Person
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public int Age { get; private set; }

    protected Person(int id, string name, int age)
    {
        if (id <= 0)
            throw new ArgumentException("ID must be positive.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        if (age <= 0)
            throw new ArgumentException("Age must be positive.");

        Id = id;
        Name = name;
        Age = age;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Age: {Age}");
    }

    public abstract void PerformRole();
}

// ==========================================
// 3. STUDENT
// ==========================================

class Student : Person, IReportable
{
    private readonly List<Enrollment> enrollments =
        new List<Enrollment>();

    private decimal balance;

    public static int TotalStudents { get; private set; }

    public string Department { get; private set; }

    public decimal Balance => balance;

    public IReadOnlyList<Enrollment> Enrollments => enrollments.AsReadOnly();

    public double GPA
    {
        get
        {
            var graded = enrollments
                .Where(e => e.Grade.HasValue)
                .ToList();

            if (graded.Count == 0)
                return 0;

            return Math.Round(
                graded.Average(e => e.Grade!.Value) / 25.0, 2);
        }
    }

    public Student(
        int id,
        string name,
        int age,
        string department,
        decimal initialBalance = 0)
        : base(id, name, age)
    {
        if (string.IsNullOrWhiteSpace(department))
            throw new ArgumentException("Department is required.");

        if (initialBalance < 0)
            throw new ArgumentException("Balance cannot be negative.");

        Department = department;
        balance = initialBalance;

        TotalStudents++;
    }

    public bool Enroll(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));

        if (enrollments.Any(e => e.Course.Id == course.Id))
        {
            Console.WriteLine("Student is already enrolled.");
            return false;
        }

        if (!course.AddStudent(this))
            return false;

        enrollments.Add(new Enrollment(this, course));

        Console.WriteLine($"{Name} enrolled in {course.Title}.");
        return true;
    }

    public bool Withdraw(int courseId)
    {
        Enrollment enrollment =
            enrollments.FirstOrDefault(e => e.Course.Id == courseId);

        if (enrollment == null)
        {
            Console.WriteLine("Enrollment not found.");
            return false;
        }

        if (enrollment.Grade.HasValue)
        {
            Console.WriteLine("Cannot withdraw after grading.");
            return false;
        }

        enrollment.Course.RemoveStudent(Id);
        enrollments.Remove(enrollment);

        Console.WriteLine("Enrollment removed successfully.");
        return true;
    }

    public void ShowCourses()
    {
        Console.WriteLine($"\nCourses of {Name}:");

        if (enrollments.Count == 0)
        {
            Console.WriteLine("No enrolled courses.");
            return;
        }

        foreach (Enrollment enrollment in enrollments)
            enrollment.DisplayInfo();
    }

    public void AddCharge(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Charge must be positive.");

        balance += amount;
    }

    public bool Pay(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Invalid payment amount.");
            return false;
        }

        if (amount > balance)
        {
            Console.WriteLine("Insufficient balance.");
            return false;
        }

        balance -= amount;

        Console.WriteLine(
            $"Payment successful. Remaining balance: {balance:C}");

        return true;
    }

    public void AssignGrade(int courseId, double grade)
    {
        Enrollment enrollment =
            enrollments.FirstOrDefault(e => e.Course.Id == courseId);

        if (enrollment == null)
            throw new InvalidOperationException(
                "Student is not enrolled in this course.");

        enrollment.AssignGrade(grade);
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();

        Console.WriteLine($"Department: {Department}");
        Console.WriteLine($"GPA: {GPA:F2}");
        Console.WriteLine($"Outstanding balance: {Balance:C}");
    }

    // Method Overloading
    public void DisplayInfo(bool showCourses)
    {
        DisplayInfo();

        if (showCourses)
            ShowCourses();
    }

    public override void PerformRole()
    {
        Console.WriteLine($"{Name} studies at the university.");
    }

    public void GenerateReport()
    {
        Console.WriteLine("\n========== STUDENT REPORT ==========");

        DisplayInfo(true);

        Console.WriteLine("====================================");
    }
}

// ==========================================
// 4. INSTRUCTOR
// ==========================================

class Instructor : Person
{
    private readonly List<Course> courses =
        new List<Course>();

    public string Specialization { get; private set; }

    public decimal Salary { get; private set; }

    public IReadOnlyList<Course> Courses => courses.AsReadOnly();

    public Instructor(
        int id,
        string name,
        int age,
        string specialization,
        decimal salary)
        : base(id, name, age)
    {
        if (string.IsNullOrWhiteSpace(specialization))
            throw new ArgumentException("Specialization is required.");

        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative.");

        Specialization = specialization;
        Salary = salary;
    }

    public void AddCourse(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));

        if (!courses.Any(c => c.Id == course.Id))
            courses.Add(course);
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();

        Console.WriteLine($"Specialization: {Specialization}");
        Console.WriteLine($"Salary: {Salary:C}");
    }

    public override void PerformRole()
    {
        Console.WriteLine($"{Name} teaches university courses.");
    }
}

// ==========================================
// 5. EMPLOYEE
// ==========================================

class Employee : Person
{
    public string JobTitle { get; private set; }

    public decimal Salary { get; private set; }

    public Employee(
        int id,
        string name,
        int age,
        string jobTitle,
        decimal salary)
        : base(id, name, age)
    {
        if (string.IsNullOrWhiteSpace(jobTitle))
            throw new ArgumentException("Job title is required.");

        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative.");

        JobTitle = jobTitle;
        Salary = salary;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();

        Console.WriteLine($"Job: {JobTitle}");
        Console.WriteLine($"Salary: {Salary:C}");
    }

    public override void PerformRole()
    {
        Console.WriteLine($"{Name} manages university operations.");
    }
}

// ==========================================
// 6. COURSE
// ==========================================

class Course : IReportable
{
    private readonly List<Student> students =
        new List<Student>();

    public int Id { get; private set; }

    public string Title { get; private set; }

    public int Credits { get; private set; }

    public int Capacity { get; private set; }

    public Instructor Instructor { get; private set; }

    public IReadOnlyList<Student> Students => students.AsReadOnly();

    public Course(
        int id,
        string title,
        int credits,
        int capacity,
        Instructor instructor)
    {
        if (id <= 0)
            throw new ArgumentException("Course ID must be positive.");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        if (credits <= 0)
            throw new ArgumentException("Credits must be positive.");

        if (capacity <= 0)
            throw new ArgumentException("Capacity must be positive.");

        Id = id;
        Title = title;
        Credits = credits;
        Capacity = capacity;
        Instructor = instructor;

        instructor?.AddCourse(this);
    }

    public bool AddStudent(Student student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));

        if (students.Any(s => s.Id == student.Id))
            return false;

        if (students.Count >= Capacity)
        {
            Console.WriteLine("Course is full.");
            return false;
        }

        students.Add(student);
        return true;
    }

    public void RemoveStudent(int studentId)
    {
        Student student =
            students.FirstOrDefault(s => s.Id == studentId);

        if (student != null)
            students.Remove(student);
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"\nCourse ID: {Id}");
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Credits: {Credits}");
        Console.WriteLine($"Instructor: {Instructor?.Name ?? "Not assigned"}");
        Console.WriteLine($"Students: {students.Count}/{Capacity}");
    }

    public void GenerateReport()
    {
        DisplayInfo();

        Console.WriteLine("Enrolled Students:");

        foreach (Student student in students)
            Console.WriteLine($"{student.Id} - {student.Name}");
    }
}

// ==========================================
// 7. ENROLLMENT
// ==========================================

class Enrollment
{
    public Student Student { get; private set; }

    public Course Course { get; private set; }

    public DateTime EnrollmentDate { get; private set; }

    public double? Grade { get; private set; }

    public bool IsPassed => Grade.HasValue && Grade.Value >= 50;

    public Enrollment(Student student, Course course)
    {
        Student = student ??
            throw new ArgumentNullException(nameof(student));

        Course = course ??
            throw new ArgumentNullException(nameof(course));

        EnrollmentDate = DateTime.Now;
    }

    public void AssignGrade(double grade)
    {
        if (grade < 0 || grade > 100)
            throw new ArgumentOutOfRangeException(
                nameof(grade),
                "Grade must be between 0 and 100.");

        Grade = grade;

        Console.WriteLine(
            $"Grade {grade} assigned to {Student.Name} in {Course.Title}.");
    }

    public void DisplayInfo()
    {
        Console.WriteLine(
            $"Course: {Course.Title} | " +
            $"Date: {EnrollmentDate:d} | " +
            $"Grade: {(Grade.HasValue ? Grade.Value.ToString("F1") : "Not graded")} | " +
            $"Result: {(Grade.HasValue ? (IsPassed ? "Pass" : "Fail") : "Pending")}");
    }
}

// ==========================================
// 8. DEPARTMENT
// ==========================================

class Department
{
    private readonly List<Course> courses =
        new List<Course>();

    private readonly List<Instructor> instructors =
        new List<Instructor>();

    public int Id { get; private set; }

    public string Name { get; private set; }

    public IReadOnlyList<Course> Courses => courses.AsReadOnly();

    public IReadOnlyList<Instructor> Instructors => instructors.AsReadOnly();

    public Department(int id, string name)
    {
        if (id <= 0)
            throw new ArgumentException("Department ID must be positive.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Department name is required.");

        Id = id;
        Name = name;
    }

    public void AddCourse(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));

        if (!courses.Any(c => c.Id == course.Id))
            courses.Add(course);
    }

    public void AddInstructor(Instructor instructor)
    {
        if (instructor == null)
            throw new ArgumentNullException(nameof(instructor));

        if (!instructors.Any(i => i.Id == instructor.Id))
            instructors.Add(instructor);
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"\nDepartment: {Name}");
        Console.WriteLine($"Courses: {courses.Count}");
        Console.WriteLine($"Instructors: {instructors.Count}");

        foreach (Course course in courses)
            course.DisplayInfo();
    }
}

// ==========================================
// 9. UNIVERSITY MANAGEMENT
// ==========================================

class University : IReportable
{
    private readonly List<Student> students =
        new List<Student>();

    private readonly List<Instructor> instructors =
        new List<Instructor>();

    private readonly List<Employee> employees =
        new List<Employee>();

    private readonly List<Course> courses =
        new List<Course>();

    private readonly List<Department> departments =
        new List<Department>();

    public string Name { get; private set; }

    public University(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("University name is required.");

        Name = name;
    }

    public bool AddStudent(Student student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));

        if (students.Any(s => s.Id == student.Id))
        {
            Console.WriteLine("Student ID already exists.");
            return false;
        }

        students.Add(student);
        return true;
    }

    public bool AddInstructor(Instructor instructor)
    {
        if (instructor == null)
            throw new ArgumentNullException(nameof(instructor));

        if (instructors.Any(i => i.Id == instructor.Id))
        {
            Console.WriteLine("Instructor ID already exists.");
            return false;
        }

        instructors.Add(instructor);
        return true;
    }

    public bool AddEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (employees.Any(e => e.Id == employee.Id))
        {
            Console.WriteLine("Employee ID already exists.");
            return false;
        }

        employees.Add(employee);
        return true;
    }

    public bool AddCourse(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));

        if (courses.Any(c => c.Id == course.Id))
        {
            Console.WriteLine("Course ID already exists.");
            return false;
        }

        courses.Add(course);
        return true;
    }

    public bool AddDepartment(Department department)
    {
        if (department == null)
            throw new ArgumentNullException(nameof(department));

        if (departments.Any(d => d.Id == department.Id))
        {
            Console.WriteLine("Department ID already exists.");
            return false;
        }

        departments.Add(department);
        return true;
    }

    public Student FindStudent(int id)
    {
        return students.FirstOrDefault(s => s.Id == id);
    }

    public Course FindCourse(int id)
    {
        return courses.FirstOrDefault(c => c.Id == id);
    }

    public void ShowAllPeople()
    {
        Console.WriteLine("\n========== UNIVERSITY PEOPLE ==========");

        List<Person> people = new List<Person>();

        people.AddRange(students);
        people.AddRange(instructors);
        people.AddRange(employees);

        foreach (Person person in people)
        {
            Console.WriteLine("\n--------------------");
            person.DisplayInfo();
            person.PerformRole();
        }
    }

    public void ShowAllCourses()
    {
        Console.WriteLine("\n========== ALL COURSES ==========");

        foreach (Course course in courses)
            course.DisplayInfo();
    }

    public void ShowAllStudents()
    {
        Console.WriteLine("\n========== ALL STUDENTS ==========");

        foreach (Student student in students)
        {
            student.DisplayInfo();
            Console.WriteLine("--------------------");
        }
    }

    public void GenerateReport()
    {
        Console.WriteLine("\n========== UNIVERSITY REPORT ==========");
        Console.WriteLine($"University: {Name}");
        Console.WriteLine($"Students: {students.Count}");
        Console.WriteLine($"Instructors: {instructors.Count}");
        Console.WriteLine($"Employees: {employees.Count}");
        Console.WriteLine($"Courses: {courses.Count}");
        Console.WriteLine($"Departments: {departments.Count}");
        Console.WriteLine($"Total Students Created: {Student.TotalStudents}");
        Console.WriteLine("=======================================");
    }
}

// ==========================================
// 10. PAYMENT RECORD
// ==========================================

class PaymentRecord
{
    public int StudentId { get; private set; }

    public decimal Amount { get; private set; }

    public DateTime PaymentDate { get; private set; }

    public PaymentRecord(int studentId, decimal amount)
    {
        if (studentId <= 0)
            throw new ArgumentException("Student ID must be positive.");

        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");

        StudentId = studentId;
        Amount = amount;
        PaymentDate = DateTime.Now;
    }

    public void DisplayInfo()
    {
        Console.WriteLine(
            $"Student ID: {StudentId} | Amount: {Amount:C} | Date: {PaymentDate}");
    }
}

// ==========================================
// 11. MAIN PROGRAM
// ==========================================

class Program
{
    static void Main()
    {
        University university =
            new University("Assiut University");

        List<PaymentRecord> paymentRecords =
            new List<PaymentRecord>();

        // Create Department
        Department csDepartment =
            new Department(1, "Computer Science");

        // Create Instructors
        Instructor instructor1 =
            new Instructor(101, "Ahmed", 35, "C# Programming", 15000);

        Instructor instructor2 =
            new Instructor(102, "Mohamed", 40, "Database Systems", 18000);

        university.AddInstructor(instructor1);
        university.AddInstructor(instructor2);

        csDepartment.AddInstructor(instructor1);
        csDepartment.AddInstructor(instructor2);

        // Create Courses
        Course course1 =
            new Course(201, "Object Oriented Programming", 3, 30, instructor1);

        Course course2 =
            new Course(202, "Database Systems", 3, 25, instructor2);

        Course course3 =
            new Course(203, "Data Structures", 3, 20, instructor1);

        university.AddCourse(course1);
        university.AddCourse(course2);
        university.AddCourse(course3);

        csDepartment.AddCourse(course1);
        csDepartment.AddCourse(course2);
        csDepartment.AddCourse(course3);

        university.AddDepartment(csDepartment);

        // Create Students
        Student student1 =
            new Student(1001, "Mostafa", 20, "Computer Science", 5000);

        Student student2 =
            new Student(1002, "Ali", 21, "Computer Science", 3000);

        Student student3 =
            new Student(1003, "Omar", 19, "Computer Science", 2500);

        university.AddStudent(student1);
        university.AddStudent(student2);
        university.AddStudent(student3);

        // Create Employee
        Employee employee =
            new Employee(301, "Mahmoud", 45, "Administrator", 12000);

        university.AddEmployee(employee);

        // Enroll Students
        student1.Enroll(course1);
        student1.Enroll(course2);
        student1.Enroll(course3);

        student2.Enroll(course1);
        student2.Enroll(course2);

        student3.Enroll(course1);

        // Assign Grades
        student1.AssignGrade(201, 90);
        student1.AssignGrade(202, 85);
        student1.AssignGrade(203, 95);

        student2.AssignGrade(201, 75);
        student2.AssignGrade(202, 80);

        student3.AssignGrade(201, 45);

        // Demonstrate Interface and Payments
        student1.AddCharge(1500);
        student2.AddCharge(1000);

        if (student1.Pay(500))
        {
            paymentRecords.Add(new PaymentRecord(student1.Id, 500));
        }

        if (student2.Pay(300))
        {
            paymentRecords.Add(new PaymentRecord(student2.Id, 300));
        }

        // Main Menu
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n==================================");
            Console.WriteLine($"  {university.Name}");
            Console.WriteLine("  UNIVERSITY MANAGEMENT SYSTEM");
            Console.WriteLine("==================================");
            Console.WriteLine("1. Show All People");
            Console.WriteLine("2. Show All Students");
            Console.WriteLine("3. Show All Courses");
            Console.WriteLine("4. Search Student");
            Console.WriteLine("5. Enroll Student in Course");
            Console.WriteLine("6. Withdraw Student from Course");
            Console.WriteLine("7. Assign Student Grade");
            Console.WriteLine("8. Pay Student Fees");
            Console.WriteLine("9. Show Student Courses");
            Console.WriteLine("10. Show Department");
            Console.WriteLine("11. Show Payment Records");
            Console.WriteLine("12. Generate University Report");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        university.ShowAllPeople();
                        break;

                    case "2":
                        university.ShowAllStudents();
                        break;

                    case "3":
                        university.ShowAllCourses();
                        break;

                    case "4":
                        Console.Write("Enter Student ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int searchId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        Student foundStudent =
                            university.FindStudent(searchId);

                        if (foundStudent == null)
                            Console.WriteLine("Student not found.");
                        else
                            foundStudent.GenerateReport();

                        break;

                    case "5":
                        Console.Write("Enter Student ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int enrollStudentId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        Console.Write("Enter Course ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int enrollCourseId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        Student enrollingStudent =
                            university.FindStudent(enrollStudentId);

                        Course enrollingCourse =
                            university.FindCourse(enrollCourseId);

                        if (enrollingStudent == null || enrollingCourse == null)
                        {
                            Console.WriteLine("Student or course not found.");
                            break;
                        }

                        enrollingStudent.Enroll(enrollingCourse);
                        break;

                    case "6":
                        Console.Write("Enter Student ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int withdrawStudentId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        Console.Write("Enter Course ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int withdrawCourseId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        Student withdrawingStudent =
                            university.FindStudent(withdrawStudentId);

                        if (withdrawingStudent == null)
                        {
                            Console.WriteLine("Student not found.");
                            break;
                        }

                        withdrawingStudent.Withdraw(withdrawCourseId);
                        break;

                    case "7":
                        Console.Write("Enter Student ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int gradeStudentId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        Console.Write("Enter Course ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int gradeCourseId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        Console.Write("Enter Grade (0-100): ");

                        if (!double.TryParse(Console.ReadLine(), out double grade))
                        {
                            Console.WriteLine("Invalid grade.");
                            break;
                        }

                        Student gradingStudent =
                            university.FindStudent(gradeStudentId);

                        if (gradingStudent == null)
                        {
                            Console.WriteLine("Student not found.");
                            break;
                        }

                        gradingStudent.AssignGrade(gradeCourseId, grade);
                        break;

                    case "8":
                        Console.Write("Enter Student ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int payStudentId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        Console.Write("Enter Payment Amount: ");

                        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                        {
                            Console.WriteLine("Invalid amount.");
                            break;
                        }

                        Student payingStudent =
                            university.FindStudent(payStudentId);

                        if (payingStudent == null)
                        {
                            Console.WriteLine("Student not found.");
                            break;
                        }

                        if (payingStudent.Pay(amount))
                        {
                            paymentRecords.Add(
                                new PaymentRecord(payingStudent.Id, amount));
                        }

                        break;

                    case "9":
                        Console.Write("Enter Student ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int coursesStudentId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        Student coursesStudent =
                            university.FindStudent(coursesStudentId);

                        if (coursesStudent == null)
                            Console.WriteLine("Student not found.");
                        else
                            coursesStudent.ShowCourses();

                        break;

                    case "10":
                        csDepartment.DisplayInfo();
                        break;

                    case "11":
                        Console.WriteLine("\n========== PAYMENT RECORDS ==========");

                        if (paymentRecords.Count == 0)
                        {
                            Console.WriteLine("No payments recorded.");
                        }
                        else
                        {
                            foreach (PaymentRecord record in paymentRecords)
                                record.DisplayInfo();
                        }

                        break;

                    case "12":
                        university.GenerateReport();
                        break;

                    case "0":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (running)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
