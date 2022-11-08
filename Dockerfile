FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.15 AS build

WORKDIR /app

COPY . .

RUN dotnet nuget add source https://www.myget.org/F/disqord/api/v3/index.json -n Disqord

RUN dotnet publish --self-contained -c Release -r alpine-x64 -o ./publish \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true

FROM alpine:3.16.2

RUN apk add --no-cache libstdc++ libintl krb5-libs icu-libs

COPY --from=build /app/publish /app

CMD ["/app/TarkovItemBot"]
