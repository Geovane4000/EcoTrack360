Feature: Readiness do serviço
  Para garantir estabilidade operacional e rastreabilidade
  Como consumidor da API
  Eu quero validar o comportamento de prontidão

  Scenario: Consultar readiness quando Mongo está indisponível
    Given que a API do EcoTrack360 está em execução
    And que o MongoDB está indisponível para a aplicação
    When eu faço uma requisição GET para "/health/ready"
    Then o status da resposta deve ser 503
    And a resposta deve estar no formato problem details

  Scenario: Consultar readiness quando Mongo está disponível
    Given que a API do EcoTrack360 está em execução
    And que o MongoDB está disponível para a aplicação
    When eu faço uma requisição GET para "/health/ready"
    Then o status da resposta deve ser 200
    And o valor do campo "status" deve ser "ready"
    And o contrato JSON da resposta deve ser válido para "ready"
