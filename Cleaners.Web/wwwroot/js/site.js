var corvo = corvo || {};
(function ($) {
    /*============= GLOBALS  =============*/

    corvo.generateRandomId = function () {
        return '_' + Math.random().toString(36).substr(2, 9);
    };

    /*============= CLIENT SIDE ALERTS =============*/

    corvo.alerts = corvo.alerts || {};
    corvo.alerts.alertBlockId = '#alertBlock';
    corvo.alerts.autoHideDelay = 5000;

    corvo.alerts.alertTypes = {
        SUCCESS: 'success',
        DANGER: 'danger',
        WARNING: 'warning',
        INFO: 'info'
    };

    corvo.alerts.create = function (text, type = corvo.alerts.alertTypes.INFO, isDissmisable = true, autoHide = true) {
        var alert = $('<div>', { class: 'alert alert-' + type + ' no-margin', role: "alert", id: corvo.generateRandomId() });

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
            }, corvo.alerts.autoHideDelay);
        }

        alert.append(text);
        $(corvo.alerts.alertBlockId).append(alert);
    };

    /*============= CLIENT SIDE LOGGING  =============*/

    corvo.logger = corvo.logger || {};

    corvo.logger.levels = {
        DEBUG: 1,
        INFO: 2,
        WARN: 3,
        ERROR: 4,
        FATAL: 5
    };

    corvo.logger.level = corvo.logger.levels.DEBUG;

    corvo.logger.log = function (message, level, styles) {
        if (!window.console && !window.console.log) {
            return;
        }
        if (level === undefined && level < corvo.logger.level) {
            return;
        }

        console.log(message, styles || "");
    };

    corvo.logger.styles = {
        DEBUG: 'background: #6c757d; padding: 1px 4px; border-radius: 2px',
        INFO: 'background: #17a2b8; padding: 1px 4px; border-radius: 2px',
        WARN: 'background: #ffc107; padding: 1px 4px; border-radius: 2px',
        ERROR: 'background: #dc3545; padding: 1px 4px; border-radius: 2px',
        FATAL: 'background: #dc3545; padding: 1px 4px; border-radius: 2px'
    };

    corvo.logger.info = function (message) {
        corvo.logger.log("%cINFO %cCao", corvo.logger.levels.INFO, corvo.logger.styles.INFO, 'color: #fff');
        corvo.logger.log(message, corvo.logger.levels.INFO);
    };

    corvo.logger.debug = function (message) {
        corvo.logger.log("%cDEBUG", corvo.logger.levels.DEBUG, corvo.logger.styles.DEBUG);
        corvo.logger.log(message, corvo.logger.levels.DEBUG);
    };

    corvo.logger.warn = function (message) {
        corvo.logger.log("%cWARN", corvo.logger.levels.WARN, corvo.logger.styles.WARN);
        corvo.logger.log(message, corvo.logger.levels.WARN);
    };

    corvo.logger.error = function (message) {
        corvo.logger.log("%cERROR", corvo.logger.levels.ERROR, corvo.logger.styles.ERROR);
        corvo.logger.log(message, corvo.logger.levels.ERROR);
    };

    corvo.logger.fatal = function (message) {
        corvo.logger.log("%cFATAL", corvo.logger.levels.FATAL, corvo.logger.styles.FATAL);
        corvo.logger.log(message, corvo.logger.levels.FATAL);
    };

    /*============= USEFUL UTILS  =============*/

    corvo.utils = corvo.utils || {};

    corvo.utils.getRandomNumber = function (threshold, startFromZero = false) {
        return startFromZero ? Math.floor(Math.random() * threshold)
            : Math.floor(Math.random() * threshold) + 1;
    };
})(jQuery);