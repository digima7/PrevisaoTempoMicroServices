# PrevisaoTempoMicroServices

Aplicação possui dois micro serviços sendo o primeiro que ira Expor uma API para poder cadastrar uma cidade para buscar informações e outro que ira solicitar as informações a API OpenWeatherMap.

### Arquitetura

Os dois micro serviços foram feitos em console, pois não havia necessidade de tela.
Foi criado uma biblioteca "MicroServiceBase" para poder fazer toda a parte de mensageria, publicando e consumindo registros do Rabbit.

Utilizar RabbitMQ como Message broker. 
  - Dependencia do RabbitMQ erlang = Link para download: https://www.erlang.org/downloads
  - Link para download do RabbitMQ: https://www.rabbitmq.com/install-windows.html#installer versão 3.8.3      
  - Para liberar o management do Rabbit pode ser necessário ativar o plugin, pra isso basta acessar a pasta onde foi instalado o rabbit ("C:\Program Files\RabbitMQ Server\rabbitmq_server-3.8.3\sbin>"), pelo cmd e executar o comando "rabbitmq-plugins enable rabbitmq_management"
  - para visualizar o management do Rabbit basta acessar o link "http://localhost:15672/".
    - Usuario : guest
    - senha   : guest

### Tecnologias
  - C#, .NET Core 3.1
  - EntityFramework
  - Nancy
  - Sqlite
  - RabbitMq

### Executando o Projeto

É necessário ter instaldo o RabbitMQ para poder rodar o projeto, e também rodar o Visual Studio como administrador por causa do Nancy.
Ao rodar será criado duas "Exchanges" no RabbitMQ com os seguintes nomes "WeatherRequest" e "WeatherReport".
Rodando o projeto MicroService1, será exposto uma API para cadastro das Cidades, para testar pode-se utilizar o postman, fazer uma solicitação Post para "localhost:8000\" enviando o seguinte Json no Body :

    {
      "Name": "Blumenau"
    }
    
 Com isso a o microservice1 ira cadastrar essa cidade no Banco de dados e ira mandar uma solicitação para o Rabbit na fila "WeatherRequest". 
 
 Neste momento o microservice2 ira receber essa mensagem, processar ela utilizando a API OpenWeatherMap e ira mandar a resposta para o Rabbit na fila ""WeatherReport".
 
 O microservice1 ira consumir essa resposta e atualizar o banco de dados com ela.
 
 Para testar se a resposta ja venho basta fazer uma solicitação Get no postman passando o nome da cidade que foi cadastrada na url, 
 por exemplo se você cadastrou Blumenau, basta fazer um o seguinte GET "localhost:8000\Blumenau" e o postman irá lher retornar os dados pegos pela API OpenWeatherMap.

Para o banco de dados foi utilizado SQLite, com isso quando executado o programa ele irá criar um banco chamado "database.db" dentro da pasta "...\MicroServicesPrevisaoTempo\PrevisaoTempo\MicroService1\bin\Debug\netcoreapp3.1"

Foi adicionado um serviço que roda de tempo em tempo, conforme configurado, para atualizar os dados das cidades ja cadastradas, ele apenas pegas as cidades do banco e manda novamente para a fila "WeatherRequest" do Rabbit.
