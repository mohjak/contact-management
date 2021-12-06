# Contact Management

## Brief

Web API application to manage Extendable Customers data (simple contact management).

## Requirements

- Store data for two entities Companies and Contacts. Every contact can be in multiple listings
  - Listing
  - ID: auto number
    - Name: unique text
    - Number of Employees: integer
  - Contact
    - ID: auto number
  - Name: unique text
  - Listing: The related listing (allow multiple)
- Users can extend the listing and contact entities to add as many fields as they want to customize the tables from types (Text, Number, and Date). For example, Birthdate for Contact

- Provide APIs for
  - Add a new column to the tables listing or contact
  - Read, Create,Update, Delete, Listing Contact. Note, the extended fields should be updated, read, etc.
  - Filters on any existing field or user extended field
  - The apis should be working with millions of records without any performance issues.

## Technical notes

- Use MongoDB to store row data (contacts and listings)
- Use asp.net core for the APIs
- Feel free to use any other technologies you think needed
- Host it on GitHub, and share it with us!

## Sample Requests

### Create Listing

```json
{
  "fields": [
    {
      "dataType": 0,
      "name": "Name",
      "value": "Amazon",
      "required": true
    },
    {
      "dataType": 1,
      "name": "numberOfEmployees",
      "value": "1000",
      "required": true
    },
    {
      "dataType": 0,
      "name": "Country",
      "value": "USA"
    },
    {
      "dataType": 2,
      "name": "EstablishDate",
      "value": "1999-11-14T18:35:55"
    },
    {
      "dataType": 1,
      "name": "NumberOfBranches",
      "value": "7777"
    }
  ]
}
```

### Create Related Listing

```json
{
  "listings": ["61adb0227c3cecff3a134302"],
  "fields": [
    {
      "dataType": 0,
      "name": "Firstname",
      "value": "Amer"
    },
    {
      "dataType": 2,
      "name": "Birthdate",
      "value": "1975-11-14T17:01:55"
    },
    {
      "dataType": 1,
      "name": "FavoriteNumber",
      "value": "7"
    }
  ]
}
```

### Update Listing

listingId "61adb0227c3cecff3a134302"

```json
{
  "fields": [
    {      
      "id": "61adb0237c3cecff3a134304",
      "value": "20000"
    }
  ]
}
```

## Fitler Existing Or Extended Fields

```json
{
    "fields": [
        {
            "listingId": "61adb0227c3cecff3a134302",
            "id": "61adb0237c3cecff3a134305",
            "value": "USA"
        }
    ]
}
```

## Field

### Create Required Field In A Listing

```json
{
  "listingId": "61ab0a978361dcfd29a562fd",
  "name": "Description",
  "dataType": 0,
  "value": "Lorem ipsum dolor sit amet consectetur adipisicing elit. Voluptas porro, ad quos culpa deleniti, quis eaque itaque ea perferendis sint nisi quaerat, odit et dicta in ipsum fuga vitae consequatur."
  "required": true
}
```

# Releases

## 0.2

- Manage Listings.
- Manage Fields.
- Filter Fields.

## License

[MIT](LICENSE)
