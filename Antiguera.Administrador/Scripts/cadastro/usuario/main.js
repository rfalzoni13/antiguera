antiguera.usuario.index = antiguera.usuario.index || {
    carregarComponentes: function () {
        moment.locale("pt-br");

        //Definição de colunas
        let columns = [
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
                        data = '<a href="' + antiguera.core.rotas.cadastro.usuario.editar + '?id=' + data.Id + '" class="btn btn-primary" data-toggle="tooltip" data-placement="top" title="Editar"><i class="fa fa-pencil-square-o"></a>';
                    }
                    return data;
                }
            },
            {
                "data": null, "render": function () {
                    return '<button type="button" class="btn btn-danger deletar-usuario" data-toggle="tooltip" data-placement="top" title="Deletar"><i class="fa fa-trash-o"></button>';
                }
            }

        ];

        //Definição de coluna - Remover ordenação dos botões de exclusão e edição
        let columnDefs = [
            { orderable: false, targets: [6, 7] },
        ];

        antiguera.core.dataTableConfiguration.carregarTabela($("#TabelaUsuario"), false, antiguera.core.rotas.cadastro.usuario.carregarTabela, columnDefs, columns);

        $(".alert-close").click(antiguera.core.configuracoes.modais.fecharModal);
    },

    deletarUsuario: function (id) {
        antiguera.core.dataTableConfiguration.deletarRegistro(id, antiguera.core.rotas.cadastro.usuario.excluir, antiguera.core.rotas.cadastro.usuario.index);
    }
}

$(document).on("click", ".deletar-usuario", function () {
    var id = $(this).parent().parent().attr("data-id");
    antiguera.usuario.index.deletarUsuario(id);
});

$(document).ready(antiguera.usuario.index.carregarComponentes());