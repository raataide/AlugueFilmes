function set_dados_form(dados) {
    $('#id_cliente').val(dados.Id_Cliente);
    /*$('#txt_nome').val(dados.Nome);
    $('#txt_login').val(dados.Login);
    $('#txt_senha').val(dados.Senha);
    $('#txt_email').val(dados.Email);*/
    $('#txt_cli_nome').prop('readonly', true);
    $('#txt_cpf').prop('readonly', true);
    $('#txt_logradouro').prop('readonly', true);
    $('#txt_numero').prop('readonly', true);
  
}

function set_focus_form() {
    $('#txt_filtro_cli').focus();
}

function set_dados_grid(dados) {
    return '<td style="text-align:center;vertical-align:middle">' +
        '<input type="checkbox" data-id="' + dados.Id + '"></td>' +
        '<td>' + dados.Nome + '</td>' +
        '<td>' + dados.Login + '</td>';
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        Login: '',
        Senha: '',
        Email: ''
    };
}

function get_dados_form() {
    var Filmes_Id = [],
        lista_filmes = $('#grid_filmes');

    lista_filmes.find(' tr:nth-child(n) td:nth-child(1)').each(function (index, td) {
        var td = $(td).text();
        Filmes_Id.push(td);
    });

    return {
        ClienteId : $('#id_cliente').val(),
        FilmesId: Filmes_Id,
        Nome: $('#txt_nome').val(),
        Login: $('#txt_login').val(),
        Senha: $('#txt_senha').val(),
        Email: $('#txt_email').val()
    };
}

function preencher_linha_grid(linha, param) {
    linha
        .eq(1).html(param.Nome).end()
        .eq(2).html(param.Login).end();
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
            var quant = $('#grid_cadastro > tbody > tr').length;
            if (quant == 0) {
                $('#grid_cadastro').addClass('invisivel');
                $('#mensagem_grid').removeClass('invisivel');
            }
        }
    });

    return ids;
}