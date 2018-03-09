FROM microsoft/aspnetcore-build:2.0

# Copy  client app and build Client
COPY . /app
WORKDIR /app

RUN npm install --unsafe-perm -g webpack@3.9.1
RUN npm install
RUN npm rebuild node-sass --force
RUN webpack
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
EXPOSE 80/tcp

ENV ASPNETCORE_ENVIRONMENT=Development
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh
