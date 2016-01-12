using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class QuesAtenuanteRepository
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

        public ques_atenuante GetOne(int id)
        {
            return DataModel.ques_atenuante.FirstOrDefault(e => e.id_quesito_atenuante == id);
        }

        public ques_atenuante GetOne(string quesito_atenuante)
        {
            return DataModel.ques_atenuante.FirstOrDefault(e => e.quesito_atenuante == quesito_atenuante);
        }

        public void Create(ques_atenuante entity)
        {
            Save(entity);
        }

        public void Edit(ques_atenuante entity)
        {
            Save(entity);
        }

        public void Save(ques_atenuante entity)
        {
            DataModel.Entry(entity).State = entity.id_quesito_atenuante == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(ques_atenuante entity)
        {
            DataModel.ques_atenuante.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<ques_atenuante> GetAll()
        {
            return DataModel.ques_atenuante.ToList();
        }
        public List<ques_atenuante> GetAllByIdProcess(int id)
        {
            return DataModel.ques_atenuante.Where(e => e.fk_id_processo == id).ToList();
        }
        public List<ques_atenuante> GetAllByCriteria(string quesito_atenuante)
        {
            return DataModel.ques_atenuante.Where(e => e.quesito_atenuante == quesito_atenuante).ToList();
        }

        #endregion
    }
}