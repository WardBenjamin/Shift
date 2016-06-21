#!/bin/bash -e

# THIS SHOULD NEVER BE RUN MANUALLY. 
# Instead, run just run `./docs generate` on Linux
# or `docs.bat generate` on Windows from the root 
# project directory for a manual documentation build.

# Generate the HTML documentation
doxygen Documentation/Doxyfile

