# Categorize - Technical Test

Este projeto foi desenvolvido como parte de um teste técnico para uma entrevista de emprego. O objetivo do projeto é categorizar operações financeiras, utilizando boas práticas de programação, arquitetura limpa, e princípios de SOLID.

## Tecnologias Utilizadas

- **Visual Studio 2022**
- **.NET 8**: Utilizado para desenvolver a aplicação console e implementar a lógica de categorização.
- **xUnit**: Framework de testes unitários.
- **Moq**: Biblioteca de mocking utilizada para criar testes unitários.

## Estrutura do Projeto

O projeto segue os princípios da arquitetura limpa, dividindo o código em várias camadas de responsabilidade:

- **ConsoleApp**: Contém o ponto de entrada da aplicação e a interface do usuário.
- **Application**: Contém interfaces e serviços de aplicação.
- **Domain**: Contém entidades de domínio e regras de negócio.
- **Infrastructure**: Contém a infraestrutura de dados e o repositório de categorias.
- **IoC**: Contém a configuração da injeção de dependências

## Exemplo de Uso

A primeira linha de entrada é composta pela data de referência (referenceDate).

A segunda linha de entrada indica o número de operações (n). 

Cada linha seguinte representa uma operação, composta por 3 componentes, o valor negociado, o setor 
do cliente e a data do próximo pagamento previsto, os elementos são separados por espaço 
Todos os valores são double e todas as datas estão no formato MM/dd/yyyy.

### Exemplo de Entrada

12/11/2020  
4  
2000000 Private 12/29/2025  
400000 Public 07/01/2020  
5000000 Public 01/02/2024  
3000000 Public 10/26/2023  

### Exemplo de Saída

HIGHRISK  
EXPIRED  
MEDIUMRISK  
MEDIUMRISK  