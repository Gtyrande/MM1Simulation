using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM1Simulation
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        const int QLimit = 100;
        const int BUSY = 1;
        const int IDLE = 0;

        static int nextEventType;
        static int numCustsDelayed;
        static int numDelaysRequired;
        static int numEvents = 2;
        static int numInQ;
        static int serverStatus;

        static double areaNumInQ;
        static double areaServerStatus;
        static double meanInterarrival;
        static double meanService;
        static double simTime;
        static double timeLastEvent;
        static double totalOfDelays;

        static double[] timeArrival = new double[QLimit + 1];
        static double[] timeNextEvent = new double[3];
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //
            Console.WriteLine("Input mean interarrival: ");
            meanInterarrival = double.Parse(Console.ReadLine());
            Console.WriteLine("Input mean service time: ");
            meanService = double.Parse(Console.ReadLine());
            Console.WriteLine("Imput number of customers: ");
            numDelaysRequired = int.Parse(Console.ReadLine());

            //
            Initialize();

            //
            while (numCustsDelayed < numDelaysRequired)
            {
                Timing();
                UpdateTimeAvgStats();

                //
                switch (nextEventType)
                {
                    case 1:
                        Arrive();
                        break;
                    case 2:
                        Depart();
                        break;
                }
            }

            //
            Console.Write("Average delay in queue: ");
            Console.WriteLine(totalOfDelays / numCustsDelayed);
            Console.Write("Average number in queue: ");
            Console.WriteLine(areaNumInQ / simTime);
            Console.Write("Server utilization: ");
            Console.WriteLine(areaServerStatus / simTime);
            Console.Write("Time simulation ended: ");
            Console.WriteLine(simTime);
            Console.WriteLine(numCustsDelayed);
        }

        static void Initialize()
        {
            //
            simTime = 0;

            //
            serverStatus = IDLE;
            numInQ = 0;
            timeLastEvent = 0.0;

            //
            numCustsDelayed = 0;
            totalOfDelays = 0;
            areaNumInQ = 0;
            areaServerStatus = 0;

            //
            timeNextEvent[1] = simTime + expon(meanInterarrival);
            timeNextEvent[2] = 1.0e+30;
        }

        static void Timing()
        {
            double minTimeNextEvent = 1.0e+29;
            nextEventType = 0;

            //
            for (int i = 1; i < numEvents + 1; i++)
            {
                if (timeNextEvent[i] < minTimeNextEvent)
                {
                    minTimeNextEvent = timeNextEvent[i];
                    nextEventType = i;
                }
            }

            //
            simTime = minTimeNextEvent;
        }

        static void Arrive()
        {
            double delay;

            //
            timeNextEvent[1] = simTime + expon(meanInterarrival);
            //
            if (serverStatus == BUSY)
            {
                ++numInQ;
                if (numInQ > QLimit)
                {
                    Console.WriteLine("Shit!");
                }
                timeArrival[numInQ] = simTime;
            }
            else
            {
                delay = 0;
                totalOfDelays += delay;

                ++numCustsDelayed;
                serverStatus = BUSY;

                timeNextEvent[2] = simTime + expon(meanService);
            }
        }

        static void Depart()
        {
            double delay;

            if (numInQ == 0)
            {
                //
                serverStatus = IDLE;
                timeNextEvent[2] = 1.0e+30;
            }
            else
            {
                //
                --numInQ;

                //
                delay = simTime - timeArrival[1];
                totalOfDelays += delay;

                //
                ++numCustsDelayed;
                timeNextEvent[2] = simTime + expon(meanService);

                for (int i = 1; i < numInQ + 1; i++)
                {
                    timeArrival[i] = timeArrival[i + 1];
                }
            }
        }

        static void UpdateTimeAvgStats()
        {
            double timeSinceLastEvent;

            //
            timeSinceLastEvent = simTime - timeLastEvent;
            timeLastEvent = simTime;

            //
            areaNumInQ += numInQ * timeSinceLastEvent;
            areaServerStatus += serverStatus * timeSinceLastEvent;
        }

        static double expon(double mean)
        {
            Lgcrand rand = new Lgcrand();
            return -mean * Math.Log(rand.Lcgrand(3));
        }
    }
}
