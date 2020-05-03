# HiperRestApiPack
Rest Api Helper, Select special fields, Paging

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
