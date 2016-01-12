using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class VitimaRepository
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

        public vitima GetOne(int id)
        {
            return DataModel.vitima.FirstOrDefault(e => e.id_vitima == id);
        }

        public vitima GetOne(string nome_vitima)
        {
            return DataModel.vitima.FirstOrDefault(e => e.nome_vitima == nome_vitima);
        }

        public void Create(vitima entity)
        {
            Save(entity);
        }

        public void Edit(vitima entity)
        {
            Save(entity);
        }

        public void Save(vitima entity)
        {
            DataModel.Entry(entity).State = entity.id_vitima == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(vitima entity)
        {
            DataModel.vitima.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<vitima> GetAll()
        {
            return DataModel.vitima.ToList();
        }
        public List<vitima> GetAllByIdProcess(int id)
        {
            return DataModel.vitima.Where(e => e.fk_id_processo == id).ToList();
        }
        public List<vitima> GetAllByCriteria(string nome_vitima)
        {
            return DataModel.vitima.Where(e => e.nome_vitima == nome_vitima).ToList();
        }

        #endregion
    }
}