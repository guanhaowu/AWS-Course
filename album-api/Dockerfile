  # syntax=docker/dockerfile:1
  FROM mcr.microsoft.com/dotnet/aspnet:5.0
  COPY Album-Api/app/ App/
  WORKDIR /App
  
  ENTRYPOINT ["dotnet", "Album.Api.dll"]
  
  EXPOSE 5000
  EXPOSE 5001
  
  
 