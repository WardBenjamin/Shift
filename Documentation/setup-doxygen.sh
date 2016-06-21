#!/bin/bash -e

# THIS SHOULD NEVER BE RUN MANUALLY. 
# Instead, run just run `./docs generate` on Linux
# or `docs.bat generate` on Windows from the root 
# project directory for a manual documentation build.

# Settings
REPO_PATH=git@github.com:WardBenjamin/Shift
HTML_PATH=Documentation/html

# Get a clean version of the HTML documentation
rm -rf ${HTML_PATH}
mkdir -p ${HTML_PATH}
git clone -b gh-pages "${REPO_PATH}" --single-branch ${HTML_PATH}

# RM all of the files through git to prevent stale files
cd ${HTML_PATH}
git rm -rf .
cd -

