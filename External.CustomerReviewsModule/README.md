# Customer Reviews Module RESTful
Functionally, this module is similar to the original CustomerReviews module. The main goal when creating a module is to show the implementation of the best RESTful practices. CustomerReviews module implements the following API:

* <b>POST</b> api/customerReviews/search - returns the search result of Customer Reviews;
* <b>POST</b> api/customerReviews - creates a new or updates existing Customer Review;
* <b>DELETE</b> api/customerReviews - deletes Customer Reviews by Ids.

The new module implements the API as follows:
* <b>POST</b> api/customerReviews/search - returns the search result of Customer Reviews;
* <b>POST</b> api/customerReviews - creates a new Customer Review;
* <b>GET</b> api/customerReviews/{id} - returns Customer Review by Id;
* <b>PUT</b> api/customerReviews/{id} - updates an existing Customer Review;
* <b>DELETE</b> api/customerReviews/{id} - deletes Customer Review by Id.

The new module can be used as an example of the correct implementation of the API.

#### Why should we use RESTful API?
RESTful approach does not have strict implementation rules, it defines a set of constraints. The client only needs to know the initial URI, then chooses from the parameters provided by the server to navigate or perform actions. This approach enables rapid evolution of servers and allows a number of applications to interact freely on an ad hoc basis (e.g. the whole Internet). A REST is a valid design choice when evolution and scalability are important. For more information, https://en.wikipedia.org/wiki/Representational_state_transfer

#### Learn more about RESTful best practices:
* https://code-maze.com/top-rest-api-best-practices/
* https://github.com/WhiteHouse/api-standards
