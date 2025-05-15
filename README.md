This is a C# (.NET Core) console application designed to find the total number of navigation paths from the astronaut’s starting position (S) to the final destination (F) based a dynamic cosmic navigation map. 
The map is represented as a 2D grid and contains also open spaces (O), obstacles (X) symbols.

Features:
Generates a random cosmic navigation map based on user input.
Calculates and displays all possible paths and print shortest path from start to finish.
As bonus feature the app exports the shortest path and map to a CSV file (ShortestPathReport.csv) located in the bin\Debug folder after running the app.

CSV Report Export:
The app includes a feature to export the shortest path and the map to a CSV file, saved in the bin\Debug folder after running.
Although I wasn’t very familiar with CSV reporting, I decided to implement it as part of the project requirements.
I used guidance and support to understand how to create CSV files in C# and and it was good opportunity for learning.
The email-sending feature mentioned in the bonus objectives was not implemented

How to Run:
Clone or download this repository.
Build and run the console app using .NET Core.
After execution, check the bin\Debug folder for the generated ShortestPathReport.csv file.

