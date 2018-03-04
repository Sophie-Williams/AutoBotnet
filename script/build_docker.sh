# Build Docker Image
COMMIT_HASH=$(git log -1 --format=%h)
docker build -t speercs --build-arg GIT_COMMIT=$COMMIT_HASH .
# Tag and prepare
docker tag speercs cookieeaters/speercs:$COMMIT_HASH
