# Apresentação - EcoTrack360 (Template)

---

## Slide 1 - Título
Projeto: EcoTrack360 - Cidades ESGInteligentes
Integrantes: (adicionar nomes)

---

## Slide 2 - Objetivo do projeto
- Integrar práticas de DevOps em um projeto C# (.NET 8)
- Pipeline CI/CD, containerização e orquestração

---

## Slide 3 - Arquitetura
- Aplicação: .NET 8 (EcoTrack360)
- Banco: MongoDB
- Orquestração: Docker Compose / Kubernetes
- Imagens Docker armazenadas no registry

---

## Slide 4 - Pipeline CI/CD
- Build: dotnet build
- Testes: dotnet test
- Build e push de imagem docker
- Deploy automático para `staging` e `main`

---

## Slide 5 - Dockerfile
- Multi-stage: SDK build + Runtime
- Publicação otimizada para Release

---

## Slide 6 - Kubernetes
- Manifests publicados em `k8s/`
- Deployments para app e mongo, Service, PVC e Secret
- Ingress opcional para acesso externo

---

## Slide 7 - Como executar (local)
1. Copiar `.env.example` para `.env`
2. `docker compose up --build`
3. Verificar logs: `docker compose logs -f app`

---

## Slide 8 - Deploy (staging/production)
- GitHub Actions executa build/test/push
- Deploy via SSH: copia `docker-compose.yml` e `.env` e executa `docker compose up -d`
- Kubernetes: `kubectl apply -f k8s/`

---

## Slide 9 - Evidências
- (Adicionar prints do pipeline, containers, endpoints funcionando)

---

## Slide 10 - Desafios e soluções
- Configuração de secrets e conexões
- Garantir restart controlado em deploys

---

## Slide 11 - Checklist
- Dockerfile, docker-compose, manifests, pipeline, documentação

---

## Fim
Obrigado!
