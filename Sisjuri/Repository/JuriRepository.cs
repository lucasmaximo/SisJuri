using System.Collections.Generic;
using System.Linq;
using Sisjuri.Models;
using System.Data.Entity;

namespace Sisjuri.Repository
{
    public class JuriRepository
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

        public juri GetOne(int id)
        {
            return DataModel.juri.FirstOrDefault(e => e.id_juri == id);
        }

        public juri GetOne(string Juri)
        {
            return DataModel.juri.FirstOrDefault(e => e.nome_juri == Juri);
        }

        public void Create(juri entity)
        {
            Save(entity);
        }

        public void Edit(juri entity)
        {
            Save(entity);
        }

        public void Save(juri entity)
        {
            DataModel.Entry(entity).State = entity.id_juri == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(juri entity)
        {
            DataModel.juri.Remove(entity);
            DataModel.SaveChanges();
        }

        public List<juri> GetAll()
        {
            return DataModel.juri.ToList();
        }

        public List<juri> GetAllByNotSorted()
        {
            return DataModel.juri.Where(e => e.juri_sorteado == false).ToList();
        }

        public List<juri> GetAllByCriteria(string nome_juri)
        {
            return DataModel.juri.Where(e => e.nome_juri == nome_juri).ToList();
        }

        #endregion
    }
}