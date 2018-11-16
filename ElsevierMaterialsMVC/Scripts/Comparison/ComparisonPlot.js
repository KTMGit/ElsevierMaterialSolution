;
var comparisonPlot = {};

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


comparisonPlot.Data = new Array();
var objNames = new Array();
var objProperties = new Array();

comparisonPlot.DataXY = new Array();
var objNamesXY = new Array();
var objPropertiesXY = new Array();

comparisonPlot.Property = function () {
    this.GroupId = -1;
    this.TypeId = -1;
    this.SourceTypeId = -1;
    this.RowId = -1;
    this.ConditionId = -1;
    this.Group = '';
    this.Unit = '';
    this.Name = '';
    this.Materials = new Array();
}

comparisonPlot.Material = function () {
    this.MaterialId = -1;
    this.Name = '';
    this.SubgroupId = -1;
    this.SourceId = -1;
    this.SourceMaterialId = -1;
    this.Value = '';
    this.ConditionId = -1;
    this.Condition = '';
}

//comparisonPlot.PropertyProperty = function () {
//    this.Name = '';
//    this.MaxValue = null;
//    this.MinValue = null;
//}


comparisonPlot.InitComparisonPlotData = function () {
    var i;
    var comparisonPlotData = new Array();
    for (i = 0; i < objProperties.length; ++i) {
        var prop = objProperties[i].PropertyInfo;
        var mats = objProperties[i].Materials;
        //
        var oneProperty = new comparisonPlot.Property();
        oneProperty.GroupId = prop.GroupId;
        oneProperty.TypeId = prop.TypeId;
        oneProperty.SourceTypeId = prop.SourceTypeId;
        oneProperty.RowId = prop.RowId;
        oneProperty.ConditionId = prop.ConditionId;
        oneProperty.Group = prop.Group;
        oneProperty.Unit = prop.Unit;
        oneProperty.Name = prop.Name;

        oneProperty.Materials = new Array();
        for (j = 0; j < mats.length; ++j) {
            var mat = mats[j];
            //
            var oneMaterial = new comparisonPlot.Material();
            oneMaterial.MaterialId = mat.MaterialId;
            oneMaterial.Name = mat.Name;
            oneMaterial.SubgroupId = mat.SubgroupId;
            oneMaterial.SourceId = mat.SourceId;
            oneMaterial.SourceMaterialId = mat.SourceMaterialId;
            oneMaterial.Value = mat.Value;
            oneMaterial.ConditionId = mat.ConditionId;
            oneMaterial.Condition = mat.Condition;
            oneProperty.Materials.push(oneMaterial);
        }
        comparisonPlotData.push(oneProperty);
    }

    return comparisonPlotData;
}



comparisonPlot.InitRadarChart = function (modelValue) {
    // Read data from model
    objProperties = $.parseJSON(modelValue).Properties;
    comparisonPlot.Data = comparisonPlot.InitComparisonPlotData(objProperties);

    // Get all names
    objNames = $.parseJSON(modelValue).MaterialNames;

    ///
    /// Initiate all check boxes
    ///
    $('#radarMaterials').empty();
    for (i = 0; i < objNames.length; ++i) {
        comparisonPlot.AddCheckBox(
            'radarMaterials',
            'cb_radarMat_' + i,
             $.trim(objNames[i].Name),
             $.trim(objNames[i].Name),
             objNames[i].MaterialId.toString() + '#' + objNames[i].SourceMaterialId.toString() + '#' + objNames[i].SubgroupId.toString(),
             $.trim(objNames[i].SubgroupName),
             $.trim(objNames[i].Name),
             true
            );
    };

    var props = new Array();
    for (m = 0; m < objNames.length; ++m) {
        for (i = 0; i < comparisonPlot.Data.length; ++i) {
            var prop = comparisonPlot.Data[i];
            var propUnit = $.trim(prop.Unit) == '' ? $.trim(prop.Unit) : ' (' + prop.Unit + ')';
            var propAxes = comparisonPlot.ClearHtml($.trim(prop.Name) + propUnit);
            var matProp = { axis: propAxes, value: null, origvalue: null, propGroupId: prop.GroupId, propGroup: prop.Group, htmlName: $.trim(prop.Name) + propUnit };

            // prepare propery list from the first material (all is the same!)
            if (m == 0) {
                props.push({
                    Name: matProp.axis,
                    MaxValue: null,
                    MinValue: null,
                    Ratio: 1,
                    propGroupId: matProp.propGroupId,
                    propGroup: matProp.propGroup,
                    htmlName: matProp.htmlName
                });
            }
        }
    }

    $('#radarProperties').empty();
    var prevProp = '';
    for (i = 0; i < props.length; ++i) {
        //if ( prevProp != props[i].propGroup) {
        //    prevProp = props[i].propGroup;
        //    $('#radarProperties').append('<div style="clear: both;"></div>');
        //    var lbl = '<label class="radarLabel">' + props[i].propGroup + '</label>';
        //    $('#radarProperties').append(lbl);
        //}
        comparisonPlot.AddCheckBox(
            'radarProperties',
            'cb_radarProp_' + i,
            $.trim(props[i].Name),
            $.trim(props[i].Name),
            '',
            '',
            $.trim(props[i].htmlName),
            true
            );
    };
    // End of initiate check boxes
}

comparisonPlot.PlotRadarChart = function () {
    // Prepare data for the chart
    var d = new Array();
    var propList = new Array();

    // main material loop
    var LegendOptions = new Array();
    for (m = 0; m < objNames.length; ++m) {
        // check if material checkbox is checked
        var isMatChecked = false;
        $('#radarMaterials').children('[id^=cb_radarMat]').each(function () {
            var uniqueId = objNames[m].MaterialId.toString() + '#' + objNames[m].SourceMaterialId.toString() + '#' + objNames[m].SubgroupId.toString();
            if (uniqueId == $(this).attr('uid')) {
                isMatChecked = $(this).is(':checked');
            }
        });

        if (isMatChecked == true) {
            var matId = objNames[m].MaterialId.toString() + '#' + objNames[m].SourceMaterialId.toString() + '#' + objNames[m].SubgroupId.toString();
            var matData = new Array();
            LegendOptions.push(objNames[m].Name);

            // property loop
            for (i = 0; i < comparisonPlot.Data.length; ++i) {
                var prop = comparisonPlot.Data[i];
                var propUnit = $.trim(prop.Unit) == '' ? $.trim(prop.Unit) : ' (' + prop.Unit + ')';
                var propAxes = comparisonPlot.ClearHtml($.trim(prop.Name) + propUnit);
                var matProp = { axis: propAxes, value: null, origvalue: null };

                // check if property checkbox is checked
                var isPropChecked = false;
                $('#radarProperties').children('[id^=cb_radarProp]').each(function () {
                    if (propAxes == $(this).attr('name')) {
                        isPropChecked = $(this).is(':checked');
                    }
                });

                if (isPropChecked == true) {
                    // prepate propery list from the first material (all is the same!)
                    if (m == 0) {
                        propList.push({
                            Name: matProp.axis,
                            MaxValue: null,
                            MinValue: null,
                            Ratio: 1
                        });
                    }

                    // inner material loop
                    for (j = 0; j < prop.Materials.length; ++j) {
                        var mat = prop.Materials[j];
                        var uniqueId = mat.MaterialId.toString() + '#' + mat.SourceMaterialId.toString() + '#' + mat.SubgroupId.toString();
                        if (matId == uniqueId) {

                            // transform (>=), (<=) and (-) to numeric value
                            if (mat.Value.indexOf("&GreaterEqual;") >= 0) {
                                mat.Value = $.trim(mat.Value.replace("&GreaterEqual;", ""));
                            }
                            if (mat.Value.indexOf("&le;") >= 0) {
                                mat.Value = $.trim(mat.Value.replace("&le;", ""));
                            }
                            if (mat.Value.indexOf("-") >= 0 && mat.Value.indexOf("E") == 0) {
                                mat.Value = $.trim(mat.Value.split('-')[0]);
                            }

                            if (mat.Value.indexOf("E") >= 0) {
                                var firstPart = $.trim(mat.Value.split('E')[0]).replace(">", "").replace(">=", "").replace("<", "").replace("<=", "").replace(" ", "");
                                var secondPart = $.trim(mat.Value.split('E')[1]);

                                mat.Value = firstPart * Math.pow(10, parseInt(secondPart));
                            }
                            if ($.isNumeric(mat.Value)) {
                                matProp.value = parseFloat(mat.Value);
                                matProp.origvalue = matProp.value;
                            }

                            // exit from inner material loop
                            break;
                        }
                    }

                    // add each property fo every material!
                    matData.push(matProp);

                }   // isPropChecked == true
            }

            // add material with all properties
            d.push(matData);

        } // isMatChecked == true
    }

    // find max and min value for every property
    for (p = 0; p < propList.length; ++p) {
        var prop = propList[p].Name;
        var propMax = null;
        var propMin = null;
        for (i = 0; i < d.length; ++i) {
            matData = d[i];

            for (j = 0; j < matData.length; ++j) {
                propData = matData[j];

                if (propData.axis == prop) {
                    if ($.isNumeric(propData.value)) {
                        propMax = propMax == null ? propData.value : (propData.value > propMax ? propData.value : propMax);
                        propMin = propMin == null ? propData.value : (propData.value < propMin ? propData.value : propMin);
                    }
                    break;
                }
            }
        }
        propList[p].MaxValue = propMax;
        propList[p].MinValue = propMin;
    }
    //
    // find max and min value for all properties and calculate Ratios
    var propMaxValue = null;
    var propMinValue = null
    for (i = 0; i < propList.length; ++i) {
        if ($.isNumeric(propList[i].MaxValue)) {
            propMaxValue = propMaxValue == null ? propList[i].MaxValue : (propList[i].MaxValue > propMaxValue ? propList[i].MaxValue : propMaxValue);
            propMinValue = propMinValue == null ? propList[i].MinValue : (propList[i].MinValue < propMinValue ? propList[i].MinValue : propMinValue);
        }
    }

    // flooring number to order of magnitude
    var orderMax = Math.floor(Math.log(propMaxValue) / Math.LN10 + 0.000000001);
    var ratioMax = Math.pow(10, orderMax);
    if (propMaxValue != null && propMaxValue != 0) {
        for (i = 0; i < propList.length; ++i) {
            if ($.isNumeric(propList[i].MaxValue)) {
                var order = Math.floor(Math.log(propList[i].MaxValue) / Math.LN10 + 0.000000001); // because float math works like that
                propList[i].Ratio = Math.pow(10, order);
            }
        }
    }

    // scale diagram data
    if (propMaxValue != null && propMaxValue != 0) {
        for (i = 0; i < d.length; ++i) {
            matData = d[i];
            for (j = 0; j < matData.length; ++j) {
                propData = matData[j];
                for (p = 0; p < propList.length; ++p) {
                    if (propList[p].Name == propData.axis
                        && $.isNumeric(propList[p].Ratio)
                        && $.isNumeric(propData.value)) {
                        propData.value = propData.value * ratioMax / propList[p].Ratio;
                    }
                }
            }
        }
    }

    // additional config parameters
    var w = 540,
        h = 500,
        ew = 150;

    //Options for the Radar chart, other than default
    var mycfg = {
        w: w,
        h: h,
        //maxValue: orderMax == null ? 0 : orderMax,
        levels: 6,      // Number of spider nets (o to hide net and net values)
        ExtraWidthX: ew
    }

    var colorscale = d3.scale.category10();

    //Call function to draw the Radar chart
    //Will expect that data is in %'s
    if (d.length > 0) {
        RadarChart.draw("#chart", d, mycfg);
    } else {
        $("#chart").empty();
    }

    ////////////////////////////////////////////
    /////////// Initiate legend ////////////////
    ////////////////////////////////////////////

    var svg = d3.select('#bodyRadarChart')
        .selectAll('svg')
        .append('svg')
        .attr("width", w + ew)
        .attr("height", h)

    //Create the title for the legend
    var text = svg.append("text")
        .attr("class", "title")
        .attr('transform', 'translate(90,0)')
        .attr("x", w - 70)
        .attr("y", 10)
        .attr("font-size", "12px")
        .attr("fill", "#404040")
        .text("Materials");

    //Initiate Legend	
    var legend = svg.append("g")
        .attr("class", "legend")
        .attr("height", 100)
        .attr("width", 200)
        .attr('transform', 'translate(90,20)')
    ;
    //Create colour squares
    legend.selectAll('rect')
      .data(LegendOptions)
      .enter()
      .append("rect")
      .attr("x", w - 65)
      .attr("y", function (d, i) { return i * 20; })
      .attr("width", 10)
      .attr("height", 10)
      .style("fill", function (d, i) { return colorscale(i); })
    ;
    //Create text next to squares
    legend.selectAll('text')
      .data(LegendOptions)
      .enter()
      .append("text")
      .attr("x", w - 52)
      .attr("y", function (d, i) { return i * 20 + 9; })
      .attr("font-size", "11px")
      .attr("fill", "#737373")
      .text(function (d) { return d; })
    ;
    /////////// End of Initiate legend ////////////////
};




/////////////////////////////////////////////
////////  X-Y PLOT
/////////////////////////////////////////////
var seriesDataXY = new Array();
var legendDataXY = new Array();

comparisonPlot.InitXYChart = function (modelValue) {
    // Read data from model
    objPropertieXY = $.parseJSON(modelValue).Properties;
    comparisonPlot.DataXY = comparisonPlot.InitComparisonPlotData(objPropertieXY);

    // Get all names
    objNamesXY = $.parseJSON(modelValue).MaterialNames;

    //console.log("objPropertieXY: ", objPropertieXY);
    //console.log("comparisonPlot.DataXY: ", comparisonPlot.DataXY);

    // Initiate check boxes for materials
    $('#xyMaterials').empty();
    for (i = 0; i < objNamesXY.length; ++i) {
        comparisonPlot.AddCheckBoxXY(
            'xyMaterials',
            'cb_xyMat_' + i,
             $.trim(objNamesXY[i].Name),
             $.trim(objNamesXY[i].Name),
             objNamesXY[i].MaterialId.toString() + '#' + objNamesXY[i].SourceMaterialId.toString() + '#' + objNamesXY[i].SubgroupId.toString(),
             $.trim(objNamesXY[i].SubgroupName),
             $.trim(objNamesXY[i].Name),
             true
            );
    };

    // Get list of properties
    var props = new Array();
    for (m = 0; m < objNamesXY.length; ++m) {
        for (i = 0; i < comparisonPlot.DataXY.length; ++i) {
            var prop = comparisonPlot.DataXY[i];
            var propUnit = $.trim(prop.Unit) == '' ? $.trim(prop.Unit) : ' (' + prop.Unit + ')';
            var propAxes = comparisonPlot.ClearHtml($.trim(prop.Name) + propUnit);
            var matProp = { axis: propAxes, value: null, origvalue: null, propGroupId: prop.GroupId, propGroup: prop.Group, htmlName: $.trim(prop.Name) + propUnit };

            // prepare propery list from the first material (all is the same!)
            if (m == 0) {
                props.push({
                    Name: matProp.axis,
                    MaxValue: null,
                    MinValue: null,
                    Ratio: 1,
                    propGroupId: matProp.propGroupId,
                    propGroup: matProp.propGroup,
                    htmlName: matProp.htmlName
                });
            }
        }
    }

    //console.log("Properties: ", props);

    var selectedItem;

    selectedItem = 0;
    $("#xyPropertiesX").html('<select id="xySelectPropX"></select>');
    comparisonPlot.AddOptionsXY(props, "xySelectPropX");
    $("select[id='xySelectPropX'] option:eq(" + selectedItem.toString() + ")").attr("selected", "selected");
    //
    selectedItem = (props.length > 1 ? 1 : 0);
    $("#xyPropertiesY").html('<select id="xySelectPropY"></select>');
    comparisonPlot.AddOptionsXY(props, "xySelectPropY");
    $("select[id='xySelectPropY'] option:eq(" + selectedItem.toString() + ")").attr("selected", "selected");
};


comparisonPlot.InitSeriesDataXY = function () {
    var dataCurves = comparisonPlot.DataXY;
    var seriesData = new Array();
    legendDataXY = new Array();

    // Find list of selected materials
    var selectedMaterials = new Array();
    for (i = 0; i < objNamesXY.length; ++i) {
        var id = 'cb_xyMat_' + i;
        var isMaterialChecked = $("#" + id).is(':checked');
        var uid = objNamesXY[i].MaterialId.toString() + '#' + objNamesXY[i].SourceMaterialId.toString() + '#' + objNamesXY[i].SubgroupId.toString();
        var vkKey = objNamesXY[i].SourceMaterialId.toString();
        var material = $.trim(objNamesXY[i].Name);

        if (isMaterialChecked == true) {
            selectedMaterials.push(
               {
                   uid: uid
                   , vkKey: vkKey
                   , material: material
               });
            //legendDataXY.push(material);
        }
    };

    //console.log("Selected Materials: ", selectedMaterials);

    // Find list of two selected properties
    var selectedProperties = new Array();
    selectedProperties.push($('#xySelectPropX').find(":selected").text());
    selectedProperties.push($('#xySelectPropY').find(":selected").text());
    //console.log("Selected Properties: ", selectedProperties);

    for (m = 0; m < selectedMaterials.length; ++m) {
        var material = selectedMaterials[m].material;
        var uid = selectedMaterials[m].uid;
        var materialId = uid.split("#")[0];
        var vkKey = selectedMaterials[m].vkKey;

        var materialData = { name: material, values: new Array() };
        materialData.values.push({ label: null, name: material, value: null });

        // Loop for x and y valuees
        for (i = 0; i < dataCurves.length; ++i) {
            var prop = dataCurves[i];
            var propUnit = $.trim(prop.Unit) == '' ? $.trim(prop.Unit) : ' (' + prop.Unit + ')';
            var propAxes = comparisonPlot.ClearHtml($.trim(prop.Name) + propUnit);
            var matProp = { axis: propAxes, value: null, origvalue: null, propGroupId: prop.GroupId, propGroup: prop.Group, htmlName: $.trim(prop.Name) + propUnit };

            // x value
            if (matProp.htmlName == selectedProperties[0]) {
                for (j = 0; j < prop.Materials.length; ++j) {
                    if (prop.Materials[j].SourceMaterialId == vkKey && prop.Materials[j].Name == material && prop.Materials[j].MaterialId) {
                        materialData.values[0].label = parseFloat(prop.Materials[j].Value);
                    }
                }
            }

            // y value
            if (matProp.htmlName == selectedProperties[1]) {
                for (j = 0; j < prop.Materials.length; ++j) {
                    if (prop.Materials[j].SourceMaterialId == vkKey && prop.Materials[j].Name == material && prop.Materials[j].MaterialId) {
                        materialData.values[0].value = parseFloat(prop.Materials[j].Value);
                        //materialData.values[1].value = 0;
                    }
                }
            }
        };

        if (materialData.values[0].label != null && materialData.values[0].value != null) {
            seriesData.push(materialData);
            legendDataXY.push(material);
        }
    }

    seriesDataXY = seriesData;
    return;
}

comparisonPlot.ClearHtml = function (toClear) {
    var cleaned = toClear.replace(/\b(<sup>)/gi, '^');
    cleaned = cleaned.replace(/\b(<\/sup>|<sub>|<\/sub>)/gi, '');
    cleaned = comparisonPlot.replaceAll('&deg;', '°', cleaned);
    return cleaned;
}

comparisonPlot.replaceAll = function (find, replace, str) {
    return str.replace(new RegExp(find, 'g'), replace);
}

comparisonPlot.AddCheckBox = function (containerId, id, name, value, uid, title, htmlName, checked) {
    $('#' + containerId).append('<div style="clear: both;"></div>');
    $('#' + containerId)
    .append(
       $(document.createElement('input')).attr({
           id: id
          , name: name
          , value: value
          , uid: uid
          , type: 'checkbox'
          , checked: checked
       })
    );

    $('#' + id).change(function () {
        // Count number and status of checked options. Result numbers is AFTER click action!
        var matChecked = $('#radarMaterials').children('[id^=cb_radarMat]:checked').length;
        var propChecked = $('#radarProperties').children('[id^=cb_radarProp]:checked').length;
        var currUid = $(this).attr('uid');
        var isChecked = $(this).is(':checked');

        if ($(this).attr('id').substring(0, 11) == 'cb_radarMat') {
            if (matChecked == 0) {
                $(this).prop('checked', true);
                alert('At least one material has to be selected.');
                return;
            }
            $('#radarMaterials').children('[id^=cb_radarMat]').each(function (index, element) {
                if ($(this).attr('uid') == currUid) {
                    isChecked ? $('.radar-chart-serie' + index).show() : $('.radar-chart-serie' + index).hide();
                }
            });
        }
        //
        if ($(this).attr('id').substring(0, 12) == 'cb_radarProp') {
            if (propChecked == 0) {
                $(this).prop('checked', true);
                alert('At least one property has to be selected.');
                return;
            }
            comparisonPlot.PlotRadarChart();
        }
    });
    //
    var lbl = '<label class="radarLabel" title="' + title + '" id="lbl_' + id + '">' + htmlName + '</label>';
    $('#' + containerId).append(lbl);
}

comparisonPlot.AddCheckBoxXY = function (containerId, id, name, value, uid, title, htmlName, checked) {
    $('#' + containerId).append('<div style="clear: both;"></div>');
    $('#' + containerId)
    .append(
       $(document.createElement('input')).attr({
           id: id
          , name: name
          , value: value
          , uid: uid
          , type: 'checkbox'
          , checked: checked
       })
    );

    $('#' + id).change(function () {
        // Count number and status of checked options. Result numbers is AFTER click action!
        var matChecked = $('#xyMaterials').children('[id^=cb_xyMat]:checked').length;
        var currUid = $(this).attr('uid');
        var isChecked = $(this).is(':checked');

        if ($(this).attr('id').substring(0, 'cb_xyMat'.length) == 'cb_xyMat') {
            if (matChecked == 0) {
                $(this).prop('checked', true);
                alert('At least one material has to be selected.');
                return;
            }
            comparisonPlot.InitSeriesDataXY();
            comparisonPlot.PlotXYChartTimeout(1);
        }
    });
    //
    var lbl = '<label class="radarLabel" title="' + title + '" id="lbl_' + id + '">' + htmlName + '</label>';
    $('#' + containerId).append(lbl);
};

comparisonPlot.AddOptionsXY = function (props, selectID) {
    var dropdown = document.getElementById(selectID);
    var propsOrdered = props.sort(function (a, b) {
        if (a < b) return -1;
        if (a > b) return 1;
        return 0;
    });
    for (p = 0; p < propsOrdered.length; ++p) {
        var opt = new Option();
        opt.value = propsOrdered[p].htmlName;
        opt.text = propsOrdered[p].htmlName;
        opt.setAttribute("MinValue", propsOrdered[p].MinValue);
        opt.setAttribute("MaxValue", propsOrdered[p].MaxValue);
        dropdown.options.add(opt);
    }

    // attach onchange event to dropdown
    $("#" + selectID).on("change", function (e) { comparisonPlot.InitSeriesDataXY(); comparisonPlot.PlotXYChartTimeout(2); });
};

comparisonPlot.PlotXYChartTimeout = function (type) {
    // 0 - after init
    // 1 - from materials
    // 2 - from properties 

    // wait some time before js finalized initialization
    setTimeout(function () {
        comparisonPlot.PlotXYChart(type);
    }, 100);
};

comparisonPlot.PlotXYChart = function (type) {
    // 0 - after init
    // 1 - from materials
    // 2 - from properties 

    var varNames, seriesData, xLabel, yLabel, legendWidth;

    varNames = legendDataXY;
    seriesData = seriesDataXY;
    xLabel = $('#xySelectPropX').find(":selected").text();
    yLabel = $('#xySelectPropY').find(":selected").text();
    legendWidth = 90;

    //console.log("Series Data: ", seriesData);
    //console.log("Legend Data: ", varNames);
    //console.log("Label X: ", xLabel);
    //console.log("Label Y: ", yLabel);

    if (typeof seriesData === "undefined") {
        return;
    }

    var svgContainer = "#xyChart";
    $(svgContainer).html("");

    // Init variables
    var scale = 1;
    var margin = { top: 20, right: legendWidth, bottom: 35, left: 60 };
    var width = 550 - margin.left - margin.right;
    var height = 400 - margin.top - margin.bottom;

    //var x = d3.scale.ordinal()
    //    .rangeRoundBands([0, width], .1);
    var x = d3.scale.linear()
      .rangeRound([0, width]);

    var y = d3.scale.linear()
        .rangeRound([height, 0]);

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
       //console.log("xMin" + type + ":", d3.min(c.values, function (d) { return +d.label; }));
       //return d3.min(c.values, function (d) { return +d.label; });
       return 0;
   }),
   d3.max(seriesData, function (c) {
       //console.log("xMax" + type + ":", d3.max(c.values, function (d) { return +d.label; }));
       //return d3.max(c.values, function (d) { return +d.label; });
       return d3.max(c.values, function (d) { return +d.label; }) * 1.2;
   })
    ]);
    y.domain([
      d3.min(seriesData, function (c) {
          //return d3.min(c.values, function (d) { return +d.value; });
          return 0;
      }),
      d3.max(seriesData, function (c) {
          //return d3.max(c.values, function (d) { return +d.value; });
          return d3.max(c.values, function (d) { return +d.value; }) * 1.2;
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
            .attr("transform", "rotate(-90)")
            .attr("y", 6)
            .attr("dy", ".7em")
            .style("text-anchor", "end")
            .text(yLabel);


    // See https://github.com/mbostock/d3/wiki/SVG-Shapes for different interpolates
    // Add points, one for each material (one serie is one point!)
    svg.selectAll("circle")
		.data(seriesData).enter()
		.append("circle")
		.attr("cx", function (d) { /*console.log("cx: ", d.values[0].label);*/ return x(d.values[0].label) })
		.attr("cy", function (d) { /*console.log("cy: ", d.values[0].value);*/ return y(d.values[0].value) })
		.attr("r", "12px")
		.style("fill", function (d, i) { return color(d.name); })

    // Tooltip and data
    svg.selectAll("circle")
            .on("mouseover", function (d) { document.body.style.cursor = "pointer"; })
            .on("mouseout", function (d) { document.body.style.cursor = "default"; })
            .append("svg:title")
                .text(function (d, i) {
                    var toReturn = "";
                    toReturn += 'Material: ' + d.name;
                    toReturn += "\n" + xLabel + ": " + d.values[0].label;
                    toReturn += "\n" + yLabel + ": " + d.values[0].value;
                    return toReturn;
                });

    // Legend
    var legend = svg.selectAll(".legend")
        .data(varNames.slice().reverse()).enter()
            .append("g")
            .attr("class", "legend")
            .attr("transform", function (d, i) { return "translate(" + legendWidth.toString() + "," + i * 20 + ")"; });

    legend.append("rect")
           .attr("x", width - 10)
           .attr("width", 6)
           .attr("height", 6)
           .style("fill", color)
           .style("stroke", "grey")
           .attr("transform", "translate(0,2)");

    legend.append("text")
        .attr("x", width - 12)
        .attr("y", 6)
        .attr("dy", ".35em")
        .style("text-anchor", "end")
        .text(function (d) { return d /*+ '°C'*/; });
};



//////// END OF X-Y PLOT
/////////////////////////////////////////////
