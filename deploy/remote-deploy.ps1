param(
    [Parameter(Mandatory=$true)] [string] $Remote,
    [string] $RemoteDir = '/opt/esg',
    [string] $SshKey = ''
)

# Requer OpenSSH instalado e configurado no Windows
Write-Host "Copiando arquivos para $Remote:$RemoteDir"
$scpArgs = @()
if ($SshKey -ne '') { $scpArgs += '-i'; $scpArgs += $SshKey }
$scpArgs += 'docker-compose.yml'; $scpArgs += '.env'; $scpArgs += "$Remote:`$RemoteDir/"

scp @scpArgs

Write-Host "Executando docker compose no remoto"
$sshArgs = @()
if ($SshKey -ne '') { $sshArgs += '-i'; $sshArgs += $SshKey }
$sshArgs += $Remote; $sshArgs += "cd $RemoteDir && docker compose pull && docker compose up -d --remove-orphans"
ssh @sshArgs

Write-Host "Deploy remoto concluído"