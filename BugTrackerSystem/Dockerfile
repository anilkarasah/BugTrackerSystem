FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /source
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app
 
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime
ENV ASPNETCORE_URLS http://+:5000
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT [ "dotnet", "BugTrackerAPI.dll" ]
