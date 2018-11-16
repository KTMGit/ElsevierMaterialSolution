;
var chemicalMaterialsPlot = {};

chemicalMaterialsPlot.DiagramData = {};

var diagramData = null;

var seriesDataTrue = new Array();
var legendDataTrue = new Array();
var propertyName = '';
//
// http://www.delimited.io/blog/2014/3/3/creating-multi-series-charts-in-d3-lines-bars-area-and-streamgraphs
//
chemicalMaterialsPlot.InitData = function (modelValue, property,phaseName, legendX, legendY) {
    //.PointsForDiagram.Curves[i; i=0,..,n-1].PointsForDiagram[j; j=0,..,n-1].X
    //                                                                       .Y
    //                                       .Temperature

    //if (diagramData === null && typeof diagramData === "object") {
    diagramData = $.parseJSON(modelValue);
    //};

    // Prepare data for the True chart
    var dataCurves = [];
    var curveData = {};
    curveData.serieName = property;
    curveData.diagramData = diagramData;
    curveData.legendX = legendX;
    curveData.legendY = legendY;
    dataCurves.push(curveData);
    propertyName = property;
    seriesDataTrue = chemicalMaterialsPlot.InitSeriesData(dataCurves);
    // legendDataTrue = chemicalMaterialsPlot.InitLegendData(dataCurves);

    //
    //var engData = diagramData.EngineeringDetails;
    //var engDataCurves = engData.PointsForDiagram.Curves;
    //seriesDataEng = chemicalMaterialsPlot.InitSeriesData(engDataCurves);
    //legendDataEng = chemicalMaterialsPlot.InitLegendData(engDataCurves);

}

chemicalMaterialsPlot.InitSeriesData = function (dataCurves) {
    var seriesData = new Array();
    for (i = 0; i < dataCurves.length; ++i) {
        var oneSerieName = diagramData[0];
        //   var oneSeriePoints = dataCurves[i].PointsForDiagram;

        var chartValuesValues = new Array();

        for (j = 0; j < diagramData.length; ++j) {
            chartValuesValues.push(
                {
                    name: oneSerieName
                    , label: diagramData[j].X.toString()
                    , value: diagramData[j].Y.toString()
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

chemicalMaterialsPlot.InitLegendData = function (dataCurves) {
    var legendData = new Array();
    for (i = 0; i < dataCurves.length; ++i) {
        var oneLegendName = 'Temperature °C';
        //var oneLegendName = dataCurves
        legendData.push(oneLegendName);
    }
    return legendData.sort(function (a, b) {
        return parseFloat(b) - parseFloat(a);
    });
}


chemicalMaterialsPlot.PlotChartAll = function (id,phaseId,legendX,legendY) {
    // wait some time before js finalized initialization
    //  setTimeout(function () {
    chemicalMaterialsPlot.PlotChart(id, phaseId, legendX, legendY);

    //}, 1000);
}


chemicalMaterialsPlot.PlotChart = function (id, phaseId, legendX, legendY) {

    var varNames;
    var seriesData;
    var xValues = new Array();

    var svgContainer;
    var saveContainer;
    var imgContainer

    svgContainer = "#chemPropChart_" + id+'_'+phaseId;
    saveContainer = "#saveChemicalMaterials_" + id + '_' + phaseId;
    varNames = legendDataTrue;
    seriesData = seriesDataTrue;
    imgContainer = "#imgTrueSS"

    var isIE = false || !!document.documentMode; // At least IE6
    if (isIE == true) {
        $(saveContainer).hide();
    } else {
        $(saveContainer).show();
    };


    // reset SVG chart container
    $(svgContainer).html('');

    // Fill x-labels
    var scale = 1;
    for (i = 0; i < seriesData.length; ++i) {
        var vals = seriesData[i].values;
        for (j = 0; j < vals.length; ++j) {
            xValues.push(+vals[j].label);
        };
    }

    scale = 1;
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
        .x(function (d) { return x(+d.label / scale); })
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
         return d3.min(c.values, function (d) { return +d.label; });
     }),
     d3.max(seriesData, function (c) {
         return d3.max(c.values, function (d) { return +d.label; });
     })
    ]);
    y.domain([
      d3.min(seriesData, function (c) {
          if (d3.min(c.values, function (d) { return parseFloat(d.value); }) > 0) {
              return d3.min(c.values, function (d) { return parseFloat(d.value); }) * 0.5;
          } else {
              return d3.min(c.values, function (d) { return parseFloat(d.value); }) * 1.2;
          }  
      }),
      d3.max(seriesData, function (c) {
          return d3.max(c.values, function (d) { return parseFloat(d.value); }) * 1.2;
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
    //.tickFormat("");


    var rotateTranslateY = d3.svg.transform().rotate(-90).translate(0, -60);
    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .ticks(5)
        .tickSize(-width, 0, 0)
    var axisX = '';
    switch (legendX) {
        case 'K':
            axisX = 'temperature';
            break;
        case '°C':
            axisX = 'temperature';
            break;
        case 'kPa':
            axisX = 'pressure';
            break;
        case 'nm':
            axisX = 'wavelength';
            break;
        case 'mg/kg':
            axisX = 'concentration';
            break;
        case 'ppm':
            axisX = 'concentration';
            break;
        case 'wt%':
            axisX = 'solution';
            break;
        default:
            // nothing to do
    }

    var textAxisX = axisX + " (" + legendX + ")";
    var textAxisY = propertyName;    
    if (legendY != null) {
        legendY = legendY.replace("<sup>", "").replace("</sup>", "").replace("<sub>", "").replace("</sub>", "");
        textAxisY += " (" + legendY + ")";
    }    
    

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
            .text(textAxisX);
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
        .style("text-anchor", "end")
        .style("font-size", "15px")		// font za labelu na y-osi
        .style("fill", "grey")
        .style("font-weight", "100")
        .style("stroke-opacity", "0.75")
	    .style("stroke-width", "0.1px")
        .text(textAxisY);


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
    //if (seriesData.length > 1) {
    //    series.selectAll(".line")
    //         .on("click", function (d) { showData.call(this, d); })
    //         .append("svg:title")
    //            .text(function (d) { return 'Click to view table points for: ' + d.name + '/C'; });
    //} else {
    //    series.selectAll(".line")
    //         .on("click", function (d) { return false; })
    //         .append("svg:title")
    //            .text(function (d) { return d.name + 'C'; });
    //}

    //function showData(d) {
    //    if (type == 1) {
    //        $('#selectTemperature_TrueSS').val(d.name);
    //        materialDetails.changeTemperatureTrueSS();
    //    };

    //    if (type == 2) {
    //        $('#selectTemperature_EngineeringSS').val(d.name);
    //        materialDetails.changeTemperatureEngineeringSS();
    //    };
    //}

    // Legend
    //var legend = svg.selectAll(".legend")
    //      .data(varNames.slice().reverse())
    //    .enter().append("g")
    //      .attr("class", "legend")
    //      .attr("transform", function (d, i) { return "translate(80," + i * 20 + ")"; });

    //legend.append("rect")
    //       .attr("x", width - 10)
    //       .attr("width", 10)
    //       .attr("height", 3)
    //       .style("fill", color)
    //       .style("stroke", "grey")
    //       .attr("transform", "translate(0,4)");

    //legend.append("text")
    //    .attr("x", width - 12)
    //    .attr("y", 6)
    //    .attr("dy", ".35em")
    //    .style("text-anchor", "end")
    //    .style("font-family", "NexusSansCompPro, Arial, Helvetica, sans-serif")
    //    .style("font-size", "12px")
    //    .style("fill", "grey")
    //    .text(function (d) { return d + '°C'; });


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


chemicalMaterialsPlot.FillTablePoints = function (modelValue, name, phaseName,legendX, legendY, id, phaseId) {

    diagramData = $.parseJSON(modelValue);
    var axisX = '';
    switch (legendX) {
        case 'K':
            axisX = 'temperature';
            break;
        case '°C':
            axisX = 'temperature';
            break;
        case 'kPa':
            axisX = 'pressure';
            break;
        case 'nm':
            axisX = 'wavelength';
            break;
        case 'mg/kg':
            axisX = 'concentration';
            break;
        case 'ppm':
            axisX = 'concentration';
            break;
        case 'wt%':
            axisX = 'solution';
            break;
        default:
            // nothing to do
    }

    var allLegendX = legendX != '' ? ' (' + legendX + ')' : '';
    var allLegendY = legendY != '' ? ' (' + legendY + ')' : '';


    html = '';
    html += '<thead><tr>';
    html += '<th>' + axisX + allLegendX + '</th>';
    html += '<th>' + name + allLegendY + '</th>';
    html += '</thead>';
    html += '<tbody>';
    for (var i = 0; i < diagramData.length; i++) {  
        html += '<tr class="trOrigin">';
        html += '<td>' + diagramData[i].X + '</td>';
        html += '<td>' + diagramData[i].Y + '</td>';
        html += '</tr>';        
    }
    html += '</tbody>';
    var tableData = '';
    tableData = "#chemPropTableData_" + id + '_' + phaseId;
    $(tableData).html(html);
}