angular.module('irsa.pdm.service.singleFile', [])
       .factory('singleFileService', [
           '$http',           
           function ($http) {

               var target = null;
               var id = null;
               var params = {};
               var fileName = null;

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

                           reader.onload = function (e) {
                               target = e.target.result;
                           };

                           fileName = this.files[0].name;
                           reader.readAsDataURL(this.files[0]);
                       });

                       $(id).fileupload({
                           acceptFileTypes: /(\.|\/)(gif|jpe?g|png|csv)$/i,
                           url: data.urlFile,
                           dataType: 'json',
                           done: function (e, result) {
                               if (result.result.hasErrors || (result.result.result && result.result.result.hasErrors)) {
                                   data.onError(result.result.hasErrors ? result.result.messages[0] : result.result.result.messages[0], result.result.data);
                               } else {
                                   var response = data.returnDetailedFile ? { data: target, name: fileName } : target;
                                   data.onSuccess(result.result.data || response);
                               }
                           },
                           fail: function () {
                           },
                           progress: function (e, d) {
                               if (data.uploadProgressCallBack) {
                                   var progress = parseInt(d.loaded / d.total * 100, 10);
                                   data.uploadProgressCallBack(progress);
                               }
                           }
                       }).bind('fileuploadsubmit', function (e, d) {
                           d.formData = params;
                       }).prop('disabled', !$.support.fileInput).parent().addClass($.support.fileInput ? undefined : 'disabled');                     
                   }
               };
       }]);