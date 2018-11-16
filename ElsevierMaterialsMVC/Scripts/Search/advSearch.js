;
var advSearchFunc = {}

//
// Definitions for selected properties
//

// enum for property types
advSearchFunc.propertyType = {
    NotDefined: -1,
    Material: 0,
    Property: 1
}

// enum for binary operators
advSearchFunc.binaryOperators = {
    NotDefined: -1,
    And: 0,
    Or: 1,
    Not: 2
}

// enum for logical operators
advSearchFunc.logicalOperators = {
    NotDefined: -1,
    Exists: 0,
    Eq: 1,
    Lte: 2,
    Gte: 3,
    Between: 4
}

// 
advSearchFunc.property = function () {
    this.propertyType = -1;
    this.binaryOperators = -1;
    this.logicalOperators = -1;
    this.uid = '';
    this.propertyId = -1;
    this.propertyName = '';
    this.valueFrom = '';
    this.valueTo = '';
    this.valueFrom_orig = '';
    this.valueTo_orig = '';
    this.unitId = -1;
    this.unitName = '';

    this.propertyConditions = new Array();
    this.isPropertyConditionsActive = false;
}


advSearchFunc.selectedProperties = new Array();

advSearchFunc.addSelectedProperty = function (
        propertyType
        , binaryOperators
        , logicalOperators
        , uid
        , propertyId
        , propertyName
        , valueFrom
        , valueTo
        , valueFrom_orig
        , valueTo_orig
        , unitId
        , unitName
        , isPropertyConditionsActive
        , propertyConditions
    ) {
    // 1: as advSearchFunc.property
    var def = new advSearchFunc.property();
    def.propertyType = propertyType;
    def.binaryOperators = binaryOperators;
    def.logicalOperators = logicalOperators;
    def.uid = uid;
    def.propertyId = propertyId;
    def.propertyName = propertyName;
    def.valueFrom = valueFrom;
    def.valueTo = valueTo;
    def.valueFrom_orig = valueFrom_orig;
    def.valueTo_orig = valueTo_orig;
    def.unitId = unitId;
    def.unitName = unitName;
    def.isPropertyConditionsActive = isPropertyConditionsActive;
    def.propertyConditions = propertyConditions;
    advSearchFunc.selectedProperties.push(def);

    // 2: as Object
    //advSearchFunc.selectedProperties.push({
    //    propertyType: propertyType,
    //    binaryOperators: binaryOperators,
    //    logicalOperators: logicalOperators,
    //    uid: uid,
    //    propertyId: propertyId,
    //    propertyName: propertyName,
    //    valueFrom: valueFrom,
    //    valueTo: valueTo,
    //    propertyName: propertyName,
    //    valueFrom_orig: valueFrom_orig,
    //    valueTo_orig: valueTo_orig,
    //    unitId: unitId,
    //    unitName: unitName
    //}
    //    )
}




advSearchFunc.propertyCondition = function () {
    this.propertyID = -1;
    this.x_label = '';
    this.unitGroup = -1;
}


advSearchFunc.propertyConditionModel = function () {
    this.uid = '';
    this.condition = new advSearchFunc.propertyCondition();
    this.logicalOperators = -1;
    this.selectedLogical = -1;
    this.valueFrom = '';
    this.valueTo = '';
    this.valueFrom_orig = '';
    this.valueTo_orig = '';
    this.unitId = -1;
    this.unitName = '';
}

advSearchFunc.selectedConditions = new Array();

advSearchFunc.addSelectedConditionModel = function (
    uid
    , propertyId
    , x_label
    , unitGroup
    , logicalOperators
    , selectedLogical
    , valueFrom
    , valueTo
    , valueFrom_orig
    , valueTo_orig
    , unitId
    , unitName
    ) {
    var def = new advSearchFunc.propertyConditionModel();
    def.uid = uid
    def.condition = new advSearchFunc.propertyCondition();
    def.condition.propertyID = propertyId;
    def.condition.x_label = x_label;
    def.condition.unitGroup = unitGroup;
    def.logicalOperators = logicalOperators;
    def.selectedLogical = selectedLogical;
    def.valueFrom = valueFrom;
    def.valueTo = valueTo;
    def.valueFrom_orig = valueFrom_orig;
    def.valueTo_orig = valueTo_orig;
    def.unitId = unitId;
    def.unitName = unitName;

    advSearchFunc.selectedConditions.push(def);
}


// Set visability for propery all elemens
advSearchFunc.setPropElementsVisability = function () {
    advSearchFunc.setBopVisability();
    advSearchFunc.setArrUpVisability();
    advSearchFunc.setArrDownVisability();
}

// Hide first Bop in div
advSearchFunc.setBopVisability = function () {
    $("#propItems").find('.onePropertyDef').each(function (index, element) {
        var uid = $(this).attr('id');
        if (index == 0) {
            $(this).find('#bop_' + uid).hide();
        } else {
            $(this).find('#bop_' + uid).show();
        }
    });
};

// Hide first Arrow Up in div
advSearchFunc.setArrUpVisability = function () {
    $("#propItems").find('.onePropertyDef').each(function (index, element) {
        var uid = $(this).attr('id');
        if (index == 0) {
            $(this).find('#arrup_' + uid).hide();
        } else {
            $(this).find('#arrup_' + uid).show();
        }
    });
};

// Hide last Arrow Down in div
advSearchFunc.setArrDownVisability = function () {
    var n = $("#propItems").find('.onePropertyDef').size();
    $("#propItems").find('.onePropertyDef').each(function (index, element) {
        var uid = $(this).attr('id');
        if (index == n - 1) {
            $(this).find('#arrdw_' + uid).hide();
        } else {
            $(this).find('#arrdw_' + uid).show();
        }
    });
};

// Check textboxes and unit selector visability
advSearchFunc.setEnteredValuesVisibility = function () {
    $("#propItems").find('.onePropertyDef').each(function (index, element) {

        if ($(this).attr('proptype') == 'Property') {
            var uid = $(this).attr('id');
            var $selector = $(this).find('.selectOperator:visible');

            if (typeof $selector !== "undefined") {
                var selectedVal = $($selector).find(":selected").val();
                if (typeof selectedVal != 'undefined') {
                    advSearchFunc.onSelectOperatorChanged($selector);
                };
            }
        }

    });
};

advSearchFunc.setConditionEnteredValuesVisibility = function () {
    $("#propItems").find('.onePropertyConditionDef').each(function (index, element) {

        if ($(this).attr('proptype') == 'Property') {
            var uid = $(this).attr('id');
            var $selector = $(this).find('.selectOperator:visible');

            if (typeof $selector !== "undefined") {
                var selectedVal = $($selector).find(":selected").val();
                if (typeof selectedVal != 'undefined') {
                    advSearchFunc.onSelectConditionOperatorChanged($selector);
                };
            }
        }

    });
};





// Count number of selected properties
advSearchFunc.countSelectedProperties = function () {
    return $('#advancedSearchBtns div#propItems').children().length;
}

advSearchFunc.checkSelectedProperties = function () {
    return advSearchFunc.countSelectedProperties() > 0;
}



// Fill advSearchFunc.selectedProperties with selected properties
advSearchFunc.fillSelectedProperties = function () {
    //var isChemical = searchFunc.getParameterByName('isChemical');
    var isChemical = $("#hdnFlegIsChemical").attr('isChemical');
    isChemical = (isChemical == "True" ? true : false);

    // Check for input values
    var isAllNumeric = true;
    $(".numeric:visible").each(function () {
        var enteredVal = jQuery.trim($(this).val());

        //if (!$.isNumeric(enteredVal)) {
        //    $(this).css("background", "red");
        //}

        isAllNumeric = isAllNumeric && $.isNumeric(enteredVal);
    });

    var enteredCounter = 0;
    $(".numericCondition:visible").each(function () {
        var enteredVal = jQuery.trim($(this).val());
        enteredCounter = enteredCounter + ($.isNumeric(enteredVal) ? 1 : 0);
        isAllNumeric = isAllNumeric && ($.isNumeric(enteredVal) || enteredVal == "");
    });


    if (!isAllNumeric || ($(".numericCondition:visible").length > 0 && enteredCounter == 0)) {
        alert("Field(s) must be numeric.");
        return;
    }

    advSearchFunc.selectedProperties = new Array();

    $("#propItems").find('.onePropertyDef').each(function () {
        // define propertyType for Property
        var propertyType = advSearchFunc.propertyType.Property;

        if ($(this).attr('proptype') == 'Material') {
            propertyType = advSearchFunc.propertyType.Material;
        }

        var uid = $(this).attr('id');

        // find binaryOperators
        var binaryOperators = advSearchFunc.binaryOperators.NotDefined;
        $(this).find('#bop_' + uid + ' :input').each(function (index, element) {
            if ($(element).prop('checked')) {
                switch (element.value.toString().toUpperCase()) {
                    case "AND":
                        binaryOperators = advSearchFunc.binaryOperators.And;
                        break;
                    case "OR":
                        binaryOperators = advSearchFunc.binaryOperators.Or;
                        break;
                    case "NOT":
                        binaryOperators = advSearchFunc.binaryOperators.Not;
                        break;
                    default:
                        binaryOperators = advSearchFunc.binaryOperators.NotDefined;
                        break;
                }
            }
        });

        // Read property id and name
        var propertyId = $(this).attr('propid');
        var propertyName = jQuery.trim($(this).find('#nm_' + uid).text());

        // Check if Condition is checked
        var isPropertyConditionsActive = $(this).find('#cond_' + uid).is(':checked');

        // find logicalOperators and entered values for value/interval
        var logicalOperators = advSearchFunc.logicalOperators.NotDefined;
        var valueFrom = '';
        var valueTo = '';
        var valueFrom_orig = '';
        var valueTo_orig = '';
        var unitId = -1;
        var unitName = '';
        var factor = 1;
        var offset = 0;

        if (propertyType == advSearchFunc.propertyType.Property) {

            var selectedValue = $(this).find('#lop_' + uid).find(":selected").val();
            switch (selectedValue.toString().toLowerCase()) {
                case "exists":
                    logicalOperators = advSearchFunc.logicalOperators.Exists;
                    break;

                case "equals":
                case "eq":
                    logicalOperators = advSearchFunc.logicalOperators.Eq;
                    valueFrom = $(this).find('.ivcDefinitions').find('.inputExactValue').val();
                    valueFrom_orig = $(this).find('.ivcDefinitions').find('.inputExactValue').val();

                    var $selectedUnit = $(this).find('#un_' + uid).find(":selected");
                    unitId = $selectedUnit.val();
                    unitName = $selectedUnit.attr('name');
                    factor = $selectedUnit.attr('factor');
                    offset = $selectedUnit.attr('offset');
                    //
                    valueFrom = parseFloat(valueFrom);
                    factor = typeof factor != 'undefined' ? parseFloat(factor) : 1;
                    offset = typeof offset != 'undefined' ? parseFloat(offset) : 0;
                    //
                    valueFrom = valueFrom * factor + offset;
                    break;

                case "lte":
                    logicalOperators = advSearchFunc.logicalOperators.Lte;
                    valueFrom = $(this).find('.ivcDefinitions').find('.inputExactValue').val();
                    valueFrom_orig = $(this).find('.ivcDefinitions').find('.inputExactValue').val();

                    var $selectedUnit = $(this).find('#un_' + uid).find(":selected");
                    unitId = $selectedUnit.val();
                    unitName = $selectedUnit.attr('name');
                    factor = $selectedUnit.attr('factor');
                    offset = $selectedUnit.attr('offset');
                    //
                    valueFrom = parseFloat(valueFrom);
                    factor = typeof factor != 'undefined' ? parseFloat(factor) : 1;
                    offset = typeof offset != 'undefined' ? parseFloat(offset) : 0;
                    //
                    valueFrom = valueFrom * factor + offset;
                    break;

                case "gte":
                    logicalOperators = advSearchFunc.logicalOperators.Gte;
                    valueFrom = $(this).find('.ivcDefinitions').find('.inputExactValue').val();
                    valueFrom_orig = $(this).find('.ivcDefinitions').find('.inputExactValue').val();

                    var $selectedUnit = $(this).find('#un_' + uid).find(":selected");
                    unitId = $selectedUnit.val();
                    unitName = $selectedUnit.attr('name');
                    factor = $selectedUnit.attr('factor');
                    offset = $selectedUnit.attr('offset');
                    //
                    valueFrom = parseFloat(valueFrom);
                    factor = typeof factor != 'undefined' ? parseFloat(factor) : 1;
                    offset = typeof offset != 'undefined' ? parseFloat(offset) : 0;
                    //
                    valueFrom = valueFrom * factor + offset;
                    break;

                case "between":
                    logicalOperators = advSearchFunc.logicalOperators.Between;
                    valueFrom = $(this).find('.ivcDefinitions').find('.inputFromValue').val();
                    valueTo = $(this).find('.ivcDefinitions').find('.inputToValue').val();
                    valueFrom_orig = $(this).find('.ivcDefinitions').find('.inputFromValue').val();
                    valueTo_orig = $(this).find('.ivcDefinitions').find('.inputToValue').val();

                    var $selectedUnit = $(this).find('#un_' + uid).find(":selected");
                    unitId = $selectedUnit.val();
                    unitName = $selectedUnit.attr('name');
                    factor = $selectedUnit.attr('factor');
                    offset = $selectedUnit.attr('offset');
                    //
                    valueFrom = parseFloat(valueFrom);
                    valueTo = parseFloat(valueTo);
                    factor = typeof factor != 'undefined' ? parseFloat(factor) : 1;
                    offset = typeof offset != 'undefined' ? parseFloat(offset) : 0;
                    //
                    valueFrom = valueFrom * factor + offset;
                    valueTo = valueTo * factor + offset;
                    break;

                default:
                    logicalOperators = advSearchFunc.logicalOperators.NotDefined;
                    break;
            }

            // read uniti definition
        }


        // Get Conditions
        advSearchFunc.selectedConditions = new Array();
        advSearchFunc.fillSelectedConditions(this);

        // add property to the list
        advSearchFunc.addSelectedProperty(
            propertyType
            , binaryOperators
            , logicalOperators
            , uid
            , propertyId
            , propertyName
            , valueFrom
            , valueTo
            , valueFrom_orig
            , valueTo_orig
            , unitId
            , unitName
            , isPropertyConditionsActive
            , advSearchFunc.selectedConditions
       );
    }); // the end of the main DIV loop

    // Prepare JSON  and run search
    //alert(JSON.stringify(advSearchFunc.selectedProperties));
    //if (advSearchFunc.selectedProperties.length > 0) {
    var filters = new Object();
    filters.AllFilters = advSearchFunc.selectedProperties;
    filters.SelectedSource = "0";
    filters.IsChemical = isChemical;


    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    console.log('filters; ', filters);
    ini.ajax.post(advSearchFunc.applyAdvSearchFiltershPath, ini.ajax.contentType.Json, { filters: filters },
               function (result) {
                   //$(".filtersPage").hide();
                   //alert('i');
                   //  $('#advSearchPropertiesContainer').hide();
                   // $('#advSearchResultsContainer').html(result.list);

                   //$('#divBC').html(result.breadcrumbs);
                   //    $('#advSearchResultsContainer').show();

                   // ProgressIndicator is removed on the result page!!! (AdvResultsContainer.cshtml)
                   //removeProgressIndicator('main');
                   //$('.progress-indicator').css('display', 'none');

                   window.location.href = result.url;
               },
               function (error) {
               }
           );
    //} else {
    //    //alert("Please enter advanced search parameters.");
    //}
    //window.location.href = searchFunc.fullTextSearchPath + '?filter=' + searchText;

}


// Fill advSearchFunc.selectedConditions with selected conditions
advSearchFunc.fillSelectedConditions = function (propertyContainer) {
    var uid = $(propertyContainer).attr('id');

    $("#" + uid).find('.onePropertyConditionDef').each(function () {
        var cid = $(this).attr('id');

        // Read property id and condition name
        var propertyId = $(this).attr('propid');
        var x_label = jQuery.trim($(this).find('#nmc_' + cid).text());
        var unitGroup = -1;

        // find selectedLogical and entered values for value/interval
        var logicalOperators = new Array();
        var selectedLogical = advSearchFunc.logicalOperators.NotDefined;
        var valueFrom = '';
        var valueTo = '';
        var valueFrom_orig = '';
        var valueTo_orig = '';
        var unitId = -1;
        var unitName = '';
        var factor = 1;
        var offset = 0;


        var selectedValue = $(this).find('#lopc_' + cid).find(":selected").val();
        switch (selectedValue.toString().toLowerCase()) {
            /*
            case "exists":
                selectedLogical = advSearchFunc.logicalOperators.Exists;
                break;

            case "equals":
            case "eq":
                selectedLogical = advSearchFunc.logicalOperators.Eq;
                valueFrom = $(this).find('.ivcDefinitions').find('.inputExactValue').val();
                valueFrom_orig = $(this).find('.ivcDefinitions').find('.inputExactValue').val();

                var $selectedUnit = $(this).find('#un_' + cid).find(":selected");
                unitId = $selectedUnit.val();
                unitName = $selectedUnit.attr('name');
                factor = $selectedUnit.attr('factor');
                offset = $selectedUnit.attr('offset');
                //
                valueFrom = parseFloat(valueFrom);
                factor = typeof factor != 'undefined' ? parseFloat(factor) : 1;
                offset = typeof offset != 'undefined' ? parseFloat(offset) : 0;
                //
                valueFrom = valueFrom * factor + offset;
                break;
            */

            case "lte":
                selectedLogical = advSearchFunc.logicalOperators.Lte;
                valueFrom = $(this).find('.ivcConditionDefinitions').find('.inputConditionExactValue').val();
                valueFrom_orig = $(this).find('.ivcConditionDefinitions').find('.inputConditionExactValue').val();

                var $selectedUnit = $(this).find('#unc_' + cid).find(":selected");
                unitId = $selectedUnit.val();
                unitName = $selectedUnit.attr('name');
                factor = $selectedUnit.attr('factor');
                offset = $selectedUnit.attr('offset');
                //
                valueFrom = parseFloat(valueFrom);
                factor = typeof factor != 'undefined' ? parseFloat(factor) : 1;
                offset = typeof offset != 'undefined' ? parseFloat(offset) : 0;
                //
                factor = 1;
                offset = 0;
                valueFrom = valueFrom * factor + offset;
                break;

            case "gte":
                selectedLogical = advSearchFunc.logicalOperators.Gte;
                valueFrom = $(this).find('.ivcConditionDefinitions').find('.inputConditionExactValue').val();
                valueFrom_orig = $(this).find('.ivcConditionDefinitions').find('.inputConditionExactValue').val();

                var $selectedUnit = $(this).find('#unc_' + cid).find(":selected");
                unitId = $selectedUnit.val();
                unitName = $selectedUnit.attr('name');
                factor = $selectedUnit.attr('factor');
                offset = $selectedUnit.attr('offset');
                //
                valueFrom = parseFloat(valueFrom);
                factor = typeof factor != 'undefined' ? parseFloat(factor) : 1;
                offset = typeof offset != 'undefined' ? parseFloat(offset) : 0;
                //
                factor = 1;
                offset = 0;
                valueFrom = valueFrom * factor + offset;
                break;

            case "between":
                selectedLogical = advSearchFunc.logicalOperators.Between;
                valueFrom = $(this).find('.ivcConditionDefinitions').find('.inputConditionFromValue').val();
                valueTo = $(this).find('.ivcConditionDefinitions').find('.inputConditionToValue').val();
                valueFrom_orig = $(this).find('.ivcConditionDefinitions').find('.inputConditionFromValue').val();
                valueTo_orig = $(this).find('.ivcConditionDefinitions').find('.inputConditionToValue').val();

                var $selectedUnit = $(this).find('#unc_' + cid).find(":selected");
                unitId = $selectedUnit.val();
                unitName = $selectedUnit.attr('name');
                factor = $selectedUnit.attr('factor');
                offset = $selectedUnit.attr('offset');
                //
                valueFrom = parseFloat(valueFrom);
                valueTo = parseFloat(valueTo);
                factor = typeof factor != 'undefined' ? parseFloat(factor) : 1;
                offset = typeof offset != 'undefined' ? parseFloat(offset) : 0;
                //
                factor = 1;
                offset = 0;
                valueFrom = valueFrom * factor + offset;
                valueTo = valueTo * factor + offset;
                break;

            default:
                selectedLogical = advSearchFunc.logicalOperators.NotDefined;
                break;
        }


        advSearchFunc.addSelectedConditionModel(
            cid
            , propertyId
            , x_label
            , unitGroup
            , logicalOperators
            , selectedLogical
            , valueFrom
            , valueTo
            , valueFrom_orig
            , valueTo_orig
            , unitId
            , unitName
        );

    });
}



advSearchFunc.clearSelectedProperties = function () {

    ini.ajax.post(advSearchFunc.clearAdvSearchFiltershPath, ini.ajax.contentType.Json, null,
                   function (result) {
                       //$(".filtersPage").hide();
                       //alert('i');
                       $('#divConditions').html(result);
                   },
                   function (error) {
                   }
               );

}


advSearchFunc.searchByPropertiesAndSource = function () {
    // Prepare JSON  and run search
    var filters = new Object();
    filters.AllFilters = advSearchFunc.selectedProperties;

    // read selected source from ddl
    var source = "";
    var selectedSourceId = "";
    var selectedSourceDatabookId = "";
    //var selectedSource = $('#sourcesSelection :selected').val().split('#');
    var selectedSource = $('#sourcesSelection :selected').val();
    if (typeof selectedSource === "undefined") {
        source = '0';
    } else {
        selectedSource = selectedSource.split('#');
        var selectedSourceId = selectedSource[0];
        var selectedSourceDatabookId = selectedSource[1];

        if (selectedSourceId != undefined) {
            source = selectedSourceId;
        }
        if (selectedSourceDatabookId != undefined) {
            source = selectedSourceId + "," + selectedSourceDatabookId;
        }
    }

    filters.SelectedSource = source;
    console.log('Filters;', filters);

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(advSearchFunc.applyAdvSearchSourcePath, ini.ajax.contentType.Json, { filters: filters },
               function (result) {
                   window.location.href = result.url;
               },
               function (error) {
               }
           );
}

//
/////////////////////// /Definitions for selected properties


advSearchFunc.addProperty = function (id) {
}

advSearchFunc.removeProperty = function (id) {
}

advSearchFunc.onSelectOperatorChanged = function (selector) {
    var selectedVal = $(selector).find(":selected").val().toString();

    // input one value
    //var val = '<input type="text" class="inputExactValue" style="width:70px;">';

    // input range of values (between)
    //var valr = '<input type="text" class="inputFromValue" style="width:70px;">';
    //valr += '<label class="browseFacetContent"> And </label>';
    //valr += '<input type="text" class="inputToValue" style="width:70px;">';

    var $inputSelector = $(selector).parent().closest('div').find(".ivcDefinitions");
    //
    $($inputSelector).find('.inputExactValue').hide();
    //
    $($inputSelector).find('.inputFromValue').hide();
    $($inputSelector).find('.inputToValueLabel').hide();
    $($inputSelector).find('.inputToValue').hide();

    var uid = $(selector).parent().closest('div').attr('id');

    switch (selectedVal.toLowerCase()) {
        case "exists":
            //$(selector).parent().closest('div').find(".ivcDefinitions").text('');
            $(selector).parent().closest('div').find("#un_" + uid).hide();
            break;
        case "equals":
        case "eq":
            //$(selector).parent().closest('div').find(".ivcDefinitions").html(val);
            $($inputSelector).find('.inputExactValue').show();

            if ($(selector).parent().closest('div').find("#un_" + uid + " option").size() > 0) {
                $(selector).parent().closest('div').find("#un_" + uid).show();
            } else {
                $(selector).parent().closest('div').find("#un_" + uid).hide();
            }
            break;
        case "gte":
            //$(selector).parent().closest('div').find(".ivcDefinitions").html(val);
            $($inputSelector).find('.inputExactValue').show();

            if ($(selector).parent().closest('div').find("#un_" + uid + " option").size() > 0) {
                $(selector).parent().closest('div').find("#un_" + uid).show();
            } else {
                $(selector).parent().closest('div').find("#un_" + uid).hide();
            }
            break;
        case "lte":
            //$(selector).parent().closest('div').find('.ivcDefinitions').html(val);
            $($inputSelector).find('.inputExactValue').show();
            if ($(selector).parent().closest('div').find("#un_" + uid + " option").size() > 0) {
                $(selector).parent().closest('div').find("#un_" + uid).show();
            } else {
                $(selector).parent().closest('div').find("#un_" + uid).hide();
            }
            break;
        case "between":
            //$(selector).parent().closest('div').find(".ivcDefinitions").html(valr);
            $($inputSelector).find('.inputFromValue').show();
            $($inputSelector).find('.inputToValueLabel').show();
            $($inputSelector).find('.inputToValue').show();

            if ($(selector).parent().closest('div').find("#un_" + uid + " option").size() > 0) {
                $(selector).parent().closest('div').find("#un_" + uid).show();
            } else {
                $(selector).parent().closest('div').find("#un_" + uid).hide();
            }
            break;
    }
}



advSearchFunc.onSelectConditionOperatorChanged = function (selector) {
    var selectedVal = $(selector).find(":selected").val().toString();

    var $inputSelector = $(selector).parent().closest('div').find(".ivcConditionDefinitions");
    //
    $($inputSelector).find('.inputConditionExactValue').hide();
    //
    $($inputSelector).find('.inputConditionFromValue').hide();
    $($inputSelector).find('.inputConditionToValueLabel').hide();
    $($inputSelector).find('.inputConditionToValue').hide();

    var uid = $(selector).parent().closest('div').attr('id');

    switch (selectedVal.toLowerCase()) {
        case "exists":
            //$(selector).parent().closest('div').find(".ivcConditionDefinitions").text('');
            $(selector).parent().closest('div').find("#unc_" + uid).hide();
            break;
        case "equals":
        case "eq":
            //$(selector).parent().closest('div').find(".ivcConditionDefinitions").html(val);
            $($inputSelector).find('.inputConditionExactValue').show();

            if ($(selector).parent().closest('div').find("#unc_" + uid + " option").size() > 0) {
                $(selector).parent().closest('div').find("#unc_" + uid).show();
            } else {
                $(selector).parent().closest('div').find("#unc_" + uid).hide();
            }
            break;
        case "gte":
            //$(selector).parent().closest('div').find(".ivcConditionDefinitions").html(val);
            $($inputSelector).find('.inputConditionExactValue').show();

            if ($(selector).parent().closest('div').find("#unc_" + uid + " option").size() > 0) {
                $(selector).parent().closest('div').find("#unc_" + uid).show();
            } else {
                $(selector).parent().closest('div').find("#unc_" + uid).hide();
            }
            break;
        case "lte":
            //$(selector).parent().closest('div').find('.ivcConditionDefinitions').html(val);
            $($inputSelector).find('.inputConditionExactValue').show();
            if ($(selector).parent().closest('div').find("#unc_" + uid + " option").size() > 0) {
                $(selector).parent().closest('div').find("#unc_" + uid).show();
            } else {
                $(selector).parent().closest('div').find("#unc_" + uid).hide();
            }
            break;
        case "between":
            //$(selector).parent().closest('div').find(".ivcConditionDefinitions").html(valr);
            $($inputSelector).find('.inputConditionFromValue').show();
            $($inputSelector).find('.inputConditionToValueLabel').show();
            $($inputSelector).find('.inputConditionToValue').show();

            if ($(selector).parent().closest('div').find("#unc_" + uid + " option").size() > 0) {
                $(selector).parent().closest('div').find("#unc_" + uid).show();
            } else {
                $(selector).parent().closest('div').find("#unc_" + uid).hide();
            }
            break;
    }
}