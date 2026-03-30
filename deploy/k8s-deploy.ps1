param(
    [string]$Namespace = 'default'
)

Write-Host "Aplicando manifests no namespace: $Namespace"
kubectl get namespace $Namespace *> $null
if ($LASTEXITCODE -ne 0) {
    kubectl create namespace $Namespace
}

kubectl apply -n $Namespace -f k8s/

Write-Host "Restart de deployment ecotrack360-app (opcional)"
kubectl rollout restart deployment/ecotrack360-app -n $Namespace
kubectl rollout status deployment/ecotrack360-app -n $Namespace --timeout=120s

Write-Host "Deploy concluído"