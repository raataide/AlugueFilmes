function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#id_cliente').val(dados.Id_Cliente);
    $('#txt_cli_nome').prop('readonly', true);
    $('#txt_cpf').prop('readonly', true);
    $('#txt_logradouro').prop('readonly', true);
    $('#txt_numero').prop('readonly', true);

}

function set_focus_form() {
    $('#txt_filtro_cli').focus();
}

function set_dados_grid(dados) {
    return '<td>' + dados.Cli_Nome + '</td>' +
        '<td>' + dados.Dt_Locacao + '</td>' +
        '<td>' + dados.Qtd_Filmes + '</td>' +
        '<td>' + dados.Devolvido + '</td>';
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
        Id: $('#id_cadastro').val(),
        ClienteId: $('#id_cliente').val(),
        FilmesId: Filmes_Id
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

$(document)
    .on('click', '.btn-devolver', function () {
        var btn = $(this),
            tr = btn.closest('tr'),
            id = tr.attr('data-id'),
            url = urlDevolucao,
            param = { 'id': id };
        bootbox.confirm({
            message: "Deseja realmente devolver os filmes?",
            buttons: {
                confirm: {
                    label: 'Sim',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Não',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post(url, add_anti_forgery_token(param), function (response) {
                        if (response) {
                            tr.remove();
                            var quant = $('#grid_cadastro > tbody > tr').length;
                            if (quant == 0) {
                                $('#grid_cadastro').addClass('invisivel');
                                $('#mensagem_grid').removeClass('invisivel');
                            }
                        }
                    })
                        .fail(function () {
                            swal('Aviso', 'Não foi possível excluir o registro. Tente novamente em instantes.', 'warning');
                        });
                }
            }
        });
    })
    .on('keyup', '#txt_nome_filme', function () {
        var filtro = $(this),
            url = urlFiltro_Filme,
            param = { 'filtro': filtro.val() };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#lista_filmes');

                table.empty();
                for (var i = 0; i < response.length; i++) {
                    table.append('<li class="list-group-item">' +
                        '<label style = "margin-bottom:0" >' +
                        '<input type="checkbox" data-id-filme=' + response[i].Id + ' data-nome-filme=' + response[i].Nome + '/>' + response[i].Nome +
                        '</label></li>');
                    
                }
            }
        })
            .fail(function () {
                swal('Aviso', 'Não foi possível filtrar as informações. Tente novamente em instantes.', 'warning');
            });
    });