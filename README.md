Projeto de API de Vendas
========================

Este projeto foi desenvolvido como parte do desafio final da formação em C# com .NET da DIO. Ao longo do desenvolvimento, foram explorados diversos pontos adicionais em relação à proposta inicial.

Descrição do Projeto
--------------------

A API é uma aplicação RESTful construída utilizando a plataforma .NET Core. Ela permite a realização do registro de vendas, busca de vendas por identificador e atualização do status de uma venda.

### Operações da API

1.  **Registrar venda**: Essa operação permite registrar uma nova venda, fornecendo os dados do vendedor e os itens vendidos. A venda é registrada com o status inicial "Aguardando pagamento".
    
2.  **Buscar venda**: Essa operação permite buscar uma venda pelo código identificador. É possível obter todas as informações da venda, incluindo os dados do vendedor e os itens vendidos.
    
3.  **Atualizar venda**: Essa operação permite atualizar o status de uma venda. As transições de status permitidas são as seguintes:
    
    *   De: `Aguardando pagamento` Para: `Pagamento Aprovado`
    *   De: `Aguardando pagamento` Para: `Cancelada`
    *   De: `Pagamento Aprovado` Para: `Enviado para Transportadora`
    *   De: `Pagamento Aprovado` Para: `Cancelada`
    *   De: `Enviado para Transportadora` Para: `Entregue`

### Estrutura da Venda

Uma venda contém as seguintes informações:

*   Identificador da venda
*   Dados do vendedor:
    *   ID do vendedor
    *   CPF do vendedor
    *   Nome do vendedor
    *   E-mail do vendedor
    *   Telefone do vendedor
*   Data da venda
*   Itens vendidos

### Persistência de Dados

A aplicação utiliza o Entity Framework Core juntamente com o Microsoft SQL Server para persistir os dados. É criado um banco de dados chamado `tech_test_payment_api_db` no contêiner do SQL Server. As configurações de conexão com o banco de dados são fornecidas através das variáveis de ambiente no serviço `webapi` do Docker Compose.

### Documentação da API

A API possui documentação Swagger, que pode ser acessada através da rota `http://localhost/swagger`.

### Pontos Explorados

Durante o desenvolvimento do projeto, foram explorados os seguintes pontos:

*   Arquitetura da aplicação
*   Testes unitários
*   SQL Server
*   Entity Framework Core
*   Docker
*   Nginx

Executando a Aplicação
----------------------

Para executar a aplicação, siga as instruções abaixo:

1.  Certifique-se de ter o Docker e o Docker Compose instalados na sua máquina.
2.  Clone o repositório do projeto.
3.  Abra um terminal na pasta raiz do projeto.
4.  Execute o comando `docker-compose up` para construir e iniciar os contêineres da aplicação.
5.  A aplicação estará disponível em `http://localhost`.

Executando os Testes
--------------------

Para executar os testes, siga as instruções abaixo:

1.  Abra um terminal no diretório api.
2.  Execute o comando `dotnet test` para executar os testes automatizados.

Observações
-----------

*   A API não possui mecanismos de autenticação/autorização implementados.
*   Para facilitar a execução da aplicação, foi utilizado o Docker Compose, permitindo a criação e execução dos contêineres de forma simples e isolada.
* O projeto utiliza o servidor Nginx como proxy reverso, redirecionando as requisições para a aplicação .NET Core.
