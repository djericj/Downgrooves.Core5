delete from iTunesCollection where Artist = @artistName;
insert into iTunesCollection
(
CollectionId,
ArtistId,
ArtistName,
CollectionCensoredName,
ArtistViewUrl,
CollectionViewUrl,
ArtworkUrl60,
ArtworkUrl100,
CollectionPrice,
CollectionExplicitness,
TrackCount,
Copyright,
Country,
Currency,
ReleaseDate,
SourceArtistId,
PrimaryGenreName,
CollectionType,
WrapperType,
Artist)

select distinct
json_extract( value, '$.collectionId' ) CollectionId,
json_extract( value, '$.artistId' ) ArtistId,
json_extract( value, '$.artistName' ) ArtistName,
replace(json_extract( value, '$.collectionCensoredName' ), ' - Single', '') CollectionCensoredName,
json_extract( value, '$.artistViewUrl' ) ArtistViewUrl,
json_extract( value, '$.collectionViewUrl' ) CollectionViewUrl,
json_extract( value, '$.artworkUrl60' ) ArtworkUrl60,
json_extract( value, '$.artworkUrl100' ) ArtworkUrl100,
json_extract( value, '$.collectionPrice' ) CollectionPrice,
json_extract( value, '$.collectionExplicitness' ) CollectionExplicitness,
json_extract( value, '$.trackCount' ) TrackCount,
json_extract( value, '$.copyright' ) Copyright,
json_extract( value, '$.country' ) Country,
json_extract( value, '$.currency' ) Currency,
json_extract( value, '$.releaseDate' ) ReleaseDate,
(select artistId from artist where name = @artistName),
json_extract( value, '$.primaryGenreName' ) PrimaryGenreName,
json_extract( value, '$.collectionType' ) CollectionType,
json_extract( value, '$.wrapperType' ) WrapperType,
@artistName
from 
(
select artist, data, lastUpdate from apiData 
where artist = @artistName
and apiDataType = 1
order by lastUpdate DESC
) as apiData, 
json_each( apiData.data ) 
where json_extract( value, '$.collectionId' ) is not NULL
and json_extract( value, '$.wrapperType' ) = 'collection'
and CollectionId not in (select collectionId from iTunesExclusion WHERE collectionId is not null)
and CollectionId not in (select collectionId from iTunesCollection WHERE collectionId is not null)
order by json_extract( value, '$.collectionCensoredName' )