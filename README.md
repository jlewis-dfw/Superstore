# Superstore
Blazor Server App. Business analysis based off of CSV data. 

## Requirements
- Use Visual Studio, .NET 8.0, C# 12.0
- Use a public GitHub repository
- Use the attached "SampleSuperstore.csv" file as the datasource. 
- Create a Blazor server-side application that allows the user to drill around the dataset and understand what categories of products sell best in what locations during what seasons. 
- Keep track of how many hours spent. 

## Nuget Packages
| Name / Nuget Link | Purpose / Reasoning | GitHub | Documentation | 
| ----------------- |---------------------| ------------|-----------------------|
| [CsvHelper](https://www.nuget.org/packages/CsvHelper/30.1.0?_src=template)  | Importing CSV data | [Link](https://github.com/JoshClose/CsvHelper) | [Link](https://joshclose.github.io/CsvHelper/) |
| [Syncfusion](https://www.nuget.org/packages/Syncfusion.Blazor/) | Blazer component set | NA | [Link](https://blazor.syncfusion.com/documentation/introduction) |


## QA / Decision log
| M/D:T     | Type | Question / Decision                                          | Answer / Assumption / Notes                                                       |  
| --------- |------| -------------------------------------------------------------|-----------------------------------------------------------------------------------|
| 2/12:0800 | AC   | What is a season? It is not part of the dataset provided     | Meteorological temperate seasons in the northern hemisphere                       |
| 2/12:0800 | TA   | How to implement seasons                                     | OrderDate.Month [12,1,2]=Winter, [3,4,5]=Spring, [6,7,8]=Summer, [9,10,11]=Autumn |
| 2/12:0830 | DD   | Decision: Use CsvHelper nuget package for importing CSV data | Implementing CSV importing correctly would take too much time.   |
| 2/22:0830 | DD   | Decision: Use Syncfusion                                     | Syncfusion has one of the most complete and well documented controls. I have a startup company license  |


 ## Timelog
| M/D:T (CT)   | EstHrs | Task                                                       | Notes                                           | Total   |
| --------- |------| -------------------------------------------------------------|-------------------------------------------------|---------|
| 2/12:0715 | .25   | Create repo                                                 |                                                 |.25      |
| 2/12:0730 | .25   | Initialize solution                                         |                                                 |.50      |
| 2/12:0745 | .50   | Setup Readme structure                                      |                                                 |1.00     |
| 2/12:0800 | .50   | Requirements and CSV review to understand data              |                                                 |1.50     |
| 2/12:0830 | .50   | Design decisions / 3rd party Nuget package desicions        |                                                 |2.00     |
| 2/12:1730 | 2.50  | Coding                                                      |                                                 |4.00     |
| 2/13:1200 | 1.50  | Coding, PR, Basic requirements met                          |                                                 |5.50     |


