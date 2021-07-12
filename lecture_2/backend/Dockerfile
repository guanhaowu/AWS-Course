FROM tomcat:8.5-jdk8

LABEL maintainer="0976154@hr.nl"

COPY cddb4.war /usr/local/tomcat/webapps/cddb.war

COPY wait-for-it.sh /usr/local/tomcat/wait-for-it.sh

EXPOSE 8080