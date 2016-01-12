using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class VListadePresencaRepository
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

        public List<vlistadepresenca> GetAllByInscritos(int id)
        {
            return DataModel.vlistadepresenca.Where(e => e.id_juri == id).OrderBy(e => e.nome_aluno).ToList();
        }
    }
}