# Makefile en la raíz del repo
# Variables
SVC_API      := Arquetipo.Api
IMAGE_API    := arquetipo-api
COMPOSE_FILE := docker-compose.yml

# Targets por defecto
.DEFAULT_GOAL := help
.PHONY: help build build-api up down logs test clean

## help:              Muestra esta ayuda
help:
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | \
	awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-15s\033[0m %s\n", $$1, $$2}'

## build:             Compila todas las imágenes (API + SQL)
build: build-api 

## build-api:         Construye la imagen de la API .NET 8
build-api:
	docker build \
	  -f $(SVC_API)/Dockerfile \
	  -t $(IMAGE_API):latest \
	  $(SVC_API)

## up:                Levanta todos los servicios con Docker Compose
up:
	docker-compose -f $(COMPOSE_FILE) up -d

## down:              Baja todos los servicios
down:
	docker-compose -f $(COMPOSE_FILE) down

## logs:              Sigue los logs de todos los servicios
logs:
	docker-compose -f $(COMPOSE_FILE) logs -f

## test:              Ejecuta todos los tests (asumiendo carpetas de test en la raíz)
test:
	dotnet test Arquetipo.Tests --logger "console;verbosity=minimal"

## clean:             Limpia contenedores, imágenes intermedias y volúmenes
clean:
	docker-compose -f $(COMPOSE_FILE) down --rmi all --volumes --remove-orphans