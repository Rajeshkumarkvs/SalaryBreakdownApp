# SalaryBreakdownApp application (.Net core 3.1)
This console application is created in .Net core 3.1 and you need install .Net core SDK to run this application. The project solution contains the below 3 projects.

## SalaryBreakdownApp - Console application
This project is to get the user input for Salary package and Pay frequency.
A Json file (TaxRules.json) is used to calculate the salary breakdown based on income rules and this file can be easily updated to handle the changes in tax laws.

## SalaryBreakdown - Class Library
This project contains the business logic to calculate the salary breakdown. This project can be used as business layer for different type of applications. For example Front End UI applications, Rest API, etc..

## SalaryBreakdownApp.Test - MSTest project
This project is to test the SalaryBreakdown class library to make sure all functionalities are working fine as expected. It used mock tax rules for the income tax rules.

## Future Considerations
Extend the logging - logging into different sources like console, files, event source, db based on application settings

Create API for others to consume
