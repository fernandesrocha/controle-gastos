# Implementação do Sistema de Controle de Gastos Residenciais

Este sistema é dividido em duas partes principais: **Back-end** (Web API em C# com .NET Core) e **Front-end** (React com TypeScript). Para persistência, usa Entity Framework Core com SQLite, pois é uma opção leve, fácil de configurar e mantém os dados persistidos em um arquivo de banco de dados local após reinícios do sistema. Isso permite persistência sem depender de um servidor externo.

## Lógica Geral e Arquitetura
- **Back-end (Web API)**: Responsável por gerenciar dados via endpoints RESTful. Usa Entity Framework Core para ORM e SQLite para o banco. Inclui validações de regras de negócio (ex.: restrições por idade, finalidade de categorias, valores positivos). Ao deletar uma pessoa, as transações associadas são cascateadas para deleção via configuração do EF Core.
- **Front-end (React TS)**: Interface web para interagir com a API. Usa Axios para chamadas HTTP. Inclui formulários para CRUD, listas e relatórios. Validações no front são mínimas, priorizando as do back-end para consistência.
- **Separação**: O back-end roda em uma porta (ex.: 5000), e o front consome a API via HTTP.
- **Documentação**: Todo o código inclui comentários explicando a lógica e funções.
- **Opcional**: Implementei a consulta por categoria como bônus, pois é similar à por pessoa e enriquece o sistema.

No back-end, uso injeção de dependência, async/await, DTOs para exposição de dados. No front, componentes funcionais, hooks (useState, useEffect), tipagem forte com TS.

Para rodar:
**Back-end**: Crie um projeto ASP.NET Core Web API, adicione pacotes via NuGet (Microsoft.EntityFrameworkCore.Sqlite, Microsoft.EntityFrameworkCore.Tools). Rode `dotnet run`.
**Front-end**: Crie um app React TS (`npx create-react-app my-app --template typescript`), instale dependências (axios, react-router-dom). Rode `npm start`.