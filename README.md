# RESTful API Coding Exercise

## Background
*"Acme Remote Flights is a helicopter flight operator which specialises in transporting passengers to remote cities.<br>*
*The flights all operate within the same time zone.  The same flights repeat every day of the year."*

## Requirements
*"Create a Rest API which facilitates the below functionality:<br>*
>*Check availability :<br> by giving start and end Date. No of pax "*

## Assumptions
- Each flight has a limited seating capacity<br>
- *'Availability'* represents the ability for **one flight** on **one day** to accomodate **all** passengers<br>
- *'Availability'* may represent a flight that is already booked, provided that there is adequate seating capacity
- Passengers will not be distributed across multiple flights (or days)<br>
- Start and End date provides the *range* in which to search for an available flight<br>

## Implementation Notes

### Keeping it simple
- Logging has not been implemented
- Authentication has not been used<br>
- Sorting and Paging concepts have not been used<br>
- Discoverability/HATEOAS has not been implemented (as per [Richardson Maturity Model - Level 3](https://martinfowler.com/articles/richardsonMaturityModel.html#level3))
- RESTful operation **has** been implemented as per [Richardson Maturity Model - Level 2](https://martinfowler.com/articles/richardsonMaturityModel.html#level2)
- As per the requirements, only a handful of GET operations have been implemented to satisfy trial of the availability functionality
- Additional operations such as POST/PUT, PATCH and DELETE have not been implemented

### Technology
- Visual Studio 2017
- ASP.NET Core 2.0
- Entity Framework Core
- AutoMapper
- Moq
- xUnit

### Architecture

The solution has been split into a handful of related projects to better support readability, maintainability and testability.<br>
All classes make use of #region blocks (where it make sense) to assist readability and navigation.<br>
All classes in non-test projects have been documented.<br>

- **RESTfulFS.Api**<br>
Main project which implements RESTful interface and references subordinate projects
- **RESTfulFS.Api.Tests**<br>
xUnit test project - for RESTfulFS.Api project<br>
Will contain higher level tests to validate the RESTful interface<br>
- **RESTfulFS.Infrastructure**<br>
Class library - content that will be shared across multiple projects
- **RESTfulFS.Models**<br>
Class library - all view models and entity models
- **RESTfulFS.Services**<br>
Class library - all services and interfaces (to support dependency injection)
- **RESTfulFS.Services.Tests**<br>
xUnit test project - for RESTfulFS.Services project<br>
Will contain lower level tests to validate the RESTful interface


### Functionality
*When using Visual Studio 2017, all tests are available via the 'Test Explorer'.<br>
(Test -> Windows -> Test Explorer)*<br>

Out of the box;
>- the service has been configured to run at the following Url: **http://localhost:61854/**<br>
>- the service will start **without** launching a browser instance

It's recommended that you use something like [Postman](https://www.getpostman.com/) to try it out.<br>
This will allow you to easily view the returned JSON as well as the HTTP return codes.<br>
(A browser will still work ... it's just not as easy or pretty)<br>

The following 3 endpoints exist:

| HTTP | Route | Query String Options | Purpose
|:---------:|:----------|:----------|:----------
| GET | api/flights | n/a | Returns stored flights
| GET | api/bookings | n/a | Returns stored bookings
| GET | api/flights/availability | ?fromDate=yyyy-mm-dd&toDate=yyyy-mm-dd&passengers=1 | Returns 'availability' details


### Examples
#### Request (GET)
http://localhost:61854/api/flights<br>
#### Response (HTTP 200 - OK)
```json
    {
        "flightId": 1,
        "flightName": "ACME001",
        "seatingCapacity": 6
    },
    {
        "flightId": 2,
        "flightName": "ACME002",
        "seatingCapacity": 6
    },
    {
        "flightId": 3,
        "flightName": "ACME003",
        "seatingCapacity": 4
    },
    {
        "flightId": 4,
        "flightName": "ACME004",
        "seatingCapacity": 4
    }
```
#### Request (GET)
http://localhost:61854/api/bookings<br>
#### Response (HTTP 200 - OK)
```json
    {
        "bookingId": 1,
        "flightId": 1,
        "bookingDate": "2018-07-05T00:00:00Z",
        "passengers": 6
    },
    {
        "bookingId": 2,
        "flightId": 1,
        "bookingDate": "2018-07-06T00:00:00Z",
        "passengers": 4
    },
    {
        "bookingId": 3,
        "flightId": 3,
        "bookingDate": "2018-07-07T00:00:00Z",
        "passengers": 4
    },
    {
        "bookingId": 4,
        "flightId": 3,
        "bookingDate": "2018-07-08T00:00:00Z",
        "passengers": 2
    }
```
#### Request (GET)
http://localhost:61854/api/flights/availability?fromDate=2018-06-28&toDate=2018-06-28&passengers=6<br>
#### Response (HTTP 200 - OK)
```json
    {
        "flightId": 1,
        "bookingDate": "2018-06-28T00:00:00Z",
        "passengers": 6
    },
    {
        "flightId": 2,
        "bookingDate": "2018-06-28T00:00:00Z",
        "passengers": 6
    }
```

#### Request (GET)
http://localhost:61854/api/flights/availability
#### Response (HTTP 400 - Bad Request)
```json
{
    "message": "Invalid query parameters.",
    "detail": "The 'fromDate' cannot be null or less than the current date.",
    "stackTrace": null
}
```
