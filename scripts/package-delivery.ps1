param(
    [string]$OutputPath = "ecotrack360-delivery.zip"
)

$root = Resolve-Path (Join-Path (Split-Path -Parent $MyInvocation.MyCommand.Path) "..")
Set-Location $root

$items = @(
    "Dockerfile",
    "docker-compose.yml",
    "k8s",
    "src",
    "EcoTrack360",
    "EcoTrack360.Tests",
    ".github",
    "README.md",
    ".env.example",
    "deploy",
    "docs",
    "CHECKLIST.md"
) | Where-Object { Test-Path $_ }

Write-Host "Gerando pacote: $OutputPath"

if (Test-Path $OutputPath) { Remove-Item $OutputPath }

Compress-Archive -Path $items -DestinationPath $OutputPath -Force

Write-Host "Pacote gerado: $OutputPath"
