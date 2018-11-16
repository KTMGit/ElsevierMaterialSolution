//var interactiveDiagram = {};
//interactiveDiagram.general = {};
//interactiveDiagram.general.series = new Array();
//interactiveDiagram.general.legendData = new Array();
//interactiveDiagram.general.diagramData = new Array();

//interactiveDiagram.general.xAxisName = '';
//interactiveDiagram.general.yAxisName = '';
//interactiveDiagram.general.xUnit = '';
//interactiveDiagram.general.yUnit = '';

//interactiveDiagram.general.xAxisWithUnit = '';
//interactiveDiagram.general.yAxisWithUnit = '';
//interactiveDiagram.general.legendWidth = 70;
//interactiveDiagram.general.divContainer = "#trueChart";

//interactiveDiagram.general.resetObjectData = function () {
   
//    interactiveDiagram.general.series = new Array();
//    interactiveDiagram.general.legendData = new Array();
//    interactiveDiagram.general.diagramData = new Array();

//    interactiveDiagram.general.xAxisName = '';
//    interactiveDiagram.general.yAxisName = '';
//    interactiveDiagram.general.xUnit = '';
//    interactiveDiagram.general.yUnit = '';

//    interactiveDiagram.general.xAxisWithUnit = '';
//    interactiveDiagram.general.yAxisWithUnit = '';
//    interactiveDiagram.general.divContainer = "#trueChart";
//    interactiveDiagram.general.legendWidth = 70;
//}

//interactiveDiagram.general.InitData = function (model) {

//    interactiveDiagram.general.diagramData = $.parseJSON(model);

//    interactiveDiagram.general.xAxisName = interactiveDiagram.general.diagramData.XName;
//    interactiveDiagram.general.yAxisName = interactiveDiagram.general.diagramData.YName;
//    interactiveDiagram.general.xUnit = interactiveDiagram.general.diagramData.XUnit;
//    interactiveDiagram.general.yUnit = interactiveDiagram.general.diagramData.YUnit;

//    if (interactiveDiagram.general.xUnit != null && interactiveDiagram.general.xUnit != '') {
//        interactiveDiagram.general.xAxisWithUnit = interactiveDiagram.general.xAxisName + ' (' + interactiveDiagram.general.xUnit + ')';
//   }
//    if (interactiveDiagram.general.yUnit != null && interactiveDiagram.general.yUnit != '') {
//        interactiveDiagram.general.yAxisWithUnit = interactiveDiagram.general.yAxisName + ' (' + interactiveDiagram.general.yUnit + ')';
//   }


    
//    var oneSeriePoints = interactiveDiagram.general.diagramData.PointsForDiagram;

//    var chartValuesValues = new Array();
//    for (j = 0; j < oneSeriePoints.length; ++j) {
//        chartValuesValues.push(
//         {
//             name: interactiveDiagram.general.diagramData.Id
//                , label: oneSeriePoints[j].X
//                , value: oneSeriePoints[j].Y
//         });
//    }

//    interactiveDiagram.general.series.push(
//    {
//        name: interactiveDiagram.general.diagramData.Id
//          , values: chartValuesValues
//    });  


//    interactiveDiagram.general.legendData.push(interactiveDiagram.general.diagramData.Id);
//    interactiveDiagram.general.legendData = interactiveDiagram.general.legendData.sort(function (a, b) {
//        return parseFloat(b) - parseFloat(a);
//    });

//};

//interactiveDiagram.general.PlotChart = function () {

//    if (typeof interactiveDiagram.general.series === "undefined") {
//        return;
//    }

//    $(interactiveDiagram.general.divContainer).html('');

//    var margin = { top: 20, right: interactiveDiagram.general.legendWidth, bottom: 35, left: 60 };
//    var width = 550 - margin.left - margin.right;
//    var height = 400 - margin.top - margin.bottom;

//    var x = d3.scale.linear().rangeRound([0, width]);
//    var y = d3.scale.linear().rangeRound([height, 0]);


//    var line = d3.svg.line().interpolate("basis").x(function (d) { return x(+d.label); }).y(function (d) { return y( +d.value); });

//    var color = d3.scale.category10();
//    var svg = d3.select(interactiveDiagram.general.divContainer).append("svg").attr("width", width + margin.left + margin.right).attr("height", height + margin.top + margin.bottom).append("g").attr("transform", "translate(" + margin.left + "," + margin.top + ")");



//    color.domain(interactiveDiagram.general.legendData);

//    x.domain([
//     d3.min(interactiveDiagram.general.series, function (c) { var toReturn = d3.min(c.values, function (d) { return +d.label; }); return toReturn > 0 ? 0 : toReturn; }),
//     d3.max(interactiveDiagram.general.series, function (c) { return d3.max(c.values, function (d) { return +d.label; }); })
//    ]);

//    y.domain([
//      d3.min(interactiveDiagram.general.series, function (c) { return 0; }),
//      d3.max(interactiveDiagram.general.series, function (c) { return d3.max(c.values, function (d) { return d.value * 1.2; }); })
//    ]);


//    var xAxis = d3.svg.axis().scale(x).orient("bottom").ticks(5).tickSize(-height, 0, 0).tickFormat(function (d, i) { return d; })
//    var yAxis = d3.svg.axis().scale(y).orient("left").ticks(5).tickSize(-width, 0, 0).tickFormat(function (d, i) { var toReturn = d; return (toReturn); })


//    var legend = svg.selectAll(".legend").data(interactiveDiagram.general.legendData.slice().reverse()).enter().append("g").attr("class", "legend").attr("transform", function (d, i) { return "translate(" + interactiveDiagram.general.legendWidth + "," + i * 20 + ")"; });
//    legend.append("rect").attr("x", width - 10).attr("width", 10).attr("height", 3).style("fill", color).style("stroke", "grey").attr("transform", "translate(0,4)");
//    legend.append("text").attr("x", width - 12).attr("y", 6).attr("dy", ".35em").style("text-anchor", "end").text(function (d) { return d /*+ '°C'*/; });

//    svg.append("g").attr("class", "x axis").attr("transform", "translate(0," + height + ")").call(xAxis).append("text").attr("transform", "translate(255,30)").attr("x", 6).attr("dx", ".71em").style("text-anchor", "end").text(interactiveDiagram.general.xAxisWithUnit);
//    svg.append("g").attr("class", "y axis").call(yAxis).append("text").attr("transform", "rotate(-90)").attr("y", 6).attr("dy", ".7em").style("text-anchor", "end").text(interactiveDiagram.general.yAxisWithUnit);



//    function showData(d) {

//        ShowPointsForCurve(d.name);
//        $("#curvesSelection").val(d.name);
//    }
    
//    var series = svg.selectAll(".series").data(interactiveDiagram.general.series).enter().append("g").attr("class", "series");
//    series.append("path").attr("class", "line").attr("d", function (d) { return line(d.values); }).style("stroke", function (d) { return color(d.name); }).style("stroke-width", "2px").style("fill", "none");

//    if (interactiveDiagram.general.series.length > 1) {
//        series.selectAll(".line").on("click", function (d) { showData.call(this, d); }).on("mouseover", function (d) { document.body.style.cursor = "pointer"; }).on("mouseout", function (d) { document.body.style.cursor = "default"; }).append("svg:title").text(function (d) { return 'Click to view table points for curve: ' + d.name; });
//    } else {
//        series.selectAll(".line").on("click", function (d) { return false; }).on("mouseover", function (d) { document.body.style.cursor = "pointer"; }).on("mouseout", function (d) { document.body.style.cursor = "default"; }).append("svg:title").text(function (d) { return d.name; });
//    }

//};

//interactiveDiagram.stressStrainPlot = {};

//interactiveDiagram.stressStrainPlot.InitData = function (modelValue, sourceTypeId) {

//    interactiveDiagram.general.diagramData = $.parseJSON(modelValue);

//    interactiveDiagram.general.legendWidth = 80;

//    interactiveDiagram.general.diagramData.xAxisName = interactiveDiagram.general.diagramData.XName;
//    interactiveDiagram.general.diagramData.yAxisName = interactiveDiagram.general.diagramData.YName;
//    interactiveDiagram.general.diagramData.xUnit = interactiveDiagram.general.diagramData.XUnit;
//    interactiveDiagram.general.diagramData.yUnit = interactiveDiagram.general.diagramData.YUnit;


//    if (interactiveDiagram.general.xUnit != null && interactiveDiagram.general.xUnit != '') {
//        interactiveDiagram.general.xAxisWithUnit = interactiveDiagram.general.xAxisName + ' (' + interactiveDiagram.general.xUnit + ')';
//    }
//    if (interactiveDiagram.general.yUnit != null && interactiveDiagram.general.yUnit != '') {
//        interactiveDiagram.general.yAxisWithUnit = interactiveDiagram.general.yAxisName + ' (' + interactiveDiagram.general.yUnit + ')';
//    }

      
//    var oneSeriePoints = interactiveDiagram.general.diagramData.PointsForDiagram;
//    var chartValuesValues = new Array();

//    if (sourceTypeId == -2 || sourceTypeId == -3 || sourceTypeId == -4 || sourceTypeId == -5) {
//        for (j = 0; j < oneSeriePoints.length; ++j) {
//            chartValuesValues.push(
//                {
//                    name: interactiveDiagram.general.diagramData.Id
//                    , label: '1E' + oneSeriePoints[j].X.toString()
//                    , value: oneSeriePoints[j].Y
//                });
//        }
//    } else {
//        for (j = 0; j < oneSeriePoints.length; ++j) {
//            chartValuesValues.push(
//                {
//                    name: interactiveDiagram.general.diagramData.Id
//                    , label: oneSeriePoints[j].X.toString()
//                    , value: oneSeriePoints[j].Y
//                });
//        }
//    }
    
//    interactiveDiagram.general.series.push({ name: interactiveDiagram.general.diagramData.Id, values: chartValuesValues });

//    interactiveDiagram.general.legendData.push(interactiveDiagram.general.diagramData.Id);
//    interactiveDiagram.general.legendData = interactiveDiagram.general.legendData.sort(function (a, b) { return parseFloat(b) - parseFloat(a); });

//}

//interactiveDiagram.stressStrainPlot.PlotChart = function () {

//    if (typeof interactiveDiagram.general.series === "undefined") {
//        return;
//    }

//    $(interactiveDiagram.general.divContainer).html('');

//    var scale = 1;
//    var xValues = new Array();
//    //???
//    for (i = 0; i < interactiveDiagram.general.series.length; ++i) {
//        var vals = interactiveDiagram.general.series[i].values;
//        for (j = 0; j < vals.length; ++j) {
//            xValues.push(+vals[j].label);
//        };
//    }
  
//    var margin = { top: 20, right: interactiveDiagram.general.legendWidth, bottom: 35, left: 60 };
//    var width = 550 - margin.left - margin.right;
//    var height = 400 - margin.top - margin.bottom;

//    var x = d3.scale.linear().rangeRound([0, width]);
//    var y = d3.scale.linear().rangeRound([height, 0]);

//    var line = d3.svg.line().interpolate("cardinal").x(function (d) { return x(+d.label / scale); }).y(function (d) { return y(d.value); });


//    var color = d3.scale.category10();

//    var svg = d3.select(interactiveDiagram.general.divContainer).append("svg").attr("width", width +margin.left + margin.right).attr("height", height +margin.top +margin.bottom).append("g").attr("transform", "translate(" +margin.left + "," +margin.top + ")");

//    color.domain(interactiveDiagram.general.legendData);

//    x.domain([d3.min(interactiveDiagram.general.series, function (c) { return 0; }), d3.max(interactiveDiagram.general.series, function (c) { return d3.max(c.values, function (d) { return +d.label; }); })]);
//    y.domain([d3.min(interactiveDiagram.general.series, function (c) { return 0; }), d3.max(interactiveDiagram.general.series, function (c) { return d3.max(c.values, function (d) { return d.value * 1.2; }); })]);

//    var xAxis = d3.svg.axis().scale(x).orient("bottom").ticks(5).tickSize(-height, 0, 0).tickFormat(function (d, i) { return d; })
//    var yAxis = d3.svg.axis().scale(y).orient("left").ticks(5).tickSize(-width, 0, 0)

 

//    svg.append("g").attr("class", "x axis").attr("transform", "translate(0," + height + ")").call(xAxis).append("text").attr("transform", "translate(225,30)").attr("x", 6).attr("dx", ".71em").style("text-anchor", "end").text(interactiveDiagram.general.xAxisWithUnit);
//    svg.append("g").attr("class", "y axis").call(yAxis).append("text").attr("transform", "rotate(-90)").attr("y", 6).attr("dy", ".7em").style("text-anchor", "end").text(interactiveDiagram.general.yAxisWithUnit);

 

//    var legend = svg.selectAll(".legend").data(interactiveDiagram.general.legendData.slice().reverse()).enter().append("g").attr("class", "legend").attr("transform", function (d, i) { return "translate(80," +i * 20 + ")"; });
//    legend.append("rect").attr("x", width - 10).attr("width", 10).attr("height", 3).style("fill", color).style("stroke", "grey").attr("transform", "translate(0,4)");
//    legend.append("text").attr("x", width - 12).attr("y", 6).attr("dy", ".35em").style("text-anchor", "end").text(function (d) { return d; });


//    function showData(d) {
//        ShowPointsForCurve(d.name);
//        $("#curvesSelection").val(d.name);
//    }

//    var series = svg.selectAll(".series").data(interactiveDiagram.general.series).enter().append("g").attr("class", "series");
//    series.append("path").attr("class", "line").attr("d", function (d) { return line(d.values); }).style("stroke", function (d) { return color(d.name); }).style("stroke-width", "2px").style("fill", "none");

//    if (interactiveDiagram.general.series.length > 1) {
//        series.selectAll(".line").on("click", function (d) { showData.call(this, d); }).on("mouseover", function (d) { document.body.style.cursor = "pointer"; }).on("mouseout", function (d) { document.body.style.cursor = "default"; }).append("svg:title").text(function (d) { return 'Click to view table points for curve: ' + d.name; });
//    } else {
//        series.selectAll(".line").on("click", function (d) { return false; }).on("mouseover", function (d) { document.body.style.cursor = "pointer"; }).on("mouseout", function (d) { document.body.style.cursor = "default"; }).append("svg:title").text(function (d) { return d.name; });
//    }
//}

//interactiveDiagram.fatiguePlot = { };

//var fatigueType;

//interactiveDiagram.fatiguePlot.InitData = function(modelValue, type) {

//    interactiveDiagram.general.diagramData = $.parseJSON(modelValue);


//    interactiveDiagram.general.xAxisName = interactiveDiagram.general.diagramData.XName;
//    interactiveDiagram.general.yAxisName = interactiveDiagram.general.diagramData.YName;
//    interactiveDiagram.general.xUnit = interactiveDiagram.general.diagramData.XUnit;
//    interactiveDiagram.general.yUnit = interactiveDiagram.general.diagramData.YUnit;


//    if (interactiveDiagram.general.xUnit != null && interactiveDiagram.general.xUnit != '') {
//        interactiveDiagram.general.xAxisWithUnit = interactiveDiagram.general.xAxisName + ' (' + interactiveDiagram.general.xUnit + ')';
//   }
//    if (interactiveDiagram.general.yUnit != null && interactiveDiagram.general.yUnit != '') {
//        interactiveDiagram.general.yAxisWithUnit = interactiveDiagram.general.yAxisName + ' (' + interactiveDiagram.general.yUnit + ')';
//   }


//    fatigueType = type;

//    if (fatigueType == -5) {

//          interactiveDiagram.fatiguePlot.InitSeriesData();
//          interactiveDiagram.fatiguePlot.InitLegendData();
//          interactiveDiagram.general.legendWidth = 90;
//    } else {

//          interactiveDiagram.general.legendWidth = 70;
//          interactiveDiagram.fatiguePlot.InitSeriesData();
//          interactiveDiagram.fatiguePlot.InitLegendData();      
//    }

//}

//interactiveDiagram.fatiguePlot.InitSeriesData = function (dataCurves) {         
       
//        var oneSeriePoints = interactiveDiagram.general.diagramData.PointsForDiagram;
//        var chartValues = new Array();

//        for (j = 0; j < oneSeriePoints.length; ++j) {
//            if(fatigueType == -5) {
//                  chartValues.push(
//            {
//                    name: interactiveDiagram.general.diagramData.Id
//                   , label: oneSeriePoints[j].X.toString()
//                   , value: parseInt(Math.pow(10, oneSeriePoints[j].Y))
//               });
              
//            } else {
//                chartValues.push(
//                {
//                    name: interactiveDiagram.general.diagramData.Id
//                   , label: oneSeriePoints[j].X.toString()
//                   , value: parseFloat(Math.pow(10, oneSeriePoints[j].Y))
//                   });
//            };

//        };

//        interactiveDiagram.general.series.push(
//            {
//                  name: interactiveDiagram.general.diagramData.Id
//                , values: chartValues
//            });
//}

//interactiveDiagram.fatiguePlot.InitLegendData = function () {

//    interactiveDiagram.general.legendData.push(interactiveDiagram.general.diagramData.Id);   
//    interactiveDiagram.general.legendData = interactiveDiagram.general.legendData.sort(function (a, b) {
//        return (a - b);
//    });
//}

//interactiveDiagram.fatiguePlot.PlotChart = function (type) {
    
//    if (typeof interactiveDiagram.general.series === "undefined") {
//        return;
//    }
//    $(interactiveDiagram.general.divContainer).html('');


//    var xValues = new Array();
//    var yValues = new Array();

//    var scale = 1;

//    for (i = 0; i < interactiveDiagram.general.series.length; ++i) {
//        var vals = interactiveDiagram.general.series[i].values;
//        for (j = 0; j < vals.length; ++j) {
//            xValues.push('1E' + vals[j].label);
//        };
//    };
//    xValues = xValues.unique().sort();

//    scale = 1;

//    var margin = { top: 20, right: interactiveDiagram.general.legendWidth, bottom: 35, left: 60 };
//    var width = 550 - margin.left - margin.right;
//    var height = 400 - margin.top - margin.bottom;
  
//    var x = d3.scale.linear().rangeRound([0, width]);
//    var y = d3.scale.log().rangeRound([height, 0]);


//    var line = d3.svg.line().interpolate("cardinal").x(function (d) { return x(+d.label / scale); }).y(function (d) { return y(d.value); });
//    var color = d3.scale.category10();
//    var svg = d3.select(interactiveDiagram.general.divContainer).append("svg").attr("width", width + margin.left + margin.right).attr("height", height + margin.top + margin.bottom).append("g").attr("transform", "translate(" + margin.left + "," + margin.top + ")");
    
//    color.domain(interactiveDiagram.general.legendData);

//    x.domain([ d3.min(interactiveDiagram.general.series, function (c) {return d3.min(c.values, function (d) { return +d.label; }); }), d3.max(interactiveDiagram.general.series, function (c) {return d3.max(c.values, function (d) { return +d.label; });}) ]);
//    y.domain([ d3.min(interactiveDiagram.general.series, function (c) {return d3.min(c.values, function (d) { return d.value; }); }), d3.max(interactiveDiagram.general.series, function (c) { return d3.max(c.values, function (d) { return d.value; }); }) ]);
  
//    var xAxis = d3.svg.axis().scale(x).orient("bottom").ticks(xValues.length - 1).tickSize(-height, 0, 0) .tickFormat(function (d, i) { return xValues[i]; })
//    var yAxis = d3.svg.axis().scale(y).orient("left").ticks(5).tickSize(-width, 0, 0).tickFormat(function (d, i) {
//            var toReturn = (parseInt(d * 100000) / 100000);
//            if (type == -5) {             
//            } else {
//                   toReturn = toReturn.toString().substr(toReturn.toString().length -1) == "1" ? toReturn: "";
//              };
//            return (toReturn);
//     })
     

  


//    svg.append("g").attr("class", "x axis").call(xAxis).append("text").attr("transform", "translate(0," + height + ")").attr("transform", "translate(300,30)").attr("x", 6).attr("dx", ".71em").style("text-anchor", "end").text(interactiveDiagram.general.xAxisName);
//    svg.append("g").attr("class", "y axis").call(yAxis).append("text").attr("transform", "rotate(-90)").attr("y", 6).attr("dy", ".7em").style("text-anchor", "end").text(interactiveDiagram.general.yAxisName);

   
//    var series = svg.selectAll(".series").data(interactiveDiagram.general.series).enter().append("g").attr("class", "series");    
//    series.append("path").attr("class", "line").attr("d", function (d) { return line(d.values); }).style("stroke", function (d) { return color(d.name); }).style("stroke-dasharray", function (d) { return ("10, 0"); }).style("stroke-width", "2px").style("fill", "none");

//    if (interactiveDiagram.general.series.length > 1) {
//        series.selectAll(".line").on("click", function (d) { $("#curveName" + type).html(d.name); showData.call(this, d.values); }).append("svg:title").on("mouseover", function (d) { document.body.style.cursor = "pointer"; }).on("mouseout", function (d) { document.body.style.cursor = "default"; }).text(function (d) { return 'Click to view table points for: ' + d.name; });
//    } else {
//        series.selectAll(".line").on("click", function (d) { return false; }).append("svg:title").on("mouseover", function (d) { document.body.style.cursor = "pointer"; }).on("mouseout", function (d) { document.body.style.cursor = "default"; }).text(function (d) { return d.name; });
//    }

//    function showData(d) {
//        ShowPointsForCurve(d[0].name);
//        $("#curvesSelection").val(d[0].name);
//    }

    
//    var legend = svg.selectAll(".legend").data(interactiveDiagram.general.legendData.slice().reverse()).enter().append("g").attr("class", "legend").attr("transform", function (d, i) { return "translate(" + interactiveDiagram.general.legendWidth + "," + i * 20 + ")"; });
//    legend.append("rect").attr("x", width - 10).attr("width", 10).attr("height", 3).style("fill", color).style("stroke", "grey").attr("transform", "translate(0,4)");
//    legend.append("text").attr("x", width - 12).attr("y", 6).attr("dy", ".35em").style("text-anchor", "end").text(function (d) { return d; });
   
//}


