angular.module('cdp.service.singleFile', [])
       .factory('singleFileService', [
           '$http',           
           function ($http) {

               var target = null;
               var id = null;
               var params = null;

               return {
                   setUploader: function (data) {                     
                       id = "#" + data.controlId;

                       $(id).change(function () {
                           params = data.getParamsCallBack ? data.getParamsCallBack() : null;

                           if (!this.files || !this.files.length) {
                               target = null;
                               return;
                           }

                           var reader = new FileReader();

                           reader.onload = function(e) {
                               target = e.target.result;
                           };

                           reader.readAsDataURL(this.files[0]);
                       });
                   

                       $(id).fileupload({
                           acceptFileTypes: /(\.|\/)(gif|jpe?g|png|csv)$/i,
                           url: data.urlFile,
                           dataType: 'json',
                           data: params,
                           done: function (e, result) {
                               if (result.result.hasErrors || (result.result.result && result.result.result.hasErrors)) {
                                   data.onError(result.result.hasErrors ? result.result.messages[0] : result.result.result.messages[0],  result.result.data);
                               } else {
                                   data.onSuccess(result.result.data || target);
                               }
                           },
                           fail: function () {                               
                           }
                       }).prop('disabled', !$.support.fileInput).parent().addClass($.support.fileInput ? undefined : 'disabled');                                                            
                   }
               };
       }]);