### ERM.ConsumptionParser 
Project to parse and apply 1 or more processors/filters to customer energy consumption feeds. 
The default startup project is "ERM.ConsumptionParser.Console" and be advised that my project naming conventions
are influenced by Domain Driven Design. 
 
## Requirement Assumptions 
- 20% variance requirement is "inclusive" 
- Workflow to manage lifecycle / persist metadata regarding application runs is out of scope 
- Third party CSV parsers not allowed. 
 
## Alex Design Considerations 

[SQL Server]

Although I assume a purely c# implementation is required for this demonstration it is in my experience with large
consumption feeds that a relational database should be considered as part of the design 
(better suited for data retrieval, memory management and aggregate calculations) allowing for scale. 
 
[Extensibility]

I did not see a real need to use MEF for this simple example project and thus relied on SOLID and Microsoft Unity
for IOC. New processors can easily be added without modifying any code from the body of the application.  

[Other]
 
Object to Object mapping provided by the popular AutoMapper.

Finally I assumed ERM would choose .NET 4.5.2 over ".NET Core" as it may be considered too young for critical
production releases. 
 
 
## Performance 
I did some amount of benchmarking with file operations (1GB+) and found a blend of 
parallel processing and reading line by line using the simple StreamReader to be the fastest overall. 
Reading large files entirely into memory to then process does not scale. 
Another interesting note was the use of String.Split() being a performance choke point that could be mitigated 
via a custom implementation. There was also an option of bypassing. NET and using the much faster native windows
API but this would limit deployment options and kill any chance of upgrading the project to .NET Core in the future.