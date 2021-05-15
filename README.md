## Diagram
![diagram](https://github.com/Much4cho/Restpirators/blob/master/diagram.png)

## Uruchamianie w środowisku lokalnym
```
docker-compose up
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
