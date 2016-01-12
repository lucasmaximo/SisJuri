using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class QuesTentativaRepository
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

        public ques_tentativa GetOne(int id)
        {
            return DataModel.ques_tentativa.FirstOrDefault(e => e.id_quesito_tentativa == id);
        }

        public ques_tentativa GetOne(string quesito_tentativa)
        {
            return DataModel.ques_tentativa.FirstOrDefault(e => e.quesito_tentativa == quesito_tentativa);
        }

        public void Create(ques_tentativa entity)
        {
            Save(entity);
        }

        public void Edit(ques_tentativa entity)
        {
            Save(entity);
        }

        public void Save(ques_tentativa entity)
        {
            DataModel.Entry(entity).State = entity.id_quesito_tentativa == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(ques_tentativa entity)
        {
            DataModel.ques_tentativa.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<ques_tentativa> GetAll()
        {
            return DataModel.ques_tentativa.ToList();
        }
        public List<ques_tentativa> GetAllByIdProcess(int id)
        {
            return DataModel.ques_tentativa.Where(e => e.fk_id_processo == id).ToList();
        }
        public List<ques_tentativa> GetAllByCriteria(string quesito_tentativa)
        {
            return DataModel.ques_tentativa.Where(e => e.quesito_tentativa == quesito_tentativa).ToList();
        }

        #endregion
    }
}