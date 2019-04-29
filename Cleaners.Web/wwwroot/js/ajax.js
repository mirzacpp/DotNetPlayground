// Executes ajax GET request against given URI
var loader = '.loading';

function getData(action, target, blockUi) {
    var $target = $('#' + target);

    if (blockUi) {
        $(loader).show();
    }

    $.get(action, function (data) {
        $target.html(data);
        $(loader).hide();
    });
}

function makeRequest(element, options) {
    $.extend(options, {
        type: element.getAttribute("data-ajax-method") || undefined,
        url: element.getAttribute("data-ajax-url") || undefined,
        cache: !!element.getAttribute("data-ajax-cache"),
        beforeSend: function () {
            $(element).find('button[type=submit]')
                .prop('disabled', true)
                .html('<i class="fa fa-spinner fa-spin"></i>');
        },
        success: function (data) {
            onSuccess(element, data);
        },
        error: function (data) {
            onError(data);
        }
    });

    options.data.push({ name: "X-Requested-With", value: "XMLHttpRequest" });

    $.ajax(options);
}

$(document).on("submit", "form[data-ajax=true]", function (event) {
    event.preventDefault();

    var val = '<div>Ok vlada</div>';
    var val2 = 'Ok vlada';

    // Check for HTML markup
    console.log(/<[a-z][\s\S]*>/i.test(val));
    console.log(/<[a-z][\s\S]*>/i.test(val2));
    
    makeRequest(this, {
        // Create ajax options based on form attributes
        url: this.action,
        type: this.method || 'GET',
        data: $(this).serializeArray()
    });
});