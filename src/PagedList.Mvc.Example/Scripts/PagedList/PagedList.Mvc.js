(function ($) {
    $.pagedList = function (template, generatePageUrl, options) {
        var options = $.extend({
            pagesToDisplay: 10
        }, options);

        function getRenderOptions(list) {
            var pagesToDisplay = options.pagesToDisplay;

            var halfOfPagesToDisplay = Math.floor(pagesToDisplay / 2);
            var arr = [];
            var start = list.PageNumber - halfOfPagesToDisplay;
            if (start < 1)
                start = 1;

            var end;
            if ((start + pagesToDisplay) > list.PageCount)
                end = list.PageCount;
            else
                end = start + pagesToDisplay;

            for (var i = start; i <= end; i++)
                arr.push(i);

            return {
                pages: arr,
                showStartEllipses: start != 1,
                showEndEllipses: arr[arr.length - 1] != list.PageCount
            };
        }

        return {
            render: function (list) {
                return $.tmpl(template, {
                    pagedList: list,
                    renderOptions: getRenderOptions(list),
                    generatePageUrl: generatePageUrl
                });
            }
        };
    };
})(jQuery); 