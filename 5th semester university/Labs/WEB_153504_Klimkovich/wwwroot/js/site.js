$(document).ready(function () {
    $('.page-link').click(function (event) {
        event.preventDefault();
        var url = $(this).attr('href');

        $.get(url, function (data) {
            $('#partialContainer').html(data);
        });
    });
});
