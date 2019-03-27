# FrameworkFS

O projeto foi desenvolvido para facilitar o desenvolvimento de sistemas que são fornecidas como um serviço.

Atualmente está disponível a integração com o banco de dados MySQL, pondendo ser inseridas outras integrações de maneira simples, visto que a instância do banco de dados é feita através do padrão de projeto Abstract Factory.

## Bancos de Dados Suportados

  - [MySQL](DbConnection/MySQLConnection.cs)

## Recursos Implementados

 - Envio de E-mail
 - Criptografia de senha MD5
 - Autenticação via Bearer Token

## Estrutura Banco de Dados
O projeto exige uma estrutura básica do banco de dados, sendo essa escrita no arquivo [script_banco.sql](ScriptBanco/script_banco.sql), que também possui a inserção de um usuário padrão para uma empresa, sendo ele:
Usuário: Admin
Senha: 123@mudar
Email: admin@site.com.br

## Classes Abstratas
O projeto possui 2 classes que serão mais utilizadas, sendo elas:
 - [AbstractCRUDController](Controller/AbstractCRUDController.cs): que possui os métodos Insert, IdentityInsert, IdentityInsertImage, Find, List(Get), List(Post, que recebe um objeto para realizar filtros), Update, UpdateImage, Delete, Deactivate, Activate;
 - [AbstractDAO](DAO/AbstractDAO.cs): que possui os métodos DDL e DML.

O projeto também possui as seguintes interfaces:
 - [IActivableObject](Interfaces/IActivableObject.cs): Interface responsável por definir que a entidade pode possuir o status de Ativo ou Inativo;
 - [IChildEmpresaObject](Interfaces/IChildEmpresaObject.cs): Interface responsável por definir que a entidade possui uma propriedade chamada EmpresaId;
 - [IChildUsuarioObject](Interfaces/IChildUsuarioObject.cs): Interface responsável por definir que a entidade possui uma propriedade chamada UsuarioId;
 - [IConnection](Interfaces/IConnection.cs): Interface responsável por definir os métodos que o objeto de conexão deve implementar, para uso do padrão Abstract Factory;
 - [IFileUploadObject](Interfaces/IFileUploadObject.cs): Interface responsável por definir que a entidade possui a propriedade Imagem, onde será salvo o caminho da imagem salva no servidor.

## Fluxo do Projeto
Ao ser realizada uma requisição para um Controller que herde de AbstractCRUDController ou [AbstractController](Controller/AbstractController.cs), que o primeiro herda do segundo, no método construtor do segundo é instanciado um objeto de conexão com o banco, sendo definido o objeto no arquivo Web.config da API.

Trechos usados:

	<add key="ConnectionString" value="Server=localhost;Database=webapi;UID=root;Password=" />
    <add key="ConnectionType" value="0" />
    <add key="AuthorizedUrls" value="http://localhost" />
    
Caso a requisição tenha sido feita com um token, será verificado se o token possui as Claims "Id" e "EmpresaId", caso possua o valor de cada uma será adicionado em memória e estará disponível para uso tanto no Controller, quanto no Business Object, quanto no Data Access Object.


### Todos

 - Adicionar integração com ExpoNotification

Licença
----

GNU GPL
