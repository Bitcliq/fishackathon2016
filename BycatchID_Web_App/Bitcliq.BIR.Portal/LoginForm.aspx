<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginForm.aspx.cs" Inherits="Bitcliq.BIR.Portal.LoginForm" %>




<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>BycatchID Desk</title>
    
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="BIR">
    <meta name="author" content="Bitcliq">

    <!-- <link href="less/styles.less" rel="stylesheet/less" media="all"> -->
    <link rel="stylesheet" href="assets/css/styles.css?v=34">
    <link href='http://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600' rel='stylesheet' type='text/css'>

    <!-- <script type="text/javascript" src="js/less.js"></script> -->



    <link rel='stylesheet' type='text/css' href='assets/plugins/pines-notify/jquery.pnotify.default.css' />
    <script type='text/javascript' src='assets/js/jquery-1.10.2.min.js'></script>
    <script type='text/javascript' src='assets/js/jqueryui-1.10.3.min.js'></script>
    <script type='text/javascript' src='assets/js/bootstrap.min.js'></script>
    <script type='text/javascript' src='assets/js/enquire.js'></script>
    <script type='text/javascript' src='assets/js/jquery.cookie.js'></script>
    <script type='text/javascript' src='assets/js/jquery.nicescroll.min.js'></script>
    <script type='text/javascript' src='assets/plugins/codeprettifier/prettify.js'></script>
    <script type='text/javascript' src='assets/plugins/form-toggle/toggle.min.js'></script>
    <script type='text/javascript' src='assets/plugins/form-parsley/parsley.min.js'></script>
    <script type='text/javascript' src='assets/plugins/form-validation/formvalidation.js'></script>
    <script type='text/javascript' src='assets/js/placeholdr.js'></script>
    <script type='text/javascript' src='assets/js/application.js'></script>
    <script type='text/javascript' src='assets/plugins/pines-notify/jquery.pnotify.min.js'></script>


</head>
<body class="focusedform">


<div style="margin: 0px; padding: 0px; position: fixed; left: 0px; top: 0px; width: 100%; height: 100%; background-image: url(img/02.png);"></div>

    <form class="form-horizontal" data-validate="parsley" id="form1" runat="server" style="margin-bottom: 0px !important;">


        <div class="verticalcenter">
		<br/><br/>
         <img src="img/logo_backoffice.png" alt="Logo" class="brand">
	<div class="panel panel-primary">
        <div class="panel-body">
           <h4 class="text-center" style="margin-bottom: 25px;">BycatchID Desk</h4>
            <div class="form-group">
                <div class="col-sm-12">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-envelope-o"></i></span>


                
                        <input type="text" id="userTxt" runat="server"  data-type="email" placeholder="Email" required="required" class="form-control">
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-lock"></i></span>

                        <input type="password" id="passTxt" placeholder="Password"  runat="server" required="required" class="form-control">
                    </div>
                </div>
            </div>
            <div class="clearfix" style="display: none;">
                <div class="pull-right">
                    <label>
                        <input type="checkbox" name="xxx" style="margin-bottom: 20px" checked="">
                        Remember Me</label></div>
            </div>


        </div>
        <div class="panel-footer">
            <a href="ForgottPassword.aspx" class="pull-left btn btn-link" >Forgot password</a>

            <div class="pull-right">
                <a href="#" class="btn btn-default" onclick='document.getElementById("form1").reset();'>Clean</a>

                <asp:LinkButton ID="myLink" Text="Log In" OnClientClick="return validateInputs();" OnClick="logintBtn_Click" class="btn btn-primary" runat="server" />

  <asp:Button ID="saveBtn" runat="server" OnClick="logintBtn_Click" Style="display: none;" />
                


              
        </div>
        </div>
        <div class="panel-footer">
           <div class="alert alert-dismissable alert-danger" id="errorDiv" runat="server" style="display: none">
            <strong>Invalid data!</strong>  Check username and password!
								<button type="button" class="close" data-dismiss="alet" aria-hidden="true">&times;</button>
        </div>
       

			 <div style="text-align:center;">
				Download <a href="http://http://desk.bycatchid.bitcliq.com/bycatchid.apk" target="_blank">Android BycatchID Apk</a>
            </div>
       </div>


      
    </div>
        </div>



        <script>

            $(document).ready(function () {

                $('input[id$="userNameTextBox"], input[id$="passwordTextBox"]')
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