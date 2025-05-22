using System;
using System.Collections.Generic;

class Program
{
    public static void Main()
    {
        Console.WriteLine("Total safe squares from (0,0): " + TotalSafeSquares());
        var (stepsToMineSurrounded, coordinates) = StepsToMineSurrounded(0, 0);
        Console.WriteLine("Steps to position surrounded by mines from (0, 0): " + stepsToMineSurrounded);
        Console.WriteLine("Coordinates of the position: " + coordinates);
    }


    //Logic to determine if square is safe.
    public static bool IsSafe(int x, int y)
    {
        int product = x * y;
        int sum = 0;
        foreach (char digit in Math.Abs(product).ToString())
            sum += digit - '0';
        return sum < 19;
    }


    //determines all safe squares within the boundaries given.
    public static int TotalSafeSquares()
    {
        var visited = new HashSet<(int, int)>();
        var queue = new Queue<(int, int)>();
        queue.Enqueue((0, 0));
        visited.Add((0, 0));
        int count = 0;
        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            count++;

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (nx >= -9999 && nx <= 9999 && ny >= -9999 && ny <= 9999 && !visited.Contains((nx, ny)) && IsSafe(nx, ny))
                {
                    visited.Add((nx, ny));
                    queue.Enqueue((nx, ny));
                }
            }
        }

        return count;
    }


    //We defined a journey as the maximum number of steps taken before you reach mines in all possible future directions, disregarding the path you just came from by avoiding backtracking.
    //This code loops all possible journeys and returns the shortest safest journey and the coordinates and number of steps of that journey
    public static (int, (int, int)) StepsToMineSurrounded(int a, int b)
    {
        var visited = new HashSet<(int, int)>();
        var queue = new Queue<((int, int) position, int steps, (int, int) previous)>();
        queue.Enqueue(((a, b), 0, (a, b)));
        visited.Add((a, b));
        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        while (queue.Count > 0)
        {
            var ((currentX, currentY), steps, previous) = queue.Dequeue();
            bool allNewDirectionsMined = true;

            for (int i = 0; i < 4; i++)
            {
                int nx = currentX + dx[i];
                int ny = currentY + dy[i];

                if ((nx, ny) == previous)
                {
                    continue; // Ignore the step backwards
                }

                if (nx >= -9999 && nx <= 9999 && ny >= -9999 && ny <= 9999)
                {
                    if (!IsSafe(nx, ny))
                    {
                        continue;
                    }
                    else
                    {
                        allNewDirectionsMined = false;
                        if (!visited.Contains((nx, ny)))
                        {
                            visited.Add((nx, ny));
                            queue.Enqueue(((nx, ny), steps + 1, (currentX, currentY)));
                        }
                    }
                }
            }

            if (allNewDirectionsMined)
            {
                return (steps, (currentX, currentY));
            }
        }

        return (-1, (0, 0)); // No such position found
    }
}