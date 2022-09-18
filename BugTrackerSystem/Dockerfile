FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /source
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime

WORKDIR /app
COPY --from=build /app ./

RUN dotnet dev-certs https --trust

ENTRYPOINT [ "dotnet", "BugTrackerAPI.dll" ]
