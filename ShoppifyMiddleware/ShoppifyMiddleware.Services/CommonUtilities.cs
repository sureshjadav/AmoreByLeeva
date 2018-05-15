using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppifyMiddleware.Services
{
    public enum ProductStatus
    {
        Created = 0,
        Verified = 1,
        ReadyToBeUploaded = 2,
        Uploaded = 3,
        Discontinued = 4
    }
}
