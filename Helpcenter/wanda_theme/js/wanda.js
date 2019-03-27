$(document).ready(function() {
    $('#goBackButton').on('click', function() {
        if (document.referrer.length > 0)
            window.history.back();
        else
            window.location.href = $(this).attr("data-target");
    });

    $('#toTopButton').on('click', function() {
        scrollToItem('body');
    });

    $("a[data-target]").on('click', function() {
        scrollToItem($(this).attr("data-target"));
    });
});

function scrollToItem(idTag) {
    var offset = $(idTag).offset().top - $('#header').innerHeight();
    $('body,html').animate({ scrollTop: offset }, 250);
}