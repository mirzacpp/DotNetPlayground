/*============= UTILS FOR CHARTS  =============*/
var corvo = corvo || {};
(function ($) {
    corvo.charts = corvo.charts || {};

    // Some random colors
    corvo.charts.colors = [
        '#007bff',
        '#6c757d',
        '#28a745',
        '#dc3545',
        '#ffc107',
        '#17a2b8'
    ];

    corvo.charts.getDefinedRandomColor = function () {
        console.log(corvo.charts.colors[corvo.utils.getRandomNumber(corvo.charts.colors.length)]);
        return corvo.charts.colors[corvo.utils.getRandomNumber(corvo.charts.colors.length)];
    };
})(jQuery);