using jcReactive.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpStore
{
    public interface IStoreModel: IReactiveDbObject
    {
        Guid Key { get; set; }

        string Societe { get; set; }

        #region Lock Properties
        string LockBy { get; set; }
        DateTime? LockAt { get; set; }
        #endregion

        #region Tracability
        string CreatedBy { get; set; }
        DateTime? CreatedAt { get; set; }

        string ModifiedBy { get; set; }
        DateTime? ModifiedAt { get; set; }
        #endregion
    }
}
