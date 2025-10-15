# README

## Running the Application

Start all services:

```bash
docker compose up
```

Run in detached mode:

```bash
docker compose up -d
```

Stop all services:

```bash
docker compose down
```

## Services

- **Client**: http://localhost:3000
- **Server**: http://localhost:5000
- **pgAdmin**: http://localhost:5050
- **PostgreSQL**: localhost:5432

## Adding Database to pgAdmin

1. Open pgAdmin at http://localhost:5050
2. Right-click **Servers** → **Register** → **Server**
3. **General** tab:
   - Name: `{{APP_NAME_LOWER}}` (or any name)
4. **Connection** tab:
   - Host: `postgres`
   - Port: `5432`
   - Database: `{{APP_NAME_LOWER}}`
   - Username: `{{APP_NAME_LOWER}}`
   - Password: `{{APP_NAME_LOWER}}`
5. Click **Save**

## Database Credentials

- Host: `postgres` (from within Docker) or `localhost` (from host machine)
- Database: `{{APP_NAME_LOWER}}`
- Username: `{{APP_NAME_LOWER}}`
- Password: `{{APP_NAME_LOWER}}`