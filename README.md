# WINDOWS SVC YAML
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

