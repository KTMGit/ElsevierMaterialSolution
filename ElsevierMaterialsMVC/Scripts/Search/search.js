;
var searchFunc = {}



var classificationId = null;
var classificationTypeId = null;
var showPropertiesFilters = null;

function ShowMessage(success) {
    if (success == 1) {
        showPopup({
            width: 300,
            height: 60,
            title: "",
            close: function () {
                //function on close
            }
        }, "Successfully added to comparison!");
        removeProgressIndicator('main');
        $('.progress-indicator').css('display', 'none');
    }
    else if (success == 3) {
        showPopup({
            width: 400,
            height: 100,
            title: "",
            close: function () {
                //function on close   
            }
        }, "You have already added the maximum number of 12 curves for selected property to Comparison! Please remove at least one curve from Comparison before selecting a new curve to add.");
        removeProgressIndicator('main');
        $('.progress-indicator').css('display', 'none');
    }
    else {
        showPopup({
            width: 400,
            height: 100,
            title: "",
            close: function () {
                //function on close
            }
        }, "You have already added the maximum number of 5 materials to Comparison! Please remove at least one material from Comparison before selecting a new material to add.");
        removeProgressIndicator('main');
        $('.progress-indicator').css('display', 'none');
    }
}




function addChemicalToComparison() {

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(comparisonAddChemicalPath, ini.ajax.contentType.Json, {
        materialId: selectedMaterialId,
        subgroupId: selectedSubgroupId,
        conditionId: selectedChemicalConditionId,
        propertyId: selectedChemicalPropertyId

    },
         function (data) {
             ShowMessage(data.success);
         },
        function (error) {

        }
    );
}
function addPhysicalToComparison(conditionId) {

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(comparisonAddPhysicalPath, ini.ajax.contentType.Json, {
        materialId: selectedMaterialId,
        sourceMaterialId: selectedSourceMaterialId,
        sourceId: selectedSourceId,
        subgroupId: selectedSubgroupId,
        conditionId: conditionId,
        sourcePropertyId: selectedSourcePropertyIdDiagram,
        propertyId: selectedPropertyIdDiagram

    },
         function (data) {
             ShowMessage(data.success);
         },
        function (error) {

        }
    );
}

function addMechanicalToComparison(conditionId) {

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(comparisonAddMechanicalPath, ini.ajax.contentType.Json, {
        materialId: selectedMaterialId,
        sourceMaterialId: selectedSourceMaterialId,
        sourceId: selectedSourceId,
        subgroupId: selectedSubgroupId,
        conditionId: conditionId,
        sourcePropertyId: selectedMechSourcePropertyIdDiagram,
        propertyId: selectedMechPropertyIdDiagram
    },
         function (data) {
             ShowMessage(data.success);
         },
        function (error) {

        }
    );
}

function addFatigueToComparison(selectedCurveName) {


    var inputValue = $('input[name=fatigueStrainStress]').val();
    var selectedMaterialConditionId = $("#selectMaterialCondFatigueStrain").val();
    var selectedTestConditionId = $("#selectCondFatigueStrain").val();
    selectedCurveName = $("#curveName1").html();

    if (stressChecked == true) {
        selectedMaterialConditionId = $("#selectMaterialCondFatigueStress").val();
        selectedTestConditionId = $("#selectCondFatigueStress").val();
        selectedCurveName = $("#curveName2").html();
    }



    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(comparisonAddFatiguePath, ini.ajax.contentType.Json, {
        materialId: selectedMaterialId,
        sourceMaterialId: selectedSourceMaterialId,
        sourceId: selectedSourceId,
        subgroupId: selectedSubgroupId,
        materialConditionId: selectedMaterialConditionId,
        testConditionId: selectedTestConditionId,
        selectedCurveName: selectedCurveName
    },
         function (data) {

             ShowMessage(data.success);
         },
        function (error) {

        }
    );
}

function addStressStrainToComparison(type) {

    var addForAllTemperatues = $("#addToComparisonAllTemperaturesCurves").prop("checked");
    var selectedMaterialConditionId = $("#selectedMaterialConditionSS").val();
    var selectedTestConditionId = $("#selectCondSS").val();

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(comparisonAddStressStrainPath, ini.ajax.contentType.Json, {
        materialId: selectedMaterialId, subgroupId: selectedSubgroupId, sourceId: selectedSourceId, sourceMaterialId: selectedSourceMaterialId, temperature: selectedTemperatureStressStrain, materialConditionId: selectedMaterialConditionId,
        testConditionId: selectedTestConditionId, addForAllTemperatues: addForAllTemperatues
    },
         function (data) {
             ShowMessage(data.success);
         },
        function (error) {
        }
    );
}


searchFunc.fullTextSearch = function (status) {
    // status:
    // undefined - (default) direct from the search form; transform it to 0!
    // 1         - from show/hide filters (from searchFunc.applyColumnsSelector())
    var status = ('undefined' == typeof status ? 0 : status);

    var searchText = $('#tbFullTexSearch').val();
    var clId = $('#classificationId').val();
    var clTypeId = $('#classificationTypeId').val();
    if (searchText.length < 2) {
        alert('Please enter search criteria');
    }
    else {
        // Reset saved filters
        if ('undefined' !== typeof searchFunc.columnsStatus && status == 0) {
            // first, reset column filters on the client
            $('#columnsSelectorId').children('input[id^="tb_"]').val('');
            jQuery.each(searchFunc.columnsStatus, function (index, item) {
                item.Filter = '';
            });
            // ... and save it on the server
            var filters = new Object();
            filters.AllFilters = searchFunc.columnsStatus;
            ini.ajax.post(searchFunc.applyFiltersColumnsPath, ini.ajax.contentType.Json, { filters: filters },
                function (result) {
                },
                function (error) {
                });
        }

        // Continue with the search
        var $progressElement = $('#fullTextSearch');
        displayProgressIndicator('main');
        $('.progress-indicator').css('display', 'block');
        window.location.href = searchFunc.fullTextSearchPath + '?filter=' + searchText;
        removeProgressIndicator('main');
    }
}


searchFunc.getEquivalenceMaterials = function (materialId, filter) {

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    window.location.href = searchFunc.subgroupsPath + '?materialId=' + materialId + '&filter=' + filter + '&fromBrowse=' + $('#fromBrowse').val();


}

searchFunc.getEquivalenceMaterialsAdv = function (materialId, filter) {

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    window.location.href = searchFunc.subgroupsPath + '?materialId=' + materialId + '&filter=' + filter + '&fromAdvanced=true';


}

searchFunc.getEquivalenceMaterialsBySourceId = function (sourceMaterialId) {
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    window.location.href = searchFunc.subgroupsBySourceIdPath + '?sourceId=2&sourceMaterialId=' + sourceMaterialId;
}


searchFunc.browseMaterials = function (selectedNodeId) {
    //displayProgressIndicator('main');
    //$('.progress-indicator').css('display', 'block');
    //var typeId = $("#" + selectedNodeId).attr("nodetype");

    //var url = searchFunc.browseSearchPath + '?ClasificationId=' + selectedNodeId + '&ClasificationTypeId=' + typeId + '&fromBrowse=true' + '&Source=0';
    //window.location.href = url;

    var typeId = $("#" + selectedNodeId).attr("nodetype");
    searchFunc.browseCommonWithFilters(selectedNodeId, typeId, 'true');
}

searchFunc.browseMaterialsWithFilters = function () {
    // Get parameters from url
    // ?ClasificationId=727&ClasificationTypeId=2&fromBrowse=true&Source=0
    var selectedNodeId = searchFunc.getParameterByName('ClasificationId');
    var typeId = $("#" + selectedNodeId).attr("nodetype");
    var fromBrowse = searchFunc.getParameterByName('fromBrowse');
    if (typeof fromBrowse === "undefined") {
        fromBrowse = 'true';
    }

    //// Get source
    //var source = "";
    //var selectedSourceId = "";
    //var selectedSourceDatabookId = "";
    //var selectedSource = $('#sourcesSelection :selected').val().split('#');
    //var selectedSourceId = selectedSource[0];
    //var selectedSourceDatabookId = selectedSource[1];

    //if (selectedSourceId != undefined) {
    //    source = selectedSourceId;
    //}
    //if (selectedSourceDatabookId != undefined) {
    //    source = selectedSourceId + "," + selectedSourceDatabookId;
    //}

    //var url = searchFunc.browseSearchPath + '?ClasificationId=' + selectedNodeId + '&ClasificationTypeId=' + typeId + '&fromBrowse=' + fromBrowse + '&Source=' + source;
    //window.location.href = url;

    searchFunc.browseCommonWithFilters(selectedNodeId, typeId, fromBrowse);
}

searchFunc.browseCommonWithFilters = function (selectedNodeId, typeId, fromBrowse) {
    displayProgressIndicator('main');

    // Get source
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

    var url = searchFunc.browseSearchPath + '?ClasificationId=' + selectedNodeId + '&ClasificationTypeId=' + typeId + '&fromBrowse=' + fromBrowse + '&Source=' + source;
    window.location.href = url;
}


var globalWidth = 50;

/*SR  Page*/

function InitResizableContent() {

    var cw = $('#mainContent').width();
    var filtersWidth = $("#resizable").width() + 100;
    var resultsWidth = cw - filtersWidth - 15;

    $("#resultsResizable").width(resultsWidth);
    $(".corner-wrapper").width(resultsWidth);


    $('#resizable').resizable({

        //handles: "n, e, s, w, nw, ne, sw,se",
        resize: function (event, ui) {

            //ui.size.height = ui.originalSize.height;

            var cw = $('#mainContent').width();

            var filtersWidth = $("#resizable").width() + 100;

            globalWidth = filtersWidth;
            var resultsWidth = cw - filtersWidth - 15;

            $("#resultsResizable").width(resultsWidth);
            $(".corner-wrapper").width(resultsWidth);

            var percentage = filtersWidth / initialWidthOfContainer;
            if (percentage < 0.4) {
                $("#ulTabsSearch").hide();

            } else {
                $("#ulTabsSearch").show();
            }
            if (percentage < 0.2) {
                $(".filtersContainerHeader").hide();

            } else {
                $(".filtersContainerHeader").show();
            }

            $("#sourcesSelection").width(sourcesWidth * percentage);

        }
    });
}
/*SR  Page*/
function resizeFilters(filtersWidth) {

    var cw = $('#mainContent').width();
    var resultsWidth = cw - filtersWidth - 15;
    $("#resultsResizable").width(resultsWidth);
    $(".corner-wrapper").width(resultsWidth);
    $("#divResizableFiltersOpen").position(0, 0);

}
function openFiltersResizableContainer() {

    $("#divResizableFiltersOpen").hide();
    $("#divResizableFiltersClose").show();
    $("#resizable").show();
    $(".ui-icon-gripsmall-diagonal-se").show();

    resizeFilters(globalWidth);
}

function closeFiltersResizableContainer() {

    globalWidth = $("#resizable").width();


    $("#divResizableFiltersClose").hide();
    $("#divResizableFiltersOpen").show();
    $("#resizable").hide();
    $(".ui-icon-gripsmall-diagonal-se").hide();

    resizeFilters(10);



}

function openFiltersContainer() {
    //$("#menageFiltersVisibility").css("background-color", "#dfdfdf");
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    $('#menageColumnsVisibility').hide();
    var filtersPage = $('#resizable').html().trim();
    if (filtersPage != '') {
        openFiltersResizableContainer();

        $("#resultsResizable").css("margin-left", "8px");
        $("#resultsResizable").css("margin-top", "10px");

        $(".ui-icon-gripsmall-diagonal-se").show();
        $('#resizable').show();
        removeProgressIndicator('main');
        $('.progress-indicator').css('display', 'none');
    }
    else {
        ini.ajax.post(searchFunc.fillSearchConditionItems, ini.ajax.contentType.Json, null,
            function (success) {

                openFiltersResizableContainer();

                $("#resultsResizable").css("margin-left", "8px");
                $("#resultsResizable").css("margin-top", "10px");

                $(".ui-icon-gripsmall-diagonal-se").show();
                $("#resizable").html(success.dataSearch)
                //resizeFilters(400);
                //InitResizableContent();
                $('#divPropAdvancedSearch').html(success.dataAdv);
                if (success.showTitle) {
                    $('#divSearchedPropertiesTitle').show();
                    searchProp.setPropElementsVisability();
                    searchProp.setEnteredValuesVisibility();
                }



                //    resizeFilters(300);
                removeProgressIndicator('main');
                $('.progress-indicator').css('display', 'none');
            },
             function (error) {

             }
         );
    }








}

function closeFiltersContainer() {

    $("#resultsResizable").css("margin-left", "0px");
    $("#resultsResizable").css("margin-top", "0px");

    //$("#menageFiltersVisibility").css("background-color", "");

    $("#menageColumnsVisibility").show();
    globalWidth = $("#resizable").width();


    $("#divResizableFiltersClose").hide();
    $("#divResizableFiltersOpen").show();
    $("#resizable").hide();
    $(".ui-icon-gripsmall-diagonal-se").hide();

    resizeFilters(10);

}

function openSubgroupFiltersContainer() {
    $(".filtersPage").show();
    $("#closeFilters").show();
    $("#openFilters").hide();
    $("#materialsSubgroupList").removeAttr("style");
    $("#materialsSubgroupList").attr("style", "width:665px;");
    $("#filtersSearch").attr("style", "margin-right:10px;");


    //$(".corner-wrapper").removeAttr("style");
    //$(".corner-wrapper").attr("style", "width:665px");

    $("#divResizableSubgroupFiltersOpen").hide();
    $("#openSubgroupColumnsSelectorId").hide();

}

function closeSubgroupFiltersContainer() {
    $(".filtersPage").hide();
    $("#closeFilters").hide();
    $("#openFilters").show();
    $("#materialsSubgroupList").removeAttr("style");
    $("#materialsSubgroupList").attr("style", "width:950px");
    $("#filtersSearch").removeAttr("style");
    //$(".corner-wrapper").removeAttr("style");
    //$(".corner-wrapper").attr("style", "width:950px");
    $("#divResizableSubgroupFiltersOpen").show();
    $("#openSubgroupColumnsSelectorId").show();


}
var classificationsSearch = "";
function applyFilters() {
    searchProp.fillSelectedProperties();
}


searchFunc.goToBrowsePage = function (hasFiltersSearched) {

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');


    window.location.href = browsePath + '?HasSearchFilters=' + hasFiltersSearched;





}

searchFunc.goToSearchResults = function (hasFiltersSearched) {
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');


    window.location.href = searchResultsPath + '?HasSearchFilters=' + hasFiltersSearched;
}




//searchFunc.removeFilterAdvancedSearch = function (filterId, filterType) {


//    //ini.ajax.post(searchFunc.removeFiltersPathAdvancedSearch, ini.ajax.contentType.Json, {
//    //    filterId: filterId
//    //      },
//    //        function (result) {

//    //            window.location.href = result.url;
//    //                },
//    //        function (error) {
//    //        });



//    window.location.href = searchFunc.removeFiltersPathAdvancedSearch + '?filterId=' + filterId;


//}

searchFunc.removeFilter = function (filterId, filterType) {

    //If Filterspage does not exists
    var filtersPage = $('#resizable').html().trim();
    if (filtersPage == '') {

        ini.ajax.post(searchFunc.fillSearchConditionItems, ini.ajax.contentType.Json, null,
                    function (success) {


                        $("#resizable").html(success.dataSearch)

                        $('#divPropAdvancedSearch').html(success.dataAdv);
                        if (success.showTitle) {
                            $('#divSearchedPropertiesTitle').show();
                            searchProp.setPropElementsVisability();
                            searchProp.setEnteredValuesVisibility();
                        }
                        removeProgressIndicator('main');
                        $('.progress-indicator').css('display', 'none');

                        if (filterType == 1) {
                            searchFunc.removeFilterFromTree(filterId);
                            $("#class_" + filterId).remove();
                        }

                        if (filterType == 2) {
                            $("div[propid='" + filterId + "']").remove();
                            searchProp.setPropElementsVisability();
                            $("#prop_" + filterId).remove();
                        }

                        if (filterType == 3) {
                            $("#selectedSource").hide();
                            $('#sourcesSelection').val("0");
                            $("#selectedSource").remove();
                        }






                        searchProp.fillSelectedProperties();

                    },
                     function (error) {

                     }
                 );
    }
    else {
        if (filterType == 1) {
            searchFunc.removeFilterFromTree(filterId);
            $("#class_" + filterId).remove();
        }

        if (filterType == 2) {
            $("div[propid='" + filterId + "']").remove();
            searchProp.setPropElementsVisability();
            $("#prop_" + filterId).remove();
        }

        if (filterType == 3) {
            $("#selectedSource").hide();
            $('#sourcesSelection').val("0");
            $("#selectedSource").remove();
        }

        searchProp.fillSelectedProperties();
    }






}

searchFunc.removeFilterFromTree = function (id) {
    var aLink = $("a[id$=" + id + "]");

    //var aLink = $('#link_TYPE_' +id);
    if (aLink.attr('treeClicked') != undefined) {
        aLink.css("background-color", "white");
        aLink.css("color", "rgb(119,119,119)");
        aLink.removeAttr('treeClicked', id);
    } else {
        //aLink.css("background-color", "rgb(255,130,0)");
        //aLink.css("color", "rgb(249,249,249)");
        //aLink.attr('treeClicked', id);
    }
}

function resetFilters() {
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    var fullTextSearch = $("#tbFullTexSearch").val();
    var $prop = $('#containerProperty').find("a.jstree-clicked");
    var $mats = $('#containerMaterial').find("a.jstree-clicked");
    var classificationsSearch = "";

    var searchFilters = new Object();
    searchFilters.ClasificationId = 0;
    searchFilters.ClasificationTypeId = 0;

    searchFilters.filter = fullTextSearch;


    ini.ajax.post(searchFunc.applyFiltershPath, ini.ajax.contentType.Json, {
        advFilters: null, classIds: classificationsSearch, filters: searchFilters, reset: 1
    },
               function (result) {
                   removeProgressIndicator('main');
                   $('.progress-indicator').css('display', 'none');
                   $('#divPropAdvancedSearch').html('');
                   $prop.css("background-color", "white");
                   $prop.css("color", "rgb(119,119,119)");
                   $prop.removeAttr('treeClicked');
                   $mats.css("background-color", "white");
                   $mats.css("color", "rgb(119,119,119)");
                   $mats.removeAttr('treeClicked');




                   $('#resultsResizable').html(result);

               },
               function (error) {
               }
           );

}

function applySubgroupFilters(materialId) {

    var selectedSource = $("#sources").val();
    var selectedSourceId = selectedSource.split('#')[0];
    var selectedStandard = $("#standards").val();
    if (selectedStandard == "0") {
        selectedStandard = "";
    }
    var fullTextSearch = $("#tbFullTexSearch").val();
    var specification = $("#SpecificationId").val();


    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(searchFunc.applySubgroupFiltershPath, ini.ajax.contentType.Json, {
        Specification: specification, SourceId: selectedSourceId, StandardId: selectedStandard, filter: fullTextSearch, MaterialId: materialId, Source: selectedSource
    },
               function (success) {
                   $(".filtersPage").hide();
                   $('#divSubgroupsList').html(success);

                   $("#menageColumnsSubgroupVisibility").css("background-color", "");
                   $('#openSubgroupColumnsSelectorId').show();
                   $('#closeSubgroupColumnsSelectorId').hide();
                   $('#menageColumnsFiltersVisibility').show();
                   $(".columnsSelector").hide();

                   removeProgressIndicator('main');
                   $('.progress-indicator').css('display', 'none');
               },
               function (error) {

               }
           );
}

function resetSubgroupFilters(materialId) {
    var fullTextSearch = $("#tbFullTexSearch").val();
    ini.ajax.post(searchFunc.applySubgroupFiltershPath, ini.ajax.contentType.Json, {
        Specification: '', SourceId: 0, StandardId: '0', filter: fullTextSearch, MaterialId: materialId, Source: '0'
    },
              function (success) {
                  $(".filtersPage").hide();

                  $('#divSubgroupsList').html(success);
                  $(".columnsSelector").hide();
                  removeProgressIndicator('main');
                  $('.progress-indicator').css('display', 'none');
              },
               function (error) {

               }
           );
}





function GetCheckedProperties() {

    var properties = [];

    $("#groupsContainer").find("input[type=checkbox]:checked").each(function () {
        if ($(this).attr("modul") == undefined || $(this).attr("modul") == "regular") {
            if ($(this).attr("hasdiagram") == undefined || $(this).attr("hasdiagram") == null) {
                var property = {};
                property.TypeId = $(this).attr("typeid");
                property.GroupId = $(this).attr("groupid");
                property.ConditionId = $('#selectCond_' + property.GroupId).val();
                if (property.ConditionId === undefined) {
                    property.ConditionId = $(this).attr("conditionid");
                }
                property.RowId = $(this).attr("rowid");
                property.SourceTypeId = $(this).attr("sourcepropertyid");

                properties.push(property);

            }

        }
    });

    return properties;
}

function hasRegular() {

    var counter = 0;
    var hasReg = false;

    $("#groupsContainer").find("input[type=checkbox]:checked").each(function () {
        if ($(this).attr("modul") == undefined || $(this).attr("modul") == "regular") {
            if ($(this).attr("hasdiagram") == undefined || $(this).attr("hasdiagram") == null) {
                counter += 1;
            }
        }
    });

    if (counter > 0) {
        hasReg = true;
    }
    return hasReg;
}

function hasStressStrain() {

    var counter = 0;
    var hasSS = false;
    $("#groupsContainer").find("input[type=checkbox]:checked").each(function () {
        if ($(this).attr("modul") == "stress_strain") {
            counter += 1;
        }
    });

    if (counter > 0) {
        hasSS = true;
    }
    return hasSS;
}

function checkCheckedStressStrain() {
    $("#groupsContainer").find("input[type=checkbox]:checked").each(function () {
        if ($(this).attr("modul") == "stress_strain") {


            var arrayTemperatures = [];
            for (var i = 0; i < SStemperatures.length; i++) {
                arrayTemperatures.push(SStemperatures[i].Temperature);
            }

            if (arrayTemperatures.length > 1) {
                displayProgressIndicator('main');
                $('.progress-indicator').css('display', 'block');


                window.hasPopup = true;
                ini.ajax.post(comparisonSSSelectTemperatures, ini.ajax.contentType.Json, {
                    arrayTemperatures: arrayTemperatures
                },
                    function (data) {
                        $('#SSSelectTemperatures').html(data.data);
                        showPopup({
                            width: 500,
                            height: 250,
                            title: "",
                            close: function () {
                                window.hasPopup = false;
                            }
                        }, $('#SSSelectTemperatures'), $('#SSSelectTemperaturesContainer'));
                    },
                    function (error) { }
                );
                removeProgressIndicator('main');
                $('.progress-indicator').css('display', 'none');
            }

        }
    });
}

function addSelectedSSTemperaturesToComparison() {
    $("#SSselectedTemperaturesForComparison").find("input[type=checkbox]:checked").each(function () {
        window.selectedTemp.push($(this).attr("temp"));
        if (window.hasPopup) {
            hidePopup();
        }
    });

    selectedTemperaturesSS = window.selectedTemp;
    var selectedMaterialConditionId = $("#selectedMaterialConditionSS").val();
    var selectedTestConditionId = $("#selectCondSS").val();
    addSSToComparison(selectedTemperaturesSS, selectedMaterialConditionId, selectedTestConditionId);
}


function addToComparison(materialId, subgroupId, sourceId, sourceMaterialId) {
    var searchText = $('#tbFullTexSearch').val();
    window.hasPopup = false;
    if (hasRegular()) {
        var properties = GetCheckedProperties();
        addRegularToComparison(properties, materialId, subgroupId, sourceId, sourceMaterialId);
    }
    if (hasStressStrain()) {
        window.selectedTemp = [];
        checkCheckedStressStrain();
        var selectedTemperaturesSS = [];
        if (!window.hasPopup) {
            if (window.selectedTemp.length > 0) {
                selectedTemperaturesSS = window.selectedTemp;
            }
            else {
                selectedTemperaturesSS.push(selectedTemperatureStressStrain);
            }
            var selectedMaterialConditionId = $("#selectedMaterialConditionSS").val();
            var selectedTestConditionId = $("#selectCondSS").val();
            addSSToComparison(selectedTemperaturesSS, selectedMaterialConditionId, selectedTestConditionId);
        }

    }

}

function addRegularToComparison(properties, materialId, subgroupId, sourceId, sourceMaterialId) {
    if (properties.length > 0) {
        displayProgressIndicator('main');
        $('.progress-indicator').css('display', 'block');

        ini.ajax.post(comparisonAddMaterialPath, ini.ajax.contentType.Json, {
            properties: properties, materialId: materialId, subgroupId: subgroupId, sourceId: sourceId, sourceMaterialId: sourceMaterialId
        },
                 function (data) {
                     if (data.success == true) {
                         showPopup({
                             width: 300,
                             height: 60,
                             title: "",
                             close: function () {
                                 //function on close
                             }
                         }, "Successfully added to comparison!");
                         removeProgressIndicator('main');
                         $('.progress-indicator').css('display', 'none');
                     } else {
                         showPopup({
                             width: 400,
                             height: 100,
                             title: "",
                             close: function () {
                                 //function on close
                             }
                         }, "You have already added the maximum number of 5 materials to Comparison! Please remove at least one material from Comparison before selecting a new material to add.");
                         removeProgressIndicator('main');
                         $('.progress-indicator').css('display', 'none');
                     }

                 },
                  function (error) {

                  }
              );
    } else {
        showPopup({
            width: 300,
            height: 60,
            title: "",
            close: function () {
                //function on close
            }
        }, "Please select at least one property!");
    }
}

function addSSToComparison(selectedTemperaturesSS, selectedMaterialConditionId, selectedTestConditionId) {
    if (selectedTemperaturesSS.length > 0) {
        displayProgressIndicator('main');
        $('.progress-indicator').css('display', 'block');

        ini.ajax.post(comparisonAddSSPath, ini.ajax.contentType.Json, {
            materialId: selectedMaterialId,
            subgroupId: selectedSubgroupId,
            sourceId: selectedSourceId,
            sourceMaterialId: selectedSourceMaterialId,
            temperatures: selectedTemperaturesSS,
            materialConditionId: selectedMaterialConditionId,
            testConditionId: selectedTestConditionId
        },
                 function (data) {
                     if (data.success == true) {
                         showPopup({
                             width: 300,
                             height: 60,
                             title: "",
                             close: function () {
                                 //function on close
                             }
                         }, "Successfully added to comparison!");
                         removeProgressIndicator('main');
                         $('.progress-indicator').css('display', 'none');
                     } else {
                         showPopup({
                             width: 400,
                             height: 100,
                             title: "",
                             close: function () {
                                 //function on close
                             }
                         }, "You have already added the maximum number of 5 materials to Comparison! Please remove at least one material from Comparison before selecting a new material to add.");
                         removeProgressIndicator('main');
                         $('.progress-indicator').css('display', 'none');
                     }

                 },
                  function (error) {

                  }
              );
    } else {
        showPopup({
            width: 300,
            height: 60,
            title: "",
            close: function () {
                //function on close
            }
        }, "Please select at least one property!");
    }
}


function addToExporter(materialId, subgroupId, sourceId, sourceMaterialId) {

    var searchText = $('#tbFullTexSearch').val();

    var properties = GetCheckedProperties();

    if (properties.length > 0) {

        displayProgressIndicator('main');
        $('.progress-indicator').css('display', 'block');

        ini.ajax.post(eXporterAddMaterialPath, ini.ajax.contentType.Json, {
            properties: properties, materialId: materialId, subgroupId: subgroupId, sourceId: sourceId, sourceMaterialId: sourceMaterialId
        },
                 function (data) {
                     if (data.success == true) {
                         showPopup({
                             width: 400,
                             height: 260,
                             title: "",
                             close: function () {
                                 //function on close
                             }
                         }, data.message);
                         removeProgressIndicator('main');
                         $('.progress-indicator').css('display', 'none');
                     } else {

                     }

                 },
                  function (error) {
                  }
          );
    } else {
        showPopup({
            width: 300,
            height: 60,
            title: "",
            close: function () {
                //function on close
            }
        }, "Please select at least one property!");
    }
}

function goToCompareMaterialsPage() {
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    window.location.href = comparisonMaterialsPath;
}


function goToExporter() {
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    window.location.href = eXporterPath;
}


searchFunc.goToAdvancedSearch = function () {
    window.location.href = advSearchPath;
}


searchFunc.advSearch = function () {
    window.location.href = searchFunc.advSearchPath;
}

searchFunc.advSearchHome = function (initial, isChemical, groupId) {
    window.location.href = searchFunc.advSearchPath + "?initial=true&isChemical=" + isChemical+"&groupId="+groupId;
}



/////////////  Show/Hide Columns
// 

searchFunc.columnDefinition = function () {
    this.Id = -1;
    this.isVisible = true;
    this.Name = '';
    this.Filter = '';

}

//searchFunc.columnsStatus = new Array();
//searchFunc.columnsStatus;
if (!searchFunc.columnsStatus) {
    searchFunc.columnsStatus;
};

searchFunc.openColumnsSelector = function () {


    $("#menageColumnsVisibility").css("background-color", "#f5f3f3");

    $('#menageFiltersVisibility').hide();
    $('#openColumnsSelectorId').hide();
    $('#closeColumnsSelectorId').show();
    //$('#openFiltersId').hide();
    $('#columnsSelectorId').empty();

    // add heading line
    var heading1 = '<label style="font-size: 16px; color: rgb(119,119,119);" >Column</label>';
    $('#columnsSelectorId').append(heading1);
    var heading2 = '<label style="margin-left:20px; font-size: 16px; color: rgb(119,119,119);" >Filter</label>';
    $('#columnsSelectorId').append(heading2);

    // add h.line
    $('#columnsSelectorId').append('<div style="clear: both;"></div>');
    $('#columnsSelectorId').append('<div class="hline" style="margin-bottom:10px;"></div>');

    //Get filter values from the SERVER and store them into the array searchFunc.columnsStatus
    if ('undefined' !== typeof searchFuncColumnsStatusFromServer && searchFuncColumnsStatusFromServer.trim() != "") {
        searchFunc.columnsStatus = searchFuncColumnsStatusFromServer;
    }

    jQuery.each(searchFunc.columnsStatus, function (index, item) {
        var columnId = item.Id;
        var isVisible = item.isVisible;
        var name = $.trim(item.Name);
        var filter = $.trim(item.Filter);
        var isDisabled = (index == 0 ? true : false);   // Disable checkbox for column "Material Name"

        searchFunc.AddCheckBox('columnsSelectorId', columnId, name, filter, isVisible, isDisabled);
    });

    $(".columnsSelector").show();
}

searchFunc.closeColumnsSelector = function () {
    $("#menageColumnsVisibility").css("background-color", "");
    $('#openColumnsSelectorId').show();
    $('#closeColumnsSelectorId').hide();
    $('#menageFiltersVisibility').show();
    $(".columnsSelector").hide();
    //$('#openFiltersId').show();
    $('#openColumnsSelectorId').show();
}

searchFunc.resetColumnsSelector = function () {
    $('#columnsSelectorId').children('input[id^="tb_"]').val('');
    searchFunc.applyColumnsSelector();
}

searchFunc.applyColumnsSelector = function () {
    // For each cb...
    var isFilterChanged = false;
    var isColumnVisibilityChanged = false;
    $('.cbSelectColumn1').each(function (index, item) {
        var isVisible = $(this).is(':checked');
        var columnId = parseInt($(this).attr('colid'));
        var filter = '';

        // set properties in the class
        jQuery.each(searchFunc.columnsStatus, function (index2, item2) {
            var id = item2.Id;
            if (id == columnId) {
                filter = $.trim($('input#typeFilter').val());
                isFilterChanged = isFilterChanged || (item2.Filter != filter);
                isColumnVisibilityChanged = isColumnVisibilityChanged || (item2.isVisible != isVisible);
                item2.isVisible = isVisible;
                item2.Filter = filter;

                searchFunc.setColumnVisibility(columnId, isVisible);
            };
        });

        // set cb attribute for the value to entered filter value
        $(this).attr('filter', filter);
    });


    // Set filter values from array searchFunc.columnsStatus to the SERVER
    if (isFilterChanged || isColumnVisibilityChanged) {
        var filters = new Object();
        filters.AllFilters = searchFunc.columnsStatus;
        //displayProgressIndicator('main', 'Updating, please wait');
        //$('.progress-indicator').css('display', 'block');
        ini.ajax.post(searchFunc.applyFiltersColumnsPath, ini.ajax.contentType.Json, {
            filters: filters
        },
            function (result) {
                //removeProgressIndicator('main');
                //$('.progress-indicator').css('display', 'none');
                //window.location.href = result.url;
            },
            function (error) {
            });

        if (isFilterChanged) {
            searchFunc.fullTextSearch(1);
        }
    }

    // set column visibility
    searchFunc.setAllColumnsVisibility(searchFunc.columnsStatus);
    searchFunc.closeColumnsSelector();
}

searchFunc.setAllColumnsVisibility = function (colDefinitions) {
    if ('undefined' !== typeof colDefinitions) {
        jQuery.each(colDefinitions, function (index, item) {
            searchFunc.setColumnVisibility(item.Id, item.isVisible);
        });
    };
}

searchFunc.setColumnVisibility = function (columnId, isVisible) {
    if (isVisible) {
        $('#materialList th:nth-child(' + (columnId + 1) + ')').show();
        $('#materialList td:nth-child(' + (columnId + 1) + ')').show();
    } else {
        $('#materialList th:nth-child(' + (columnId + 1) + ')').hide();
        $('#materialList td:nth-child(' + (columnId + 1) + ')').hide();
    }
}




searchFunc.AddCheckBox = function (containerId, id, title, filter, checked, disabled) {
    $('#' + containerId).append('<div style="clear: both; margin-bottom:5px;"></div>');
    $('#' + containerId)
    .append(
       $(document.createElement('input')).attr({
           id: id
           , name: title
           , value: title
           , type: 'checkbox'
           , checked: checked
           , class: 'cbSelectColumn'
           , colid: id
           , filter: filter
           , disabled: disabled
       })
    );
    //
    var lbl = '<label title="' + title + '" id="lbl_' + id + '">' + title + '</label>';
    $('#' + containerId).append(lbl);
    var tb = '<input style="width:130px;" type="text" id="tb_' + id + '" value="' + filter + '">';
    $('#' + containerId).append(tb);
}

//
///////////// / Show/Hide Columns


/////////////  Show/Hide Subgroup Columns
// 

searchFunc.columnSubgroupDefinition = function () {
    this.Id = -1;
    this.isVisible = true;
    this.Name = '';
    this.Filter = '';

}

if (!searchFunc.columnsSubgroupStatus) {
    searchFunc.columnsSubgroupStatus;
};


searchFunc.openColumnsSubgroupSelector = function () {

    $("#menageColumnsSubgroupVisibility").css("background-color", "#f5f3f3");

    $('#menageColumnsFiltersVisibility').hide();
    $('#openSubgroupColumnsSelectorId').hide();
    $('#closeSubgroupColumnsSelectorId').show();
    //$('#openSubgroupFiltersId').hide();
    $('#columnsSelectorId').empty();

    // add heading line
    var heading1 = '<label style="font-size: 16px; color: rgb(119,119,119);" >Column</label>';
    $('#columnsSelectorId').append(heading1);
    //var heading2 = '<label style="margin-left:20px; font-size: 16px; color: rgb(119,119,119);" >Filter</label>';
    //$('#columnsSelectorId').append(heading2);

    // add h.line
    $('#columnsSelectorId').append('<div style="clear: both;"></div>');
    $('#columnsSelectorId').append('<div class="hline" style="margin-bottom:10px;"></div>');

    //Get filter values from the SERVER and store them into the array searchFunc.columnsSubgroupStatus
    if ('undefined' !== typeof searchFuncColumnsSubgroupStatusFromServer && searchFuncColumnsSubgroupStatusFromServer != '') {
        searchFunc.columnsSubgroupStatus = searchFuncColumnsSubgroupStatusFromServer;
    }

    jQuery.each(searchFunc.columnsSubgroupStatus, function (index, item) {
        var columnId = item.Id;
        var isVisible = item.isVisible;
        var name = $.trim(item.Name);
        var filter = $.trim(item.Filter);
        var isDisabled = (index == 0 ? true : false);   // Disable checkbox for column "reference"

        // skip last column with links
        if (index <= 4) {
            searchFunc.AddSubgroupCheckBox('columnsSelectorId', columnId, name, filter, isVisible, isDisabled);
        };
    });

    $(".columnsSelector").show();

}

searchFunc.AddSubgroupCheckBox = function (containerId, id, title, filter, checked, disabled) {
    $('#' + containerId).append('<div style="clear: both; margin-bottom:5px;"></div>');
    $('#' + containerId)
    .append(
       $(document.createElement('input')).attr({
           id: id
           , name: title
           , value: title
           , type: 'checkbox'
           , checked: checked
           , class: 'cbSelectColumn'
           , colid: id
           , filter: filter
           , disabled: disabled
       })
    );
    //
    var lbl = '<label title="' + title + '" id="lbl_' + id + '">' + title + '</label>';
    $('#' + containerId).append(lbl);
    //var tb = '<input style="width:130px;" type="text" id="tb_' + id + '" value="' + filter + '">';
    //$('#' + containerId).append(tb);
}



searchFunc.closeColumnsSubgroupSelector = function () {
    $("#menageColumnsSubgroupVisibility").css("background-color", "");
    $('#openSubgroupColumnsSelectorId').show();
    $('#closeSubgroupColumnsSelectorId').hide();
    $('#menageColumnsFiltersVisibility').show();
    $(".columnsSelector").hide();
    //$('#openFiltersId').show();
    //$('#openColumnsSelectorId').show();
}

searchFunc.resetColumnsSubgroupSelector = function () {

}

function setActivePluginForSubgroupListPage() {
    if ($('#fromBrowse').val() == 'True') {
        $('#pluginSearchResults').removeClass('pluginActive');
        $('#pluginSearchResults').addClass('plugin');
        $('#pluginBrowseProperties').removeClass('plugin');
        $('#pluginBrowseProperties').addClass('pluginActive');
    }
    if ($('#fromAdvanced').val() == 'True') {
        $('#pluginSearchResults').removeClass('pluginActive');
        $('#pluginSearchResults').addClass('plugin');
        $('#pluginAdvancedSearch').removeClass('plugin');
        $('#pluginAdvancedSearch').addClass('pluginActive');
    }

}

searchFunc.applyColumnsSubgroupSelector = function () {
    // For each cb...
    var isFilterChanged = false;
    var isColumnVisibilityChanged = false;
    $('#columnsSelectorId').children('.cbSelectColumn').each(function (index, item) {
        var isVisible = $(this).is(':checked');
        var columnId = parseInt($(this).attr('colid'));
        var filter = '';

        // set properties in the class
        jQuery.each(searchFunc.columnsSubgroupStatus, function (index2, item2) {
            var id = item2.Id;
            if (id == columnId) {
                filter = $.trim($('input#tb_' + columnId.toString()).val());
                isFilterChanged = isFilterChanged || (item2.Filter != filter);
                isColumnVisibilityChanged = isColumnVisibilityChanged || (item2.isVisible != isVisible);
                item2.isVisible = isVisible;
                item2.Filter = filter;

                searchFunc.setColumnSubgroupVisibility(columnId, isVisible);
            };
        });

        // set cb attribute for the value to entered filter value
        $(this).attr('filter', filter);
    });

    //// Set filter values from array searchFunc.columnsSubgroupStatus to the SERVER
    //if (isFilterChanged || isColumnVisibilityChanged) {
    //    var filters = new Object();
    //    filters.AllFilters = searchFunc.columnsSubgroupStatus;
    //    //displayProgressIndicator('main', 'Updating, please wait');
    //    //$('.progress-indicator').css('display', 'block');
    //    ini.ajax.post(searchFunc.applyFiltersSubgroupColumnsPath, ini.ajax.contentType.Json, { filters: filters },
    //        function (result) {
    //            //removeProgressIndicator('main');
    //            //$('.progress-indicator').css('display', 'none');
    //            //window.location.href = result.url;
    //        },
    //        function (error) {
    //        });

    //    //if (isFilterChanged) {
    //    //    searchFunc.fullTextSearch(1);
    //    //}
    //}

    // set column visibility
    searchFunc.setAllColumnsSubgroupVisibility(searchFunc.columnsSubgroupStatus);
    searchFunc.closeColumnsSubgroupSelector();
}

searchFunc.setAllColumnsSubgroupVisibility = function (colDefinitions) {
    if ('undefined' !== typeof colDefinitions) {
        jQuery.each(colDefinitions, function (index, item) {
            searchFunc.setColumnSubgroupVisibility(item.Id, item.isVisible);
        });
    };
}

searchFunc.setColumnSubgroupVisibility = function (columnId, isVisible) {
    if (isVisible) {
        $('#materialsSubgroupList th:nth-child(' + (columnId + 1) + ')').show();
        $('#materialsSubgroupList td:nth-child(' + (columnId + 1) + ')').show();
    } else {
        $('#materialsSubgroupList th:nth-child(' + (columnId + 1) + ')').hide();
        $('#materialsSubgroupList td:nth-child(' + (columnId + 1) + ')').hide();
    }
}

//
///////////// / Show/Hide Subgroup Columns


///////////Adv search per properties 

var searchProp = {
}

// enum for property types
searchProp.propertyType = {
    NotDefined: -1,
    Property: 1
}

// enum for binary operators
searchProp.binaryOperators = {
    NotDefined: -1,
    And: 0
}

// enum for logical operators
searchProp.logicalOperators = {
    NotDefined: -1,
    Exists: 0,
    Eq: 1,
    Lte: 2,
    Gte: 3,
    Between: 4
}

searchProp.property = function () {
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
    this.unitName = ''
}

searchProp.selectedProperties = new Array();

searchProp.addSelectedProperty = function (
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
    ) {
    // 1: as advSearchFunc.property
    var def = new searchProp.property();
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
    searchProp.selectedProperties.push(def);

}


// Set visability for propery all elemens
searchProp.setPropElementsVisability = function () {
    searchProp.setBopVisability();
}

// Hide first Bop in div
searchProp.setBopVisability = function () {
    $("#divPropAdvancedSearch").find('.onePropertyDef').each(function (index, element) {
        var uid = $(this).attr('id');
        if (index == 0) {
            $(this).find('#bop_' + uid).hide();
        } else {
            $(this).find('#bop_' + uid).show();
        }
    });
};

// Check textboxes and unit selector visability
searchProp.setEnteredValuesVisibility = function () {
    $("#divPropAdvancedSearch").find('.onePropertyDef').each(function (index, element) {

        if ($(this).attr('proptype') == 'Property') {
            var uid = $(this).attr('id');
            var $selector = $(this).find('.selectOperator');

            if (typeof $selector !== "undefined") {

                var selectedVal = $($selector).find(":selected").val();
                if (typeof selectedVal != 'undefined') {
                    searchProp.onSelectOperatorChanged($selector);
                };
            }
        }

    });
};

// Fill advSearchFunc.selectedProperties with selected properties
searchProp.fillSelectedProperties = function () {
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');



    // Check for input values
    var isAllNumeric = true;
    $(".numeric:visible").each(function () {
        var enteredVal = jQuery.trim($(this).val());

        //if (!$.isNumeric(enteredVal)) {
        //    $(this).css("background", "red");
        //}


        isAllNumeric = isAllNumeric && $.isNumeric(enteredVal);
    });

    if (!isAllNumeric) {
        alert("Field(s) must be numeric.");
        removeProgressIndicator('main');
        $('.progress-indicator').css('display', 'none');
        return;
    }

    searchProp.selectedProperties = new Array();

    $("#divPropAdvancedSearch").find('.onePropertyDef').each(function () {
        // define propertyType for Property
        var propertyType = searchProp.propertyType.Property;

        if ($(this).attr('proptype') == 'Material') {
            propertyType = searchProp.propertyType.Material;
        }

        var uid = $(this).attr('id');

        // find binaryOperators
        var binaryOperators = searchProp.binaryOperators.And;


        // Read property id and name
        var propertyId = $(this).attr('propid');
        var propertyName = jQuery.trim($(this).find('#nm_' + uid).text());

        // find logicalOperators and entered values for value/interval
        var logicalOperators = searchProp.logicalOperators.NotDefined;
        var valueFrom = '';
        var valueTo = '';
        var valueFrom_orig = '';
        var valueTo_orig = '';
        var unitId = -1;
        var unitName = '';
        var factor = 1;
        var offset = 0;

        if (propertyType == searchProp.propertyType.Property) {

            var selectedValue = $(this).find('#lop_' + uid).find(":selected").val();
            switch (selectedValue.toString().toLowerCase()) {
                case "exists":
                    logicalOperators = searchProp.logicalOperators.Exists;
                    break;

                case "equals":
                case "eq":
                    logicalOperators = searchProp.logicalOperators.Eq;
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
                    logicalOperators = searchProp.logicalOperators.Lte;
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
                    logicalOperators = searchProp.logicalOperators.Gte;
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
                    logicalOperators = searchProp.logicalOperators.Between;
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
                    logicalOperators = searchProp.logicalOperators.NotDefined;
                    break;
            }
        }

        // add property to the list
        searchProp.addSelectedProperty(
            propertyType
            , binaryOperators
            , logicalOperators
            , uid, propertyId
            , propertyName
            , valueFrom
            , valueTo
            , valueFrom_orig
            , valueTo_orig
            , unitId
            , unitName
       );
    }); // the end of the main DIV loop

    /*search per material and source*/
    var filtersPage = $('#resizable').html().trim();
    var selectedSource = "";
    var selectedSourceId = "";
    var selectedSourceDatabookId = "";
    var selectedMaterialFilterId = "";
    var materialTypeId;
    var selectedSource = $('#sourcesSelection :selected').val().split('#');
    var selectedSourceId = selectedSource[0];
    var selectedSourceDatabookId = selectedSource[1];

    var selectedMaterialFilterId = $("#containerMaterial").jstree('get_selected');
    //var selectedPropertyFilterId = $("#containerProperty").jstree('get_selected');

    var materialTypeId = $("#" + selectedMaterialFilterId).attr("nodetype");
    //var propertyTypeId = $("#" + selectedPropertyFilterId).attr("nodetype");


    var fullTextSearch = $("#tbFullTexSearch").val();

    var classificationsSearch = "";
    $("a[id^='link_']").each(function () {
        if ($(this).attr('treeClicked') != undefined) {
            classificationsSearch = classificationsSearch + $(this).attr('id').replace("link_", "") + ",";
        }
    });

    var advFilters = new Object();
    advFilters.AllFilters = searchProp.selectedProperties;

    var searchFilters = new Object();
    searchFilters.ClasificationId = selectedMaterialFilterId;
    searchFilters.ClasificationTypeId = materialTypeId;
    if (selectedSourceId != undefined) {
        searchFilters.Source = selectedSourceId;
    }
    if (selectedSourceDatabookId != undefined) {
        searchFilters.Source = selectedSourceId + "," + selectedSourceDatabookId;
    }

    searchFilters.filter = fullTextSearch;


    ini.ajax.post(searchFunc.applyFiltershPath, ini.ajax.contentType.Json, {
        advFilters: advFilters, classIds: classificationsSearch, filters: searchFilters
    },
               function (result) {
                   removeProgressIndicator('main');
                   $('.progress-indicator').css('display', 'none');
                   $('#resultsResizable').html(result);

                   if (selectedSourceId != "0") {

                       $("#selectedSource").show();
                       $("#selectedSourceName").html($('#sourcesSelection :selected').html());
                   }
               },
               function (error) {
               }
           );
}






//////////searchProp.clearSelectedProperties = function () {

//////////    ini.ajax.post(searchProp.clearAdvSearchFiltershPath, ini.ajax.contentType.Json, null,
//////////                   function (result) {
//////////                       //$(".filtersPage").hide();
//////////                       //alert('i');
//////////                       $('#divConditions').html(result);
//////////                   },
//////////                   function (error) {
//////////                   }
//////////               );

//////////}

//
/////////////////////// /Definitions for selected properties


searchProp.addProperty = function (id) {
}

searchProp.removeProperty = function (id) {
}

searchProp.onSelectOperatorChanged = function (selector) {
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




///////////Adv search per properties 




searchFunc.getParameterByName = function (name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

