# Family Budget 🏦💰

<!-- Family Budget Management -->
Gerenciamento de orçamento familiar.

<!-- ## Introduction 📖 -->
## Introdução 📖

<!-- Welcome to Family Budget, a family budgeting app! With this app you can control your monthly expenses and income in a simple and easy way. -->
Bem-vindo ao Family Budget, um aplicativo de orçamento familiar! Com este aplicativo você pode controlar suas despesas e receitas mensais de uma maneira simples e fácil.

<!-- ## Technologies Used 🚀 -->
## Tecnologias Utilizadas 🚀

<!-- Family Budget was developed using the following technologies: -->
O Family Budget foi desenvolvido usando as seguintes tecnologias:

- C# 11
- .NET 7
- Entity Framework
- SqlServer
- Docker
- Swagger

<!-- ## Features 📋 -->
## Funcionalidades 📋

<!-- The application is divided into 2 services: -->
O aplicativo é dividido em 2 serviços:

<!-- - Budget: webservice that provides the CRUD of expenses and income, in addition to providing a monthly summary. -->
- Budget: webservice que disponibiliza o CRUD de despesas e receitas, além de disponibilizar um resumo mensal.
<!-- - Identity: webservice that centralizes authentication using the Identity Server. The Budget uses the Identity to perform authentication. -->
- Identity: webservice que centraliza a autenticação utilizando o Identity Server. O Budget utiliza o Identity para realizar a autenticação.

<!-- ## Requirements 📋 -->
## Requisitos 📋

<!-- To run the application, you must have the following tools installed: -->
Para executar o aplicativo, você deve ter as seguintes ferramentas instaladas:

- [Docker](https://www.docker.com/products/docker-desktop)

<!-- ## How to Run 🏃‍♂️ -->
## Como Executar 🏃‍♂️

<!-- Clone the repository: `git clone https://github.com/brunoaragao/FamilyBudget.git` -->
Clone o repositório: `git clone https://github.com/brunoaragao/FamilyBudget.git`

<!-- Open the terminal in the Docker folder and run the command `docker-compose up -d`, this will start the services in the background. -->
Abra o terminal na pasta Docker e execute o comando `docker-compose up -d`, isso irá iniciar os serviços em segundo plano.

<!-- ## How to Use 📚 -->
## Como Utilizar 📚

<!-- To use the application, you must open the swagger documentation through the link http://localhost:5000/swagger/index.html. Click on the `Authorize` button and use the default credentials: -->
Para utilizar o aplicativo, você deve abrir a documentação do swagger através do link http://localhost:5000/swagger/index.html. Clique no botão `Authorize` e utilize as credenciais padrão:

- client_id: `budget-swagger`
- secret: `secret`

<!-- After that, you can use the swagger documentation to test the application. -->
Após isso, você pode utilizar a documentação do swagger para testar o aplicativo.

<img src="https://github.com/brunoaragao/FamilyBudget/blob/main/img/budget-swagger-doc.png">

<!-- ## License 📝 -->
## Licença 📝

<!-- This project is under the MIT license. See the [LICENSE](LICENSE) file for more details. -->
Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

<!-- ## Acknowledgments -->
## Agradecimentos

<!-- This project was developed as part of the [challenge](https://www.alura.com.br/challenges/back-end-2) proposed by Alura. -->
Este projeto foi desenvolvido como parte do [desafio](https://www.alura.com.br/challenges/back-end-2) proposto pela Alura.
