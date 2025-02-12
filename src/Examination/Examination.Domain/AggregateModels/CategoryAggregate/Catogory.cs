using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Examination.Domain.AggregateModels.CategoryAggregate
{
    public class Catogory
    {
        [BsonElement("name")]
        public required string Name { get; set; }
        [BsonElement("urlPath")]
        public required string UrlPath { get; set; }
    }
}