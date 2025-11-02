
✅ ASP.NET Core 9
✅ PostgreSQL
✅ Docker Compose
✅ `.sln` file explanation
✅ How to run, test, and manage the project

A simple **To-Do list REST API** built with **ASP.NET Core 9** and **Entity Framework Core**, using **PostgreSQL** for data persistence.  
Fully containerized using **Docker Compose**, ready to run on any environment.


## 🚀 Tech Stack

| Layer | Technology |
|--------|-------------|
| Backend | ASP.NET Core 9 (C#) |
| ORM | Entity Framework Core 9 |
| Database | PostgreSQL 16 |
| Containerization | Docker + Docker Compose |
| Documentation | Swagger / OpenAPI |
| Build File | `.sln` (Solution) for Visual Studio / .NET CLI |

---

## 📁 Project Structure

```

dotnet-to-do/
├── TodoApi/
│   ├── Controllers/
│   │   └── TodoController.cs      # CRUD API endpoints
│   ├── Data/
│   │   └── TodoContext.cs         # EF Core DbContext
│   ├── Models/
│   │   └── TodoItem.cs            # Entity model
│   ├── Migrations/                # EF Core migrations (auto-generated)
│   ├── Program.cs                 # Application startup
│   ├── appsettings.json           # Configuration (Postgres connection)
│   └── TodoApi.csproj             # Project file
├── dotnet-to-do.sln               # Solution file (groups projects)
├── Dockerfile                     # Docker build file for the API
└── docker-compose.yml             # Compose file for API + PostgreSQL

````

---

## ⚙️ Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download) *(optional for local development)*
- `psql`, [pgAdmin](https://www.pgadmin.org/), or [DBeaver](https://dbeaver.io/) for DB inspection
- cURL or Postman for API testing

---

## 🐳 Run the Project with Docker

### 1️⃣ Build and Start Containers

```bash
docker compose up --build
````

This spins up:

* `todo-api` → ASP.NET Core app running at [http://localhost:8080](http://localhost:8080)
* `todo-postgres` → PostgreSQL database at `localhost:5432`

---

### 2️⃣ Check Running Containers

```bash
docker ps
```

Expected output:

```
todo-api         Up  ...  0.0.0.0:8080->8080/tcp
todo-postgres    Up  ...  0.0.0.0:5432->5432/tcp
```

---

### 3️⃣ Access the API

#### Swagger UI:

👉 [http://localhost:8080/swagger](http://localhost:8080/swagger)

#### Example cURL requests:

```bash
# Create a new ToDo
curl -X POST http://localhost:8080/api/todo \
  -H "Content-Type: application/json" \
  -d '{"title":"Buy groceries","isComplete":false}'

# List all todos
curl http://localhost:8080/api/todo

# Update a todo
curl -X PUT http://localhost:8080/api/todo/1 \
  -H "Content-Type: application/json" \
  -d '{"id":1,"title":"Buy groceries","isComplete":true}'

# Delete a todo
curl -X DELETE http://localhost:8080/api/todo/1
```

---

## 🧠 How It Works

1. The app connects to PostgreSQL via EF Core:

   ```csharp
   builder.Services.AddDbContext<TodoContext>(opt =>
       opt.UseNpgsql(connectionString));
   ```
2. On startup, EF Core automatically applies migrations (`db.Database.Migrate()`).
3. All CRUD endpoints are defined in `TodoController.cs`.
4. Data is stored in PostgreSQL and persisted via Docker volume `pgdata`.

---

## 🗄️ Database Access

To open a Postgres shell inside Docker:

```bash
docker exec -it todo-postgres psql -U postgres -d todo_db
```

Then check tables:

```sql
\dt
SELECT * FROM "Todos";
```

Or connect from your host:

```bash
psql -h localhost -U postgres -d todo_db
# Password: postgres
```

---

## 🧰 Docker Commands

| Command                                                     | Description                           |
| ----------------------------------------------------------- | ------------------------------------- |
| `docker compose up -d`                                      | Run containers in background          |
| `docker compose down`                                       | Stop containers                       |
| `docker compose down -v`                                    | Stop and remove all containers + data |
| `docker logs -f todo-api`                                   | Follow API logs                       |
| `docker exec -it todo-postgres psql -U postgres -d todo_db` | Open DB shell                         |

---

## ⚙️ Environment Variables

Defined in `docker-compose.yml`:

| Variable                 | Description         | Default       |
| ------------------------ | ------------------- | ------------- |
| `POSTGRES_HOST`          | Database host       | `postgres`    |
| `POSTGRES_DB`            | Database name       | `todo_db`     |
| `POSTGRES_USER`          | Database username   | `postgres`    |
| `POSTGRES_PASSWORD`      | Database password   | `postgres`    |
| `ASPNETCORE_ENVIRONMENT` | ASP.NET environment | `Development` |

---

## 💾 Data Persistence

Data is persisted in Docker volume `pgdata`.
To clear everything and start fresh:

```bash
docker compose down -v
```

---

## 🧪 Local Development (No Docker)

You can run it directly on your machine:

1. Ensure PostgreSQL is running (port `5432`).
2. Update your `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "Postgres": "Host=localhost;Port=5432;Database=todo_db;Username=postgres;Password=postgres"
   }
   ```
3. Run migrations:

   ```bash
   dotnet ef database update
   ```
4. Start the API:

   ```bash
   dotnet run --project TodoApi/TodoApi.csproj
   ```
5. Visit: [http://localhost:5185/swagger](http://localhost:5185/swagger)

---

## 🧩 About the `.sln` File

* `dotnet-to-do.sln` groups all .NET projects (like `TodoApi`, future test projects, etc.) into one **solution**.
* Helps when:

  * Using **Visual Studio** or **Rider**.
  * Building multiple projects at once.
  * Running CI/CD pipelines (builds everything via `dotnet build dotnet-to-do.sln`).

It’s safe and recommended to **commit the `.sln` file** to Git.

---

## 🧱 Recommended Repository `.gitignore`

```gitignore
# Build output
bin/
obj/

# User-specific files
*.user
*.suo

# IDE
.vs/
.idea/

# Logs
*.log

# OS
.DS_Store
Thumbs.db

# Docker volumes or generated data
pgdata/
.env
```

---

## 🚀 Future Enhancements

✅ JWT Authentication
✅ Role-Based Access Control
✅ Docker pgAdmin service for DB UI
✅ CI/CD via GitHub Actions
✅ Deploy to Azure / AWS ECS / Kubernetes

---

## 🧑‍💻 Author

**Arshad Khan**
Built with ❤️ using **.NET 9**, **Entity Framework Core**, and **PostgreSQL**.
Feel free to fork, star ⭐, and improve this project!

---

## 📜 License

This project is open source under the [MIT License](LICENSE).

```

---

Would you like me to include a **pgAdmin service** section (so you can view your PostgreSQL data visually at `http://localhost:5050` and add that setup into this README)?
```
