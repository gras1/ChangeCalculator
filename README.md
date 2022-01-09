# ChangeCalculator

## Task
Create an application that will, given a UK currency amount and the purchase price of a product, display the change to be returned split down by denomination, largest first.

Example:
  Given £20 and a product price of £5.50, the application will output:

  Your change is:
  1 x £10
  2 x £2
  1 x 50p


## To get started

### Running the .NET Api and unit/integration tests

1. ensure you have the .NET 6 SDK installed on your computer

2. in a Terminal window type 'git clone https://github.com/gras1/ChangeCalculator.git'

3. in a Terminal window cd in to the ChangeCalcuator root folder and type 'dotnet build' - this will install the nuget packages and compile the projects

4. in a Terminal window cd in to the ChangeCalcuator root folder and type 'dotnet test' - to run all unit tests

5. in a Terminal window cd in to the Experian.Net.ChangeCalcuator.Api project folder and type 'dotnet run', then in a browser go to http://localhost:5164/index.html. This presents the swagger window. Click the 'Try it out' button, use the following values: Currency: "GBP", AmountOfCash: 20, Cost: 5.50 and click the 'Execute' button. Once it has successfully run, take a look at the response body and it should provide the correct list of change as listed above.

### Running the website

1. ensure you have Node installed on your computer

2. ensure you have npm installed on your computer

3. Run the following command to install http-server:
npm install http-server -g

4. Before starting the http-server, you may need to temporarily increase your privledges, I did this in Powershell running as Administrator by typing the following (and saying "Yes to All" when prompted):
Set-ExecutionPolicy -ExecutionPolicy Unrestricted

5. in a Terminal window cd in to the Experian.React.ChangeCalcuator.Web folder, to launch the HTTP server on port 8080 type:
http-server

6. in a browser, go to http://localhost:8080/index.html