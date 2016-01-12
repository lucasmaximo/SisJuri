using Sisjuri.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sisjuri.Repository
{
    public class DocumentoRepository
    {
        private DataModel _DataModel;

        public DataModel DataModel
        {
            get {
                if (_DataModel == null)
                    _DataModel = new DataModel();
                return _DataModel;
            }
            set { _DataModel = value; }
        }

        #region Métodos

        public documento GetOne(int id)
        {
            return DataModel.documento.FirstOrDefault(e => e.id_documento == id);
        }

        public documento GetOne(string nome_documento)
        {
            return DataModel.documento.FirstOrDefault(e => e.nome_documento == nome_documento);
        }

        public void Create(documento entity)
        {
            Save(entity);
        }

        public void Edit(documento entity)
        {
            Save(entity);
        }

        public void Save(documento entity)
        {
            DataModel.Entry(entity).State = entity.id_documento == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(documento entity)
        {
            DataModel.documento.Remove(entity);
            DataModel.SaveChanges();
        }

        public List<documento> GetAll()
        {
            return DataModel.documento.ToList();
        }

        public List<documento> GetAllByJuri(int fk_id_juri)
        {
            return DataModel.documento.Where(e => e.fk_id_juri == fk_id_juri).ToList();
        }

        public List<documento> GetAllByCriteria(string nome_documento, int fk_id_juri)
        {
            if (!string.IsNullOrEmpty(nome_documento) && (fk_id_juri == 0))
            {
                return DataModel.documento.Where(e => e.nome_documento == nome_documento).ToList();
            }

            if (!string.IsNullOrEmpty(nome_documento) && (fk_id_juri != 0))
            {
                return DataModel.documento.Where(e => e.nome_documento == nome_documento
                    && e.fk_id_juri == fk_id_juri).ToList();
            }
            else
            {
                return DataModel.documento.Where(e => e.fk_id_juri == fk_id_juri).ToList();
            }
        }

        #endregion

    }
}