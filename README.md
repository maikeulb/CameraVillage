# RolleiShop

E-commerce application where users may browse the catalog, manage their
shopping cart, submit orders (dummy or through stripe), and browse through
their order history. Admin users may manage the catalog (add and edit catalog
items). Features include distributed caching for the catalog items (via Redis),
in-memory catching for the catalog types and brands, localization, server-side
filtering (via specification pattern), and emails (via MailKit).

Architecture is vertically sliced, CQRS, with a rich, encapsulated
domain<sup>1</sup> (private collections and setters) with domain notifications
(for sending emails and product price changes) to explicitly implement side
effects. The application IO is fully-asynchronous and the errors are handled
with command results (similar to F#'s Option Type or Haskell's Maybe monad).

1. The only exception to this (as far as I'm aware) is the ApplicationUser
   class which references Identity Framework.

Technology
----------
* ASP.NET Core 2.0
* PostgreSQL
* Redis
* Identity 2.0
* Entity Framework Core 2.0 
* MediatR
* FluentValidation
* NLog
* CSharpFunctionalExtensions
* Semantic UI
* Stripe API
* Noty
* Rellax
* Google Maps API

Screenshots
---
### Index 
The index and layout templates are translated to Japanese thanks to Google
translate (Most likely not too accurate).
![index](/screenshots/index.png?raw=true "Index")
***
![japanese](/screenshots/japanese.png?raw=true "Japanese")
### Catalog  
Filter catalog-items by brand and/or format. 
![catalog](/screenshots/catalog.png?raw=true "Catalog")
### Cart
Displays cart items with the abilities to update, clear, and checkout.
Checking out the cart publishes a domain notification to send customer emails.
![cart](/screenshots/cart.png?raw=true "Cart")
***
![mail](/screenshots/mail.png?raw=true "Mail")
### Orders - Details
Displays your detailed order history.
![order](/screenshots/order.png?raw=true "Order")
### Admin 
Admin users may manage the catalog (and inventory). Updating the product price publishes
a domain notification to reflect the price change in the customer's cart.
![admin](/screenshots/admin.png?raw=true "Admin")
***
![add](/screenshots/add.png?raw=true "Add")

Run
---

With docker:
```
docker-compose build
docker-compose up
Go to http://localhost:5000
```
Alternatively, you will need .NET Core 2.0 SDK. If you have the SDK installed,
then open `appsettings.Development.json` and point the connection strings to
your PostgreSQL and Redis servers. Install the javascript dependencies (e.g.
`npm install`). You may optionally fill out the credentials for the mail
server.

`cd` into `./src/RolleiShop` (if you are not already); then run:
```
webpack build
dotnet restore
dotnet ef database update -c ApplicationDbContext
dotnet ef database update -c IdentityDbContext
dotnet run
Go to http://localhost:5000
```

Deploy (Dockerized hosts)
---
This process is more thoroughly explained
[here](https://www.digitalocean.com/community/tutorials/how-to-provision-and-manage-remote-docker-hosts-with-docker-machine-on-ubuntu-16-04),
but I'll summarize the steps required (mostly for my own reference).

Prerequisites: Docker Machine installed on your local machine and DigitalOcean
API token.

1. Create Dockerized hos
```
docker-machine create --driver digitalocean --digitalocean-access-token
$DOTOKEN machine-name
```
2. Activate Dockerized host
```
eval (docker-machine env machine-name)
```
3. Build and run containers
```
docker-compose -f docker-compose.yml -f docker-compose.prod.yml build
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```
4. Unset Dockerized host
```
eval (docker-machine env -u)
```

NOTE
----
The resources I use to create this project were plentiful, coming from several
projects and tutorials provided by Microsoft (mostly eShopOnWeb,
eShopOnContainers, MVCMusicStore, and ContosoUniversity), Pluralsight, Jimmy
Bogards Contoso University remake, and several blogs.

TODO
----
configure webpack (for production)  
Add more unit tests  
Add Serverside sorting by price  
Fix AJAX remove cart-items (low priority)  
