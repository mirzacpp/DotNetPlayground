// Executes ajax GET request against given URI
var loader = '.loading';
var htmlRegex = '/<[a-z][\s\S]*>/i';

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

// Handles success of ajax request
function onSuccess(element, data) {
    // Check if response is of type text/html
    if (/<[a-z][\s\S]*>/i.test(data)) {
        console.log(data);
    }
    else {
        
    }
}

// Handles errors of ajax request
function onError(data) {
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

// Executes ajax request for all form elements that contains "data-ajax" attribute
$(document).on("submit", "form[data-ajax=true]", function (event) {
    event.preventDefault();

    makeRequest(this, {
        // Create ajax options based on form attributes
        url: this.action,
        type: this.method || 'GET',
        data: $(this).serializeArray()
    });
});