;
materialDetails = {};

materialDetails.getMaterialDetails = function (materialId, subgroupId, sourceId, sourceMaterialId, searchText, tabId, totalMat) {
    var partId = sourceMaterialId.toString() + '_' + subgroupId.toString() + '_' + sourceId.toString();
    if (sourceId != 1 && totalMat == "1") {
        $('#materialsSubgroupList > tbody  > tr').each(function () {
            if ($(this).attr('class').toString().indexOf('trHideMat') >= 0) {
                $(this).css("display", "table-row");
            } else {
                if ($(this).attr('id') == innerHTMLId) {
                    var specId = innerHTMLId.toString().replace('trDetails_', 'tdSpec_');
                    if ($('#' + specId)[0].innerHTML != "") {
                        displayProgressIndicator('main');
                        $('.progress-indicator').css('display', 'block');
                        window.location.href = materialDetails.getMaterialDetailsPath + '?materialId=' + materialId + '&subgroupId=' + subgroupId + '&sourceId=' + sourceId + '&sourceMaterialId=' + sourceMaterialId + '&searchText=' + searchText + '&tabId=' + tabId;
                    }

                    var standardId = innerHTMLId.toString().replace('trDetails_', 'tdStandard_');
                    var specId = innerHTMLId.toString().replace('trDetails_', 'tdSpec_');
                    var detailsId = innerHTMLId.toString().replace('trDetails_', 'tdDetails_');
                    var sourceId1 = innerHTMLId.toString().replace('trDetails_', 'tdSourceText_');
                    if (standardId.substring(standardId.length - 1, standardId.length) != '1') {
                        $('#' + standardId).html(innerHTMLstandard);
                        $('#' + specId).html(innerHTMLspec);
                        $('#' + detailsId).html(innerHTMLdetails);
                        $('#' + sourceId1).html(innerHTMLsource);
                    }

                }
                    //if ($(this).attr('id') == innerHTMLId) {

                    //}
                else {
                    var specId = $(this).attr('id').toString().replace('trDetails_', 'tdSpec_');
                    
                    if (specId.indexOf(partId) >= 0) {
                        if ($('#' + specId)[0].innerHTML != "") {
                            displayProgressIndicator('main');
                            $('.progress-indicator').css('display', 'block');
                            window.location.href = materialDetails.getMaterialDetailsPath + '?materialId=' + materialId + '&subgroupId=' + subgroupId + '&sourceId=' + sourceId + '&sourceMaterialId=' + sourceMaterialId + '&searchText=' + searchText + '&tabId=' + tabId;
                        }
                    }
                }
            }
        });
    } else {
        displayProgressIndicator('main');
        $('.progress-indicator').css('display', 'block');
        window.location.href = materialDetails.getMaterialDetailsPath + '?materialId=' + materialId + '&subgroupId=' + subgroupId + '&sourceId=' + sourceId + '&sourceMaterialId=' + sourceMaterialId + '&searchText=' + searchText + '&tabId=' + tabId;
    }
}


materialDetails.changeConditionFirst = function (productGroupId, element) {

  


    if (changeConditionSourceId == 1) {

        displayProgressIndicator('main');
        $('.progress-indicator').css('display', 'block');

        var conditionIdProductFormId = $('#selectCondFirst_' + productGroupId).val();
        var conditionId = conditionIdProductFormId.split(";")[0]; 
        var productFormId = conditionIdProductFormId.split(";")[1];
        var type = $("input[name='name']:checked").val();
        var $this = $(element);
        var selectedElement = $this.find('option:selected');
        //alert(stand.attr("matdescr"));
        //var selectedMaterialDescription = matedescr
        var searchText = $('#searchTextHolder').val();

        ini.ajax.post(materialDetails.changedFirstCondition, ini.ajax.contentType.Json, {
            materialDescription: selectedElement.attr("matdescr"), thickness: selectedElement.attr("thickness"), materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, conditionId: conditionId, productFormId: productFormId, searchText: searchText, type: type
        },
             function (success) {
                 $('#selectCondFirst_' + productGroupId).attr("matedescr", success.SelectedMaterilaDescription);
                 $('[id^="secondConditionsAndDataId_' + productGroupId + '"]').html(success);
                 convertValuesELS(type);
                 removeProgressIndicator('main');
                 $('.progress-indicator').css('display', 'none');
             },
             function (error) {

             }
         );



    } else if (changeConditionSourceId == 2) {



        if (productGroupId == 806) {

            var conditionId = $('#selectCond_' + productGroupId).val();
            var type = $("input[name='name']:checked").val();
            var searchText = $('#searchTextHolder').val();

            ini.ajax.post(materialDetails.getMechanicalMetal, ini.ajax.contentType.Json, { materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, searchText: searchText, type: type,conditionId: conditionId },
              function (success) {
                  $('[id^="cond_' + productGroupId + '"]').html(success)

                  removeProgressIndicator('main');
                  $('.progress-indicator').css('display', 'none');
              },
              function (error) {
              }
             );
        } else if (productGroupId == 807) {

            var conditionId = $('#selectCond_' + productGroupId).val();
            var type = $("input[name='name']:checked").val();
            var searchText = $('#searchTextHolder').val();

            ini.ajax.post(materialDetails.getPhysicalMetal, ini.ajax.contentType.Json, { materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, conditionId: conditionId, searchText: searchText, type: type },
             function (success) {
                 $('[id^="cond_' + productGroupId + '"]').html(success)

                 removeProgressIndicator('main');
                 $('.progress-indicator').css('display', 'none');
             },
             function (error) {
             }
             );
        }

    } else {

        var t = $('#selectCond_' + productGroupId).val();
        $('[id^="cond_' + productGroupId + '"]').hide();
        $('#cond_' + productGroupId + '_' + t).show();
    }
}



materialDetails.changeConditionSecond = function (productGroupId) {
    if (changeConditionSourceId == 1) {
        var rowId = $('#selectCondSecond_' + productGroupId).val();
       
        var type = $("input[name='name']:checked").val();

        var searchText = $('#searchTextHolder').val();

        ini.ajax.post(materialDetails.changedSecondCondition, ini.ajax.contentType.Json, { materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, rowId: rowId, searchText: searchText, type: type },
             function (success) {
                 $('[id^="cond_' + productGroupId + '"]').html(success)
                 convertValuesELS(type);
                 removeProgressIndicator('main');
                 $('.progress-indicator').css('display', 'none');
             },
             function (error) {

             }
         );
    } else if (changeConditionSourceId == 2) {



        if (productGroupId == 806) {

            var conditionId = $('#selectCond_' + productGroupId).val();
            var type = $("input[name='name']:checked").val();
            var searchText = $('#searchTextHolder').val();

            ini.ajax.post(materialDetails.getMechanicalMetal, ini.ajax.contentType.Json, { materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, searchText: searchText, type: type, conditionId: conditionId },
              function (success) {
                  $('[id^="cond_' + productGroupId + '"]').html(success)

                  removeProgressIndicator('main');
                  $('.progress-indicator').css('display', 'none');
              },
              function (error) {
              }
             );
        } else if (productGroupId == 807) {

            var conditionId = $('#selectCond_' + productGroupId).val();
            var type = $("input[name='name']:checked").val();
            var searchText = $('#searchTextHolder').val();

            ini.ajax.post(materialDetails.getPhysicalMetal, ini.ajax.contentType.Json, { materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, conditionId: conditionId, searchText: searchText, type: type },
             function (success) {
                 $('[id^="cond_' + productGroupId + '"]').html(success)

                 removeProgressIndicator('main');
                 $('.progress-indicator').css('display', 'none');
             },
             function (error) {
             }
             );
        }

    } else {

        var t = $('#selectCond_' + productGroupId).val();
        $('[id^="cond_' + productGroupId + '"]').hide();
        $('#cond_' + productGroupId + '_' + t).show();
    }
}

function convertValuesELS(type) {
        $(".valueTd").each(function () {
            if (type == 1) {
                $(this).html($(this).attr("origvalue"));
            }
            if (type == 2) {
                $(this).html($(this).attr("defaultvalue"));
            }
            if (type == 3) {
                $(this).html($(this).attr("usvalue"));
            }
        });

        $(".unitTd").each(function () {
            if (type == 1) {
                $(this).html($(this).attr("origunit"));
            }
            if (type == 2) {
                $(this).html($(this).attr("defaultunit"));
            }

            if (type == 3) {
                $(this).html($(this).attr("usunit"));
            }
        });

        $(".noteTd").each(function () {
            if (type == 1) {
                $(this).html($(this).attr("orignote"));
            }

            if (type == 2) {
                $(this).html($(this).attr("defaultnote"));
            }

            if (type == 3) {
                $(this).html($(this).attr("usnote"));
            }
        });
    //for chemical materials
        //materialDetails.closeTemperatureDiagrams();
    }
function convertValuesTM(materialId, sourceMaterialId, subgroupId, sourceId, type) {

        setActiveUnit(type);
        fillOpenedAccordions();
       
        for (var i = 0; i < active.length; i++) {
            var productGroupId = active[i];

            var type = $("input[name='name']:checked").val();

            if (productGroupId == 806 || productGroupId == 807) {

                var conditionId = $('#selectCond_' + productGroupId).val();
                var searchText = $('#searchTextHolder').val();

            getConditionDataForSelectedUnit(conditionId, searchText, type, productGroupId);

            } else if (productGroupId == 3) {             
                        

                    var conditionId = $('#selectCondSS').val();
                    //displayProgressIndicator('main');
                    //$('.progress-indicator').css('display', 'block');
                    ini.ajax.post(materialDetails.getStressStrainConditionDetails, ini.ajax.contentType.JSon, { materialId: sourceMaterialId, subgroupId: subgroupId, conditionId: conditionId },
                                function (success) {
                            $('#stressStrainPerCondition').html(success);
                            //removeProgressIndicator('main');
                            //$('.progress-indicator').css('display', 'none');
                        },
                        function (error) {
                            //showAlert('Error', error);
                        }
                    );
        }

        else if (productGroupId == 4) {


            var materialId = $('#materialId').val();
            var conditionId = $('#selectCondFatigueStrain').val();

              var materialConditionId = $('#selectMaterialCondFatigueStrain').val();

            //displayProgressIndicator('main');
            //$('.progress-indicator').css('display', 'block');
            ini.ajax.post(materialDetails.getFatigueStrainCondition, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, materialConditionId: materialConditionId, conditionId: conditionId },
                        function (success) {
                            $('#fatigueStrainPerCondition').html(success);
                            //removeProgressIndicator('main');
                            //$('.progress-indicator').css('display', 'none');
                        },
                        function (error) {

                        }
                    );

            var materialId = $('#materialId').val();
            var conditionId = $('#selectCondFatigueStress').val();



            //displayProgressIndicator('main');
            //$('.progress-indicator').css('display', 'block');
            ini.ajax.post(materialDetails.getFatigueStressCondition, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, materialConditionId: materialConditionId, conditionId: conditionId
            },
                        function (success) {
                            $('#fatigueStressPerCondition').html(success);
                                    //removeProgressIndicator('main');
                                    //$('.progress-indicator').css('display', 'none');
                                },
                                function (error) {
                                    //showAlert('Error', error);
                                }
                            );

                  
             
            }
        else if (productGroupId == 5) {

            var materialId = $('#materialId').val();
            var conditionId = $('#selectCreepDataCond').val();

            //displayProgressIndicator('main');
            //$('.progress-indicator').css('display', 'block');
            ini.ajax.post(materialDetails.getCreepDataMetal, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, conditionId: conditionId },
                        function (success) {
                            $('#creepCond').html(success);
                            //removeProgressIndicator('main');
                            //$('.progress-indicator').css('display', 'none');
                        },
                        function (error) {

        }
                    );
    }

}
}


var active = [];
var initialOpenedPage = true;




function convertValue(materialId, sourceMaterialId, subgroupId, sourceId, type) {
    if (sourceId == 1) {
        convertValuesELS(type);
    }
    else {

        convertValuesTM(materialId, sourceMaterialId, subgroupId, sourceId, type);
}
}

function getConditionDataForSelectedUnit(conditionId, searchText, type, productGroupId) {
    ini.ajax.post(materialDetails.conditionDataPath, ini.ajax.contentType.Json, {
        materialId: changeConditionMaterialID,
        sourceMaterialId: changeConditionSourceMaterialId,
        sourceId: changeConditionSourceId,
        subgroupId: changeConditionSubgroupId,
        groupId: productGroupId,
        conditionId: conditionId,
        searchText: searchText,
        type: type
    },
             function (success) {
               
                 $('[id^="cond_' +productGroupId + '"]').html(success);
                 materialDetails.selectOneValueForSameProperty();
                 removeProgressIndicator('main');
                 $('.progress-indicator').css('display', 'none');
             },
            function (error) {

            }
         );
}
function fillOpenedAccordions() {

    initialOpenedPage = false;
    active = [];
    $('.accordion').children('.ui-state-active').each(function () {
        active.push($(this).attr("indexofaccordion"));
    });

}

function setActiveUnit(type) {

    displayProgressIndicator('main', '');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(convertValues, ini.ajax.contentType.Json, {
        type: type
    },
          function (data) {
              removeProgressIndicator('main');
              $('.progress-indicator').css('display', 'none');
          },
        function (error) {
        }
    );
}

materialDetails.openActiveAccordion= function() {
        var firstIsActive = false;

        if (active.length == 0 && initialOpenedPage) {
            $('[id^="propAccordion_"]').accordion({ active: false });
        }
        else {
            $('.accordion').each(function () {
                var isActive = false;
                var indexOfAccordion = $(this).attr("indexofaccordionparent");
                for (i = 0; i < active.length; i++) {

                    if (active[i] == 1 && indexOfAccordion == 1) {
                        firstIsActive = false;
                    }

                    if (active[i] == indexOfAccordion) {
                        isActive = true;
                        break;
                    }
                }
                if (!firstIsActive) {
                    $('#propAccordion_first').accordion({ "active": false });
                }

                if (indexOfAccordion > 1 && !isActive) {
                    $('#propAccordion_other_' + indexOfAccordion).accordion({ "active": false });
                }

            });
        }
    }
//    materialDetails.getPropertyGroupDetails = function (sourceMaterialId, subgroupId, sourceId, productGroup) {


//    if (sourceId == 1) {
//        displayProgressIndicator('main', 'Updating, please wait');
//        $('.progress-indicator').css('display', 'block');

//        ini.ajax.post(searchFunc.applySubgroupFiltershPath, ini.ajax.contentType.Json, { Specification: specification, SourceId: selectedSourceId, StandardId: selectedStandard, filter: fullTextSearch, MaterialId: materialId, Source: selectedSource },
//                   function (success) {
//                       $(".filtersPage").hide();
//                       $('#divSubgroupsList').html(success);

//                       removeProgressIndicator('main');
//                       $('.progress-indicator').css('display', 'none');
//                   },
//                   function (error) {

//                   }
//               );
//    }

//}

var displayProperties = [];

materialDetails.getPropertyGroupDetails = function (productGroupId, isChemical) {   
        
        if (changeConditionSourceId == 1) {

            var typeString = $("input[name='name']:checked").val();
            var type = parseInt(typeString);
            var searchText = $('#searchTextHolder').val();
            var isChemicalBool = false
            if (isChemical == 'true') {
                isChemicalBool=true
            }

            $('div[id^="propertyData_"]').each(function () {
                $(this).hide();
            });
       
            ini.ajax.post(materialDetails.propertyGroupDataPath, ini.ajax.contentType.Json, { materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, searchText: searchText, type: type, isChemical: isChemicalBool },
                   function (success) {                       
                       $('[id^="accordion-content_' + productGroupId + '"]').html(success)
                       for (var i = 0; i < displayProperties.length; i++) {
                           $('#propertyData_' + displayProperties[i].id + '_' + displayProperties[i].phaseId).show();
                       }
                       convertValuesELS(type);
                       $('.accordion').accordion({ collapsible: true, autoHeight: false });
                       removeProgressIndicator('main');
                       $('.progress-indicator').css('display', 'none');
                   },
                   function (error) {

                   }
               );
           
        } else {


            if (productGroupId == 806)
            {
              
                var conditionId = $('#selectCond_' + productGroupId).val();                
                var type = $("input[name='name']:checked").val();
                var searchText = $('#searchTextHolder').val();

                ini.ajax.post(materialDetails.getMechanicalMetal, ini.ajax.contentType.Json, {
                    materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, searchText: searchText, type: type, conditionId: conditionId
                },
                function (success) {
                    $('[id^="cond_' + productGroupId + '"]').html(success)

                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                }
                );
                
            }
            else if (productGroupId == 807)
            {
                
                    var conditionId = $('#selectCond_' + productGroupId).val();
                    var type = $("input[name='name']:checked").val();
                    var searchText = $('#searchTextHolder').val();

                    ini.ajax.post(materialDetails.getPhysicalMetal, ini.ajax.contentType.Json, {
                        materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, conditionId: conditionId, searchText: searchText, type: type
                    },
                             function (success) {
                                 $('[id^="cond_' + productGroupId + '"]').html(success)

                                 removeProgressIndicator('main');
                                 $('.progress-indicator').css('display', 'none');
                             },
                             function (error) {
                             }
                                );
                }
            }
        }
    

materialDetails.getPropertyGroupDetails_new = function (productGroupId, isChemical) {

    if (changeConditionSourceId == 1) {
        var typeString = $("input[name='name']:checked").val();
        var type = parseInt(typeString);
        var searchText = $('#searchTextHolder').val();
        var isChemicalBool = false
        if (isChemical == 'true') {
            isChemicalBool = true
        }

        $('div[id^="propertyData_"]').each(function () {
            $(this).hide();
        });

        ini.ajax.post(materialDetails.propertyGroupDataPath, ini.ajax.contentType.Json, {
            materialId: changeConditionMaterialID,
            sourceMaterialId: changeConditionSourceMaterialId,
            sourceId: changeConditionSourceId,
            subgroupId: changeConditionSubgroupId,
            groupId: productGroupId,
            searchText: searchText,
            type: type,
            isChemical: isChemicalBool
        },
            function (success) {
                $('[id^="accordion-content_' + productGroupId + '"]').html(success)
                for (var i = 0; i < displayProperties.length; i++) {
                    $('#propertyData_' + displayProperties[i].id + '_' + displayProperties[i].phaseId).show();
                }
                convertValuesELS(type);
                $('.accordion').accordion({
                    collapsible: true,
                    autoHeight: false
                });
                removeProgressIndicator('main');
                $('.progress-indicator').css('display', 'none');
            },
            function (error) {

            }
        );

    } else {
        if (productGroupId == 806) {
            var conditionId = $('#selectCond_' + productGroupId).val();
            var type = $("input[name='name']:checked").val();
            var searchText = $('#searchTextHolder').val();

            ini.ajax.post(materialDetails.getMechanicalMetal, ini.ajax.contentType.Json, {
                materialId: changeConditionMaterialID,
                sourceMaterialId: changeConditionSourceMaterialId,
                sourceId: changeConditionSourceId,
                subgroupId: changeConditionSubgroupId,
                groupId: productGroupId,
                searchText: searchText,
                type: type,
                conditionId: conditionId
            },
                function (success) {
                    $('[id^="cond_' + productGroupId + '"]').html(success)

                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) { }
            );

        } else if (productGroupId == 807) {

            var conditionId = $('#selectCond_' + productGroupId).val();
            var type = $("input[name='name']:checked").val();
            var searchText = $('#searchTextHolder').val();

            ini.ajax.post(materialDetails.getPhysicalMetal, ini.ajax.contentType.Json, {
                materialId: changeConditionMaterialID,
                sourceMaterialId: changeConditionSourceMaterialId,
                sourceId: changeConditionSourceId,
                subgroupId: changeConditionSubgroupId,
                groupId: productGroupId,
                conditionId: conditionId,
                searchText: searchText,
                type: type
            },
                function (success) {
                    $('[id^="cond_' + productGroupId + '"]').html(success)

                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) { }
            );
        }
    }
}






materialDetails.changeCondition=function(productGroupId) {
    if (changeConditionSourceId == 1) {
        var conditionId = $('#selectCond_' + productGroupId).val();
        var type = $("input[name='name']:checked").val();

        var searchText = $('#searchTextHolder').val();

        ini.ajax.post(materialDetails.conditionDataPath, ini.ajax.contentType.Json, { materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, conditionId: conditionId, searchText: searchText, type: type },
             function (success) {                
                 $('[id^="cond_' + productGroupId + '"]').html(success)
                 convertValuesELS(type);
                 removeProgressIndicator('main');
                 $('.progress-indicator').css('display', 'none');
             },
             function (error) {

    }
         );
    } else if (changeConditionSourceId == 2) {



        if (productGroupId == 806) {

        var conditionId = $('#selectCond_' + productGroupId).val();
        var type = $("input[name='name']:checked").val();
            var searchText = $('#searchTextHolder').val();

            ini.ajax.post(materialDetails.getMechanicalMetal, ini.ajax.contentType.Json, { materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, searchText: searchText, type: type, conditionId: conditionId },
              function (success) {
                  $('[id^="cond_' + productGroupId + '"]').html(success)

                  removeProgressIndicator('main');
                  $('.progress-indicator').css('display', 'none');
              },
              function (error) {
              }
             );
        } else if (productGroupId == 807) {

            var conditionId = $('#selectCond_' + productGroupId).val();
            var type = $("input[name='name']:checked").val();
        var searchText = $('#searchTextHolder').val();

            ini.ajax.post(materialDetails.getPhysicalMetal, ini.ajax.contentType.Json, { materialId: changeConditionMaterialID, sourceMaterialId: changeConditionSourceMaterialId, sourceId: changeConditionSourceId, subgroupId: changeConditionSubgroupId, groupId: productGroupId, conditionId: conditionId, searchText: searchText, type: type },
             function (success) {
                 $('[id^="cond_' + productGroupId + '"]').html(success)

                 removeProgressIndicator('main');
                 $('.progress-indicator').css('display', 'none');
             },
             function (error) {
              }
             );
        }
       
    } else {

        var t = $('#selectCond_' + productGroupId).val();
        $('[id^="cond_' + productGroupId + '"]').hide();
        $('#cond_' + productGroupId + '_' + t).show();
             }
    }

materialDetails.showHideProperties = function ( sourceMaterialId, subgroupId, sourceId) {
    if (sourceId != 1) {
    //    if (id == 1) {
            $('#divProperties').show();
            $('#divProcessing').hide();
            $('#divEquivalency').hide();
            $('#aProperties').removeClass('component');
            $('#aProperties').addClass('componentActive');
            $('#aEquivalency').removeClass('componentActive');
            $('#aEquivalency').addClass('component');
            $('#aProcessing').removeClass('componentActive');
            $('#aProcessing').addClass('component');
            $('#CurrentNavigable').html('Properties')
     //   }
        //if (id == 2) {
        //    $('#divProperties').hide();
        //    $('#divProcessing').hide();
        //    $('#divEquivalency').show();
        //    $('#aProperties').removeClass('componentActive');
        //    $('#aProperties').addClass('component');
        //    $('#aEquivalency').removeClass('component');
        //    $('#aEquivalency').addClass('componentActive');
        //    $('#aProcessing').removeClass('componentActive');
        //    $('#aProcessing').addClass('component');
        //    $('#CurrentNavigable').html('Equivalency')
        //}
        //if (id == 3) {
        //    $('#divProperties').hide();
        //    $('#divProcessing').show();
        //    $('#divEquivalency').hide();
        //    $('#aProperties').removeClass('componentActive');
        //    $('#aProperties').addClass('component');
        //    $('#aEquivalency').removeClass('componentActive');
        //    $('#aEquivalency').addClass('component');
        //    $('#aProcessing').removeClass('component');
        //    $('#aProcessing').addClass('componentActive');
        //    $('#CurrentNavigable').html('Processing')
        //}
    }
    else {
        displayProgressIndicator('main');
        $('.progress-indicator').css('display', 'block');
        var searchText = $('#searchTextHolder').val();
     
        $('#divProperties').show();
        $('#divProcessing').hide();
        $('#divEquivalency').hide();
        $('#aProperties').removeClass('component');
        $('#aProperties').addClass('componentActive');
        $('#aEquivalency').removeClass('componentActive');
        $('#aEquivalency').addClass('component');
        $('#aProcessing').removeClass('componentActive');
        $('#aProcessing').addClass('component');
        $('#CurrentNavigable').html('Properties')


        window.location.href = materialDetails.getPropertiesPath + "?sourceMaterialId=" + sourceMaterialId + "&subgroupId= " + subgroupId + "&sourceId=" + sourceId + "&searchText=" + searchText;
       
    }
}

materialDetails.showProcessing= function(sourceMaterialId, subgroupId, sourceId) {

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getProcessingPath, ini.ajax.contentType.JSon, { sourceMaterialId: sourceMaterialId, subgroupId: subgroupId, sourceId: sourceId },
                function (success) {

                    $('#divProcessing').html(success)
                    $('#divProperties').hide();
                    $('#divProcessing').show();
                    $('#divEquivalency').hide();
                    $('#aProperties').removeClass('componentActive');
                    $('#aProperties').addClass('component');
                    $('#aEquivalency').removeClass('componentActive');
                    $('#aEquivalency').addClass('component');
                    $('#aProcessing').removeClass('component');
                    $('#aProcessing').addClass('componentActive');
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                    //showAlert('Error', error);
                }
            );
    //showProgressIndicator('main', 'Loading...');
}
materialDetails.showEquivalency = function(sourceMaterialId, subgroupId, sourceId) {

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(materialDetails.getEquivalencyPath, ini.ajax.contentType.JSon, { sourceMaterialId: sourceMaterialId, subgroupId: subgroupId, sourceId: sourceId },
                function (success) {

                    $('#divEquivalency').html(success)
                    $('#divProperties').hide();
                    $('#divProcessing').hide();
                    $('#divEquivalency').show();

                    $('#aProperties').removeClass('componentActive');
                    $('#aProperties').addClass('component');
                    $('#aEquivalency').addClass('componentActive');
                    $('#aEquivalency').removeClass('component');
                    $('#aProcessing').addClass('component');
                    $('#aProcessing').removeClass('componentActive');

                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                    //showAlert('Error', error);
                }
            );

    //showProgressIndicator('main', 'Loading...');
}

materialDetails.changeConditionMetallography = function(condId) {
    var t = $('#selectCondMetallography').val();
    $('[id^="condMetallography_"]').hide();
    $('#condMetallography_' + t).show();
}


materialDetails.changeConditionManufacturing = function (condId) {
    var t = $('#selectCondManufacturing').val();
    $('[id^="condManufacturing_"]').hide();
    $('#condManufacturing_' + t).show();
}
materialDetails.changeMaterialConditionsSS = function (materialId, subgroupId, plus) {
   

    var conditionId = $('#selectedMaterialConditionSS').val();
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getStressStrainTestConditionsWithDetails, ini.ajax.contentType.JSon, { materialId: materialId, subgroupId: subgroupId, conditionId: conditionId, plus: plus },
                function (results) {
                    $('#stressStrainGroup').html(results);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                    //showAlert('Error', error);
                }
            );
}

materialDetails.changeConditionSS = function () {

    var materialId = $("#ss_sourceMaterialId").html();
    var subgroupId = $("#ss_subgroupId").html();
    var plus = $("#ss_plus").html();


    var conditionId = $('#selectCondSS').val();
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getStressStrainConditionDetails, ini.ajax.contentType.JSon, { materialId: materialId, subgroupId: subgroupId, conditionId: conditionId, plus:plus },
                function (data) {
                    $('#stressStrainPerCondition').html(data.result);
                    stressStrainPlot.InitData(data.model);
                    stressStrainPlot.PlotChartAll();
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                    //showAlert('Error', error);
                }
            );
}

materialDetails.changeConditionSSPlus = function(materialId, subgroupId, plus) {
    var conditionId = $('#selectCondSS').val();
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getStressStrainConditionDetails, ini.ajax.contentType.JSon, { materialId: materialId, subgroupId: subgroupId, conditionId: conditionId, plus: plus },
                function (success) {
                    $('#stressStrainPerCondition').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                    },
                function (error) {
                        //showAlert('Error', error);
                        }
                );
}
materialDetails.changeTemperatureTrueSS=function() {
    var materialId = $('#materialId').val();
    var subgroupId = $('#subgroupId').val();
    var conditionId = $('#selectCondSS').val();
    var temperature = $('#selectTemperature_TrueSS').val();
    selectedTemperatureStressStrain = temperature;
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getStressStrainTemperatureDetails, ini.ajax.contentType.JSon, { materialId: materialId, subgroupId: subgroupId, conditionId: conditionId, temperature: temperature, typeSS: 1 },
                function (success) {
                    $('#stressStrainTableTrue').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                    //showAlert('Error', error);
                }
            );

}

materialDetails.changeTemperatureEngineeringSS=function() {
    var materialId = $('#materialId').val();
    var subgroupId = $('#subgroupId').val();
    var conditionId = $('#selectCondSS').val();
    var temperature = $('#selectTemperature_EngineeringSS').val();
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getStressStrainTemperatureDetails, ini.ajax.contentType.JSon, { materialId: materialId, subgroupId: subgroupId, conditionId: conditionId, temperature: temperature, typeSS: 2 },
                function (success) {
                    $('#stressStrainTableEngineering').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                    //showAlert('Error', error);
                }
            );

}

materialDetails.changeMaterialConditionsFatigueStrain = function (materialId) {
    var materialId = $('#materialId').val();
    var conditionId = $('#selectMaterialCondFatigueStrain').val();

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getFatigueStrainTestConditions, ini.ajax.contentType.JSon, {
                materialId: materialId, conditionId: conditionId
        },
        function (results) {
            $('#fatigueStrainGroup').html(results);
             removeProgressIndicator('main');
             $('.progress-indicator').css('display', 'none');
        },
        function (error) { 
        }
    );
}

materialDetails.changeMaterialConditionFatigueStress = function (materialId) {
    var materialId = $('#materialId').val();
    var conditionId = $('#selectMaterialCondFatigueStress').val();
        
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getFatigueStressTestConditions, ini.ajax.contentType.JSon, {
            materialId: materialId, conditionId: conditionId
               },
                function (results) {
                    $('#fatigueStressGroup').html(results);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                    //showAlert('Error', error);
                }
            );

 }

materialDetails.changeConditionFatigueStrain = function(materialId) {
    var materialId = $('#materialId').val();
    var materialConditionId = $('#selectMaterialCondFatigueStrain').val();
    var conditionId = $('#selectCondFatigueStrain').val();
        


    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getFatigueStrainCondition, ini.ajax.contentType.JSon, {
        sourceMaterialId: materialId, conditionId: conditionId, materialConditionId: materialConditionId
        },
                function (success) {
                    $('#fatigueStrainPerCondition').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {

                }
            );

}

materialDetails.changeConditionFatigueStress = function(materialId) {
    var materialId = $('#materialId').val();
    var conditionId = $('#selectCondFatigueStress').val();
    var materialConditionId = $('#selectMaterialCondFatigueStress').val();


    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getFatigueStressCondition, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, conditionId: conditionId, materialConditionId: materialConditionId
    },
                function (success) {
                    $('#fatigueStressPerCondition').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                    //showAlert('Error', error);
                }
            );

}

materialDetails.changeTemperaturePlusSS=function() {
    var materialId = $('#materialId').val();
    var subgroupId = $('#subgroupId').val();
    var conditionId = $('#selectCondSS').val();
    var temperature = $('#selectTemperature_PlusSS').val();
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getStressStrainTemperatureDetails, ini.ajax.contentType.JSon, { materialId: materialId, subgroupId: subgroupId, conditionId: conditionId, temperature: temperature, typeSS: 2, plus: true },
                function (success) {
                    $('#stressStrainTablePlus').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                    //showAlert('Error', error);
                }
            );

}

materialDetails.changeConditionFatiguePlus = function(materialId) {
    var materialId = $('#materialId').val();
    var conditionId = $('#selectCondFatiguePlus').val();



    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getFatiguePlusCondition, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, conditionId: conditionId },
                function (success) {
                    $('#fatiguePlusPerCondition').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {

                }
            );

}

materialDetails.changeTypeDiagramMultipointData = function(materialId) {
    var materialId = $('#materialId').val();
    var diagramTypeMP = $('#selectMPDiagramType').val();



    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getMultipointDataTypeDiagram, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, diagramTypeMP: diagramTypeMP },
                function (success) {
                    $('#diagramTypes').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {

                }
            );

}

materialDetails.changeConditionMultipointData = function(materialId) {
    var materialId = $('#materialId').val();
    var diagramTypeMP = $('#selectMPDiagramType').val();
    var conditionIdMP = $('#selectMPCond').val();


    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getMultipointDataCondition, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, diagramTypeMP: diagramTypeMP, conditionIdMP: conditionIdMP },
                function (success) {
                    $('#condMP').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {

                }
            );

}

materialDetails.changeDiagramLegendMultipointData = function(materialId) {
    var materialId = $('#materialId').val();
    var diagramTypeMP = $('#selectMPDiagramType').val();
    var conditionIdMP = $('#selectMPCond').val();
    //if(conditionIdMP==null || conditionIdMP==)
    //{
    //    conditionIdMP=1;
    //}
    var legendIdMP = $('#selectMPLegend').val();

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getMultipointDataDiagramLegend, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, diagramTypeMP: diagramTypeMP, conditionIdMP: conditionIdMP, legendIdMP: legendIdMP },
                function (success) {
                    $('#mPTablePoints').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {

                }
            );

}


materialDetails.changeCreepDataMaterialCond = function () {
    var materialId = $('#materialId').val();
    var conditionId = $('#selectCreepDataMaterialCond').val();
 
        displayProgressIndicator('main');
        $('.progress-indicator').css('display', 'block');
        ini.ajax.post(materialDetails.getCreepTestConditions, ini.ajax.contentType.JSon, {
            materialId: materialId, conditionId: conditionId
                },
        function (results) {
                $('#creepGroup').html(results);
                 removeProgressIndicator('main');
                 $('.progress-indicator').css('display', 'none');
                 },
        function (error) {
        }
    ); 
}

materialDetails.changeCreepDataCond = function() {
    var materialId = $('#materialId').val();

    var conditionId = $('#selectCreepDataCond').val();
    var materialConditionId = $('#selectMaterialCondFatigueStrain').val();

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getCreepDataMetal, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, conditionId: conditionId, materialConditionId: materialConditionId },
                function (success) {
                    $('#creepCond').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {

                }
            );

}

materialDetails.changeCreepPlusDataCond = function() {
    var materialId = $('#materialId').val();

    var conditionId = $('#selectCreepPlusDataCond').val();


    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getCreepDataPlus, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, conditionId: conditionId },
                function (success) {
                    $('#creepPlusDiagramPerCondition').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {

                }
            );

}

materialDetails.changeTemperatureCreepPlus = function() {
    var materialId = $('#materialId').val();

    var conditionId = $('#selectCreepPlusDataCond').val();
    var temperature = $('#selectTemperature_CreepPlus').val();


    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getCreepDataPlusTemp, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, conditionId: conditionId, temperature: temperature },
                function (success) {
                    $('#creepPlusDiagramPerCondition').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {

                }
            );

}

materialDetails.changeStressesCreepPlus = function(temp) {
    var materialId = $('#materialId').val();

    var conditionId = $('#selectCreepPlusDataCond').val();
    var temperature = $('#selectTemperature_CreepPlus').val();
    var additional = $('#selectStresses_CreepPlus').val();

    if (!temperature) {
        temperature = temp;
    }

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getCreepDataPlusPoints, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, conditionId: conditionId, temperature: temperature, additional: additional, iso: 0 },
                function (success) {
                    $('#creepPlusTable').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {

                }
            );

}

materialDetails.changeTimesCreepPlus=function(temp) {
    var materialId = $('#materialId').val();

    var conditionId = $('#selectCreepPlusDataCond').val();
    var temperature = $('#selectTemperature_CreepPlus').val();
    var additional = $('#selectTimes_CreepPlus').val();

    if (!temperature) {
        temperature = temp;
    }

    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');
    ini.ajax.post(materialDetails.getCreepDataPlusPoints, ini.ajax.contentType.JSon, { sourceMaterialId: materialId, conditionId: conditionId, temperature: temperature, additional: additional, iso: 1 },
                function (success) {
                    $('#creepPlusTableIso').html(success);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                function (error) {

                }
            );

}

materialDetails.showSelectedRefMP = function () {
    var divRef = $('#selectedReferenceMP');
    var closedA = $('#selectedRefMPLink');
    if (closedA[0].innerHTML == "+ ") {
        divRef.show();
        closedA[0].innerHTML = "- ";
    } else {
        divRef.hide();
        closedA[0].innerHTML = "+ ";
    }
}


materialDetails.showAllRefMP = function () {
    var divRef = $('#allReferenceMP');
    var closedA = $('#allRefMPLink');
    if (closedA[0].innerHTML == "+ ") {
        divRef.show();
        closedA[0].innerHTML = "- ";
    } else {
        divRef.hide();
        closedA[0].innerHTML = "+ ";
    }
}


materialDetails.selectOneValueForSameProperty = function () {


        $("input[type=checkbox]").click(function () {
            selectedBox = this.id;
            var name = $(this).attr("name");
            $("input[name=" +name + "]").each(function () {
                if(this.id == selectedBox && $(this).attr("checked")) {
                    this.checked = true;
                }
                else {
                    this.checked = false;
                };
            });
        });



//    $("input[type=checkbox]").click(function () {

//   var $this = $(e);
//    selectedBox = $this.attr("id");
//            var name = $this.attr("name");
//            $("input[name=" + name + "]").each(function () {
//                if (this.id == selectedBox && $this.attr("checked")) {
//                    this.checked = true;
//                }
//                else {
//                    this.checked = false;
//                };
//            });
//});
}


var selectedChemicalConditionId = null;
var selectedChemicalPropertyId = null;

materialDetails.plotTemperatures=  function (id, rowId, name, phaseId,phaseName, legendX, legendY, condTxt) {
    selectedChemicalConditionId = rowId;
    selectedChemicalPropertyId = id;
    $("#condName_" + id + '_' + phaseId).text(condTxt);

    var type = $("input[name='name']:checked").val();
    var temp;
    switch (type) {
        case '1':
            temp = $('#plot_' + id + '_' + rowId).attr('origvalue');

            break
        case '2':
            temp = $('#plot_' + id + '_' + rowId).attr('defaultvalue');
            break
        case '3':
            temp = $('#plot_' + id + '_' + rowId).attr('usvalue');
            break
       
    }

    chemicalMaterialsPlot.InitData(temp, name, phaseName, legendX, legendY);
    chemicalMaterialsPlot.PlotChartAll(id, phaseId, legendX, legendY);
    $('#propertyData_' + id+'_'+phaseId).hide();
    $('.trOrigin').hide();
    $('.trDefault').hide();
    $('.trUs').hide();
    switch (type) {
        case '1':
            $('.trOrigin').show();

            break
        case '2':
            $('.trDefault').show();
            break
        case '3':
            $('.trUs').show();
            break

    }
    chemicalMaterialsPlot.FillTablePoints(temp, name, phaseName, legendX, legendY, id,phaseId)
    $('#propertyChart_' + id + '_' + phaseId).show();
    var elementId = '#conditionName_' + id + '_' + phaseId;
    materialDetails.scrollToGraph(elementId);

}
materialDetails.closeTemperatureDiagram = function (id, phaseId) {
    $('#propertyData_' + id + '_' + phaseId).show();
    $('#propertyChart_' + id + '_' + phaseId).hide();
}

materialDetails.closeTemperatureDiagrams = function () {
    //$('[id^="propertyData_"]').each(function () {
    //    $(this).show();
    // });
    $('[id^="propertyChart_"]').each(function () {
        $(this).hide();
    });   
}

materialDetails.scrollToGraph = function (elementId) {
    var elementId = elementId;
    $('html, body').animate({
        scrollTop: $(elementId).offset().top
    }, 500);
}

materialDetails.changeDisplayDataForCondition = function (id, phaseId) {
    var title = $('#conditionTitle_' + id + '_' + phaseId).text();
    if (title.indexOf("-") >= 0) {
        $('#propertyData_' + id + '_' + phaseId).hide();
        $('#conditionTitle_' + id + '_' + phaseId).text(title.replace("-", "+"));
    }
    else if (title.indexOf("+") >= 0) {
        $('#propertyData_' + id + '_' + phaseId).show();
        $('#conditionTitle_' + id + '_' + phaseId).text(title.replace("+", "-"));
        var dataObj = {};
        dataObj["id"] = id;
        dataObj["phaseId"] = phaseId;
        displayProperties.push(dataObj);        
    }
}
materialDetails.AddCheckedToComparison = function () {
    var dataForComparisonArray = [];    

    $('.all_forComparison').each(function () {
        
            var ChemicalPropertiesForComparison = {};
            var $this = $(this);
            if ($this.is(":checked")) {
                if ($(this).attr("hasdiagram") != null) {
                    ChemicalPropertiesForComparison.MaterialId = selectedMaterialId;
                    ChemicalPropertiesForComparison.SubgroupId = selectedSubgroupId;
                    ChemicalPropertiesForComparison.ConditionId = $this.attr("conditionid");
                    ChemicalPropertiesForComparison.PropertyId = $this.attr("propertyid");
                    ChemicalPropertiesForComparison.Variable = $this.attr("variable");
                    dataForComparisonArray.push(ChemicalPropertiesForComparison);
                }
            }             
    });

    if (dataForComparisonArray.length > 0)
    {
        displayProgressIndicator('main');
        $('.progress-indicator').css('display', 'block');

        ini.ajax.post(comparisonAddChemicalsPath, ini.ajax.contentType.Json, {
            listOfChemicalProperties: dataForComparisonArray

        },
             function (data) {
                 ShowMessage(data.success);
             },
            function (error) {

            }
        );
    } 
}

materialDetails.showStressStrainDiag = function (no) {
    $('#cond_SS_' + no).css("display", "block");
    $('#cond_SS_Table_' + no).css("display", "none");    
}

materialDetails.hideStressStrainDiag = function (no) {
    $('#cond_SS_' + no).css("display", "none");
    $('#cond_SS_Table_' + no).css("display", "block");
}

//citations
materialDetails.showCitations = function (cit_record_id) {

    ini.ajax.post(materialDetails.getCitation, ini.ajax.contentType.Json,
        {cit_record_id: cit_record_id},
          function (data) {
              $('#citations').html(data.data);
              showPopup({
                  width: 750,
                  height: 300,
                  title: "",
                  close: function () {                    
                  }
              }, $('#citations'), $('#citationsContainer'));
          },
       function (error) {
       }
   );
}

