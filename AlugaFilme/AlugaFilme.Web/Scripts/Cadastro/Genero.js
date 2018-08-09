function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#cbx_ativo').prop('checked', dados.Ativo);    
}


function set_focus_form() {
    $('#txt_nome').focus();
}

function set_dados_grid(dados) {
    return '<td style="text-align:center;vertical-align:middle">'+
        '<input type="checkbox" data-id="' + dados.Id +'"></td>'+
        '<td>' + dados.Nome + '</td>' +
        '<td>' + (dados.Ativo ? 'SIM' : 'NÃO') + '</td>';
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',        
        Ativo: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function preencher_linha_grid(linha, param) {
    linha
        .eq(1).html(param.Nome).end()
        .eq(2).html(param.Ativo ? 'SIM' : 'NÃO');
}

function get_lista_marcados() {
    var ids = [],
        lista_generos = $('#grid_cadastro > tbody');
    lista_generos.find('input[type=checkbox]').each(function (index, input) {
        var cbx = $(input),
            marcado = cbx.is(':checked');
        if (marcado) {
            ids.push(parseInt(cbx.attr('data-id')));
        }
    });

    return ids;
}

function remover_trs() {
    var lista_generos = $('#grid_cadastro > tbody');
    lista_generos.find('input[type=checkbox]').each(function (index, input) {
        var cbx = $(input),
            tr = cbx.closest('tr'),
            marcado = cbx.is(':checked');
        if (marcado) {
            tr.remove();
        }
    });

    return ids;
}