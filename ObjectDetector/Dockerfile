# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files into the container
COPY . ./

# Restore dependencies and build the application
RUN dotnet publish -c Release -o /out

# Use the official .NET runtime image to run the application
#FROM mcr.microsoft.com/dotnet/runtime:9.0
FROM ubuntu:24.10 


RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
RUN apt-get update; apt-get install -y ttf-mscorefonts-installer fontconfig
RUN apt-get install -y dotnet-runtime-9.0

# Set the working directory for the runtime container
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /out .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "ObjectDetector.dll"]
