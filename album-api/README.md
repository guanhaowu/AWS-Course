<p align="center">
  <a href="" rel="noopener">
 <img width=200px height=200px src="https://www.hogeschoolrotterdam.nl/globalassets/afbeeldingen/go/huisstijl/logo-hr_linkedin200x200px.jpg" alt="Project logo"></a>
</p>

<h3 align="center">Advanced Programming: AWS Cloud assignment 3</h3>

---
<h3 align="center">Topics:</h3>
<p align="center"> Set up a .NetCore Web Api, Create Test unit, Create Integration Test, Create Buildspec file to automate all commands for AWS CodeBuild, Elastic Container Registry 
    <br> 
</p>

## 📝 Table of Contents

- [About](#about)
- [Getting Started](#getting_started)
- [Usage](#usage)
  [Testing](#testing)
- [Built Using](#built_using)
- [Authors](#authors)

---

## About <a name = "about"></a>

The purpose of this project is to get some insight on the following topics:
- How to make a very basic Web API in .NetCore.
- Create a Test project next to it to validate the inputs and the endpoint (**Integration** & **unit tests**).
- Buildspec to automate executing commands such as build project, run test, create image, push image etc.
- Elastic Container Registry (ECR) to host images.

### Notes
Due to CodeBuild being disabled for students, I had to manually execute the commands i wrote in ``buildspec`` to build, test, create image and upload image to ECR. So this Readme will be made with that in mind.

## Getting Started <a name = "getting_started"></a>

### Prerequisites

```
- Docker installed.
- Hyper-V enabled in BIOS.
- Enable Hyper-V in Windows Feature (for Windows).
- Knowledge on how to create a Dockerfile.
- ASP.NET Core 5.0
```

---

## Usage <a name="usage"></a>
Run the commands below in your windows Terminal (CMD).

### Restoring project packages
First, in order to start using the project, execute:
```
dotnet restore Album-Api
```
This will restore all **packages** needed for the 2 projects within this **solution**.


### Building project files
Now in order to run the Web Api, it is first required to have the backend files be build.
Use this command to build:
```
dotnet build Album-Api/Album.Api
```

### Run project Album-Api
To run the project locally on port ``5000`` & ``5001``:
```
dotnet run --project Album-Api/Album.Api
```
* Note: It will first build then run the project.

### Run project for health check
To run the basic configuration scenario:
```
dotnet run --project Album-Api/Album.Api --scenario basic
```
* Note: More info about Health Check status can be found at https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0.

---

## Testing <a name = "testing"></a>
In this project, there is **Integration Testing** and **Unit testing** to test the Web Api ``endpoint`` and the ``GreetingService`` implementation.

In order to test both in one command, run the command below:
```
dotnet test Album-Api --logger "console;verbosity=detailed"
```
* Note: the ``--logger`` parameter is meant to set the level of detailed output for the test.

GreetingService test:
```
dotnet test Album-Api --filter Album.Api.Tests.GreetingServiceTest --logger "console;verbosity=detailed"
```

Integration test:
```
dotnet test Album-Api --filter Album.Api.Tests.IntegrationTest --logger "console;verbosity=detailed"
```

### Docker File
Docker File is to automate the process of creating images with certain configuration. This docker file can then be used on AWS services to create the image for you. In this context, it will be manually created locally to upload to ECR.
Read https://docs.docker.com/engine/reference/builder/ for more information.

##### Example
```
  # syntax=docker/dockerfile:1
  FROM mcr.microsoft.com/dotnet/aspnet:5.0
  COPY Album-Api/app/ App/
  WORKDIR /App
  
  ENTRYPOINT ["dotnet", "Album.Api.dll"]
  
  EXPOSE 5000
  EXPOSE 5001
```
* Note
  * The ``COPY`` and ``EXPOSE`` are instructions. ``Expose`` is to define which port this container is bound to.
  * Port 5000 and 5001 to be used for the Web API inside the container. To use this image for a container on the local machine, bind port ``80:80`` when using ``docker run``.
    * Port: [Host_Port]:[Container_Port]
  * This example creates the image outside of Docker container. Read https://docs.docker.com/samples/dotnetcore/ for more information.

---

## Built Using <a name = "built_using"></a>

### Buildspec
Normally, the ``buildspec.yaml`` file is used on CodeBuild to automate all the steps. But due to limitations, this guide will ommit the CodeBuild steps and only show the content of the Buildspec file and some informations.
```
version: 0.2

phases:
  install:
    commands:
      - echo "Executing Dotnet restore command to ensure all packages are installed"
      - dotnet restore Album-Api
    build:
      on-failure: ABORT
      commands:
        - echo "Building the Web API backend..."
        - dotnet build Album-Api/Album.Api
        - echo "Build completed."
        - echo "Running Unit tests..." # These 4 command lines could be moved to post-build phase too.
        - dotnet test Album-Api --filter Album.Api.Tests.GreetingServiceTest --logger "console;verbosity=detailed"
        - echo "Running Integration tests..."
        - dotnet test Album-Api --filter Album.Api.Tests.IntegrationTest --logger "console;verbosity=detailed"
        - echo "Creating a Linux release build version in Root DIR 'Album-Api/app/' ..." # To create the folder required by Dockerfile
        - dotnet publish -c Release -r linux-x64 -o ./Album-Api/app/ ./Album-Api/Album.Api/Album.Api.csproj
        - echo "Release build completed."
      post-build:
        commands:
          - echo Logging in to Amazon ECR...
          - aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 374872396823.dkr.ecr.us-east-1.amazonaws.com
          - echo "Creating Image from Dockerfile..."
          - docker build -t cnsd/album:latest .
          - echo "Tagging image to prepare for push to ECR..."
          - docker tag cnsd/album:latest 374872396823.dkr.ecr.us-east-1.amazonaws.com/cnsd/album:$CODEBUILD_BUILD_NUMBER
          - echo "Pushing image to ECR..."
          - docker push 374872396823.dkr.ecr.us-east-1.amazonaws.com/cnsd/album:$CODEBUILD_BUILD_NUMBER
```
**Notes**
- ``version``(required) Defines the version of your buildspec file. You can choose between version `0.1` or `0.2` (latest).
- ``phases`` (required) Represents the commands CodeBuild runs during each phase of the build. 
  <br /> Build Options:
    - ``install`` Can define all your package installation commands that are required by your build at the start.
        - ``commands`` The commands to execute from top to bottom one by one. In the context above, it is used for restoring packages to the project.
    - ``build`` Build phase where you define the instructions to build, test etc.
        - ``on-failure`` What should happen should failure occur. Options: ``ABORT`` or ``CONTINUE``.
    - ``post-build`` Is used to handle other instructions that should happen after build phase is completed.
    * More information can be found at https://docs.aws.amazon.com/codebuild/latest/userguide/build-spec-ref.html
    * Reference used for Buildspec:
      https://docs.aws.amazon.com/codebuild/latest/userguide/sample-docker.html
      
### Manual build
In order for the Dockerfile to create an image with its contents outside of the docker container, it is required for the Album.Api project to build and published in the project.
```
dotnet publish -c Release -r linux-x64 -o ./Album-Api/app/ ./Album-Api/Album.Api/Album.Api.csproj
```

Given that you have AWS CLI installed and up and configured, we can start building the image, login using AWS CLI and upload to ECR.
```
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 374872396823.dkr.ecr.us-east-1.amazonaws.com
```

#### Build image according to the Dockerfile:
```
docker build -t cnsd/album:latest .
```
Use ``docker images`` command to check if the image has been created.
<a href="https://i.imgur.com/gqxpPJC.png" rel="noopener">
  <img src="https://i.imgur.com/gqxpPJC.png" alt="Docker Image list">
</a>

#### Tag & Push image to ECR
```
docker tag cnsd/album:latest 374872396823.dkr.ecr.us-east-1.amazonaws.com/cnsd/album:$CODEBUILD_BUILD_NUMBER
docker push 374872396823.dkr.ecr.us-east-1.amazonaws.com/cnsd/album:$CODEBUILD_BUILD_NUMBER
```
<a href="https://i.imgur.com/fl60eP0.png" rel="noopener">
<img src="https://i.imgur.com/fl60eP0.png" alt="Docker Image list">
</a>

* Note 1: The digits in front of ``.dkr`` are unique to the repository.
* Note 2: $CODEBUILD_BUILD_NUMBER is replaced with ``v1`` or ``v1.1`` as tag in this manual part.

#### ECR
At this point, the Image should be on ECR.
<a href="https://i.imgur.com/CvIURxs.png" rel="noopener">
  <img src="https://i.imgur.com/CvIURxs.png" alt="Docker Image list">
</a>


### Live url
Once the Web Api is running locally with ``dotnet run --project Album-Api/Album.Api``, visit:
* https://localhost:5001/api/Hello - API endpoint.
* https://localhost:5001/api/Hello?name=Test - API endpoint with parameter name and value Test.
* https://localhost:5001/swagger/index.html - Swagger UI.

---

## ✍️ Authors <a name = "authors"></a>

- [@guanhaowu](https://github.com/guanhaowu)
