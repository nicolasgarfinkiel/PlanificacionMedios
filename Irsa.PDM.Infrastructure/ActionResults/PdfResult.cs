using System.Web.Mvc;

namespace Irsa.PDM.Infrastructure.ActionResults
{
    public class PdfResult : ActionResult
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Buffer = true;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            context.HttpContext.Response.ContentType = "application/octet-stream";
            context.HttpContext.Response.BinaryWrite(Content);
            context.HttpContext.Response.End();
        }        
    }
}