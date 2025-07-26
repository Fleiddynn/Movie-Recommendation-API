# WebApplication1

Movie Controller:

GET /api/Movies:
Returns all the movies in the database with the response 200 OK if succeed.
[
{
"id": "e1cd2f22-b0e5-4350-98c2-734afdbc642d",
"title": "jıjıjıj",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-24"
},
{
"id": "68f94195-38f4-4d55-9580-93e9e568549d",
"title": "string",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-25"
}
]

POST /api/Movies:
Creates a new movie from the information given in the request body.
{
"title": "string",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-25"
}
Returns created movie:
{
"id": "655850e0-0cc1-4503-aafe-a67cba89908d",
"movieCategories": [],
"userReviews": [],
"title": "string",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-26",
"createdAt": "2025-07-26T12:05:30.2642633Z",
"updatedAt": "2025-07-26T12:05:30.2642634Z"
}

GET /api/Movies/{id}:
Finds and returns the movie in the database from the id given in the request parameter with the response 200 OK if succeed.
{
"id": "68f94195-38f4-4d55-9580-93e9e568549d",
"title": "string",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-25"
}

PUT /api/Movies/{id}:
Finds and updates the movie in the database from the id given in the request parameter with the response 200 OK if succeed.
{
"title": "string",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-25"
}

PATCH /api/Movies/{id}:
Finds and partially updates the movie in the database from the id given in the request parameter with the response 200 OK if succeed.
[
{
"operationType": 0,
"path": "string",
"op": "string",
"from": "string",
"value": "string"
}
]

DELETE /api/Movies/{id}
Deletes the movie in the database from the id given in the request parameter with the response 200 OK if succeed.

GET /api/Movies/category/{categoryId}
Returns all the movies in the database that contains the category id given in the request parameter. Response 200 OK if succeed.
[
{
"id": "68f94195-38f4-4d55-9580-93e9e568549d",
"title": "string",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-25"
},
{
"id": "5967a8ef-9fe0-4501-859f-3a4d7135c4ce",
"title": "string",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-26"
}
]

POST /api/Movies/{movieId}/addcategory/{categoryId}
Adds categoryId to the existing movie if the movie is in database. Response 200 OK if succeed.

GET /api/Movies/category/{categoryId}/page/{pageNumber}/size/{pageSize}
Returns the movies that containst the categoryId. Can be filtered with pageNumber and pageSize attributes given in request parameters. Response 200 OK if succeed.

"/api/Movies/category/3fa85f64-5717-4562-b3fc-2c963f66afa6/page/1/size/1":
[
{
"id": "68f94195-38f4-4d55-9580-93e9e568549d",
"title": "string",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-25"
}
]

"/api/Movies/category/3fa85f64-5717-4562-b3fc-2c963f66afa6/page/2/size/1":
[
{
"id": "5967a8ef-9fe0-4501-859f-3a4d7135c4ce",
"title": "string",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-26"
}
]

GET /api/Movies/{id}/details
Returns the detailed version of the movie that contains user reviews if the movie with the given id is found. Response 200 OK if succeed.
{
"id": "68f94195-38f4-4d55-9580-93e9e568549d",
"title": "string",
"description": "string",
"duration": 0,
"director": "string",
"categories": [
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
],
"imdb": 0,
"length": 0,
"releaseDate": "2025-07-25",
"userReviews": []
}

Category Controller:

GET /api/Categories
Returns all the categories in the database. Response 200 OK if succeed.
[
{
"id": "dc41a6bc-f14e-4bb6-8d81-bee15d2cf20b",
"name": "Adventure"
},
{
"id": "d0514c76-6332-4957-88b6-81f66da73dd2",
"name": "Sci-fi"
}
]

POST /api/Categories
Creates a new category from the information given in the request body. Response 200 OK if succeed.
Request Body:
{
"name": "Comedy"
}

GET /api/Categories/{id}
Returns the category with the matching id given in the request parameter. Response 200 OK if succeed.
"/api/Categories/ad1d2864-ef7d-4c5c-8e1e-f115e6a61950":
Response Body:
{
"id": "ad1d2864-ef7d-4c5c-8e1e-f115e6a61950",
"name": "Comedy"
}

PUT /api/Categories/{id}
Updates the category with the matching id given in the request parameter. Response 200 OK if succeed.
"/api/Categories/ad1d2864-ef7d-4c5c-8e1e-f115e6a61950"
Request Body:
{
"name": "Drama"
}

DELETE /api/Categories/{id}
Deletes the category with the matching id given in the request parameter. Response 200 OK if succeed.
"/api/Categories/dc41a6bc-f14e-4bb6-8d81-bee15d2cf20b":

User Controller

GET /api/User
Returns all the users in the database. Response 200 OK if succeed.

GET /api/User/{id}

PUT /api/User/{id}

PATCH /api/User/{id}

DELETE /api/User/{id}

POST /api/User/login

GET /api/User/login/google

GET /login/google-response

GET /api/User/login/facebook-login

GET /api/User/facebook-response

POST /api/User/register

GET /api/User/watchlist/{id}

POST /api/User/watchlist/{id}

DELETE /api/User/watchlist/{id}
