# HiperRestApiPack
Rest Api Helper, Select special fields, Paging
This project provide a simple helper for you to add paging and custom field filtering to your .net core Web API

# Install
For using with entity framework
```
Install-Package HiperRestApiPack.EF 
```
or if you need to use it with MongoDB
```
Install-Package HiperRestApiPack.Mongo
```
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
# Ignore fileds
You can add [IgnoreField] attribute to the field to ignore return field in result permanently 

```
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [IgnoreField]
        public int SomeIgnoredField { get; set; }

        public List<Variant> Variants { get; set; }

    }
```
# Request Model
To have a custom request parameters for get method crate a request model which inherit from PageRequest
```
    public class SampleResuest : PagedRequest
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
    }
```
PagedRequest Automatically provide some paramters for you get method
```
    public class PagedRequest
    {
        public string Select { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public OrderType Order { get; set; }
        public string OrderBy { get; set; }
        public string Search { get; set; }
    }
```
No in version 2.0 we use dynamic linq and you can use these queries

http://localhost:5000/api/product?select=new (id as id, name as name, Variants.Select(new (Id,Description)) as variants)

http://localhost:5000/api/product?select=new (id, name, variants.Select(new (id,description)) as variants)

http://localhost:5000/api/product?select=new (id , name, variants.Select(new (id,description)) as vars, variants.count() as count)

http://localhost:5000/api/product?select=new (id , name, variants.Count() as count)

http://localhost:5000/api/product?sum=price

http://localhost:5000/api/product/1?select=new(Id as id,name as name)
we have previous response and new response if you need:
```
{
  "data": {
    "items": [
      {
        "Id": 1,
        "Name": "Candy Land 1",
        "variantCount": 3
      },
      {
        "Id": 2,
        "Name": "Candy Land 2",
        "variantCount": 3
      }
    ],
    "index": 1,
    "size": 10,
    "totalCount": 7,
    "totalPages": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
  },
  "success": true,
  "errorCode": null,
  "message": null
}
```

