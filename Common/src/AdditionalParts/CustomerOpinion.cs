using System;
using System.Collections.Generic;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class CustomerOpinion
    {
        public CustomerOpinion(Guid deliveryId, string comment, IEnumerable<KeyValuePair<Guid, int>> marks)
            : this()
        {
            Id = Guid.NewGuid();
            DeliveryId = deliveryId;
            Comment = comment;

            foreach (var mark in marks)
            {
                Marks.Add(mark.Key, mark.Value);
            }
        }
    }
}