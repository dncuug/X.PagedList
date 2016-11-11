$('input#TagHelpers').change(function (eventObject) {
    $('.HtmlHelpers').hide();
    $('.TagHelpers').show();
});

$('input#HtmlHelpers').change(function (eventObject) {
    $('.TagHelpers').hide();
    $('.HtmlHelpers').show();
});
