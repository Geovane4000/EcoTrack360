<#
Script para converter Markdown da apresentação em PPTX usando pandoc.
Requisitos locais:
 - pandoc (https://pandoc.org/installing.html)
 - opcional: imagem(ens) em `docs/prints/` para substituir placeholders

Uso:
  PowerShell> .\scripts\convert-md-to-pptx.ps1

O script gera: docs\EcoTrack360_Presentation.pptx
#>

param(
    [string]$Input = "docs/Presentation_for_ppt.md",
    [string]$Output = "docs/EcoTrack360_Presentation.pptx"
)

if (-not (Get-Command pandoc -ErrorAction SilentlyContinue)) {
    Write-Host "pandoc não encontrado. Instale pandoc e execute novamente. https://pandoc.org/installing.html" -ForegroundColor Yellow
    exit 1
}

Write-Host "Convertendo $Input → $Output"

# Use slide level 1 as slide separator
$pandocArgs = @(
    "-s",
    "--slide-level=1",
    "-t", "pptx",
    "$Input",
    "-o", "$Output"
)

# Execute pandoc
$p = Start-Process -FilePath pandoc -ArgumentList $pandocArgs -NoNewWindow -Wait -PassThru
if ($p.ExitCode -eq 0) {
    Write-Host "PPTX gerado: $Output" -ForegroundColor Green
} else {
    Write-Host "Falha na conversão. Código de saída: $($p.ExitCode)" -ForegroundColor Red
}
