using Sisjuri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sisjuri.Repository
{
    public class ParticipantesReuniaoRepository
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

        public participantes_reuniao GetOne(int id)
        {
            return DataModel.participantes_reuniao.FirstOrDefault(e => e.id_participante_reuniao == id);
        }

        public List<participantes_reuniao> GetAll()
        {
            return DataModel.participantes_reuniao.ToList();
        }

        public List<participantes_reuniao> GetParticipantesByReuniao(int id)
        {
            //Retornando os participantes da reunião, menos o id_funcao = 8, porque o réu não participa da reunião
            return DataModel.participantes_reuniao.Where(e => e.pfk_id_reuniao == id && e.inscricao.fk_id_funcao != 8).ToList();     
        }

        public List<participantes_reuniao> GetAllByCriteria(int pfk_id_reuniao, int fk_id_juri)
        {
            if ((pfk_id_reuniao != 0) && (fk_id_juri == 0))
            {
                return DataModel.participantes_reuniao.Where(e => e.pfk_id_reuniao == pfk_id_reuniao).ToList();
            }

            if ((pfk_id_reuniao != 0) && (fk_id_juri != 0))
            {
                return DataModel.participantes_reuniao.Where(e => e.pfk_id_reuniao == pfk_id_reuniao
                    && e.reuniao.fk_id_juri == fk_id_juri).ToList();
            }
            else
            {
                return DataModel.participantes_reuniao.Where(e => e.reuniao.fk_id_juri == fk_id_juri).ToList();
            }
        }

        #endregion

        public void Create(participantes_reuniao entity)
        {
            Save(entity);
        }

        public void Edit(participantes_reuniao entity)
        {
            Save(entity);
        }

        public void Save(participantes_reuniao entity)
        {
            DataModel.Entry(entity).State = entity.id_participante_reuniao == 0 ?
               EntityState.Added :
               EntityState.Modified;
            DataModel.SaveChanges();
        }

        public void Delete(participantes_reuniao entity)
        {
            DataModel.participantes_reuniao.Remove(entity);
            DataModel.SaveChanges();
        }

        public List<participantes_reuniao> GetAllByIdReuniao(int idReuniao)
        {
            return DataModel.participantes_reuniao.Where(e => e.pfk_id_reuniao == idReuniao).ToList();
        }
    }
}