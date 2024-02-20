# .NET Project (SmartGarage)

## Overview

SmartGarage is a web application designed to manage and organize garage visits efficiently. Users can expect a range of features including user authentication, garage visit management, vehicle registration, invoicing, and administrative controls.

Application Link - [smartgarage54a.azurewebsites.net](https://smartgarage54a.azurewebsites.net)

---
<br />

## Features

- User authentication and authorization.
- Garage visit management: Add, remove, and update visits to the garage.
- Vehicle registration: Register vehicles to specific garage customers.
- Administrative controls: Admin-only access to manage employees and services offered by the garage.
- PDF Generator: Users can generate and download PDF invoices for each visit.
- Map Integration: After logging in, customers are greeted with a homepage featuring a map displaying the location of the garage. 

<br />

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SQL Database
- Azure for application and database hosting
- Microsoft Identity for user authentication and authorization
- JavaScript

---

## Local Installation

Follow these steps to set up and run the application:

1. **Clone the repository**:

    ```
    git clone https://github.com/TelerikAcademySmartCrew/SmartGarage.git
    ```

2. **Navigate to project directory**:

    ```
    cd SmartGarage
    ```

3. **Set up the database**:
    - Open Package Manager Console.
    - Make sure your default project reference is SmartGarage.Data
    - Run the following commands:
    ```
    Add-Migration Initial
    Update-Database
    ```

4. **Run the application**:
    - Press F5 or run the application in your preferred IDE.

<br>

## SmartGarage App Step-by-Step Guide

Welcome to the SmartGarage app step-by-step guide! This guide will walk you through using the SmartGarage application effectively. SmartGarage is a comprehensive platform designed to manage garage operations seamlessly. Whether you're an admin, an employee, or a customer, this guide will cover everything you need to know.

### For Admins

#### 1. Employee Management
   1. Log in using the provided admin credentials.
   2. Navigate to the "EMPLOYEES" section.![Screenshot 2024-02-20 124725](https://github.com/TelerikAcademySmartCrew/SmartGarage/assets/157811229/dd0dd346-4fac-48a0-86a7-bdcc57c44b7d)

   3. Register new employees by inputting their email addresses.
   4. Employees can log in using their email and a predefined password.

#### 2. Service Types Management
   1. Admins can manage the types of services offered by the garage.
   2. Navigate to the "GARAGE REPAIR ACTIVITIES" section.
   3. Add service types based on the garage's offerings.![Screenshot 2024-02-20 125226](https://github.com/TelerikAcademySmartCrew/SmartGarage/assets/157811229/db70ac26-6614-4f60-9cef-15816f20cd82)


### For Employees

#### 1. Customer Registration
   1. Log in using your credentials.
   2. Navigate to the "REGISTER CUSTOMER" section. ![image](https://github.com/TelerikAcademySmartCrew/SmartGarage/assets/157811229/4925406f-85ad-43b8-8928-93a35686c722)

   3. Register new walk-in customers by entering their email addresses.
   4. Customers will receive an email with their login credentials.

#### 2. Vehicle Registration
   1. Navigate to the "REGISTER VEHICLE" section.
   2. Register a new vehicle for a customer in the system.

#### 3. Visit Management
   1. Navigate to the "CREATE VISIT" section.
   2. Create new visits for each vehicle.
   3. Advance the visit status from "Pending" to "Active" to start adding services.
   4. Add the services performed on the vehicle. ![Screenshot 2024-02-20 132102](https://github.com/TelerikAcademySmartCrew/SmartGarage/assets/157811229/755254cd-f6db-4391-8d0d-b0719391b591)

   5. Enter the price of each service.
   6. Change the visit status from "Active" to "Complete" to mark it as done.
   7. Advance the status from "Complete" to "Paid" once the customer has paid for the services.
   8. Access a list of all active visits in a separate tab.

### For Customers

#### 1. Visit Overview
   1. Log in using your credentials.
   2. Access the "MY VISITS" tab to view details of your registered vehicles' visits.
   3. Rate each visit upon completion.![Screenshot 2024-02-20 131341](https://github.com/TelerikAcademySmartCrew/SmartGarage/assets/157811229/14358a2e-967d-45c5-a216-751beb69829b)


#### 2. Visit Details
   1. Choose your preferred currency for the total price display.
   2. Download a PDF report once the visit status is set to "Paid".![Screenshot 2024-02-20 131341](https://github.com/TelerikAcademySmartCrew/SmartGarage/assets/157811229/c44366ab-8ccf-423e-bec5-616c4395296c)


#### 3. Profile Management
   1. Update your profile details as needed.![Screenshot 2024-02-20 131534](https://github.com/TelerikAcademySmartCrew/SmartGarage/assets/157811229/7a98b87f-5398-4df3-a25c-86e3554ab01f)


### Conclusion

SmartGarage provides a user-friendly interface for managing garage operations efficiently. Whether you're an admin, employee, or customer, the application offers tailored features to streamline your experience. Follow these steps to make the most out of SmartGarage. Enjoy using SmartGarage and feel free to reach out for any assistance or feedback!

## Database Diagram

<img>![image](https://github.com/TelerikAcademySmartCrew/SmartGarage/assets/157811229/ff3fea0d-1e4c-4c50-9b45-3ea059a99c40)
</img>

<br>

## Solution Structure

<img>![image](https://github.com/TelerikAcademySmartCrew/SmartGarage/assets/157811229/20e72ad5-04c1-4d3c-aff3-21747aef8c89)
</img>

---

## Contributors
For further information, please feel free to contact us:

| Authors              | Emails    | GitHub|
| ------               | ------    |------ |
| Borislav Stefanov | traev50@gmail.com     | [bobistefxA54](https://github.com/bobistefxA54)  |
| Vencislav Georgiev | venci1983@gmail.com    | [ventsy-TA-54](https://github.com/ventsy-TA-54)  |
| Plamen Nedelchev | plamenss99@abv.bg    | [plmnnedelchev](https://github.com/plmnnedelchev)  |

---
<br />
