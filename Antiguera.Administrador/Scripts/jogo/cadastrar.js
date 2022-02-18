antiguera.jogo.cadastrar = {
    registrarComponentes: function () {
        $(".input-group.date").datepicker({
            format: "dd/mm/yyyy",
            language: "pt-BR"
        });
        $("#Salvar").click(antiguera.jogo.cadastrar.enviarFormulario);
        $("#FileBoxArt").change(antiguera.jogo.cadastrar.mudarBoxArt);
        $(".cancelar-upload").click(antiguera.jogo.cadastrar.cancelarUpload);
        $("#Selecionar").click(antiguera.jogo.cadastrar.selecionarBoxArt);
    },

    mudarBoxArt: function () {
        antiguera.jogo.cadastrar.carregarBoxArt(this, $(".modal-boxart .img-responsive"));
        $(".modal-boxart").modal({
            backdrop: 'static',
            keyboard: false
        });
    },

    carregarBoxArt: function (input, output) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                output.attr('src', e.target.result);
            };

            reader.readAsDataURL(input.files[0]);
        }
    },

    cancelarUpload: function () {
        $(".modal-boxart").modal("hide");
        $("#FileBoxArt").val("");
        setTimeout(function () {
            $(".modal-boxart .img-responsive").removeAttr("src");
        }, 500)
    },

    selecionarBoxArt: function () {
        $(".modal-boxart").modal("hide");
    },

    enviarFormulario: function () {
        var formData = new FormData();

        $(this).attr("disabled", "disabled").html("<i class='fa fa-spinner fa-pulse' ></i> Processando...");
        formData.set("Nome", $("#Nome").val());
        formData.set("Developer", $("#Developer").val());
        formData.set("Lancamento", $("#Lancamento").val());
        formData.set("Genero", $("#Genero").val());
        formData.set("Publisher", $("#Publisher").val());
        formData.set("Plataforma", $("#Plataforma").val());
        formData.set("FileBoxArt", $("#FileBoxArt")[0].files[0]);
        formData.set("FileJogo", $("#FileJogo")[0].files[0]);
        formData.set("Descricao", $("#Descricao").val());

        if ($("#Id").val() !== undefined) {
            formData.set("Id", $("#Id").val())
            url = antiguera.core.configuracoes.rotas.jogo.editar;
        } else {
            url = antiguera.core.configuracoes.rotas.jogo.cadastrar;
        }

        $.ajax(url, {
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.success) {
                    antiguera.core.configuracoes.modais.configurarModalSucesso(data.message, antiguera.core.configuracoes.rotas.jogo.index);
                } else {
                    console.log(data.errors);
                    antiguera.core.configuracoes.modais.configurarModalErro(data.errors);
                }
            },
            error: function (xHR, status, error) {
                console.log(error);
                antiguera.core.configuracoes.modais.configurarModalAjaxErro;
            }
        });

        if (url === antiguera.core.configuracoes.rotas.jogo.editar) {
            $(this).removeAttr("disabled").html("Salvar");
        } else {
            $(this).removeAttr("disabled").html("Cadastrar");
        }
    }
};

$(document).ready(antiguera.jogo.cadastrar.registrarComponentes());

