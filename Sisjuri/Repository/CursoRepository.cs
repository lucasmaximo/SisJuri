using Sisjuri.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sisjuri.Repository
{
    public class CursoRepository
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

        #region

        public curso GetOne(int id)
        {
            return DataModel.curso.FirstOrDefault(e => e.id_curso == id);
        }

        public curso GetOne(string Curso)
        {
            return DataModel.curso.FirstOrDefault(e => e.nome_curso == Curso);
        }

        public void Create(curso entity)
        {
            Save(entity);
        }

        public void Edit(curso entity)
        {
            Save(entity);
        }

        public void Save(curso entity)
        {
            DataModel.Entry(entity).State = entity.id_curso == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(curso entity)
        {
            DataModel.curso.Remove(entity);
            DataModel.SaveChanges();
        }

        public List<curso> GetAll()
        {
            return DataModel.curso.ToList();
        }

        public List<curso> GetAllByCriteria(string Nome_curso)
        {
            {
                return DataModel.curso.Where(e => e.nome_curso == Nome_curso).ToList();
            }
        }

        #endregion

    }
}