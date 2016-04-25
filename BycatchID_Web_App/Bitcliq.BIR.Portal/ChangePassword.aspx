<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Bitcliq.BIR.Portal.ChangePassword" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>BycatchID Desk</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
  

    <!-- <link href="less/styles.less" rel="stylesheet/less" media="all"> -->
    <link rel="stylesheet" href="assets/css/styles.css?=113">
    <link href='http://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600' rel='stylesheet' type='text/css'>

    <link rel='stylesheet' type='text/css' href='assets/plugins/pines-notify/jquery.pnotify.default.css' />
    <script type='text/javascript' src='assets/js/jquery-1.10.2.min.js'></script>
    <script type='text/javascript' src='assets/js/jqueryui-1.10.3.min.js'></script>
    <script type='text/javascript' src='assets/js/bootstrap.min.js'></script>
    <script type='text/javascript' src='assets/js/enquire.js'></script>
    <script type='text/javascript' src='assets/js/jquery.cookie.js'></script>
    <script type='text/javascript' src='assets/js/jquery.nicescroll.min.js'></script>
    <script type='text/javascript' src='assets/plugins/codeprettifier/prettify.js'></script>
    <script type='text/javascript' src='assets/plugins/easypiechart/jquery.easypiechart.min.js'></script>
    <script type='text/javascript' src='assets/plugins/sparklines/jquery.sparklines.min.js'></script>
    <script type='text/javascript' src='assets/plugins/form-toggle/toggle.min.js'></script>
    <script type='text/javascript' src='assets/plugins/form-parsley/parsley.min.js'></script>
    <script type='text/javascript' src='assets/plugins/form-validation/formvalidation.js'></script>
    <script type='text/javascript' src='assets/js/placeholdr.js'></script>
    <script type='text/javascript' src='assets/js/application.js'></script>
    <script type='text/javascript' src='assets/demo/demo.js'></script>
    <script type='text/javascript' src='assets/plugins/pines-notify/jquery.pnotify.min.js'></script>
    
</head>



<body class="focusedform">
<div style="margin: 0px; padding: 0px; position: fixed; left: 0px; top: 0px; width: 100%; height: 100%; background-image: url(img/02.png);"></div>
    <form id="form1" runat="server" class="form-horizontal" style="margin-bottom: 0px !important;">
        <div class="verticalcenter">
            <br/><br/>
          <img src="img/logo_backoffice.png" alt="Logo" class="brand">
	<div class="panel panel-primary">
        <div class="panel-body">
            <h4 class="text-center" style="margin-bottom: 25px;">Change Password</h4>



            <div class="form-group">
                <div class="col-sm-12">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-lock"></i></span>


                        <asp:TextBox CssClass="form-control" TextMode="Password" ID="password" runat="server" placeholder="Password" required="required"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-lock"></i></span>

                        <asp:TextBox CssClass="form-control" TextMode="Password" data-equalto="#password" ID="password1" runat="server" placeholder="Repeat Password" required="required"></asp:TextBox>
                    </div>
                </div>
            </div>




        </div>
        <div class="panel-footer">
            <div class="pull-left">
            </div>
            <div class="pull-right">

                <asp:LinkButton ID="myLink" Text="Change" OnClientClick="return validateInputs();" class="btn btn-success" OnClick="changeButton_Click" runat="server" />
                <asp:Button ID="saveBtn" runat="server" OnClick="changeButton_Click" Style="display: none;" />
            </div>


        </div>

        <div class="alert alert-dismissable alert-danger" id="errorDiv" runat="server" style="display: none">
            <strong>Erro!</strong>  Could not change password
								<button type="button" class="close" data-dismiss="alet" aria-hidden="true">&times;</button>
        </div>





    </div>
        </div>


        <script>

            $(document).ready(function () {

                $('input[id$="password1"], input[id$="password"]')
                    .keypress(function (e) {

                        var keyCode;

                        if (window.event) keyCode = window.event.keyCode;
                        else if (e) keyCode = e.which;
                        else return true;

                        if (keyCode == 13) {
                            $('[id$="myLink"]').click();
                            return false;

                        } else return true;

                    });

            });
            function validateInputs() {

                if (!$('#form1').parsley('validate')) {
                    return false;
                }
                else {
                    $('#<%= saveBtn.ClientID %>').click();
                    return true;
                }

            }

        </script>
    </form>
</body>
</html>



