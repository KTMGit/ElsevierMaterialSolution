var tableFilters = {};
tableFilters.selectedColumn = null;

tableFilters.SubgroupListColumnsGroupsEnum = {
    None : 0,
    MaterialInfo : 1,
    SubgroupList: 2,
    SearchResults: 3
}

tableFilters.SubgroupListColumnsGroups = new Array();

tableFilters.SubgroupListColumnsGroup = function () {
    this.columnsGroup = tableFilters.SubgroupListColumnsGroupsEnum.None;
    this.columns = new Array();
    this.tableName = null;
}

tableFilters.getListOfColumnsForSelectedTableId = function (tableId) {
    for(var i = 0; i < tableFilters.SubgroupListColumnsGroups.length; i++) {
        if(tableFilters.SubgroupListColumnsGroups[i].tableName == tableId) {
            return tableFilters.SubgroupListColumnsGroups[i].columns;
        }
    }
    return null;
}

tableFilters.getListOfColumnsForSelectedColumnsGroup = function (columnsGroup) {
   for(var i = 0; i < tableFilters.SubgroupListColumnsGroups.length; i++) {
       if(tableFilters.SubgroupListColumnsGroups[i].columnsGroup == columnsGroup) {
           return tableFilters.SubgroupListColumnsGroups[i].columns;
        }
    }
    return null;
}

tableFilters.changeVisibility = function (cb, tableId, filtersGroup) {
 
    var isVisible = $(cb).is(':checked');
    var columnId = parseInt($(cb).attr('colid'));
    tableFilters.reloadTable(tableId, columnId, isVisible, filtersGroup);
}


tableFilters.reloadTable = function (tableId, columnId, isVisible, filtersGroup) {

    var columns = tableFilters.getListOfColumnsForSelectedTableId(tableId);

    var path = tableFilters.SetPathValue(tableId);
    var isFilterChanged = false;
    var isColumnVisibilityChanged = false;

        $('.columns_' +tableId).find('.cbSelectColumn1').each(function (index, item) {
            var isVisible = $(this).is(':checked');
            var columnId = parseInt($(this).attr('colid'));
            var filter = '';
            jQuery.each(columns, function (index2, item2) {
                var id = item2.Id;
                if (id == columnId) {

                    for (var i = 0; i < columns.length; i++) {
                        if (columns[i].Id == columnId) {
                            $("#typeFilter").val(columns[i].Filter);
                                filter = columns[i].Filter;
                            break;
                        }
                    }
             
                    //filter = $.trim($('input#typeFilter').val()); 
                
                    isFilterChanged = isFilterChanged ||(item2.Filter != filter);
                    isColumnVisibilityChanged = isColumnVisibilityChanged || (item2.isVisible != isVisible);
                    item2.isVisible = isVisible;
                    item2.Filter = filter;
                    tableFilters.setColumnVisibility(columnId, isVisible, tableId);
                };
                });
            $(this).attr('filter', filter);
        });
    tableFilters.saveChangesToSession(isFilterChanged, isColumnVisibilityChanged, path, filtersGroup, tableId);
    tableFilters.setAllColumnsVisibility(columns, tableId);
    searchFunc.closeColumnsSelector();
}

tableFilters.fillColumnsGroup = function (tableName) {
    searchFunc.columnsStatus = new Array();
    $('#' + tableName + ' tr:first').each(function () {
        var this_row = $(this);
        $(this_row).children().each(function (i, el) {
            var columnId = $.trim($(this).attr('colid'));
            var isVisible = $.trim($(this).attr('isvisible'));
            isVisible = (isVisible == 'true' ? true : false);
            var name = $.trim($(this).text());
            var filter = $.trim($(this).attr('filter'));

            var def = new searchFunc.columnDefinition();
            def.Id = columnId;
            def.isVisible = isVisible;
            def.Name = name;

            for (var i = 0; i < columnsFromServer.length; i++) {
                if (columnsFromServer[i].Id == def.Id) {
                    def.Filter = columnsFromServer[i].Filter;
                    def.isVisible = columnsFromServer[i].isVisible;
                 }
        }
        if (tableName == "materialsSubgroupList" && def.Name == "") {
        } else {
            searchFunc.columnsStatus.push(def);
        }
               
       });
                });
    $(".columnsSelector").hide();
    return searchFunc.columnsStatus;
}

tableFilters.initColumnList = function (tableName) {
    
    var def = new tableFilters.SubgroupListColumnsGroup();
    def.tableName = tableName;

    if (tableName == "materialList") {
        def.columnsGroup = tableFilters.SubgroupListColumnsGroupsEnum.SearchResults;
    }else if(tableName == "materialsSubgroupList" ){
        def.columnsGroup = tableFilters.SubgroupListColumnsGroupsEnum.SubgroupList;
    }
   else {
        def.columnsGroup = tableFilters.SubgroupListColumnsGroupsEnum.MaterialInfo;
    }
    //
    //"materialDetailsInfoTable"

        def.columns  = tableFilters.fillColumnsGroup(tableName);
        tableFilters.SubgroupListColumnsGroups.push(def);
}

tableFilters.setAllColumnsVisibility = function (colDefinitions, tableName) {

    var columns = tableFilters.getListOfColumnsForSelectedTableId(tableName);

   if (columns != null) {
       jQuery.each(columns, function (index, item) {
           tableFilters.setColumnVisibility(parseInt(item.Id), item.isVisible, tableName);
           tableFilters.setColumnFilterImage(parseInt(item.Id), item.Filter, tableName);
        });
    }
}

tableFilters.setColumnVisibility = function (columnId, isVisible, tableName) {
    var selectedColumnId =parseInt(columnId)+ 1;
    if (isVisible) {
        $('#' + tableName + ' th:nth-child(' + (selectedColumnId) + ')').show();
        $('#' + tableName + ' td:nth-child(' + (selectedColumnId) + ')').show();
    }
    else {
        if (selectedColumnId == tableFilters.selectedColumn) {
            $(".menuFilters").hide();
        }
        $('#' + tableName + ' th:nth-child(' + (selectedColumnId) + ')').hide();
        $('#' + tableName + ' td:nth-child(' + (selectedColumnId) + ')').hide();
    }
}

tableFilters.setColumnFilterImage = function (columnId, filter, tableName) {
       var selectedColumnId = parseInt(columnId) +1;
       $('#columnFilterImg_' + selectedColumnId).remove();
    if (filter != '') {
        $('#' + tableName + ' th:nth-child(' + (selectedColumnId) + ')').append("<img id='columnFilterImg_" + selectedColumnId + "' src='/Content/images/filter_icon.png'/>");
    } else {
        $('#columnFilterImg_' + selectedColumnId).remove();
    }
}

tableFilters.SetPathValue = function (tableId) {
        if (tableId == "materialDetailsInfoTable") {
              path = searchFunc.applyMaterialDetailsTableFilters;
        }
        else if (tableId == "materialInfoTable" || tableId == "materialsSubgroupList") {
            path = searchFunc.applySubgroupsFiltersColumnsPath;
        }
        else if (tableId == "materialList") {
              path = searchFunc.applySearchResutsTableFilters;
              }
   return path;
}

tableFilters.saveChangesToSession = function (isFilterChanged, isColumnVisibilityChanged, path, filtersGroup, tableId) {
     
    if (isFilterChanged || isColumnVisibilityChanged) {
             var filters = new Object();
             filters.AllFilters = tableFilters.getListOfColumnsForSelectedTableId(tableId);
             ini.ajax.post(path, ini.ajax.contentType.Json, { filters: filters, type: filtersGroup
             },
             function (result) {
                if(isFilterChanged) {
                    if (tableId== "materialList") {
                         searchFunc.fullTextSearch(1);
                    }
                 }
             },
             function (error) {}
             );
    }
}

tableFilters.initClickOnBody = function (tableId) {

        $("body").click(function (e) {

            if ($(e.target).attr('class') === "headerSearch" && $(e.target).closest('table').attr('id') === tableId) {
                var $divMenu = $('#divMenu_' + tableId);
                var menuWidth = $divMenu.width();
                var menuHeight = $divMenu.height();
                var senderOffset = $(e.target).offset();
                var senderWidth = $(e.target).width();
                var left = senderOffset.left + senderWidth + 6;
                var top = senderOffset.top + 13;
                $divMenu.css('left', left + 'px');
                $divMenu.css('top', top + 'px');
                var colName = $(e.target).parent().attr('colName')
                tableFilters.selectedColumn = $(e.target).parent().attr('colid');
                var filters = new Object();
                filters.AllFilters = tableFilters.getListOfColumnsForSelectedTableId(tableId);
                
                for (var i = 0; i < filters.AllFilters.length; i++) {
                    if (filters.AllFilters[i].Id == tableFilters.selectedColumn) {
                        $("#typeFilter").val(filters.AllFilters[i].Filter);
                        break;
                    }
                 }
              
                $("#filterMenuColumn").val(colName);
                $divMenu.show();
            }
            else {
                var container = $("#divMenu_" + tableId);
                if ((!container.is(e.target) // if the target of the click isn't the container...
                && container.has(e.target).length === 0) || ($(e.target).attr('class') === "headerSearch" && $(e.target).closest('table').attr('id') === tableId)) // ... nor a descendant of the container
                {
                    container.hide();
                    $("#filterMenuColumn").val('');

                }
            }
        });
 }
//???!!!
 tableFilters.sortTable = function (ord, containerId) {
     var prop = $("#filterMenuColumn").val();
    _sortGrid(containerId, prop, ord);
    searchFunc.setAllColumnsVisibility(columnsFromServer);
 }

tableFilters.Page = {
    None: 0,
    SearchResuts : 3,
    SubgroupList : 4

}
/*Input field*/

tableFilters.initEnterOnFilterInputField = function (tableName, filtersGroup) {
    $("#typeFilter").keyup(function (event) {
        if (event.keyCode == 32) {
            $("#typeFilter").val($("#typeFilter").val() + " ");
        }
        if (event.keyCode == 13) {
            if (filtersGroup == 2 && tableName == "materialsSubgroupList" || tableName == "materialList") {
                 tableFilters.filterResults($("#typeFilter").val(), page, pageName, filtersGroup);
        }
             return false;
        }

    });
   $('#typeFilter').focus();
}


tableFilters.filterResults = function (filterText, page, tableName, filtersGroup) {
    var path = null;
     displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    switch(page) {
        case tableFilters.Page.SearchResuts:

            var filters = new Object();
            filters.AllFilters = tableFilters.getListOfColumnsForSelectedTableId(tableName);

            for (var i = 0; i < filters.AllFilters.length; i++) {
                if (filters.AllFilters[i].Id == tableFilters.selectedColumn) {
                    filters.AllFilters[i].Filter = filterText;
                    break;
                    }
            }
            path = searchFunc.applySearchResutsTableFilters;
            ini.ajax.post(path, ini.ajax.contentType.Json, {
            filters: filters
            },
            function (result) {

                ini.ajax.post(searchFunc.searchResultsApplyInputFilter, ini.ajax.contentType.Json, null,
                function (result) {
                        $("#resultsResizable").html(result);
                        removeProgressIndicator('main');
                        $('.progress-indicator').css('display', 'none');
                },
                function (error) {
                });
            },
            function (error) {
            });

            break;

        case tableFilters.Page.SubgroupList:

            var filters = new Object();
            filters.AllFilters = tableFilters.getListOfColumnsForSelectedColumnsGroup(filtersGroup);

              for (var i = 0; i < filters.AllFilters.length; i++) {
                     if(filters.AllFilters[i].Id == tableFilters.selectedColumn) {
                          filters.AllFilters[i].Filter = filterText;
                          break;
                    }
                }

            var materialId = $("#MaterialId").val();
        var fullTextSearch = $("#tbFullTexSearch").val();
            path = searchFunc.applySubgroupsFiltersColumnsPath;


            ini.ajax.post(path, ini.ajax.contentType.Json, {
                    filters: filters, type: filtersGroup
            },
            function (result) {

                  var prop = $("#filterMenuColumn").val();
                  _sortGrid("subgroupListTableContainer", prop, SortOrder.Ascending);
                  searchFunc.setAllColumnsVisibility(columnsFromServer);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                  },
            function (error) {
            });

            break;
    }
}

function clearAllTableFilters() {
  ini.ajax.post(searchFunc.resetAllSubgroupListResultsFilter, ini.ajax.contentType.Json, {
                    },
                    function (result) {
                        var prop = $("#filterMenuColumn").val();
                          if (prop == "") {
                               prop = "SourceId";
                           }
                          _sortGrid("subgroupListTableContainer", prop, SortOrder.Ascending);
                          searchFunc.setAllColumnsVisibility(columnsFromServer);
                          },
                    function (error) {
    });             
}

function clearAllSearchResultsTableFilters() {
     displayProgressIndicator('main');
     $('.progress-indicator').css('display', 'block');
     ini.ajax.post(searchFunc.resetAllSearchResultsFilter, ini.ajax.contentType.Json, {
            },
            function (result) {
                var prop = $("#filterMenuColumn").val();
                    if (prop == "") {
                        prop = "Name";
                    }
                    _sortGrid("resultsResizable", prop, SortOrder.Ascending);
                    searchFunc.setAllColumnsVisibility(columnsFromServer);
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');

                    },
            function (error) {
        });       
}

function clearFilterForSelectedColumn(tableId) {
     displayProgressIndicator('main');
     $('.progress-indicator').css('display', 'block');

    var columnId = tableFilters.selectedColumn;
    if (tableId == "materialList") {
      ini.ajax.post(searchFunc.resetSearchResultsFilter, ini.ajax.contentType.Json, {
                 columnId: columnId
         },
                function (result) {

                        ini.ajax.post(searchFunc.searchResultsApplyInputFilter, ini.ajax.contentType.Json, null,
                        function (result) {
                            $("#resultsResizable").html(result);
                                removeProgressIndicator('main');
                                $('.progress-indicator').css('display', 'none');
                        },
                        function (error) {
                        });
           
         },
                function (error) {
        });       
     }
     else
     {
        ini.ajax.post(searchFunc.resetSubgroupListResultsFilterForSelectedColumn, ini.ajax.contentType.Json, {
                 columnId: columnId
         },
                function (result) {

                      ini.ajax.post(subgroupListResutsPath, ini.ajax.contentType.Json, null,
                        function (result) {
                              $("#subgroupListTableContainer").html(result);
                              removeProgressIndicator('main');
                              $('.progress-indicator').css('display', 'none');
                        },
                        function (error) {
                        });
         },
                function (error) {
        });   
     }
   
}




