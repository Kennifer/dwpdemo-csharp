# DWP Demo Api

## Intro
* I emailed the 'Contact point for applicants' on the job application looking for some clarity on the job role and the languages expected, however the email bounced back with the address not found. I have no problem using Java, but do not want to dedicate all my time to it, I prefer using C#.  I notice that you do have positions for c#, but it wasn't explicitly mentioned in the application, only Java was.  If this position is 100% Java, it probably isn't the job for me.
* I've tried to structure this repository so you can see my process of working, each commit on master was a squash merge of a branch, each branch has commits at each step so you can see the progression of code.

## Assumptions
* Strict TDD hasn't been adhered to, but red, green, refactor has mostly been used where applicable.
* Failures from source API (https://bpdts-test-app.herokuapp.com/) will be handled with empty lists.
* Even though the scope of the application asks for users of London, and the users within the radius of London, I've assumed that both cities won't always be the same location, and therefore list users by city won't always be a subset of list by location with radius. 
* It's fine to have duplicates removed in the controller via Distinct.  Due to the use of record compares the object values, it is therefore assumed that /users and /city/{city}/users will always return identical object models.
* Users 50 miles outside of London are based on radial distance around a centre point, not the bounding regions of any London region (i.e. city, greater, m25)

## Not Implemented
* CorrelationIds would normally be captured or generated on incoming requests, and appended to external requests.  This would normally be done in the form of a http client decorator.
* Monitoring of service e.g. request/response times.  Plenty of approaches to this, given more time I'd probably look into something like prometheus
* Actual logging hasn't been implemented but a proxy class for logging has been used, where direct logging would occur. Usually logs would also be taken in middleware during requests.
* A rudimentary implementation of the Great-circle distance has been implemented, due to libraries being thin on the ground, no time has been spent checking the validity of this. There are many databases which could manage spatial queries better than I've done, however for the scope of the test it would be a little overkill.
 
## In Hindsight/Not Done
* A struct should have been used for Lat/Long
* A further integration test could be written, where instead of using the source API, a mock API is supplied (via configuration) instead.  This would allow finer control, and testing of the data supplied from the source API and the final presented result.
* A better flow could have been developed for aggregating and filtering users, however due to the simplicity of requests, they still fall into the 'Write everything twice, just not a third time' philosophy
