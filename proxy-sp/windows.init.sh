#!/bin/sh
echo "Ingrese usuario de sharepoint"
read -p 'Usuario: ' usuario
echo "Ingrese password para el usuario"
read -sp 'Password:' password
echo "Ingrese el puerto de escucha (opcional)"
read -p 'Puerto: ' port
port=${port:-3000}
echo Iniciando el servidor con el usuario $usuario en el puerto $port

node server.js _user_$usuario _pass_$password _port_$port