function abrir_form(dados) {
    set_dados_form(dados);
    var modal_cadastro = $('#modal_cadastro');

    $('#msg-erro').hide();
    $('#msg-aviso').hide();
    $('#msg-mensagem-aviso').empty();
    $('#msg-mensagem-aviso').hide();

    bootbox.dialog({
        title: "Cadastro de Usuários",
        message: modal_cadastro,
        className: 'dialogo',
    })
        .on('shown.bs.modal', function () {
            modal_cadastro.show(0, function () {
                $('#txt_nome').focus();
            });
        })
        .on('hidden.bs.modal', function () {
            modal_cadastro.hide().appendTo('body');
        });
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
function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_login').val(dados.Login);
    $('#txt_senha').val(dados.Senha);
    $('#txt_email').val(dados.Email);
}
function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Login: $('#txt_login').val(),
        Senha: $('#txt_senha').val(),
        Email: $('#txt_email').val()
    };
}
function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}

$(document)
    .on('click', '#Incluir_User', function () {
        abrir_form(get_dados_inclusao());
    })
    .on('click', '#btn_salvar', function () {
        var btn = $(this),
            url = urlConfirmacao,
            param = get_dados_form();
        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response.Resultado == "OK") {

                swal('Confirmação', 'Usuário salvo com sucesso.', 'sucess');
                $('#modal_cadastro').parents('.bootbox').modal('hide');
            }
        })
            .fail(function () {
                swal('Aviso', 'Não foi possível salvar as informações.', 'warning');
            });

    });