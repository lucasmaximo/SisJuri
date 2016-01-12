using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class QuesAgravanteRepository
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

        public ques_agravante GetOne(int id)
        {
            return DataModel.ques_agravante.FirstOrDefault(e => e.id_quesito_agravante == id);
        }

        public ques_agravante GetOne(string quesito_agravante)
        {
            return DataModel.ques_agravante.FirstOrDefault(e => e.quesito_agravante == quesito_agravante);
        }

        public void Create(ques_agravante entity)
        {
            Save(entity);
        }

        public void Edit(ques_agravante entity)
        {
            Save(entity);
        }

        public void Save(ques_agravante entity)
        {
            DataModel.Entry(entity).State = entity.id_quesito_agravante == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(ques_agravante entity)
        {
            DataModel.ques_agravante.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<ques_agravante> GetAll()
        {
            return DataModel.ques_agravante.ToList();
        }
        public List<ques_agravante> GetAllByIdProcess(int id)
        {
            return DataModel.ques_agravante.Where(e => e.fk_id_processo == id).ToList();
        }
        public List<ques_agravante> GetAllByCriteria(string quesito_agravante)
        {
            return DataModel.ques_agravante.Where(e => e.quesito_agravante == quesito_agravante).ToList();
        }

        #endregion
    }
}