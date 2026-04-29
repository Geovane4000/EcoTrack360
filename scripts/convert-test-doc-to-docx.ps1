<#
Converte `docs/Documentation_Testes.md` para Word (.docx) usando pandoc.

Requisitos:
 - pandoc instalado (https://pandoc.org/installing.html)

Uso:
 PowerShell> .\scripts\convert-test-doc-to-docx.ps1

Saída padrão:
 docs\Documentation_Testes.docx
#>

param(
    [string]$InputFile = "docs/Documentation_Testes.md",
    [string]$OutputFile = "docs/Documentation_Testes.docx"
)

if ([string]::IsNullOrWhiteSpace($InputFile)) {
    Write-Host "Parâmetro InputFile vazio." -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $InputFile)) {
    Write-Host "Arquivo de entrada não encontrado: $InputFile" -ForegroundColor Red
    exit 1
}

if (-not (Get-Command pandoc -ErrorAction SilentlyContinue)) {
    Write-Host "pandoc não encontrado. Instale pandoc e execute novamente: https://pandoc.org/installing.html" -ForegroundColor Yellow
    exit 1
}

Write-Host "Convertendo $InputFile -> $OutputFile"

$pandocArgs = @(
    "-s",
    "--wrap=none",
    "-t", "docx",
    "$InputFile",
    "-o", "$OutputFile"
)

$p = Start-Process -FilePath pandoc -ArgumentList $pandocArgs -NoNewWindow -Wait -PassThru
if ($p.ExitCode -eq 0) {
    Write-Host "DOCX gerado: $OutputFile" -ForegroundColor Green
} else {
    Write-Host "Falha na conversão. Código de saída: $($p.ExitCode)" -ForegroundColor Red
}
