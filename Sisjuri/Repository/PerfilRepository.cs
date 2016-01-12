using Sisjuri.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sisjuri.Repository
{
    public class PerfilRepository
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

        public perfil GetOne(int id)
        {
            return DataModel.perfil.FirstOrDefault(e => e.id_perfil == id);
        }

        public perfil GetOne(string perfil)
        {
            return DataModel.perfil.FirstOrDefault(e => e.nome_perfil == perfil);
        }   
        public void Create(perfil entity)
        {
            Save(entity);
        }

        public void Edit(perfil entity)
        {
            Save(entity);
        }

        public void Save(perfil entity)
        {
            DataModel.Entry(entity).State = entity.id_perfil == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(perfil entity)
        {
            DataModel.perfil.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<perfil> GetAll()
        {
            return DataModel.perfil.ToList();
        }   
        public List<perfil> GetAllByCriteria(string perfil)
        {
                return DataModel.perfil.Where(e => e.nome_perfil == perfil).ToList();
        }

        #endregion
    }
}