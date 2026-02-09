```mermaid
graph TD
    %% Nós (Projetos)
    API[Ecommerce.API]
    APP[Ecommerce.Application]
    DOM[Ecommerce.Domain]
    INF[Ecommerce.Infra]
    IDN[Ecommerce.Identity]
    TST[Ecommerce.Tests]

    %% Estilos
    style DOM fill:#f9f,stroke:#333,stroke-width:4px,color:black
    style API fill:#bbf,stroke:#333,stroke-width:2px,color:black
    style APP fill:#bfb,stroke:#333,stroke-width:2px,color:black
    style INF fill:#fbb,stroke:#333,stroke-width:2px,color:black
    style IDN fill:#fbb,stroke:#333,stroke-width:2px,color:black

    %% Relacionamentos (Setas)
    API -->|Usa| APP
    API -->|Injeta| INF
    API -->|Injeta| IDN

    APP -->|Define Regras| DOM
    INF -->|Implementa| DOM
    IDN -->|Usa Entidades| DOM
    
    TST -.->|Testa| APP
    TST -.->|Testa| DOM
    ````