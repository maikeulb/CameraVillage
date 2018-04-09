#!/bin/bash

set -e
run_cmd="dotnet run --server.urls http://*:80"

# until dotnet ef database update; do
# >&2 echo "Postgres is starting up"
# sleep 1
# done

until dotnet ef database update --context ApplicationDbContext; do
>&2 echo "Postgres is starting up"
sleep 1
done

until dotnet ef database update --context IdentityDbContext; do
>&2 echo "Postgres is starting up"
sleep 1
done

>&2 echo "Postgres is up - executing command"
exec $run_cmd
