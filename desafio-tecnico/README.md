# Desafio Técnico - Sistema de Gestão de Departamentos e Colaboradores

Sistema web desenvolvido em ASP.NET Core MVC com Razor Pages para gerenciamento de departamentos e colaboradores.

## 📋 Pré-requisitos

- Docker e Docker Compose
- .NET 8.0 SDK
- Git (opcional)

## 🚀 Como Executar

### 1. Subir o banco de dados PostgreSQL

```bash
docker-compose up -d
```

### 2. Restaurar dependências

```bash
dotnet restore
```

### 3. Aplicar migrations

```bash
dotnet ef database update
```

### 4. Executar o projeto

```bash
dotnet run
```


## 🛠️ Comandos Úteis

### Docker

```bash
# Parar containers
docker-compose down

# Parar e remover volumes (limpar dados)
docker-compose down -v

# Ver logs do PostgreSQL
docker-compose logs postgres
```

### Entity Framework

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Reverter última migration
dotnet ef migrations remove

# Listar migrations
dotnet ef migrations list
```

### Instalar ferramentas EF Core (se necessário)

```bash
dotnet tool install --global dotnet-ef
```

## 📁 Estrutura do Projeto

```
desafio-tecnico/
├── Controllers/          # Controllers MVC e API
├── Data/                 # DbContext e configurações
├── Migrations/           # Migrations do Entity Framework
├── Models/               # Entidades (Employee, Departament)
├── Services/             # Lógica de negócio
├── ViewModels/           # DTOs e ViewModels
├── Views/                # Views Razor
└── wwwroot/              # Arquivos estáticos (CSS, JS)
```

## 🔧 Configuração

A string de conexão está configurada em `appsettings.json`:

```
Server=localhost;Database=postgres;Username=postgres;Password=postgres;Port=5432;
```

## 🐛 Troubleshooting

**Erro de conexão ao aplicar migrations:**
- Aguarde alguns segundos após subir o Docker
- Verifique se o container está rodando: `docker ps`

**Porta 5432 já em uso:**
- Pare outros serviços PostgreSQL ou altere a porta no `docker-compose.yml`

**Ferramenta `dotnet ef` não encontrada:**
```bash
dotnet tool install --global dotnet-ef
```

## 📝 Funcionalidades

- ✅ CRUD de Departamentos
- ✅ CRUD de Colaboradores
- ✅ Soft Delete
- ✅ Paginação
- ✅ Filtros avançados
- ✅ Árvore hierárquica de departamentos
- ✅ Busca de colaboradores por gerente
- ✅ Validações de negócio

## 🔗 Endpoints da API

### Departamentos
- `GET /api/departaments` - Listar departamentos (com filtros)
- `GET /api/departaments/{id}` - Buscar departamento por ID
- `POST /api/departaments` - Criar departamento
- `PUT /api/departaments/{id}` - Atualizar departamento
- `DELETE /api/departaments/{id}` - Deletar departamento (soft delete)

### Colaboradores
- `GET /api/employees` - Listar colaboradores (com filtros)
- `GET /api/employees/{id}` - Buscar colaborador por ID
- `GET /api/employees/by-manager/{managerId}` - Buscar colaboradores por gerente
- `POST /api/employees` - Criar colaborador
- `PUT /api/employees/{id}` - Atualizar colaborador
- `DELETE /api/employees/{id}` - Deletar colaborador (soft delete)

## 📄 Licença

Este projeto foi desenvolvido como parte de um desafio técnico.
