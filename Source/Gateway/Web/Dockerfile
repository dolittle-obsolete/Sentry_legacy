FROM node:11.6 AS build

# Copy package.json and install dependencies
WORKDIR /Source/
COPY ./package.json ./package.json
RUN yarn install

# Build Web project
COPY ./ ./
RUN yarn build

# Build runtime image
FROM nginx:1.15-alpine
COPY --from=build /Source/public /usr/share/nginx/html
COPY --from=build /Source/public /usr/share/nginx/html/sentry
COPY --from=build /Source/nginx-default.conf /etc/nginx/conf.d/default.conf