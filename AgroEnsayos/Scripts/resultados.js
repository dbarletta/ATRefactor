
$(document).ready(function () {
    /*
    if ($("#detailstable").height() > $("#div_filter").height()) {
        $("#div_filter").height($("#detailstable").height());
    }else{
        $("#detailstable").height($("#div_filter").height());
    }
    */
    $("#select_agroup").val($("#h_select_agroup").val());

});


function OpenValues(id_val)
{
    if ($('#' + id_val).is(":visible"))
        $('#' + id_val).hide();
    else
        $('#' + id_val).show();

}

function MostrarDt(element) {
    var dt = $(element);
    var dd = dt.next();
    if (dd.css("display") == "block") {
        dt.addClass("collapsed");
        dd.slideUp(150);
    }
    else {
        dt.removeClass("collapsed");
        dd.slideDown(300);
    }
}

function sendFilter(elem_name,filter_name)
{
    $('#new_id_filter').val($('#' + elem_name).val());
    $('#new_val_filter').val($('#' + elem_name + ' option:selected').text());
    $('#new_name_filter').val(filter_name);

    $('#Resultform').submit();
}

function sendFilter_a(elem_name, filter_name,index_atr,itemID) {
    //alert(itemID);
    //alert($('#' + elem_name + index_atr).text());
    $('#new_id_filter').val(itemID);
    $('#new_val_filter').val($('#' + elem_name + index_atr).text());
    $('#new_name_filter').val(filter_name);

    $('#Resultform').submit();
}

function removeFilter(id_fil)
{
    $('#remove_id_filter').val(id_fil);
    $('#Resultform').submit();
}

function sendSubmit(id_sort)
{
    $('#newSort').val(id_sort);
    $('#Resultform').submit();
}

function AgroupSubmit()
{
    $('#Resultform').submit();
}

function SaveChkComp(id)
{
    //alert(id);
    if ($("#" + id).is(':checked')) {
        if ($("#comp1").val() == 0)
            $("#comp1").val(id);
        else if ($("#comp2").val() == 0)
            $("#comp2").val(id);
        else if ($("#comp3").val() == 0)
            $("#comp3").val(id);
        else if ($("#comp4").val() == 0)
            $("#comp4").val(id);
        else if ($("#comp5").val() == 0)
            $("#comp5").val(id);
        else {
            alert('Se pueden comparar hasta 5 productos');
            $("#check_" + id).attr('checked', false);
        }
    } else {
        if ($("#comp1").val() == id)
            $("#comp1").val(0);
        else if ($("#comp2").val() == id)
            $("#comp2").val(0);
        else if ($("#comp3").val() == id)
            $("#comp3").val(0);
        else if ($("#comp4").val() == id)
            $("#comp4").val(0);
        else if ($("#comp5").val() == id)
            $("#comp5").val(0);

    }
}

function SendCompare() {
    var page = "Comparar";
    var param = '';
    param = 'CategoriaIdProductos=' + $('#CategoriaIdProductos').val();
    param = param + '&BuscarProductos=' + $('#BuscarProductos').val();
    param = param + '&id1=' + $('#comp1').val().substring(6);
    param = param + '&id2=' + $('#comp2').val().substring(6);
    param = param + '&id3=' + $('#comp3').val().substring(6);
    param = param + '&id4=' + $('#comp4').val().substring(6);
    param = param + '&id5=' + $('#comp5').val().substring(6);

    /*
    param = param + '&id1=' + $('#comp1').val();
    param = param + '&id2=' + $('#comp2').val();
    param = param + '&id3=' + $('#comp3').val();
    param = param + '&id4=' + $('#comp4').val();
    param = param + '&id5=' + $('#comp5').val();
    */
    window.location.href = page + '?' + param;

}

function SendTestimonios() {
    var page = "Testimonios";
    var param = 'CategoriaIdProductos=' + $('#CategoriaIdProductos').val();
    param = param + '&BuscarProductos=' + $('#BuscarProductos').val();

    window.location.href = page + '?' + param;

}

function SendProducto(id) {
    var page = "Ficha";
    var param = 'id=' + id + '&CategoriaIdProductos=' + $('#CategoriaIdProductos').val();
    param = param + '&BuscarProductos=' + $('#BuscarProductos').val();

    window.location.href = page + '?' + param;

}

//paginacion
function GetProductoFromNextPage(scrollPage) {
    var lastRowId = ($("#lastRowId").val() * 1);
    var isHistoryBack = (lastRowId > 0 && !scrollPage);

    var listFilter = new Array();

    $("input:hidden.listFilter").each(function () {
        listFilter.push($(this).val());
    });

    if ($('#detailstable').height() > $('#div_filter').height())
        $('#div_product').height($('#detailstable').height() + 60);
    else
        $('#div_product').height($('#div_filter').height());
    $('#loading').show();

    $.ajax({
        type: 'POST',
        traditional: true,
        url: 'GetPageProducts',
        data: JSON.stringify({
            lastRowId: lastRowId
                                , isHistoryBack: isHistoryBack
                                , list_filters: listFilter
                                , select_agroup: $("#h_select_agroup").val()
                                , newSort: $("#newSort").val()
                                , CategoriaIdProductos: $("#CategoriaIdProductos").val()
                                , BuscarProductos: $("#BuscarProductos").val()
        }),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var rowid = lastRowId;

            if (data == null) {
                return;
            }

            var articleData = "";
            var selectAgroup = $("#h_select_agroup").val();
            var selectAgroupAux = $("#h_select_agroup_aux").val();

            $.each(data, function (index, section) {
                var sectionIndex = (isHistoryBack ? index + 1 : rowid);
                //alert(JSON.stringify(section));

                if (section.AlturaPlanta == "0")
                    section.AlturaPlanta = "-";
                if (section.Ciclo == "")
                    section.Ciclo = "-";
                if (section.Material == "")
                    section.Material = "-";
                if (section.DiasMadurez == "0")
                    section.DiasMadurez = "-";

                switch (selectAgroup) {
                    case "empresa":
                        if (selectAgroupAux != section.Empresa || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="6" class="agroup_td">' + section.Empresa + '</td></tr>';
                            selectAgroupAux = section.Empresa;
                        }
                        break;
                    case "nombre":
                        if (selectAgroupAux != section.Nombre || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="6" class="agroup_td">' + section.Nombre + '</td></tr>';
                            selectAgroupAux = section.Nombre;
                        }
                        break;
                    case "ciclo":
                        if (selectAgroupAux != section.Ciclo || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="6" class="agroup_td">' + section.Ciclo + '</td></tr>';
                            selectAgroupAux = section.Ciclo;
                        }
                        break;
                    case "madurez":
                        if (selectAgroupAux != section.DiasMadurez || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="6" class="agroup_td">' + section.DiasMadurez + '</td></tr>';
                            selectAgroupAux = section.DiasMadurez;
                        }
                        break;
                    case "altura":
                        if (selectAgroupAux != section.AlturaPlanta || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="6" class="agroup_td">' + section.AlturaPlanta + '</td></tr>';
                            selectAgroupAux = section.AlturaPlanta;
                        }
                        break;
                    case "material":
                        if (selectAgroupAux != section.Material || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="6" class="agroup_td">' + section.Material + '</td></tr>';
                            selectAgroupAux = section.Material;
                        }
                        break;

                }

                esnuevo = "";
                if (section.EsNuevo)
                    esnuevo = "esnuevo";

                articleData += '<tr>' +
                                 '<td><input type="checkbox" class="checkboxTable" title="Clickear aquí para agregar este producto a la comparación" id="check_' + section.Id + '" onclick="SaveChkComp(\'check_' + section.Id + '\')">' +
                               '<td class="' + esnuevo + '"><a href="javaScript:void(0);" class="table_link" onclick="SendProducto(' + section.Id + ');" alt="Click aquí para ver la ficha de este producto" title="Click aquí para ver la ficha de este producto">' + section.Nombre + '</a></td>' +
                               '<td>' + section.Empresa + '</td>';

                if ($("#CategoriaIdProductos").val() == 6) {
                    articleData += '<td>' + section.DiasMadurez + '</td>';
                }
                else {
                    articleData += '<td>' + section.Ciclo + '</td>';
                }

                articleData += '<td>' + section.AlturaPlanta + '</td>' +
                               '<td>' + section.Material + '</td>' +
                    '</tr>';

                rowid = rowid + 1;
            });

            $("#h_select_agroup_aux").val(selectAgroupAux);
            $("#productList").append(articleData);
            
            if (!isHistoryBack) {
                $("#lastRowId").val(rowid);
            }

            $('#div_product').height($('#detailstable').height()+60);
            $('#loading').hide();
        },
        error: function (data) {
            alert(data.error);
        }
    });

    return false;
}


