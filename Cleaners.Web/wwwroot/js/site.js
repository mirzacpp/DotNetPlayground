var admin = admin || {};
(function ($) {
    /*============= GLOBALS  =============*/

    admin.generateRandomId = function () {
        return '_' + Math.random().toString(36).substr(2, 9);
    };

    /*============= CLIENT SIDE ALERTS =============*/

    admin.alerts = admin.alerts || {};
    admin.alerts.alertBlockId = '#alertBlock';
    admin.alerts.autoHideDelay = 5000;

    admin.alerts.alertTypes = {
        SUCCESS: 'success',
        DANGER: 'danger',
        WARNING: 'warning',
        INFO: 'info'
    };

    admin.alerts.create = function (text, type = admin.alerts.alertTypes.INFO, isDissmisable = true, autoHide = true) {
        var alert = $('<div>', { class: 'alert alert-' + type + ' no-margin', role: "alert", id: admin.generateRandomId() });

        if (isDissmisable) {
            var button = $('<button>', { class: 'close', type: 'button' });

            button.attr('data-dismiss', 'alert').attr('aria-label', 'Close');
            button.html('<span aria-hidden="true">&times;</span>');
            alert.append(button);
        }

        if (autoHide) {
            setTimeout(function () {
                alert.fadeOut(3000, function () {
                    $(this).remove();
                });
            }, admin.alerts.autoHideDelay);
        }

        alert.append(text);
        $(admin.alerts.alertBlockId).append(alert);
    };
})(jQuery);