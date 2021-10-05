using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.MongoDB.BaseTypes
{
    public interface IEntity
    {
    }

    public interface IEntity<out TKey> : IEntity where TKey : IEquatable<TKey>
    {
        public TKey Id { get; }
        DateTime CreatedAt { get; set; }
    }
}
