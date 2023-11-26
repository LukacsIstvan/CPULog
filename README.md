# CPULog
Simple program thats logging the CPU statistics, storing the data on a server, controlled from a separate frontend app.

 ## About The Project
CPULog is a .Net Core 3.1 project that contains the following parts:
1. **CPULogMonitor**:  .Net framework windows service that runs in the background and continously recording the hosts CPU load(%) and temperature(°C) in a set sensor time interval using OpenHardwareMonitor. The monitor periodicaly sending the collected data to the CPULogServer and if the data recived archive them into JSON.
2. **CPULogServer**: .Net Core 3.1 Web API application that stores data from the connected monitor services using SQLite, controlling the monitors sensor timer via TCP, and providing API routes for the CPULogFrontend with the sored data.
3. **CPULogFrontend**: Blazor WebAssembly app that displays the stored data on the CPULogServer, managing the server starting and the connected monitors sensor timers. The app using a JWT based authentication system that restricts the management of the connected clients.

## Installation


#### CPULogMonitor

1.   Build the project
2.  Open Commandline in windows as adminsitrator
3.  `cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319\`
4. `installutil.exe <ThePathWhereTheClonedRepoIs>\CPULog\CPULogMonitor\bin\Debug\CPULogMonitor.exe`
5. Open Services  in windows as adminsitrator
6. Find CPULogMonitorService and run the service
#### CPULogServer & Frontend

1.   The projects should be runing as multiple startup projects
2.  Login or register then login on the frontend after
3. Navigate to the server page and start the server
4. Alternatively you can use the SwaggerUI API routes
5. Use the app



