using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3._3
{
    public interface IDriveable
    {
        void Move();
        void Stop();
    }

    public class Road
    {
        public double Length { get; set; } 
        public double Width { get; set; }  
        public int Lanes { get; set; }     
        public double TrafficLevel { get; set; } 

        public Road(double length, double width, int lanes, double trafficLevel)
        {
            Length = length;
            Width = width;
            Lanes = lanes;
            TrafficLevel = trafficLevel;
        }
    }

    public abstract class Vehicle : IDriveable
    {
        public double Speed { get; set; } 
        public double Size { get; set; } 
        public string Type { get; set; }  

        protected Vehicle(double speed, double size, string type)
        {
            Speed = speed;
            Size = size;
            Type = type;
        }

        public virtual void Move()
        {
            Console.WriteLine($"{Type} рухається зі швидкістю {Speed} км/год.");
        }

        public virtual void Stop()
        {
            Console.WriteLine($"{Type} зупинився.");
        }
    }

    public class Car : Vehicle
    {
        public Car(double speed, double size) : base(speed, size, "Автомобіль") { }
    }

    public class Truck : Vehicle
    {
        public Truck(double speed, double size) : base(speed, size, "Вантажівка") { }
    }

    public class Bus : Vehicle
    {
        public Bus(double speed, double size) : base(speed, size, "Автобус") { }
    }

    public class TrafficSimulation
    {
        private List<Vehicle> vehicles;
        private Road road;

        public TrafficSimulation(Road road)
        {
            this.road = road;
            vehicles = new List<Vehicle>();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
            Console.WriteLine($"Додано {vehicle.Type} до симуляції.");
        }

        public void SimulateMovement()
        {
            Console.WriteLine($"Симуляція руху на дорозі з {road.Lanes} смугами і рівнем трафіку {road.TrafficLevel}.");

            foreach (var vehicle in vehicles)
            {
                if (road.TrafficLevel > 0.7)
                {
                    vehicle.Stop();
                }
                else
                {
                    vehicle.Move();
                }
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
      
            Road road = new Road(5000, 20, 3, 0.5);

           
            Car car = new Car(80, 4);
            Truck truck = new Truck(60, 12);
            Bus bus = new Bus(50, 10);

        
            TrafficSimulation simulation = new TrafficSimulation(road);
            simulation.AddVehicle(car);
            simulation.AddVehicle(truck);
            simulation.AddVehicle(bus);

            simulation.SimulateMovement();
        }
    }

}
