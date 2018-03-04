# Export and compress the Docker image for deployment
docker save -o ./speercs-docker.tar cookieeaters/speercs
# Compress with 7z
7z a -mmt ./speercs-docker.tar.7z ./speercs-docker.tar
