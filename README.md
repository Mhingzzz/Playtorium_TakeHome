# Playtorium_TakeHome

```
dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Username=playtoriumadmin;Password=password;Database=ptr" Npgsql.EntityFrameworkCore.PostgreSQL -o Entities --context-dir ../Infrastructure/Database --context DataContext --context-namespace Infrastructure.Database --namespace Domain --no-onconfiguring --force --no-pluralize
```
