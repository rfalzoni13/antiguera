antiguera.usuario.cadastrar = antiguera.usuario.cadastrar || {
    registrarComponentes: function () {
        $("#BtnFoto").click(antiguera.usuario.cadastrar.clicarBotaoFoto);
        $("#ArquivoPerfil").change(antiguera.usuario.cadastrar.mudarFoto);
        $("#CortarImagem").click(antiguera.usuario.cadastrar.cortarImagem);
        //$("#Salvar").click(antiguera.usuario.cadastrar.enviarFormulario);
        $(".cancelar-corte").click(antiguera.usuario.cadastrar.cancelarCorte);
        $(".modal-foto").on("show.bs.modal", function () {
            setTimeout(function () {
                antiguera.usuario.cadastrar.ajustarImagem();
            }, 500)
        });
        $(".modal-foto").on("hidden.bs.modal", function () {
            $("#ImagePreview").removeAttr("src").removeAttr("style").cropper("destroy");
        });

        $(".modal-danger").on("hidden.bs.modal", function () {
            $(this).find("ul").empty();
        });

        $(".alert-close").click(antiguera.core.configuracoes.modais.fecharModal);

        $("#Acessos").select2();

        antiguera.core.datepicker.registrarConfiguracoes();
    },

    clicarBotaoFoto: function () {
        antiguera.core.imageUtility.clicarBotaoFoto($("#ArquivoPerfil"), $("#ImgFoto"), 'usuario');
    },

    cancelarCorte: function () {
        antiguera.core.imageUtility.cancelarCorte($("#ArquivoPerfil"), $("#ImgFoto"), 'usuario');
    },

    mudarFoto: function (e) {
        antiguera.core.imageUtility.mudarFoto(e, this, $("#ImagePreview"), $(".modal-foto"));
    },

    ajustarImagem: function () {
        antiguera.core.imageUtility.ajustarImagem($("#ImagePreview"));
    },

    cortarImagem: function () {
        let formData = new FormData();
        antiguera.core.imageUtility.cortarImagem($("#ImagePreview"), $("#ImgFoto"), formData, 'ArquivoPerfil');
    },

    //enviarFormulario: function () {
    //    $(this).attr("disabled", "disabled").html("<i class='fa fa-spinner fa-pulse' ></i> Processando...");
    //    formData.set("Nome", $("#Nome").val());
    //    formData.set("AcessoId", $("#AcessoId").val());
    //    formData.set("Sexo", $("#Sexo").val());
    //    formData.set("Login", $("#Login").val());
    //    formData.set("Email", $("#Email").val());
    //    formData.set("Senha", $("#Senha").val());
    //    formData.set("ConfirmarSenha", $("#ConfirmarSenha").val());

    //    if ($("#Id").val() !== undefined) {
    //        formData.set("Id", $("#Id").val())
    //        url = antiguera.core.configuracoes.rotas.usuario.editar;
    //    } else {
    //        url = antiguera.core.configuracoes.rotas.usuario.cadastrar;
    //    }

    //    $.ajax(url, {
    //        method: 'POST',
    //        data: formData,
    //        processData: false,
    //        contentType: false,
    //        success: function (data) {
    //            if (data.success) {
    //                antiguera.core.configuracoes.modais.configurarModalSucesso(data.message, antiguera.core.configuracoes.rotas.usuario.index);
    //            } else {
    //                console.log(data.errors);
    //                antiguera.core.configuracoes.modais.configurarModalErro(data.errors);
    //            }
    //        },
    //        error: function (xHR, status, error) {
    //            console.log(error);
    //            antiguera.core.configuracoes.modais.configurarModalAjaxErro;
    //        }
    //    });

    //    if (url === antiguera.core.configuracoes.rotas.usuario.editar) {
    //        $(this).removeAttr("disabled").html("Salvar");
    //    } else {
    //        $(this).removeAttr("disabled").html("Cadastrar");
    //    }
    //}
};

$(document).ready(antiguera.usuario.cadastrar.registrarComponentes());

