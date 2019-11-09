using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxAndLineCrossover
{
    class Program
    {
        static double a = 0.25;
        static int Tries = 2;
        static int PopulationNumber = 5;
        static List<int> Cities = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        static int[,] DistanceBetweenCities = new int[,]
        {
            { 0, 29, 82, 46, 68, 52, 72, 42, 51, 55, 29, 74, 23, 72, 46 },
            { 29, 0, 55, 46, 42, 43, 43, 23, 23, 31, 41, 51, 11, 52, 21 },
            { 82, 55, 0, 68, 46, 55, 23, 43, 41, 29, 79, 21, 64, 31, 51 },
            { 46, 46, 68, 0, 82, 15, 72, 31, 62, 42, 21, 51, 51, 43, 64 },
            { 68, 42, 46, 82, 0, 74, 23, 52, 21, 46, 82, 58, 46, 65, 23 },
            { 52, 43, 55, 15, 74, 0, 61, 23, 55, 31, 33, 37, 51, 29, 59 },
            { 72, 43, 23, 72, 23, 61, 0, 42, 23, 31, 77, 37, 51, 46, 33 },
            { 42, 23, 43, 31, 52, 23, 42, 0, 33, 15, 37, 33, 33, 31, 37 },
            { 51, 23, 41, 62, 21, 55, 23, 33, 0, 29, 62, 46, 29, 51, 11 },
            { 55, 31, 29, 42, 46, 31, 31, 15, 29, 0, 51, 21, 41, 23, 37 },
            { 29, 41, 79, 21, 82, 33, 77, 37, 62, 51, 0, 65, 42, 59, 61 },
            { 74, 51, 21, 51, 58, 37, 37, 33, 46, 21, 65, 0, 61, 11, 55 },
            { 23, 11, 64, 51, 46, 51, 51, 33, 29, 41, 42, 61, 0, 62, 23 },
            { 72, 52, 31, 43, 65, 29, 46, 31, 51, 23, 59, 11, 62, 0, 59 },
            { 46, 21, 51, 64, 23, 59, 33, 37, 11, 37, 61, 55, 23, 59, 0 }
        };

        static void Main(string[] args)
        {
            int current = 1;
            List<Individual> allSolutions = new List<Individual>();

            List<Individual> solutions = GenerateParents();
            allSolutions.AddRange(solutions);

            while (Tries > current)
            {
                Random random = new Random();
                for (int i = 0; i < PopulationNumber; i++)
                {
                    List<Individual> currentParents = solutions.OrderBy(x => random.Next()).Take(2).ToList();
                    Individual individual = GenerateNewIndividual(currentParents.FirstOrDefault(), currentParents.LastOrDefault());

                    solutions.Add(individual);
                    allSolutions.Add(individual);
                }
                solutions.OrderBy(x => random.Next()).ToList();
                current++;
            }

            PrintBestSolution(allSolutions.OrderBy(x => x.Evaluation).FirstOrDefault());
            Console.ReadKey();
        }

        public static List<Individual> GenerateParents()
        {
            Random r = new Random();
            List<Individual> response = new List<Individual>();
            for (int i = 0; i < PopulationNumber; i++)
            {
                Individual individual = new Individual()
                {
                    Cities = Cities.OrderBy(x => r.Next()).ToList()
                };
                individual.Evaluation = EvaluateSolution(individual.Cities);
                response.Add(individual);
            }
            return response;
        }

        public static Individual GenerateNewIndividual(Individual parentOne, Individual parentTwo)
        {
            List<int> cities;
            bool isValidSolution = false;
            Individual response = new Individual();

            while (!isValidSolution)
            {
                cities = new List<int>();
                double u = RandomNumberBetween(0, 1 + 2 * a);
                for (int i = 0; i < parentOne.Cities.Count(); i++)
                {
                    cities = new List<int>(); double sln = (parentOne.Cities[i] - a) + u * (parentTwo.Cities[i] - parentOne.Cities[i]);
                    cities.Add((int)sln < 0 ? 0 : (int)sln > 14 ? 14 : (int)sln);
                }
                response.Cities = cities;
                isValidSolution = cities.Distinct().Count() == parentOne.Cities.Count();
            }

            return response;
        }

        public static int EvaluateSolution(List<int> currentSolution)
        {
            int evaluation = 0;
            int solutionLength = currentSolution.Count - 1;
            for (int i = 0; i <= solutionLength; i++)
            {
                if (i != solutionLength)
                {
                    evaluation += DistanceBetweenCities[currentSolution[i], currentSolution[i + 1]];
                }
                else
                {
                    evaluation += DistanceBetweenCities[currentSolution[i], currentSolution[0]];
                }
            }
            return evaluation;
        }

        public static double RandomNumberBetween(double minValue, double maxValue)
        {
            Random random = new Random();
            return minValue + (random.NextDouble() * (maxValue - minValue));
        }

        public static void PrintBestSolution(Individual bestSolution)
        {
            Console.WriteLine($"Minimum distance after { Tries } tries is: { bestSolution.Evaluation } ({ string.Join(",", bestSolution.Cities.Select(x => x.ToString()).ToArray()) })");
        }
    }
}
