
# Documentação - EcoTrack360 DevOps

Este documento serve como base e roteiro para gerar o material de entrega (PDF ou PPT) exigido pelo desafio.

## 1. Título e integrantes
- Projeto: EcoTrack360 - Cidades ESGInteligentes
- Integrantes: (adicione nomes aqui)

## 2. Objetivo do exercício

Integrar práticas completas de DevOps ao projeto C# existente, automatizando o ciclo de vida da aplicação: build, testes, containerização, orquestração e deploy em ambientes `staging` e `production`.

### Principais objetivos
- Criar pipeline funcional de CI/CD.
- Containerizar a aplicação com `Docker`.
- Orquestrar serviços com `docker-compose` ou `Kubernetes`.
- Entregar código, documentação técnica e evidências (prints/logs).

## 3. Requisitos e escopo

1) Pipeline de CI/CD

- Ferramenta recomendada (projeto já contém): `GitHub Actions` (arquivo: `.github/workflows/ci-cd.yml`).
- Etapas mínimas do pipeline:
  - Build automático (`dotnet build`).
  - Execução de testes (`dotnet test`).
  - Build e push de imagem Docker (para registry: DockerHub / GitHub Packages / Azure ACR).
  - Deploy automatizado para `staging` e `production` (p. ex. via scripts de deploy, `kubectl` ou Docker Compose remoto).

2) Containerização e orquestração

- `Dockerfile`: multi-stage para produzir imagem runtime leve (ex.: `mcr.microsoft.com/dotnet/aspnet:8.0`).
- Orquestração: `docker-compose.yml` (local / staging) ou manifests em `k8s/` para Kubernetes (staging/production).
- Uso de volumes, variáveis de ambiente e redes para compor o ambiente (ex.: `MONGO_URI`, `ASPNETCORE_ENVIRONMENT`).

## 4. Comandos úteis

- Build e testes locais:
  - `dotnet restore`
  - `dotnet build --configuration Release`
  - `dotnet test` (executa testes automatizados presentes em `EcoTrack360.Tests`)
- Docker (build / run):
  - `docker build -t ecotrack360:latest -f Dockerfile .`
  - `docker run --env-file .env --rm -p 5000:80 ecotrack360:latest`
- Docker Compose (local):
  - `docker compose up --build`
  - `docker compose up -d --build`
  - `docker compose down`

## 5. Estrutura mínima do pacote (.zip) para entrega

Seu pacote compactado deve seguir a estrutura mínima abaixo:

```
seu-projeto/
├── Dockerfile
├── docker-compose.yml    (ou pasta `k8s/` com manifests)
├── src/                  (código-fonte ou pasta do projeto)
├── README.md
└── .github/workflows/    (arquivos de pipeline, ex: `ci-cd.yml`)
```

Inclua também:
- `.env.example`
- scripts de deploy (`deploy/`)
- prints ou logs em `docs/prints/` (opcional)

## 6. README.md (modelo / seções obrigatórias)

No `README.md` do projeto inclua as seções abaixo (resuma aqui e mantenha instruções completas no arquivo real):

- `# Projeto - Cidades ESGInteligentes`
- `## Como executar localmente com Docker`
  - Passos para usar `docker compose` ou `docker run` (exemplos de comandos acima).
- `## Pipeline CI/CD`
  - Ferramenta utilizada (ex.: `GitHub Actions`).
  - Descrição das etapas (build, test, docker build/push, deploy staging, deploy production).
  - Indicação de secrets necessários (ex.: `REGISTRY_USERNAME`, `REGISTRY_TOKEN`, `KUBE_CONFIG`, `SSH_KEY`).
- `## Containerização`
  - Explicar `Dockerfile` (multi-stage) e imagens base utilizadas.
- `## Prints do funcionamento`
  - Inserir imagens que comprovem build, testes e deploys.
- `## Tecnologias utilizadas`
  - .NET 8, MongoDB, Docker, GitHub Actions, Kubernetes (opcional), etc.

## 7. Documentação (PDF ou PPT)

O material para submissão (PDF ou PPT) deve conter:

- Título do projeto e lista de integrantes.
- Descrição do pipeline (ferramenta, etapas e lógica).
- Docker: arquitetura adotada, comandos e imagem gerada.
- Prints do pipeline rodando (build/test/push/deploy).
- Prints dos ambientes `staging` e `production` funcionando.
- Desafios encontrados e como foram resolvidos.

Sugestão: gere o PDF a partir do `docs/` ou exporte o `Presentation.md` para PPT/PDF.

## 8. Checklist de entrega (preencher e anexar no final do PDF/README)

Item | OK
:----|:--:
Projeto compactado em .ZIP com estrutura organizada | ☐
`Dockerfile` funcional | ☐
`docker-compose.yml` ou arquivos Kubernetes | ☐
Pipeline com etapas de build, teste e deploy | ☐
`README.md` com instruções e prints | ☐
Documentação técnica com evidências (PDF ou PPT) | ☐
Deploy realizado nos ambientes staging e production | ☐

---

## 9. Segredos e configuração (exemplos)

- `.github/workflows/ci-cd.yml` deve referenciar secrets do repositório:
  - `REGISTRY_USERNAME`
  - `REGISTRY_PASSWORD` (ou `REGISTRY_TOKEN`)
  - `KUBE_CONFIG` (caso faça deploy para Kubernetes)
  - `SSH_PRIVATE_KEY` (caso use SSH para deploy remoto)

## 10. Evidências/prints sugeridos

- Print do job `build`/`test` no GitHub Actions.
- Print do job de `push` de imagem para o registry.
- Print/URLs do deploy em `staging` e `production` (captura de tela da aplicação respondendo).

---

Adicione neste arquivo (ou no README) os links/paths das evidências e os nomes dos integrantes antes de gerar o PDF/PPT final.

--

Implementações feitas no repositório nesta tarefa:

- Atualizado `.github/workflows/ci-cd.yml` para suportar build/test, build/push e deploy automático para `staging` e `production` via `KUBE_CONFIG`.
- `Dockerfile` multi-stage já presente e verificado.
- `docker-compose.yml` já configurado com `mongo` e `app`.
- Manifests Kubernetes em `k8s/` (Deployment, Service, PVC, Secret, Ingress) atualizados para usar `imagePullPolicy` e variáveis.
- Scripts de deploy: `deploy/k8s-deploy.sh`, `deploy/k8s-deploy.ps1`, `deploy/remote-deploy.sh`, `deploy/remote-deploy.ps1`.
- `README.md` atualizado com instruções.
- `scripts/package-delivery.ps1` adicionado para gerar o ZIP de entrega.

Próximos passos recomendados antes da submissão:

1. Preencher nomes dos integrantes em `docs/Presentation.md` e `docs/Documentation.md`.
2. Criar e configurar secrets no repositório GitHub: `DOCKER_REGISTRY`, `DOCKER_USERNAME`, `DOCKER_PASSWORD`, `DOCKER_IMAGE`, `KUBE_CONFIG`.
3. Gerar prints das execuções (pipeline, deploys, app rodando) e adicioná-los em `docs/prints/`.
4. Executar `.	emplates\scripts\package-delivery.ps1` ou `scripts/package-delivery.ps1` para gerar o `.zip` final.

Se quiser, posso agora:

- Gerar `docs/prints/` com templates de imagem placeholder.
- Preencher o `docs/Presentation.md` com nomes e screenshots se você fornecer as imagens.
- Ajustar `k8s/app-deployment.yaml` para aceitar `imagePullSecrets` caso use registry privado.
- Executar a build local e testes e reportar qualquer falha adicional.

