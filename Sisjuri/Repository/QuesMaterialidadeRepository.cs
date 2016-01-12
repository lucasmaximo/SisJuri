using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class QuesMaterialidadeRepository
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

        public ques_materialidade GetOne(int id)
        {
            return DataModel.ques_materialidade.FirstOrDefault(e => e.id_quesito_materialidade == id);
        }

        public ques_materialidade GetOne(string quesito_materialidade)
        {
            return DataModel.ques_materialidade.FirstOrDefault(e => e.quesito_materialidade == quesito_materialidade);
        }

        public void Create(ques_materialidade entity)
        {
            Save(entity);
        }

        public void Edit(ques_materialidade entity)
        {
            Save(entity);
        }

        public void Save(ques_materialidade entity)
        {
            DataModel.Entry(entity).State = entity.id_quesito_materialidade == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(ques_materialidade entity)
        {
            DataModel.ques_materialidade.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<ques_materialidade> GetAll()
        {
            return DataModel.ques_materialidade.ToList();
        }
        public List<ques_materialidade> GetAllByIdProcess(int id)
        {
            return DataModel.ques_materialidade.Where(e => e.fk_id_processo == id).ToList();
        }
        public List<ques_materialidade> GetAllByCriteria(int id_ques_mat)
        {
            return DataModel.ques_materialidade.Where(e => e.id_quesito_materialidade == id_ques_mat).ToList();
        }

        #endregion
    }
}