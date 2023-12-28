antiguera.jogo.index = antiguera.jogo.index || {
    carregarComponentes: function () {
        moment.locale("pt-br");
        $("#TabelaJogo").DataTable({
            processing: true,
            serverSide: true,
            autoWidth: false,
            createdRow: function (row, data, dataIndex) {
                $(row).attr('data-id', data.Id);
            },
            fnDrawCallback: function (settings) {
                $('[data-toggle="tooltip"]').tooltip();
            },
            columnDefs: [
                { orderable: false, targets: [7, 8] },
            ],
            ajax: {
                url: "Jogo/CarregarJogos",
                type: "POST",
                error: function (jqXHR, textStatus, errorThrow) {
                    console.log(jqXHR);
                }
            },
            columns: [
                { "data": "Nome" },
                { "data": "Developer" },
                { "data": "Publisher" },
                { "data": "Genero" },
                { "data": "Plataforma" },
                {
                    "data": "Created", "render": function (value) {
                        if (value === null) return "";
                        return moment(value).format('LLL');
                    }
                },
                {
                    "data": "Modified", "render": function (value) {
                        if (value === null) return "Nunca modificado";
                        return moment(value).format('LLL');
                    }
                },
                {
                    "data": null, "render": function (data, type) {
                        if (type === 'display') {
                            data = '<a href="Jogo/Editar?id=' + data.Id + '" class="btn btn-primary" data-toggle="tooltip" data-placement="top" title="Editar"><i class="fa fa-pencil-square-o"></a>';
                        }
                        return data;
                    }
                },
                {
                    "data": null, "render": function () {
                        return '<button type="button" class="btn btn-danger deletar-jogo" data-toggle="tooltip" data-placement="top" title="Deletar"><i class="fa fa-trash-o"></button>';
                    }
                }

            ]
        });
    },

    deletarJogo: function (id) {

        $.ajax({
            url: antiguera.core.configuracoes.rotas.jogo.excluir,
            data: { id: id },
            dataType: 'json',
            type: 'POST',
            success: function (data) {
                if (data.success) {
                    antiguera.core.configuracoes.modais.configurarModalSucesso(data.message, antiguera.core.configuracoes.rotas.jogos.index);
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

    }
}

$(document).on("click", ".deletar-jogo", function () {
    var id = $(this).parent().parent().attr("data-id");
    antiguera.jogo.index.deletarJogo(id);
});


$(document).ready(antiguera.jogo.index.carregarComponentes());