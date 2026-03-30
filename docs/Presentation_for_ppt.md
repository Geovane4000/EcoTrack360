% EcoTrack360 - Apresentação
% Integrantes: (adicione nomes)
% EcoTrack360 Team

# Slide 1 - Título

Projeto: EcoTrack360 - Cidades ESGInteligentes

Integrantes: (adicione nomes aqui)

---

# Slide 2 - Objetivo do projeto

- Integrar práticas de DevOps em um projeto C# (.NET 8)
- Pipeline CI/CD, containerização e orquestração

---

# Slide 3 - Arquitetura

- Aplicação: .NET 8 (EcoTrack360)
- Banco: MongoDB
- Orquestração: Docker Compose / Kubernetes
- Imagens Docker armazenadas no registry

**Placeholder de print:**

![Arquitetura](docs/prints/architecture.svg)

Descrição: Print da arquitetura / diagrama da solução (ex.: diagrama mostrando app, mongo, k8s/rancher)

---

# Slide 4 - Pipeline CI/CD

- Build: `dotnet build`
- Testes: `dotnet test`
- Build e push de imagem docker
- Deploy automático para `staging` e `main`

**Placeholders de prints:**

1) [INSERIR PRINT] Descrição: Print do job `build`/`test` rodando no GitHub Actions — mostre a execução completa com status "passed".

![Build e Test](docs/prints/build-test.svg)

Descrição: Print do job `build`/`test` rodando no GitHub Actions — mostre a execução completa com status "passed".

![Push de imagem](docs/prints/push-image.svg)

Descrição: Print do job de `push` de imagem para o registry (output com `docker/build-push-action`).

---

# Slide 5 - Dockerfile

- Multi-stage: SDK build + Runtime
- Publicação otimizada para Release

**Placeholder de print:**

![Docker Build](docs/prints/docker-build.svg)

Descrição: Trecho do Dockerfile ou resultado do `docker build` mostrando imagem criada com tag e tamanho.

---

# Slide 6 - Kubernetes

- Manifests publicados em `k8s/`
- Deployments para app e mongo, Service, PVC e Secret
- Ingress opcional para acesso externo

**Placeholder de prints:**

1) [INSERIR PRINT] Descrição: `kubectl apply -f k8s/` exibindo saída positiva de criação/aplicação de recursos.

![Kubectl Apply](docs/prints/kubectl-apply.svg)

Descrição: `kubectl apply -f k8s/` exibindo saída positiva de criação/aplicação de recursos.

![Pods em staging](docs/prints/pods-staging.svg)

Descrição: `kubectl get pods -n staging` mostrando pods `Running` com READY e STATUS OK.

---

# Slide 7 - Como executar (local)

1. Copiar `.env.example` para `.env`
2. `docker compose up --build`
3. Verificar logs: `docker compose logs -f app`

**Placeholder de print:**

![Docker Compose Up](docs/prints/docker-compose-up.svg)

Descrição: Terminal mostrando `docker compose up --build` e serviços sendo iniciados (app + mongo).

---

# Slide 8 - Deploy (staging/production)

- GitHub Actions executa build/test/push
- Deploy via `kubectl apply -f k8s/` usando `KUBE_CONFIG` no secret do GitHub
- Alternativa: deploy via SSH e `docker compose` em host remoto

**Placeholder de prints:**

1) [INSERIR PRINT] Descrição: GitHub Actions execução do passo de deploy para `staging` (log mostrando `kubectl apply`).

![Deploy Staging](docs/prints/deploy-staging.svg)

Descrição: GitHub Actions execução do passo de deploy para `staging` (log mostrando `kubectl apply`).

![Host remoto containers](docs/prints/remote-containers.svg)

Descrição: Shell do host remoto após `docker compose up -d` exibindo containers `Up`.

---

# Slide 9 - Evidências

- Adicione aqui os prints coletados.

Placeholders (lista):

- [INSERIR PRINT] Print do job build/test no GitHub Actions (arquivo: `docs/prints/build-test.png`)
- [INSERIR PRINT] Print do push de imagem para o registry (arquivo: `docs/prints/push-image.png`)
- [INSERIR PRINT] Print do deploy em staging via kubectl (arquivo: `docs/prints/kubectl-apply.png`)
- [INSERIR PRINT] Print do app respondendo em staging/production (arquivo: `docs/prints/app-running.png`)

- [INSERIR PRINT] Print do job build/test no GitHub Actions (arquivo: `docs/prints/build-test.svg`)
- [INSERIR PRINT] Print do push de imagem para o registry (arquivo: `docs/prints/push-image.svg`)
- [INSERIR PRINT] Print do deploy em staging via kubectl (arquivo: `docs/prints/kubectl-apply.svg`)
- [INSERIR PRINT] Print do app respondendo em staging/production (arquivo: `docs/prints/app-running.svg`)

---

# Slide 10 - Desafios e soluções

- Configuração de secrets e conexões
- Garantir restart controlado em deploys
- Uso de readiness/liveness probes para estabilidade

**Placeholder de print:**

[INSERIR PRINT] Descrição: Logs mostrando correção aplicada (ex.: variáveis de ambiente corrigidas ou pod reiniciado com sucesso).

---

# Slide 11 - Checklist

- Dockerfile, docker-compose, manifests, pipeline, documentação

**Placeholder final:**

[INSERIR PRINT] Descrição: Screenshot do checklist preenchido (CHECKLIST.md ou seção do README com todos os itens marcados).

---

# Fim

Obrigado!
