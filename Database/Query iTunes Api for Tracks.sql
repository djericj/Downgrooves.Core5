select 
apiData.data
from 
(
select artist, data, lastUpdate from apiData 
where artist = 'Downgrooves' 
and apiDataType = 2
order by lastUpdate DESC
) as apiData, 
json_each( apiData.data ) 
--where json_extract( value, '$.wrapperType' ) = 'track'
order by json_extract( value, '$.collectionId' )