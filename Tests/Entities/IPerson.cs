using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Entities
{
    public interface IPerson : IIdentifiable<int>
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
