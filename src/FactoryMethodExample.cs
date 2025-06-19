using System;

// Интерфейс для продукта
public interface ITransport
{
    void Deliver();
}

// Конкретные продукты
public class Truck : ITransport
{
    public void Deliver()
    {
        Console.WriteLine("Delivering by land in a truck.");
    }
}

public class Ship : ITransport
{
    public void Deliver()
    {
        Console.WriteLine("Delivering by sea in a ship.");
    }
}

// Абстрактный создатель (Creator)
public abstract class Logistics
{
    // Фабричный метод
    public abstract ITransport CreateTransport();

    public void PlanDelivery()
    {
        ITransport transport = CreateTransport();
        transport.Deliver();
    }
}

// Конкретные создатели (Concrete Creators)
public class RoadLogistics : Logistics
{
    public override ITransport CreateTransport()
    {
        return new Truck();
    }
}

public class SeaLogistics : Logistics
{
    public override ITransport CreateTransport()
    {
        return new Ship();
    }
}

public class FactoryMethodExample
{
    public static void Run()
    {
        Console.WriteLine("--- Running Factory Method Example ---");
        
        Logistics roadLogistics = new RoadLogistics();
        roadLogistics.PlanDelivery();

        Logistics seaLogistics = new SeaLogistics();
        seaLogistics.PlanDelivery();
        
        Console.WriteLine("-----------------------------------");
    }
} 