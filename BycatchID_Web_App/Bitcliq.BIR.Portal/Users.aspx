<%@ Page Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Bitcliq.BIR.Portal.Users" %>

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
         


            <h1>Users</h1>
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
                            <h4>List</h4>

                        </div>
                        <div class="panel-body collapse in">


                            <table id="editable" cellpadding="0" cellspacing="0" border="0" class="table table-striped table-bordered datatables ">
                                <thead>
                                    <tr>
										<th>ID</th>
                                        <th>Name</th>
										<th>Email</th>
										<th>Profile</th>
                                        <th>Status</th>  

                                       
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



    
    <asp:Literal id="litScript" runat="server"></asp:Literal>
    
    <script type='text/javascript' src='assets/js/application.js'></script>

  

    <script>

        
        function setBoot(url) {
            bootbox.dialog({
                message: '<iframe style="border:0;" src="' + url+'" height="600" width="100%"></iframe>',
                title: "Associar categorias"
            });
        }

    </script> 

    




</asp:Content>