#!/usr/bin/env bash
set -euo pipefail

# Usage:
# ./scripts/create-regcred.sh <registry> <username> <password> [namespace]
# Example:
# ./scripts/create-regcred.sh docker.io myuser mypassword staging

REGISTRY=${1:-}
USERNAME=${2:-}
PASSWORD=${3:-}
NAMESPACE=${4:-default}

if [ -z "$REGISTRY" ] || [ -z "$USERNAME" ] || [ -z "$PASSWORD" ]; then
  echo "Usage: $0 <registry> <username> <password> [namespace]"
  exit 1
fi

DOCKERCFG_JSON=$(printf '{"auths":{"%s":{"username":"%s","password":"%s","auth":"%s"}}}' \
  "$REGISTRY" "$USERNAME" "$PASSWORD" "$(printf "%s:%s" "$USERNAME" "$PASSWORD" | base64 | tr -d '\n')")

ENC=$(printf '%s' "$DOCKERCFG_JSON" | base64 | tr -d '\n')

cat > k8s/regcred.yaml <<EOF
apiVersion: v1
kind: Secret
metadata:
  name: regcred
type: kubernetes.io/dockerconfigjson
data:
  .dockerconfigjson: ${ENC}
EOF

echo "Wrote k8s/regcred.yaml (namespace: $NAMESPACE). To apply: kubectl apply -f k8s/regcred.yaml -n $NAMESPACE"
