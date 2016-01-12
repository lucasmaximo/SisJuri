using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class VHistoricoJuriRepository
    {
        DataModel _DataModel;

        public DataModel DataModel
        {
            get {
                if (_DataModel == null)
                    _DataModel = new DataModel();
                return _DataModel; 
            }
            set { _DataModel = value; }
        }

        public List<vhistoricojuri> GetOneByIdJuri(int idJuri)
        {
            return DataModel.vhistoricojuri.Where(e => e.id_juri == idJuri).ToList();
        }

    }
}