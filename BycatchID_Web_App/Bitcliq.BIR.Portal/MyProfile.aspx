<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteMaster.Master" CodeBehind="MyProfile.aspx.cs" Inherits="Bitcliq.BIR.Portal.MyProfile" %>


<asp:Content ContentPlaceHolderID="masterCp" runat="server">







    <div id='wrap'>
        <div id="page-heading">



            <h1>O meu Perfil</h1>
            <div class="options" style="display: none;">
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

                        <div class="panel-body collapse in">
                            <div class="form-horizontal row-border">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Nome</label>
                                    <div class="col-sm-6">
                                        <input type="text" id="txtName" maxlength="255" required="required" runat="server" class="form-control">
                                    </div>

                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Email</label>
                                    <div class="col-sm-6">
                                        <input type="text" id="txtEmail" maxlength="255" required="required" runat="server" class="form-control">
                                    </div>
                                </div>


                                <div class="form-group" runat="server" id="dvEmail">
                                    <label class="col-sm-3 control-label">Não desejo receber mais notificações</label>
                                    <div class="col-sm-6">
                                        <div class="checkbox block">
                                        <label>
                                        <input type="checkbox" id="chkNot" runat="server" >
                                            </label>
                                    </div>
                                        </div>
                                </div>
                              
                                <div class="form-group">
                                    <label class="col-sm-3 control-label"></label>
                                    <div class="col-sm-6">
                                        <a href="#myModal" data-toggle="modal" id="clickM"><i class="fa fa-lock"></i>&nbsp;Alterar a minha senha</a>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer">
                                <div class="row">
                                    <div class="col-sm-6 col-sm-offset-3">
                                        <div class="btn-toolbar">
                                            <asp:Button ID="btnSave" class="btn-primary btn" runat="server" Text="Gravar" OnClick="btnSave_Click" />




                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>





        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 id="H1" class="modal-title" runat="server">Alterar senha</h4>
                    </div>
                    <div class="modal-body">


                        <div class="panel-body collapse in">
                            <div class="form-horizontal row-border">



                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-lock"></i></span>


                                            <asp:TextBox CssClass="form-control" TextMode="Password" ID="password" runat="server" placeholder="Senha" ></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-lock"></i></span>

                                            <asp:TextBox CssClass="form-control" TextMode="Password" data-equalto="#password" ID="password1" runat="server" placeholder="Repetir senha" ></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="modal-footer">

                                <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>

                                <button type="button" class="btn btn-primary" onclick="savePassword();">Guardar</button>

                            </div>
                        </div>
                        <!-- /.modal-content -->
                    </div>
                    <!-- /.modal-dialog -->
                </div>
                <!-- /.modal -->
            </div>
        </div>


    </div>


    <asp:Literal ID="litScript" runat="server"></asp:Literal>

    <script type='text/javascript' src='assets/js/application.js'></script>

    <script>

        function savePassword()
        {
            $.ajax({
                type: "POST",
                url: 'MyProfile.aspx/SavePassword',
                contentType: "application/json; charset=utf-8",
                data: '{"password":"' + $("#<%=password.ClientID%>").val() + '", "confirm":"' + $("#<%=password1.ClientID%>").val() + '"}',

                    dataType: "json",
                    success: function (response) {

                        if (response.d == 'OK') {
                            $.pnotify({
                                title: 'Sucesso!',
                                text: 'Senha gravada com sucesso com sucesso!',
                                type: 'success',
                                hide: false
                            });

                        }
                        else {
                           
                            $.pnotify({
                                title: 'Erro!',
                                text: response.d + '',
                                type: 'error',
                                hide: false
                            });

                        }
                    },
                    failure: function (response) {
                        //alert(response);
                    }

                });
        }

        function showError(message)
        {

            setTimeout(function () {
                $.pnotify({
                    title: 'Erro!',
                    text: message,
                    type: 'error',
                    hide: false
                });
                }, 2000);


            
        }


        function showSuccess() {

            setTimeout(function () {
                $.pnotify({
                    title: 'Sucesso!',
                    text: 'Dados gravados com sucesso!',
                    type: 'success',
                    hide: false
                });
            }, 2000);



        }

    </script>

</asp:Content>
