using Sisjuri.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sisjuri.Repository
{
    public class FuncaoRepository
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

        #region Métodos

        public funcao GetOne(int id)
        {
            return DataModel.funcao.FirstOrDefault(e => e.id_funcao == id);
        }

        public funcao GetOne(string nome_funcao)
        {
            return DataModel.funcao.FirstOrDefault(e => e.nome_funcao == nome_funcao);
        }

        public void Create(funcao entity)
        {
            Save(entity);
        }

        public void Edit(funcao entity)
        {
            Save(entity);
        }

        public void Save(funcao entity)
        {
            DataModel.Entry(entity).State = entity.id_funcao == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(funcao entity)
        {
            DataModel.funcao.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<funcao> GetAll()
        {
            return DataModel.funcao.ToList();
        }
        public List<funcao> GetAllByCriteria(string Funcao)
        {
            return DataModel.funcao.Where(e => e.nome_funcao == Funcao).ToList();
        }

        #endregion

    }
}