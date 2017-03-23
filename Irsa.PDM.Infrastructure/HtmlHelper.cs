using System.Web.Mvc;

namespace Irsa.PDM.Infrastructure
{
    public static class HtmlHelpers
    {
        public static string MenuItemClass(this HtmlHelper htmlHelper, string action, string controller, bool hasChilds)
        {
            if (hasChilds) return string.Empty;

            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            return (string.Equals(action, currentAction) && string.Equals(controller, currentController)) ? "active" : string.Empty;
        }

         //public static string GetNodeClass(this HtmlHelper htmlHelper, SiteMapNodeModel node)
         //{
         //    return node.Attributes.ContainsKey("Class") ? node.Attributes["Class"].ToString() : node.Parent != null ? node.Parent.Attributes["Class"].ToString(): string.Empty;
         //}

        
    }
}
