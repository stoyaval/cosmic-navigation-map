// See https://aka.ms/new-console-template for more information

using System;

int m; // the number of rows in the navigation map
int n; // the number of columns in the navigation map

//ask the user to enter the number of rows
Console.Write("Map rows: ");
m = int.Parse(Console.ReadLine());
Console.WriteLine();

//validate that the number of the rows is between 2 and 100 (inclusive)

while (m < 2 || m > 100)
{
    Console.WriteLine("Invalid number.It should be between 2 and 100 inclusive. Please try again!");
    Console.WriteLine();
    Console.Write("Map rows: ");
    m = int.Parse(Console.ReadLine());
}

//ask the user to enter the number of columns
Console.Write("Map columns: ");
n = int.Parse(Console.ReadLine());
Console.WriteLine();

//validate that the number of the columns is between 2 and 100 (inclusive)

while (n < 2 || n > 100)
{
    Console.WriteLine("Invalid number. It should be between 2 and 100 inclusive. Please try again!");
    Console.WriteLine();
    Console.Write("Map columns: ");
    n = int.Parse(Console.ReadLine());
}


Console.WriteLine("Cosmic map:");
Console.WriteLine();

// create 2D array to store the navigation map symbols

char[,] map = new char[m, n];

//Read each row of the map
for (int row = 0; row < m; row++)
{
    string rowInput = Console.ReadLine().Trim().ToUpper();

    // If spaces are missing, this code insert them between the characters
    if (!rowInput.Contains(" "))
    {
        rowInput = string.Join(" ", rowInput.ToCharArray());
    }
    string[] rowSymbols = rowInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    //Check if the row contains the expected number of symbols
    if (rowSymbols.Length != n)
    {
        Console.WriteLine($"Invalid row! Expected {n} symbols. Please try again!"); //show an error message if the number of symbols is not as expected
        Console.WriteLine();
        row--;
        continue;
    }

    // validate if the entered symbol is any of the allowed characters S, O, X, F
    bool isValid = true;
    foreach (string symbol in rowSymbols)
    {
        if (symbol != "S" && symbol != "O" && symbol != "X" && symbol != "F")
        {
            Console.WriteLine("Invalid cosmic symbol! Only S, O, X, F are allowed. Please try again!"); // show an error message if the condition isn't met
            Console.WriteLine();
            isValid = false;
            break;
        }
    }
    if (!isValid)
    {
        row--;
        continue;
    }

    //if the input is valid, store the symbols in the map
    for (int col = 0; col < n; col++)
    {
        map[row, col] = rowSymbols[col][0];

    }
    Console.WriteLine();
}

// locate the coordinates of the S (the starting point) and F (the final destination)
// and count them to check whether there is duplicated S or F.

int startRow = -1; int startColumn = -1;
int finishRow = -1; int finishColumn = -1;
int startCount = 0; int finishCount = 0;

for (int row = 0; row < m; row++)
{
    for (int col = 0; col < n; col++)
    {
        if (map[row, col] == 'S')
        {
            startCount++;
            startRow = row;
            startColumn = col;

        }
        else if (map[row, col] == 'F')
        {
            finishCount++;
            finishRow = row;
            finishColumn = col;

        }
    }
}
// throw an error if the starting or the final position are not found or appear more than once

if (startCount > 1 || finishCount > 1)
{
    Console.WriteLine("Error: More than one 'S' (start) or 'F' (finish) found.");
    return;
}
else if (startCount == 0 || finishCount == 0)
{
    Console.WriteLine("Error! Start or finish position is missing.");
    return;
}

//Initialization of the variables needed
bool[,] visitedPosition = new bool[m, n]; //tracks the already visited positions on the navigation map ("true" means visited) 

int totalPaths = 0; // counts all valid paths from 'S' to 'F'

int shortestPathLength = int.MaxValue; // //keep a track of the shortest path found 


//step 1: create a function which explores all possible paths from 'F' to 'S'

void ExploreCosmicPath(char[,] map, bool[,] visitedPosition, int row, int col, int stepCount)
{
    // If current position is 'F' (Finish), update total paths and the shortest path

    if (map[row, col] == 'F')
    {
        totalPaths++;

        if (stepCount < shortestPathLength)
        {
            shortestPathLength = stepCount;
        }
        return;
    }


    visitedPosition[row, col] = true; // Mark this cell as visited

    // Explore all 4 directions (up/down/left/right)

    if (row - 1 >= 0 && map[row - 1, col] != 'X' && visitedPosition[row - 1, col] == false)
    {

        ExploreCosmicPath(map, visitedPosition, row - 1, col, stepCount + 1);

    }
    if (row + 1 < m && map[row + 1, col] != 'X' && visitedPosition[row + 1, col] == false)
    {

        ExploreCosmicPath(map, visitedPosition, row + 1, col, stepCount + 1);

    }
    if (col - 1 >= 0 && map[row, col - 1] != 'X' && visitedPosition[row, col - 1] == false)

    {
        ExploreCosmicPath(map, visitedPosition, row, col - 1, stepCount + 1);

    }

    if (col + 1 < n && map[row, col + 1] != 'X' && visitedPosition[row, col + 1] == false)
    {

        ExploreCosmicPath(map, visitedPosition, row, col + 1, stepCount + 1);

    }

    visitedPosition[row, col] = false; //unmark the cell so it can be used for other paths

}

int[,] pathMap = new int[m, n]; // map that will show the step numbers along the shortest path (from 1 to L)

bool pathFound = false;

visitedPosition = new bool[m, n];

// step 2: create a function to visualize the shortest path
void VisualizeShortestPath(char[,] map, int[,] pathMap, bool[,] visitedPosition, int row, int col, int stepCount)
{

    if (pathFound) // // Stops the program if the path is already found
        return;

    if (row < 0 || row >= m || col < 0 || col >= n) // Stop if the cell are out of bounds
        return;

    // Stop if the cell is an asteroid, already visited, or already part of a path
    if (map[row, col] == 'X' || visitedPosition[row, col] || pathMap[row, col] > 0)
        return;


    if (stepCount > shortestPathLength) //Stop if stepCount exceeds shortest path length
        return;


    if (map[row, col] == 'F' && stepCount == shortestPathLength) // update the map if the shortest path is found
    {
        pathMap[row, col] = stepCount; // Replace 'F' with the number at the finish position
        pathFound = true;
        return;
    }

    // Mark the cell as visited and store the step into the pathMap
    visitedPosition[row, col] = true;
    pathMap[row, col] = stepCount;

    // Explore in all four directions
    VisualizeShortestPath(map, pathMap, visitedPosition, row - 1, col, stepCount + 1); // up
    VisualizeShortestPath(map, pathMap, visitedPosition, row + 1, col, stepCount + 1); // down
    VisualizeShortestPath(map, pathMap, visitedPosition, row, col - 1, stepCount + 1); // left
    VisualizeShortestPath(map, pathMap, visitedPosition, row, col + 1, stepCount + 1); // right

    // If path was not found in any direction, return back
    if (!pathFound)
    {
        pathMap[row, col] = 0;
        visitedPosition[row, col] = false;
    }

}

ExploreCosmicPath(map, visitedPosition, startRow, startColumn, 0);

// throw error messages if no paths found
if (totalPaths == 0)
{
    Console.WriteLine("Error: No valid cosmic paths found!");
    return;

}
// throw error messages if no shortest path found
if (shortestPathLength == 0 || shortestPathLength == int.MaxValue)
{
    Console.WriteLine("Error: No shortest path found!");
    return;
}

VisualizeShortestPath(map, pathMap, visitedPosition, startRow, startColumn, 0);

// throw an error message if the shortest path couldn't be visualized

if (!pathFound)
{
    Console.WriteLine("Error: Unable to display the shortest path.");
    return;
}

//Print the final output 

Console.WriteLine($"Number of possible paths: {totalPaths}");
Console.WriteLine();
Console.WriteLine($"Shortest path length: {shortestPathLength}");
Console.WriteLine();
Console.WriteLine($"Shortest path map: ");
Console.WriteLine();
for (int row = 0; row < m; row++)
{
    for (int col = 0; col < n; col++)
    {
        if (pathMap[row, col] > 0)
        {
            Console.Write($"{pathMap[row, col]} ");
        }
        else
        {
            Console.Write($"{map[row, col]} ");
        }

    }
    Console.WriteLine();
    Console.WriteLine();
}

ExportShortestPathToCsv("ShortestPathReport.csv", map, pathMap, map.GetLength(0), map.GetLength(1));
static void ExportShortestPathToCsv(string filePath, char[,] map, int[,] pathMap, int rows, int cols)
{
    // Create the CSV file for writing
    using (StreamWriter writer = new StreamWriter(filePath))
    {
        // Itarate through each row of the map
        for (int row = 0; row < rows; row++)
        {

            string[] line = new string[cols]; // Prepare an array that will hold each cell CSV value for the current row
            //Itarate through each column in the current row
            for (int col = 0; col < cols; col++)
            {
                if (pathMap[row, col] > 0)// If the cell is part of the shortest path (step number > 0) write the step number
                    line[col] = pathMap[row, col].ToString();
                else //  Otherwise, write the original map symbols
                    line[col] = map[row, col].ToString();
            }
            // Join the row's cells with commas and write the line to the CSV file
            writer.WriteLine(string.Join(",", line));
        }
    }
}

