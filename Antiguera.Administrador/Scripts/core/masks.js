//Aplicação de máscaras
antiguera.mascaras = {
    registrarMascaras: function () {
        $(".telefone").mask("(00) 0000-0000")
    }
}

$(document).ready(antiguera.mascaras.registrarMascaras());