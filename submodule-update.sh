#!/bin/bash
cd WPFLib
if [ -d .git ] || git rev-parse --git-dir > /dev/null 2>&1; then
	git pull origin master
	cd ..
	git add .
	git commit -m "Updated WPFLib to Latest master version"
	echo "Update complete. Commit created"
else
	git submodule init
	git submodule update
	echo "Submodule checkout complete"
fi
