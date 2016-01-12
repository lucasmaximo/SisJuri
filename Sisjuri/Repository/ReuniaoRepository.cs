using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class ReuniaoRepository
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

        public reuniao GetOne(int id)
        {
            return DataModel.reuniao.FirstOrDefault(e => e.id_reuniao == id);
        }

        public reuniao GetOne(string nome_reuniao)
        {
            return DataModel.reuniao.FirstOrDefault(e => e.nome_reuniao == nome_reuniao);
        }

        public void Create(reuniao entity)
        {
            Save(entity);
        }

        public void Edit(reuniao entity)
        {
            Save(entity);
        }

        public void Save(reuniao entity)
        {
            DataModel.Entry(entity).State = entity.id_reuniao == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(reuniao entity)
        {
            DataModel.reuniao.Remove(entity);
            DataModel.SaveChanges();
        }
        public List<reuniao> GetAll()
        {
            return DataModel.reuniao.ToList();
        }

        public List<reuniao> GetAllByCriteria(string nome_reuniao, int fk_id_juri)
        {
            if (!string.IsNullOrEmpty(nome_reuniao) && (fk_id_juri == 0))
            {
                return DataModel.reuniao.Where(e => e.nome_reuniao == nome_reuniao).ToList();
            }

            if (!string.IsNullOrEmpty(nome_reuniao) && (fk_id_juri != 0))
            {
                return DataModel.reuniao.Where(e => e.nome_reuniao == nome_reuniao
                    && e.fk_id_juri == fk_id_juri).ToList();
            }
            else
            {
                return DataModel.reuniao.Where(e => e.fk_id_juri == fk_id_juri).ToList();
            }
        }

        #endregion
    }
}