;
var stressStrainPlot = {};

stressStrainPlot.DiagramData = function () {
    this.LegendDesc = null;
    this.LegendDescText = null;
    this.RT = null;
    this.Temperature = null;
    this.TemperatureText = null;
    this.TrueDetails = null;
    this.EngineeringDetails = null;
};

var diagramData = null;

var seriesDataEng = new Array();
var legendDataEng = new Array();
var seriesDataTrue = new Array();
var legendDataTrue = new Array();

//
// http://www.delimited.io/blog/2014/3/3/creating-multi-series-charts-in-d3-lines-bars-area-and-streamgraphs
//
stressStrainPlot.InitData = function (modelValue) {
    //.PointsForDiagram.Curves[i; i=0,..,n-1].PointsForDiagram[j; j=0,..,n-1].X
    //                                                                       .Y
    //                                       .Temperature

    //if (diagramData === null && typeof diagramData === "object") {
    diagramData = $.parseJSON(modelValue);
    //};

    // Prepare data for the True chart
    var trueData = diagramData.TrueDetails;
    var trueDataCurves = trueData.PointsForDiagram.Curves;
    seriesDataTrue = stressStrainPlot.InitSeriesData(trueDataCurves);
    legendDataTrue = stressStrainPlot.InitLegendData(trueDataCurves);
    //
    var engData = diagramData.EngineeringDetails;
    var engDataCurves = engData.PointsForDiagram.Curves;
    seriesDataEng = stressStrainPlot.InitSeriesData(engDataCurves);
    legendDataEng = stressStrainPlot.InitLegendData(engDataCurves);
}

stressStrainPlot.InitSeriesData = function (dataCurves) {
    var seriesData = new Array();
    for (i = 0; i < dataCurves.length; ++i) {
        var oneSerieName = dataCurves[i].Temperature.toString();
        var oneSeriePoints = dataCurves[i].PointsForDiagram;

        var chartValuesValues = new Array();

        for (j = 0; j < oneSeriePoints.length; ++j) {
            chartValuesValues.push(
                {
                    name: oneSerieName
                    , label: oneSeriePoints[j].X.toString()
                    , value: oneSeriePoints[j].Y
                });
        }
        //
        //seriesData.push(chartValues);
        seriesData.push(
            {
                name: oneSerieName
                , values: chartValuesValues
            });
    }
    return seriesData;
}

stressStrainPlot.InitLegendData = function (dataCurves) {
    var legendData = new Array();
    for (i = 0; i < dataCurves.length; ++i) {
        var oneLegendName = dataCurves[i].Temperature.toString();
        legendData.push(oneLegendName);
    }
    return legendData.sort(function (a, b) {
        return parseFloat(b) - parseFloat(a);
    });
}


stressStrainPlot.PlotChartAll = function () {
    // wait some time before js finalized initialization
    setTimeout(function () {
        stressStrainPlot.PlotChart(1);
        stressStrainPlot.PlotChart(2);
    }, 1000);
}


stressStrainPlot.PlotChart = function (type) {
    // type = 1: True
    // type = 2; Eng

    var varNames;
    var seriesData;

    var svgContainer;
    var saveContainer;
    var imgContainer
    if (type == 1) {
        svgContainer = "#trueChart";
        saveContainer = "#saveTrue";
        varNames = legendDataTrue;
        seriesData = seriesDataTrue;
        imgContainer = "#imgTrueSS"
    } else {
        svgContainer = "#engChart";
        saveContainer = "#saveEng";
        varNames = legendDataEng;
        seriesData = seriesDataEng;
        imgContainer = "#imgEngSS"
    }

    var isIE = false || !!document.documentMode; // At least IE6
    if (isIE == true) {
        $(saveContainer).hide();
    } else {
        $(saveContainer).show();
    };

    // reset SVG chart container
    $(svgContainer).html('');

   
    //console.log("seriesData", seriesData);
    //console.log("legendData", varNames);

    // Init variables
    var margin = { top: 20, right: 80, bottom: 35, left: 60 };
    var width = 550 - margin.left - margin.right;
    var height = 400 - margin.top - margin.bottom;

    //var x = d3.scale.ordinal()
    //    .rangeRoundBands([0, width], .1);
    var x = d3.scale.linear()
      .rangeRound([0, width]);

    var y = d3.scale.linear()
        .rangeRound([height, 0]);



    // See https://github.com/mbostock/d3/wiki/SVG-Shapes for different interpolates
    var line = d3.svg.line()
        .interpolate("cardinal")
        //.x(function (d) { return x(d.label) + x.rangeBand() / 2; })
        .x(function (d) { return x(+d.label); })
        .y(function (d) { return y(d.value); });

    //var color = d3.scale.ordinal()
    //    .range(["#001c9c", "#101b4d", "#475003", "#9c8305", "#d3c47c"]);
    var color = d3.scale.category10();

    var svg = d3.select(svgContainer).append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
      .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


    // Init domains
    color.domain(varNames);
    //x.domain(xValues.map(function (d) {
    //    return d.toString();
    //}));
    x.domain([
     d3.min(seriesData, function (c) {
         //return d3.min(c.values, function (d) { return +d.label; });
         return 0;
     }),
     d3.max(seriesData, function (c) {
         return d3.max(c.values, function (d) { return +d.label; });
     })
    ]);
    y.domain([
      d3.min(seriesData, function (c) {
          //return d3.min(c.values, function (d) { return d.value; });
          return 0;
      }),
      d3.max(seriesData, function (c) {
          return d3.max(c.values, function (d) { return d.value; }) * 1.2;
      })
    ]);


    // Axes
    //var xAxis = d3.svg.axis()
    //  .scale(x)
    //  .orient("bottom")
    //  .ticks(5);
    var xAxis = d3.svg.axis()
                .scale(x)
                .orient("bottom")
                .ticks(5)
                .tickSize(-height, 0, 0)
                .tickFormat(function (d, i) { return d; })

    var yAxis = d3.svg.axis()
                .scale(y)
                .orient("left")
                .ticks(5)
                .tickSize(-width, 0, 0)

    var rotateTranslateY = d3.svg.transform().rotate(-90).translate(-150, -60);
    svg.append("g")
           .attr("class", "x axis")
           //.style("stroke", "red")    // font na x osi
           //.style("stroke-opacity", "0.75")
	       //.style("stroke-width", "0.1px")
            .style("stroke", "grey")
           .style("font-size", "15px")		// font za vrednosti na x-osi
           .style("font-family", "NexusSansCompPro, Arial, Helvetica, sans-serif")
           .style("stroke-opacity", "0.75")
           .style("stroke-width", "0.5px")	 // debljina x-mreze
           .attr("transform", "translate(0," + height + ")")
           .call(xAxis)
        .append("text")
            .attr("transform", "translate(225,30)")
            .attr("x", 6)
            .attr("dx", ".71em")
            .style("text-anchor", "end")
            .style("font-size", "15px")		// font za labelu na x-osi
            .style("fill", "grey")
            .style("font-weight", "100")
            .style("stroke-opacity", "0.75")
	        .style("stroke-width", "0.1px")
            .text("Strain [m/m]");
    svg.append("g")
            .attr("class", "y axis")
            .style("stroke", "grey")
            .style("font-size", "15px")		// font za vrednosti na y-osi
            .style("font-family", "NexusSansCompPro, Arial, Helvetica, sans-serif")
            .style("stroke-opacity", "0.75")
	        .style("stroke-width", "0.5px")	 // debljina y-mreze
            .call(yAxis)
      .append("text")
        //.attr("transform", "rotate(-90)")
        .attr("transform", rotateTranslateY)
        .attr("y", 6)
        .attr("dy", ".7em")
        .style("text-anchor", "end")
        .style("font-size", "15px")		// font za labelu na y-osi
        .style("fill", "grey")
        .style("font-weight", "100")
        .style("stroke-opacity", "0.75")
	    .style("stroke-width", "0.1px")
        .text("Stress [MPa]");

    // Series
    var series = svg.selectAll(".series")
            .data(seriesData)
          .enter().append("g")
            .attr("class", "series");

    series.append("path")
         .attr("class", "line")
         .attr("d", function (d) { return line(d.values); })
         .style("stroke", function (d) { return color(d.name); })
         .style("stroke-width", "2px")
         .style("fill", "none");


    // Tooltip and data
    if (seriesData.length > 1) {
        series.selectAll(".line")
             .on("click", function (d) { showData.call(this, d); })
             .on("mouseover", function (d) { document.body.style.cursor = "pointer"; })
             .on("mouseout", function (d) { document.body.style.cursor = "default"; })
             .append("svg:title")
                .text(function (d) { return 'Click to view table points for: ' + d.name + '/C'; });
    } else {
        series.selectAll(".line")
             .on("click", function (d) { return false; })
             .on("mouseover", function (d) { document.body.style.cursor = "pointer"; })
             .on("mouseout", function (d) { document.body.style.cursor = "default"; })
             .append("svg:title")
                .text(function (d) { return d.name + 'C'; });
    }

    function showData(d) {
        if (type == 1) {
            $('#selectTemperature_TrueSS').val(d.name);
            materialDetails.changeTemperatureTrueSS();
        };

        if (type == 2) {
            $('#selectTemperature_EngineeringSS').val(d.name);
            materialDetails.changeTemperatureEngineeringSS();
        };
    }

    // Legend
    var legend = svg.selectAll(".legend")
          .data(varNames.slice().reverse())
        .enter().append("g")
          .attr("class", "legend")
          .attr("transform", function (d, i) { return "translate(80," + i * 20 + ")"; });

    legend.append("rect")
           .attr("x", width - 10)
           .attr("width", 10)
           .attr("height", 3)
           .style("fill", color)
           .style("stroke", "grey")
           .attr("transform", "translate(0,4)");

    legend.append("text")
        .attr("x", width - 12)
        .attr("y", 6)
        .attr("dy", ".35em")
        .style("text-anchor", "end")
        .style("font-family", "NexusSansCompPro, Arial, Helvetica, sans-serif")
        .style("font-size", "12px")
        .style("fill", "grey")
        .text(function (d) { return d + '°C'; });

    // Attach action for Save image
    //
    // http://techslides.com/save-svg-as-an-image
    // http://spin.atomicobject.com/2014/01/21/convert-svg-to-png/
    //


    $('head').append("<meta name='charset' content='utf-8'>");

    d3.select(saveContainer).on("click", function () {

        // create  image and attach src on it
        var image = new Image();

        // Version a), with SVG diagram
        var html = d3.select(svgContainer + " svg")
             .attr("version", 1.1)
             .attr("xmlns", "http://www.w3.org/2000/svg")
             .node().parentNode.innerHTML;

        var doctype = '<?xml version="1.0" standalone="no"?><!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">';
        html = doctype + html;

        //console.log("img html: ", html);
        //image.src = 'data:image/svg+xml;base64,' + btoa(html);      // ne radi, a tako bi trebalo da bude ??????
        image.src = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(html)));
        // End of Version a), with SVG diagram


        // Version b), with already generated <img> on the page
        //image.src = $(imgContainer).attr('src');
        // End of Version b), with already generated <img> on the page

        image.onload = function () {
            // create canvas fom image
            var canvas = document.createElement('canvas');
            canvas.width = image.width;
            canvas.height = image.height;
            var context = canvas.getContext('2d');
            context.drawImage(image, 0, 0);

            // create a element for download action
            var a = document.createElement('a');
            a.download = "image.png";
            a.href = canvas.toDataURL('image/png');
            document.body.appendChild(a);

            a.addEventListener("click", function (e) {
                a.parentNode.removeChild(a);
            });
            a.click();
        };

    });
    // /Attach action for Save image
}