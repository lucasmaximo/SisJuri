using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class SentencaRepository
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

        public sentenca GetOne(int id)
        {
            return DataModel.sentenca.FirstOrDefault(e => e.id_sentenca == id);
        }

        public sentenca GetOne(string texto_sentenca)
        {
            return DataModel.sentenca.FirstOrDefault(e => e.texto_sentenca == texto_sentenca);
        }

        public void Create(sentenca entity)
        {
            Save(entity);
        }

        public void Edit(sentenca entity)
        {
            Save(entity);
        }

        public void Save(sentenca entity)
        {
            DataModel.Entry(entity).State = entity.id_sentenca == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(sentenca entity)
        {
            DataModel.sentenca.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<sentenca> GetAll()
        {
            return DataModel.sentenca.ToList();
        }
        public List<sentenca> GetAllByIdProcess(int id)
        {
            return DataModel.sentenca.Where(e => e.fk_id_processo == id).ToList();
        }
        public List<sentenca> GetAllByCriteria(string texto_sentenca)
        {
            return DataModel.sentenca.Where(e => e.texto_sentenca == texto_sentenca).ToList();
        }

        #endregion
    }
}
