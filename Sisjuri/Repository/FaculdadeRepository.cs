using Sisjuri.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sisjuri.Repository
{
    public class FaculdadeRepository
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

        #region Metodos

        public faculdade GetOne(int id)
        {
            return DataModel.faculdade.FirstOrDefault(e => e.id_faculdade == id);
        }

        public faculdade GetOne(string nome_faculdade, string cnpj_faculdade)
        {
            return DataModel.faculdade.FirstOrDefault(e => e.nome_faculdade == nome_faculdade && e.cnpj_faculdade == cnpj_faculdade);
        }

        public void Create(faculdade entity)
        {
            Save(entity);
        }

        public void Edit(faculdade entity)
        {
            Save(entity);
        }

        public void Save(faculdade entity)
        {
            DataModel.Entry(entity).State = entity.id_faculdade == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(faculdade entity)
        {
            DataModel.faculdade.Remove(entity);
            DataModel.SaveChanges();
        }

        public List<faculdade> GetAll()
        {
            return DataModel.faculdade.ToList();
        }

        public List<faculdade> GetAllByCriteria(string nome_faculdade, string cnpj_faculdade)
        {
            if (!string.IsNullOrEmpty(nome_faculdade) && (string.IsNullOrEmpty(cnpj_faculdade)))
            {
                return DataModel.faculdade.Where(e => e.nome_faculdade == nome_faculdade).ToList();
            }

            if (!string.IsNullOrEmpty(nome_faculdade) && (!string.IsNullOrEmpty(cnpj_faculdade)))
            {
                return DataModel.faculdade.Where(e => e.nome_faculdade == nome_faculdade
                    && e.cnpj_faculdade == cnpj_faculdade).ToList();
            }
            else
            {
                return DataModel.faculdade.Where(e => e.cnpj_faculdade == cnpj_faculdade).ToList();
            }
        }

        #endregion

    }
}