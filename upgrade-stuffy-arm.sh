#!/bin/bash

# Скрипт для перезапуска Docker Compose с обновлением образов
set -e  # Завершить скрипт при любой ошибке

echo "🛑 Останавливаем Docker Compose..."
sudo docker compose down


# Удалить все dangling образы
docker image prune -f

echo "🔨 Собираем новые образы..."
sudo docker build -f src/StuffyHelper.Authorization.Api/Dockerfile-arm -t slavadno/stuffy-auth-arm .
sudo docker build -f src/StuffyHelper.Api/Dockerfile-arm -t slavadno/stuffy-core-arm .
sudo docker build -f src/StuffyHelper.EmailService.Api/Dockerfile-arm -t slavadno/stuffy-email-arm .
sudo docker build -f src/StuffyHelper.ApiGateway/Dockerfile-arm -t slavadno/stuffy-gateway-arm .


echo "🚀 Запускаем Docker Compose..."
sudo docker compose up -d

echo "✅ Готово! Контейнеры запущены с обновленными образами."
echo "📋 Статус контейнеров:"
sudo docker-compose ps