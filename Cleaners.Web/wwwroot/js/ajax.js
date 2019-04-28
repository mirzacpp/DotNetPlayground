// Executes ajax GET request against given URI

function getData(action, target, blockUi) {
    var $target = $('#' + target);

    if (blockUi) {
        $('.loading').show();
        console.log($('.loading'))
        console.log('ok')
    }

    $.get(action, function (data) {
        $target.html(data);
    });

    $('.loading').hide();
}