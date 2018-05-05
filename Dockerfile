# Build the .NET part
FROM microsoft/dotnet:2.0.5-sdk-2.1.4 AS dotnet-build
WORKDIR /src
COPY ./README.md /Packages
COPY ./Build ./Build
COPY ./NuGet.Config ./
COPY ./Source/. ./Source
WORKDIR /src/Source/Web
RUN dotnet restore --ignore-failed-sources
RUN dotnet publish -c Release -o out

# Build the static content
FROM node:latest AS node-build
WORKDIR /src
COPY ./Source/Web/. ./Source/Web
RUN find . -name "*.cs" -type f -delete
RUN find . -name "*.csproj" -type f -delete
WORKDIR /src/Source/Web
RUN yarn global add webpack
RUN yarn global add webpack-cli
RUN yarn add babel-loader
RUN yarn
RUN webpack -p --env.production

# Build runtime image
FROM microsoft/aspnetcore:2.0.5
WORKDIR /app
COPY --from=dotnet-build /src/Source/Web/out ./
COPY --from=node-build /src/Source/Web/wwwroot ./wwwroot
EXPOSE 80
ENTRYPOINT ["dotnet", "Web.dll"]