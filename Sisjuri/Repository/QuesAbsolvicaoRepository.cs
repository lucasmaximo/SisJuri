using Sisjuri.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sisjuri.Repository
{
    public class QuesAbsolvicaoRepository
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

        public ques_absolvicao GetOne(int id)
        {
            return DataModel.ques_absolvicao.FirstOrDefault(e => e.id_quesito_absolvicao == id);
        }

        public ques_absolvicao GetOne(string quesito_absolvicao)
        {
            return DataModel.ques_absolvicao.FirstOrDefault(e => e.quesito_absolvicao == quesito_absolvicao);
        }

        public void Create(ques_absolvicao entity)
        {
            Save(entity);
        }

        public void Edit(ques_absolvicao entity)
        {
            Save(entity);
        }

        public void Save(ques_absolvicao entity)
        {
            DataModel.Entry(entity).State = entity.id_quesito_absolvicao == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(ques_absolvicao entity)
        {
            DataModel.ques_absolvicao.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<ques_absolvicao> GetAll()
        {
            return DataModel.ques_absolvicao.ToList();
        }
        public List<ques_absolvicao> GetAllByIdProcess(int id)
        {
            return DataModel.ques_absolvicao.Where(e => e.fk_id_processo == id).ToList();
        }
        public List<ques_absolvicao> GetAllByCriteria(string quesito_absolvicao)
        {
            return DataModel.ques_absolvicao.Where(e => e.quesito_absolvicao == quesito_absolvicao).ToList();
        }

        #endregion
    }
}