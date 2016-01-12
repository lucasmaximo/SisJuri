using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class VSorteioFuncoesRepository
    {
        DataModel _DataModel;

        public DataModel DataModel
        {
            get
            {
                if (_DataModel == null)
                    _DataModel = new DataModel();
                return _DataModel;
            }
            set { _DataModel = value; }
        }

        public List<vsorteiofuncoes> GetAllBySortJuriAndExport(int id)
        {
            return DataModel.vsorteiofuncoes.Where(e => e.id_juri == id).OrderBy(e => e.nome_funcao).ToList();
        }

    }
}