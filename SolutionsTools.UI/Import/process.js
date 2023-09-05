$(document).ready(function () {
    $('#btnProcessar').click(function (evt) {
        var file = $("#hdnFile").val();

        if (file.length == 0) {
            ShowMessage('Partner Solution', 'Favor selecionar um arquivo para o processamento!', 0);
            return;
        }

        doIncrementProgressBar(0);

        //processFile();

        var Dados = {};
        Dados.arquivo = $("#hdnFile").val();
        Dados.empresa = $(".mselEmpresas option:selected").text();
        Dados.empresaId = $(".mselEmpresas").val();
        Dados.idusuario = $("#hdnIdUsuario").val();

        $.ajax({
            url: "ImportarNew.aspx/ProcessData",
            data: JSON.stringify(Dados),
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (retorno) {
                showLogReport(retorno.d);
            },
            error: function (req, status, error) {
                alert(req.status + '|' + error);
            }
        });

        return false;
    });
});

function processFile() {
    var file = $("#hdnFile").val();

    var empresa = $(".mselEmpresas option:selected").text();

    var empresaId = $(".mselEmpresas").val();

    var idUsuario = $("#hdnIdUsuario").val();

    var formData = new FormData();

    formData.append("File", file);
    formData.append("Empresa", empresa);
    formData.append("EmpresaId", empresaId);
    formData.append("IdUsuario", idUsuario);

    $.ajax({
        type: 'POST',
        url: "PartnerService.asmx/ProcessData",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            var file = $(data).find("string").text();
            showLogReport(file);
        },
        error: error = function (err, ajaxoptions, thrownError) {
            alert(err.status + "|" + thrownError);
        }
    });

    /*
    options = {};
    options.url = "FileProcessHandler.ashx";
    options.type = "POST";
    options.data = formData;
    options.contentType = false;
    options.processData = false;
    options.timeout = 600000;
    options.uploadProgress = function (event, position, total, percentComplete) {
        doIncrementProgressBar(percentComplete);
    };
    options.success = function (data) {
        showLogReport(data);
    };
    options.error = function (xhr, ajaxoptions, thrownError) {
        alert("Error1:" + xhr.status + "|" + thrownError);
    };

    $.ajax(options);
    
    */
}

function showLogReport(data) {
    var msg = "";

    var div = $('#divExportReport');

    div.empty();

    if (data.length > 0) {
        var file = baseUrl() + "logs/" + data;
        div.show();
        div.append("<a href='" + file + "' class='btn btn-link btn-sm'>Download Report Errors</a>");
        msg = 'Processo concluído com erros. Para mais informações, faça download do [Report Erros].';
        updateRowUnsuccessInfo(file);
    }
    else {
        div.hide();
        msg = 'Processo concluído sem erros.';
        updateRowSuccessInfo();
    }

    ShowMessage('Partner Solution', msg, 0);
}

function updateRowUnsuccessInfo(file) {
    $.ajax(file).done(function (content) {
        var allTextLines = content.split(/\r\n|\n/);

        for (var i = 1; i < allTextLines.length; i++) {
            var data = allTextLines[i].split(';');
            var line = data[0];
            var info = data[3];
            var row = $('#csv-table').find('tr:eq(' + line + ')').children('td');
            row.prop('title', info);
            row.css('background-color', '#FBCFD0');
        }
    });

    updateRowSuccessInfo();
}

function updateRowSuccessInfo() {
    $('#csv-table tbody tr').each((tr_idx, tr) => {
        if ($(tr).children('td').prop('title').length == 0) {
            $(tr).children('td').prop('title', 'Ação realizada com sucesso.');
            $(tr).children('td').css('background-color', '#B8D4E0');
        }
    });
}

function processRows(row) {

    var nome = row.eq(1).text();
    var sobrenome = row.eq(2).text();

    if (sobrenome.length == 0) {
        var nomes = nome.split(' ');

        if (nomes.length > 1) {
            sobrenome = nome.replace(nomes[0], '').trim();
            nome = nomes[0];
        }
    }

    var cpf = formatCPF(row.eq(3).text());

    if (!isCPF(cpf)) {
        row.css('background-color', '#FBCFD0');
        row.prop('title', 'O CPF ' + cpf + ' é inválido!');
        $('#divExportReport').show();
        return;
    }

    var email = row.eq(4).text().replace(" ", "");

    if (!isEmail(email)) {
        row.css('background-color', '#FBCFD0');
        row.prop('title', 'O Email ' + email + ' é inválido!');
        $('#divExportReport').show();
        return;
    }

    var acao = "";

    if (row.eq(9).text().length > 0)
        acao = row.eq(9).text().toLowerCase();

    var idUsuario = $("#hdnIdUsuario").val();

    var empresa = $(".mselEmpresas option:selected").text();

    var empresaId = $(".mselEmpresas").val();

    var formData = new FormData();

    formData.append("Empresa", empresa);
    formData.append("EmpresaId", empresaId);
    formData.append("Nome", nome);
    formData.append("Sobrenome", sobrenome);
    formData.append("Cpf", cpf);
    formData.append("Email", email);
    formData.append("Sexo", row.eq(5).text());
    formData.append("Dt_Nascimento", row.eq(6).text());
    formData.append("Departamento", row.eq(7).text());
    formData.append("Cargo", row.eq(8).text());
    formData.append("Acao", acao);
    formData.append("IdUsuario", idUsuario);

    var options = {};
    options.url = "FileProcessHandler.ashx";
    options.type = "POST";
    options.data = formData;
    options.async = false;
    options.contentType = false;
    options.processData = false;
    options.timeout = 60000;
    options.uploadProgress = function (event, position, total, percentComplete) {
        doIncrementProgressBar(percentComplete);
    };
    options.success = function (data) {
        doIncrementProgressBar(100);
        if (data.includes('sucesso')) {
            row.css('background-color', '#B8D4E0');
        }
        else {
            row.css('background-color', '#FBCFD0');
            $('#divExportReport').show();
        }
        row.prop('title', data);
    };
    options.error = function (data) {
        doIncrementProgressBar(100);
        row.prop('title', data);
        row.css('background-color', '#FBCFD0');
        $('#divExportReport').show();
    };

    $.ajax(options);
}

function exportLogError() {
    var log = "";

    $('#csv-table tbody tr').each((tr_idx, tr) => {
        var row = $(tr).children('td');

        var toolTip = row.prop('title');

        if (toolTip.length > 0) {
            if (!toolTip.includes("sucesso")) {

                var now = new Date(Date.now());

                var linha = "";
                linha += (tr_idx + 1) + ";";
                linha += row.eq(1).text() + ";";
                linha += row.eq(2).text() + ";";
                linha += toolTip + ";";
                linha += now.toISOString() + "\n";

                log += linha;
            }
        }
    });

    var file = "Log_" + Date.now() + ".csv";

    var blob = new Blob([log], { type: "text/plain;charset=utf-8" });
    saveAs(blob, file);
}