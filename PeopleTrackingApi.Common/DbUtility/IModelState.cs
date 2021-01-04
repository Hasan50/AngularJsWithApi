using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleTrackingApi.Common
{
    public interface IModelState
    {
        ModelState ModelState
        {
            get;
            set;
        }
    }
}
