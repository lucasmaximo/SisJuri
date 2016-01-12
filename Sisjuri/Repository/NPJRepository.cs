using Sisjuri.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sisjuri.Repository
{
    public class NPJRepository
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

        public npj GetOne(int id)
        {
            return DataModel.npj.FirstOrDefault(e => e.id_npj == id);
        }

        public npj GetOne(string nome_npj, string cnpj_npj)
        {
            return DataModel.npj.FirstOrDefault(e => e.nome_npj == nome_npj && e.cnpj_npj == cnpj_npj);
        }

        public void Create(npj entity)
        {
            Save(entity);
        }

        public void Edit(npj entity)
        {
            Save(entity);
        }

        public void Save(npj entity)
        {
            DataModel.Entry(entity).State = entity.id_npj == 0 ? 
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(npj entity)
        {
            DataModel.npj.Remove(entity);
            DataModel.SaveChanges();
        }

        public List<npj> GetAll()
        {
            return DataModel.npj.ToList();
        }

        public List<npj> GetAllByCriteria(string nome_npj, string cnpj_npj)
        {
            if (!string.IsNullOrEmpty(nome_npj) && (string.IsNullOrEmpty(cnpj_npj)))
            {
                return DataModel.npj.Where(e => e.nome_npj == nome_npj).ToList();
            }

            if (!string.IsNullOrEmpty(nome_npj) && (!string.IsNullOrEmpty(cnpj_npj)))
            {
                return DataModel.npj.Where(e => e.nome_npj == nome_npj
                    && e.cnpj_npj == cnpj_npj).ToList();
            }
            else
            {
                return DataModel.npj.Where(e => e.cnpj_npj == cnpj_npj).ToList();
            }
        }

        #endregion
    }
}