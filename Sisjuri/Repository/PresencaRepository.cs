using Sisjuri.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sisjuri.Repository
{
    public class PresencaRepository
    {
        private DataModel _DataModel;

        public DataModel DataModel
        {
            get {
                if (_DataModel == null)
                    _DataModel = new DataModel();
                return _DataModel; }
            set { _DataModel = value; }
        }

        #region Metodos

        public presenca GetOne(int id)
        {
            return DataModel.presenca.FirstOrDefault(e => e.id_presenca == id);
        }


        public void Create(presenca entity)
        {
            Save(entity);
        }

        public void Edit(presenca entity)
        {
            Save(entity);
        }

        public void Save(presenca entity)
        {
            DataModel.Entry(entity).State = entity.id_presenca == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(presenca entity)
        {
            DataModel.presenca.Remove(entity);
            DataModel.SaveChanges();
        }

        public List<presenca> GetAll()
        {
            return DataModel.presenca.ToList();
        }

        public presenca GetOneByJuriAndPresenca(int idJuri, int idUsuario)
        {
            return DataModel.presenca.FirstOrDefault(e => e.pfk_id_juri == idJuri && e.inscricao.pfk_id_usuario == idUsuario);
        }

        public List<presenca> GetAllByCriteria(string nome_completo, int id_funcao, int pfk_id_juri)
        {
            List<presenca> lst = DataModel.presenca.ToList();

            if (!string.IsNullOrEmpty(nome_completo))
            {
                lst = DataModel.presenca.Where(e => e.inscricao.usuario.nome_completo == nome_completo).ToList();
            }

            if (pfk_id_juri != 0)
            {
                lst = lst.Where(e => e.pfk_id_juri == pfk_id_juri).ToList();
            }

            if (id_funcao != 0)
            {
                lst = lst.Where(e => e.inscricao.funcao.id_funcao == id_funcao).ToList();
            }

            return lst.ToList();

        }

        public List<presenca> GetAllByAutorizados(int id)
        {
            return DataModel.presenca.Where(e => e.inscricao.fk_id_juri == id && e.inscricao.status_inscricao == "Autorizada").ToList();
        }

        #endregion

    }
}
 