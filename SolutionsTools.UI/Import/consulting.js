$(document).ready(function () {
    $('#btnConsultar').click(function (evt) {

        $(".clientes-table tbody tr input[type=checkbox]:checked").each(function () {
            var row = $(this).closest("tr").children('td');
            processRow(row);
        });

    });

    $('.chkSelecionarTodos').on('click', function () {
        if (this.checked) {
            $('.chkSelecionado').each(function () {
                this.checked = true;
            });
        } else {
            $('.chkSelecionado').each(function () {
                this.checked = false;
            });
        }
    });
});

function processRow(row) {

    var id = row.eq(0).text();

    if (id.length == 0)
        return;

    var formData = new FormData();

    formData.append("Id_Cliente", id);

    var options = {};
    options.url = "DataConsultingHandler.ashx";
    options.type = "POST";
    options.data = formData;
    options.contentType = false;
    options.processData = false;
    options.success = function (data, status) {
        if (status == "success") {
            row.eq(5).text(formatCPF(data));
        }
        else {
            row.eq(5).text(status);
        }
    };
    options.error = function (data) {
        row.eq(5).text("err" + data);
    };

    $('#divProcessando').show();
    $.ajax(options);
}