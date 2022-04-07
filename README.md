# Introduction 
Projeto Loss Control

# Getting Started
1.	Requisitos
  - AspNet Core 6
  - MySql Server - (alterar a connectionString em appsettings.json)
  - Dotnet ef tools para executar as migrations ( dotnet tool install --global dotnet-ef )

2.	Processo de Instalação
  - Para rodar as migrations é necessário a instalação das ferramentas do Entity Framework: 
  - dotnet tool install --global dotnet-ef
  Em seguida use:
    - dotnet ef database update -p Infrastructure -s Api --context ContextBase

  - Para publicar o projeto e executar o build, acesse a pasta Api e execute:
    dotnet publish -c Release -o <PASTA_DE_DESTINO>
    
3.	Api references
  - Esse projeto utiliza o Swagger para gerar o documentação da Api.
    Para salvar a collection e executar via Postman, acesse: 
      https://{URL_DE_EXECUÇÃO}/swagger/v1/swagger.json

