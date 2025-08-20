using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Ordering.Domain.Abstractions
{
    public interface IDomainEvent : INotification
    {
        Guid EventId => Guid.NewGuid();
        public DateTime OccurrendOn => DateTime.Now;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}
