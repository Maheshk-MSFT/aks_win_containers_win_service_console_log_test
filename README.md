1) Build the project
2) Setup AKS
3) Push the container to ACR
4) Host this container on Windows nodes; enable container insights
5) useful commands
```
    mikkyacr1.azurecr.io
    az acr login --name mikkyacr1.azurecr.io
    docker image tag mikkywinsvc1:latest mikkyacr1.azurecr.io/mikkywinsvc1:latest
    docker push mikkyacr1.azurecr.io/mikkywinsvc1:latest
    kubectl exec -it mikkywinsvc-798dc868f7-p8g7s -- cmd  <--- this is to check the files are getting created but there is no console logs going elsewhere as expected
```

# docker file used for building this container

```
# Use the official Microsoft Windows Server Core image for Windows Server 2022
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8-windowsservercore-ltsc2022

# Set the working directory
WORKDIR /app

# Copy the service executable and other necessary files
COPY bin/Release/ /app

# Install the service
RUN powershell -Command \
    New-Service -Name "Service1" -BinaryPathName "C:\app\WindowsService3Mikky.exe" -DisplayName "Service1" -StartupType Automatic; \
    Start-Service -Name "Service1"

# Expose any ports the service uses (if applicable)
# EXPOSE 1234

# Define the entry point for the container
ENTRYPOINT ["powershell", "-Command", "Start-Service -Name Service1; Wait-Event -Timeout 2147483647"]

```

# windows service yaml
```
apiVersion: apps/v1
kind: Deployment
metadata:
  name: mikkywinsvc
  labels:
    app: mikkywinsvc
spec:
  replicas: 1
  template:
    metadata:
      name: mikkywinsvc
      labels:
        app: mikkywinsvc
    spec:
      nodeSelector:
        "kubernetes.io/os": windows
      containers:
      - name: mikkywinsvc
        image: mikkyacr1.azurecr.io/mikkywinsvc1:latest
        resources:
          limits:
            cpu: 1
            memory: 800M
        ports:
          - containerPort: 80
  selector:
    matchLabels:
      app: mikkywinsvc
```

