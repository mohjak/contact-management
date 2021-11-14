# Contact Management

## Brief
Web API application to manage Extendable Customers data (simple contact management).

## Requirements

- Store data for two entities Companies and Contacts. Every contact can be in multiple companies
  - Company
	- ID: auto number
    - Name: unique text
    - Number of Employees: integer
  - Contact
    - ID: auto number
	- Name: unique text
	- Company: The related company (allow multiple)
	
- Users can extend the company and contact entities to add as many fields as they want to customize the tables from types (Text, Number, and Date). For example, Birthdate for Contact

- Provide APIs for
  - Add a new column to the tables company or contact
  - Read, Create,Update, Delete, Company Contact. Note, the extended fields should be updated, read, etc. 
  - Filters on any existing field or user extended field
  - The apis should be working with millions of records without any performance issues.
 
## Technical notes

- Use MongoDB to store row data (contacts and companies)
- Use asp.net core for the APIs
- Feel free to use any other technologies you think needed
- Host it on GitHub, and share it with us!

## License
[MIT](LICENSE)
