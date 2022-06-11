select 
json_extract( value, '$.wrapperType' ) WrapperType,
json_extract( value, '$.collectionType' ) collectionType,
json_extract( value, '$.artistId' ) artistId,
json_extract( value, '$.artistName' ) artistName,
json_extract( value, '$.collectionCensoredName' ) collectionCensoredName,
json_extract( value, '$.artistViewUrl' ) artistViewUrl,
json_extract( value, '$.collectionViewUrl' ) collectionViewUrl,
json_extract( value, '$.artworkUrl60' ),
json_extract( value, '$.artworkUrl100' ),
json_extract( value, '$.collectionPrice' ),
json_extract( value, '$.collectionExplicitness' ),
json_extract( value, '$.trackCount' ),
json_extract( value, '$.copyright' ),
json_extract( value, '$.country' ),
json_extract( value, '$.currency' ),
json_extract( value, '$.releaseDate' ),
json_extract( value, '$.primaryGenreName' ),
apiData.lastUpdate, value, type, atom, id, parent, fullkey, path
from 
(
select artist, data, lastUpdate from apiData 
where artist = 'Downgrooves' 
and apiDataType = 1
order by lastUpdate DESC
) as apiData, 
json_each( apiData.data ) 
where json_extract( value, '$.wrapperType' ) = 'collection'
order by json_extract( value, '$.collectionId' )