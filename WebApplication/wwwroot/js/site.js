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
    $('form[name="datapick"]').submit();
});
$('#my_hidden_input')


var active = false;

function toggel() {
    const notif = document.getElementById('box');
    if (active) {
        notif.style.height = '0px';
        notif.style.opacity = 0;
        active = false;
    }
    else {
        notif.style.height = '510px';
        notif.style.opacity = 1;
        active = true;
    }
    console.log("toggel");
}

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