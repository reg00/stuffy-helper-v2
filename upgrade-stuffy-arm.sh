#!/bin/bash

# Скрипт для перезапуска Docker Compose с обновлением образов
sudo set -e  # Завершить скрипт при любой ошибке

sudo echo "🛑 Останавливаем Docker Compose..."
sudo docker compose down


# Удалить все dangling образы
sudo docker image prune -f

sudo echo "🔨 Собираем новые образы..."
sudo docker build -f src/StuffyHelper.Authorization.Api/Dockerfile-arm -t slavadno/stuffy-auth-arm .
sudo docker build -f src/StuffyHelper.Api/Dockerfile-arm -t slavadno/stuffy-core-arm .
sudo docker build -f src/StuffyHelper.EmailService.Api/Dockerfile-arm -t slavadno/stuffy-email-arm .
sudo docker build -f src/StuffyHelper.ApiGateway/Dockerfile-arm -t slavadno/stuffy-gateway-arm .


sudo echo "🚀 Запускаем Docker Compose..."
sudo docker compose up -d

sudo echo "✅ Готово! Контейнеры запущены с обновленными образами."
sudo echo "📋 Статус контейнеров:"
sudo docker-compose ps