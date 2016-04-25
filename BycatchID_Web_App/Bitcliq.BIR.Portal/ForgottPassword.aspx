<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgottPassword.aspx.cs" Inherits="Bitcliq.BIR.Portal.ForgottPassword" %>


<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>BycatchID Desk</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
  
    <!-- <link href="assets/less/styles.less" rel="stylesheet/less" media="all"> -->
    <link rel="stylesheet" href="assets/css/styles.css?v=2">
    <link href='http://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600' rel='stylesheet' type='text/css'>
    
    <!-- <script type="text/javascript" src="assets/js/less.js"></script> -->
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
									<span class="input-group-addon"><i class="fa fa-envelope-o"></i></span>
									<input type="text" runat="server" class="form-control" id="email" placeholder="Email">
								</div>
							</div>
						</div>
				
					
		</div>
		<div class="panel-footer">
			<div class="pull-left">
				<a href="Default.aspx" class="btn btn-default"><i class="fa fa-arrow-left"></i> Login</a>
			</div>
			<div class="pull-right">
				

                <asp:LinkButton id="myLink" Text="Reset" OnClientClick="return validateInputs();" class="btn btn-success" OnClick="loginButton_Click" runat="server"/>
                    <asp:Button ID="saveBtn" runat="server"  OnClick="loginButton_Click" Style="display:none;" />
			</div>
		</div>
          <div class="alert alert-dismissable alert-danger" id="errorDiv" runat="server" style="display:none" >
								<strong>Invalid data!</strong>  Email does not exist
								<button type="button" class="close" data-dismiss="alet" aria-hidden="true">&times;</button>
							</div>

             <div class="alert alert-dismissable alert-success" id="successDiv" runat="server" style="display:none">
								<strong>Sucess!</strong> Check your inbox 
								<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
							</div>
	</div>
 </div>


        <script>

            $(document).ready(function () {

                $('input[id$="email"]')
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
