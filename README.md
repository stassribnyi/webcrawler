# webcrawler
An attempt to implement Web Crawler (BFS) using Web API Core and ReactJS.

Implemented ReactJs UI part with flux architecture.
Connection to the api was done with Axios api library.

Backend part was implemented with ASP.NET Core and HtmlAgilityPack.

Life search process update was implemented with two signalR clients, one on the react side and one on the .net core side.

Also there was an attempt to implement pause/resume of several threads that runs inside BFS, but it still requires some better implementation,
as it not always works. Consider to implement some kind of queue instead of reassignment of ParallelService.
