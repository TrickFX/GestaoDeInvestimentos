
# API de Gestão de Investimentos

Esta é uma API de Gestão de Investimentos, permitindo que os usuários da operação gerenciem os investimentos disponíveis e que os clientes possam comprar, vender e acompanhar seus investimentos. Esta API oferece endpoints seguros para realizar operações relacionadas a investimentos e transações, além de autenticação baseada em tokens JWT (JSON Web Tokens).

## Tecnologias Utilizadas

- .NET Core 8
- Entity Framework Core
- SQL Server
- Docker
- MailKit para envio de emails
- Swagger para documentação da API
- Scalar

## Requisitos

- .NET Core SDK 8.0 ou superior
- Docker (para execução em contêiner)
- SQL Server

## Estrutura do Projeto

- **Core**: Contém as entidades e interfaces de repositório.
- **Infrastructure**: Implementação dos repositórios e contexto do Entity Framework.
- **Authentication**: Implementação do serviço de autenticação e geração de tokens.
- **WebGestao**: Projeto principal da API.
- **EmailService**: Serviço de envio de emails diários.

## Passo a Passo para Utilização

### 1. Configuração sem Docker

#### 1.1. Clone o Repositório

```sh
git clone https://github.com/TrickFX/GestaoDeInvestimentos.git
cd GestaoDeInvestimentos
```

#### 1.2. Configure o Banco de Dados

Crie um banco de dados no SQL Server e atualize a string de conexão no arquivo `appsettings.json` localizado no projeto `WebGestao` e `EmailService`.
Caso quiser, também pode alterar a tag SecretJWT conforme a necessidade.

```json
"SecretJWT": "WFAgSW52ZXN0aW1lbnRvcyBTRUNSRVQgSldUIGRlIFRlc3RlLg==",
"ConnectionStrings": {
  "ConnectionString": "Server=localhost;Database=Gestao;User Id=sa;Password=YourStrong!Passw0rd;"
}
```

#### 1.3. Aplique as Migrations

No terminal, navegue até a pasta do projeto `WebGestao` e execute o seguinte comando:

```sh
dotnet ef database update --startup-project WebGestao --project Infrastructure
```

#### 1.4. Execute a Aplicação

Ainda na pasta do projeto `WebGestao`, execute:

```sh
dotnet run
```

A aplicação estará disponível em `http://localhost:5016/swagger`.

![Logo](https://i.ibb.co/xYWHh1B/API-Swagger.png)


Vale ressaltar que a documentação do _Scalar_ também foi implementada no projeto, sendo acessível pela URL: `http://localhost:5016/api-docs`

![Logo](https://i.ibb.co/RBvCc5Q/API-Scalar.png)

### 2. Configuração com Docker

#### 2.1. Clone o Repositório

```sh
git clone https://github.com/TrickFX/GestaoDeInvestimentos.git
cd GestaoDeInvestimentos
```
#### 2.2. Crie a imagem no docker

No diretório raiz do projeto (onde se encontra o docker-compose), digite o seguinte comando `docker-compose build`.
Após a finalização, o mesmo gerará uma imagem da aplicação desenvolvida:

![Logo](https://i.ibb.co/FbDwYBn/docker-imagem.png)

#### 2.3. Construa e Execute o Contêiner Docker

No terminal, navegue até o diretório raiz do projeto e execute o comando:

```sh
docker-compose up
```

Após o término, os containers serão geradas e a aplicação estará disponível na URL `http://localhost:8080/swagger`:

![Logo](https://i.ibb.co/6XJSyXc/container.png)
