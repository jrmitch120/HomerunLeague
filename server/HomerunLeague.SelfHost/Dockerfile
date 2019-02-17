FROM mono:4.2.1.102

# Set Timezone
ENV TZ=America/Chicago
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# Install FTP
RUN apt-get update && apt-get install -y \
ftp

# Copy in application and run
RUN mkdir /app
COPY . /app
EXPOSE 9001
CMD ["mono", "/app/HomerunLeague.SelfHost.exe", "-d"]