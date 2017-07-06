using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpStore
{
    public interface IParametre: IStoreModel
    {
        int Numero { get; set; }

        string Code { get; set; }

        string Description { get; set; }
    }
}
