# Define base image
FROM microsoft/dotnet:2.2-sdk AS build-env

# Copy project files
WORKDIR /source
COPY ["DESPortal.Widgets.API/DESPortal.Widgets.API.csproj", "./DESPortal.Widgets.API/DESPortal.Widgets.API.csproj"]

# Restore
RUN dotnet restore "./DESPortal.Widgets.API/DESPortal.Widgets.API.csproj"

# Copy all source code
COPY . .

# Publish
WORKDIR /source
RUN dotnet publish -c Release -o /publish

# Runtime
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /publish
COPY --from=build-env /publish .
ENTRYPOINT ["dotnet", "DESPortal.Widgets.API.dll"]
