;
var mechanicalPlot = {};

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

// model for all  mechanical properties
var diagramData = null;
var tableDataId = '';

// plot data
var seriesData = new Array();
var legendData = new Array();


//
// http://www.delimited.io/blog/2014/3/3/creating-multi-series-charts-in-d3-lines-bars-area-and-streamgraphs
//
mechanicalPlot.InitData = function (modelValue) {
    diagramData = $.parseJSON(modelValue).Properties;
    //console.log('Diagram Data', diagramData)
};

mechanicalPlot.InitDataInitial = function (model, productGroupId) {
    //console.log('productGroupId: ', productGroupId);

    var modelData = $.parseJSON(model);
    //console.log('modelData: ', modelData);

    for (i = 0; i < modelData.length; ++i) {
        if (modelData[i].ProductGroupId.toString() == productGroupId.toString()) {
            diagramData = modelData[i].Conditions[0].Properties;
        }
    }
    //console.log('Diagram Data', diagramData)
};

mechanicalPlot.InitSeriesData = function (propertyId) {
    // propertyId - selected property

    var tmpData = new Array();
    for (i = 0; i < diagramData.length; ++i) {
        if (diagramData[i].PropertyId == propertyId) {
            if (diagramData[i].Temperature != null && diagramData[i].OrigValue != null) {
                tmpData.push(
                    {
                        origUnit: diagramData[i].OrigUnit
                        , origValue: diagramData[i].OrigValue
                        , origValueText: diagramData[i].OrigValueText
                        , propertyId: diagramData[i].PropertyId
                        , propertyName: diagramData[i].PropertyName
                        , sourcePropertyId: diagramData[i].SourcePropertyId
                        , temperature: diagramData[i].Temperature
                    });
            };
        }
    }
    //console.log('Temp Data', tmpData)

    seriesData = new Array();
    var oneSerieName = '';
    var chartValues = new Array();
    for (i = 0; i < tmpData.length; ++i) {
        var unit = (tmpData[i].origUnit == null && tmpData[i].origUnit.toString() == '' ? "" : ' (' + tmpData[i].origUnit.toString() + ')');
        oneSerieName = (oneSerieName == '' ? tmpData[i].propertyName.toString() + unit : oneSerieName);
        chartValues.push(
              {
                  name: oneSerieName
                  , label: tmpData[i].temperature.toString()
                  , value: tmpData[i].origValue.replace("≥", "").replace("&LessEqual;", "").replace("&GreaterEqual;", "").replace("≤", "").trim()
              });
    };
    seriesData.push(
            {
                name: oneSerieName
                , values: chartValues
            });

    $("#diagMechanicalPropertyName").html(oneSerieName);
    $("#diagMechanicalPropertyHeading").html(oneSerieName);

    //console.log('Series Data Init', seriesData);
}
var selectedMechSourcePropertyIdDiagram = null;
var selectedMechPropertyIdDiagram = null;

mechanicalPlot.PlotChart = function (propertyId, tableId, sourcePropertyId) {

    selectedMechSourcePropertyIdDiagram = sourcePropertyId;
    selectedMechPropertyIdDiagram = propertyId;

    tableDataId = tableId;
    mechanicalPlot.InitSeriesData(parseInt(propertyId));
    mechanicalPlot.ShowTableData();
    $("#" + tableId).hide();
    $("#diagMechanicalContainer").show();

    if (typeof seriesData === "undefined") {
        return;
    }


    ///// Plot Chart
    //
    var varNames = [seriesData[0].name];
    var xLabel = "temperature (°C)";
    var yLabel = seriesData[0].name.replace("<sup>", "").replace("</sup>", "").replace("<sub>", "").replace("</sub>", "");
    var legendWidth = 70;
    var svgContainer = "#diagMechanicalChart";

    var saveContainer = "#saveMechanical";

    var isIE = false || !!document.documentMode; // At least IE6
    if (isIE == true) {
        $(saveContainer).hide();
    } else {
        $(saveContainer).show();
    };

    // reset SVG chart container
    $(svgContainer).html('');

    // Init variables
    var margin = { top: 20, right: legendWidth, bottom: 35, left: 60 };
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
        //.interpolate("cardinal")
        .interpolate("basis")
        //.x(function (d) { return x(d.label) + x.rangeBand() / 2; })
        .x(function (d) { return x(+d.label); })
        .y(function (d) { return y(+d.value); });

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

    x.domain([
     d3.min(seriesData, function (c) {
         var toReturn = d3.min(c.values, function (d) { return +d.label; });
         return toReturn > 0 ? 0 : toReturn;
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
          return d3.max(c.values, function (d) { return d.value * 1.2; });
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
        .tickFormat(function (d, i) {
            var toReturn = d;
            return (toReturn);
        })

    var rotateTranslateY = d3.svg.transform().rotate(-90).translate(0, -60);
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
            .attr("transform", "translate(255,30)")
            .attr("x", 6)
            .attr("dx", ".71em")
            .style("text-anchor", "end")
            .style("font-size", "15px")		// font za labelu na x-osi
            .style("fill", "grey")
            .style("font-weight", "100")
            .style("stroke-opacity", "0.75")
	        .style("stroke-width", "0.1px")
            .text(xLabel);
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
         .style("stroke-width", "2px")
         .style("fill", "none");

    //
    // /Plot Chart

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

    //
    // /Attach action for Save image
};

mechanicalPlot.ShowData = function () {
    $("#diagMechanicalContainer").hide();
    $("#" + tableDataId).show();
};

mechanicalPlot.ShowTableData = function () {
    var html = "";
    for (i = 0; i < seriesData[0].values.length; ++i) {
        html += "<tr>";
        html += "<td>" + seriesData[0].values[i].label + "</td>";
        html += "<td>" + seriesData[0].values[i].value + "</td>";
        html += "</tr>";
    }
    $("#diagMechanicalTable > tbody").html(html);
};
