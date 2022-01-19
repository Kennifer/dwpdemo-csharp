# DWP Demo Api
 
## Assumptions
* Strict TDD hasn't been adhered to, but red, green, refactor has been used.
* Failures from source API (https://bpdts-test-app.herokuapp.com/) will be handled with empty lists.
* Even though the scope of the application asks for users of London, and the users within the radius of London, I've assumed that both cities won't always be the same location, and therefore list users by city won't always be a subset of list by location with radius. 
* It's fine to have duplicates removed in the controller via Distinct.  Due to the use of record compares the object values, it is therefore assumed that /users and /city/{city}/users will always return identical object models.
* Users 50miles outside of London are based on radial distance around a centre point, not the bounding regions of any London region (i.e. city, greater, m25)
## Not Implemented
* CorrelationIds would normally be captured or generated on incoming requests, and appended to external requests.  This would normally be done in the form of a http client decorator.
* Monitoring of service e.g. request/response times.  Plenty of approaches to this, given more time I'd probably look into something like prometheus
* Actual logging hasn't been implemented but a proxy class for logging has been used, where direct logging would occur. Usually logs would also be taken in middleware during requests.
* A rudimentary implementation of the Great-circle distance has been implemented, due to libraries being thin on the ground, no time has been spent checking the validity of this
 
## In Hindsight/Not Done
* A struct should have been used for Lat/Long
* A futher integration test could be written, where instead of using the source API, a mock API is supplied (via configuration) instead.  This would allow finer control, and testing of the data supplied from the source API and the final presented result.