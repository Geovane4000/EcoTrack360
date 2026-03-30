#!/usr/bin/env bash
set -euo pipefail

# Deploy remoto via SSH: copia docker-compose e .env e executa docker compose up -d
# Usage: ./remote-deploy.sh user@host /path/to/remote/dir /path/to/key
REMOTE=${1:-}
REMOTE_DIR=${2:-/opt/esg}
SSH_KEY=${3:-}

if [ -z "$REMOTE" ]; then
  echo "Usage: $0 user@host [remote_dir] [ssh_key]"
  exit 1
fi

echo "Copiando arquivos para $REMOTE:$REMOTE_DIR"
scp -i "$SSH_KEY" docker-compose.yml .env "$REMOTE:$REMOTE_DIR/"

echo "Executando docker compose no remoto"
ssh -i "$SSH_KEY" "$REMOTE" "cd $REMOTE_DIR && docker compose pull && docker compose up -d --remove-orphans"

echo "Deploy remoto concluído"