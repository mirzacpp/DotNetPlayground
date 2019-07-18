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
    corvo.logger.modul = "Global";

    corvo.logger.log = function (level, message, styles) {
        if (!window.console && !window.console.log) {
            return;
        }
        if (level === undefined && level < corvo.logger.level) {
            return;
        }
        
        console.log(message, corvo.logger.styles.MODUL, styles, corvo.logger.styles.MESSAGE);
    };

    corvo.logger.styles = {
        MESSAGE: 'color:#fff',
        MODUL: 'background: #acace6; color:#fff; padding: 1px 4px; border-radius: 2px; margin-right: 10px',
        DEBUG: 'background: #6c757d; color:#fff; padding: 1px 4px; border-radius: 2px; margin-right: 15px',
        INFO: 'background: #17a2b8; color:#fff; padding: 1px 4px; border-radius: 2px; margin-right: 15px',
        WARN: 'background: #ffc107; color:#fff; padding: 1px 4px; border-radius: 2px; margin-right: 10px',
        ERROR: 'background: #dc3545; color:#fff; padding: 1px 4px; border-radius: 2px; margin-right: 10px',
        FATAL: 'background: #dc3545; color:#fff; padding: 1px 4px; border-radius: 2px; margin-right: 10px'
    };

    corvo.logger.info = function (message, modul = corvo.logger.modul) {        
        corvo.logger.log(corvo.logger.levels.INFO, `%c${modul}%cINFO%c${message}`, corvo.logger.styles.INFO);        
    };

    corvo.logger.debug = function (message, modul = corvo.logger.modul) {
        corvo.logger.log(corvo.logger.levels.INFO, `%c${modul}%cINFO%c${message}`, corvo.logger.styles.DEBUG);
    };

    corvo.logger.warn = function (message, modul = corvo.logger.modul) {
        corvo.logger.log(corvo.logger.levels.INFO, `%c${modul}%cINFO%c${message}`, corvo.logger.styles.WARN);
    };

    corvo.logger.error = function (message, modul = corvo.logger.modul) {
        corvo.logger.log(corvo.logger.levels.INFO, `%c${modul}%cINFO%c${message}`, corvo.logger.styles.ERROR);
    };

    corvo.logger.fatal = function (message, modul = corvo.logger.modul) {
        corvo.logger.log(corvo.logger.levels.INFO, `%c${modul}%cINFO%c${message}`, corvo.logger.styles.FATAL);
    };

    /*============= USEFUL UTILS  =============*/

    corvo.utils = corvo.utils || {};

    corvo.utils.getRandomNumber = function (threshold, startFromZero = false) {
        return startFromZero ? Math.floor(Math.random() * threshold)
            : Math.floor(Math.random() * threshold) + 1;
    };
})(jQuery);