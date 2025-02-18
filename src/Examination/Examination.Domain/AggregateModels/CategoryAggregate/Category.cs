using System.Diagnostics.CodeAnalysis;
using Examination.Domain.SeedWork;
using MongoDB.Bson.Serialization.Attributes;

namespace Examination.Domain.AggregateModels.CategoryAggregate
{
    public class Category : Entity
    {
        [SetsRequiredMembers]
        public Category(string id, string name, string urlPath) {
            Id = id;
            Name = name;
            UrlPath = urlPath;
        } 
        
        [BsonElement("name")]
        public required string Name { get; set; }
        [BsonElement("urlPath")]
        public required string UrlPath { get; set; }
    }
}