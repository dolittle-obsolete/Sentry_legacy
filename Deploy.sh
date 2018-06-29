#!/bin/bash
docker push dolittle/sentry
kubectl patch deployment sentry --namespace dolittle -p "{\"spec\":{\"template\":{\"metadata\":{\"labels\":{\"date\":\"`date +'%s'`\"}}}}}"
sleep 30
kubectl delete -f K8s/ingress.yml
kubectl apply -f K8s/ingress.yml