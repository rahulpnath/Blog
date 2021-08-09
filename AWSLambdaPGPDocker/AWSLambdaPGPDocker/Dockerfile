FROM amazon/aws-lambda-dotnet:core3.1 AS base  
  
FROM mcr.microsoft.com/dotnet/sdk:3.1 as build  
WORKDIR /src  
COPY ["AWSLambdaPGPDocker.csproj", "base/"]  
RUN dotnet restore "base/AWSLambdaPGPDocker.csproj"  
  
WORKDIR "/src"  
COPY . .  
RUN dotnet build "AWSLambdaPGPDocker.csproj" --configuration Release --output /app/build  
  
FROM build AS publish  
RUN dotnet publish "AWSLambdaPGPDocker.csproj" \  
            --configuration Release \
            --framework netcoreapp3.1 \
            --self-contained false \   
            --output /app/publish
  
FROM base AS final  
WORKDIR /var/task  
COPY --from=publish /app/publish .  
CMD ["AWSLambdaPGPDocker::AWSLambdaPGPDocker.Function::FunctionHandler"]