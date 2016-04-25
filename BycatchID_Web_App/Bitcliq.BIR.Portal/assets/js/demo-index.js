jQuery(document).ready(function() {

  
    


    //Flot

    function randValue() {
        return (Math.floor(Math.random() * (2)));
    }

    var viewcount = [
        [1, 787 + randValue()],
        [2, 740 + randValue()],
        [3, 560 + randValue()],
        [4, 860 + randValue()],
        [5, 750 + randValue()],
        [6, 910 + randValue()],
        [7, 730 + randValue()]
    ];

    var uniqueviews = [
        [1, 179 + randValue()],
        [2, 320 + randValue()],
        [3, 120 + randValue()],
        [4, 400 + randValue()],
        [5, 573 + randValue()],
        [6, 255 + randValue()],
        [7, 366 + randValue()]
    ];

    
    var usercount = [
        [1, 70 + randValue()],
        [2, 260 + randValue()],
        [3, 30 + randValue()],
        [4, 147 + randValue()],
        [5, 333 + randValue()],
        [6, 155 + randValue()],
        [7, 166 + randValue()]
    ];

    var plot_statistics = $.plot($("#site-statistics"), [{
        data: viewcount,
        label: "View Count"
    }, {
        data: uniqueviews,
        label: "Unique Views"
    }, {
        data: usercount,
        label: "User Count"
    }], {
        series: {
            lines: {
                show: true,
                lineWidth: 1.5,
                fill: 0.05
            },
            points: {
                show: true
            },
            shadowSize: 0
        },
        grid: {
            labelMargin: 10,
            hoverable: true,
            clickable: true,
            borderWidth: 0
        },
        colors: ["#60cde8", "#71a5e7", "#aa73c2"],
        xaxis: {
            tickColor: "transparent",
            ticks: [[1, "S"], [2, "M"], [3, "T"], [4, "W"], [5, "T"], [6, "F"], [7, "S"]],
            tickDecimals: 0,
            autoscaleMargin: 0,
            font: {
                color: '#8c8c8c',
                size: 12
            }
        },
        yaxis: {
            ticks: 4,
            tickDecimals: 0,
            tickColor: "#e3e4e6",
            font: {
                color: '#8c8c8c',
                size: 12
            },
            tickFormatter: function (val, axis) {
                if (val>999) {return (val/1000) + "K";} else {return val;}
            }
        },
        legend : {
            labelBoxBorderColor: 'transparent'
        }
    });

   



    var previousPoint = null;
        $("#site-statistics").bind("plothover", function (event, pos, item) {
        $("#x").text(pos.x.toFixed(2));
        $("#y").text(pos.y.toFixed(2));
        if (item) {
            if (previousPoint != item.dataIndex) {
                previousPoint = item.dataIndex;

                $("#tooltip").remove();
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);

                showTooltip(item.pageX, item.pageY-7, item.series.label + ": " + Math.round(y));

            }
        } else {
            $("#tooltip").remove();
            previousPoint = null;
        }
    });

    var previousPointBar = null;
        $("#budget-variance").bind("plothover", function (event, pos, item) {
        $("#x").text(pos.x.toFixed(2));
        $("#y").text(pos.y.toFixed(2));
        if (item) {
            if (previousPointBar != item.dataIndex) {
                previousPointBar = item.dataIndex;

                $("#tooltip").remove();
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);

                showTooltip(item.pageX+20, item.pageY, item.series.label + ": $" + Math.round(y)+"K");

            }
        } else {
            $("#tooltip").remove();
            previousPointBar = null;
        }
    });



    function showTooltip(x, y, contents) {
        $('<div id="tooltip" class="tooltip top in"><div class="tooltip-inner">' + contents + '<\/div><\/div>').css({
            display: 'none',
            top: y - 40,
            left: x - 55,
        }).appendTo("body").fadeIn(200);
    }



    var container = $("#server-load");

    // Determine how many data points to keep based on the placeholder's initial size;
    // this gives us a nice high-res plot while avoiding more than one point per pixel.

    var maximum = container.outerWidth() / 2 || 300;
    var data = [];

    function getRandomData() {

        if (data.length) {
            data = data.slice(1);
        }

        while (data.length < maximum) {
            var previous = data.length ? data[data.length - 1] : 50;
            var y = previous + Math.random() * 10 - 5;
            data.push(y < 0 ? 0 : y > 100 ? 100 : y);
        }

        // zip the generated y values with the x values
        var res = [];
        for (var i = 0; i < data.length; ++i) {
            res.push([i, data[i]])
        }
        return res;
    }

    //

    series = [{
        data: getRandomData()
    }];

    //

    var plot = $.plot(container, series, {
        series: {
            lines: {
                show: true,
                lineWidth: 1.5,
                fill: 0.15
            },
            shadowSize: 0
        },
        grid: {
            
            labelMargin: 10,
            tickColor: "#e6e7e8",
            borderWidth: 0
        },
        colors: ["#f1c40f"],
        xaxis: {
            tickFormatter: function() {
                return "";
            },
            tickColor: "transparent"
        },
        yaxis: {
            ticks: 2,
            min: 0,
            max: 100,
            tickFormatter: function (val, axis) {
                return val + "%";
            },
            font: {
                color: '#8c8c8c',
                size: 12
            }
        },
        legend: {
            show: true
        }
    });

    // Update the random dataset at 25FPS for a smoothly-animating chart

    setInterval(function updateRandom() {
        series[0].data = getRandomData();
        plot.setData(series);
        plot.draw();
    }, 40);


});


