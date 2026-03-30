Param(
    [Parameter(Mandatory=$true)][string]$Source,
    [string]$Name = $(Split-Path $Source -Leaf),
    [string]$CommitMessage = $("Add print image: $Name"),
    [string]$Branch = "master",
    [string]$DestinationDir = "docs\prints"
)

# Resolve paths
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$RepoRoot = Resolve-Path (Join-Path $ScriptDir "..")

try {
    $FullSource = Resolve-Path $Source -ErrorAction Stop
} catch {
    Write-Error "Arquivo de origem não encontrado: $Source. Verifique o caminho e tente novamente."
    exit 1
}

$DestDirFull = Join-Path $RepoRoot $DestinationDir

# Create destination directory if needed
if (-not (Test-Path $DestDirFull)) { New-Item -ItemType Directory -Force -Path $DestDirFull | Out-Null }

$DestPath = Join-Path $DestDirFull $Name
Copy-Item $FullSource -Destination $DestPath -Force
Write-Output "Imagem copiada para: $DestPath"

# Check for git
$gitCmd = Get-Command git -ErrorAction SilentlyContinue
if (-not $gitCmd) {
    Write-Warning "Git não encontrado no PATH. Apenas a cópia do arquivo foi realizada."
    Write-Output "Para commitar e enviar, execute na raiz do repositório:"
    Write-Output "  git add -- '$DestinationDir/$Name'"
    Write-Output "  git commit -m '$CommitMessage'"
    Write-Output "  git push origin $Branch"
    exit 0
}

# Commit and push to git
Push-Location $RepoRoot
try {
    git add -- "$DestPath"
    $commitResult = git commit -m "$CommitMessage" --quiet 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Output "Nenhum commit criado (talvez já exista um arquivo idêntico). Saída do git: $commitResult"
    } else {
        git push origin $Branch
        Write-Output "Arquivo comitado e enviado para origin/$Branch"
    }
} catch {
    Write-Error "Erro ao commitar/enviar: $_"
} finally {
    Pop-Location
}
