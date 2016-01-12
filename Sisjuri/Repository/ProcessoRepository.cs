using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class ProcessoRepository
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

        public processo GetOne(int id)
        {
            return DataModel.processo.FirstOrDefault(e => e.id_processo == id);
        }

        public processo GetOne(string num_processo)
        {
            return DataModel.processo.FirstOrDefault(e => e.num_processo == num_processo);
        }

        public void Create(processo entity)
        {
            Save(entity);
        }

        public void Edit(processo entity)
        {
            Save(entity);
        }

        public void Save(processo entity)
        {
            DataModel.Entry(entity).State = entity.id_processo == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(processo entity)
        {
            DataModel.processo.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<processo> GetAll()
        {
            return DataModel.processo.ToList();
        }
        public List<processo> GetAllByCriteria(string num_processo, int fk_id_juri)
        {
            if (!string.IsNullOrEmpty(num_processo) && (fk_id_juri == 0))
            {
                return DataModel.processo.Where(e => e.num_processo == num_processo).ToList();
            }

            if (!string.IsNullOrEmpty(num_processo) && (fk_id_juri != 0))
            {
                return DataModel.processo.Where(e => e.num_processo == num_processo
                    && e.fk_id_juri == fk_id_juri).ToList();
            }
            else
            {
                return DataModel.processo.Where(e => e.fk_id_juri == fk_id_juri).ToList();
            }
        }

        #endregion
    }
}