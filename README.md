# Campaign Budget Calculator

This project contains a Campaign Budget Calculator application with a React frontend and a .NET backend API.

## Prerequisites

- Docker
- Docker Compose

## Running the Containers

Follow these steps to run the application using Docker:

1. Clone the repository:
   ```
   git clone https://github.com/tobbyy3/CampaignMediaPlanning
   cd CampaignPlanner
   ```

2. Build and start the containers:
   ```
   docker-compose up --build
   ```
    Let the build run and finish.

3. Access the application:
   - Frontend: Open your browser and navigate to `http://localhost:3000`
   - Backend API: Available at `http://localhost:5049`

4. To stop the containers:
   ```
   docker-compose down
   ```

## Development

For development purposes, you can run the frontend and backend separately:

### Frontend

1. Navigate to the frontend directory:
   ```
   cd campaign-budget-frontend
   ```

2. Install dependencies:
   ```
   npm install
   ```

3. Start the development server:
   ```
   npm start
   ```

The frontend will be available at `http://localhost:3000`.

### Backend

1. Navigate to the backend directory:
   ```
   cd CampaignBudgetAPI/CampaignBudgetAPI
   ```

2. Run the API:
   ```
   dotnet run
   ```

The Swagger UI will be available at `http://localhost:5049`.
The Swagger UI is also available if you run the backend docker container. 

## API Documentation

The backend API uses Swagger for documentation and testing. When running the backend, you can access the Swagger UI at `http://localhost:5049` to explore and test the available endpoints.


## Troubleshooting

- If you encounter any issues with the containers, check the logs:
  ```
  docker-compose logs
  ```

## Additional Information

For more details on the project structure, API endpoints, and frontend components, please refer to the respective source code files in the frontend and backend directories.
