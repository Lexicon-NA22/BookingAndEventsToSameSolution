// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const target = document.querySelector('#createajax');
document.querySelector('#fetch').addEventListener('click', getcreateform);

function getcreateform() {
    fetch('https://localhost:7097/gymclasses/createfetch', {
        method: 'GET'
    })
        .then(res => res.text())
        .then(data => {
            target.innerHTML = data;
            fixvalidation();
        })
        .catch(err => failcreate(err));
}

function removeform() {
    target.innerHTML = '';
}

function failcreate(response) {
    console.log(response, 'fail to create');
    target.innerHTML = response.responseText;
}

function fixvalidation() {
    const form = target.querySelector('form');
    $.validator.unobtrusive.parse(form);
}

$('#ajax').click(function () {
    $.ajax({
        url: "https://localhost:7097/gymclasses/create",
        type: 'get',
        success: success,
        fail: failcreate
    })
})

function success(response) {
    target.innerHTML = response;
    fixvalidation();
}

