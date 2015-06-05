Development environment
------------------------
1. Visual Studio Community 2013
2. .Net 4.5
3. Web API 2.2
4. SQL Server 2012 (MusicStore.mdf, MusicStore.ldf included)
5. Entity Framework Version 6.1.3
6. Ninject
7. Json.Net

Functionalities implemented
-------------------------------------
1. Get album details by name. (Return information about Songs in album.)
Class - AlbumsController
Method - public IHttpActionResult Get(string name)
Route - /api/albums/{name}

2. Add songs to an album
Class - SongsController
Method - public IHttpActionResult Post(int albumId, [FromBody]SongModel songModel)
Route - /api/albums/{albumId}/songs

3. Update song info
Class - SongsController
Method - public IHttpActionResult Patch(int albumId, int id, [FromBody] SongModel songModel)
Route - /api/albums/{albumId}/songs/{id}

For Add and Update funcitonalities, please provide a song json object in request body

4. Concurrency implemented by disabling session State in Web.config
	<sessionState mode="Off"/>

5. Cached xml file using MemoryCache class(System.Runtime.Caching)

6. Implemented dependency injection by using third party library Ninject. 
Currently, the data source is SQL Db. You can switch it to XML by pointing IMusicStoreRepository dependency to MusicStoreXmlRepository in /App_Start/NinjectWebCommon.cs in the method private static void RegisterServices(IKernel kernel) 

7. Get all albums with or without songs
Class - AlbumsController
Method - public IHttpActionResult Get(bool includeSongs=true)
Route - /api/albums (to include songs)
	/api/albums?includeSongs=false (to exclude songs)

8. Get songs for an album
Class - SongsController
Method - public IHttpActionResult Get(int albumId)
Route - /api/albums/{albumId}/songs

9. Get a particular song for an album
Class - SongsController
Method - public IHttpActionResult Get(int albumId, int id)
Route - /api/albums/{albumId}/songs/{id}





