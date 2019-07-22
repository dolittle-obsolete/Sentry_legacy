#!/bin/bash
docker tag dolittle/sentry-gateway-core dolittle/sentry-gateway-core:poc
docker tag dolittle/sentry-gateway-web dolittle/sentry-gateway-web:poc
docker push dolittle/sentry-gateway-core:poc
docker push dolittle/sentry-gateway-web:poc
