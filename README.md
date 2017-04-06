>>>>>
Studentnaam:
Wijnand Honselaar
Studentnummer: 
533747
---
# Algemene beschrijving applicatie
HttpLoadBalancer

##  Class diagram van de Load Balancer
![alt-tag](https://github.com/wijnandhonselaar/HttpLoadBalancer/blob/develop/classdiagram.png?raw=true)

##  Verdeling van load over servers
Een loadbalancer moet de load verdelen over verschillende servers. De gebruikte methodes hieronder maken allemaal gebruik van een healthcheck, om zeker te weten dat de server online is voordat deze wordt gebruikt om een request naartoe te sturen.
### Round Robin
Round robin gaat het rijtje af. Van boven naar onder krijgt iedere server op zijn beurt de request binnen. Klik [hier](https://github.com/wijnandhonselaar/HttpLoadBalancer/blob/develop/HttpLoadBalancer/HttpLoadBalancer/Models/Methods/RoundRobinMethod.cs) om de methode te bekijken.
### Random
Random kiest iedere keer een willekeurige server uit de lijst van beschikbare servers. Klik [hier](https://github.com/wijnandhonselaar/HttpLoadBalancer/blob/develop/HttpLoadBalancer/HttpLoadBalancer/Models/Methods/RandomMethod.cs) om de methode te bekijken.
### Ping (ReponseTime)
Reponsetime stuurt een headRequest en houdt de tijd bij. De server met de snelste tijd van request sent to last byte received wordt gekozen. Klik [hier](https://github.com/wijnandhonselaar/HttpLoadBalancer/blob/develop/HttpLoadBalancer/HttpLoadBalancer/Models/Methods/PingMethod.cs) om de methode te bekijken.

### Toevoegen van nieuwe Load Balance methodes
Een nieuwe methode toevoegen is vrij simpel. Iedere methode is afgeleid van de abstracte `Method` class. Door een nieuwe class aan te maken die hiervan afgeleid is en deze toe te voegen in de MethodService gaat de rest vanzelf.

## Session Persistence implementatie, keuzes en algoritmiek
Je moet de mogelijkheid hebben gebruik te maken van cookies of sessions. Een van beide methodes kan worden geselecteerd, met een checkbox erbij om het uit te zetten om duidelijk de werking hiervan te tonen.

Allereerst wordt er gekeken of er een sessie is, als er geen sessie is wordt het Loadbalance algoritme uitgevoerd. Wanneer er wel een sessie is wordt er nagegeaan of deze server nog gebruikt wordt EN online is. Wanneer dit het geval is wordt deze server gebruikt. Anders wordt het LoadBalance algoritme uitgevoerd om een server te selecteren.

Wanneer de server responseheader "set-cookie" bevat wordt de sessie opgeslagen in een session of een cookie, op basis van welke methode geselecteerd is.
### Cookies
Wanneer de server responseheader "set-cookie" bevat wordt de set-cookie header property van de repsonse aangepast naar `$"serverID={currentServer.Address}-{currentServer.Port}"`.
Wanneer de LB een nieuwe request van de browser deze cookie bevat, worden de stappen doorlopen zoals hiervoor beschreven en wordt aan de hand van de informatie in de cookie de server opgehaald. Als deze server niet meer bestaat, wordt er een een 503 Server Unavailable terug gestuurd.
### Sessions
Wanneer de server responseheader "set-cookie" bevat wordt er op de LoadBalancer een nieuwe sessie toegevoegd. Hiervoor wordt een cookie object gebruikt in een `Dictionary<string, Cookie>`, string = de sessionID uit de header, de Cookie bevat de server van de response en expires.

Wanneer de LB een nieuwe request van de browser deze sessionID bevat, wordt de sessie opgehaald uit de lijst en wordt de server gebruikt die in de sessie opgeslagen staat. Als deze server niet meer bestaat, wordt er een een 503 Server Unavailable terug gestuurd.

De `SessionService` heeft een property `SessionManager`. Deze wordt gebruikt om sessies te beheren en wordt geset vanuit de dropdown in de interface.
### Toevoegen van nieuwe Session Persistence methodes
Iedere Session Persistence methode is afgeleid van de IPersistenceMethod interface. 

Om een nieuwe methode toe te voegen hoeft er alleen een nieuwe class aan te worden gemaakt die deze interface overerft.
De `Nam`e van de class wordt in de lijst gezet in de interface. De `FullName` wordt gebruikt om later een instantie van de methode te starten.
```C#
// Set Persistence Methods
_persistenceMethods = new Dictionary<string, string>();
var pType = typeof(IPersistenceMethod);
var methodTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(s => s.GetTypes())
    .Where(p => pType.IsAssignableFrom(p));
foreach (var method in methodTypes)
{
    if (method.Name != "IPersistenceMethod")
        _persistenceMethods.Add(method.Name, method.FullName);
}
_gui.PersistenceMethods.Items.AddRange(_persistenceMethods.Select(method => method.Key).ToArray());
```
Hier word het type opgehaald uit de dictionary en een nieuwe instantie aangemaakt.
```C#
var type = Type.GetType(_persistenceMethods[name]);
if (type != null)
    SessionService.SessionManager = Activator.CreateInstance(type) as IPersistenceMethod;
```

## Health Monitoring implementatie, keuzes en algoritmiek
Health monitoring gebeurt op twee verschillende manieren.
### IsOnline
Dit is de eenvoudigste van de twee. Voordat een server wordt toegevoegd aan de lijst van beschikbare servers, wordt er gekeken of een TcpClient kan connecten met deze server. Als dit niet het geval is wordt de server niet toegevoegd.
```C#
public Server AddServer(string address, int port)
{
    var server = new Server(address, port);
    try
    {
        using (var tcpClient = new TcpClient())
        {
            tcpClient.Connect(server.Address, server.Port);
        }
    }
    catch
    {
        return null;
    }
    SessionService.AddServer(server);
    return server;
}
```
### IHealthMonitor
Voor meer actieve healthMonitoring kan de IHealthMonitor interface worden gebruikt. 
#### ResponseTimeMonitor
De ResponseTimeMonitor bekijkt de responsetime van de server. Als de server geen response geeft, betekent het dat de server offline is.
```C#
/// <summary>
/// Gets the time it takes between sending the request and receiving the last byte
/// </summary>
/// <param name="server"></param>
/// <returns></returns>
public async Task<long> Ping(Server server)
{
    var message = "HEAD / HTTP/1.1\r\n" +
                    $"Host: {server.Address}\r\n" +
                    "Connection: keep-alive\r\n" +
                    "Content-Length: 0\r\n\r\n";
    var serverClient = new TcpClient(server.Address, server.Port);
    var bytes = Encoding.ASCII.GetBytes(message);
    try
    {
        var sentTime = DateTime.Now;
        serverClient.GetStream().Write(bytes, 0, bytes.Length);

        Array.Clear(bytes, 0, bytes.Length);
        bytes = new byte[2048];
        await serverClient.GetStream().ReadAsync(bytes, 0, bytes.Length);
        var receiveTime = DateTime.Now;
        TimeSpan span = receiveTime - sentTime;
        return (int)span.TotalMilliseconds;
    }
    catch
    {
        return -1;
    }
}
```
#### Toevoegen van nieuwe HealthMonitors
Zie beschrijving van het toevoegen van nieuwe Persistence Methods, maar dan op basis van de IHealthMonitor interface.

## Reflectie
Samenvatting van dingen waar ik trots op ben en wat nog beter kan
### Ups!
Ik ben zeer tevreden met het resultaat. Ik heb mijn code goed onderverdeeld in korte functies, classes, services en interfaces.
Load balance methodes, Session Persistence methodes en HealthMonitors kunnen eenvoudig worden toegevoegd met aleen minimale aanpassing van de code bij het toevoegen van Load Balance Methods. 

Ik heb goede namen gebruikt voor functies, waardoor op de meeste plaatsen weinig tot geen commentaar geplaatst hoeft te worden en overal heb ik gebruik gemaakt van async await om asynchrone taken uit te voeren.

Door de interface is het duidelijk te zien welke servers online zijn die zijn toegevoegd en met de checkbox om session persistence aan/uit te zetten is de werking hiervan eenvoudig zichtbaar te maken.

### Downs!
Achteraf gezien had ik de Load Balance methodes ook als Interface neer kunnen zetten om dit net als de twee andere interfaces volledig dynamisch af te laten handelen.

Ik heb geen onderzoek gedaan naar wat de standaarden zijn betreft mijn gebruikte dynamische initialisatie van classes. Dus wellicht heb ik hier wat gemaakt wat volledig buiten de standaarden van C# valt. Mocht dit het geval zijn, zou ik het niet aanpassen. Ik ben erg tevreden met deze dynamische aanpak.

Het mappen van requests voelt toch altijd nog wat kwetsbaar. De request/repsonse gaat door een algoritme heen welke deze omzet naar een `Dictionary<string, string>`. Het werkt zoals het staat, het lijkt ook goed te werken. Maar het is gewoon niet mooi. Dit zou kunnen worden opgelost door geen gebruik te maken raw streams, maar door HttpListener te gebruiken. Dan wordt er met objecten gewerkt waar de properties simpelweg kunnen worden aangepast en hoeft al dat gepuzzel niet te gebeuren.

Ik heb door de applicatie Thread.Sleep(int) staan. Niet netjes, maar soms lijkt er niets anders op te zitten dan even geduldig wachten op een response van de server. Als ik een een product als dit voor productie zou bouwen zou ik onderzoek doen hoe dit beter opgelost kan worden. Mijn vermoeden is namelijk dat dit allemaal wordt afgevangen als er gebruik wordt gemaakt van moderne methodieken als HttpListener.















