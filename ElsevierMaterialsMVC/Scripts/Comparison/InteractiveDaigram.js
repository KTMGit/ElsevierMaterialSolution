;

    // Define Array prototipes for unique and contains functionality
if (typeof Array.prototype.contains !== 'function') {
    Array.prototype.contains = function (v) {
        for (var i = 0; i < this.length; i++) {
            if (this[i]=== v) return true;
            }
        return false;
        };
        };

    if (typeof Array.prototype.unique !== 'function') {
            Array.prototype.unique = function () {
                var arr =[];
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

var stressStrainPlot = {};
var fatiguePlot = { };

var legendData = new Array();
var seriesData = new Array();
var xAxisName = null;
var yAxisName = null;
var xUnit = null;
var yUnit = null;

var seriesDataTrue = new Array();       // Array of stressStrainPlot.ChartValues
var legendDataTrue = new Array();       // Array of series (temperatures)

var seriesDataStrain = new Array();
var legendDataStrain = new Array();
var seriesDataStress = new Array();
var legendDataStress = new Array();
var fatigueType;

var varNames, seriesData, xLabel, yLabel, legendWidth;

stressStrainPlot.DiagramData = function () {
    this.LegendDesc = null;
    this.LegendDescText = null;
    this.RT = null;
    this.Temperature = null;
    this.TemperatureText = null;
    this.TrueDetails = null;
};




//
// http://www.delimited.io/blog/2014/3/3/creating-multi-series-charts-in-d3-lines-bars-area-and-streamgraphs
//
stressStrainPlot.InitData = function (modelValue, sourceTypeId) {
    //.PointsForDiagram.Curves[i; i=0,..,n-1].PointsForDiagram[j; j=0,..,n-1].X
    //                                                                       .Y
    //                                       .Temperature

    //if (diagramData === null && typeof diagramData === "object") {
    diagramData = $.parseJSON(modelValue);
    //};
    xAxisName = diagramData.XName;
    yAxisName = diagramData.YName;
    xUnit = diagramData.XUnit;
    yUnit = diagramData.YUnit;
    // Prepare data for the True chart
    var trueData = diagramData.Points;
    var trueDataCurves = diagramData.PointsForDiagram;

    var oneSerieName = diagramData.Id;
        var oneSeriePoints = diagramData.PointsForDiagram;

        var chartValuesValues = new Array();
        if (sourceTypeId == -2 ||sourceTypeId == -3 || sourceTypeId == -4 ||sourceTypeId == -5) {
            for (j = 0; j < oneSeriePoints.length; ++j) {
                chartValuesValues.push(
                    {
                        name: oneSerieName
                        , label: '1E' + oneSeriePoints[j].X.toString()
                        , value: oneSeriePoints[j].Y
                    });
            }
        } else {
            for (j = 0; j < oneSeriePoints.length; ++j) {
                chartValuesValues.push(
                    {
                        name: oneSerieName
                        , label: oneSeriePoints[j].X.toString()
                        , value: oneSeriePoints[j].Y
                    });
            }
        }
      
        //
        //seriesData.push(chartValues);
        seriesData.push(
            {
                name: oneSerieName
                , values: chartValuesValues
            });

       
        seriesDataTrue = seriesData;

        var oneLegendName = diagramData.Id;
            legendData.push(oneLegendName);
        
            legendDataTrue =  legendData.sort(function (a, b) {
            return parseFloat(b) - parseFloat(a);
        });



}

stressStrainPlot.PlotChartAll = function () {
    stressStrainPlot.PlotChart();
}

stressStrainPlot.PlotChart = function () {

    var varNames;
    var seriesData;
    var xValues = new Array();

    var svgContainer;
    var saveContainer;

  
    svgContainer = "#trueChart";
    saveContainer = "#saveTrue";
    varNames = legendDataTrue;
    seriesData = seriesDataTrue;
   
   

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
     //d3.min(seriesData, function (c) {
     //    return d3.min(c.values, function (d) { return +d.label; });
     //}),
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
           return 0;
      }),
      //d3.max(seriesData, function (c) {
      //    return d3.max(c.values, function (d) { return d.value; });
      //})
       d3.max(seriesData, function (c) {
          return d3.max(c.values, function (d) { return d.value * 1.2;  });  })
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
                  .tickSize(- height, 0, 0)
                         .tickFormat(function (d, i) { return d; })
    //.tickFormat("");

    var xAxisWithUnit = xAxisName;
    var yAxisWithUnit = yAxisName;

    if (xUnit != null && xUnit != '') {
         xAxisWithUnit = xAxisName + ' (' + xUnit + ')';
    }
    if (yUnit != null && yUnit != '') {
        var yAxisWithUnit = yAxisName + ' (' + yUnit + ')';
    }

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .ticks(5)
        .tickSize(-width, 0, 0)

    svg.append("g")
           .attr("class", "x axis")
           //.style("stroke", "red")    // font na x osi
           //.style("stroke-opacity", "0.75")
	       //.style("stroke-width", "0.1px")
           .attr("transform", "translate(0," + height + ")")
           .call(xAxis)
        .append("text")
            .attr("transform", "translate(225,30)")
            .attr("x", 6)
            .attr("dx", ".71em")
            .style("text-anchor", "end")
            .text(xAxisWithUnit);
    svg.append("g")
            .attr("class", "y axis")
            .call(yAxis)
      .append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", -50)
        .attr("dy", ".7em")
        .style("text-anchor", "end")
        .text(yAxisWithUnit);


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
            .on("mouseover", function (d) { document.body.style.cursor = "pointer";  })
            .on("mouseout", function (d) { document.body.style.cursor = "default"; })
             .append("svg:title")
                .text(function (d) { return 'Click to view table points for curve'; });
    } else {
        series.selectAll(".line")
             .on("click", function(d) { return false; })
               .on("mouseover", function (d) { document.body.style.cursor = "pointer"; })
            .on("mouseout", function (d) {
document.body.style.cursor = "default"; })
             .append("svg:title")
                .text(function (d) { return d.name; });
    }

    function showData(d) {

        ShowPointsForCurve(d.name);
        $("#curvesSelection").val(d.name);
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
        .text(function (d) { return d; });



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
        //var html = d3.select(svgContainer + " svg")
        //     .attr("version", 1.1)
        //     .attr("xmlns", "http://www.w3.org/2000/svg")
        //     .node().parentNode.innerHTML;

        //var doctype = '<?xml version="1.0" standalone="no"?><!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">';
        //html = doctype + html;

        //console.log("img html: ", html);
        //image.src = 'data:image/svg+xml;base64,' + btoa(html);      // ne radi, a tako bi trebalo da bude ??????
        //image.src = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(html)));

        // End of Version a), with SVG diagram


        // Version b), with already generated <img> on the page
        image.src = $(imgContainer).attr('src');
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


fatiguePlot.InitData = function (modelValue, type) {
    diagramData = $.parseJSON(modelValue);
    fatigueType = type;
    //
    if (fatigueType == -5) {
          seriesDataStress = fatiguePlot.InitSeriesData(diagramData);
         legendDataStress = fatiguePlot.InitLegendData(diagramData);
    } else {
         seriesDataStrain = fatiguePlot.InitSeriesData(diagramData);
        legendDataStrain = fatiguePlot.InitLegendData(diagramData);

      
    }

}

fatiguePlot.InitSeriesData = function (dataCurves) {
            var oneSerieName = dataCurves.Id.toString();
        var oneSeriePoints = dataCurves.PointsForDiagram;

        var chartValues = new Array();

        for (j = 0; j < oneSeriePoints.length; ++j) {
            if(fatigueType == -5) {
                  chartValues.push(
            {
                    name: oneSerieName
                   , label: oneSeriePoints[j].X.toString()
                   , value: parseInt(Math.pow(10, oneSeriePoints[j].Y))
               });
              
            } else {
                chartValues.push(
                {
                    name: oneSerieName
                   , label: oneSeriePoints[j].X.toString()
                   , value: parseFloat(Math.pow(10, oneSeriePoints[j].Y))
                   });
            };

        };

        seriesData.push(
            {
                  name: oneSerieName
                , values: chartValues
            });
    
    return seriesData;
}

fatiguePlot.InitLegendData = function (dataCurves) {
   var oneLegendName = dataCurves.Id.toString();
        legendData.push(oneLegendName);
   
        legendData =  legendData.sort(function (a, b) {
        return (a - b);
        });

        return legendData;
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

    if (type == -5) {
        varNames = legendDataStress;
        seriesData = seriesDataStress;
        xLabel = "Cycle to failure, Nf (log scale)";
        yLabel = "Stress Amplitude (log scale) [MPa]";
        legendWidth = 90;
       
    } else {
        varNames = legendDataStrain;
        seriesData = seriesDataStrain;
        xLabel = "Reversals to failure, 2Nf (log scale)";
        yLabel = "Strain Amplitude (log scale)";
        legendWidth = 70;
    };

    if (typeof seriesData === "undefined") {
        return;
    }

    //console.log("seriesData" + type + ":", seriesData);
    //console.log("legendData", varNames);

    // Show actual curve-name
    //$("#curveName" + type).html("Points for: " + varNames.reverse()[0]);

    var svgContainer = "#trueChart";
    var saveContainer = "#saveFatigue" + type;
    var imgContainer = "#imgFatigue" + type;

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

    var rotateTranslateY = d3.svg.transform().rotate(-90).translate(-100, -60);
    svg.append("g")
           .attr("class", "x axis")
           //.style("stroke", "red")    // font na x osi
           //.style("stroke-opacity", "0.75")
	       //.style("stroke-width", "0.1px")
           .attr("transform", "translate(0," + height + ")")
           .call(xAxis)
       .append("text")
            .attr("transform", "translate(300,30)")
            .attr("x", 6)
            .attr("dx", ".71em")
            .style("text-anchor", "end")
            .text(xLabel);
    svg.append("g")
            .attr("class", "y axis")
            .call(yAxis)
        .append("text")
            //.attr("transform", "rotate(-90)")
            .attr("transform", rotateTranslateY)
            .attr("y", 6)
            .attr("dy", ".7em")
            .style("text-anchor", "end")
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
        ShowPointsForCurve(d[0].name);
        $("#curvesSelection").val(d[0].name);
    }

    // Legend
    var legend = svg.selectAll(".legend")
          .data(varNames.slice())
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
        //     .attr("version", 1.1)
        //     .attr("xmlns", "http://www.w3.org/2000/svg")
        //     .node().parentNode.innerHTML;

        //var doctype = '<?xml version="1.0" encoding="utf-8" standalone="no"?><!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">';
        ////var doctype = '<?xml version="1.0" encoding="utf-8" standalone="no"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">';
        //html = doctype + html;

        //console.log("img html: ", html);
        ////image.src = 'data:image/svg+xml;base64,' + btoa(html);      // ne radi, a tako bi trebalo da bude ??????
        //image.src = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(html)));
        // End of Version a), with SVG diagram


        // Version b), with already generated <img> on the page
        image.src = $(imgContainer).attr('src');
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

//fatiguePlot.PlotChart = function (type) {
//    /*
//    1 - Strain
//    2 - Stress
//    */
//    var varNames, seriesData, xLabel, yLabel, legendWidth;

//    if (type == -5) {

//        varNames = legendDataStress;
//        seriesData = seriesDataStress;
//        xLabel = "Cycle to failure, Nf (log scale)";
//        yLabel = "Stress Amplitude (log scale)";
//        legendWidth = 90;
//    } else {
//        varNames = legendDataStrain;
//        seriesData = seriesDataStrain;
//        xLabel = "Reversals to failure, 2Nf (log scale)";
//        yLabel = "Strain Amplitude (log scale)";
//        legendWidth = 70;
//    };

//    if (typeof seriesData === "undefined") {
//        return;
//    }

//    //console.log("seriesData" + type + ":", seriesData);
//    //console.log("legendData", varNames);

//    // Show actual curve-name
//    //$("#curveName" + type).html("Points for: " + varNames.reverse()[0]);

//    //var svgContainer = "#fatigueChart" + type;
//    //var saveContainer = "#saveFatigue" + type;
//    //var imgContainer = "#imgFatigue" + type;
//        var svgContainer = "#trueChart";
//    var saveContainer = "#saveFatigue";
//    var imgContainer = "#imgFatigue";

//    var xValues = new Array();
//    var yValues = new Array();

//    // reset SVG chart container
//    $(svgContainer).html('');

//    // Fill x-labels
//    var scale = 1;
//    for (i = 0; i < seriesData.length; ++i) {
//        var vals = seriesData[i].values;
//        for (j = 0; j < vals.length; ++j) {
//            xValues.push('1E' + vals[j].label);
//        };
//    };
//    xValues = xValues.unique().sort();
//    //console.log("xValues" + type + ":", xValues);

//    scale = 1;

//    // Init variables
//    var margin = { top: 20, right: legendWidth, bottom: 35, left: 60 };
//    var width = 550 - margin.left - margin.right;
//    var height = 400 - margin.top - margin.bottom;

//    //var x = d3.scale.ordinal()
//    //    .rangeRoundBands([0, width], .1);
//    var x = d3.scale.linear()
//      .rangeRound([0, width]);

//    var y = d3.scale.log()
//        .rangeRound([height, 0]);


//    // See https://github.com/mbostock/d3/wiki/SVG-Shapes for different interpolates
//    var line = d3.svg.line()
//        .interpolate("cardinal")
//        //.x(function (d) { return x(d.label) + x.rangeBand() / 2; })
//        .x(function (d) { return x(+d.label / scale); })
//        .y(function (d) { return y(d.value); });

//    //var color = d3.scale.ordinal()
//    //    .range(["#001c9c", "#101b4d", "#475003", "#9c8305", "#d3c47c"]);
//    var color = d3.scale.category10();

//    var svg = d3.select(svgContainer).append("svg")
//        .attr("width", width + margin.left + margin.right)
//        .attr("height", height + margin.top + margin.bottom)
//      .append("g")
//        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


//    // Init domains
//    color.domain(varNames);
//    //x.domain(xValues.map(function (d) {
//    //    return d.toString();
//    //}));
//    x.domain([
//     d3.min(seriesData, function (c) {
//         //console.log("xMin" + type + ":", d3.min(c.values, function (d) { return +d.label; }));
//         return d3.min(c.values, function (d) { return +d.label; });
//     }),
//     d3.max(seriesData, function (c) {
//         //console.log("xMax" + type + ":", d3.max(c.values, function (d) { return +d.label; }));
//         return d3.max(c.values, function (d) { return +d.label; });
//     })
//    ]);
//    y.domain([
//      d3.min(seriesData, function (c) {
//          return d3.min(c.values, function (d) { return d.value; });
//      }),
//      d3.max(seriesData, function (c) {
//          return d3.max(c.values, function (d) { return d.value; });
//      })
//    ]);


//    // Axes
//    //var xAxis = d3.svg.axis()
//    //  .scale(x)
//    //  .orient("bottom")
//    //  .ticks(5);
//    var xAxis = d3.svg.axis()
//        .scale(x)
//        .orient("bottom")
//        .ticks(xValues.length - 1)
//        .tickSize(-height, 0, 0)
//        .tickFormat(function (d, i) { return xValues[i]; })

//    var yAxis = d3.svg.axis()
//        .scale(y)
//        .orient("left")
//        .ticks(5)
//        .tickSize(-width, 0, 0)
//        .tickFormat(function (d, i) {
//            var toReturn = (parseInt(d * 100000) / 100000);
//            if (type == -5) {
             
//            } else {
//                   toReturn = toReturn.toString().substr(toReturn.toString().length -1) == "1" ? toReturn: "";
//              };
//            return (toReturn);
//            })
//        //.tickFormat(function (d, i) {
//        //    var toReturn = (parseInt(d * 100000) / 100000);
//        //    return (toReturn);
//        //})



//    svg.append("g")
//           .attr("class", "x axis")
//           //.style("stroke", "red")    // font na x osi
//           //.style("stroke-opacity", "0.75")
//	       //.style("stroke-width", "0.1px")
//           .attr("transform", "translate(0," + height + ")")
//           .call(xAxis)
//       .append("text")
//            .attr("transform", "translate(300,30)")
//            .attr("x", 6)
//            .attr("dx", ".71em")
//            .style("text-anchor", "end")
//            .text(xLabel);
//    svg.append("g")
//            .attr("class", "y axis")
//            .call(yAxis)
//        .append("text")
//            .attr("transform", "rotate(-90)")
//            .attr("y",-50)
//            .attr("dy", ".7em")
//            .style("text-anchor", "end")
//            .text(yLabel);

//    // Series
//    var series = svg.selectAll(".series")
//            .data(seriesData)
//          .enter().append("g")
//            .attr("class", "series");

//    series.append("path")
//         .attr("class", "line")
//         .attr("d", function (d) { /*console.log("path d:", d);*/ return line(d.values); })
//         .style("stroke", function (d) { return color(d.name); })
//         //.style("stroke-dasharray", ("10, 3"))  // for dashed line!!
//        .style("stroke-dasharray", function (d) { return (d.name.indexOf("st.dev") > -1 ? "10, 3" : "10, 0"); })  // for conditional dashed line!!
//         .style("stroke-width", "2px")
//         .style("fill", "none");


//    // Tooltip and data
//    if (seriesData.length > 1) {
//        series.selectAll(".line")
//             .on("click", function (d) { $("#curveName" + type).html(d.name); showData.call(this, d.values); })
//             .append("svg:title").on("mouseover", function (d) { document.body.style.cursor = "pointer"; })
//            .on("mouseout", function (d) { document.body.style.cursor = "default"; }).text(function (d) { return 'Click to view table points for: ' + d.name; });
//    } else {
//        series.selectAll(".line")
//             .on("click", function (d) { return false; })
//             .append("svg:title").on("mouseover", function (d) { document.body.style.cursor = "pointer"; })
//         .on("mouseout", function (d) { document.body.style.cursor = "default"; })
//                .text(function (d) { return d.name; });
//    }

//    function showData(d) {
//        //console.log("data for table" + type + ": ", d);
                
//             ShowPointsForCurve(d[0].name);
//             $("#curvesSelection").val(d[0].name);
//        //var html = "";
//        //for (i = 0; i < d.length; ++i) {
//        //    html += "<tr>";
//        //    html += "<td>" + '1E' + d[i].label + "</td>";
//        //    html += "<td>" + parseInt(d[i].value * 100000) / 100000 + "</td>";
//        //    html += "</tr>";
//        //}

//        //$("#tableData" + type + " > tbody").html(html);
//    }

//    // Legend
//    var legend = svg.selectAll(".legend")
//          .data(varNames)
//        .enter().append("g")
//          .attr("class", "legend")
//          .attr("transform", function (d, i) { return "translate(" + legendWidth.toString() + "," + i * 20 + ")"; });

//    legend.append("rect")
//           .attr("x", width - 10)
//           .attr("width", 10)
//           .attr("height", 3)
//           .style("fill", color)
//           .style("stroke", "grey")
//           .attr("transform", "translate(0,4)");

//    legend.append("text")
//        .attr("x", width - 12)
//        .attr("y", 6)
//        .attr("dy", ".35em")
//        .style("text-anchor", "end")
//        .text(function (d) { return d /*+ '°C'*/; });
//    //.on("click", function (d) { console.log("legend d:", d); $("#curveName" + type).html(d.name); showData.call(this, d.values); });  // under construction :)


//    //d3.selectAll(".legend")
//    //      .on("click", function (d) { $("#curveName" + type).html(d.name); showData.call(this, d.values); })
//    //      .append("svg:title")
//    //         .text(function (d) { return 'Click to view table points for: ' + d.name; });


//    // Attach action for Save image
//    //
//    // http://techslides.com/save-svg-as-an-image
//    // http://spin.atomicobject.com/2014/01/21/convert-svg-to-png/
//    //


//    $('head').append("<meta name='charset' content='utf-8'>");

//    d3.select(saveContainer).on("click", function () {

//        // create  image and attach src on it
//        var image = new Image();

//        // Version a), with SVG diagram
//        //var html = d3.select(svgContainer + " svg")
//        //     .attr("version", 1.1)
//        //     .attr("xmlns", "http://www.w3.org/2000/svg")
//        //     .node().parentNode.innerHTML;

//        //var doctype = '<?xml version="1.0" standalone="no"?><!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">';
//        //html = doctype + html;

//        //console.log("img html: ", html);
//        //image.src = 'data:image/svg+xml;base64,' + btoa(html);      // ne radi, a tako bi trebalo da bude ??????
//        //image.src = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(html)));

//        // End of Version a), with SVG diagram


//        // Version b), with already generated <img> on the page
//        image.src = $(imgContainer).attr('src');
//        // End of Version b), with already generated <img> on the page

//        image.onload = function () {
//            // create canvas fom image
//            var canvas = document.createElement('canvas');
//            canvas.width = image.width;
//            canvas.height = image.height;
//            var context = canvas.getContext('2d');
//            context.drawImage(image, 0, 0);

//            // create a element for download action
//            var a = document.createElement('a');
//            a.download = "image.png";
//            a.href = canvas.toDataURL('image/png');
//            document.body.appendChild(a);

//            a.addEventListener("click", function (e) {
//                a.parentNode.removeChild(a);
//            });
//            a.click();
//        };

//    });
//    // /Attach action for Save image


//}

var interactiveDiagram = {};
interactiveDiagram.general = { };
interactiveDiagram.general.series = new Array();
interactiveDiagram.general.legendData = new Array();
interactiveDiagram.general.diagramData = new Array();

interactiveDiagram.general.xAxisName = '';
interactiveDiagram.general.yAxisName = '';
interactiveDiagram.general.xUnit = '';
interactiveDiagram.general.yUnit = '';

interactiveDiagram.general.xAxisWithUnit = '';
interactiveDiagram.general.yAxisWithUnit = '';
interactiveDiagram.general.legendWidth = 70;
interactiveDiagram.general.divContainer = "#trueChart";

interactiveDiagram.general.resetObjectData = function () {
   
    interactiveDiagram.general.series = new Array();
    interactiveDiagram.general.legendData = new Array();
    interactiveDiagram.general.diagramData = new Array();

    interactiveDiagram.general.xAxisName = '';
    interactiveDiagram.general.yAxisName = '';
    interactiveDiagram.general.xUnit = '';
    interactiveDiagram.general.yUnit = '';

    interactiveDiagram.general.xAxisWithUnit = '';
    interactiveDiagram.general.yAxisWithUnit = '';
    interactiveDiagram.general.divContainer = "#trueChart";
    interactiveDiagram.general.legendWidth = 70;
}

interactiveDiagram.general.InitData = function (model) {

    interactiveDiagram.general.diagramData = $.parseJSON(model);

    interactiveDiagram.general.xAxisName = interactiveDiagram.general.diagramData.XName;
    interactiveDiagram.general.yAxisName = interactiveDiagram.general.diagramData.YName;
    interactiveDiagram.general.xUnit = interactiveDiagram.general.diagramData.XUnit;
    interactiveDiagram.general.yUnit = interactiveDiagram.general.diagramData.YUnit;

    if (interactiveDiagram.general.xUnit != null && interactiveDiagram.general.xUnit != '') {      
            interactiveDiagram.general.xAxisWithUnit = interactiveDiagram.general.xAxisName + ' (' + interactiveDiagram.general.xUnit + ')';      
    }
    else {
        interactiveDiagram.general.xAxisWithUnit = interactiveDiagram.general.xAxisName;
    }

    if (interactiveDiagram.general.yUnit != null && interactiveDiagram.general.yUnit != '') {
        interactiveDiagram.general.yUnit = interactiveDiagram.general.diagramData.YUnit.replace("<sup>", "").replace("</sup>", "").replace("<sub>", "").replace("</sub>", "");      
            interactiveDiagram.general.yAxisWithUnit = interactiveDiagram.general.yAxisName + ' (' + interactiveDiagram.general.yUnit + ')';  
    }
    else {
        interactiveDiagram.general.yAxisWithUnit = interactiveDiagram.general.yAxisName;
    }
    
    var oneSeriePoints = interactiveDiagram.general.diagramData.PointsForDiagram;

    var chartValuesValues = new Array();
    for (j = 0; j < oneSeriePoints.length; ++j) {
        chartValuesValues.push(
         {
             name: interactiveDiagram.general.diagramData.Id
                , label: oneSeriePoints[j].X
                , value: oneSeriePoints[j].Y
         });
    }

    interactiveDiagram.general.series.push(
    {
        name: interactiveDiagram.general.diagramData.CounterId
          , values: chartValuesValues
    });  


    interactiveDiagram.general.legendData.push(interactiveDiagram.general.diagramData.CounterId);
    interactiveDiagram.general.legendData = interactiveDiagram.general.legendData.sort(function (a, b) {
        return parseFloat(b) - parseFloat(a);
    });

};

interactiveDiagram.general.PlotChart = function () {

    if (typeof interactiveDiagram.general.series === "undefined") {
        return;
    }

    $(interactiveDiagram.general.divContainer).html('');

    var margin = { top: 20, right: interactiveDiagram.general.legendWidth, bottom: 35, left: 60 };
    var width = 550 - margin.left - margin.right;
    var height = 400 - margin.top - margin.bottom;

    var x = d3.scale.linear().rangeRound([0, width]);
    var y = d3.scale.linear().rangeRound([height, 0]);


    var line = d3.svg.line().interpolate("basis").x(function (d) { return x(+d.label); }).y(function (d) { return y( +d.value); });

    var color = d3.scale.category10();    
    var svg = d3.select(interactiveDiagram.general.divContainer).append("svg").attr("width", width + margin.left + margin.right).attr("height", height + margin.top + margin.bottom).append("g").attr("transform", "translate(" + margin.left + "," + margin.top + ")");


    color.domain(interactiveDiagram.general.legendData);  
   

    x.domain([
        //d3.min(interactiveDiagram.general.series, function (c) { var toReturn = d3.min(c.values, function (d) { return +d.label; }); return toReturn > 0 ? 0 : toReturn; }),
     d3.min(interactiveDiagram.general.series, function (c) { return d3.min(c.values, function (d) { return +d.label; }); }),
     d3.max(interactiveDiagram.general.series, function (c) { return d3.max(c.values, function (d) { return +d.label; }); })
    ]);

    y.domain([
      d3.min(interactiveDiagram.general.series, function (c) { return d3.min(c.values, function (d) { return d.value * 0.8; }); }),
      d3.max(interactiveDiagram.general.series, function (c) { return d3.max(c.values, function (d) { return d.value * 1.2; }); })
    ]);


    var xAxis = d3.svg.axis().scale(x).orient("bottom").ticks(5).tickSize(-height, 0, 0).tickFormat(function (d, i) { return d; })
    var yAxis = d3.svg.axis().scale(y).orient("left").ticks(5).tickSize(-width, 0, 0).tickFormat(function (d, i) { var toReturn = d; return (toReturn); })


    var legend = svg.selectAll(".legend").data(interactiveDiagram.general.legendData.slice().reverse()).enter().append("g").attr("class", "legend").attr("transform", function (d, i) { return "translate(" + interactiveDiagram.general.legendWidth + "," + i * 20 + ")"; });
    legend.append("rect").attr("x", width - 10).attr("width", 10).attr("height", 3).style("fill", color).style("stroke", "grey").attr("transform", "translate(0,4)");
    legend.append("text").attr("x", width - 12).attr("y", 6).attr("dy", ".35em").style("text-anchor", "end").text(function (d) { return d /*+ '°C'*/; });

    svg.append("g").attr("class", "x axis").attr("transform", "translate(0," + height + ")").call(xAxis).append("text").attr("transform", "translate(255,30)").attr("x", 6).attr("dx", ".71em").style("text-anchor", "end").text(interactiveDiagram.general.xAxisWithUnit);
    svg.append("g").attr("class", "y axis").call(yAxis).append("text").attr("transform", "rotate(-90)").style("text-anchor", "end").attr("y", -50).attr("dy", ".7em").text(interactiveDiagram.general.yAxisWithUnit);
    //;



    function showData(d) {

        ShowPointsForCurve(d.name);
        $("#curvesSelection").val(d.name);
    }
    
    var series = svg.selectAll(".series").data(interactiveDiagram.general.series).enter().append("g").attr("class", "series");
    series.append("path").attr("class", "line").attr("d", function (d) { return line(d.values); }).style("stroke", function (d) { return color(d.name); }).style("stroke-width", "2px").style("fill", "none");

    if (interactiveDiagram.general.series.length > 1) {
        series.selectAll(".line").on("click", function (d) { showData.call(this, d); }).on("mouseover", function (d) { document.body.style.cursor = "pointer"; }).on("mouseout", function (d) { document.body.style.cursor = "default"; }).append("svg:title").text(function (d) { return 'Click to view table points for curve '; });
    } else {
        series.selectAll(".line").on("click", function (d) { return false; }).on("mouseover", function (d) { document.body.style.cursor = "pointer"; }).on("mouseout", function (d) { document.body.style.cursor = "default"; }).append("svg:title").text(function (d) { return d.name; });
    }

};
