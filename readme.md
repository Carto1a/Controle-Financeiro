# Controle Financeiro

Sistema simples para **gerenciar pessoas, categorias e transações**, permitindo acompanhar receitas, despesas e saldo de forma organizada.  

O backend foi desenvolvido seguindo **Clean Architecture** e os princípios de **Domain-Driven Design (DDD)**

---

## Funcionalidades

### Pessoas
- Criar, listar e deletar pessoas
- Ao deletar uma pessoa, todas as suas transações também são removidas

Campos:

| Campo         | Tipo                |
|---------------|---------------------|
| Id            | UUID                |
| Nome          | Texto               |
| DataNascimento | DateTime |
| Transacoes | Lista de Transacoes |
| Idade | Numero | Calculada a partir da data de nascimento, não precisa ser atualizada manualmente |

---

### Categorias
- Criar e listar categorias

Campos:

| Campo         | Tipo                  |
|---------------|---------------------|
| Id | UUID |
| Nome | Texto | Nome curto da categoria, facilita exibição em relatórios e dropdowns | 
| Descrição     | Texto                 | Explicação detalhada da categoria |
| Finalidade    | `despesa`, `receita`, `ambas` |

---

### Transações
- Criar e listar transações
- Menores de 18 anos só podem registrar **despesas**
- Categorias devem respeitar a finalidade da transação

Campos:

| Campo         | Tipo                  | Observações |
|---------------|---------------------|---------------|
| Id | UUID |
| Descrição     | Texto                 |
| Valor         | Decimal |
| Tipo          | `despesa`, `receita` |
| CategoriaId     | UUID para Categoria |
| Data | DateTime | Data da transação, permite registrar quando a despesa ou receita ocorreu |
| CriadoEm | DateTime | Data de criação do registro, importante para histórico e auditoria |

* Não foi feito uma referencia a pessoa pq não existe a necessidade do aggregado Transação ter conhecimento de pessoa.

---

### Relatórios

#### Totais por pessoa
- Total de receitas, despesas e saldo de cada pessoa
- Total geral de todas as pessoas

#### Totais por categoria
- Total de receitas, despesas e saldo de cada categoria
- Total geral de todas as categorias

### Eventos de Domínio

Para modelar a regra de negócio de que **ao deletar uma pessoa, todas as suas transações devem ser removidas**, foi utilizado um **evento de domínio**:

- **Evento:** `PessoaDeletadaEvent`
  Local: `ControleFinanceiro.Domain/Pessoas/PessoaDeletadaEvent.cs`
  Representa que uma pessoa foi deletada no sistema.

- **Handler:** `RemoverTransacoesAoDeletarPessoaEventHandler`
  Local: `ControleFinanceiro.Domain/EventHandlers/RemoverTransacoesAoDeletarPessoaEventHandler.cs`
  Responsável por ouvir o evento e **remover todas as transações associadas** à pessoa deletada.

Essa abordagem mantém a regra de negócio no domínio, sem depender da infraestrutura ou da API, garantindo consistência ao deletar uma pessoa.

---

## Arquitetura e Padrões

O backend foi estruturado com:

- **Clean Architecture**: separação clara entre camadas (Domain, Application, Infrastructure), como é um projeto pequeno, a api e db foi mantido na mesma camada, Infrastructure.
- **DDD (Domain-Driven Design)**: foi utilizado para mandar a separação clara das regras de negocio e modelos.

Essa organização facilita a **testabilidade, manutenção e evolução futura do sistema**.

---

## Tecnologias
- Backend: C# (.NET 10)
- Banco de dados: PostgreSQL
- Frontend: React

---

## Pré-requisitos

Antes de rodar a aplicação, é necessário **configurar as variáveis de ambiente** para acesso ao banco de dados.  
Crie um arquivo `.env` na raiz do projeto (ou copie do `.env.example`) e defina pelo menos:

```env
DB_USER=controlefinanceiro
DB_PASSWORD=cvnVNUn!3bnaos12893n@!0nkn
DB_HOST=db
DB_PORT=5432
DB_NAME=ControleFinanceiro
```

## Como rodar

```bash
git clone https://github.com/Carto1a/Controle-Financeiro.git
cd Controle-Financeiro

docker compose up --build
```

A aplicação vai ficar disponivel em `http://localhost:8080`
