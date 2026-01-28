PaymentHub AI is an enterprise-grade payment orchestration engine built on .NET 10, designed to bridge the gap between high-security financial workflows and generative AI. 
The core objective is to provide a scalable, provider-agnostic platform that manages the complete transaction lifecycle—from idempotent order creation to fund capture and multi-provider refunds across PayPal and Westpac PayWay.

The architecture:
  - Security: A Python-based AI Sidecar that performs real-time fraud assessment using XGBoost, ensuring bank-grade security before any capital is moved.
  - Support: An integrated RAG (Retrieval-Augmented Generation) ChatBot powered by Azure OpenAI and Azure AI Search.

1. Feature, Status
  - **[In Progress]** PayPal Sandbox Integration
  - **[In Progress]** Intelligent Fraud Detection: Python(FastAPI) / XGBoost
  - **[In Progress]** Transaction Idempotency: Custom Header Validation
  - **[In Progress]** Frontend UI, React: [[Visit payment-ui](https://github.com/maya8624/payment-ui)]
  - **[In Progress]** Unit Testing & Mocking: xUnit and moq
  - **[Planned]** Direct Card Payments Integration: Westpac PayWay API
  - **[Planned]** Webhook support for real-time updates: Planned
  - **[Planned]** Support Chatbot (RAG): Azure OpenAI + AI Search
  - **[Planned]** Cloud Security: Azure Key Vault / Managed Identity
  - **[Planned]** Infrastructure: App Service / Key Vault / SQL

2. Technologies
  - Backend: .NET 10, EF Core, Entity Framework Core, FluentValidation, Swagger.
  - AI/Python: FastAPI, XGBoost.
  - Cloud: Azure AI Search, Azure OpenAI, Azure SQL, Azure Key Vault, Azure Blob Storage
  - Frontend: React 19, Tailwind CSS.

3. Technical Challenges & Solutions
  - Cross-Language Service Coordination (The Sidecar Pattern)
  - Ensuring Transactional Idempotency
  - Implementing a Fail-Closed Security Policy
  
<img width="1453" height="575" alt="image" src="https://github.com/user-attachments/assets/5b55961b-3872-4042-9322-23fd575d49e4" />

⚠️ Note: This project is still a work in progress. Features are being expanded, and improvements are ongoing to support more payment providers and robust error handling and testing.
