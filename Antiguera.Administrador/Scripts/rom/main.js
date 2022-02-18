antiguera.rom.index = antiguera.rom.index || {
    carregarComponentes: function () {
        moment.locale("pt-br");
        $("#TabelaRom").DataTable({
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
                url: antiguera.core.configuracoes.rotas.rom.carregarRoms,
                type: "POST",
                error: function (jqXHR, textStatus, errorThrow) {
                    console.log(jqXHR);
                }
            },
            columns: [
                { "data": "Nome" },
                { "data": "Genero" },
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
                            data = '<a href="' + antiguera.core.configuracoes.rotas.rom.editar + '?id=' + data.Id + '" class="btn btn-primary" data-toggle="tooltip" data-placement="top" title="Editar"><i class="fa fa-pencil-square-o"></a>';
                        }
                        return data;
                    }
                },
                {
                    "data": null, "render": function () {
                        return '<button type="button" class="btn btn-danger" data-toggle="tooltip" data-placement="top" title="Deletar"><i class="fa fa-trash-o"></button>';
                    }
                }

            ]
        });
    }
}

$(document).ready(antiguera.rom.index.carregarComponentes());