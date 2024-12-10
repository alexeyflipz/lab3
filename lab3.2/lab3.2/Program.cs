using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3._2
{
    public interface IConnectable
    {
        void Connect(IConnectable device);
        void Disconnect(IConnectable device);
        void SendData(IConnectable device, string data);
        void ReceiveData(string data);
    }

    public abstract class Computer : IConnectable
    {
        public string IPAddress { get; set; }
        public int Power { get; set; } 
        public string OperatingSystem { get; set; }

        public List<IConnectable> ConnectedDevices { get; private set; } = new List<IConnectable>();

        public Computer(string ipAddress, int power, string operatingSystem)
        {
            IPAddress = ipAddress;
            Power = power;
            OperatingSystem = operatingSystem;
        }

        public virtual void Connect(IConnectable device)
        {
            if (!ConnectedDevices.Contains(device))
            {
                ConnectedDevices.Add(device);
                Console.WriteLine($"{IPAddress} connected to {((Computer)device).IPAddress}.");
            }
        }

        public virtual void Disconnect(IConnectable device)
        {
            if (ConnectedDevices.Contains(device))
            {
                ConnectedDevices.Remove(device);
                Console.WriteLine($"{IPAddress} disconnected from {((Computer)device).IPAddress}.");
            }
        }

        public virtual void SendData(IConnectable device, string data)
        {
            if (ConnectedDevices.Contains(device))
            {
                Console.WriteLine($"{IPAddress} sends data to {((Computer)device).IPAddress}: {data}");
                device.ReceiveData(data);
            }
            else
            {
                Console.WriteLine($"{IPAddress} cannot send data to {((Computer)device).IPAddress} as they are not connected.");
            }
        }

        public virtual void ReceiveData(string data)
        {
            Console.WriteLine($"{IPAddress} received data: {data}");
        }
    }

    public class Server : Computer
    {
        public int StorageCapacity { get; set; } 

        public Server(string ipAddress, int power, string operatingSystem, int storageCapacity)
            : base(ipAddress, power, operatingSystem)
        {
            StorageCapacity = storageCapacity;
        }
    }

    public class Workstation : Computer
    {
        public string User { get; set; }

        public Workstation(string ipAddress, int power, string operatingSystem, string user)
            : base(ipAddress, power, operatingSystem)
        {
            User = user;
        }
    }

    public class Router : Computer
    {
        public int MaxConnections { get; set; }

        public Router(string ipAddress, int power, string operatingSystem, int maxConnections)
            : base(ipAddress, power, operatingSystem)
        {
            MaxConnections = maxConnections;
        }

        public override void Connect(IConnectable device)
        {
            if (ConnectedDevices.Count < MaxConnections)
            {
                base.Connect(device);
            }
            else
            {
                Console.WriteLine($"{IPAddress} cannot connect more devices. Max connections reached.");
            }
        }
    }

    public class Network
    {
        private List<Computer> devices = new List<Computer>();

        public void AddDevice(Computer device)
        {
            devices.Add(device);
            Console.WriteLine($"Device with IP {device.IPAddress} added to the network.");
        }

        public void RemoveDevice(Computer device)
        {
            devices.Remove(device);
            Console.WriteLine($"Device with IP {device.IPAddress} removed from the network.");
        }

        public void SimulateDataTransfer(Computer sender, Computer receiver, string data)
        {
            sender.SendData(receiver, data);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var server = new Server("192.168.1.1", 500, "Linux", 2000);
            var workstation = new Workstation("192.168.1.2", 300, "Windows", "John Doe");
            var router = new Router("192.168.1.254", 100, "FirmwareOS", 5);

            var network = new Network();
            network.AddDevice(server);
            network.AddDevice(workstation);
            network.AddDevice(router);

            router.Connect(server);
            router.Connect(workstation);

            network.SimulateDataTransfer(server, workstation, "Hello from server!");
            network.SimulateDataTransfer(workstation, server, "Acknowledged.");

            router.Disconnect(server);
            network.SimulateDataTransfer(server, workstation, "Are you still there?");
        }
    }

}
