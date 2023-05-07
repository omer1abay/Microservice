using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Catalog.Models
{
    public class Course
    {
        [BsonId] //mongodb tarafında id olduğunu belirt
        [BsonRepresentation(BsonType.ObjectId)] //mongodb tarafında tutulan objectid'yi string'e çevirecek string gönderirsek de objectid'ye çevirir
        public string Id { get; set; }
        public string Name { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string Picture { get; set; }

        public string Description { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedTime { get; set; }
        public string UserId { get; set; }
        public Feature Feature { get; set; }

        [BsonRepresentation(BsonType.ObjectId)] //asıl sınıfta objectid ise burada da öyle işaretlememiz lazım
        public string CategoryId { get; set; }

        [BsonIgnore] //mongodb'de karşılığı olmayacak collection'a satır olarak yansıtırken göz ardı eder. Kendi içimizde kullancağız 
        public Category Category { get; set; }

    }
}
