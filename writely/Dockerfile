FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy *.csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the project code into the container
COPY . ./

# Publish the code with the Release configuration
RUN dotnet publish -c Release -o ./out

# Now build the runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

# Copy the published code from the out dir into the container root (app)
COPY --from=build-env app/out .
ENTRYPOINT ["dotnet", "writely.dll"]