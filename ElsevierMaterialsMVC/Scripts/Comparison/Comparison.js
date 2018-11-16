function removeMaterialFromComparison(materialId, subgroupId, sourceId, sourceMaterialId) {
    showConfirm("Are you sure you want to remove this material from comparison?",
           { width: 400, height: 100, title: '', close: null },
           function () {     
               removeFromServerMaterial(materialId, subgroupId, sourceId, sourceMaterialId);
           },
           function (error) { },
           function (session) { }
      );   
}
function removeFromServerMaterial(materialId, subgroupId, sourceId, sourceMaterialId) {
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(comparisonRemoveMaterialPath, ini.ajax.contentType.Json, { materialId: materialId, subgroupId: subgroupId, sourceId: sourceId, sourceMaterialId: sourceMaterialId },
          function (data) {                        
              showAlert(options = { width: 300, height: 50, title: '', message: "Successfully removed from comparison!", close: function () { } });
              $("#comaprisonProperties").html(data.data);
              $("#comaprisonPlot").html(data.plot);
              comparisonPlot.InitRadarChart(data.model);
              comparisonPlot.InitXYChart(data.model);

              removeProgressIndicator('main');
              $('.progress-indicator').css('display', 'none');

              if (data.hasProperties) {
                  showPopup({
                      width: 300,
                      height: 60,
                      title: "",
                      close: function () {
                          //function on close
                      }
                  }, "Successfully removed from comparison!");
     
                  
              }             
          },
          function (error) {

          }
      );
}
function openDiagramForProperty(propertyId, sourcePropertyId) {
    ini.ajax.post(showDiagram, ini.ajax.contentType.Json, { propertyId: propertyId, sourcePropertyId: sourcePropertyId },
       function (data) {

           $('#comparison').html(data.data);

           showPopup({
               width: 900,
               height: 530,
               title: "",
               close: function () {
                   //function on close
               }
           }, $('#comparison'), $('#comparisonContainer'));       
       },
       function (error) {

       }
   );
}



function removeProperty(propertyTypeId, sourcePropertyTypeId, rowId) {

    showConfirm("Are you sure you want to remove this property from comparison?",
       { width: 400, height: 100, title: '', close: null },
       function () {
      

           displayProgressIndicator('main');
           $('.progress-indicator').css('display', 'block');

           ini.ajax.post(removePropertyPath, ini.ajax.contentType.Json, { propertyId: propertyTypeId, sourcePropertyId: sourcePropertyTypeId, rowId: rowId },
                 function (data) {
                     showAlert(options = { width: 300, height: 50, title: '', message: "Successfully removed from comparison!", close: function () { } });
                     $("#comaprisonProperties").html(data.data);
                     $("#comaprisonPlot").html(data.plot);
                     comparisonPlot.InitRadarChart(data.model);
                     comparisonPlot.InitXYChart(data.model);

                     removeProgressIndicator('main');
                     $('.progress-indicator').css('display', 'none');

                     if (data.hasProperties) {
                         showPopup({
                             width: 300,
                             height: 60,
                             title: "",
                             close: function () {
                                 //function on close
                             }
                         }, "Successfully removed from comparison!");


                     }
                 },
                 function (error) {

                 }
             );




       },
       function (error) { },
       function (session) { }
    );

}


function btnCompareTableClick() {
    $("#comaprisonPlot").hide();
    $("#comaprisonProperties").show();
}

function btnComparePlotClick() {
    $("#comaprisonProperties").hide();
    $("#comaprisonPlot").show();

}

function removeCurve(id, propertyId) {

    showConfirm("Are you sure you want to remove this property from comparison?",
     { width: 400, height: 100, title: '', close: null },
     function () {

         displayProgressIndicator('main');
         $('.progress-indicator').css('display', 'block');

         ini.ajax.post(removeCurveFromProperty, ini.ajax.contentType.Json, {
             curveId: id, propertyid: propertyId
         },
         function (result) {
             if (result.comparisonHasPropeties) {
                 if (result.removedProperrty) {
                     $("#comaprisonProperties").html(result.data);
                     $('#btnBackToTableView').hide();
                 } else {

                     interactiveDiagram.general.resetObjectData();
                     seriesData = new Array();
                     legendData = new Array();
                     
                     $('#comparisonInteractiveDiagrams').html(result.data);

                     showPopup({
                             width: 300,
                             height: 60,
                             title: "",
                             close: function () {
                                 //function on close
                             }
                         }, "Successfully removed from comparison!");                    

                 }

             } else {
                 $("#comparisonCont").html('<div style="color: rgb(102,102,102);margin-left: 5px;margin-top: 15px;">You have not added any materials to Comparison!</div>');
             }
             removeProgressIndicator('main');
             $('.progress-indicator').css('display', 'none');
         },
        function (error) {

        }
    );




     },
     function (error) { },
     function (session) { }
  );






}



function showComparisonDiagram(id) {
   
    ini.ajax.post(showInterctiveDiagramPath, ini.ajax.contentType.Json, {
            diagramId: id
        },
        function (result) {

            removeProgressIndicator('main');
            $('.progress-indicator').css('display', 'none');
            $('#comparisonInteractiveDiagrams').html(result.data);
            $('#comparisonTableContainer').hide();
            $('#btnBackToTableView').show();
            $('#comparisonInteractiveDiagrams').show();
            $('#btnCompareTable').hide();
            $('#btnComparePlot').hide();

        },
        function (error) { }
    );
}

function removeInteractiveDiagram(id) {

    showConfirm("Are you sure you want to remove this property from comparison?",
     { width: 400, height: 100, title: '', close: null },

     function () {

         displayProgressIndicator('main');
         $('.progress-indicator').css('display', 'block');

         ini.ajax.post(removeInteractiveDiagramPath, ini.ajax.contentType.Json, {
             diagramId: id
         },
         function (data) {
             showAlert(options = { width: 300, height: 50, title: '', message: "Successfully removed from comparison!", close: function () { } });
             $("#comaprisonProperties").html(data.data);
             $("#comaprisonPlot").html(data.plot);
             comparisonPlot.InitRadarChart(data.model);

             removeProgressIndicator('main');
             $('.progress-indicator').css('display', 'none');

             if (data.hasProperties) {
                 showPopup({
                     width: 300,
                     height: 60,
                     title: "",
                     close: function () {
                         //function on close
                     }
                 }, "Successfully removed from comparison!");


             }
         },
         function (error) { }
     );
     },
     function (error) { },
     function (session) { }
  );
}



function changeInteractiveCurve() {
  ShowPointsForCurve($("#curvesSelection").val());    
}
function btnSpiderPlotClick() {
    $("#xyPlotContainer").hide();
    $("#spiderPlotContainer").show();
}

function btnXYPlotClick() {
    $("#spiderPlotContainer").hide();
    $("#xyPlotContainer").show();
}

function ShowPointsForCurve(id) {
    $('[id^="tablePointsContainer_"]').hide();
    $('[id^="tablePointsContainer_"]').each(function () {
        $(this).hide();
    });
    $('#tablePointsContainer_' + id).show();
}

function showInteractiveDiagramSelected(id) {

    interactiveDiagram.general.resetObjectData();
    var selectedXName = $('#XName').val();
    displayProgressIndicator('main');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(showInterctiveDiagramSelectedPath, 
    ini.ajax.contentType.Json, 
    {
        diagramId: id, selectedXValue: selectedXName
    },

              function (results) {
                  $('#interactiveDiagramSelected').html(results.data);
                  removeProgressIndicator('main');
                  $('.progress-indicator').css('display', 'none');
              },
              function (error) {                 
              }
          );
}

