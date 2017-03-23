using System.Web.Mvc;

namespace Irsa.PDM.Infrastructure.ActionResults
{
    public class TxtResult : ActionResult
    {
        public string FileName { get; set; }
        public string Txt { get; set; }        

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Buffer = true;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            context.HttpContext.Response.ContentType = "application/text";            
            context.HttpContext.Response.Write(Txt);            

            context.HttpContext.Response.End();
        }     
    }
}