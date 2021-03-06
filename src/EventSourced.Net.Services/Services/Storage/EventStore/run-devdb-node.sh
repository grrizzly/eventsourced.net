#!/usr/bin/env bash
EVENTSTORE_DIR="{EventStoreInstallPath}"
cd $EVENTSTORE_DIR
LD_LIBRARY_PATH=${EVENTSTORE_DIR}:$LD_LIBRARY_PATH ${EVENTSTORE_DIR}/eventstored --db ./db --log ./log --run-projections=system --start-standard-projections $@