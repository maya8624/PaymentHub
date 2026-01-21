This project is a scalable, testable, and secure payment system designed to handle online transactions.
It supports the full payment lifecycle, including order creation, payment approval, fund capture, and refunds.
The architecture is built with extensibility in mind, allowing additional payment providers to be integrated in the future.
Currently, the system integrates with PayPal Sandbox for testing and development purposes.

1. Features
- **[Completed]** Create and manage orders
- **[Completed]** Approve and capture payments
- **[Completed]** Idempotency support
- **[Completed]** Refund transactions
- **[In Progress]** Implementing Unit Tests using xUnit and moq
- **[In Progress]** Frontend: In progress [[Visit payment-ui](https://github.com/maya8624/payment-ui)]
- **[Planned]** Webhook support for real-time updates: Planned
- **[Planned]** Authentication
- **[Planned]** Azure deployment 
  

2. Technologies
- C# .NET 8 / ASP.NET Core Web API
- Entity Framework Core
- Azure App Service, Key Vault, SQL
- xUnit
- PayPal Sandbox API
- FluentValidation
- React for front-end
- Swagger
- <img width="1470" height="776" alt="image" src="https://github.com/user-attachments/assets/6389e65e-1785-4711-91d8-c85d95d019fc" />

  
⚠️ Note: This project is still a work in progress. Features are being expanded, and improvements are ongoing to support more payment providers and robust error handling and testing.
