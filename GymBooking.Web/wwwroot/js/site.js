// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const target = document.querySelector('#createajax');

function removeform() {
    target.innerHTML = '';
}

function failcreate(response) {
    console.log(response, 'fail to create');
    target.innerHTML = response.responseText;
}