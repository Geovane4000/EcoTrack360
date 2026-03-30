param(
    [Parameter(Mandatory=$true)] [string] $Remote,
    [string] $RemoteDir = '/opt/esg',
    [string] $SshKey = ''
)

# Requer OpenSSH instalado e configurado no Windows
Write-Host "Copiando arquivos para $Remote:$RemoteDir"
$sshArgs = @()
$scpArgs = @()

if ($SshKey -ne '') {
    $scpArgs += '-i'
    $scpArgs += $SshKey
    $sshArgs += '-i'
    $sshArgs += $SshKey
}

ssh @sshArgs $Remote "mkdir -p $RemoteDir"

$scpArgs += 'docker-compose.yml'
$scpArgs += '.env'
$scpArgs += "$($Remote):$RemoteDir/"
scp @scpArgs

Write-Host "Executando docker compose no remoto"
$sshArgs += $Remote
$sshArgs += "cd $RemoteDir && docker compose pull && docker compose up -d --remove-orphans"
ssh @sshArgs

Write-Host "Deploy remoto concluído"