<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Issues.aspx.cs" Inherits="Bitcliq.BIR.Portal.Issues" %>


<!DOCTYPE html>
<html>
<head>
    <title>BIR</title>
    <link rel="stylesheet" href="assets/css/styles.css?=113">

    <link href='http://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600' rel='stylesheet'
        type='text/css'>
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries. Placeholdr.js enables the placeholder attribute -->
    <!--[if lt IE 9]>
        <link rel="stylesheet" href="css/ie8.css">
		<script type="text/javascript" src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
		<script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/respond.js/1.1.0/respond.min.js"></script>
        
	<![endif]-->
    <!-- The following CSS are included as plugins and can be removed if unused-->
    <link rel='stylesheet' type='text/css' href='assets/plugins/form-markdown/css/bootstrap-markdown.min.css' />
    <link rel='stylesheet' type='text/css' href='assets/plugins/codeprettifier/prettify.css' />
    <link rel='stylesheet' type='text/css' href='assets/plugins/form-toggle/toggles.css' />
    <script type='text/javascript' src='assets/js/jquery-1.10.2.min.js'></script>
    <script type='text/javascript' src='assets/js/jqueryui-1.10.3.min.js'></script>

    <script src="http://code.jquery.com/jquery-migrate-1.0.0.js"></script>

    <script type='text/javascript' src='assets/js/bootstrap.min.js'></script>

    <script type='text/javascript' src='assets/js/enquire.js'></script>

    <script type='text/javascript' src='assets/js/jquery.cookie.js'></script>



    <script type='text/javascript' src='assets/plugins/codeprettifier/prettify.js'></script>


    <link rel='stylesheet' type='text/css' href='assets/plugins/pines-notify/jquery.pnotify.default.css' />

    <script type='text/javascript' src='assets/plugins/pines-notify/jquery.pnotify.min.js'></script>


    <style>
        ul {
            list-style: none;
        }
    </style>

</head>
<body style="background-color: #FFF; padding-top: 0px !important;">
    <form id="form1" runat="server">

        <asp:HiddenField ID="HDReporter" runat="server" />

        <script src="http://code.jquery.com/jquery-migrate-1.2.1.js"></script>

        <script src="assets/plugins/datatables/jquery.dataTables.js"></script>


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
                            <div class="panel-heading">
                                <h4>Lista</h4>

                            </div>
                            <div class="panel-body collapse in">


                                <table id="editable" cellpadding="0" cellspacing="0" border="0" class="table table-striped table-bordered datatables ">
                                    <thead>
                                        <tr>
                                            <th>ID</th>
                                            <th>Imagem</th>
                                            <th>Assunto</th>
                                            <th>Mensagem</th>
                                            <th>Tipo</th>
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

            var handlerUrl = 'IssuesHandler.ashx?r=' + $("#<%=HDReporter.ClientID%>").val();
            var editor;
            $(function () {


                var tableInstance = $('#editable').dataTable({

                    "sDom": "C<'row'<'col-sm-6'T><'col-sm-6'f>r>t<'row'<'col-sm-6'i><'col-sm-6'p>> ",
                    "sAjaxSource": handlerUrl,
                    "bServerSide": true,
                    "bAutoWidth": false,
                    "bDestroy": true,

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
                            { "sName": "Subject", "sClass": "required", "aTargets": [2] }

                    ],
                    "aoColumns": [
                            { "mData": "ID" },
                               { "mData": "Check", "mRender": function (data, type, row) { return '<img src="GetImage.ashx?id=' + data + '"/>'; } },

                           { "mData": "Subject" },
                           { "mData": "Message" },


                               { "mData": "Check", "mRender": function (data, type, row) { return '<ul class="pager"><li><a  onclick="setBoot(\'Issues.aspx?user=' + data + '\');" href="#">Ocorrências Reportadas' + '</a></li></a>'; } }

                    ]

                });



                $('.dataTables_filter input').addClass('form-control').attr('placeholder', 'Pesquisar...');
                $('.dataTables_length select').addClass('form-control');

            });




            var calcDataTableHeight = function () {
                return $(window).height() * 40 / 100;
            };

            $(window).resize(function () {
                var oSettings = oTable.fnSettings();
                oSettings.oScroll.sY = calcDataTableHeight();
                oTable.fnDraw();
            });

            function setBoot(url) {
                bootbox.dialog({
                    message: '<iframe style="border:0;" src="' + url + '" height="600" width="100%"></iframe>',
                    title: "Associar categorias"
                });
            }

        </script>

    </form>
</body>
</html>
