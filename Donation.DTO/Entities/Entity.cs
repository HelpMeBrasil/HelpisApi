using Donation.Domain.Contract;
using MongoDB.Bson;
using System;

namespace Donation.Domain.Entities
{
    public abstract class Entity : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}