var jcrop_api;

antiguera.programa.cadastrar = {
    registrarComponentes: function () {
        $(".input-group.date").datepicker({
            format: "dd/mm/yyyy",
            language: "pt-BR"
        });
        $("#FileBoxArt").change(antiguera.programa.cadastrar.mudarBoxArt);
        $(".modal").on("show.bs.modal", function () {
            setTimeout(function () {
                antiguera.programa.cadastrar.cortarImagem();
            }, 500)
        });
        $(".modal").on("hidden.bs.modal", function () {
            $("#ImagePreview").removeAttr("src").removeAttr("style");
            $("#FileBoxArt").val("");
            jcrop_api.destroy();
        });
    },

    mudarBoxArt: function () {
        antiguera.programa.cadastrar.carregarBoxArt(this, $("#ImagePreview"));
        $(".modal").modal("show");
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


    cortarImagem: function () {
    },
};

$(document).ready(antiguera.programa.cadastrar.registrarComponentes());

