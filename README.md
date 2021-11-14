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

## Sample Requests

### Create Company

```json
{
  "name": "Amazon",
  "numberOfEmployees": 1000,
  "fields": [
    {
      "type": "Text",
      "name": "Country",
      "value": "USA"
    },
    {
      "type": "Date",
      "name": "EstablishDate",
      "value": "1999-11-14T18:35:55"
    },
    {
      "type": "Number",
      "name": "NumberOfBranches",
      "value": "7777"
    }
  ]
}
```

### Update Company

```json
{
  "fields": [
    {
      "type": "Text",
      "name": "Country",
      "value": "USA"
    }
  ],
  "numberOfEmployees": 25000
}
```


### Create Contact

```json
{
  "name": "amer",
  "companies": ["6190477a06ff0f957c050e26"],
  "fields": [
    {
      "type": "Text",
      "name": "Firstname",
      "value": "Amer"
    },
    {
      "type": "Date",
      "name": "Birthdate",
      "value": "1975-11-14T17:01:55"
    },
    {
      "type": "Number",
      "name": "FavoriteNumber",
      "value": "7"
    }
  ]
}
```

### Update Contact

```json
{
  "fields": [
    {
      "type": "Text",
      "name": "LastName",
      "value": "Abdullah"
    }
  ],
  "companies": [
    "6190477a06ff0f957c050e26"
  ]
}
```

## License
[MIT](LICENSE)
