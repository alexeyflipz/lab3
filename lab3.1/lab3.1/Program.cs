using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3._1
{
    public interface IReproducible
    {
        LivingOrganism Reproduce();
    }

    public interface IPredator
    {
        void Hunt(LivingOrganism prey);
    }

    public abstract class LivingOrganism
    {
        public int Energy { get; protected set; }
        public int Age { get; protected set; }
        public int Size { get; protected set; }

        public LivingOrganism(int energy, int size)
        {
            Energy = energy;
            Age = 0;
            Size = size;
        }

        public void Grow()
        {
            Age++;
            Size++;
            Energy -= 2;
        }

        public void ConsumeEnergy(int amount)
        {
            Energy -= amount;
            if (Energy <= 0)
            {
            }
        }
    }

    public class Animal : LivingOrganism, IReproducible, IPredator
    {
        public int Speed { get; private set; }
        public bool IsPredator { get; private set; }

        public Animal(int energy, int size, int speed, bool isPredator)
            : base(energy, size)
        {
            Speed = speed;
            IsPredator = isPredator;
        }

        public LivingOrganism Reproduce()
        {
            return new Animal(Energy / 2, Size / 2, Speed, IsPredator);
        }

        public void Hunt(LivingOrganism prey)
        {
            if (prey.Size < this.Size && this.Energy > 10)
            {
                prey.ConsumeEnergy(prey.Energy);
                this.Energy += 10;
            }
        }
    }

    public class Plant : LivingOrganism, IReproducible
    {
        public int PhotosynthesisRate { get; private set; }

        public Plant(int energy, int size, int photosynthesisRate)
            : base(energy, size)
        {
            PhotosynthesisRate = photosynthesisRate;
        }

        public LivingOrganism Reproduce()
        {
            return new Plant(Energy / 2, Size / 2, PhotosynthesisRate);
        }

        public void Photosynthesize()
        {
            Energy += PhotosynthesisRate;
        }
    }

    public class Microorganism : LivingOrganism, IReproducible
    {
        public int DivisionRate { get; private set; }

        public Microorganism(int energy, int size, int divisionRate)
            : base(energy, size)
        {
            DivisionRate = divisionRate;
        }

        public LivingOrganism Reproduce()
        {
            return new Microorganism(Energy / 2, Size / 2, DivisionRate);
        }

        public void Divide()
        {
            Energy += DivisionRate;
        }
    }

    public class Ecosystem
    {
        private List<LivingOrganism> organisms;

        public Ecosystem()
        {
            organisms = new List<LivingOrganism>();
        }

        public void AddOrganism(LivingOrganism organism)
        {
            organisms.Add(organism);
        }

        public void Simulate()
        {
            foreach (var organism in organisms)
            {
                organism.Grow();

                if (organism is Plant plant)
                {
                    plant.Photosynthesize();
                }
                else if (organism is Animal animal && animal.IsPredator)
                {
                    var prey = organisms.Find(o => !(o is Animal) && o.Size < animal.Size);
                    if (prey != null)
                    {
                        animal.Hunt(prey);
                    }
                }
            }

            var newOrganisms = new List<LivingOrganism>();
            foreach (var organism in organisms)
            {
                if (organism.Energy > 50 && organism is IReproducible reproducible)
                {
                    newOrganisms.Add(reproducible.Reproduce());
                }
            }

            organisms.AddRange(newOrganisms);
        }

        public void DisplayEcosystem()
        {
            foreach (var organism in organisms)
            {
                Console.WriteLine($"{organism.GetType().Name} | Size: {organism.Size}, Energy: {organism.Energy}, Age: {organism.Age}");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Ecosystem ecosystem = new Ecosystem();

            ecosystem.AddOrganism(new Animal(100, 10, 5, true));
            ecosystem.AddOrganism(new Plant(50, 5, 3));
            ecosystem.AddOrganism(new Microorganism(30, 1, 2));

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Cycle {i + 1}:");
                ecosystem.Simulate();
                ecosystem.DisplayEcosystem();
                Console.WriteLine();
            }
        }
    }
}
