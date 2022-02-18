var url;
var formData = new FormData();
var fileName;

antiguera.usuario.cadastrar = antiguera.usuario.cadastrar || {
    registrarComponentes: function () {
        $("#BtnFoto").click(antiguera.usuario.cadastrar.clicarBotaoFoto);
        $("#FileFoto").change(antiguera.usuario.cadastrar.mudarFoto);
        $("#CortarImagem").click(antiguera.usuario.cadastrar.cortarImagem);
        $("#Salvar").click(antiguera.usuario.cadastrar.enviarFormulario);
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
    },

    clicarBotaoFoto: function () {
        if ($("#FileFoto").val().length > 0) {
            antiguera.usuario.cadastrar.cancelarCorte();
        }

        $("#FileFoto").click();
    },

    cancelarCorte: function () {
        $("#FileFoto").val("");
        $("#ImgFoto").attr("src", "/Content/Images/Profile/user.png");
    },

    mudarFoto: function (e) {
        var target = e.target || e.srcElement;
        if (target.value.length === 0) {
            return null;
        }
        $(".modal-foto").modal({
            backdrop: 'static',
            keyboard: false
        });
        antiguera.usuario.cadastrar.carregarFoto(this, $("#ImagePreview"));
    },

    carregarFoto: function (input, output) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                output.attr('src', e.target.result);
            };

            reader.readAsDataURL(input.files[0]);
            fileName = input.files[0].name;
        }
    },

    ajustarImagem: function () {
        var minCroppedWidth = 320;
        var minCroppedHeight = 320;
        var maxCroppedWidth = 2048;
        var maxCroppedHeight = 2048;
        $("#ImagePreview").cropper({
            autoCropArea: 0.5,
            viewMode: 2,
            responsive: true,
            data: {
                width: (minCroppedWidth + maxCroppedWidth) / 2,
                height: (minCroppedHeight + maxCroppedHeight) / 2
            },
            crop: function (event) {

                var width = event.detail.width;
                var height = event.detail.height;

                if (
                    width < minCroppedWidth
                    || height < minCroppedHeight
                    || width > maxCroppedWidth
                    || height > maxCroppedHeight
                ) {
                    antiguera.usuario.cadastrar.definirImagemCrop(Math.max(minCroppedWidth, Math.min(maxCroppedWidth, width)), Math.max(minCroppedHeight, Math.min(maxCroppedHeight, height)));
                }

                console.log("Posição X é: " + event.detail.x);
                console.log("Posição Y é: " + event.detail.y);
                console.log("Largura é: " + event.detail.width);
                console.log("Altura é: " + event.detail.height);
                console.log("Rotação é: " + event.detail.rotate);
                console.log("Escala X é: " + event.detail.scaleX);
                console.log("Escala Y é: " + event.detail.scaleY);
            }
        });
    },

    definirImagemCrop: function (w, h) {
        var cropper = $("#ImagePreview").data('cropper');
        cropper.setData({
            width: w,
            height: h
        });
    },

    cortarImagem: function () {
        var cropper = $("#ImagePreview").data('cropper');
        var canvas = cropper.getCroppedCanvas();
        $("#ImgFoto").attr("src", canvas.toDataURL());
        antiguera.usuario.cadastrar.prepararImagem(cropper);
        $(".modal").modal("hide");
    },

    prepararImagem: function (cropper) {
        cropper.getCroppedCanvas().toBlob((blob) => {

            // Pass the image file name as the third parameter if necessary.
            formData.append('FileFoto', blob, fileName);

        });
    },/*, 'image/png' */

    enviarFormulario: function () {
        $(this).attr("disabled", "disabled").html("<i class='fa fa-spinner fa-pulse' ></i> Processando...");
        formData.set("Nome", $("#Nome").val());
        formData.set("AcessoId", $("#AcessoId").val());
        formData.set("Sexo", $("#Sexo").val());
        formData.set("Login", $("#Login").val());
        formData.set("Email", $("#Email").val());
        formData.set("Senha", $("#Senha").val());
        formData.set("ConfirmarSenha", $("#ConfirmarSenha").val());

        if ($("#Id").val() !== undefined) {
            formData.set("Id", $("#Id").val())
            url = antiguera.core.configuracoes.rotas.usuario.editar;
        } else {
            url = antiguera.core.configuracoes.rotas.usuario.cadastrar;
        }

        $.ajax(url, {
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.success) {
                    antiguera.core.configuracoes.modais.configurarModalSucesso(data.message, antiguera.core.configuracoes.rotas.usuario.index);
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

        if (url === antiguera.core.configuracoes.rotas.usuario.editar) {
            $(this).removeAttr("disabled").html("Salvar");
        } else {
            $(this).removeAttr("disabled").html("Cadastrar");
        }
    }
};

$(document).ready(antiguera.usuario.cadastrar.registrarComponentes());

