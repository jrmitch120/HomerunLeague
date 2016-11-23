FROM mono:4.2.1.102
RUN mkdir /app
COPY . /app
EXPOSE 9001
CMD ["mono", "/app/HomerunLeague.SelfHost.exe", "-d"]