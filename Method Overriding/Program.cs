class Anmial
{
    public virtual void MakeSound()
    {
        Console.WriteLine("Animal makes a sound");
    }
} 
class Dog : Anmial
{
    public override void MakeSound()
    {
        Console.WriteLine("Dog says Woof");
    }
}
class Cat : Anmial
{
    public override void MakeSound()
    {
        Console.WriteLine("Cat says Meow");
    }
}
class Program
{
    static void Main()
    { 
        Anmial anmial1 = new Dog ();
        Anmial anmial2 = new Cat();
        Anmial anmial3 = new Anmial();
        anmial1 .MakeSound ();
        anmial2 .MakeSound ();
        anmial3 .MakeSound ();
    }
}