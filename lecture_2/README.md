<p align="center">
  <a href="" rel="noopener">
 <img width=200px height=200px src="https://www.hogeschoolrotterdam.nl/globalassets/afbeeldingen/go/huisstijl/logo-hr_linkedin200x200px.jpg" alt="Project logo"></a>
</p>

<h3 align="center">Advanced Programming: AWS Cloud assignment 2</h3>

---

<p align="center"> How to set up and run the frontend, backend and the database containers.
    <br> 
</p>

## 📝 Table of Contents

- [About](#about)
- [Getting Started](#getting_started)
- [Usage](#usage)
- [Built Using](#built_using)
- [Authors](#authors)

---

## About <a name = "about"></a>

The purpose of this project is to get some insight on how to use Docker and setup the AWS services.

## Getting Started <a name = "getting_started"></a>

These instructions will get you a copy of the project up and running on your **Windows local machine** for development and testing purposes.

### Prerequisites

```
- Docker installed.
- Hyper-V enabled in BIOS.
- Enable Hyper-V in Windows Feature (for Windows).
- Knowledge on how to create a Dockerfile.
```

---
## Usage <a name="usage"></a>
Run the commands below in your windows Terminal (CMD).

### Images
Images is a file that contains everything what a Virtual Machine is needed to run. Think of it as a virtual disc that contains the OS or the software.

To see list of images downloaded, use ``docker images`` in the Terminal.

A step by step series of examples that tell you how to ``pull`` images from repository and ``run`` a Containers based off the ``image``.

#### Pull image

If the image doesn't exist locally, it will automatically lookup in the repository for the ``latest`` version when no version tag has been specified.

```
docker pull mysql:5.6
```

#### Remove image

If you pulled a wrong image or would like to delete an image, execute this command:

```
docker image rm [image name:version]
```
* Note: You can only delete images that are not currently in use. If a container is using the image, first delete the container.

### Containers
Containers are based off from images. They are essentially a Virtual Machine that run as a copy of the image.

#### Containers list
To see list of active containers use ``docker ps`` or ``docker ps -a`` to see all containers including inactives ones.

#### Remove Container
To remove a **container**, execute: ``docker rm [container name:tag]``

#### Create and Running MYSQL container
With ``docker run mysql:5.6``, it download the images with the specified tags (version) if it doesn't exist yet and runs it. 

#### Starting & stopping container
If the container exists, using ``docker start [name:tag]`` will start the container. And to stop the container, use ``docker stop [name:tag]``.

### Additional parameters
The following additional parameters are meant for ``docker run``.

#### Passing param as env
With ``-e`` you can add environment key-value as parameter.

#### Daemon
With  ``-d`` parameter, it can be run as a daemon. 

#### Naming the container
With ``--name`` parameter, you can give a name to the container.

#### Port binding/mapping
With ``-p`` parameter, the port can be mapped like this: ``-p 23306:3306``:

#### Usage example
```
docker run -d -e MYSQL_ROOT_PASSWORD=quintor_pw -e MYSQL_DATABASE=cddb_quintor -e MYSQL_USER=cddb_quintor -e MYSQL_PASSWORD=quintor_pw -p 23306:3306  --name cddb_mysql mysql:5.6
```
* _Note: To test if the connection is up, use MYSQL WorkBench._

---

### Docker File
Docker File is to automate the process of creating images with certain configuration. This docker file can then be used on AWS services to create the image for you.

#### Create DockerFile
To create a dockerfile, you need to define at least the following:
- From which image to use.
- An instruction.

Read https://docs.docker.com/engine/reference/builder/ for more information.

##### Example
```
FROM nginx:alpine

LABEL maintainer="0976154@hr.nl"

COPY frontend/src /usr/share/nginx/html

COPY frontend/resources/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
```

* The ``LABEL`` is used to define who a variable with its value. In the example above, the maintainer of this dockerfile.
* The ``COPY`` and ``EXPOSE`` are instructions. ``Expose`` is to define which port this container is bound to.

## Built Using <a name = "built_using"></a>

### Dockerfile

If your project already contains a **dockerfile**, use the command ``docker build .`` to build the image from it. Using ``-t`` parameter to give it an custom tag.
<br> 
Read https://docs.docker.com/engine/reference/commandline/build/ for more information.

#### Example
```
docker build -t nginx:custom .
```
<a href="https://i.imgur.com/AgJoRow.png" rel="noopener">
  <img src="https://i.imgur.com/AgJoRow.png" alt="docker build"></img>
</a>

### Docker Compose file
If the project contains a docker compose file, by executing it, it will run the containers defined inside with the given parameters.

#### Docker Compose file example
The code below must be saved as ``docker-compose.yml``.

```
version: "3.9"

services:
  mysql:
    image: mysql:5.6
    volumes:
      - ./mysql_data:/var/lib/mysql
    environment:
      - "MYSQL_ROOT_PASSWORD=quintor_pw"
      - "MYSQL_DATABASE=cddb_quintor"
      - "MYSQL_USER=cddb_quintor"
      - "MYSQL_PASSWORD=quintor_pw"
    ports:
      - "23306:3306"

  cddb_frontend:
    depends_on:
      - cddb_backend
    build: ./frontend
    # links:
    # - "cddb_backend:cddb_backend"
    ports:
      - "20080:80"

  cddb_backend:
    depends_on:
      - mysql
    command:
      ["./wait-for-it.sh", "mysql:3306", "--strict", "--", "catalina.sh", "run"]
    build: ./backend
    # links:
    # - "mysql:cddb_mysql"
    ports:
      - "28080:8080"
```

#### Important Docker Compose commands
- ``Docker compose up`` - run the docker compose file as a service.
- ``Docker compose up -d`` - run the docker in the background (detached mode).
- ``Docker compose stop`` - Stops the running services.
- ``Docker compose down`` - Stops the service and removes the container.

#### Running the example

```
docker compose up
```
<a href="https://i.imgur.com/DwrCu56.png" rel="noopener">
  <img src="https://i.imgur.com/DwrCu56.png" alt="Docker compose"></img>
</a>

OR 

```
docker compose up -d
```
<a href="https://i.imgur.com/M5zX5Ey.png" rel="noopener">
  <img src="https://i.imgur.com/M5zX5Ey.png" alt="Docker compose detached"></img>
</a>

### Local Live
Once the containers are up and running, visit to following link to see the result:
* http://localhost:28080/cddb/rest/ - cddb_backend container
* http://localhost:20080/ - cddb_frontend container

---

## ✍️ Authors <a name = "authors"></a>

- [@guanhaowu](https://github.com/guanhaowu)

