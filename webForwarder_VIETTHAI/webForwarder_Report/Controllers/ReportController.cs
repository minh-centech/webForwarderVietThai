using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using webForwarder_Report.Models;
using webForwarder_Report.Code;

namespace webForwarder_Report.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        [HttpPost]
        [Route("TaiPDF")]
        public HttpResponseMessage TaiPDF(TaiPDFRequest request)
        {
            ReportDocument rpt = null;
            var ctx = System.Web.HttpContext.Current;
            var root = ctx.Server.MapPath("~/App_Data");
            try
            {
                if (request == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Dữ liệu không được để trống.");
                }

                string fileName;

                if (request.IDDanhMucChungTu == 552)
                {
                    fileName = Path.Combine(
                           root, "ctXacNhanHoanThanhDongHangXuatKhau_TallySheet.rpt");
                }
                else if (request.IDDanhMucChungTu == 534)
                {
                    fileName = Path.Combine(
                           root, "ctXacNhanHoanThanhNhapKhoHangXuatKhau_BienBanGiaoNhan.rpt");
                }
                else
                {
                    return Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest,
                        "IDDanhMucChungTu không được hỗ trợ: "
                        + request.IDDanhMucChungTu);
                }

                rpt = new ReportDocument();

                rpt.Load(fileName);

                Common.ApplyConnection(rpt);

                rpt.SetParameterValue("@IDDanhMucDonVi", request.IDDanhMucDonVi);
                rpt.SetParameterValue("@IDDanhMucChungTu", request.IDDanhMucChungTu);
                rpt.SetParameterValue("@So", request.So);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Stream reportStream = rpt.ExportToStream(ExportFormatType.PortableDocFormat))
                    {
                        reportStream.CopyTo(memoryStream);
                    }

                    byte[] fileBytes = memoryStream.ToArray();

                    var response = Request.CreateResponse(HttpStatusCode.OK);

                    response.Content = new ByteArrayContent(fileBytes);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = "Report_" + request.So + ".pdf";

                    return response;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
            finally
            {
                if (rpt != null)
                {
                    try
                    {
                        rpt.Close();
                        rpt.Dispose();
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}