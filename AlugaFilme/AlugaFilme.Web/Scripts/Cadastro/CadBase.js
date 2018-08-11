var salvar_customizado = null;

function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}

function formatar_mensagem_aviso(mensagens) {
    var ret = '';
    for (var i = 0; i < mensagens.length; i++) {
        ret += '<li>' + mensagens[i] + '</li>';
    }
    return '<ul>' + ret + '</ul>'
}

function salvar_ok(response, param) {
    if (response.Resultado == "OK") {
        if (param.Id == 0) {
            param.Id = response.IdSalvo;

            var table = $('#grid_cadastro').find('tbody'),
                linha = criar_linha_grid(param);
            table.append(linha);

            $('#grid_cadastro').removeClass('invisivel');
            $('#mensagem_grid').addClass('invisivel');

        }
        else {
            var linha = $('#grid_cadastro').find('tr[data-id=' + param.Id + ']').find('td');
            preencher_linha_grid(linha, param);
        }

        $('#modal_cadastro').parents('.bootbox').modal('hide');
    }
    else if (response.Resultado == "ERRO") {
        $('#msg-erro').show();
        $('#msg-aviso').hide();
        $('#msg-mensagem-aviso').hide();
    }
    else if (response.Resultado == "AVISO") {
        $('#msg-mensagem-aviso').html(formatar_mensagem_aviso(response.Mensagens));
        $('#msg-erro').hide();
        $('#msg-aviso').show();
        $('#msg-mensagem-aviso').show();
    }
}

function salvar_erro() {
    swal('Aviso', 'Não foi possível salvar as informações. Tente novamente em instantes.', 'warning');
}

$(document)
    .on('click', '#btn_incluir', function () {
        abrir_form(get_dados_inclusao());
    })
    .on('click', '.btn-alterar', function () {
        var btn = $(this),
            id = btn.closest('tr').attr('data-id'),
            url = urlAlteracao,
            param = { 'id': id };
        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                abrir_form(response);
            }
        })
            .fail(function () {
                swal('Aviso', 'Não foi possível recuperar as informações. Tente novamente em instantes.', 'warning');
            });
    })
    .on('click', '.btn-excluir', function () {
        var btn = $(this),
            tr = btn.closest('tr'),
            id = tr.attr('data-id'),
            url = urlExclusao,
            param = { 'id': id };

        bootbox.confirm({
            message: "Deseja realmente excluir o " + tituloPagina + "? ",
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
    .on('click', '#btn_remover', function () {
        var url = urlExclusaoVarios,
            ids = get_lista_marcados(),
            param = { 'id': ids };

        if (ids.length > 0) {
            bootbox.confirm({
                message: "Deseja realmente excluir o " + tituloPagina + "? ",
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
                                remover_trs();
                            }
                        })
                            .fail(function () {
                                swal('Aviso', 'Não foi possível excluir o registro. Tente novamente em instantes.', 'warning');
                            });
                    }
                }
            });
        } else {
            swal('Aviso', 'Para excluir é necessário selecionar ao menos 1 registro.', 'warning');
        }
    })
    .on('click', '#btn_confirmar', function () {
        var btn = $(this),
            url = urlConfirmacao,
            param = get_dados_form();

        if (salvar_customizado && typeof (salvar_customizado) == 'function') {
            salvar_customizado(url, param, salvar_ok, salvar_erro);

        }
        else {

            $.post(url, add_anti_forgery_token(param), function (response) {
                salvar_ok(response, param);
            })
                .fail(function () {
                    salvar_erro();
                });
        }
    })
    .on('click', '.page-item', function () {
        var btn = $(this),
            filtro = $('#txt_filtro'),
            pagina = btn.text(),
            tamPag = $('#ddl_tam_pag').val(),
            url = urlPaginaNumero,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val(), 'ordenacao': "nome" };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');

                table.empty();
                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));
                }
                btn.siblings().removeClass('active');
                btn.addClass('active');
            }
        })
            .fail(function () {
                swal('Aviso', 'Não foi possível alterar a página. Tente novamente em instantes.', 'warning');
            });
    })
    .on('change', '#ddl_tam_pag', function () {
        var ddl = $(this),
            filtro = $('#txt_filtro'),
            pagina = 1,
            tamPag = ddl.val(),
            url = urlChangePage,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val(), 'ordenacao': "nome" };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');

                table.empty();
                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));
                }
            }
        })
            .fail(function () {
                swal('Aviso', 'Não foi possível exibir os registros. Tente novamente em instantes.', 'warning');
            });
    })
    .on('keyup', '#txt_filtro', function () {
        var filtro = $(this),
            ddl = $('#ddl_tam_pag'),
            pagina = 1,
            tamPag = ddl.val(),
            url = urlFiltro,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val(), 'ordenacao': "nome" };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');

                table.empty();
                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));
                }

            }
        })
            .fail(function () {
                swal('Aviso', 'Não foi possível filtrar as informações. Tente novamente em instantes.', 'warning');
            });
    })
    .on('click', '.coluna-ordenacao', function () {
        var coluna = $(this),
            btn_page_iten = $('.pagination').find('.active'),
            ordem_crescente = true,
            ordem = coluna.find('i'),
            tamPag = $('#ddl_tam_pag').val(),
            url = urlOrdenacao,
            pagina = btn_page_iten.text() > 0 ? btn_page_iten.text() : 0,
            filtro = $('#txt_filtro'),
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val(), 'ordenacao': "" };

        if (ordem.length > 0) {
            ordem_crescente = ordem.hasClass('glyphicon-arrow-down');
            if (ordem_crescente) {
                ordem.removeClass('glyphicon-arrow-down');
                ordem.addClass('glyphicon-arrow-up');
                param.ordenacao = coluna.attr('data-campo') + ' desc';
            }
            else {
                ordem.addClass('glyphicon-arrow-down');
                ordem.removeClass('glyphicon-arrow-up');
                param.ordenacao = coluna.attr('data-campo') + ' asc';
            }
        }
        else {
            $('.coluna-ordenacao i').remove();
            coluna.append('&nbsp;<i class="glyphicon glyphicon-arrow-down" style="color:#000000"></i>');
            param.ordenacao = coluna.attr('data-campo') + ' asc';
        }

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');

                table.empty();
                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));
                }
            }
        });

    })
    .on('click', '.btn-pesq', function () {
        var tipo_filtro = $('#ddl_tipo').val(),
            filtro = $('#txt_filtro_cli').val(),
            param = { 'tipo': tipo_filtro, 'filtro': filtro },
            url = urlRecuperarCliente;
        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                preenche_dados_cli(response[0]);
            }
        })

    })
    .on('click', '.btn-add-filme', function () {
        var btn = $(this),
            filme = $('#lista_filmes'),
            tbody_filmes = $('#grid_filmes > tbody');

        filme.find('input[type=checkbox]').each(function (index, input) {
            var cbx = $(input),
                marcado = cbx.is(':checked');
            if (marcado) {
                tbody_filmes.append("<tr><td>" + cbx.attr('data-id-filme') + "</td><td>" +
                    cbx.attr('data-nome-filme') + "</td>" +
                    "<td><a class=\"btn btn-default btn-remove-filme\" role=\"button\"><i class=\"glyphicon glyphicon-remove\"></i></a></td>" +
                    "</tr > ");
            }
        });


    })
    .on('click', '.btn-remove-filme', function () {
        var tr = $(this).closest('tr');
        tr.remove();
    });

function abrir_form(dados) {
    set_dados_form(dados);
    var modal_cadastro = $('#modal_cadastro');

    $('#msg-erro').hide();
    $('#msg-aviso').hide();
    $('#msg-mensagem-aviso').empty();
    $('#msg-mensagem-aviso').hide();

    bootbox.dialog({
        title: "Cadastro de " + tituloPagina,
        message: modal_cadastro,
        className: 'dialogo',
    })
        .on('shown.bs.modal', function () {
            modal_cadastro.show(0, function () {
                set_focus_form();
            });
        })
        .on('hidden.bs.modal', function () {
            modal_cadastro.hide().appendTo('body');
        });
}

function criar_linha_grid(dados) {
    if (dados.Cli_Nome != null) {
        var ret = '<tr data-id=' + dados.Id + '>' +
            set_dados_grid(dados) +
            '<td>' +
            '<a class="btn btn-info btn-devolver" role="button"><i class="glyphicon glyphicon-file"></i> Devolver</a>' +
            '</td>' +
            '</tr>';
    } else {
        var ret =
            '<tr data-id=' + dados.Id + '>' +
            set_dados_grid(dados) +
            '<td>' +
            '<a class="btn btn-primary btn-alterar" role="button" style="margin-right: 3px"><i class="glyphicon glyphicon-pencil"></i> Alterar</a>' +
            '<a class="btn btn-danger btn-excluir" role="button"><i class="glyphicon glyphicon-trash"></i> Excluir</a>' +
            '</td>' +
            '</tr>';
    }
    return ret;
}

function preenche_dados_cli(dados) {
    $('#id_cliente').val(dados.Id);
    $('#txt_cli_nome').val(dados.Nome);
    $('#txt_cpf').val(dados.NumDocumento);
    $('#txt_logradouro').val(dados.Logradouro);
    $('#txt_numero').val(dados.Numero);
}

function get_lista_marcados() {
    var ids = [],
        lista = $('#grid_cadastro');
    lista.find('input[type=checkbox]').each(function (index, input) {
        var cbx = $(input),
            marcado = cbx.is(':checked');
        if (marcado) {
            ids.push(parseInt(cbx.attr('data-id')));
        }
    });

    return ids;
}
