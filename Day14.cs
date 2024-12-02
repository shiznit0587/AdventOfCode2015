using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day14
    {
        //const int SECONDS = 1000;
        const int SECONDS = 2503;

        public Day14()
        {
            Console.WriteLine("Running Day 14 - a");

            List<Reindeer> reindeers = new List<Reindeer>();

            FileUtil.Parse(14, delegate(string line)
            {
                string[] parts = line.Split(' ');
                reindeers.Add(new Reindeer(parts[0], Int32.Parse(parts[3]), Int32.Parse(parts[6]), Int32.Parse(parts[13])));
            });

            int winningDistance = reindeers.Select(r => r.GetDistance(SECONDS)).Max();

            Console.WriteLine("Winner - " + winningDistance + " km");

            Console.WriteLine("Running Day 14 - b");

            List<ReindeerState> states = reindeers.Select(r => new ReindeerState(r)).ToList();

            for (int i = 0; i < SECONDS; ++i)
            {
                states.ForEach(s => s.Tick());
                int maxDistance = states.Select(s => s.Distance).Max();
                states.FindAll(s => s.Distance == maxDistance).ForEach(s => s.AwardPoint());
            }

            int maxPoints = states.Select(s => s.Points).Max();

            Console.WriteLine("Winner - " + maxPoints + " points");
        }
    }

    class Reindeer
    {
        public readonly string Name;
        public readonly int Speed;
        public readonly int FlyTime;
        public readonly int RestTime;

        public Reindeer(string name, int speed, int flyTime, int restTime)
        {
            Name = name;
            Speed = speed;
            FlyTime = flyTime;
            RestTime = restTime;
        }
        public int GetDistance(int duration)
        {
            //Console.WriteLine("Calculating distance for " + Name + " : duration=" + duration);
            int fullCycleTime = FlyTime + RestTime;
            int fullCycles = duration / fullCycleTime;
            int distance = fullCycles * Speed * FlyTime + Math.Min(duration - fullCycleTime * fullCycles, FlyTime) * Speed;
            //Console.WriteLine("flyTimeCycle=" + flyTimeCycle + ", fullCycles=" + fullCycles + ", distance=" + distance);
            return distance;
        }
    }

    class ReindeerState
    {
        public bool Resting {get; private set;} = false;
        public int Distance {get; private set;} = 0;
        public int Points {get; private set;} = 0;
        private int FlyTime = 0;
        private int RestTime = 0;
        private readonly Reindeer Reindeer;

        public ReindeerState(Reindeer reindeer)
        {
            Reindeer = reindeer;
        }

        public void Tick()
        {
            if (Resting)
            {
                if (++RestTime == Reindeer.RestTime)
                {
                    RestTime = 0;
                    Resting = false;
                }
            }
            else
            {
                Distance += Reindeer.Speed;
                if (++FlyTime == Reindeer.FlyTime)
                {
                    FlyTime = 0;
                    Resting = true;
                }
            }
        }

        public void AwardPoint() {++Points;}
    }
}
