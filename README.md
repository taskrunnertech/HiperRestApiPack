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


