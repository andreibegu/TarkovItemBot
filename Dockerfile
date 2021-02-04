FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
ADD . /app
WORKDIR /app
RUN dotnet restore -r linux-x64
RUN dotnet publish --self-contained -c Release -r linux-x64 -p:PublishSingleFile=true

FROM gcr.io/distroless/cc-debian10
WORKDIR /app
COPY --from=build /app /app/
CMD ["bin/Release/net5.0/linux-x64/publish/TarkovItemBot"]