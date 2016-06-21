#!/bin/bash -e

# Settings

REPO_PATH=git@github.com:WardBenjamin/Shift
HTML_PATH=Documentation/html
COMMIT_USER="Documentation Builder"
COMMIT_EMAIL="ward.programm3r@gmail.com"
CHANGESET=$(git rev-parse --verify HEAD)
BUILD_NUM=$1

# Create and commit the documentation repo
cd ${HTML_PATH}
git add .
git config user.name "${COMMIT_USER}"
git config user.email "${COMMIT_EMAIL}"
git commit -m "Automated documentation build for changeset ${CHANGESET}, Travis CI build #: ${BUILD_NUM}"
git push origin gh-pages
cd -
