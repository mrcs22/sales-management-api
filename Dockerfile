FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /source
COPY . .
RUN dotnet restore "./src/tech-test-payment-api.csproj" 

RUN dotnet publish "./src/tech-test-payment-api.csproj" -c release -o /app --no-restore

EXPOSE 5000

