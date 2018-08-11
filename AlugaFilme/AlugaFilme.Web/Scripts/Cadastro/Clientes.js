function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_documento').val(dados.NumDocumento);
    $('#txt_telefone').val(dados.Telefone);
    $('#txt_logradouro').val(dados.Logradouro);
    $('#txt_complemento').val(dados.Complemento);
    $('#txt_cep').val(dados.Cep);
    $('#cbx_ativo').prop('checked', dados.Ativo);
    $('#txt_cidade').val(dados.Cidade);
    $('#txt_estado').val(dados.Estado);
    $('#txt_pais').val(dados.Pais);
    $('#txt_numero').val(dados.Numero);
}

function set_focus_form() {
    $('#txt_nome').focus();
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        NumDocumento: '',
        Numero: '',
        Telefone: '',
        Logradouro: '',
        Complemento: '',
        Cep: '',
        Pais: '',
        Estado: '',
        Cidade: '',
        Ativo: true
    };
}

function set_dados_grid(dados) {
    return '<td style="text-align:center;vertical-align:middle">' +
        '<input type="checkbox" data-id="' + dados.Id + '"></td>' +
        '<td>' + dados.Nome + '</td>' +
        '<td>' + dados.Telefone + '</td>' +
        '<td>' + (dados.Ativo ? 'SIM' : 'NÃO') + '</td>';
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        NumDocumento: $('#txt_documento').val(),
        Numero: $('#txt_numero').val(),
        Telefone: $('#txt_telefone').val(),
        Logradouro: $('#txt_logradouro').val(),
        Complemento: $('#txt_complemento').val(),
        Cep: $('#txt_cep').val(),
        Pais: $('#txt_pais').val(),
        Estado: $('#txt_estado').val(),
        Cidade: $('#txt_cidade').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function preencher_linha_grid(linha, param) {
    linha
        .eq(1).html(param.Nome).end()
        .eq(2).html(param.Telefone).end()
        .eq(3).html(param.Ativo ? 'SIM' : 'NÃO');
}

$(document)
    .ready(function () {
        $('#txt_telefone').mask('(00) 00000-0000');
        $('#txt_cep').mask('00000-000');
        $('#txt_documento').mask('000.000.000-00')
    });


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
            var quant = $('#grid_cadastro > tbody > tr').length;
            if (quant == 0) {
                $('#grid_cadastro').addClass('invisivel');
                $('#mensagem_grid').removeClass('invisivel');
            }
        }
    });

    return ids;
}