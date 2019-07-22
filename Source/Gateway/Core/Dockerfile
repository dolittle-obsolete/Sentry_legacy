FROM microsoft/dotnet:2.2-sdk-bionic as build

# Copy all project files and restore first
WORKDIR /work/

COPY ./Build/MSBuild/ ./Build/MSBuild
COPY ./Source/Gateway/Gateway.sln ./Source/Gateway/Gateway.sln
COPY ./Source/Gateway/Concepts/Concepts.csproj ./Source/Gateway/Concepts/Concepts.csproj
COPY ./Source/Gateway/Core/Core.csproj ./Source/Gateway/Core/Core.csproj
COPY ./Source/Gateway/Domain/Domain.csproj ./Source/Gateway/Domain/Domain.csproj
COPY ./Source/Gateway/Domain.Specs/Domain.Specs.csproj ./Source/Gateway/Domain.Specs/Domain.Specs.csproj
COPY ./Source/Gateway/Events/Events.csproj ./Source/Gateway/Events/Events.csproj
COPY ./Source/Gateway/Policies/Policies.csproj ./Source/Gateway/Policies/Policies.csproj
COPY ./Source/Gateway/Read/Read.csproj ./Source/Gateway/Read/Read.csproj
COPY ./Source/Gateway/Read.Specs/Read.Specs.csproj ./Source/Gateway/Read.Specs/Read.Specs.csproj

WORKDIR /work/Source/Gateway
RUN dotnet restore

# Build Core project
WORKDIR /work/Source/Gateway
COPY ./Source/Gateway/bounded-context.json ./bounded-context.json
COPY ./Source/Gateway/Concepts/ ./Concepts/
COPY ./Source/Gateway/Core/ ./Core/
COPY ./Source/Gateway/Domain/ ./Domain/
COPY ./Source/Gateway/Domain.Specs/ ./Domain.Specs/
COPY ./Source/Gateway/Events/ ./Events/
COPY ./Source/Gateway/Policies/ ./Policies/
COPY ./Source/Gateway/Read/ ./Read/
COPY ./Source/Gateway/Read.Specs/ ./Read.Specs/
WORKDIR /work/Source/Gateway/Core/
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime-bionic

WORKDIR /App
COPY --from=build /work/Source/Gateway/Core/out ./
COPY --from=build /work/Source/Gateway/Core/.dolittle/ ./.dolittle/
COPY ./Source/application.json ./Source/Gateway/bounded-context.json ./

EXPOSE 80
ENTRYPOINT ["dotnet", "Core.dll"]