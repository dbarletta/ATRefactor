
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

function sendFilter_a(elem_name, filter_name, index_atr, itemID) {
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

function SendProducto(id) {
    var page = "../Productos/Ficha";
    var param = 'id=' + id + '&CategoriaIdProductos=' + $('#CategoriaIdEnsayos').val();
    param = param + '&BuscarProductos=' + $('#BuscarEnsayos').val();

    window.location.href = page + '?' + param;

}

//paginacion
function GetEnsayoFromNextPage(scrollPage) {
    var lastRowId = ($("#lastRowId").val() * 1);
    var isHistoryBack = (lastRowId > 0 && !scrollPage);

    var listFilter = new Array();

    $("input:hidden.listFilter").each(function () {
        listFilter.push($(this).val());
    });

    if ($('#detailstable').height() > $('#div_filter').height())
        $('#div_product').height($('#detailstable').height()+60);
    else
        $('#div_product').height($('#div_filter').height());
    $('#loading').show();

    $.ajax({
        type: 'POST',
        traditional: true,
        url: 'GetPageEnsayos',
        data: JSON.stringify({
            lastRowId: lastRowId
                                , isHistoryBack: isHistoryBack
                                , list_filters: listFilter
                                , select_agroup: $("#h_select_agroup").val()
                                , newSort: $("#newSort").val()
                                , CategoriaIdEnsayos: $("#CategoriaIdEnsayos").val()
                                , BuscarEnsayos: $("#BuscarEnsayos").val()
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

                switch (selectAgroup) {
                    case "campana":
                        if (selectAgroupAux != section.Campana || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="9" class="agroup_td">' + section.Campana + '</td></tr>';
                            selectAgroupAux = section.Campana;
                        }
                        break;
                    case "fuente":
                        if (selectAgroupAux != section.Fuente || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="9" class="agroup_td">' + section.Fuente + '</td></tr>';
                            selectAgroupAux = section.Fuente;
                        }
                        break;
                    case "producto":
                        if (selectAgroupAux != section.Producto || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="9" class="agroup_td">' + section.Producto + '</td></tr>';
                            selectAgroupAux = section.Producto;
                        }
                        break;
                    case "provincia":
                        if (selectAgroupAux != section.Provincia || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="9" class="agroup_td">' + section.Provincia + '</td></tr>';
                            selectAgroupAux = section.Provincia;
                        }
                        break;
                    case "localidad":
                        if (selectAgroupAux != section.Localidad || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="9" class="agroup_td">' + section.Localidad + '</td></tr>';
                            selectAgroupAux = section.Localidad;
                        }
                        break;
                    case "rinde":
                        if (selectAgroupAux != section.Rinde || selectAgroupAux == "") {
                            articleData += '<tr class="agroup_tr"><td colspan="9" class="agroup_td">' + section.Rinde + '</td></tr>';
                            selectAgroupAux = section.Rinde;
                        }
                        break;

                }

                articleData += '<tr>';
                
                articleData += '<td><a href="#" onclick=\'SendFiltro1("' + section.Fuente + '","' + section.Campana + '","' + section.Localidad + '","' + section.CampanaId + '")\'><img src="../Images/verensayo.png" title="Haga un click aqui para ver este ensayo completo" /></a></td>';
                articleData += '<td><a href="#" onclick=\'SendFiltro2("' + $("#CategoriaIdEnsayos").val() + '","' + section.Fuente + '","' + section.LugarId + '","' + section.CampanaId + '")\'><img src="../Images/Chart_16x16.png" title="Haga un click aqui para ver el gráfico de este ensayo" /></a></td>';

                articleData += '<td>' + section.Campana + '</td>';
                articleData += '<td>' + section.Fuente + '</td>';

                articleData += '<td><a href="javaScript:void(0);" class="table_link" onclick="SendProducto(' + section.ProductoId + ');" alt="Click aqui para ver la ficha de este producto" title="Click aqui para ver la ficha de este producto">' + section.Producto + '</a></td>';

                articleData += '<td>' + section.Provincia + '</td>';
                articleData += '<td>' + section.Localidad + '</td>';
                
                articleData += '<td>' + section.Rinde.toFixed(0) + '</td>';
                articleData += '<td>' + (section.Ranking / section.Total).toFixed(2) + '</td>';

                articleData += '</tr>';

                rowid = rowid + 1;
            });

            $("#h_select_agroup_aux").val(selectAgroupAux);
            $("#ensayoList").append(articleData);

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


