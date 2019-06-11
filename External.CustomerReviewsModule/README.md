# Customer Reviews Module RESTfull
From a functional point of view, the module is similar to the CustomerReviews module. The main purpose what for this module is created is to show the RESTfull approach best practices implementation. In the CustomerReviews module implemented following API:

* <b>POST</b> api/customerReviews/search - Return product Customer review search results
* <b>POST</b> api/customerReviews - Create new or update existing customer review
* <b>DELETE</b> api/customerReviews - Customer Reviews by IDs

New module implement APIs in this way:
* <b>POST</b> api/customerReviews/search Return product Customer review search results
* <b>POST</b> api/customerReviews Create new review
* <b>GET</b> api/customerReviews/{id} Get Customer Review by id
* <b>PUT</b> api/customerReviews/{id} Update existing customer review
* <b>DELETE</b> api/customerReviews/{id} Delete Customer Reviews by Id

The new module may be used as an example of the correct implementation of the API.

#### Why we should use RESTful API?
RESTfull approach is no fixed API above and beyond what REST itself defines. The client needs only know the initial URI, and subsequently chooses from server-supplied choices to navigate or perform actions. It allows for rapid evolution of servers and allows a number of applications to interact freely on an ad hoc basis (e.g. the whole Internet). A REST is a valid design choice when evolution and scalability is more important.

#### Read more about RESTfull best practice
* https://code-maze.com/top-rest-api-best-practices/
* https://github.com/WhiteHouse/api-standards
