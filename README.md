# Budget 🏦💰

Budget - Family Budget Management

## Introduction 📖

Welcome to Budget, a family budgeting app! With this app you can control your monthly expenses and income in a simple and easy way.

## Technologies Used 🚀

Budget was developed using the following technologies:

- C# 11
- .NET 7
- ASP.NET
- Entity Framework Core
- SqlServer 2022
- Docker
- Git
- Swagger

## Features 📋

Budget is a webservice that allows the CRUD of expenses and income, in addition to providing a monthly summary. To access the application, you must perform OAuth2 authentication.

In addition, Budget has a service called Identity that centralizes authentication using the Identity Server.

## How to Run 🏃‍♂️

Budget can be run locally or through Docker Compose. In both cases, the Budget service is mapped to port 5000.

To run locally, follow the steps below:

- Clone the repository: `git clone https://github.com/brunoaragao/Budget.git`
- Open the project in Visual Studio and run the application.
- Access the swagger documentation through the link http://localhost:5000/swagger/index.html.

To run with Docker, follow the steps below:

- Make sure you have Docker installed.
- Clone the repository: `git clone https://github.com/brunoaragao/Budget.git`
- Navigate to the `docker` folder.
- Run the command `docker-compose up -d`.
- Access the swagger documentation through the link http://localhost:5000/swagger/index.html.

## How to Use 📚

To use the application, you must open the swagger documentation and click on the `Authorize` button and use the default credentials:

- client_id: `budget-swagger`
- secret: `secret`

After that, you can use the swagger documentation to test the application.

## How to Contribute 🤝

- Fork the repository.
- Create a new branch with your changes: `git checkout -b my-feature`
- Save your changes and create a commit message telling you what you did: `git commit -m "feature: My new feature"`
- Submit your changes: `git push origin my-feature`

After the merge of your pull request is done, you can delete your branch.

## License 📝

This project is under the MIT license. See the [LICENSE](LICENSE) file for more details.

## Acknowledgments 🎁

This project was developed as part of the [challenge](https://www.alura.com.br/challenges/back-end-2) proposed by Alura.