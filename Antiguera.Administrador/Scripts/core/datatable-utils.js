antiguera.core.dataTableConfiguration = antiguera.core.dataTableConfiguration || {
    carregarTabela: function (element, isServerSide, url, columnDefs, columns) {
        element.DataTable({
            processing: true,
            serverSide: isServerSide,
            autoWidth: false,
            order: [[0, 'asc']],
            createdRow: function (row, data, dataIndex) {
                $(row).attr('data-id', data.Id);
            },
            fnDrawCallback: function (settings) {
                $('[data-toggle="tooltip"]').tooltip();
            },
            columnDefs: columnDefs,
            ajax: {
                url: url,
                type: 'GET',
                error: function (jqXHR, textStatus, errorThrow) {
                    console.error(jqXHR);
                }
            },
            columns: columns
        });
    },
    deletarRegistro: function (id, url, urlCallBack) {

        $.ajax({
            url: url,
            data: { id: id },
            dataType: 'json',
            type: 'POST',
            success: function (data) {
                if (data.success) {
                    antiguera.core.configuracoes.modais.configurarModalSucesso(data.message, urlCallBack);
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

//Tradução Datatable
$.extend(true, $.fn.dataTable.defaults, {
    language: {
        "sEmptyTable": "Nenhum registro encontrado",
        "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
        "sInfoFiltered": "(Filtrados de _MAX_ registros)",
        "sInfoPostFix": "",
        "sInfoThousands": ".",
        "sLengthMenu": "_MENU_ resultados por página",
        "sLoadingRecords": "Carregando...",
        "sProcessing": "Processando...",
        "sZeroRecords": "Nenhum registro encontrado",
        "sSearch": "Pesquisar",
        "oPaginate": {
            "sNext": "Próximo",
            "sPrevious": "Anterior",
            "sFirst": "Primeiro",
            "sLast": "Último"
        },
        "oAria": {
            "sSortAscending": ": Ordenar colunas de forma ascendente",
            "sSortDescending": ": Ordenar colunas de forma descendente"
        },
        "select": {
            "rows": {
                "_": "Selecionado %d linhas",
                "0": "Nenhuma linha selecionada",
                "1": "Selecionado 1 linha"
            }
        },
        "buttons": {
            "copy": "Copiar para a área de transferência",
            "copyTitle": "Cópia bem sucedida",
            "copySuccess": {
                "1": "Uma linha copiada com sucesso",
                "_": "%d linhas copiadas com sucesso"
            }
        }
    }
});