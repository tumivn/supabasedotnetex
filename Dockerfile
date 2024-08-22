FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /hellomvc

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /hellomvc
COPY --from=build-env /hellomvc/out .
ENTRYPOINT ["dotnet", "hellomvc.dll"]