FROM node:12.10-alpine
WORKDIR /app
COPY package.json ./package.json
RUN npm install
COPY . .
CMD npm test