//Inicializar classes
var antiguera = antiguera || {};
antiguera.core = antiguera.core || {};
antiguera.core.configuracoes = antiguera.core.configuracoes || {};
antiguera.acesso = antiguera.acesso || {};
antiguera.emulador = antiguera.emulador || {};
antiguera.jogo = antiguera.jogo || {};
antiguera.programa = antiguera.programa || {};
antiguera.rom = antiguera.rom || {};
antiguera.usuario = antiguera.usuario || {};

//Demais configurações
antiguera.core.configuracoes.modais = antiguera.core.configuracoes.modais || {
    configurarModalSucesso: function (mensagem, url) {
        $(".modal-success").modal({
            backdrop: 'static',
            keyboard: false
        });
        $(".modal-success").find("h1").html(mensagem);
        $(".modal-success").find("a").attr("href", url);
    },
    configurarModalErro: function (errors) {
        $(".modal-danger").modal({
            backdrop: 'static',
            keyboard: false
        });

        for (var i = 0; i < errors.length; i++) {
            $(".modal-danger").find("ul").append("<li><h3>" + errors[i] + "</h3></li>");
        }
    },
    configurarModalAjaxErro: function () {
        $(".modal-danger").modal({
            backdrop: 'static',
            keyboard: false
        });

        $(".modal-danger").find("h3").html("Ocorreu um erro ao tentar processar sua solicitação!");
    }
};

$(document).ready()