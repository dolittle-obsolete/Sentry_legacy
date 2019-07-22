#!/bin/bash
pushd
cd ./Web
docker build -t dolittle/sentry-gateway-web .
cd ../../..
docker build -t dolittle/sentry-gateway-core -f Source/Gateway/Core/Dockerfile .
popd