# Movie Reccommendation API Documentation

This is a backend project that uses aspnetcore and postgresql. It includes movie, category, review and user endpoints, its repositories and DTOs.

---

## Entities

This section lists the entities used in project.

### Category

- `Guid Id`
- `ICollection<MovieCategory> MovieCategories`
- `string Name`
- `DateTime CreatedAt`
- `DateTime UpdatedAt`

### Movie

- `Guid Id`
- `ICollection<MovieCategory> MovieCategories`
- `ICollection<Review> UserReviews`
- `string Title`
- `string? Description`
- `int Duration`
- `string Director`
- `List<Guid> Categories`
- `double IMDB`
- `int Length`
- `DateOnly? ReleaseDate`
- `DateTime CreatedAt`
- `DateTime UpdatedAt`

### MovieWCategory

- `Guid MovieId`
- `Movie Movie`
- `Guid CategoryId`
- `Category Category`

### Review

- `Guid Id`
- `Guid UserId`
- `User User`
- `Guid MovieId`
- `Movie Movie`
- `string Note`
- `double Rating`
- `DateTime CreatedAt`
- `DateTime UpdatedAt`

### User

- `Guid Id`
- `string first_name`
- `string last_name`
- `string email`
- `string password`
- `string? social_login_provider`
- `ICollection<Review> UserReviews`
- `List<Guid>? watchedMovies`
- `DateTime created_at`
- `DateTime updated_at`

---

## 2. Repositories

This section explains the repository used for each entity.

### Review Repository

- `GetReviewsAsync()`: Returns all the reviews as a review list.
- `GetReviewByIdAsync(Guid id)`: Finds and returns the review using the given ID.
- `GetReviewByMovieIdAsync(Guid movieId)`: Finds and returns all reviews for a given movie ID.
- `GetReviewsByUserIdAsync(Guid userId)`: Finds and returns all reviews by a given user ID.
- `GetReviewsByUserAndMovieIdAsync(Guid userId, Guid movieId)`: Finds and returns the review by user and movie IDs.
- `Create(Review review)`: Creates a review.
- `Update(Review review)`: Updates a review.
- `Delete(Guid id)`: Deletes a review.

### Movie Repository

- `GetMoviesAsync()`: Returns all movies as a movie list.
- `GetMovieByIdAsync(Guid id)`: Finds and returns the movie using the given ID.
- `GetMoviesByCategoryAsync(Guid categoryId)`: Finds and returns movies by the given category ID.
- `Create(Movie movie)`: Creates a movie.
- `Update(Movie movie)`: Updates a movie.
- `Delete(Guid id)`: Deletes a movie.
- `GetMoviesByCategoryAsync(Guid categoryId, int pageNumber, int pageSize)`: Finds and returns movies by category, page number, and page size.
- `GetMoviesAsync(string? sortBy, string? sortOrder)`: Finds and returns the sorted movie list using `sortBy` and `sortOrder` values.
- `GetReviewsByMovieAsync(Guid id)`: Finds and returns reviews for a given movie ID.

### User Repository

- `GetUsersAsync()`: Returns all users as a user list.
- `GetUserByIdAsync(Guid id)`: Finds and returns the user using the given ID.
- `Create(User user)`: Creates a user.
- `Update(User user)`: Updates a user.
- `Delete(Guid id)`: Deletes a user.

---

## 3. API Endpoints / Controllers

This section describes all API endpoints and its use cases.

### Movie Controller

- **`GET /api/Movies`**

  - **Description:** Returns all movies in the database. Responds with `200 OK` on success.
  - **Example Response Body:**
    ```json
    [
      {
        "id": "e1cd2f22-b0e5-4350-98c2-734afdbc642d",
        "title": "jıjıjıj",
        "description": "string",
        "duration": 0,
        "director": "string",
        "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
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
        "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
        "imdb": 0,
        "length": 0,
        "releaseDate": "2025-07-25"
      }
    ]
    ```

- **`POST /api/Movies`**

  - **Description:** Creates a new movie from the information given in the request body.
  - **Example Request Body:**
    ```json
    {
      "title": "string",
      "description": "string",
      "duration": 0,
      "director": "string",
      "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
      "imdb": 0,
      "length": 0,
      "releaseDate": "2025-07-25"
    }
    ```
  - **Example Response Body:**
    ```json
    {
      "id": "655850e0-0cc1-4503-aafe-a67cba89908d",
      "movieCategories": [],
      "userReviews": [],
      "title": "string",
      "description": "string",
      "duration": 0,
      "director": "string",
      "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
      "imdb": 0,
      "length": 0,
      "releaseDate": "2025-07-26",
      "createdAt": "2025-07-26T12:05:30.2642633Z",
      "updatedAt": "2025-07-26T12:05:30.2642634Z"
    }
    ```

- **`GET /api/Movies/{id}`**

  - **Description:** Finds and returns the movie with the given ID. Responds with `200 OK` on success.
  - **Example Response Body:**
    ```json
    {
      "id": "68f94195-38f4-4d55-9580-93e9e568549d",
      "title": "string",
      "description": "string",
      "duration": 0,
      "director": "string",
      "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
      "imdb": 0,
      "length": 0,
      "releaseDate": "2025-07-25"
    }
    ```

- **`PUT /api/Movies/{id}`**

  - **Description:** Finds and updates the movie with the given ID. Responds with `200 OK` on success.
  - **Example Request Body:**
    ```json
    {
      "title": "string",
      "description": "string",
      "duration": 0,
      "director": "string",
      "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
      "imdb": 0,
      "length": 0,
      "releaseDate": "2025-07-25"
    }
    ```

- **`PATCH /api/Movies/{id}`**

  - **Description:** Finds and partially updates the movie with the given ID. Responds with `200 OK` on success.
  - **Example Request Body:**
    ```json
    [
      {
        "operationType": 0,
        "path": "string",
        "op": "string",
        "from": "string",
        "value": "string"
      }
    ]
    ```

- **`DELETE /api/Movies/{id}`**

  - **Description:** Deletes the movie with the given ID. Responds with `200 OK` on success.

- **`GET /api/Movies/category/{categoryId}`**

  - **Description:** Returns all movies containing the specified category ID. Responds with `200 OK` on success.
  - **Example Response Body:**
    ```json
    [
      {
        "id": "68f94195-38f4-4d55-9580-93e9e568549d",
        "title": "string",
        "description": "string",
        "duration": 0,
        "director": "string",
        "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
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
        "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
        "imdb": 0,
        "length": 0,
        "releaseDate": "2025-07-26"
      }
    ]
    ```

- **`POST /api/Movies/{movieId}/addcategory/{categoryId}`**

  - **Description:** Adds a category to an existing movie. Responds with `200 OK` on success.

- **`GET /api/Movies/category/{categoryId}/page/{pageNumber}/size/{pageSize}`**

  - **Description:** Returns movies filtered by category and paginated by page number and page size. Responds with `200 OK` on success.
  - **Example Request URL:** `/api/Movies/category/3fa85f64-5717-4562-b3fc-2c963f66afa6/page/1/size/1`
  - **Example Response Body (Page 1):**
    ```json
    [
      {
        "id": "68f94195-38f4-4d55-9580-93e9e568549d",
        "title": "string",
        "description": "string",
        "duration": 0,
        "director": "string",
        "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
        "imdb": 0,
        "length": 0,
        "releaseDate": "2025-07-25"
      }
    ]
    ```
  - **Example Request URL:** `/api/Movies/category/3fa85f64-5717-4562-b3fc-2c963f66afa6/page/2/size/1`
  - **Example Response Body (Page 2):**
    ```json
    [
      {
        "id": "5967a8ef-9fe0-4501-859f-3a4d7135c4ce",
        "title": "string",
        "description": "string",
        "duration": 0,
        "director": "string",
        "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
        "imdb": 0,
        "length": 0,
        "releaseDate": "2025-07-26"
      }
    ]
    ```

- **`GET /api/Movies/{id}/details`**
  - **Description:** Returns the detailed version of a movie, including user reviews. Responds with `200 OK` on success.
  - **Example Response Body:**
    ```json
    {
      "id": "68f94195-38f4-4d55-9580-93e9e568549d",
      "title": "string",
      "description": "string",
      "duration": 0,
      "director": "string",
      "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
      "imdb": 0,
      "length": 0,
      "releaseDate": "2025-07-25",
      "userReviews": []
    }
    ```

---

### Category Controller

- **`GET /api/Categories`**

  - **Description:** Returns all categories in the database. Responds with `200 OK` on success.
  - **Example Response Body:**
    ```json
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
    ```

- **`POST /api/Categories`**

  - **Description:** Creates a new category from the information in the request body. Responds with `200 OK` on success.
  - **Example Request Body:**
    ```json
    {
      "name": "Comedy"
    }
    ```

- **`GET /api/Categories/{id}`**

  - **Description:** Returns the category with the matching ID. Responds with `200 OK` on success.
  - **Example Request URL:** `/api/Categories/ad1d2864-ef7d-4c5c-8e1e-f115e6a61950`
  - **Example Response Body:**
    ```json
    {
      "id": "ad1d2864-ef7d-4c5c-8e1e-f115e6a61950",
      "name": "Comedy"
    }
    ```

- **`PUT /api/Categories/{id}`**

  - **Description:** Updates the category with the matching ID. Responds with `200 OK` on success.
  - **Example Request URL:** `/api/Categories/ad1d2864-ef7d-4c5c-8e1e-f115e6a61950`
  - **Example Request Body:**
    ```json
    {
      "name": "Drama"
    }
    ```

- **`DELETE /api/Categories/{id}`**
  - **Description:** Deletes the category with the matching ID. Responds with `200 OK` on success.
  - **Example Request URL:** `/api/Categories/dc41a6bc-f14e-4bb6-8d81-bee15d2cf20b`

---

### User Controller

- **`GET /api/User`**

  - **Description:** Returns all users in the database. Responds with `200 OK` on success.
  - **Example Response Body:**
    ```json
    [
      {
        "id": "693d080f-72cf-44ab-b785-db3516bf3b43",
        "email": "string",
        "first_name": "string",
        "last_name": "string"
      }
    ]
    ```

- **`GET /api/User/{id}`**

  - **Description:** Finds and returns the user with the given ID. Responds with `200 OK` on success.
  - **Example Request URL:** `/api/User/693d080f-72cf-44ab-b785-db3516bf3b43`
  - **Example Response Body:**
    ```json
    {
      "id": "693d080f-72cf-44ab-b785-db3516bf3b43",
      "email": "string",
      "first_name": "string",
      "last_name": "string"
    }
    ```

- **`PUT /api/User/{id}`**

  - **Description:** Finds and updates the user with the given ID. Responds with `200 OK` on success.
  - **Example Request Body:**
    ```json
    {
      "first_name": "name",
      "last_name": "surname",
      "email": "mail",
      "password": "password",
      "social_login_provider": "none"
    }
    ```

- **`PATCH /api/User/{id}`**

  - **Description:** Finds and partially updates the user with the given ID. Responds with `200 OK` on success.
  - **Example Request Body:**
    ```json
    {
      "first_name": "string",
      "last_name": "string",
      "email": "string",
      "password": "string",
      "social_login_provider": "string"
    }
    ```

- **`DELETE /api/User/{id}`**

  - **Description:** Finds and deletes the user with the given ID. Responds with `200 OK` on success.

- **`POST /api/User/login`**

  - **Description:** Returns a token and user details if the provided credentials are correct.
  - **Example Request Body:**
    ```json
    {
      "email": "string",
      "password": "string"
    }
    ```
  - **Example Response Body:**
    ```json
    {
      "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjhhODRiNjQ4LThmYTgtNDdmMy1iNzU0LWU2MTE3ZjY3MjRhNCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZW5lbWUiLCJmaXJzdF9uYW1lIjoic3RyaW5nIiwibGFzdF9uYW1lIjoic3RyaW5nIiwiZXhwIjoxNzUzNTQ3NDE0LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTgwLyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcxODAvIn0.Fd8tPsYUV3uoZf4yddvqlKwJD3E4xyJHaoVzYJqnFt0",
      "user": {
        "id": "8a84b648-8fa8-47f3-b754-e6117f6724a4",
        "email": "string",
        "first_name": "string",
        "last_name": "string"
      }
    }
    ```

- **`GET /api/User/login/google`**

  - **Description:** Redirects to the Google login page.

- **`GET /login/google-response`**

  - **Description:** Redirects here after a successful Google login and returns the token and user details.
  - **Example Response Body:**
    ```json
    {
      "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjhhODRiNjQ4LThmYTgtNDdmMy1iNzU0LWU2MTE3ZjY3MjRhNCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZW5lbWUiLCJmaXJzdF9uYW1lIjoic3RyaW5nIiwibGFzdF9uYW1lIjoic3RyaW5nIiwiZXhwIjoxNzUzNTQ3NDE0LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTgwLyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcxODAvIn0.Fd8tPsYUV3uoZf4yddvqlKwJD3E4xyJHaoVzYJqnFt0",
      "user": {
        "id": "8a84b648-8fa8-47f3-b754-e6117f6724a4",
        "email": "string",
        "first_name": "string",
        "last_name": "string"
      }
    }
    ```

- **`GET /api/User/login/facebook-login`**

  - **Description:** Redirects to the Facebook login page.

- **`GET /api/User/facebook-response`**

  - **Description:** Redirects here after a successful Facebook login and returns the token and user details.
  - **Example Response Body:**
    ```json
    {
      "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjhhODRiNjQ4LThmYTgtNDdmMy1iNzU0LWU2MTE3ZjY3MjRhNCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZW5lbWUiLCJmaXJzdF9uYW1lIjoic3RyaW5nIiwibGFzdF9uYW1lIjoic3RyaW5nIiwiZXhwIjoxNzUzNTQ3NDE0LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTgwLyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcxODAvIn0.Fd8tPsYUV3uoZf4yddvqlKwJD3E4xyJHaoVzYJqnFt0",
      "user": {
        "id": "8a84b648-8fa8-47f3-b754-e6117f6724a4",
        "email": "string",
        "first_name": "string",
        "last_name": "string"
      }
    }
    ```

- **`POST /api/User/register`**

  - **Description:** Creates a new user using the information in the request body.
  - **Example Request Body:**
    ```json
    {
      "first_name": "string",
      "last_name": "string",
      "email": "string",
      "password": "string",
      "social_login_provider": "string"
    }
    ```
  - **Example Response Body:**
    ```json
    {
      "userReviews": [],
      "first_name": "string",
      "last_name": "string",
      "email": "dfsdfsdf",
      "password": "$2a$11$zgxlVgIqVwe0tl.8Ob.0f.U8X3o5HZ3p/lAEFcYWernCwtpHz1uv6",
      "social_login_provider": "",
      "watchedMovies": [],
      "created_at": "2025-07-26T14:36:40.0838154Z",
      "updated_at": "2025-07-26T14:36:40.0838824Z",
      "id": "4ad87198-6392-4e3a-97be-a5f56cdf9826",
      "userName": null,
      "normalizedUserName": null,
      "normalizedEmail": null,
      "emailConfirmed": false,
      "passwordHash": null,
      "securityStamp": null,
      "concurrencyStamp": "7941eba3-3e9a-4b25-965b-ec58b20b896d",
      "phoneNumber": null,
      "phoneNumberConfirmed": false,
      "twoFactorEnabled": false,
      "lockoutEnd": null,
      "lockoutEnabled": false,
      "accessFailedCount": 0
    }
    ```

- **`GET /api/User/watchlist/{id}`**

  - **Description:** Returns all movies in the user's watchlist.
  - **Example Request URL:** `/api/User/watchlist/693d080f-72cf-44ab-b785-db3516bf3b43`
  - **Example Response Body:**
    ```json
    [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "description": "string",
        "duration": 0,
        "director": "string",
        "categories": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
        "imdb": 0,
        "length": 0,
        "releaseDate": "2025-07-26"
      }
    ]
    ```

- **`POST /api/User/watchlist/{id}`**

  - **Description:** Adds a movie ID to the user's watchlist. Responds with `200 OK` on success.
  - **Example Request URL:** `/api/User/watchlist/693d080f-72cf-44ab-b785-db3516bf3b43`
  - **Example Request Body:** `"3fa85f64-5717-4562-b3fc-2c963f66afa6"`

- **`DELETE /api/User/watchlist/{id}`**
  - **Description:** Removes a movie ID from the user's watchlist. Responds with `200 OK` on success.
  - **Example Request URL:** `/api/User/watchlist/693d080f-72cf-44ab-b785-db3516bf3b43`
  - **Example Request Body:** `"b5dfb0b8-ab47-4168-a6d8-ff6b873ca4c8"`

---

### Review Controller

- **`GET /api/Review`**

  - **Description:** Returns all reviews in the database.
  - **Example Response Body:**
    ```json
    [
      {
        "id": "77685ba5-978f-4235-a87e-4201247bc97f",
        "userId": "8a84b648-8fa8-47f3-b754-e6117f6724a4",
        "userName": "",
        "movieId": "b5dfb0b8-ab47-4168-a6d8-ff6b873ca4c8",
        "movieName": "",
        "note": "Beğendim",
        "rating": 6,
        "createdAt": "2025-07-26T14:54:45.558055Z"
      }
    ]
    ```

- **`POST /api/Review`**

  - **Description:** Creates a new review from the information in the request body.
  - **Example Request Body:**
    ```json
    {
      "userId": "8a84b648-8fa8-47f3-b754-e6117f6724a4",
      "movieId": "b5dfb0b8-ab47-4168-a6d8-ff6b873ca4c8",
      "note": "Beğendim",
      "rating": 6.0
    }
    ```
  - **Example Response Body:**
    ```json
    {
      "id": "77685ba5-978f-4235-a87e-4201247bc97f",
      "userId": "8a84b648-8fa8-47f3-b754-e6117f6724a4",
      "userName": "",
      "movieId": "b5dfb0b8-ab47-4168-a6d8-ff6b873ca4c8",
      "movieName": "string",
      "note": "Beğendim",
      "rating": 6,
      "createdAt": "2025-07-26T14:54:45.5580556Z"
    }
    ```

- **`GET /api/Review/{movieId}`**

  - **Description:** Returns all reviews for a specific movie, given the `movieId`.
  - **Example Request URL:** `/api/Review/b5dfb0b8-ab47-4168-a6d8-ff6b873ca4c8`
  - **Example Response Body:**
    ```json
    [
      {
        "id": "77685ba5-978f-4235-a87e-4201247bc97f",
        "userId": "8a84b648-8fa8-47f3-b754-e6117f6724a4",
        "userName": "",
        "movieId": "b5dfb0b8-ab47-4168-a6d8-ff6b873ca4c8",
        "movieName": "",
        "note": "Beğendim",
        "rating": 6,
        "createdAt": "2025-07-26T14:54:45.558055Z"
      }
    ]
    ```

- **`PUT /api/Review/{id}`**

  - **Description:** Finds and updates the review with the given ID. Responds with `200 OK` on success.
  - **Example Request URL:** `/api/Review/77685ba5-978f-4235-a87e-4201247bc97f`
  - **Example Request Body:**
    ```json
    {
      "userId": "8a84b648-8fa8-47f3-b754-e6117f6724a4",
      "movieId": "b5dfb0b8-ab47-4168-a6d8-ff6b873ca4c8",
      "note": "Didn't like it.",
      "rating": 2
    }
    ```

- **`DELETE /api/Review/{id}`**

  - **Description:** Finds and deletes the review with the given ID. Responds with `200 OK` on success.
  - **Example Request URL:** `DELETE /api/Review/user/8a84b648-8fa8-47f3-b754-e6117f6724a4`

- **`GET /api/Review/user/{userId}`**

  - **Description:** Returns all reviews made by the user with the given `userId`.
  - **Example Request URL:** `/api/Review/user/8a84b648-8fa8-47f3-b754-e6117f6724a4`
  - **Example Response Body:**
    ```json
    [
      {
        "id": "77685ba5-978f-4235-a87e-4201247bc97f",
        "userId": "8a84b648-8fa8-47f3-b754-e6117f6724a4",
        "userName": "",
        "movieId": "b5dfb0b8-ab47-4168-a6d8-ff6b873ca4c8",
        "movieName": "",
        "note": "Didn't like it.",
        "rating": 2,
        "createdAt": "2025-07-26T14:54:45.558055Z"
      }
    ]
    ```

- **`GET /api/Review/movie/{movieId}/user/{userId}`**
  - **Description:** Returns a specific review using the provided `movieId` and `userId`.
  - **Example Request URL:** `/api/Review/movie/b5dfb0b8-ab47-4168-a6d8-ff6b873ca4c8/user/8a84b648-8fa8-47f3-b754-e6117f6724a4`
  - **Example Response Body:**
    ```json
    {
      "id": "77685ba5-978f-4235-a87e-4201247bc97f",
      "userId": "8a84b648-8fa8-47f3-b754-e6117f6724a4",
      "userName": "",
      "movieId": "b5dfb0b8-ab47-4168-a6d8-ff6b873ca4c8",
      "movieName": "",
      "note": "Didn't like it.",
      "rating": 2,
      "createdAt": "2025-07-26T14:54:45.558055Z"
    }
    ```
