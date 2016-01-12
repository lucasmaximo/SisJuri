using Sisjuri.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sisjuri.Repository
{
    public class FotoRepository
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

        public foto GetOne(string path)
        {
            return DataModel.foto.FirstOrDefault(e => e.path == path);
        }

        public foto GetFotoById(string path)
        {

            foto foto = new foto();
            foto = GetOne(path);

            return foto;
        }

        public void Create(List<foto> fotos)
        {
            foto foto;
            for (int i = 0; i < fotos.Count; i++)
            {
                foto = fotos[i];
                DataModel.Entry(foto).State = foto.id_foto == 0 ?
                   EntityState.Added :
                   EntityState.Modified;
                DataModel.SaveChanges();
            }

        }

        public void Save(foto entity)
        {
            DataModel.Entry(entity).State = entity.id_foto == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(foto entity)
        {
            DataModel.foto.Remove(entity);
            DataModel.SaveChanges();
        }

        public List<foto> GetAll()
        {
            return DataModel.foto.ToList();
        }

        public List<foto> GetAllByIdJuri(int id)
        {
            return DataModel.foto.Where(e => e.fk_id_juri == id).ToList();
        }

        #endregion

    }
}