using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class VGerarCertificadoRepository
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

        public List<vgerarcertificado> GetOneByJuriAndPresenca(int idJuri, int idUsuario)
        {
            List<vgerarcertificado> lst = new List<vgerarcertificado>();
            lst.Add(DataModel.vgerarcertificado.FirstOrDefault(e => e.id_juri == idJuri && e.id_usuario == idUsuario));
            return lst;
        }
    }
}