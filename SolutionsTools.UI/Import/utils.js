$(document).ready(function () {
    $(document).ajaxStart(function () {
        $('#divProcessando').show();
    });

    $(document).ajaxStop(function () {
        $('#divProcessando').hide();
    });


});

function doIncrementProgressBar(increment) {
    w = parseInt(document.getElementById('progressBar').style.width);
    document.getElementById('progressBar').style.width = (w + increment) + '%';
    document.getElementById('progressPercent').textContent = increment + "%";
}

function formatCPF(cpf) {
    cpf = cpf.replace(/[^\d]/g, "");
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
}

function isCPF(cpf) {
    var Soma;
    var Resto;
    Soma = 0;

    cpf = cpf.replace(/[^\d]/g, "");

    if (cpf == "00000000000") return false;

    for (i = 1; i <= 9; i++) Soma = Soma + parseInt(cpf.substring(i - 1, i)) * (11 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(cpf.substring(9, 10))) return false;

    Soma = 0;
    for (i = 1; i <= 10; i++) Soma = Soma + parseInt(cpf.substring(i - 1, i)) * (12 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(cpf.substring(10, 11))) return false;
    return true;
}

function isEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function baseUrl() {
    var origin = window.location.origin + '/';
    return origin;
}