antiguera.usuario.index = antiguera.usuario.index || {
    carregarComponentes: function () {
        moment.locale("pt-br");
        $("#TabelaUsuario").DataTable({
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
                { orderable: false, targets: [6, 7] },
            ],
            ajax: {
                url: antiguera.core.rotas.usuario.carregarTabela,
                type: "POST",
                error: function (jqXHR, textStatus, errorThrow) {
                    console.log(jqXHR);
                }
            },
            columns: [
                { "data": "Nome" },
                { "data": "Email" },
                { "data": "Login" },
                { "data": "Sexo" },
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
                            data = '<a href="' + antiguera.core.rotas.usuario.editar + '?id=' + data.Id + '" class="btn btn-primary" data-toggle="tooltip" data-placement="top" title="Editar"><i class="fa fa-pencil-square-o"></a>';
                        }
                        return data;
                    }
                },
                {
                    "data": null, "render": function () {
                        return '<button type="button" class="btn btn-danger deletar-usuario" data-toggle="tooltip" data-placement="top" title="Deletar"><i class="fa fa-trash-o"></button>';
                    }
                }
                
            ]            
        });
    },

    deletarUsuario: function (id) {

        $.ajax({
            url: antiguera.core.rotas.usuario.excluir,
            data: { id: id },
            dataType: 'json',
            type: 'POST',
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
    }
}

$(document).on("click", ".deletar-usuario", function () {
    var id = $(this).parent().parent().attr("data-id");
    antiguera.usuario.index.deletarUsuario(id);
});

$(document).ready(antiguera.usuario.index.carregarComponentes());