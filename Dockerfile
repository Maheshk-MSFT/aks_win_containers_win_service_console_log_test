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
