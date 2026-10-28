using System;

abstract public class Vehicle
{
    public string Brand;
    abstract public void Move();

    public void Stop()
    {
        Console.WriteLine("Vehicle Is Stopped");
    }
}
class Car : Vehicle
{
    public override void Move()
    {
        Console.WriteLine("Car Moves on The road ");

    }
}
class Airplane : Vehicle
{
    public override void Move()
    {
        Console.WriteLine("Airplane flies in the sky");
    }
    class Program
    {
        static void Main()
        {
          Car car = new Car();
            car.Brand = "BMW";
            Console.WriteLine(car.Brand);
            car.Move();
            car.Stop();
            Console.WriteLine();
            Airplane airplane = new Airplane();
            airplane.Brand = "Jampo";
            Console.WriteLine(airplane.Brand);
            airplane.Move();
            airplane.Stop();
        }

    }
}
