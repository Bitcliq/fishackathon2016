
<%@ Page Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Paginated.aspx.cs" Inherits="Bitcliq.BIR.Portal.Paginated" %>


<asp:Content ContentPlaceHolderID="masterCp" runat="server" ID="c1">
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=&sensor=false"></script>
    <script src="http://google-maps-utility-library-v3.googlecode.com/svn/trunk/markerclusterer/src/markerclusterer.js" type="text/javascript"></script>
    <link rel='stylesheet' type='text/css' href='assets/plugins/form-nestable/jquery.nestable.css?v=1' />
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
        <h1>Reports 
        </h1>

        <div class="options" id="filter" style="padding-bottom: 5px;">
            <div class="btn-toolbar">
               Species&nbsp;&nbsp;<asp:DropDownList ID="listFArea" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
              <asp:DropDownList ID="listFType" runat="server" Visible="false"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;

      
                <asp:DropDownList ID="listFPriority" runat="server" Visible="false">
            <asp:ListItem Value="0">Todas</asp:ListItem>
            <asp:ListItem Value="1">1</asp:ListItem>
            <asp:ListItem Value="2">2</asp:ListItem>
            <asp:ListItem Value="3">3</asp:ListItem>
            <asp:ListItem Value="4">4</asp:ListItem>
            <asp:ListItem Value="5">5</asp:ListItem>
        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;

       <asp:DropDownList ID="listFState" runat="server" Visible="false">
        </asp:DropDownList>


                <a href="#notop" class="btn btn-primary" id="btnFilter">Filter</a>
            </div>
        </div>
    </div>
    <div class="container">
        <br />
        <a href="#" onclick="initialize();"><i class="fa fa-refresh"></i>&nbsp;&nbsp;Update Map</a>
        <div id="dvMap">
            <div id="dvMapContainer"></div>
        </div>
        <br />
        <div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4>Fish specie Reports</h4>
                        </div>
                        <div class="panel-body">
                            <div class="dd" id="nestable_list">
                                <ol class="dd-list" id="dvIssues">
                                </ol>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <!-- Modal INSERT UPDATE / DELETE-->
    <div class="modal fade" id="myModal" tabindex="-1" data-replace="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 id="H1" class="modal-title" runat="server">Reports</h4>
                </div>
                <div class="modal-body">

                    <asp:HiddenField runat="server" ID="hdIssueID" />



                    <input type="button" id="pdfBtn" value="Generate PDF" class="btn btn-danger" onclick="generatePDF();" />
                    <a href="#" id="linkPDF" target="_blank" style="display: none;">Download PDF</a>
                    <br />
                    <br />
                    <div class="panel-body collapse in">
                        <div class="row">
                            <div class="col-md-6">
                                 <div class="panel-body">

                                    <div class="form-horizontal row-border">

                                        <div class="form-group">
                                            <div style="text-align: right; width: 100%">
                                                <a href="#notop" id="rotateLink" onclick="rotateImage();"><i class="fa fa-rotate-left"></i>&nbsp;&nbsp;Rotate Image</a>
                                            </div>
                                            <img src="" id="imgIssue" style="cursor: pointer;" class="img-rounded img-responsive" title="Click to rotate" />
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Reported by por:</label>
                                            <div class="col-sm-9">


                                                <label id="labelReporter" class="form-control" disabled=""></label>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Subject</label>
                                            <div class="col-sm-9">

                                                <label id="labelSubject" disabled="" class="form-control"></label>

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Message</label>
                                            <div class="col-sm-9">
                                                <label id="labelMessage" disabled="" class="form-control"></label>
                                            </div>
                                        </div>

                                        <asp:HiddenField ID="HDLatAndLong" runat="server" />


                                    </div>

                                </div>
                            </div>
                            <div class="col-md-6">

                                <div class="panel-body">
                                    <div class="form-horizontal row-border">

                                        <div class="form-group" style="display:none;">
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
                                        <div class="form-group" >
                                            <label class="col-sm-3 control-label">Fish Species</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="listArea" class="form-control" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group" style="display:none;">
                                            <label class="col-sm-3 control-label">Categoria</label>
                                            <div class="col-sm-9">
                                               

                                                <select id="listCat" class="form-control">
                                                </select>
                                            </div>
                                        </div>

                                         <div class="form-group" >
                                            <label class="col-sm-3 control-label">Ocean</label>
                                            <div class="col-sm-9">
                                               

                                                <asp:DropDownList id="listProperties" class="form-control" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>



                                        <div class="form-group" style="display:none;">
                                            <label class="col-sm-3 control-label">Fish Species</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="listState" class="form-control" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group " style="display:none;">
                                            <label class="col-sm-3 control-label">Atribuir a </label>
                                            <div class="col-sm-9">

                                                <div class="checkbox block">
                                                <asp:CheckBoxList ID="usersList" runat="server"></asp:CheckBoxList>

                                                    </div>
                                            </div>

                                        </div>
                                        <div class="form-group" >
                                            <label class="col-sm-3 control-label">Notes </label>
                                            <div class="col-sm-9">

                                                <asp:TextBox ID="txtNotes" TextMode="multiline" class="form-control" runat="server">
                                                </asp:TextBox>
                                            </div>

                                        </div>


                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">
                                                Send mail
                                            <br />
                                                <small>(To Reporter)</small></label>

                                            <div class="col-sm-9">
                                                <div class="checkbox block">
                                                    <label>
                                                        <asp:CheckBox ID="chkSendMail" runat="server" />
                                                    </label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-group" id="dvMessage" style="display: none;">
                                            <label class="col-sm-3 control-label">Message </label>
                                            <div class="col-sm-9">

                                                <asp:TextBox ID="txtMessageMail" TextMode="multiline" class="form-control" runat="server">
                                                </asp:TextBox>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>


                        <div class="modal-footer">

                            <!--button type="button" class="btn btn-default" data-dismiss="modal">Arquivar</!--button-->

                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

                            <button type="button" class="btn btn-primary" onclick="saveIssue();">Save</button>


                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <!-- /.modal -->
        </div>
    </div>




    <!-- Modal Reporter-->
    <div class="modal fade" id="myModalReporter" tabindex="-1" data-replace="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 id="H2" class="modal-title" runat="server">Reporter</h4>
                </div>
                <div class="modal-body">

                    <div class="panel-body collapse in">
                        <div class="row">
                            <div class="col-md-6">
                                 <div class="panel-body">

                                    <div class="form-horizontal row-border">


                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Repoted by:</label>
                                            <div class="col-sm-9">


                                                <label id="labelReporterRepBy" class="form-control" disabled=""></label>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Phone</label>
                                            <div class="col-sm-9">

                                                <label id="labelPhoneRepBy" disabled="" class="form-control"></label>

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Email</label>
                                            <div class="col-sm-9">
                                                <label id="labelEmailRepBy" disabled="" class="form-control"></label>
                                            </div>
                                        </div>

                                  
                                    </div>

                                </div>
                            </div>
                            
                        </div>


                        <div class="modal-footer">
                           <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

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
        var issueAddress = '';
        var infowindow;
        var map;
        var latlngbounds;
        var z = 15;

        var markers = [];
        var mc;
        var init = 0;
        var fa = 0; // area
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
                url: 'Paginated.aspx/RotateImg',
                contentType: "application/json; charset=utf-8",
                data: '{"fileName":"' + $("#imgIssue").attr('src') + '"}',
                dataType: "json",

                success: function (response) {
                    if (response.d == 'ERROR') {
                        $.pnotify({
                            title: 'Error!',
                            text: 'Error rotating image!',
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

            $("#pdfBtn").css("opacity", 0.5);
            $('#linkPDF').attr('href', '');
            $('#linkPDF').hide();
            issueAddress = codeLatLng();

           
        }


        function showData(response) {
            $('#dvIssues').append(response.d);

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
                            url: 'Paginated.aspx/ReorderIssues',
                            contentType: "application/json; charset=utf-8",
                            data: '{"idList":"' + idList + '"}',

                            dataType: "json",
                            success: function (response2) {
                                if (response2.d == 'ERROR') {
                                    $.pnotify({
                                        title: 'Error!',
                                        text: 'Error updating order!',
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

        var numRec = 30;
        var index = 0;

        var isfull = false;
        var isloading = false;


        function GetIssues() {

            if (!isfull || !isloading) {
                isloading = true;
                $.ajax({
                    type: "POST",
                    url: 'Paginated.aspx/GetIssues',
                    contentType: "application/json; charset=utf-8",


                    //data: '{"accountID":"' + $("#<%=hdAccountID.ClientID%>").val() + '", "typeID":"' + ft + '", "priority":"' + fp + '", "state":"' + fs + '", "si": "' + index + '", "numRec":"' + numRec + '"}',

                    data: '{"accountID":"' + $("#<%=hdAccountID.ClientID%>").val() + '", "typeID":"' + ft + '", "priority":"' + fp + '", "state":"' + fs + '", "si": "' + index + '", "numRec":"' + numRec + '", "parent":"' + fa + '"}',


                    dataType: "json",
                    success: function (response) {

                        showData(response);

                        if (response.d != "") {
                            if (index == 0)
                                index = numRec + 1;
                            else
                                index = index + numRec + 1;
                            // idx = numRec + 1;
                        }
                        else
                            isfull = true;

                        isloading = false;
                        $(".boot").click(function () {
                           
                            var tourID = $(this).attr('id');
                            var xxx = "#bootstro" + tourID;

                            bootstro.start(xxx, {
                                onComplete: function (params) {

                                },
                                onExit: function (params) {

                                },
                            });
                        });

                    },
                    failure: function (response) {
                        //alert(response);
                    }

                });
            }
        }

        function showInMap(id) {
            clearOverlays();

            $.ajax({
                type: "POST",
                url: 'Paginated.aspx/LoadOneMarker',
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


        function getCategories(id, dropDownID, addedVal) {
           
            $("#" + dropDownID).empty();
            $("#" + dropDownID).append(new Option("Select", "0"));
            $.ajax({
                type: "POST",
                url: 'Paginated.aspx/GetCategories',
                contentType: "application/json; charset=utf-8",
                data: '{"id":"' + id + '"}',

                dataType: "json",
                success: function (response) {

                    var dataCategories = $.parseJSON(response.d);
              

                  
                    for (var i = 0; i < dataCategories.length; i++) {
                        var cat = dataCategories[i];
                        $("#" + dropDownID).append(new Option(cat.Name, cat.ID));

                    }
                
                   
                    if(addedVal != "" && addedVal != null)
                        $("#" + dropDownID).val(addedVal);
                    else
                        $("#" + dropDownID).val("0");

                },
                failure: function (response) {
                    //alert(response);
                }

            });

        }




        



        $(document).ready(function () {


            $("#<%=listArea.ClientID%>").change(function () {
                
                var dropVal = $("#<%=listArea.ClientID%>").val();
                
                getCategories(dropVal, $("#listCat").attr('id'), "");

            });

            $("#<%=listFArea.ClientID%>").change(function () {

                var dropVal = $("#<%=listFArea.ClientID%>").val();
                getCategories(dropVal, $("#<%=listFType.ClientID%>").attr('id'), "");

            });

            GetIssues();
            initialize();







            if ($("#<%=hdProfileAdmin.ClientID%>").val() != $("#<%=hdProfile.ClientID%>").val()) {
                $("#<%=listPriority1.ClientID%>").attr('disabled', true);
                $("#<%=usersList.ClientID%>").attr('disabled', true);
                $("#<%=usersList.ClientID%>").find("input,button,textarea,select").attr("disabled", "disabled");

                $("#listCat").attr('disabled', true);
                $("#<%=listArea.ClientID%>").attr('disabled', true);
            }
            else {
                $("#<%=usersList.ClientID%> tr").remove();
                $("#<%=listArea.ClientID%>").change(function () {
                    $.ajax({
                        type: "POST",
                        url: 'Paginated.aspx/GetUsersForType',
                        contentType: "application/json; charset=utf-8",
                        data: '{"id":"' + $(this).val() + '"}',

                        dataType: "json",
                        success: function (response) {

                            var dataUsers = $.parseJSON(response.d);

                            fillCheckBoxListUsers(dataUsers);
                        },
                        failure: function (response) {
                            //alert(response);
                        }

                    });
                });

                $("#listCat").change(function () {
                    $("#<%=usersList.ClientID%> tr").remove();

                    $.ajax({
                        type: "POST",
                        url: 'Paginated.aspx/GetUsersForType',
                        contentType: "application/json; charset=utf-8",
                        data: '{"id":"' + $(this).val() + '"}',

                        dataType: "json",
                        success: function (response) {

                            var dataUsers = $.parseJSON(response.d);
                            fillCheckBoxListUsers(dataUsers);
                        },
                        failure: function (response) {
                            //alert(response);
                        }

                    });
                });
            }

            $("#btnFilter").click(function () {

                numRec = 30;
                index = 0;
                isfull = false;
                isloading = false;
                $('#dvIssues').html('');
                ft = $("#<%=listFType.ClientID%>").val();
                fp = $("#<%=listFPriority.ClientID%>").val();
                fs = $("#<%=listFState.ClientID%>").val();
                fa = $("#<%=listFArea.ClientID%>").val();
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



            $(window).scroll(function () {
                if ($(window).scrollTop() + $(window).height() > $(document).height() - 500) {

                    if (!isfull && !isloading) {
                        GetIssues();
                    }
                }
            });
        });


        var geocoder;
        var markerCluster;
        var marker;


        function initialize() {
            try {

                geocoder = new google.maps.Geocoder();
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
                url: 'Paginated.aspx/LoadMarkers',
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
            marker = new google.maps.Marker({
                id: markerData['id'],
                //map: map,
                title: markerData['name'],
                position: myLatlng,
                icon: "assets/img/markers/markers-base.png"
                //icon: markerData['icon']
            });
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
                    url: 'Paginated.aspx/GetDetails',
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



        function fillCheckBoxListUsers(dataUsers)
        {
            $("#<%=usersList.ClientID%> tr").remove();
            for (var i = 0; i < dataUsers.length; i++) {

                var usr = dataUsers[i];
                var tableRef = document.getElementById('<%= usersList.ClientID %>');

                var tableRow = tableRef.insertRow();
                var tableCell = tableRow.insertCell();

                var checkBoxRef = document.createElement('input');
                var labelRef = document.createElement('label');

                checkBoxRef.type = 'checkbox';
                labelRef.innerHTML = usr.Name;
                checkBoxRef.value = usr.UserID;

                tableCell.appendChild(checkBoxRef);
                tableCell.appendChild(labelRef);



            }

            if ($("#<%=hdProfileAdmin.ClientID%>").val() != $("#<%=hdProfile.ClientID%>").val()) {
                $("#<%=usersList.ClientID%>").attr('disabled', true);
                $("#<%=usersList.ClientID%>").find("input,button,textarea,select,checkbox").attr("disabled", "disabled");
                $("#<%=usersList.ClientID%> input[type=checkbox]").attr('disabled', 'true');

            }

        }

        // MODAL
        function openModal(id, i) {
            $('#imgIssue').attr('src', '');
            $('#linkPDF').attr('href', '');
            $('#linkPDF').hide();

            // get ISSUE Detail
            $.ajax({
                type: "POST",
                url: 'Paginated.aspx/GetIssueDetail',
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

                    if (data.Latitude + "" != "" && data.Longitude + "" != "") {
                        $("#<%=HDLatAndLong.ClientID%>").val(data.Latitude + "," + data.Longitude);
                        issueAddress = codeLatLngAddress();

                    }
                    else {
                        issueAddress = '';
                    }


                    if (data.ParentID != null)
                        $("#<%=listArea.ClientID%>").val(data.ParentID);
                    else
                        $("#<%=listArea.ClientID%>").val(data.TypeID);


                 

                    //getCategories(data.ParentID, $("#listCat").attr('id'), data.TypeID);


                    if (data.ParentID != null)
                        getCategories(data.ParentID, $("#listCat").attr('id'), data.TypeID);
                    else
                        getCategories(data.TypeID, $("#listCat").attr('id'), "");


                    if (data.PropertyID != null)
                        $("#<%=listProperties.ClientID%>").val(data.PropertyID);
                    else
                        $("#<%=listProperties.ClientID%>").val("0");



                    // get users

                    $("#<%=usersList.ClientID%>").empty();
                    $.ajax({
                        type: "POST",
                        url: 'Paginated.aspx/GetUsersForIssue',
                        contentType: "application/json; charset=utf-8",
                        data: '{"id":"' + id + '"}',

                        dataType: "json",
                        success: function (response) {

                            var dataUsers = $.parseJSON(response.d);

                            fillCheckBoxListUsers(dataUsers);

                            $("#<%=usersList.ClientID%>").find("input").each(function (i, ob) {
                                $(ob).prop("checked", false);
                            });


                            //alert(data.UsersAssigned)

                            if (data.UsersAssigned.length > 0)
                            {
                                $("#<%=usersList.ClientID%>").find("input").each(function (i, ob) {
                                    for(var i = 0; i < data.UsersAssigned.length; i++)
                                    {
                                 
                                           // alert($(ob).val());
                                           // alert(data.UsersAssigned[i]);
                                            if($(ob).val()  == data.UsersAssigned[i])
                                                $(ob).prop("checked", true);
                                  
                                    }
                                });
                            }
                          
                         




                        },
                        failure: function (response) {
                            //alert(response);
                        }

                    });



                    $("#<%=listState.ClientID%>").val(data.State);
                    $('#imgIssue').attr('src', data.PhotoUrl);

                  


                    if (data.HasPhoto == false)
                        $('#rotateLink').hide();
                    else
                        $('#rotateLink').show();

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




        // MODAL REPORTER
        function openModalReporter(id, isReporter) {
       


            // get ISSUE Detail
            $.ajax({
                type: "POST",
                url: 'Paginated.aspx/GetReporter',
                contentType: "application/json; charset=utf-8",
                data: '{"reporterID":' + id + ', "isReporter": ' + isReporter + '}',

                dataType: "json",
                success: function (response) {
                    // open modal and show data
                    var data = $.parseJSON(response.d);
                    $('#labelReporterRepBy').text(data.Name);
                    $('#labelPhoneRepBy').text(data.PhoneNumber);
                    $('#labelEmailRepBy').text(data.Email);
                   
                    $('#myModalReporter').modal('show');

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
            var type = $("#listCat").val();

          
            var state = $("#<%=listState.ClientID%>").val();

            var state = $("#<%=listState.ClientID%>").val();
            var message = $("#<%=txtMessageMail.ClientID%>").val();

            var notes = $("#<%=txtNotes.ClientID%>").val();

            var users = "";


            var property = $("#<%=listProperties.ClientID%>").val();


            $("#<%=usersList.ClientID%>").find("input:checked").each(function (i, ob) {
                users += $(ob).val() + ";";
            });


         

            var area = $("#<%=listArea.ClientID%>").val();

            if (type == 0) {
               // alert(type);
                type = area;
            }
           


            //  var address = codeLatLng($("#<%=HDLatAndLong.ClientID%>").val());

            var chk = 0;
            if ($("#<%=chkSendMail.ClientID%>").is(':checked')) {
                if ($("#<%=txtMessageMail.ClientID%>").val() == '') {
                    //openModal($("#<%=hdIssueID.ClientID%>").val());
                    $.pnotify({
                        title: 'Error!',
                        text: 'Set message!',
                        type: 'error',
                        hide: false
                    });
                    return;
                }
                chk = 1;
            }



            // para mostrar dados do inicio
         

            
            isfull = false;
            isloading = false;

            $.ajax({
                type: "POST",
                url: 'Paginated.aspx/SaveIssue',
                contentType: "application/json; charset=utf-8",
                data: '{"id":"' + $("#<%=hdIssueID.ClientID%>").val() + '", "typeID":"' + type + '", "priority": "' +
                    priority + '", "state": "' + state + '", "sendMail":"' + chk + '", "message": "' + message +
                    '", "typeFID":"' + ft + '", "priorityF":"' + fp + '", "stateF":"' + fs + '", "notes":"' + notes + '", "users": "' + users + '", "address": "' + issueAddress +
                    '", "imgSrc":"' + $('#imgIssue').attr('src') + '", "parent":"' + fa + '", "property":"' + property   + '"}',



                    dataType: "json",
                    success: function (response) {

                        if (response.d != 'ERROR') {
                            $('#myModal').modal('hide');

                            //$('#dvIssues').html('');
                            //numRec = numRecNew;


                            $.ajax({
                                type: "POST",
                                url: 'Paginated.aspx/GetIssue',
                                contentType: "application/json; charset=utf-8",
                                data: '{"id":"' + $("#<%=hdIssueID.ClientID%>").val() + '" }',



                                dataType: "json",
                                success: function (response) {

                                    if (state == 4)
                                        $('#bootstrodemo' + $("#<%=hdIssueID.ClientID%>").val()).html('');
                                    else{

                                    $('#bootstrodemo' + $("#<%=hdIssueID.ClientID%>").val()).html(response.d);

                                    $(".boot").click(function () {

                                        var tourID = $(this).attr('id');
                                        var xxx = "#bootstro" + tourID;

                                        bootstro.start(xxx, {
                                            onComplete: function (params) {

                                            },
                                            onExit: function (params) {

                                            },
                                        });
                                    });

                                    }

                                }
                            });

                         


                        }
                        else {
                            //openModal($("#<%=hdIssueID.ClientID%>").val());
                            $.pnotify({
                                title: 'Error!',
                                text: 'Error updating Fish Specie Report!',
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


            // code to get address
            function codeLatLng() {

                var input = $("#<%=HDLatAndLong.ClientID%>").val();
                var latlngStr = input.split(',', 2);
                var lat = parseFloat(latlngStr[0]);
                var lng = parseFloat(latlngStr[1]);
                var latlng = new google.maps.LatLng(lat, lng);
                geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[0]) {
                         
                            $.ajax({
                                type: "POST",
                                url: 'Paginated.aspx/GeneratePDF',
                                contentType: "application/json; charset=utf-8",
                                data: '{"id":"' + $("#<%=hdIssueID.ClientID%>").val() + '", "imgSrc":"' + $('#imgIssue').attr('src') + '", "address": "' + results[0].formatted_address + '" }',

                                dataType: "json",
                                success: function (response) {

                                    if (response.d == 'ERROR') {
                                        $.pnotify({
                                            title: 'Error!',
                                            text: 'Error generating pdf!',
                                            type: 'error',
                                            hide: false
                                        });

                                    }
                                    else {
                                        $('#linkPDF').attr('href', response.d);
                                        $('#linkPDF').show();

                                      

                                    
                                    }
                                    $("#pdfBtn").css("opacity", 1);
                                },
                                failure: function (response2) {
                                    //alert(response2);
                                    $("#pdfBtn").css("opacity", 1);
                                }

                            });



                            return results[0].formatted_address;

                        } else {
                            //alert('No results found');
                            return '';
                        }
                    } else {
                        //alert('Geocoder failed due to: ' + status);
                        return '';
                    }
                });
            }

            function codeLatLngAddress() {

                var input = $("#<%=HDLatAndLong.ClientID%>").val();
            var latlngStr = input.split(',', 2);
            var lat = parseFloat(latlngStr[0]);
            var lng = parseFloat(latlngStr[1]);
            var latlng = new google.maps.LatLng(lat, lng);
            geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        issueAddress = results[0].formatted_address;
                        return results[0].formatted_address;

                    } else {
                        //alert('No results found');
                        return '';
                    }
                } else {
                    //alert('Geocoder failed due to: ' + status);
                    return '';
                }
            });
        }


    </script>
</asp:Content>
