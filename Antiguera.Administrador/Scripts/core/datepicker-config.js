antiguera.core.datepicker = antiguera.core.datepicker || {
    registrarConfiguracoes: function () {
        $(".input-group.date").datepicker({
            format: "dd/mm/yyyy",
            language: "pt-BR",
            autoclose: true,
        });
    }
    
}