/*============= UTILS FOR CHARTS  =============*/
var corvo = corvo || {};
(function ($) {
    corvo.charts = corvo.charts || {};

    // Base bootstrap colors
    corvo.charts.colors = [
        '#007bff',
        '#6c757d',
        '#28a745',
        '#dc3545',
        '#ffc107',
        '#17a2b8'
    ];

    corvo.charts.getDefinedRandomColor = function () {
        return corvo.charts.colors[corvo.utils.getRandomNumber(corvo.charts.colors.length - 1)];
    };
})(jQuery);