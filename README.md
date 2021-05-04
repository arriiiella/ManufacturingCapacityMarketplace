# ManufacturingCapacityMarketplace

This repo is my Dissertation Product to create an Online Marketplace to buy and sell excess manufacturing.

## Prerequisites ##
- Visual Studio Code
- [Microsoft SQL Server](https://www.microsoft.com/en-gb/sql-server/sql-server-downloads)
- [Microsoft SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)
- [.Net Core](https://dotnet.microsoft.com/download/dotnet) version 5.0.102 is used.
- [NPM](https://nodejs.org/en/download/) version 15.5.1 is used.
- Install NuGet Gallery from VSCode extensions 

## Database Setup ##
  1. There is a script file in the ZIP called ProductionCapacityMarketplaceDDLScript.sql
  2. Open SQL Server Management Studio
  3. Connect to the local server instance
  4. Open the script and execute.

## API Setup ##
### NuGet Gallery ###
Within the NuGet gallery the following packages are required to be installed (List can also be found in API.csproj) :
- Microsoft.EntityFrameworkCore Version 5.0.2
- Microsoft.EntityFrameworkCore.Analyzers Version 5.0.2
- Microsoft.EntityFrameworkCore.Design Version 5.0.2
- Microsoft.EntityFrameworkCore.SqlServer Version 5.0.2
- Newtonsoft.Json Version 13.0.1
- System.IdentityModel.Tokens.Jwt Version 6.9.0
- AutoMapper Version 10.1.1
- AutoMapper.Extensions.Microsoft.DependencyInjection Version 8.1.1
- Microsoft.AspNetCore.Authentication.JwtBearer Version 5.0.2
- Microsoft.AspNetCore.Mvc.NewtonsoftJson Version 5.0.5

### Connection String ###
The server name in the connection string for the database will need to be changed in the appsettings.Development.json file. 

Server name can be found in SQL Server during connection.

## Client Setup ##
### NPM ###
[NPM](https://www.npmjs.com/get-npm) can be installed by typing the following into the command line: `npm install npm@latest -g`

### Node Modules ###
If the node modules folder is not present, go to the client directory and in the terminal write the following: `npm install` 
If there is a node modules folder then go to the client directory and type `npm update` into the terminal to ensure the versions match.

### Angular CLI ###
[Angular](https://angular.io/cli) can be installed using the Angular Command Line Interface. 

To install type the following in the terminal (it is installed globally) `npm install -g @angular/cli`

### SSL Certificate ###
There is a folder at the root of the client folder called ssl with a certificate and key inside. 
The certificate needs to be installed on the machine and can be done using the following steps:

	1. Double click on the certificate (server.crt)
	2. Click on the button “Install Certificate …”
	3. Select whether you want to store it on user level or on machine level
	4. Click “Next”
	5. Select “Place all certificates in the following store”
	6. Click “Browse”
	7. Select “Trusted Root Certification Authorities”
	8. Click “Ok”
	9. Click “Next”
	10. Click “Finish”

## Running the Application ##
  1. Change directory to the API folder in the terminal and type `dotnet clean` then `dotnet build` then `dotnet run`.
  2. Once the API is running, open a new terminal and change directory to the client folder and type `ng serve`
  3. A localhost link will appear which can be clicked and will open a new browser, the link will be https://localhost:4200/.
