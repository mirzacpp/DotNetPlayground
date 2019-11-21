var loader = '.loading';
var mainModalId = "#modal-container";

// Executes ajax GET request against given URI
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

function getIdAttribute(element, name) {
    var target = element.getAttribute(name) || "";

    if (!target) {
        return;
    }

    if (target.includes('#'))
        return target;
    return '#' + target;
}

// Handles success of ajax request
function onSuccess(element, data, contentType) {
    // Check if response is of type text/html and update target
    if (contentType.indexOf("html") > -1) {
        $(getIdAttribute(element, "data-ajax-update")).html(data);
    }
    else if (data.cancelRedirect) {
        $(mainModalId).modal('hide');
    }
    else {
        $(mainModalId).fadeOut('100', function () {
            // If page contains search form, resubmit so we keep current form state
            var target = $("form[data-form-search=true]");
            if (target.length > 0) {
                $(target).submit();
            }
            // Otherwise redirect to given Url
            else {
                window.location.href = data.redirectUrl;
            }
        });
    }
}

// Handles errors of ajax request
function onError(data) {
}

function makeRequest(element, options) {
    $.extend(options, {
        type: element.getAttribute("data-ajax-method") || undefined,
        url: element.getAttribute("data-ajax-url") || undefined,
        cache: false,
        beforeSend: function () {
            $(loader).show();
            $(element).find('button[type=submit]')
                .prop('disabled', true)
                .html('<i class="fa fa-spinner fa-spin"></i>');
        },
        success: function (data, status, xhr) {
            onSuccess(element, data, xhr.getResponseHeader("content-type"));
        },
        error: function (data) {
            onError(data);
        },
        complete: function () {
            $(loader).hide();
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