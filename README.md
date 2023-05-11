# Smart Lock 
This is a solution for processing high-traffic smart locks managment.
It has an Restfull API to be used publicly and the solution supports JWT for authentication and authorization.
It's extensible with the dispatcher mechanism that supports application events.

## Technologies
*  ASP.NET Core 7 (Web Api)
*  Entity Framework Core 7
*  MediatR, FluentValidation, AutoMapper
*  Nlog
*  xUnit, FluentAssertions

## Getting Started
The easiest way to get started is to follow these instructions to get the project up and running:

### Prerequisites
You will need the following tools:
* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (version 17.5.5 or later)
* [.NET 7](https://dotnet.microsoft.com/download/dotnet/7.0)

  

### Setup
Follow these steps to get your development environment set up:
1. Clone the repository
2. At the root directory, restore required packages by running:

```
dotnet restore

```

3. Next, build the solution by running:

```
dotnet build

```

4. Once the solution has been built, API can be started within the `\UI\SmartLock.UI.RestApi` directory, by running:

```
dotnet run

```

