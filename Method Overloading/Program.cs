using System;
class Calculator
{
    public int sum (int a , int b)
    {
        return a + b;
    }
    public double sum (double a , double b)
    {
        return a + b;
    }
    public int sum (int a , int b , int c)
    {
        return a + b + c;
    }
}
class Program
{
    static void Main()
    {
        Calculator calculator = new Calculator();
       Console.WriteLine( calculator.sum(10, 20));
       Console.WriteLine( calculator.sum(20, 30 ,10));
       Console.WriteLine( calculator.sum(2.5, 3));
    }
}