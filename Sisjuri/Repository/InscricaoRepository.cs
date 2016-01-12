using Sisjuri.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sisjuri.Repository
{
    public class InscricaoRepository
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

        #region Metodos

        public inscricao GetOne(int id)
        {
            return DataModel.inscricao.FirstOrDefault(e => e.id_inscricao == id);
        }

        public inscricao GetOne(string num_matric_aluno)
        {
            return DataModel.inscricao.FirstOrDefault(e => e.usuario.num_matric_aluno == num_matric_aluno);
        }

        public void Create(inscricao entity)
        {
            DataModel.inscricao.Add(entity);
            DataModel.SaveChanges();
        }

        public void Edit(inscricao entity)
        {
            Save(entity);
        }

        public void Save(inscricao entity)
        {
            DataModel.Entry(entity).State = entity.id_inscricao == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(inscricao entity)
        {
            DataModel.inscricao.Remove(entity);
            DataModel.SaveChanges();
        }

        public List<inscricao> GetAll()
        {
            return DataModel.inscricao.ToList();
        }

        public List<inscricao> GetAllByIdJuri(int id)
        {
            return DataModel.inscricao.Where(e => e.fk_id_juri == id).ToList();
        }

        public List<inscricao> GetPromotorByAuthorization(int id)
        {
            return DataModel.inscricao.Where(e => e.status_inscricao == "Autorizada" 
                && e.funcao.nome_funcao == "Promotor" && e.fk_id_juri == id ).ToList();
        }

        public List<inscricao> GetAdvogadoDefesaByAuthorization(int id)
        {
            return DataModel.inscricao.Where(e => e.status_inscricao == "Autorizada"
                && e.funcao.nome_funcao == "Advogado de defesa" && e.fk_id_juri == id).ToList();
        }

        public List<inscricao> GetEscrivaoByAuthorization(int id)
        {
            return DataModel.inscricao.Where(e => e.status_inscricao == "Autorizada"
                && e.funcao.nome_funcao == "Escrivão" && e.fk_id_juri == id).ToList();
        }

        public List<inscricao> GetJuradoByAuthorization(int id)
        {
            return DataModel.inscricao.Where(e => e.status_inscricao == "Autorizada"
                && e.funcao.nome_funcao == "Jurado" && e.fk_id_juri == id).ToList();
        }

        public List<inscricao> GetOficialJusticaoByAuthorization(int id)
        {
            return DataModel.inscricao.Where(e => e.status_inscricao == "Autorizada"
                && e.funcao.nome_funcao == "Oficial de Justiça" && e.fk_id_juri == id).ToList();
        }

        public List<inscricao> GetAssistenteByAuthorization(int id)
        {
            return DataModel.inscricao.Where(e => e.status_inscricao == "Autorizada"
                && e.funcao.nome_funcao == "Assistente" && e.fk_id_juri == id).ToList();
        }

        public List<inscricao> GetSegurancaByAuthorization(int id)
        {
            return DataModel.inscricao.Where(e => e.status_inscricao == "Autorizada"
                && e.funcao.nome_funcao == "Segurança" && e.fk_id_juri == id).ToList();
        }

        public List<inscricao> GetReuByAuthorization(int id)
        {
            return DataModel.inscricao.Where(e => e.status_inscricao == "Autorizada"
                && e.funcao.nome_funcao == "Réu" && e.fk_id_juri == id).ToList();
        }

        public List<inscricao> GetJuizByAuthorization(int id)
        {
            return DataModel.inscricao.Where(e => e.status_inscricao == "Autorizada"
                && e.funcao.nome_funcao == "Juiz" && e.fk_id_juri == id).ToList();
        }

        public List<inscricao> GetAllBySortAndJuri(int id)
        {
            return DataModel.inscricao.Where(e => e.fk_id_juri == id && e.inscricao_sorteada == true).ToList();
        }

        public List<inscricao> GetAllByAutorizados(int id)
        {
            return DataModel.inscricao.Where(e => e.fk_id_juri == id && e.status_inscricao == "Autorizada").ToList();
        }

        public List<inscricao> GetAllByCriteria(string nome_completo, int fk_id_juri, int id_funcao, string num_matric_aluno, 
            string status_inscricao)
        {
            List<inscricao> lst = DataModel.inscricao.ToList();

            if (!string.IsNullOrEmpty(nome_completo))
            {
                lst = DataModel.inscricao.Where(e => e.usuario.nome_completo == nome_completo).ToList();
            }

            if (fk_id_juri != 0)
            {
                lst = lst.Where(e => e.fk_id_juri == fk_id_juri).ToList();
            }

            if (id_funcao != 0)
            {
                lst = lst.Where(e => e.funcao.id_funcao == id_funcao).ToList();
            }

            if (!string.IsNullOrEmpty(num_matric_aluno))
            {
                lst = lst.Where(e => e.usuario.num_matric_aluno == num_matric_aluno).ToList();
            }

            if (!string.IsNullOrEmpty(status_inscricao))
            {
                lst = lst.Where(e => e.status_inscricao == status_inscricao).ToList();
            }

            return lst.ToList();

        }

        #endregion

    }
}