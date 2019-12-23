# Polls sample application

This folder contains a CRUD-like sample application. The intend of this sample is to demonstrate developing of a multi-service product through integration tests powered by Hodgepodge.NET. Another motivation is to improve Hodgepodge.NET as needed for the almost real life example.

## Domain description

Polls are conducted to gather feedback from users of some products or a set of products. These products can have different frontends: MPA and SPA web-applications, mobile native and hybrid applications, desktop application and more exotic examples. Let's imagine a big company that has many of different software products and wants to collect feedback about users' satisfaction in these products to present daily overall reports to management. As well the company wants to have possibility to conduct other marketing and transactional polls in different products, collect user feedback and produce reports.

In terms of the product other products called 'showcases'. They should have integrated polls client that is able to:

* Periodically get client configuration from backend. The configuration consists mostly of different time delays.
* Periodically get list of polls for this kind of showcases from backend.
* React on events that occurs in the showcase with displaying invitation for the user to answer a poll and if the user is agree display this poll.
* Send to the backend poll results. The result could be disagreement of the user to answer a poll or answers to all or a part of questions of the poll.

On the other end of the polls product live polls editors, product administrators and showcase holders. They should have some kind of management console application that is able to:

* Change the list of showcases.
* Create polls consist of questions and proposed answers.
* Set client configuration.
* Link polls to showcases using events that can happen in showcases.

## The sample application requirements and constraints

The sample application should not implement all parts of the real live product. There shouldn't be any kind of clients and frontend, only backend code and integration tests that mimic communications with those clients and frontends. 

The real life polls product has much more wide requirements. For instance there could be requirement to not bother a user often than one poll invitation in a few month. Or it should be required to conduct only the limited number of poll of the same kind during a day.

The structure of polls content isn't important, it is just some JSON that will be interpreted by poll views in showcases. But different showcases can have different poll view templates -- web applications that show the poll to the user. Templates can differ by theirs visual appearance and behavior. But again for the sake of simplicity in this sample application templates represent only as meta-data.

The polls sample should consist of following components:

1. Client BFF -- a REST API web-application that clients use to communicate with the product.
2. Administration BFF -- a REST API web-application that administration console uses to communicate with the product.
3. Configuration micro-service -- keeps and controls all configuration data: client configuration, showcases, poll view templates, polls, links like showcase-template, showcase-event-poll. It keeps it's data in a Redis database and exposes it's functionality through a REST API.
4. Poll results micro-service -- collects poll results from clients and allows to generate reports by this data. It keeps it's data in a PostgreSQL database and exposes it's functionality through a REST API.

All components should be deployed as independent web-applications in docker containers written using ASP.NET Core and .NET Core, build using docker files. Integration tests should be written using [xUnit](https://xunit.net/), Hodgepodge.NET for the technical tests and using same libraries plus [SpecFlow](https://specflow.org/) for end-to-end tests of use cases.

### Client 

* UC001. Get client configuration.
* UC002. Get polls for a showcase.
* UC003. Get poll view templates meta-data for a showcase.
* UC004. Register poll results.

### Administration

* UC005. Set client configuration.
* UC006. Manage polls.
* UC007. Manage poll view templates.
* UC008. Manage showcases.
* UC009. Add poll for an event in a showcase.

























