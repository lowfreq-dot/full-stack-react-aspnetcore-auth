FROM node:lts-alpine

WORKDIR /app

COPY . .
RUN npm install --legacy-peer-deps --save
RUN npm run build
CMD ["npm", "run", "dev"]