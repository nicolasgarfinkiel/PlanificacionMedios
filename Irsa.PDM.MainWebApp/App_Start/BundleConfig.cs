using System.Web;
using System.Web.Optimization;

namespace Irsa.PDM.MainWebApp
{
    public class BundleConfig
    {        
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css/styles")
           .Include("~/Content/css/font.css")
           .Include("~/Content/css/font-awesome.min.css")
            .Include("~/Content/css/metisMenu.min.css")
            .Include("~/Content/css/animate.min.css")
            .Include("~/Content/css/bootstrap.min.css")
            .Include("~/Content/css/awesome-bootstrap-checkbox.css")
            .Include("~/Content/css/pe-icon-7-stroke.css")
            .Include("~/Content/css/helper.css")
            .Include("~/Content/css/static_custom.css")
            .Include("~/Content/css/bootstrap-datepicker.css")
            .Include("~/Content/css/ng-grid.min.css")            
            .Include("~/Content/css/animate.min.css")
            .Include("~/Content/css/jquery.fileupload.css")
            .Include("~/Content/css/select.css")
            .Include("~/Content/css/style.css")
            .Include("~/Content/css/table-responsive.css")
            );
          
            bundles.Add(new ScriptBundle("~/bundles/scripts/basicScripts")
              .Include("~/Scripts/jquery.min.js")
              .Include("~/Scripts/jquery.slimscroll.min.js")
              .Include("~/Scripts/bootstrap.min.js")
              .Include("~/Scripts/bootstrap-datepicker.js")
              .Include("~/Scripts/metisMenu.min.js")
              .Include("~/Scripts/icheck.min.js")
              .Include("~/Scripts/jquery.peity.min.js")
              .Include("~/Scripts/index.js")
              .Include("~/Scripts/moment.js")
              .Include("~/Scripts/homer.js")
              .Include("~/Scripts/moment.js")
              );      

            bundles.Add(new ScriptBundle("~/bundles/scripts/angularScripts")
            .Include("~/Scripts/angularJs/angular-v1.2.2.min.js")
            .Include("~/Scripts/angularJs/angular-route.min.js")
            .Include("~/Scripts/angularJs/ng-grid-2.0.7.min.js")
            .Include("~/Scripts/angularJs/angular-strap.min.js")
            .Include("~/Scripts/angularJs/angular-sanitize.js")
            .Include("~/Scripts/angularJs/angular-resource.min.js")
            .Include("~/Scripts/angularJs/ui-utils.min.js")
            .Include("~/Scripts/angularJs/select.min.js")
            .Include("~/Scripts/angularJs/angular-input-masks-standalone.min.js")
            .Include("~/Scripts/app/shared/listBootstraperService.js")
            .Include("~/Scripts/app/shared/editBootstraperService.js")
            .Include("~/Scripts/app/shared/directives/loadingDirective.js")
            .Include("~/Scripts/app/shared/directives/debounceDirective.js")
            .Include("~/Scripts/app/shared/directives/intDirective.js")
            .Include("~/Scripts/app/shared/base/baseNavigationService.js")
            .Include("~/Scripts/app/shared/base/baseService.js")
            .Include("~/Scripts/jquery.blockUI.js")
            );
          
        }
    }
}
