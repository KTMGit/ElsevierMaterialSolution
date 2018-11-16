var selectedMaterialId = null;

function exporterChangeMaterial(rowId) {

    displayProgressIndicator('main', '');
    $('.progress-indicator').css('display', 'block');

    ini.ajax.post(exporterChangeMaterialPath, ini.ajax.contentType.Json, { rowId: rowId },
                function (data) {

                    $('#materialExportedProperties').html(data);
                    $('#materialProperties').show();
                    $('#exportTypes').hide();
                    $('#exportTypesButton').show();

                    if (selectedMaterialId != null) {
                        $('#arrow_' + selectedMaterialId).css({ "display": "none" });
                                            
                        $('#tr_' + selectedMaterialId).removeClass("greylinkedRow");
                    }
                

                    selectedMaterialId = rowId;
                  
                    $('#arrow_' + rowId).css({ "display": "block" });
                    $('#tr_' + rowId).addClass("greylinkedRow");
                 
                    removeProgressIndicator('main');
                    $('.progress-indicator').css('display', 'none');
                },
                 function (error) {

                 }
             );


}


function removeMaterialsFromExport() {

    displayProgressIndicator('main', '');
    $('.progress-indicator').css('display', 'block');



    var materials = GetcheckedMaterials();

    if (materials == null) {
        showPopup({
            width: 300,
            height: 60,
            title: "",
            close: function () {
                removeProgressIndicator('main');
                $('.progress-indicator').css('display', 'none');
            }
        }, "Please select at least one material for delete.");
        removeProgressIndicator('main');
        $('.progress-indicator').css('display', 'none');

    } else {
       showConfirm("Are you sure you want to remove selected materials from Export?",
          { width: 400, height: 100, title: '', close: null },
          function () {
              window.location.href = removeMaterialsFromExporterPath + '?materials=' + JSON.stringify(materials);          

          },
          function (error) { },
          function (session) { }
     );
    }



   
}

function removePropertyFromExport(materialId, sourceMaterialId, sourceId, subgroupId, propertyId, sourcePropertyId, rowId) {

    displayProgressIndicator('main', '');
    $('.progress-indicator').css('display', 'block');


    showConfirm("Are you sure you want to remove this property from exported material?",
          { width: 400, height: 100, title: '', close: null },
          function () {

              ini.ajax.post(removePropertyFromExporterPath, ini.ajax.contentType.Json, { materialId: materialId, sourceMaterialId: sourceMaterialId, sourceId: sourceId, subgroupId: subgroupId, propertyId: propertyId, sourcePropertyId: sourcePropertyId, rowId: rowId },
               function (data) {

                   if (!data.hasMaterialsAdded) {
                       $("#materialExportedProperties").remove();
                       $("#exportedMaterials").remove();
                       $("#exporterMessage").html("You have not added any materials to Export!");
                       
                       
                   }
                   else if (data.materialRowIdForDeleting > -1 ) {
                       $("#tr_" + data.materialRowIdForDeleting).remove();
                   }
                   $('#materialExportedProperties').html(data.data);
                   $('#materialProperties').show();
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

function GetcheckedMaterials() {
    var materials = null;
    $('#tblMaterials tbody tr').each(function (i, row) {
      
        var $actualrow = $(row);
        $checkbox = $actualrow.find(':checkbox:checked');

        if ($checkbox.length > 0) {
            if (materials == null) {
                materials = [];
            }
            materials.push($checkbox.attr("id"));
        }
    

    });
    return materials;
    
}

function ExportTypesShow() {




    var materials = GetcheckedMaterials();

    if (materials == null) {
        showPopup({
            width: 300,
            height: 60,
            title: "",
            close: function () {
                removeProgressIndicator('main');
                $('.progress-indicator').css('display', 'none');
            }
        }, "Please select at least one material for export.");
        removeProgressIndicator('main');
        $('.progress-indicator').css('display', 'none');
    } else {


        if (selectedMaterialId != null) {
            $('#arrow_' + selectedMaterialId).css({ "display": "none" });

            $('#tr_' + selectedMaterialId).removeClass("greylinkedRow");
        }


        $('#materialProperties').hide();
        $('#exportTypes').show();
        $('#exportTypesButton').hide();
    }
   
    

}

function Export(materialId, sourceMaterialId, sourceId, subgroupId) {

    displayProgressIndicator('main', '');
    $('.progress-indicator').css('display', 'block');

    var allVals = JSON.stringify(KOGetExportTypes());
    var materials = JSON.stringify(GetcheckedMaterials());

    if (allVals == "[]") {

        showPopup({
            width: 300,
            height: 60,
            title: "",
            close: function () {
               
            }
        }, "Please select at least one export type.");

        removeProgressIndicator('main');
        $('.progress-indicator').css('display', 'none');

    } else {
        $('#downloadContainer').attr('src', exportKTM + '?materials=' + materials + '&types=' + allVals);
        removeProgressIndicator('main');
        $('.progress-indicator').css('display', 'none');
    }

}


function KOGetExportTypes() {
    var allVals = [];

   

    if ($("#autodeskNastranCb").is(":checked")) {
        allVals.push($("#autodeskNastranCb").val());
    }

    if ($("#altairCb").is(":checked")) {
        allVals.push($("#altairCb").val());
    }
    if ($("#abaqusCb").is(":checked")) {
        allVals.push($("#abaqusCb").val());
    }
    if ($("#solidWorksCb").is(":checked")) {
        allVals.push($("#solidWorksCb").val());
    }
    if ($("#solidEdgeCb").is(":checked")) {
        allVals.push($("#solidEdgeCb").val());
    }
    if ($("#esiCb").is(":checked")) {
        allVals.push($("#esiCb").val());
    }
    if ($("#esiPamCrashCb").is(":checked")) {
        allVals.push($("#esiPamCrashCb").val());
    }
    if ($("#ExcelCb").is(":checked")) {
        allVals.push($("#ExcelCb").val());
    }
    if ($("#KTMXmlCb").is(":checked")) {
        allVals.push($("#KTMXmlCb").val());
    }
    if ($("#ansysCb").is(":checked")) {
        allVals.push($("#ansysCb").val());
    }
    if ($("#siemensCb").is(":checked")) {
        allVals.push($("#siemensCb").val());
    }
    if ($("#FEMAPCb").is(":checked")) {
        allVals.push($("#FEMAPCb").val());
    }
    if ($("#lsDynaCb").is(":checked")) {
        allVals.push($("#lsDynaCb").val());
    }
    if ($("#nastranCb").is(":checked")) {
        allVals.push($("#nastranCb").val());
    }
    if ($("#ptcCreoCb").is(":checked")) {
        allVals.push($("#ptcCreoCb").val());
    }
    return allVals;
}



