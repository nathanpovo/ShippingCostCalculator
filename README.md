# ShippingCostCalculator

## Prerequisites

- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
- [dotnet-ef tool](https://www.nuget.org/packages/dotnet-ef/)

## Setup

Create the database with the command:

``` powershell
dotnet ef database update -p ./ShippingCostCalculator.Data/ShippingCostCalculator.Data.csproj
```

This will create the database at the path `./ShippingCostCalculator/ShippingCostCalculator.Data/ShippingData.db`.

Setup the database connection string for the Blazor application by using the command

``` powershell
dotnet user-secrets set ConnectionStrings:ShippingCostCalculatorDatabase "Data Source=Path/to/db/ShippingData.db;" -p .\ShippingCostCalculator\ShippingCostCalculator.csproj
```

or by adding the following json block

``` json
{
    "ConnectionStrings": {
        "ShippingCostCalculatorDatabase": "YOUR SQLITE CONNECTION STRING HERE"
}
```

to the `appsettings.json` file.

## Building

Build the project with the commands:

``` powershell
dotnet restore
dotnet build
```

## Running

To run the Blazor application, use the command:

``` powershell
dotnet run --project .\ShippingCostCalculator\ShippingCostCalculator.csproj
```

Then go to the url https://localhost:5001/ with a browser.

## Possible improvements and additions

- Test UI
- Test observables in the domain project
- Improve aesthetics of the UI
- Add option to change the unit of weight
- Add option to change the unit of package dimensions
- Add option to change the currency (use the [exchangerate.host](https://exchangerate.host/) API to get the currency conversion rates)
