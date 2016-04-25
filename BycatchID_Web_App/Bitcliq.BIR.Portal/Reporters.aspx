<%@ Page Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Reporters.aspx.cs" Inherits="Bitcliq.BIR.Portal.Reporters" %>


<asp:Content ContentPlaceHolderID="masterCp" runat="server">

  
    <script src="http://code.jquery.com/jquery-migrate-1.2.1.js"></script>

    <script src="assets/plugins/datatables/jquery.dataTables.js"></script>
    <script type='text/javascript' src='assets/plugins/datatables/TableTools.js'></script>
    
    
    <script src="assets/plugins/datatables/dataTables.editor.js"></script>
    <script src="assets/plugins/datatables/dataTables.editor.bootstrap.js"></script>
    <script src="assets/plugins/datatables/dataTables.bootstrap.js"></script>
    




  <link rel='stylesheet' type='text/css' href='assets/plugins/datatables/dataTables.css' />
    <link rel='stylesheet' type='text/css' href='assets/plugins/codeprettifier/prettify.css' />
    <link rel='stylesheet' type='text/css' href='assets/plugins/form-toggle/toggles.css' />

    
  
     
<script type='text/javascript' src='assets/plugins/bootbox/bootbox.min.js'></script> 
    <div id='wrap'>
        <div id="page-heading">
         


            <h1>Reporters</h1>
            <div class="options" style="display:none;">
                <div class="btn-toolbar">
                    <div class="btn-group hidden-xs">
                        <a href='#' class="btn btn-default dropdown-toggle" data-toggle='dropdown'><i class="fa fa-cloud-download"></i><span class="hidden-sm">Export as  </span><span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a target="_blank" href="Export.ashx">CSV File (*.csv)</a></li>

                        </ul>
                    </div>
                    <a href="#" class="btn btn-default dropdown-toggle" data-toggle='dropdown'><i class="fa fa-cog"></i></a>

                    <ul class="dropdown-menu">
                        <li><a href="ImportData.aspx">Import Data</a></li>

                       

                    </ul>

                </div>
            </div>
        </div>

       
        <div class="container">

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4>Lista</h4>

                        </div>
                        <div class="panel-body collapse in">


                            <table id="editable" cellpadding="0" cellspacing="0" border="0" class="table table-striped table-bordered datatables ">
                                <thead>
                                    <tr>
										<th>ID</th>
                                        <th>Nome</th>
										<th>Email</th>
                                         <th>Ocorrências</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>


                    </div>
                </div>
            </div>
        </div>




    


        
    </div>



     <script>
         var handlerUrl = 'ReportersHandler.ashx';
         var editor;
         $(function () {
             // edit inline
             editor = new $.fn.dataTable.Editor({
                 "ajaxUrl": handlerUrl,
                 "domTable": "#editable",

                 "fields": [
                      {
                          "label": "Nome:",
                          "name": "Name"
                      },
                      {
                          "label": "Email:",
                          "name": "Email"
                      }

                 ]

             });
             var tableInstance = $('#editable').dataTable({

                 "sDom": "C<'row'<'col-sm-6'T><'col-sm-6'f>r>t<'row'<'col-sm-6'i><'col-sm-6'p>> ",
                 "sAjaxSource": handlerUrl,
                 "bServerSide": true,
                 "bAutoWidth": false,
                 "bDestroy": true,
                 "iDisplayLength": 50,
                 "bProcessing": true,
                 "aaSorting": [[0, "desc"]],
                 "oLanguage": {
                     "oPaginate": {
                         "sPrevious": "Anterior",
                         "sNext": "Seguinte"
                     }
                 },
                 "aoColumnDefs": [
                         { "sName": "ID", "sClass": "noteditable", "aTargets": [0] },
                         { "sName": "Name", "sClass": "required", "aTargets": [1] }

                 ],
                 "aoColumns": [
                         { "mData": "ID" },
                        { "mData": "Name" },
                        { "mData": "Email" },
                      
                           
                            { "mData": "Check", "mRender": function (data, type, row) { return '<ul class="pager"><li><a  onclick="setBoot(\'Issues.aspx?r=' + data + '\');" href="#">Ocorrências Reportadas' + '</a></li></a>'; } }




                 ],
                 "oTableTools": {
                     "sRowSelect": "single",
                     "aButtons": [
                         { "sExtends": "editor_create", "editor": editor },
                          { "sExtends": "editor_edit", "editor": editor },




                         { "sExtends": "editor_remove", "editor": editor },
                             //{ "sExtends": "copy", "sButtonText":"Ativar/Desativar", "editor": editor }
                             {
                                 "sExtends": "ajax",
                                 "sAjaxUrl": "UsersHandler.ashx",
                                 "sButtonText": "Ativar/Desativar",
                                 "fnClick": function (nButton, oConfig, oFlash) {

                                     var b = this.fnGetSelected();

                                     if (b != '') {
                                         $.ajax({
                                             type: "POST",
                                             "url": oConfig.sAjaxUrl + "?action=changeStatus&id=" + b[0].id,

                                             //"success": tableInstance.fnReloadAjax( handlerUrl ),

                                             "success": function (result) {

                                                 if (result.data == "OK")
                                                     tableInstance.fnReloadAjax(handlerUrl);
                                                 else
                                                     alert(result.data);
                                             },

                                             "dataType": "json",
                                             "type": "POST",
                                             "cache": false,

                                             "error": function (result) {

                                                 //alert( "Error detected when sending table data to server" );
                                                 alert("Ocorreu um erro! Tente mais tarde.");
                                             }
                                         });
                                     }

                                 }
                             }


                     ]
                 }

             });



             $('.dataTables_filter input').addClass('form-control').attr('placeholder', 'Pesquisar...');
             $('.dataTables_length select').addClass('form-control');
             $("#ToolTables_editable_0").prepend('<i class="fa fa-plus"/> ');
             $("#ToolTables_editable_1").prepend('<i class="fa fa-pencil-square-o"></i> ');
             $("#ToolTables_editable_2").prepend('<i class="fa fa-times-circle"/> ');

         });




         var calcDataTableHeight = function () {
             return $(window).height() * 40 / 100;
         };

         $(window).resize(function () {
             var oSettings = oTable.fnSettings();
             oSettings.oScroll.sY = calcDataTableHeight();
             oTable.fnDraw();
         });



         $(".dataTables_scroll").niceScroll({ horizrailenabled: false });


         jQuery.fn.dataTableExt.oApi.fnReloadAjax = function (oSettings, sNewSource, fnCallback, bStandingRedraw) {

             // DataTables 1.10 compatibility - if 1.10 then `versionCheck` exists.
             // 1.10's API has ajax reloading built in, so we use those abilities
             // directly.
             if (jQuery.fn.dataTable.versionCheck) {
                 var api = new jQuery.fn.dataTable.Api(oSettings);

                 if (sNewSource) {
                     api.ajax.url(sNewSource).load(fnCallback, !bStandingRedraw);
                 }
                 else {
                     api.ajax.reload(fnCallback, !bStandingRedraw);
                 }
                 return;
             }

             if (sNewSource !== undefined && sNewSource !== null) {
                 oSettings.sAjaxSource = sNewSource;
             }

             // Server-side processing should just call fnDraw
             if (oSettings.oFeatures.bServerSide) {
                 this.fnDraw();
                 return;
             }

             this.oApi._fnProcessingDisplay(oSettings, true);
             var that = this;
             var iStart = oSettings._iDisplayStart;
             var aData = [];

             this.oApi._fnServerParams(oSettings, aData);

             oSettings.fnServerData.call(oSettings.oInstance, oSettings.sAjaxSource, aData, function (json) {
                 /* Clear the old information from the table */
                 that.oApi._fnClearTable(oSettings);

                 /* Got the data - add it to the table */
                 var aData = (oSettings.sAjaxDataProp !== "") ?
                     that.oApi._fnGetObjectDataFn(oSettings.sAjaxDataProp)(json) : json;

                 for (var i = 0 ; i < aData.length ; i++) {
                     that.oApi._fnAddData(oSettings, aData[i]);
                 }

                 oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();

                 that.fnDraw();

                 if (bStandingRedraw === true) {
                     oSettings._iDisplayStart = iStart;
                     that.oApi._fnCalculateEnd(oSettings);
                     that.fnDraw(false);
                 }

                 that.oApi._fnProcessingDisplay(oSettings, false);

                 /* Callback user function - for event handlers etc */
                 if (typeof fnCallback == 'function' && fnCallback !== null) {
                     fnCallback(oSettings);
                 }
             }, oSettings);
         };


    </script>
   
    
    <script type='text/javascript' src='assets/js/application.js'></script>

  

    <script>


        function setBoot(url) {
            bootbox.dialog({
                message: '<iframe style="border:0;" src="' + url + '" height="600" width="100%"></iframe>',
                title: "Associar categorias"
            });
        }

    </script> 

    




</asp:Content>