#!/usr/bin/env bash
set -euo pipefail

# usar KUBECONFIG ou contexto atual
NAMESPACE=${1:-default}

echo "Aplicando manifests no namespace: $NAMESPACE"
kubectl apply -n $NAMESPACE -f k8s/

echo "Rolando restart nos deployments para forçar pull de imagem (opcional)"
kubectl rollout restart deployment/ecotrack360-app -n $NAMESPACE || true
kubectl rollout status deployment/ecotrack360-app -n $NAMESPACE --timeout=120s || true

echo "Deploy concluído"