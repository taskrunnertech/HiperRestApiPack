# HiperRestApiPack
Rest Api Helper, Select special fields, Paging

Run the test application and try following queries :

http://localhost:5000/api/product?OrderBy=Id&page=4&pagesize=2&select=Name,Price

```
{
"items": [
{
"name": "Candy Land 7",
"price": 6.2
}
],
"index": 4,
"size": 2,
"totalCount": 7,
"totalPages": 4,
"hasPreviousPage": true,
"hasNextPage": false,
"sum": null
}

```

http://localhost:5000/api/product?OrderBy=Id&Order=desc&page=2&pagesize=2


```

{
"items": [
{
"id": 5,
"name": "Candy Land 5",
"price": 4.2,
"variants": []
},
{
"id": 4,
"name": "Candy Land 4",
"price": 2.2,
"variants": []
}
],
"index": 2,
"size": 2,
"totalCount": 7,
"totalPages": 4,
"hasPreviousPage": true,
"hasNextPage": true,
"sum": null
}

```

