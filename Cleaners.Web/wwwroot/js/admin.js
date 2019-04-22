// Admin JavaScript helper file

var AHelper = {
    loader: '.loader-block',
    jqxhr: '',
    formbKey: '#formb',
    targetBlockKey: '#dataTarget',
    notificationDuration: 5000,
    timer: '',

    // Show or hide ajax loader
    showLoader: function (display) {
        display ? $(this.loader).show('slow') : $(this.loader).hide('slow');
    },

    // update content with ajax and use passed target selector to replace it.
    loadData: function (url, target) {
        this.showLoader(true);
        jqxhr = $.get(url);

        jqxhr.done(function (data) {
            $(target || AHelper.targetBlockKey).html(data);
            AHelper.showLoader(false);
        });

        // Implement fail mechanism ...
        jqxhr.fail(function () {
            // Redirect to error page ...
        });
    },

    // Use this for ajax calls like status change, backup database, delete etc.
    triggerAction: function (url) {
        // We clear timeout here because if there are multiple notifications, all of them will
        // use the one time out(ie. 2 notifications have 6 seconds, and one could only have 1 second out of 6).
        // This way, every notification will have its own timeout.
        clearTimeout(this.timer);
        this.showLoader(true);
        var postData = this.appendAntiforgeryToken();

        jqxhr = $.ajax({
            url: url,
            type: 'POST',
            data: postData,
            cache: false
        });

        jqxhr.done(function (data) {
            AHelper.displayNotification(data.message, data.type);
            // Reload data table only if status is true
            if (data.operationStatus) {
                AHelper.loadData(data.redirectUrl);
            }
            AHelper.showLoader(false);
        });

        // Handle exceptions in fail method
        jqxhr.fail(function (data) {
            AHelper.displayNotification(data.responseJSON.message, data.responseJSON.type);
            AHelper.showLoader(false);
        });
    },

    displayNotification: function (message, type) {
        // Remove previous class and then append current one
        $('#notify').removeClass().addClass(type);
        $('#notify').text(message).show('slow');

        this.timer = setTimeout(function () {
            $('#notify').hide('slow');
        }, AHelper.notificationDuration);
    },

    postData: function () {
        var form = $(AHelper.formbKey);

        jqxhr = $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: form.serialize(),
            cache: false
        });

        jqxhr.done(function (data) {
            if (data.operationStatus) {
                $("#modal-container").modal('hide');
                AHelper.displayNotification(data.message, data.type);
                AHelper.loadData(data.redirectUrl);
            } else {
                $("#modal-block").html(data);
            }
        });

        // Handle exceptions in fail method
        //jqxhr.fail(function (data) { })

        return false;
    },

    appendAntiforgeryToken: function (data) {
        // Instanciate data object if needed.
        data = (data || {});
        // Get token generated on view.
        var token = $('input[name=__RequestVerificationToken]');
        // If token exists, append it on data object
        if (token.length > 0) {
            data.__RequestVerificationToken = token.val();
        }
        return data;
    }
};