// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#datepicker").datepicker({
    format: "dd-mm-yyyy",
});
$('#datepicker').on('changeDate', function () {
    console.log($(this));
    $('#my_hidden_input').val(
        $('#datepicker').datepicker('getFormattedDate')       
    );
    $('form').submit();
});
$('#my_hidden_input')

const darkmodeCheckbox = document.getElementById('darkmode-checkbox');
if (localStorage.getItem('checkbox') == 'true') {
    document.body.classList.add('dark');
    darkmodeCheckbox.checked = true;
}
else {
    document.body.classList.remove('dark');
}


darkmodeCheckbox.addEventListener('change', () => {
    if (localStorage.getItem('checkbox') == 'true') {
        localStorage.setItem('checkbox', 'false');
    }
    else {
        localStorage.setItem('checkbox', 'true');
    }

    if (localStorage.getItem('checkbox') == 'true') {
        document.body.classList.add('dark');
    }
    else {
        document.body.classList.remove('dark');
    }
    
});