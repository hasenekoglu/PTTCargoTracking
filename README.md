# PTT Cargo Tracking System

Bu proje, kargo takip sistemi olarak geliştirilmiştir. Aşağıdaki adımları izleyerek projeyi kolayca çalıştırabilirsiniz.

## Gereksinimler
- Docker yüklü olmalı. (Yükleme için [Docker resmi sitesi](https://www.docker.com/get-started))
- .NET Core SDK yüklü olmalı. (Yükleme için [Microsoft .NET resmi sitesi](https://dotnet.microsoft.com/))

---

## Kurulum Adımları

### 1. Hangfire Veritabanını Oluşturma
1. Aşağıdaki komutu çalıştırarak bir SQL Server konteyneri oluşturun:
    ```bash
    docker run --name HangfireDB -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=123456aA*' -p 51434:1433 -d mcr.microsoft.com/mssql/server:2019-latest
    ```
2. Konteynerin içine erişmek için şu komutu çalıştırın:
    ```bash
    docker exec -u 0 -it HangfireDB bash
    ```
3. İçeride SQL araçlarını yüklemek için şu komutları sırasıyla çalıştırın:
    ```bash
    apt-get update
    apt-get install -y mssql-tools unixodbc-dev
    ```
4. SQL arayüzünü başlatmak için şu komutu çalıştırın:
    ```bash
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "123456aA*"
    ```
5. Hangfire veritabanını oluşturmak için şu sorguyu çalıştırın:
    ```sql
    CREATE DATABASE HangfireDatabase;
    GO
    ```

---

### 2. Tracking Veritabanını Oluşturma
1. Aşağıdaki komutu çalıştırarak bir SQL Server konteyneri daha oluşturun:
    ```bash
    docker run --name TrackingDB -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=123456aA*' -p 51433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
    ```
2. Konteynerin içine erişmek için şu komutu çalıştırın:
    ```bash
    docker exec -u 0 -it TrackingDB bash
    ```
3. Aşağıdaki komutu çalıştırarak ortamı güncelleyin:
    ```bash
    apt-get update
    ```

---

### 3. Projeyi Çalıştırma
1. Bu GitHub reposunu klonlayın veya zip dosyasını indirip çıkartın.
2. `src` klasörüne gidin. Örneğin:
    ```bash
    cd \PTTCargoTracking-master\src
    ```
3. `Tracking.API` klasöründe terminali açın.
4. Proje konteynerlerini oluşturmak için aşağıdaki komutları çalıştırın:
    ```bash
    docker-compose build
    docker-compose up
    ```
5. Proje ayağa kalktıktan sonra aşağıdaki URL'den Swagger arayüzüne erişebilirsiniz:
    [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

---

## Notlar
- Veritabanı ayarlarını projede düzenlemeniz gerekebilir. Bunun için `appsettings.json` dosyasını kontrol edin.
- Herhangi bir sorun yaşarsanız lütfen logları kontrol edin ve Docker konteynerlerinin doğru çalıştığından emin olun.

---

Bu adımları tamamladıktan sonra proje başarıyla çalışacaktır!
