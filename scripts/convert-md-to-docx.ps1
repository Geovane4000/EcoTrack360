<#
Converte `docs/Presentation_for_ppt.md` para Word (.docx) usando pandoc.
Requisitos:
 - pandoc instalado (https://pandoc.org/installing.html)

Uso:
 PowerShell> .\scripts\convert-md-to-docx.ps1

Gera: docs\EcoTrack360_Presentation.docx
#>

param(
    [string]$Input = "docs/Presentation_for_ppt.md",
    [string]$Output = "docs/EcoTrack360_Presentation.docx"
)

if (-not (Get-Command pandoc -ErrorAction SilentlyContinue)) {
    Write-Host "pandoc não encontrado. Instale pandoc e execute novamente: https://pandoc.org/installing.html" -ForegroundColor Yellow
    exit 1
}

Write-Host "Convertendo $Input → $Output"

$pandocArgs = @(
    "-s",
    "--wrap=none",
    "-t", "docx",
    "$Input",
    "-o", "$Output"
)

$p = Start-Process -FilePath pandoc -ArgumentList $pandocArgs -NoNewWindow -Wait -PassThru
if ($p.ExitCode -eq 0) {
    Write-Host "DOCX gerado: $Output" -ForegroundColor Green
} else {
    Write-Host "Falha na conversão. Código de saída: $($p.ExitCode)" -ForegroundColor Red
}
