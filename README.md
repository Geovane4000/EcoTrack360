# Projeto - Cidades ESG Inteligentes

`EcoTrack360` é uma aplicação `.NET 8` com `MongoDB` preparada para demonstração de práticas DevOps de ponta a ponta: build, testes, containerização, orquestração e deploy automatizado.

## Como executar localmente com Docker

1. Copie `.env.example` para `.env`.
2. Ajuste os valores de imagem e senha do MongoDB, se necessário.
3. Execute `docker compose up --build -d`.
4. Acesse `http://localhost:8080` para ver o resumo da aplicação.
5. Verifique a saúde da aplicação em `http://localhost:8080/health` e `http://localhost:8080/health/ready`.
6. Consulte os logs com `docker compose logs -f app`.

Endpoints úteis para prints:
- `GET /`
- `GET /health`
- `GET /health/ready`
- `POST /seed`

## Pipeline CI/CD

Ferramenta utilizada: `GitHub Actions`.

Arquivo: `.github/workflows/ci-cd.yml`

Etapas implementadas:
- `build-and-test`: restore, build da solução e execução dos testes automatizados.
- `build-and-push-image`: gera a imagem Docker; publica no registry quando os secrets Docker estiverem configurados.
- `deploy-staging`: executa deploy automático no namespace `staging` quando houver push na branch `staging`.
- `deploy-production`: executa deploy automático no namespace `production` quando houver push na branch `master`.

Comportamento sem secrets:
- se faltarem `DOCKER_*`, o workflow faz apenas build da imagem (sem push), para não falhar o pipeline.
- se faltar `KUBE_CONFIG` ou dados de imagem, os jobs de deploy são pulados.

Secrets esperados no repositório:
- `DOCKER_REGISTRY` — ex.: `docker.io`
- `DOCKER_USERNAME`
- `DOCKER_PASSWORD`
- `DOCKER_IMAGE` — ex.: `seuusuario/ecotrack360`
- `KUBE_CONFIG` — conteúdo do kubeconfig do cluster

## Containerização

O `Dockerfile` utiliza estratégia multi-stage:
- estágio de build com `mcr.microsoft.com/dotnet/sdk:8.0`
- estágio final com `mcr.microsoft.com/dotnet/aspnet:8.0`

Estratégias adotadas:
- publicação em modo `Release`
- imagem final menor e pronta para runtime web
- porta `8080` exposta
- variáveis de ambiente para conexão com MongoDB e seed inicial

O `docker-compose.yml` sobe:
- `app`
- `mongo`

Também utiliza:
- `volume` persistente para MongoDB
- `network` dedicada
- variáveis de ambiente via `.env`

## Orquestração

Os manifests Kubernetes ficam em `k8s/`:
- `app-deployment.yaml`
- `mongo-deployment.yaml`
- `secret.yaml`
- `ingress.yaml`

Deploy manual:
- PowerShell: `./deploy/k8s-deploy.ps1 -Namespace staging`
- Bash: `./deploy/k8s-deploy.sh staging`

## Prints do funcionamento

Para capturar evidências, priorize:
- execução do workflow no GitHub Actions
- resultado dos testes
- `docker compose up`
- resposta em `http://localhost:8080`
- resposta em `http://localhost:8080/health/ready`
- pods e services no Kubernetes com `kubectl get pods,svc -n staging`
- rollout do deploy em `staging` e `production`

Salve os prints em `docs/prints/` e referencie em `docs/Presentation.md` ou `docs/Documentation.md`.

## Tecnologias utilizadas

- `.NET 8`
- `xUnit`
- `Reqnroll (BDD com Gherkin)`
- `FluentAssertions`
- `NJsonSchema`
- `MongoDB`
- `Docker`
- `Docker Compose`
- `Kubernetes`
- `GitHub Actions`
- `PowerShell` e `Bash`

## Testes automatizados (BDD + API + contrato)

O projeto de testes (`EcoTrack360.Tests`) cobre:

- Cenários BDD em Gherkin (`Features/*.feature`)
- Testes de API (status code e payload JSON)
- Testes de contrato com JSON Schema

Cenários implementados incluem caminhos:

- **Positivos (Mongo online):** readiness e seed
- **Negativos (Mongo offline):** readiness e seed com `503` + `ProblemDetails`

### Executar localmente

1. Restore e build:

`dotnet restore && dotnet build`

2. Defina a conexão de teste para o Mongo ligado (se necessário):

PowerShell:

`$env:MONGO_TEST_CONN="mongodb://localhost:27017"`

3. Execute os testes:

`dotnet test`

4. (Opcional) Gerar resultado TRX:

`dotnet test --logger "trx"`

### Arquivos principais dos testes

- `EcoTrack360.Tests/Features/SummaryAndHealth.feature`
- `EcoTrack360.Tests/Features/Readiness.feature`
- `EcoTrack360.Tests/Features/Seed.feature`
- `EcoTrack360.Tests/StepDefinitions/ApiStepDefinitions.cs`
- `EcoTrack360.Tests/ApiContractTests.cs`

## CI para testes

Além do workflow principal de CI/CD, foi adicionado um workflow dedicado para validação rápida de build + testes:

- `.github/workflows/ci.yml`

Esse workflow:

1. Faz checkout do código
2. Instala .NET 8
3. Sobe MongoDB como serviço
4. Executa restore, build e testes
5. Publica artefato de resultado (`*.trx`)

## Checklist de evidências (PDF/PPT)

- Print dos cenários `.feature`
- Print da execução local do `dotnet test`
- Print da execução do workflow no GitHub Actions
- Resultado final com total de testes aprovados

## Como gerar o pacote .zip

Execute:

`./scripts/package-delivery.ps1 -OutputPath ./ecotrack360-delivery.zip`

O pacote inclui os principais artefatos do desafio, incluindo aplicação, testes, pipeline, manifests, scripts e documentação.

## Estrutura relevante do projeto

- `EcoTrack360/` — aplicação principal
- `EcoTrack360.Tests/` — testes automatizados
- `src/` — pasta auxiliar para checklist da entrega
- `.github/workflows/` — pipeline CI/CD
- `k8s/` — manifests Kubernetes
- `deploy/` — scripts de deploy
- `docs/` — documentação e prints

