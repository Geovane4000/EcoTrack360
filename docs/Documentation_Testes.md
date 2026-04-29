# Documentação e Evidências de Execução dos Testes - EcoTrack360

## 1. Identificação

- **Projeto:** EcoTrack360
- **Disciplina:** Testes Automatizados
- **Aluno(a):** [PREENCHER]
- **Turma:** [PREENCHER]
- **Data:** [PREENCHER]

---

## 2. Objetivo

Este documento apresenta a implementação dos testes automatizados do projeto EcoTrack360, incluindo:

- cenários BDD escritos em Gherkin;
- testes de API;
- testes de contrato com JSON Schema;
- evidências de execução local e em pipeline CI.

---

## 3. Tecnologias utilizadas

- .NET 8
- xUnit
- Reqnroll (BDD com Gherkin)
- FluentAssertions
- NJsonSchema
- MongoDB
- GitHub Actions

---

## 4. Configuração do ambiente

### 4.1 Pré-requisitos

- .NET SDK 8+
- MongoDB em execução (para cenários online)
- Git

### 4.2 Comandos para execução local

```powershell
dotnet restore
dotnet build
$env:MONGO_TEST_CONN="mongodb://localhost:27017"
dotnet test --logger "trx"
```

**Print da configuração/execução local:**

[INSERIR PRINT AQUI - Terminal com comandos e testes executados]

---

## 5. Cenários BDD (Gherkin)

Arquivos de cenários:

- `EcoTrack360.Tests/Features/SummaryAndHealth.feature`
- `EcoTrack360.Tests/Features/Readiness.feature`
- `EcoTrack360.Tests/Features/Seed.feature`

### 5.1 Cenário BDD 1 - Consulta de resumo da aplicação (positivo)

- **Given** a API está em execução
- **When** GET `/`
- **Then** status 200 e JSON com campos obrigatórios (`service`, `database`, `connected`)

**Print do arquivo Gherkin (cenário 1):**

[INSERIR PRINT AQUI]

### 5.2 Cenário BDD 2 - Readiness com Mongo disponível (positivo)

- **Given** Mongo disponível
- **When** GET `/health/ready`
- **Then** status 200, `status = ready`, contrato válido

**Print do arquivo Gherkin (cenário 2):**

[INSERIR PRINT AQUI]

### 5.3 Cenário BDD 3 - Readiness com Mongo indisponível (negativo)

- **Given** Mongo indisponível
- **When** GET `/health/ready`
- **Then** status 503 e resposta em formato `ProblemDetails`

**Print do arquivo Gherkin (cenário 3):**

[INSERIR PRINT AQUI]

### 5.4 Cenário BDD 4 - Seed com Mongo disponível (positivo)

- **When** POST `/seed`
- **Then** status 200 e JSON com `seeded` e `database`

**Print do arquivo Gherkin (cenário 4):**

[INSERIR PRINT AQUI]

### 5.5 Cenário BDD 5 - Seed com Mongo indisponível (negativo)

- **When** POST `/seed`
- **Then** status 503 e resposta `ProblemDetails`

**Print do arquivo Gherkin (cenário 5):**

[INSERIR PRINT AQUI]

---

## 6. Testes de API e contrato (JSON Schema)

Arquivo principal:

- `EcoTrack360.Tests/ApiContractTests.cs`

Validações implementadas:

1. **Status code**
2. **Body JSON**
3. **Contrato com JSON Schema**

APIs cobertas:

- `GET /`
- `GET /health`
- `GET /health/live`
- `GET /health/ready` (online e offline)
- `POST /seed` (online e offline)

**Print do código dos testes de API/contrato:**

[INSERIR PRINT AQUI]

---

## 7. Evidências de execução dos testes

### 7.1 Execução local

Resultado esperado (exemplo):

- Build concluída com sucesso
- Testes executados com sucesso (ex.: 14/14 aprovados)

**Print da execução local (`dotnet test`):**

[INSERIR PRINT AQUI]

### 7.2 Execução em CI/CD (GitHub Actions)

Workflow:

- `.github/workflows/ci.yml`

Etapas:

- checkout
- setup .NET
- restore
- build
- test
- upload de artefatos TRX

**Print do pipeline no GitHub Actions:**

[INSERIR PRINT AQUI]

---

## 8. Resultado final

- **Quantidade de cenários BDD:** [PREENCHER]
- **Quantidade total de testes executados:** [PREENCHER]
- **Quantidade de testes aprovados:** [PREENCHER]
- **Quantidade de testes com falha:** [PREENCHER]

---

## 9. Conclusão

A atividade foi concluída com a implementação de testes automatizados BDD, testes de API e testes de contrato, cobrindo cenários positivos e negativos das funcionalidades principais da aplicação e garantindo maior qualidade e estabilidade do sistema.

---

## 10. Checklist de entrega

- [ ] Projeto compactado em `.zip`
- [ ] Arquivos `.feature` com cenários BDD
- [ ] Testes de API com validação de status e JSON
- [ ] Testes de contrato com JSON Schema
- [ ] README com instruções de execução
- [ ] PDF com descrição + prints de evidência
