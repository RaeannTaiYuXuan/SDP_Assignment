﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.Jason.ITERATOR
{
    // Aggregate interface
    public interface IAggregate
    {
        IEnumerator<Document> GetEnumerator();
    }
}
