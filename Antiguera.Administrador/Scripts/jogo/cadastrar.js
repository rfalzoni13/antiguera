antiguera.jogo.cadastrar = {
    registrarComponentes: function () {
        $(".input-group.date").datepicker({
            format: "dd/mm/yyyy",
            language: "pt-BR"
        });
        $("#BtnArquivoJogo, #BtnArquivoCapa").click(antiguera.jogo.cadastrar.dispararBotaoArquivo);
        $("#ArquivoJogo, #ArquivoCapa").change(antiguera.jogo.cadastrar.incluirNomeArquivo);
        //$(".cancelar-upload").click(antiguera.jogo.cadastrar.cancelarUpload);
        //$("#Selecionar").click(antiguera.jogo.cadastrar.selecionarBoxArt);
    },

    dispararBotaoArquivo: function () {
        if (this.id == "BtnArquivoJogo") {
            $("#ArquivoJogo").click();
        } else {
            $("#ArquivoCapa").click();
        }
    },

    incluirNomeArquivo: function () {
        if (this.id == "ArquivoJogo") {
            $("#TextArquivo").html($(this)[0].files[0].name);
        } else {
            $("#TextBoxArt").html($(this)[0].files[0].name);
        }
    }

    //mudarBoxArt: function () {
    //    antiguera.jogo.cadastrar.carregarBoxArt(this, $(".modal-boxart .img-responsive"));
    //    $(".modal-boxart").modal({
    //        backdrop: 'static',
    //        keyboard: false
    //    });
    //},

    //carregarBoxArt: function (input, output) {
    //    if (input.files && input.files[0]) {
    //        var reader = new FileReader();

    //        reader.onload = function (e) {
    //            output.attr('src', e.target.result);
    //        };

    //        reader.readAsDataURL(input.files[0]);
    //    }
    //},

    //cancelarUpload: function () {
    //    $(".modal-boxart").modal("hide");
    //    $("#FileBoxArt").val("");
    //    setTimeout(function () {
    //        $(".modal-boxart .img-responsive").removeAttr("src");
    //    }, 500)
    //},

    //selecionarBoxArt: function () {
    //    $(".modal-boxart").modal("hide");
    //},
};

$(document).ready(antiguera.jogo.cadastrar.registrarComponentes());

