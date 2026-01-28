PaymentHub AI is an enterprise-grade payment orchestration engine built on .NET 10. Designed for scalability and high-security financial workflows, 
the system manages the complete transaction lifecycle—from idempotent order creation to fund capture and multi-provider refunds.
The architecture features a Python-based AI Sidecar for real-time fraud assessment and leverages a Provider-Agnostic design. 
This allows the platform to seamlessly switch between PayPal Sandbox and Westpac PayWay via a unified interface, ensuring the system is ready
for global card processing and bank-grade integration.

1. Feature,Status,Technology
- **[In Progress]** PayPal Sandbox Integration
- **[In Progress]** Intelligent Fraud Detection: Python(FastAPI) / XGBoost
- **[In Progress]** Transaction Idempotency: Custom Header Validation
- **[In Progress]** Frontend UI, React 19 + Tailwind CSS: [[Visit payment-ui](https://github.com/maya8624/payment-ui)]
- **[In Progress]** Unit Testing & Mocking: xUnit and moq
- **[Planned]** Direct Card Payments: Westpac PayWay API
- **[Planned]** Webhook support for real-time updates: Planned
- **[Planned]** Support Chatbot (RAG): Azure OpenAI + AI Search
- **[Planned]** Cloud Security: Azure Key Vault / Managed Identity
- **[Planned]** Infrastructure: App Service / Key Vault / SQL


2. Technologies
- Backend: .NET 10, EF Core, Azure SQL, Entity Framework Core, FluentValidation, Swagger.
- AI/Python: FastAPI, XGBoost, Azure OpenAI.
- Cloud/Data: Azure AI Search, SQL Server, Key Vault.
- Frontend: React 19, Tailwind CSS.
- <img width="1470" height="776" alt="image" src="https://github.com/user-attachments/assets/6389e65e-1785-4711-91d8-c85d95d019fc" />
  
⚠️ Note: This project is still a work in progress. Features are being expanded, and improvements are ongoing to support more payment providers and robust error handling and testing.
