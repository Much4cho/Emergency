## Opis architektury
![diagram](https://github.com/Much4cho/Restpirators/blob/master/diagram.png)

Cała aplikacja jest REST'owym API korzystającym z protokołu HTTP. Dane są przesyłane w postaci JSON.<br>W serwisie klienta dowolny użytkownik (także nieautoryzowany) może dokonać zgłoszenia np. wypadku. Serwis klienta (client) protokołem TCP na porcie 5672 przesyła do brokera RabbitMQ zgłoszenie, gdzie zostaje ono wrzucone do podanej w serwisie klienta kolejki. Do tej samej kolejki jako konsument podłączony jest serwis przetwarzania zgłoszenia i zarządzania nim (dispatcher). Gdy otrzymuje on zgłoszenie lub administrator zmodyfkuje zgłoszenie (np. przypisze zespół ratunkowy lub zakończy zgłoszenie) zostaje ono wrzucone do innej kolejki przez RabbitMQ brokera, z której odczytuje je serwis statystyk (analyzer). Oba te serwisy zapisują sobie w bazach przetworzone informacje (serwis zarządzania zgłoszeniem w bazie MSSQL w chmurze, serwis statystyk w bazie mongodb). Dane do baz także są przesyłane przy użyciu protokołu TCP (dla mongo 27017, dla MSSQL 1433). Do korzystania z serwisów innych niż klienta wymagany jest token, który otrzymuje użytkownik po poprawnym zalogowaniu przez serwis autoryzacji i autentykacji (auth), który także korzysta z własnej bazy mongo w celu przechowywania użytkowników. Aplikacja po stronie front-endu działa z użyciem portu 4200. Wszystkie zapytania z części frontowej przechodzą przez bramę API (gateway), która korzysta z protokołu HTTP na porcie 5003. Wszystkie zapytania oraz metryki są przezkazywane do Prometheusa w celu monitorowania aplikacji, wykorzsytywany jest do tego port 9090, aby przeglądać metryki w wygodnym graficznym interfejsie do aplikacji została także podpięta Grafana, która korzysta z portu 3000.<br><br> Aby spełnić wszystkie wymagania projektowe została także podpięta **Kafka** do przetwarzania danych. Działa ona na porcie 9092. Producent został zaimplementowany jako serwis działający w tle w serwisie statystyk (analyzer). Przekazuje on dane do topica Kafki, która korzysta z Zookeepera na porcie 2181 oraz bazy danych ksql na porcie 8088. Z otrzymanych danych wylicza **średnią kroczącą** i wrzuca do innego topica, z którego odczytuje je konsument, który działa na porcie 8081. Aplikacja korzysta głónie z protokołó TCP oraz HTTP.

## Uruchamianie w środowisku lokalnym
```
docker-compose up
```
### Sprawdzanie logów
#### producer
```
docker-compose logs | grep "Produced"
```
output:
```
analyzer         | Produced value: 20
analyzer         | Produced value: 64
analyzer         | Produced value: 68
analyzer         | Produced value: 3
```
#### consumer
```
docker-compose logs | grep "Received Message"
```
output (średnia liczona co 1000 wyprodukowanych wartości):
```
consumer         | Received Message: {"AVG":50.868}
consumer         | Received Message: {"AVG":48.924}
```
## Deplyment 
```
docker swarm init
```
dołączanie worker nodeów:
```
docker swarm join --token <token> <IP>:2377
```
```
docker stack deploy --compose-file .\docker-compose.yml <stack-name>
```
