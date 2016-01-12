using Microsoft.Reporting.WebForms;
using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class HistoricoJuriController : BaseController
    {

        private VHistoricoJuriRepository _VHistoricoJuriRepository;

        public VHistoricoJuriRepository VHistoricoJuriRepository
        {
            get {
                if (_VHistoricoJuriRepository == null)
                    _VHistoricoJuriRepository = new VHistoricoJuriRepository();
                return _VHistoricoJuriRepository; }
            set { _VHistoricoJuriRepository = value; }
        }

        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult HistoricoJuri(int idJuri)
        {
            String format = "PDF";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Report"), "vhistoricojurireport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            List<vhistoricojuri> cm = VHistoricoJuriRepository.GetOneByIdJuri(idJuri);
            ReportDataSource rd = new ReportDataSource("vhistoricojurireport", cm);
            lr.DataSources.Add(rd);

            //List<vsorteiofuncoes> cm2 = new VSorteioFuncoesRepository().GetAllBySortJuriAndExport(idJuri);
            //ReportDataSource rd2 = new ReportDataSource("vsorteiofuncoes", cm2);
            //lr.DataSources.Add(rd2);

            string reportType = format;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

                "<DeviceInfo>" +
                "<OutputFormat>" + format + "</OutputFormat>" +
                "<PageWidth>8.5in</PageWidth>" +
                "<PageHeight>11in</PageHeight>" +
                "<MarginTop>0.5in</MarginTop>" +
                "<MarginLeft>0.5in</MarginLeft>" +
                "<MarginRight>0.5in</MarginRight>" +
                "<MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);

            return File(renderedBytes, mimeType);
        }
    }
}
