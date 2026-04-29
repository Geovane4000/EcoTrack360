Feature: Seed de dados ESG
  Para garantir consistência de dados iniciais
  Como operador da aplicação
  Eu quero executar o endpoint de seed

  Scenario: Executar seed quando Mongo está disponível
    Given que a API do EcoTrack360 está em execução
    And que o MongoDB está disponível para a aplicação
    When eu faço uma requisição POST para "/seed"
    Then o status da resposta deve ser 200
    And o JSON de resposta deve conter o campo "seeded"
    And o JSON de resposta deve conter o campo "database"
    And o contrato JSON da resposta deve ser válido para "seed"

  Scenario: Executar seed quando Mongo está indisponível
    Given que a API do EcoTrack360 está em execução
    And que o MongoDB está indisponível para a aplicação
    When eu faço uma requisição POST para "/seed"
    Then o status da resposta deve ser 503
    And a resposta deve estar no formato problem details
