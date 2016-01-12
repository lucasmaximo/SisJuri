using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class UsuarioRepository
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

        public usuario GetOne(int id)
        {
            return DataModel.usuario.FirstOrDefault(e => e.id_usuario == id);
        }

        public usuario GetOne(string cpf, string senha)
        {
            return DataModel.usuario.FirstOrDefault(e => e.cpf_usuario == cpf && e.senha_usuario == senha);
        }

        public void Create(usuario entity)
        {
            Save(entity);
        }

        public void Edit(usuario entity)
        {
            Save(entity);
        }

        public void Save(usuario entity)
        {
            DataModel.Entry(entity).State = entity.id_usuario == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(usuario entity)
        {
            DataModel.usuario.Remove(entity);
            DataModel.SaveChanges();
        }

        public usuario GetOneByCPF(string cpf_usuario)
        {
            return DataModel.usuario.Where(e => e.cpf_usuario == cpf_usuario).FirstOrDefault();
        }

        public List<usuario> GetAll()
        {
            return DataModel.usuario.ToList();
        }

        public List<usuario> GetAllByCriteria(string nome_completo, string cpf_usuario)
        {
            if (!string.IsNullOrEmpty(nome_completo) && (string.IsNullOrEmpty(cpf_usuario)))
            {
                return DataModel.usuario.Where(e => e.nome_completo == nome_completo).ToList();
            }

            if (!string.IsNullOrEmpty(nome_completo) && (!string.IsNullOrEmpty(cpf_usuario)))
            {
                return DataModel.usuario.Where(e => e.nome_completo == nome_completo
                    && e.cpf_usuario == cpf_usuario).ToList();
            }
            else
            {
                return DataModel.usuario.Where(e => e.cpf_usuario == cpf_usuario).ToList();
            }
        }

        public List<usuario> GetAllByCriteria(string nome_completo, string cpf_usuario, int fk_id_curso)
        {
            List<usuario> lst = DataModel.usuario.ToList();

            if (!string.IsNullOrEmpty(nome_completo))
            {
                lst = DataModel.usuario.Where(e => e.nome_completo == nome_completo).ToList();
            }

            if (!string.IsNullOrEmpty(cpf_usuario))
            {
                lst = lst.Where(e => e.cpf_usuario == cpf_usuario).ToList();
            }

            if (fk_id_curso != 0)
            {
                lst = lst.Where(e => e.fk_id_curso == fk_id_curso).ToList();
            }

            return lst.ToList();
        }

        #endregion
    }
}