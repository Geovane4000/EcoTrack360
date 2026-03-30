param(
    [string]$Namespace = 'default'
)

Write-Host "Aplicando manifests no namespace: $Namespace"
kubectl apply -n $Namespace -f k8s/

Write-Host "Restart de deployment ecotrack360-app (opcional)"
kubectl rollout restart deployment/ecotrack360-app -n $Namespace
kubectl rollout status deployment/ecotrack360-app -n $Namespace -w -t 120

Write-Host "Deploy concluído"