using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MongoDb.ASP.NETCore3CRUDSample.DataAccess.Models
{
    [BsonIgnoreExtraElements]  //This will instruct the driver to ignore any elements that it cannot deserialize into a corresponding property
    public class Customer
    { 
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int CustomerID { get; set; }

        [BsonElement]
        [Required]
        public string Name { get; set; }

        [BsonElement]
        [Required]
        [Range(15, 100, ErrorMessage = "Age should be between 15 and 100 years" )]
        public int Age { get; set; }

        [BsonElement]
        [Required]
        [Range(1000, 10000)]
        public int Salary { get; set; }

        
    }
}
