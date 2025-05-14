# NotificationUsingSignalR  
  
A .NET-based real-time notification system that demonstrates how to implement push notifications using SignalR technology.  
  
## System Architecture  
  
```mermaid  
flowchart TD  
    subgraph "Client Applications"  
        WebApps["Web Applications"]  
        MobileApps["Mobile Applications"]  
    end  
  
    subgraph "NotificationUsingSignalR System"  
        subgraph "API Layer"  
            Controllers["API Controllers"]  
            Hub["NotificationHub"]  
            Middleware["Middleware"]  
        end  
  
        subgraph "Application Layer"  
            Services["Application Services"]  
            DTOs["Data Transfer Objects"]  
            Interfaces["Service Interfaces"]  
        end  
  
        subgraph "Domain Layer"  
            Entities["Domain Entities"]  
            DomainInterfaces["Domain Interfaces"]  
        end  
  
        subgraph "Infrastructure Layer"  
            Repositories["Repositories"]  
            DbContext["ApplicationDbContext"]  
            JwtGenerator["JWT Generator"]  
        end  
    end  
  
    DB[(Database)]  
  
    WebApps -->|"HTTP Requests"| Controllers  
    MobileApps -->|"HTTP Requests"| Controllers  
    WebApps -->|"WebSocket"| Hub  
    MobileApps -->|"WebSocket"| Hub  
      
    Controllers --> Services  
    Hub --> Services  
    Services --> Repositories  
    Services --> DomainInterfaces  
    Repositories --> DbContext  
    DbContext --> DB  
    Repositories --> Entities















