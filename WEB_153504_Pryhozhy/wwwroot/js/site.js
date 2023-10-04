// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('a.page-link').click(function (e) {
        e.preventDefault();

        var url = $(this).attr('href');

        $.get(url, function (data) {
            $('#catalog-partial').html(data);
        });
    });
});

