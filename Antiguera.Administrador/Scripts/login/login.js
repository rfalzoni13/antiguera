antiguera.login = {
    registrarComponentes: function () {
        $(".alert-close").click(antiguera.login.fecharModal);
        $(".form-signin").submit(antiguera.login.personalizarBotao);
    },

    personalizarBotao: function () {
        $(".btn-primary").attr("disabled", "disabled").html("<i class='fa fa-spinner fa-pulse' ></i> Entrando...");
        return true;
    },

    fecharModal: function () {
        $(".alert").fadeOut();
    }
};

$(document).ready(antiguera.login.registrarComponentes());