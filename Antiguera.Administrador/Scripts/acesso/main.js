var url;

antiguera.acesso.index = antiguera.acesso.index || {
    carregarComponentes: function () {
        $("#Cadastrar").click(antiguera.acesso.index.cadastrarAcesso);
        $("#Salvar").click(antiguera.acesso.index.salvarAcesso);
        moment.locale("pt-br");
        $("#TabelaAcesso").DataTable({
            processing: true,
            serverSide: true,
            autoWidth: false,
            ajax: {
                url: antiguera.core.configuracoes.rotas.acesso.carregarAcessos,
                type: "POST",
                error: function (jqXHR, textStatus, errorThrow) {
                    console.log(jqXHR);
                }
            },
            createdRow: function (row, data, dataIndex) {
                $(row).attr('data-id', data.Id);
            },
            fnDrawCallback: function (settings) {
                $('[data-toggle="tooltip"]').tooltip();
            },
            columnDefs: [
                { orderable: false, targets: [3, 4] },
            ],
            ajax: {
                url: antiguera.core.configuracoes.rotas.acesso.carregarAcessos,
                type: "POST",
                error: function (jqXHR, textStatus, errorThrow) {
                    console.log(jqXHR);
                }
            },
            columns: [
                { "data": "Nome" },
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
                    "data": null, "render": function () {
                       return '<button type="button" class="btn btn-primary editar-acesso" data-toggle="tooltip" data-placement="top" title="Editar"><i class="fa fa-pencil-square-o"></button>';
                    }
                },
                {
                    "data": null, "render": function () {
                        return '<button type="button" class="btn btn-danger deletar-acesso" data-toggle="tooltip" data-placement="top" title="Deletar"><i class="fa fa-trash-o"></button>';
                    }
                }

            ]
        });
    },

    cadastrarAcesso: function () {
        if ($(".modal-cadastro").find(".row-id").find($("#Id")).length) {
            $(".modal-cadastro").find(".row-id").find($("#Id")).remove();
        }

        $(".modal-cadastro").modal({
            backdrop: 'static',
            keyboard: false
        });
    },

    editarAcesso: function (id, name) {
        if (!$(".modal-cadastro").find(".row-id").find($("#Id")).length) {
            $(".modal-cadastro").find(".row-id").append("<input type='hidden' id='Id' name='Id' value='" + id + "' />");
        } else {
            $(".modal-cadastro").find(".modal-body").find($("#Id").val(id));
        }
        $(".modal-cadastro").find("#Nome").val(name);

        $(".modal-cadastro").modal({
            backdrop: 'static',
            keyboard: false
        });
    },

    salvarAcesso: function () {

        var data = {
            Nome: $("#Nome").val()
        };

        if ($(".modal-cadastro").find(".row-id").find($("#Id")).length) {
            url = antiguera.core.configuracoes.rotas.acesso.editar;
            data.Id = $("#Id").val();
        } else {
            url = antiguera.core.configuracoes.rotas.acesso.cadastrar;
        }

        $.ajax({
            dataType: 'json',
            type: 'POST',
            data: data,
            url: url,
            success: function (data) {
                if (data.success) {
                    antiguera.core.configuracoes.modais.configurarModalSucesso(data.message, antiguera.core.configuracoes.rotas.acesso.index);
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

        $(".modal-cadastro").modal("hide");
    },

    deletarAcesso: function (id) {

        $.ajax({
            url: antiguera.core.configuracoes.rotas.acesso.excluir,
            data: { id: id },
            dataType: 'json',
            type: 'POST',
            success: function (data) {
                if (data.success) {
                    antiguera.core.configuracoes.modais.configurarModalSucesso(data.message, antiguera.core.configuracoes.rotas.acesso.index);
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

$(document).on("click", ".deletar-acesso", function () {
    var id = $(this).parent().parent().attr("data-id");
    antiguera.acesso.index.deletarAcesso(id);
});

$(document).on("click", ".editar-acesso", function () {
    var name = $(this).parent().parent().find("td:eq(0)").html()
    var id = $(this).parent().parent().attr("data-id");
    antiguera.acesso.index.editarAcesso(id, name);
});

$(document).ready(antiguera.acesso.index.carregarComponentes());