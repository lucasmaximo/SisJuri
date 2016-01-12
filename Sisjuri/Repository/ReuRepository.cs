using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class ReuRepository
    {
        private DataModel _DataModel;

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

        #region Métodos

        public reu GetOne(int id)
        {
            return DataModel.reu.FirstOrDefault(e => e.id_reu == id);
        }

        public reu GetOne(string nome_reu)
        {
            return DataModel.reu.FirstOrDefault(e => e.nome_reu == nome_reu);
        }

        public void Create(reu entity)
        {
            Save(entity);
        }

        public void Edit(reu entity)
        {
            Save(entity);
        }

        public void Save(reu entity)
        {
            DataModel.Entry(entity).State = entity.id_reu == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(reu entity)
        {
            DataModel.reu.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<reu> GetAll()
        {
            return DataModel.reu.ToList();
        }
        public List<reu> GetAllByIdProcess(int id)
        {
            return DataModel.reu.Where(e => e.fk_id_processo == id).ToList();
        }
        public List<reu> GetAllByCriteria(string nome_reu)
        {
            return DataModel.reu.Where(e => e.nome_reu == nome_reu).ToList();
        }

        #endregion
    }
}