# Soundfy API

![Java](https://img.shields.io/badge/Java-21-orange?style=for-the-badge&logo=java)
![Spring Boot](https://img.shields.io/badge/Spring_Boot-4-6DB33F?style=for-the-badge&logo=spring)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql)

A **Soundfy API** é um sistema back-end desenvolvido em Java com Spring Boot para o gerenciamento de uma biblioteca musical. A aplicação permite cadastrar álbuns, associar músicas (armazenadas como arquivos `.mp3`) a esses álbuns e organizar playlists com controle de ordem das faixas.

## 🚀 Tecnologias Utilizadas

*   **Java 21**
*   **Spring Boot 4** (Web, Data JPA, Validation)
*   **PostgreSQL** (Banco de dados relacional)
*   **Hibernate / JPA** (Mapeamento Objeto-Relacional)
*   **Lombok** (Redução de boilerplate)
*   **Padrão DTO** usando Java Records

## 🏗️ Arquitetura e Modelagem

O projeto segue a arquitetura em camadas (Controllers, Services, Repositories e DTOs) e adota boas práticas RESTful.
Todas as chaves primárias do sistema utilizam **UUID** para maior segurança e escalabilidade.

### Relacionamentos do Banco de Dados
A regra de negócios obedece à seguinte hierarquia:
1.  **Album**: Entidade raiz, contém informações do álbum (título, artista, ano de lançamento).
2.  **Song**: Uma música sempre pertence a um Álbum e armazena a referência (`filePath`) para o arquivo `.mp3` correspondente.
3.  **Playlist**: Coleção de músicas criada pelo usuário. A relação entre Playlist e Song é N:N, mediada pela entidade associativa **PlaylistSong**, que guarda a posição (`position`) de cada música dentro da playlist, permitindo reordenação.

## ⚙️ Pré-requisitos

Para rodar o projeto localmente, você precisará de:
*   [JDK 21+](https://jdk.java.net/) (ou equivalente configurado no seu ambiente)
*   [PostgreSQL](https://www.postgresql.org/) rodando na porta `5432`
*   Maven (ou utilizar a IDE de sua preferência, como IntelliJ IDEA)

## 🛠️ Como Executar o Projeto

**1. Clone o repositório:**
```bash
git clone https://github.com/seu-usuario/soundfy.git
cd soundfy
```

**2. Configure o Banco de Dados:**
No PostgreSQL, crie um banco de dados chamado `soundfy_db`.
Verifique se as credenciais no arquivo `src/main/resources/application.properties` estão corretas para o seu ambiente local:

```properties
spring.datasource.url=jdbc:postgresql://localhost:5432/soundfy_db
spring.datasource.username=seu_usuario
spring.datasource.password=sua_senha

# Na primeira execução, mantenha 'create' ou 'update' para gerar as tabelas.
# Depois, mude para 'update' para não perder os dados salvos.
spring.jpa.hibernate.ddl-auto=update
```

**3. Rode a aplicação:**
Execute a classe principal `SoundfyApplication.java` na sua IDE ou use o Maven:

```bash
./mvnw spring-boot:run
```

A API estará disponível em: `http://localhost:8080`

## 🔗 Principais Endpoints

### Álbuns (`/api/albums`)

* `POST /api/albums` - Cadastra um novo álbum.
* `GET /api/albums` - Lista todos os álbuns.
* `GET /api/albums/{id}` - Busca álbum por UUID.
* `GET /api/albums/{id}/songs` - Lista as músicas de um álbum.
* `PUT /api/albums/{id}` - Atualiza os dados do álbum.
* `DELETE /api/albums/{id}` - Deleta o álbum (e suas músicas em cascata).

### Músicas (`/api/songs`)

* `POST /api/songs` - Cadastra uma música atrelada a um álbum.
* `GET /api/songs` - Lista todas as músicas.
* `GET /api/songs/{id}` - Busca música por UUID.
* `PUT /api/songs/{id}` - Atualiza uma música (inclusive pode trocar de álbum).
* `DELETE /api/songs/{id}` - Remove uma música.

### Playlists (`/api/playlists`)

* `POST /api/playlists` - Cria uma nova playlist.
* `GET /api/playlists` - Lista todas as playlists.
* `GET /api/playlists/{id}` - Busca playlist por UUID (com as músicas ordenadas por posição).
* `PUT /api/playlists/{id}` - Atualiza nome/descrição da playlist.
* `DELETE /api/playlists/{id}` - Deleta a playlist.
* `POST /api/playlists/{id}/songs` - Adiciona uma música existente à playlist (na próxima posição livre).
* `DELETE /api/playlists/{id}/songs/{songId}` - Remove uma música da playlist e reordena as posições restantes.

## 👨‍💻 Autor

**Guilherme Perondi**

* [LinkedIn](https://www.linkedin.com/in/guilherme-perondi)
* [GitHub](https://github.com/perondi09)
