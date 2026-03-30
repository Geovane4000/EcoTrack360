`# Projeto - Cidades ESGInteligentes (EcoTrack360)`

Resumo: este repositório contém a aplicação C# (.NET 8) e os artefatos DevOps necessários para entrega do desafio: `Dockerfile`, `docker-compose.yml`, manifests Kubernetes (`k8s/`), pipelines (`.github/workflows/ci-cd.yml`), scripts de deploy e documentação.

## Como executar localmente com Docker

1. Copie `.env.example` para `.env` e ajuste os valores (senha do Mongo, registry, tag).
2. Subir serviços: `docker compose up --build`
3. A aplicação estará exposta em `http://localhost:8080` (conforme `docker-compose.yml`).
4. Logs: `docker compose logs -f app`

Obs: se preferir executar apenas a aplicação sem Mongo local, ajuste `MONGODB_CONNECTION` para apontar a um Mongo existente.

## Pipeline CI/CD

Ferramenta: `GitHub Actions` (arquivo: `.github/workflows/ci-cd.yml`).

Fluxo implementado:
- `build-and-test`: executa `dotnet build` e `dotnet test`.
- `build-and-push-image`: constrói imagem Docker e faz push para o registry definido por `DOCKER_REGISTRY`.
- `deploy-staging`: ao push na branch `staging` aplica `k8s/` no namespace `staging` usando `KUBE_CONFIG` (secret do GitHub).
- `deploy-production`: ao push na branch `main` aplica `k8s/` no namespace `production`.

Secrets esperados no repositório (Settings → Secrets):
- `DOCKER_REGISTRY` (ex.: `docker.io/youruser`)
- `DOCKER_USERNAME`
- `DOCKER_PASSWORD`
- `DOCKER_IMAGE` (nome da imagem, ex.: `ecotrack360`)
- `KUBE_CONFIG` (conteúdo do kubeconfig para o cluster gerenciado pelo Rancher/k8s)

Se usar deploy via SSH (opção alternativa), veja `deploy/remote-deploy.sh` e as variáveis `STAGING_*` / `PROD_*` já mencionadas.

## Containerização

`Dockerfile` usa multi-stage (SDK -> publish -> Runtime). Imagem base runtime: `mcr.microsoft.com/dotnet/runtime:8.0`.

Estratégia:
- Build em etapa separada para reduzir tamanho final.
- Volumes para logs (`./logs` → `/app/logs`) em `docker-compose.yml`.
- `docker-compose.yml` orquestra `app` + `mongo` com variáveis de ambiente lidas de `.env`.

## Orquestração Kubernetes

Manifests em `k8s/`:
- `app-deployment.yaml` (Deployment + Service)
- `mongo-deployment.yaml` (Deployment + PVC + Service)
- `secret.yaml` (Secret com stringData para credenciais)
- `ingress.yaml` (exemplo para `ecotrack.local`)

Use `kubectl apply -f k8s/ -n <namespace>` ou os scripts `deploy/k8s-deploy.sh` / `deploy/k8s-deploy.ps1`.

Se estiver usando Rancher, baixe o `kubeconfig` do cluster e armazene no secret `KUBE_CONFIG` do GitHub.

## Como preparar entrega (.zip)

Um script PowerShell foi incluído para gerar o pacote de entrega: `scripts/package-delivery.ps1`.
Uso:
 - Abra PowerShell na raiz do repositório e execute: `.	emplates\scripts\package-delivery.ps1 -OutputPath .\ecotrack360-delivery.zip`
 (ou simplesmente `.	emplates\scripts\package-delivery.ps1` para gerar `ecotrack360-delivery.zip` na raiz).

O pacote inclui: `Dockerfile`, `docker-compose.yml`, `k8s/`, `src/` (código), `.github/workflows/`, `README.md`, `.env.example`, `deploy/`, `docs/`.

## Prints do funcionamento / Evidências

Inclua capturas de tela em `docs/prints/` e referencie-as no `docs/Presentation.md` ou `docs/Documentation.md` antes de gerar o PDF/PPT final.

## Tecnologias utilizadas

- .NET 8
- MongoDB
- Docker / Docker Compose
- Kubernetes (manifests) / Rancher
- GitHub Actions

## Scripts úteis

- `deploy/k8s-deploy.sh` / `deploy/k8s-deploy.ps1` — aplica manifests em um namespace.
- `deploy/remote-deploy.sh` / `deploy/remote-deploy.ps1` — deploy via SSH (copia `docker-compose.yml` + `.env` e executa `docker compose up -d`).
- `scripts/package-delivery.ps1` — gera `zip` para submissão.

## Checklist e próximos passos

Veja `CHECKLIST.md` para o checklist de entrega. Antes de compactar, adicione prints em `docs/prints/` e preencha `docs/Presentation.md` com evidências.
