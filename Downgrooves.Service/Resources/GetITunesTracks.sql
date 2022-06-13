delete from iTunesTrack where Artist = @artistName;
insert into iTunesTrack
(
Kind,
CollectionId,
ArtistId,
TrackId,
ArtistName,
CollectionName,
TrackName,
CollectionCensoredName,
TrackCensoredName,
ArtistViewUrl,
CollectionViewUrl,
TrackViewUrl,
PreviewUrl,
ArtworkUrl30,
ArtworkUrl60,
ArtworkUrl100,
CollectionPrice,
TrackPrice,
ReleaseDate,
CollectionExplicitness,
TrackExplicitness,
DiscCount,
DiscNumber,
TrackCount,
TrackNumber,
TrackTimeMillis,
Country,
Currency,
PrimaryGenreName,
IsStreamable,
Artist
)
select 
json_extract( value, '$.kind' ) Kind,
json_extract( value, '$.collectionId' ) CollectionId,
json_extract( value, '$.artistId' ) ArtistId,
json_extract( value, '$.trackId' ) TrackId,
json_extract( value, '$.artistName' ) ArtistName,
json_extract( value, '$.collectionName' ) CollectionName,
json_extract( value, '$.trackName' ) TrackName,
json_extract( value, '$.collectionCensoredName' ) CollectionCensoredName,
json_extract( value, '$.trackCensoredName' ) TrackCensoredName,
json_extract( value, '$.artistViewUrl' ) ArtistViewUrl,
json_extract( value, '$.collectionViewUrl' ) CollectionViewUrl,
json_extract( value, '$.trackViewUrl' ) TrackViewUrl,
json_extract( value, '$.previewUrl' ) PreviewUrl,
json_extract( value, '$.artworkUrl30' ) ArtworkUrl30,
json_extract( value, '$.artworkUrl60' ) ArtworkUrl60,
json_extract( value, '$.artworkUrl100' ) ArtworkUrl100,
json_extract( value, '$.collectionPrice' ) CollectionPrice,
json_extract( value, '$.trackPrice' ) TrackPrice,
json_extract( value, '$.releaseDate' ) ReleaseDate,
json_extract( value, '$.collectionExplicitness' ) CollectionExplicitness,
json_extract( value, '$.trackExplicitness' ) TrackExplicitness,
json_extract( value, '$.discCount' ) DiscCount,
json_extract( value, '$.discNumber' ) DiscNumber,
json_extract( value, '$.trackCount' ) TrackCount,
json_extract( value, '$.trackNumber' ) TrackNumber,
json_extract( value, '$.trackTimeMillis' ) TrackTimeInMillis,
json_extract( value, '$.country' ) Country,
json_extract( value, '$.currency' ) Currency,
json_extract( value, '$.primaryGenreName' ) PrimaryGenreName,
json_extract( value, '$.isStreamable' ) IsStreamable,
@artistName
from 
(
select artist, data, lastUpdate from apiData 
where artist = @artistName 
and apiDataType = 2
order by lastUpdate DESC
) as apiData, 
json_each( apiData.data ) 
where TrackId not in (select TrackId from iTunesTrack where TrackId is not null)
order by json_extract( value, '$.collectionId' )