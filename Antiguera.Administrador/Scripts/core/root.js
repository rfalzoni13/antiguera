//Inicializar classes
var antiguera = antiguera || {};
antiguera.usuario = antiguera.usuario || {};

antiguera.core = antiguera.core || {
    configuracoes: {
        modais: {
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
            },

            fecharModal: function () {
                $(".alert").fadeOut();
            }
        },
    }
};

$(document).ready()