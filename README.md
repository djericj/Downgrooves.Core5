# About this project
.NET Core 6 WebAPI.  This application provides a REST API interface for reading and writing data.  The data store is pure json data files.
The API is consumed by the Downgrooves web site at http://www.downgrooves.com as well as the Downgrooves Mobile App.

# Why no database?
There used to be a SQLite database as a backing data store.  However, the majority of this site is static data that rarely changes.  The only part of the site that changes regularly is the Releases and Remixes sections.  Both of these sections get data from the iTunes Music API, which returns queries in JSON format.  The WorkerService is responsible for periodically querying the iTunes Music API for specific terms (e.g. artist name) and storing that query data locally as JSON.  The WorkerService then parses the returned query JSON to look for new albums or singles. If one or more are found, the WorkerService then queries for each new album to get the details.  Once again this data is returned from the API as JSON.  New albums' JSON data is then stored on the file system.

In the end, a database proved to be unnecessary for such a simple dataset.  It proved to be much more error prone and difficult to parse the JSON files and attempt to insert/update/delete records from the database for new and updates data.  It was simply an unnecessary abstraction.  

# About Downgrooves
Downgrooves.com is my personal music project website.  As a DJ and music producer for over 25 years, I have ammased an extensive collection of original music, dj sets and performance YouTube videos.  
This site collects all those resources into a single space.

The original source for the Angular web site that consumes this API can be found at my GitHub at github.com/djericj/Downgrooves
The original source for the Xamarin Forms Mobile app that consumes this API can be found at my GitHub at github.com/djericj/Downgrooves.Mobile

This site and it's companion projects (Downgrooves.Angular, Downgrooves.Mobile) are full participants in the Software Development Lifecycle on Azure DevOps.  Through Azure DevOps, the site is configured with complete build pipelines and releases to a local Development server and an AWS-hosted Production environment.  Both environments are Linux-based hosts for optimum performance and low cost of hosting.
