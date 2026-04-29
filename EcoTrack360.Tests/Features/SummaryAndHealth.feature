Feature: Resumo e saúde do serviço
  Para garantir governança e disponibilidade da plataforma ESG
  Como consumidor da API
  Eu quero consultar resumo e endpoints de saúde

  Scenario: Consultar resumo da aplicação
    Given que a API do EcoTrack360 está em execução
    When eu faço uma requisição GET para "/"
    Then o status da resposta deve ser 200
    And o JSON de resposta deve conter o campo "service"
    And o JSON de resposta deve conter o campo "database"
    And o JSON de resposta deve conter o campo "connected"
    And o contrato JSON da resposta deve ser válido para "summary"

  Scenario: Consultar health liveness
    Given que a API do EcoTrack360 está em execução
    When eu faço uma requisição GET para "/health/live"
    Then o status da resposta deve ser 200
    And o JSON de resposta deve conter o campo "status"
    And o valor do campo "status" deve ser "live"
    And o contrato JSON da resposta deve ser válido para "healthLive"
