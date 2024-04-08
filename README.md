<img align="center" alt="airport" width="700px" style="padding-right:10px;" src="https://www.ifminvestors.com/siteassets/shared-media/assets/vienna-airport/vienna-airport-1.jpg" />  

# Airport
A complex end-to-end system (Backend & Frontend) that simulates activity at an airport, and combines 3 systems that are connected together.
<br>
In the Console part there is a flight simulator that communicates through an HTTP network and injects a JSON object through a Post request to an API server built in Asp.net core web Api.
<br>
On the server side, multithreading / events / EF6 is used and work against SQL Server with multiple asynchronous calls.
<br>
And a client in Angular that displays it in real time.
<br>

## Simulator:
The simulator is an independent project based on the Console application (C# - .Net Core).
<br>
The simulator produces flights that arrive for landing and approach leg 1.
<br>
The flight simulator communicates via the HTTP network and injects a JSON object (in a random period of time) via a Post request to the API server built in Asp.net core web Api.
<br>

## Server:
API server built in Asp.net core web API.
<br>
On the server side, multithreading / events / EF6 is used and work with SQL Server with multiple asynchronous calls. All logic is done on the server side.
<br>

### Server logic:
At any given time, a maximum of 4 planes are at the airport.
<br>
Each leg has a maximum of one flight/plane.
<br>
Each leg has a specific dwell time that the aircraft must be in before moving to the next stage (Leg). 
<br>

#### Total Of 9 Legs:
Legs 1-3 are in the air on approach to land.
<br>
Leg 4 with status of arrivals / departures (depending on where the flight came from).
<br>
Leg 5 serves as a shuttle to the parking terminal.
<br>
Leg 6 and 7 are used as flight sleeves for a parking position for airplanes, unloading and loading of passengers.
<br>
Leg 8 serves as a shuttle to the departure terminal at leg number 4. From there the flight leaves for leg number 9 which is an airport exit.
<br>

#### The transition between the different legs is carried out as follows:
leg number 1 can only move to leg number 2.
<br>
leg number 2 can only move to leg number 3.
<br>
leg number 3 can only move to leg number 4.
<br>
leg number 4 can only move to leg number 5 or 9 (depending on the flight status).
<br>
leg number 5 can only move to leg number 6 or 7 (depending on who is free).
<br>
leg number 8 can only move to leg number 4.
<br>
The server manages all the logic, checks which next leg is optional to switch to, waits for callbacks when a leg becomes free, and more.
<br>

## Data Base
All the activities of the flights, including transitions between the different legs, entry and exit times, loading and unloading of passengers and the flight status are saved on logs in the database, on MSSQL (SQL Server).
<br>

## Client
An Angular client that communicates with the server (API server) and receives logs and flights from the controller and displays the data in real time.


### Dependencies
* .Net Core
* ASP.Net Core Web Api
* SQL Server
* npm

### Installing
* Pull from here
* Add migration and update the local database
* Run npm i in the client folder

### Executing Program
* Run the web api
* Run the flight simulator
* Run the angular client
