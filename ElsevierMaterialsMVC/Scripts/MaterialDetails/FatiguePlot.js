;
var fatiguePlot = {};

// Define Array prototipes for unique and contains functionality
if (typeof Array.prototype.contains !== 'function') {
    Array.prototype.contains = function (v) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] === v) return true;
        }
        return false;
    };
};

if (typeof Array.prototype.unique !== 'function') {
    Array.prototype.unique = function () {
        var arr = [];
        for (var i = 0; i < this.length; i++) {
            if (!arr.contains(this[i])) {
                arr.push(this[i]);
            }
        }
        return arr;
    };
};

if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    };
};


var diagramData = null;

var seriesDataStrain = new Array();
var legendDataStrain = new Array();
var seriesDataStress = new Array();
var legendDataStress = new Array();
var fatigueType;

//
// http://www.delimited.io/blog/2014/3/3/creating-multi-series-charts-in-d3-lines-bars-area-and-streamgraphs
//
fatiguePlot.InitData = function (modelValue, type) {
    diagramData = $.parseJSON(modelValue);
    fatigueType = type;
    //
    if (fatigueType == "1") {
        seriesDataStrain = fatiguePlot.InitSeriesData(diagramData.Curves);
        legendDataStrain = fatiguePlot.InitLegendData(diagramData.Curves);
    } else {
        seriesDataStress = fatiguePlot.InitSeriesData(diagramData.Curves);
        legendDataStress = fatiguePlot.InitLegendData(diagramData.Curves);
    }

}

fatiguePlot.InitSeriesData = function (dataCurves) {
    var seriesData = new Array();
    for (i = 0; i < dataCurves.length; ++i) {
        var oneSerieName = dataCurves[i].CurveName.toString();
        var oneSeriePoints = dataCurves[i].PointsForDiagram;

        var chartValues = new Array();

        for (j = 0; j < oneSeriePoints.length; ++j) {
            if (fatigueType == "1") {
                chartValues.push(
               {
                   name: oneSerieName
                   , label: oneSeriePoints[j].X.toString()
                   , value: parseFloat(Math.pow(10, oneSeriePoints[j].Y))
               });
            } else {
                chartValues.push(
               {
                   name: oneSerieName
                   , label: oneSeriePoints[j].X.toString()
                   , value: parseInt(Math.pow(10, oneSeriePoints[j].Y))
               });
            };

        };

        seriesData.push(
            {
                name: oneSerieName
                , values: chartValues
            });
    }
    return seriesData;
}

fatiguePlot.InitLegendData = function (dataCurves) {
    var legendData = new Array();
    for (i = 0; i < dataCurves.length; ++i) {
        var oneLegendName = dataCurves[i].CurveName.toString();
        legendData.push(oneLegendName);
    }
    return legendData.sort(function (a, b) {
        return (a - b);
    });
}

fatiguePlot.PlotChartTimeout = function (type) {
    // wait some time before js finalized initialization
    /*
   1 - Strain
   2 - Stress
   */
    setTimeout(function () {
        fatiguePlot.PlotChart(type);
    }, 1000);
}


fatiguePlot.PlotChart = function (type) {
    /*
    1 - Strain
    2 - Stress
    */
    var varNames, seriesData, xLabel, yLabel, legendWidth;

    if (type == "1") {
        varNames = legendDataStrain;
        seriesData = seriesDataStrain;
        xLabel = "Reversals to failure, 2Nf (log scale)";
        yLabel = "Strain Amplitude (log scale) [MPa]";
        legendWidth = 70;
    } else {
        varNames = legendDataStress;
        seriesData = seriesDataStress;
        xLabel = "Cycle to failure, Nf (log scale)";
        yLabel = "Stress Amplitude (log scale) [MPa]";
        legendWidth = 90;
    };

    if (typeof seriesData === "undefined") {
        return;
    }

    //console.log("seriesData" + type + ":", seriesData);
    //console.log("legendData", varNames);

    // Show actual curve-name
    //$("#curveName" + type).html("Points for: " + varNames.reverse()[0]);

    var svgContainer = "#fatigueChart" + type;
    var saveContainer = "#saveFatigue" + type;
    var imgContainer = "#imgFatigue" + type;

    var isIE = false || !!document.documentMode; // At least IE6
    if (isIE == true) {
        $(saveContainer).hide();
    } else {
        $(saveContainer).show();
    };

    var xValues = new Array();
    var yValues = new Array();

    // reset SVG chart container
    $(svgContainer).html('');

    // Fill x-labels
    var scale = 1;
    for (i = 0; i < seriesData.length; ++i) {
        var vals = seriesData[i].values;
        for (j = 0; j < vals.length; ++j) {
            xValues.push('1E' + vals[j].label);
        };
    };
    xValues = xValues.unique().sort();
    //console.log("xValues" + type + ":", xValues);

    scale = 1;

    // Init variables
    var margin = { top: 20, right: legendWidth, bottom: 35, left: 60 };
    var width = 550 - margin.left - margin.right;
    var height = 400 - margin.top - margin.bottom;

    //var x = d3.scale.ordinal()
    //    .rangeRoundBands([0, width], .1);
    var x = d3.scale.linear()
      .rangeRound([0, width]);

    var y = d3.scale.log()
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
         //console.log("xMin" + type + ":", d3.min(c.values, function (d) { return +d.label; }));
         return d3.min(c.values, function (d) { return +d.label; });
     }),
     d3.max(seriesData, function (c) {
         //console.log("xMax" + type + ":", d3.max(c.values, function (d) { return +d.label; }));
         return d3.max(c.values, function (d) { return +d.label; });
     })
    ]);
    y.domain([
      d3.min(seriesData, function (c) {
          return d3.min(c.values, function (d) { return d.value; });
      }),
      d3.max(seriesData, function (c) {
          return d3.max(c.values, function (d) { return d.value; });
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
        .ticks(xValues.length - 1)
        .tickSize(-height, 0, 0)
        .tickFormat(function (d, i) { return xValues[i]; })

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .ticks(5)
        .tickSize(-width, 0, 0)
        .tickFormat(function (d, i) {
            var toReturn = (parseInt(d * 100000000000) / 100000000000);
            if (type == "1") {
                toReturn = toReturn.toExponential();
                var firstChar = toReturn.toString().substr(0, 1);
                var lastChar = toReturn.toString().substr(toReturn.toString().length - 1);
                //toReturn = toReturn.toString().substr(toReturn.toString().length - 1) == "1" ? toReturn : "";
                toReturn = (firstChar == "1" ? toReturn : "");
            };
            //console.log("toReturn: ", toReturn);
            return (toReturn);
        })

    var rotateTranslateY = d3.svg.transform()
                            .rotate(-90)
                            .translate(-80, -60);
    svg.append("g")
           .attr("class", "x axis")
           //.style("stroke", "red")    // font na x osi
           //.style("stroke-opacity", "0.75")
	       //.style("stroke-width", "0.1px")
           .style("stroke", "grey")
           .style("font-size", "15px")      // font za vrednosti na x-osi
           .style("font-family", "NexusSansCompPro, Arial, Helvetica, sans-serif")
           .style("stroke-opacity", "0.75")
	       .style("stroke-width", "0.5px")   // debljina x-mreze
           .attr("transform", "translate(0," + height + ")")
           .call(xAxis)
       .append("text")
            .attr("transform", "translate(300,30)")
            .attr("x", 6)
            .attr("dx", ".71em")
            .style("text-anchor", "end")
            .style("font-size", "15px")     // font za labelu na x-osi
            .style("fill", "grey")
            .style("font-weight", "100")
            .style("stroke-opacity", "0.75")
	        .style("stroke-width", "0.1px")
            .text(xLabel);
    svg.append("g")
            .attr("class", "y axis")
             .style("stroke", "grey")
             .style("font-size", "15px")    // font za vrednosti na y-osi
             .style("font-family", "NexusSansCompPro, Arial, Helvetica, sans-serif")
             //.style("stroke-opacity", "0.75")
	         .style("stroke-width", "0.5px")    // debljina y-mreze
             .call(yAxis)
        .append("text")
            //.attr("transform", "rotate(-90)")
            .attr("transform", rotateTranslateY)
            .attr("y", 6)
            .attr("dy", ".7em")
            .style("text-anchor", "end")
            .style("font-size", "15px")     // font za labelu na y-osi
            .style("fill", "grey")
            .style("font-weight", "100")
            .style("stroke-opacity", "0.75")
	        .style("stroke-width", "0.1px")
            .text(yLabel);


    // Series
    var series = svg.selectAll(".series")
            .data(seriesData)
          .enter().append("g")
            .attr("class", "series");

    series.append("path")
         .attr("class", "line")
         .attr("d", function (d) { /*console.log("path d:", d);*/ return line(d.values); })
         .style("stroke", function (d) { return color(d.name); })
         //.style("stroke-dasharray", ("10, 3"))  // for dashed line!!
        .style("stroke-dasharray", function (d) { return (d.name.indexOf("st.dev") > -1 ? "10, 3" : "10, 0"); })  // for conditional dashed line!!
         .style("stroke-width", "2px")
         .style("fill", "none");


    // Tooltip and data
    if (seriesData.length > 1) {
        series.selectAll(".line")
             .on("click", function (d) { $("#curveName" + type).html(d.name); showData.call(this, d.values); })
             .on("mouseover", function (d) { document.body.style.cursor = "pointer"; })
             .on("mouseout", function (d) { document.body.style.cursor = "default"; })
             .append("svg:title")
                .text(function (d) { return 'Click to view table points for: ' + d.name; });
    } else {
        series.selectAll(".line")
             .on("click", function (d) { return false; })
             .on("mouseover", function (d) { document.body.style.cursor = "pointer"; })
             .on("mouseout", function (d) { document.body.style.cursor = "default"; })
             .append("svg:title")
                .text(function (d) { return d.name; });
    }

    function showData(d) {
        //console.log("data for table" + type + ": ", d);

        var html = "";
        for (i = 0; i < d.length; ++i) {
            html += "<tr>";
            html += "<td>" + '1E' + d[i].label + "</td>";
            if (d[i].value >= 0.00001) {
                html += "<td>" + (parseInt(d[i].value * 100000) / 100000) + "</td>";
            } else {
                html += "<td>" + d[i].value.toExponential() + "</td>";
            };
            html += "</tr>";
        }
        $("#tableData" + type + " > tbody").html(html);
    }

    // Legend
    var legend = svg.selectAll(".legend")
          .data(varNames.slice().reverse())
        .enter().append("g")
          .attr("class", "legend")
          .attr("transform", function (d, i) { return "translate(" + legendWidth.toString() + "," + i * 20 + ")"; });

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
        .text(function (d) { return d /*+ '°C'*/; });
    //.on("click", function (d) { console.log("legend d:", d); $("#curveName" + type).html(d.name); showData.call(this, d.values); });  // under construction :)


    //d3.selectAll(".legend")
    //      .on("click", function (d) { $("#curveName" + type).html(d.name); showData.call(this, d.values); })
    //      .append("svg:title")
    //         .text(function (d) { return 'Click to view table points for: ' + d.name; });


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

        var doctype = '<?xml version="1.0" encoding="utf-8" standalone="no"?><!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">';
        //var doctype = '<?xml version="1.0" encoding="utf-8" standalone="no"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">';
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

            a.addEventListener("click", function () {
                a.parentNode.removeChild(a);
            });
            a.click();
        };

    });
    // /Attach action for Save image


}