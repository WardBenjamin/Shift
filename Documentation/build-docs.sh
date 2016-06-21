#!/bin/bash -e

if [[ $TRAVIS_BRANCH == 'master' && $TRAVIS_PULL_REQUEST == 'false' ]]
then
	openssl aes-256-cbc -K $encrypted_31d4dbf0621e_key -iv $encrypted_31d4dbf0621e_iv-in Documentation/travisci_rsa.enc -out Documentation/travisci_rsa -d
	chmod 0600 Documentation/travisci_rsa 
	cp Documentation/travisci_rsa ~/.ssh/id_rsa 
	cp Documentation/travisci_rsa.pub ~/.ssh/id_rsa.pub 
	Documentation/setup-doxygen.sh 
	Documentation/make-doxygen.sh 
	Documentation/publish-doxygen.sh $TRAVIS_BUILD_NUMBER 
fi