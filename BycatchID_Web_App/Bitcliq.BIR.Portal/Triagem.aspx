<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteMaster.Master" CodeBehind="Triagem.aspx.cs" Inherits="Bitcliq.BIR.Portal.Triagem" %>

<asp:Content ContentPlaceHolderID="masterCp" runat="server" ID="c1">
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=&sensor=false"></script>
    <script src="http://google-maps-utility-library-v3.googlecode.com/svn/trunk/markerclusterer/src/markerclusterer.js" type="text/javascript"></script>
    <link rel='stylesheet' type='text/css' href='assets/plugins/form-nestable/jquery.nestable.css' />
    <link rel='stylesheet' type='text/css' href='assets/plugins/bootstro.js/bootstro.min.css' />
    <style>
        #dvMapContainer {
            width: 100%;
            height: 400px;
        }
    </style>



    <a style="display: none;" href="#myModal" id="clickM" class="btn btn-primary btn-lg">+</a>

    <asp:HiddenField ID="hdProfileAdmin" runat="server" Value="" />
    <asp:HiddenField ID="hdProfile" runat="server" Value="" />

    <asp:HiddenField ID="hdAccountID" runat="server" Value="2" />
    <div id="page-heading">
        <h1>Ocorrências 
        </h1>

        <div class="options" id="filter" style="padding-bottom: 5px;">
            <div class="btn-toolbar">
                Tipo:&nbsp;&nbsp;<asp:DropDownList ID="listFType" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;

        Prioridade:&nbsp;&nbsp;<asp:DropDownList ID="listFPriority" runat="server">
            <asp:ListItem Value="0">Todas</asp:ListItem>
            <asp:ListItem Value="1">1</asp:ListItem>
            <asp:ListItem Value="2">2</asp:ListItem>
            <asp:ListItem Value="3">3</asp:ListItem>
            <asp:ListItem Value="4">4</asp:ListItem>
            <asp:ListItem Value="5">5</asp:ListItem>
        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;

        Estado:&nbsp;&nbsp;<asp:DropDownList ID="listFState" runat="server">
        </asp:DropDownList>


                <a href="#notop" class="btn btn-primary" id="btnFilter">Filtrar</a>
            </div>
        </div>
    </div>

    <div class="container">
        <br />
        <a href="#" onclick="initialize();"><i class="fa fa-refresh"></i>Atualizar Mapa</a>
        <div id="dvMap">
            <div id="dvMapContainer"></div>
        </div>
        <br />
        <div id="dvIssues">
        </div>
    </div>

    <!-- Modal INSERT UPDATE / DELETE-->
    <div class="modal fade" id="myModal" tabindex="-1" data-replace="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 id="H1" class="modal-title" runat="server">Ocorrência</h4>
                </div>
                <div class="modal-body">

                    <asp:HiddenField runat="server" ID="hdIssueID" />



                    <input type="button" value="Gerar PDF" class="btn btn-danger" onclick="generatePDF();" />
                    <a href="#" id="linkPDF" target="_blank" style="display: none;">Download PDF</a>
                    <br />
                    <br />
                    <div class="panel-body collapse in">
                        <div class="form-horizontal row-border">

                            <div class="form-group">

                                <div style="text-align: right; width: 100%">
                                    <a href="#notop" onclick="rotateImage();"><i class="fa fa-rotate-left"></i>Rodar Imagem</a>
                                </div>
                                <img src="" id="imgIssue" style="cursor: pointer;" class="img-rounded img-responsive" onclick="rotateImage();" title="Clique na imagem para rodar" />
                            </div>

                            <div class="form-group">
                                <label class="col-sm-3 control-label">Reportado por:</label>
                                <div class="col-sm-9">


                                    <label id="labelReporter" class="form-control" disabled=""></label>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-3 control-label">Assunto</label>
                                <div class="col-sm-9">

                                    <label id="labelSubject" disabled="" class="form-control"></label>

                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-3 control-label">Mensagem</label>
                                <div class="col-sm-9">
                                    <label id="labelMessage" disabled="" class="form-control"></label>
                                </div>
                            </div>


                            <div class="form-group">
                                <label class="col-sm-3 control-label">Prioridade</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="listPriority1" class="form-control" runat="server">
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>

                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-3 control-label">Tipo</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="listType" class="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-3 control-label">Estado</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="listState" class="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                             
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Notas internas </label>
                                <div class="col-sm-9">

                                    <asp:TextBox ID="txtNotes" TextMode="multiline" class="form-control" runat="server">
                                    </asp:TextBox>
                                </div>

                            </div>

                            
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Enviar email </label>
                                <div class="col-sm-9">
                                    <div class="checkbox block">
                                        <label>
                                            <asp:CheckBox ID="chkSendMail" runat="server" />
                                        </label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group" id="dvMessage" style="display: none;">
                                <label class="col-sm-3 control-label">Mensagem </label>
                                <div class="col-sm-9">
                                    <!--small>
                                        Pode usar as seguintes tags
                                        <ul>
                                            <li>#Nome</li>
                                            <li>#Estado</li>
                                            <li>#Foto</li>
                                            <li>#Estado</li>
                                        </ul>
                                        
                                        

                                    </small!-->
                                    <asp:TextBox ID="txtMessageMail" TextMode="multiline" class="form-control" runat="server">
                                    </asp:TextBox>
                                </div>

                            </div>




                        </div>
                        <div class="modal-footer">

                            <!--button type="button" class="btn btn-default" data-dismiss="modal">Arquivar</!--button-->

                            <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>

                            <button type="button" class="btn btn-primary" onclick="saveIssue();">Guardar</button>





                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <!-- /.modal -->
        </div>
    </div>



    <!-- END Modal INSERT UPDATE / DELETE-->
    <script type='text/javascript' src='assets/plugins/form-nestable/jquery.nestable.js'></script>
    <script type='text/javascript' src='assets/plugins/bootstro.js/bootstro.js'></script>

    <script type='text/javascript' src='assets/js/demo-tour.js'></script>
    <script type="text/javascript">
        var infowindow;
        var map;
        var latlngbounds;
        var z = 15;

        var markers = [];
        var mc;
        var init = 0;
        var ft = 0; // type
        var fp = 0; // priority
        var fs = 0; // state

        function gup(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(window.location.href);
            if (results == null)
                return "";
            else
                return results[1];
        }

        var version = gup('v');
        var lang = gup('lang');
        var fromApp = gup('fromapp');



        //save order

        function getIdList() {
            var idList = '';
            $('.dd-item').each(function () {
                idList += $(this).data('id') + ';';
            });

            return idList;
        }

        function rotateImage() {

            $("#imgIssue").css("opacity", 0.5);

            $.ajax({
                type: "POST",
                url: 'Triagem.aspx/RotateImg',
                contentType: "application/json; charset=utf-8",
                data: '{"fileName":"' + $("#imgIssue").attr('src') + '"}',
                dataType: "json",

                success: function (response) {
                    if (response.d == 'ERROR') {
                        $.pnotify({
                            title: 'Erro!',
                            text: 'Ocorreu um erro a rodar a imagem!',
                            type: 'error',
                            hide: false
                        });
                        $("#imgIssue").css("opacity", 1);
                    }
                    else {
                        $('#imgIssue').attr('src', response.d);
                        $("#imgIssue").css("opacity", 1);
                    }

                },
                failure: function (response2) {
                    //alert(response);
                }

            });
        }

        function generatePDF() {
            $('#linkPDF').attr('href', '');
            $('#linkPDF').hide();

            $.ajax({
                type: "POST",
                url: 'Triagem.aspx/GeneratePDF',
                contentType: "application/json; charset=utf-8",
                data: '{"id":"' + $("#<%=hdIssueID.ClientID%>").val() + '", "imgSrc":"' + $('#imgIssue').attr('src') + '" }',

                dataType: "json",
                success: function (response) {
                    if (response.d == 'ERROR') {
                        $.pnotify({
                            title: 'Erro!',
                            text: 'Ocorreu um erro a alterar a ordenação!',
                            type: 'error',
                            hide: false
                        });
                    }
                    else {
                        $('#linkPDF').attr('href', response.d);
                        $('#linkPDF').show();
                    }

                },
                failure: function (response2) {
                    //alert(response);
                }

            });
        }
        function showData(response) {

            $('#dvIssues').html();
            $('#dvIssues').html(response.d);

            var idList = getIdList();

            if ($("#<%=hdProfileAdmin.ClientID%>").val() == $("#<%=hdProfile.ClientID%>").val()) {
                $("#nestable_list").nestable();
                $('#nestable_list').on('change', function (event) {

                    //alert("Order has been changed");
                    var newIDList = getIdList();

                    if (newIDList != idList) {
                        idList = newIDList;
                        $.ajax({
                            type: "POST",
                            url: 'Triagem.aspx/ReorderIssues',
                            contentType: "application/json; charset=utf-8",
                            data: '{"idList":"' + idList + '"}',

                            dataType: "json",
                            success: function (response2) {
                                if (response2.d == 'ERROR') {
                                    $.pnotify({
                                        title: 'Erro!',
                                        text: 'Ocorreu um erro a alterar a ordenação!',
                                        type: 'error',
                                        hide: false
                                    });
                                }

                            },
                            failure: function (response2) {
                                //alert(response);
                            }

                        });


                    }

                });
            }

        }
        var idList;

        function GetIssues() {
            $.ajax({
                type: "POST",
                url: 'Triagem.aspx/GetIssues',
                contentType: "application/json; charset=utf-8",
                data: '{"accountID":"' + $("#<%=hdAccountID.ClientID%>").val() + '", "typeID":"' + ft + '", "priority":"' + fp + '", "state":"' + fs + '"}',

                dataType: "json",
                success: function (response) {

                    showData(response);



                    $(".boot").click(function () {
                        var tourID = $(this).attr('id');
                        var xxx = "#bootstro" + tourID;

                        bootstro.start(xxx, {
                            onComplete: function (params) {
                                //alert("Reached end of introduction with total " + (params.idx + 1) + " slides");
                            },
                            onExit: function (params) {
                                //alert("Introduction stopped at slide #" + (params.idx + 1));
                            },
                        });
                    });

                },
                failure: function (response) {
                    //alert(response);
                }

            });
        }



        $(document).ready(function () {


            GetIssues();
            initialize();

            if ($("#<%=hdProfileAdmin.ClientID%>").val() != $("#<%=hdProfile.ClientID%>").val()) {
                $("#<%=listPriority1.ClientID%>").attr('disabled', true);
            }

            $("#btnFilter").click(function () {
                ft = $("#<%=listFType.ClientID%>").val();
                fp = $("#<%=listFPriority.ClientID%>").val();
                fs = $("#<%=listFState.ClientID%>").val();
                GetIssues();
                initialize();
            });


            $("#<%=chkSendMail.ClientID%>").click(function () {
                var thisCheck = $(this);

                if (thisCheck.is(':checked')) {
                    $('#dvMessage').show();
                }
                else
                    $('#dvMessage').hide();
            });





        });


        var markerCluster;
        var marker;

        function initialize() {
            try {


                var myOptions = {
                    center: new google.maps.LatLng(39.406938, -9.135937),
                    zoom: z,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                    //,
                    //scrollwheel: false,
                    //disableDoubleClickZoom: true,
                    //draggable: false
                };

                infowindow = new google.maps.InfoWindow();

                map = new google.maps.Map(document.getElementById("dvMapContainer"), myOptions);

                getMarkers();
                google.maps.event.addListener(map, 'zoom_changed', function () {
                    zoomChangeBoundsListener =
                                google.maps.event.addListener(map, 'bounds_changed', function (event) {

                                });
                });

                //map.initialZoom = true;
            }
            catch (err) {
                alert(err);
            }

            //$('#preloader').hide();
            //init = 1;
        }


        //MAP
        function getMarkers() {
            clearOverlays();
            $.ajax({
                type: "POST",
                url: 'Triagem.aspx/LoadMarkers',
                //data: '{"accountID":"' + $("#<%=hdAccountID.ClientID%>").val() + '", "typeID":"' + ft + '", "priority":"' + fp + '"}',

                data: '{"accountID":"' + $("#<%=hdAccountID.ClientID%>").val() + '", "typeID":"' + ft + '", "priority":"' + fp + '", "state":"' + fs + '"}',

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (response != '') {
                        latlngbounds = new google.maps.LatLngBounds();
                        var data = response.d;
                        $.each(data, function (i, item) {
                            loadMarker(data[i], i);

                        })


                        var mcOptions = { gridSize: 50, maxZoom: 15 };

                        markerCluster = new MarkerClusterer(map, markers, mcOptions);

                    }
                },
                failure: function (response) {
                    //alert(response);
                }

            });

        }


        function loadMarker(markerData, i) {

            // create new marker location
            var myLatlng = new google.maps.LatLng(markerData['lat'], markerData['lng']);


            // create new marker
            var marker = new google.maps.Marker({
                id: markerData['id'],
                //map: map,
                title: markerData['name'],
                position: myLatlng,
                icon: "assets/img/markers/markers-base.png"
                //icon: markerData['icon']
            });
            //map.fitBounds(latlngbounds);
            //latlngbounds.extend(marker.getPosition());
            latlngbounds.extend(myLatlng);
            map.fitBounds(latlngbounds);
            google.maps.event.addListener(marker, 'click', function () {
                showMarker(i);
            });
            markers.push(marker);


        }






        function showMarker(markerId) {
            var marker = markers[markerId];
            if (marker) {

                $.ajax({
                    type: "POST",
                    url: 'Triagem.aspx/GetDetails',
                    contentType: "application/json; charset=utf-8",
                    data: '{"id":' + marker.id + '}',

                    dataType: "json",
                    success: function (response) {
                        infowindow.setContent(response.d);
                        infowindow.open(map, marker);

                        $('.dd3-content').removeClass("activeContent");
                        $('#ddcontent' + marker.id).addClass("activeContent");
                    },
                    failure: function (response) {
                        //alert(response);
                    }

                });

            } else {
                //alert('Error marker not found: ' + markerId);
            }

        }

        // END MAP



        function showInMap(id) {
            clearOverlays();

            $.ajax({
                type: "POST",
                url: 'Triagem.aspx/LoadOneMarker',
                contentType: "application/json; charset=utf-8",
                data: '{"id":"' + id + '"}',

                dataType: "json",
                success: function (response) {

                    latlngbounds = new google.maps.LatLngBounds();
                    var data = response.d;
                    $.each(data, function (i, item) {

                        loadMarker(data[i], i);
                        $('html, body').animate({ scrollTop: 0 }, 'fast');
                        var mcOptions = { gridSize: 50, maxZoom: 15 };

                        markerCluster = new MarkerClusterer(map, markers, mcOptions);
                        showMarker(0);


                    })


                },
                failure: function (response) {
                    //alert(response);
                }

            });

        }

        // MODAL
        function openModal(id) {
            $('#imgIssue').attr('src', '');
            // get ISSUE Detail
            $.ajax({
                type: "POST",
                url: 'Triagem.aspx/GetIssueDetail',
                contentType: "application/json; charset=utf-8",
                data: '{"id":"' + id + '"}',

                dataType: "json",
                success: function (response) {
                    // open modal and show data
                    var data = $.parseJSON(response.d);
                    $('#labelReporter').text(data.ReporterName);
                    $('#labelSubject').text(data.Subject);
                    $('#labelMessage').text(data.Message);
                    $("#<%=listPriority1.ClientID%>").val(data.Priority);

                    if (data.TypeID != null)
                        $("#<%=listType.ClientID%>").val(data.TypeID);
                        else
                            $("#<%=listType.ClientID%>").val("");

                    $("#<%=listState.ClientID%>").val(data.State);
                    $('#imgIssue').attr('src', data.PhotoUrl);
                    $("#<%=hdIssueID.ClientID%>").val(data.ID);
                        $("#<%=txtMessageMail.ClientID%>").val("");
                    $("#<%=txtNotes.ClientID%>").val("");
                    $("#<%=chkSendMail.ClientID%>").prop('checked', false);

                    //$('#clickM').click();
                    $('#myModal').modal('show');

                },
                failure: function (response) {
                    //alert(response);
                }

            });
        }


        //saveIssue
        function saveIssue() {
            var id = $("#<%=hdIssueID.ClientID%>").val();

                var priority = $("#<%=listPriority1.ClientID%>").val();
                var type = $("#<%=listType.ClientID%>").val();
                var state = $("#<%=listState.ClientID%>").val();

                var state = $("#<%=listState.ClientID%>").val();
                var message = $("#<%=txtMessageMail.ClientID%>").val();

                var notes = $("#<%=txtNotes.ClientID%>").val();

                var chk = 0;
                if ($("#<%=chkSendMail.ClientID%>").is(':checked')) {
                    if ($("#<%=txtMessageMail.ClientID%>").val() == '') {
                        //openModal($("#<%=hdIssueID.ClientID%>").val());
                        $.pnotify({
                            title: 'Erro!',
                            text: 'Introduza o texto da mensagem!',
                            type: 'error',
                            hide: false
                        });
                        return;
                    }
                    chk = 1;
                }

                //data: '{"accountID":"' + $("#<%=hdAccountID.ClientID%>").val() + '", "typeID":"' + ft + '", "priority":"' + fp + '", "state":"' + fs + '"}',


                $.ajax({
                    type: "POST",
                    url: 'Triagem.aspx/SaveIssue',
                    contentType: "application/json; charset=utf-8",
                    data: '{"id":"' + $("#<%=hdIssueID.ClientID%>").val() + '", "typeID":"' + type + '", "priority": "' +
                    priority + '", "state": "' + state + '", "sendMail":"' + chk + '", "message": "' + message +
                  '", "typeFID":"' + ft + '", "priorityF":"' + fp + '", "stateF":"' + fs + '", "notes":"' + notes + '"}',

                    dataType: "json",
                    success: function (response) {

                        if (response.d != 'ERROR') {
                            $('#myModal').modal('hide');
                            showData(response);
                            initialize();

                            $(".boot").click(function () {
                                var tourID = $(this).attr('id');
                                var xxx = "#bootstro" + tourID;

                                bootstro.start(xxx, {
                                    onComplete: function (params) {
                                        //alert("Reached end of introduction with total " + (params.idx + 1) + " slides");
                                    },
                                    onExit: function (params) {
                                        //alert("Introduction stopped at slide #" + (params.idx + 1));
                                    },
                                });
                            });

                        }
                        else {
                            //openModal($("#<%=hdIssueID.ClientID%>").val());
                        $.pnotify({
                            title: 'Erro!',
                            text: 'Ocorreu um erro a alterar o estado da Ocorrência!',
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


        function clearOverlays() {
            for (var i = 0; i < markers.length; i++) {
                markers[i].setMap(null);
            }
            markers.length = 0;
            markers = [];

            if (markerCluster) {
                markerCluster.clearMarkers();

                markerCluster = null;
            }

        }


        //setTimeout(function () {
        //    GetIssues();
        //}, 60000);

        //setTimeout(function () {
        //    getMarkers();
        //}, 60000);

    </script>
</asp:Content>
