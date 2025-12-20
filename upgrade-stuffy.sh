#!/bin/bash

# Скрипт для перезапуска Docker Compose с обновлением образов
sudo set -e  # Завершить скрипт при любой ошибке

sudo echo "🛑 Останавливаем Docker Compose..."
sudo sudo docker compose down


# Удалить все dangling образы
sudo docker image prune -f

sudo echo "🔨 Собираем новые образы..."
sudo docker build -f src/StuffyHelper.Authorization.Api/Dockerfile -t slavadno/stuffy-auth .
sudo docker build -f src/StuffyHelper.Api/Dockerfile -t slavadno/stuffy-core .
sudo docker build -f src/StuffyHelper.EmailService.Api/Dockerfile -t slavadno/stuffy-email .
sudo docker build -f src/StuffyHelper.ApiGateway/Dockerfile -t slavadno/stuffy-gateway .


sudo echo "🚀 Запускаем Docker Compose..."
sudo docker compose up -d

sudo echo "✅ Готово! Контейнеры запущены с обновленными образами."
sudo echo "📋 Статус контейнеров:"
sudo docker-compose ps