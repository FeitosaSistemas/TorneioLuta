// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('.alert').hide();

$('#btStart').on('click', function () {
    var valuesCB = [];
    $('.alert').hide();

    var inputs = document.querySelectorAll(".form-check-input:checked");
    if (inputs.length != 20) {
        $('.alert').show()
        return;
    }

    for (var item in inputs) {

        if (inputs[item].value !== undefined)
            valuesCB.push(inputs[item].value);
    }

    window.location.href = "Home/Resultado?ids=" + valuesCB;
});
