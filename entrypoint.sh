#!/bin/bash

# Aguardar 1 minuto
echo "Aguardando 1 minuto para o SQL Server iniciar..."
sleep 60

echo "Iniciando a aplicação"
exec dotnet WebGestao.dll
