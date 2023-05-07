using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Catalog.Models
{
    public class Category
    {
        [BsonId] //mongodb tarafında id olduğunu belirt
        [BsonRepresentation(BsonType.ObjectId)] //mongodb tarafında tutulan objectid'yi string'e çevirecek string gönderirsek de objectid'ye çevirir
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
