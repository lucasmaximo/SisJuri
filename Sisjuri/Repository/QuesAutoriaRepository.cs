using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class QuesAutoriaRepository
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

        public ques_autoria GetOne(int id)
        {
            return DataModel.ques_autoria.FirstOrDefault(e => e.id_quesito_autoria == id);
        }

        public ques_autoria GetOne(string quesito_autoria)
        {
            return DataModel.ques_autoria.FirstOrDefault(e => e.quesito_autoria == quesito_autoria);
        }

        public void Create(ques_autoria entity)
        {
            Save(entity);
        }

        public void Edit(ques_autoria entity)
        {
            Save(entity);
        }

        public void Save(ques_autoria entity)
        {
            DataModel.Entry(entity).State = entity.id_quesito_autoria == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(ques_autoria entity)
        {
            DataModel.ques_autoria.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<ques_autoria> GetAll()
        {
            return DataModel.ques_autoria.ToList();
        }
        public List<ques_autoria> GetAllByIdProcess(int id)
        {
            return DataModel.ques_autoria.Where(e => e.fk_id_processo == id).ToList();
        }
        public List<ques_autoria> GetAllByCriteria(string quesito_autoria)
        {
            return DataModel.ques_autoria.Where(e => e.quesito_autoria == quesito_autoria).ToList();
        }

        #endregion
    }
}