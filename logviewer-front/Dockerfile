## Stage 1 - Build
FROM node:14.15-alpine3.11 as build
WORKDIR /usr/src/app
COPY package.json package-lock.json ./
# Install the dependencies
RUN npm install
# Copy the files
COPY . ./
RUN npm run build

## Stage 2: deploy
FROM nginx:1.19.6-alpine
# Copy Nginx config file
COPY ./docker/nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/src/app/dist/logviewer-front /usr/share/nginx/html
