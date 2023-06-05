$.extend($.validator.messages, {
    required: "تکمیل این فیلد اجباری می باشد.",
    remote: "لطفا این فیلد را تصحیح نمائید.",
    email: "لطفا یک ایمیل صحیح وارد نمائید.",
    url: "لطفا آدرس صحیح وارد کنید.",
    date: "لطفا یک تاریخ صحیح وارد کنید",
    dateFA: "لطفا یک تاریخ صحیح وارد کنید",
    dateISO: "لطفا تاریخ صحیح وارد کنید (ISO).",
    number: "لطفا عدد صحیح وارد کنید.",
    digits: "لطفا تنها رقم وارد کنید",
    creditcard: "لطفا کریدیت کارت صحیح وارد کنید.",
    equalTo: "لطفا مقدار برابری وارد کنید",
    extension: "لطفا مقداری وارد کنید که ",
    maxlength: $.validator.format("لطفا بیشتر از {0} حرف وارد نکنید."),
    minlength: $.validator.format("لطفا کمتر از {0} حرف وارد نکنید."),
    rangelength: $.validator.format("لطفا مقداری بین {0} تا {1} حرف وارد کنید."),
    range: $.validator.format("لطفا مقداری بین {0} تا {1} حرف وارد کنید."),
    max: $.validator.format("لطفا مقداری کمتر از {0} وارد کنید."),
    min: $.validator.format("لطفا مقداری بیشتر از {0} وارد کنید."),
    minWords: $.validator.format("لطفا حداقل {0} کلمه وارد کنید."),
    maxWords: $.validator.format("لطفا حداکثر {0} کلمه وارد کنید.")
});

function loadImage(path, logintarget, forgottarget) {
    $('<img src="' + path + '">').load(function () {
        if (forgottarget != undefined) {
            $(logintarget + ', ' + forgottarget).html('');
            $(this).clone().appendTo(logintarget + ', ' + forgottarget);
        }
        else {
            $(logintarget).html('');
            $(this).clone().appendTo(logintarget);
        }
    });
}

$(document).ready(function () {
    loadImage($('#captcha-url').data('captchaUrl'), '[data-login-captcha-container]', '[data-forgot-captcha-container]');

    $('#moveright').click(function () {
        $('#textbox').animate({
            'marginRight': "0" //moves left
        });

        $('.toplam').animate({
            'marginRight': "100%" //moves right
        });

        $('#back-home').css({ 'right': '15px', 'left': '' });
        $('#login-registration').css({ 'right': '147px', 'left': '' });
    });
    $('#moveleft').click(function () {
        $('#textbox').animate({
            'marginRight': "50%" //moves right
        });

        $('.toplam').animate({
            'marginRight': "0" //moves right
        });

        $('#back-home').css({ 'left': '15px', 'right': '' });
        $('#login-registration').css({ 'left': '147px', 'right': '' });
    });

    $('form[name="logon-form"]').validate({
        errorPlacement: function (error, element) {
            element.siblings('[data-message]').text(error.text()).show();
        },
        success: function (label, element) {
            label.parent().removeClass(' ');
            label.remove();
        },
        rules: {
            password: {
                required: true,
                minlength: 8
            }
        }
    });

    $('form[name="forgotten-form"]').validate({
        errorPlacement: function (error, element) {
            element.siblings('[data-message]').text(error.text()).show();
        },
        success: function (label, element) {
            label.parent().removeClass(' ');
            label.remove();
        },
        rules: {
            email: {
                required: true,
                email: true,
                remote: {
                    url: "/login/checkemail",
                    type: "post"
                }
            },
        },
        messages: {
            email: {
                remote: 'کاربری با این آدرس ایمیل در سامانه وجود ندارد.'
            }
        }
    });

    $(document).on('submit', 'form[name="logon-form"]', function (event) {
        event.preventDefault();
        event.stopPropagation();

        var $form = $(this),
            $alert = $('#modal-alert'),
            $submitButton = $('#logon-submit'),
            $defaultSubmitText = $submitButton.val();

        $submitButton
            .addClass('disabled')
            .val('در حال پردازش ...')
            .prop('disabled', true);

        $.ajax({
            type: "POST",
            url: $form.attr('action'),
            data: $form.serialize(),
            //complete: function (xhr, status) {
            //    if (xhr.status == 401) {
            //        window.location = '/auth/signin';
            //    }
            //},
            success: function (data, textStatus, xhr) {
                //var ct = xhr.getResponseHeader("content-type") || "";
                //if (ct.indexOf('html') > -1) {
                //    $('#jcr-result').html(data);
                //    Journals.Plugins.bsTooltip('#jcr-result [data-toggle=tooltip]');
                //}
                //else if (ct.indexOf('json') > -1) {
                $alert.iziModal('destroy');

                if (data.type === 'error') {
                    $alert.iziModal({
                        title: data.title,
                        subtitle: data.message,
                        icon: 'material-icons',
                        iconText: 'error',
                        headerColor: '#BD5B5B',
                        width: 600,
                        timeout: 5000,
                        timeoutProgressbar: true,
                        transitionIn: 'fadeInUp',
                        transitionOut: 'fadeOutDown',
                        rtl: true
                    });

                    $('.captcha-container').trigger('click');
                }
                else if (data.type === 'success') {
                    $alert.iziModal({
                        title: data.title,
                        subtitle: data.message,
                        icon: 'material-icons',
                        iconText: 'done',
                        headerColor: '#00af66',
                        width: 600,
                        timeout: 2000,
                        timeoutProgressbar: true,
                        transitionIn: 'fadeInUp',
                        transitionOut: 'fadeOutDown',
                        rtl: true
                    });

                    $(document).on('closed', '#modal-alert', function (e) {
                        window.location.href = data.redirectUrl;
                    });
                }

                $alert.iziModal('open');
            },
            complete: function () {
                $submitButton
                    .val($defaultSubmitText)
                    .removeClass('disabled')
                    .prop('disabled', false);
            }
        });
    });

    $(document).on('submit', 'form[name="forgotten-form"]', function (event) {
        event.preventDefault();
        event.stopPropagation();

        var $form = $(this),
            $alert = $('#modal-alert'),
            $submitButton = $('#forgotten-submit'),
            $defaultSubmitText = $submitButton.val();

        $submitButton
            .addClass('disabled')
            .val('در حال پردازش ...')
            .prop('disabled', true);

        $.ajax({
            type: "POST",
            url: $form.attr('action'),
            data: $form.serialize(),
            success: function (data, textStatus, xhr) {
                $alert.iziModal('destroy');
                $alert.iziModal({
                    title: data.title,
                    subtitle: data.message,
                    icon: 'material-icons',
                    iconText: 'error',
                    headerColor: '#BD5B5B',
                    width: 600,
                    timeout: 7000,
                    timeoutProgressbar: true,
                    transitionIn: 'fadeInUp',
                    transitionOut: 'fadeOutDown',
                    rtl: true
                });

                if (data.type === 'success') {
                    $alert.iziModal('setIconText', 'done');
                    $alert.iziModal('setHeaderColor', '#00af66');
                }
                else {
                    $('.captcha-container').trigger('click');
                }

                $alert.iziModal('open');
            },
            complete: function () {
                $submitButton
                    .val($defaultSubmitText)
                    .removeClass('disabled')
                    .prop('disabled', false);
            }
        });
    });

});

// Show forgotten password alert
$(document).ready(function () {
    if ($('#forgotten-password-alert').length > 0) {
        var $alert = $('#modal-alert'),
            $dataElement = $('#forgotten-password-alert');

        $alert.iziModal('destroy');
        $alert.iziModal({
            title: $dataElement.data('title'),
            subtitle: $dataElement.data('message'),
            icon: 'material-icons',
            iconText: $dataElement.data('type'),
            headerColor: '#5191b1',
            width: 600,
            timeout: 7000,
            timeoutProgressbar: true,
            transitionIn: 'fadeInUp',
            transitionOut: 'fadeOutDown',
            rtl: true
        });

        if ($dataElement.data('type') === 'success') {
            $alert.iziModal('setIconText', 'done');
            $alert.iziModal('setHeaderColor', '#00af66');
        }

        setTimeout(function() { $alert.iziModal('open') }, 1000)
    }
});

$(window).load(function () {
    $('.form-control').on('focus blur', function (e) {
        $(this).parents('.form-group').toggleClass('focused', (e.type === 'focus' || this.value.length > 0));
    }).trigger('blur');
});

$(document).on('click', '.captcha-container', function () {
    var captchaUrl = $('#captcha-url').data('captchaUrl').split('=')[0] + '=';
    var date = new Date();
    var ticks = ((date.getTime() * 10000) + 621355968000000000);

    loadImage(captchaUrl + ticks, '[data-login-captcha-container]', '[data-forgot-captcha-container]');
});

$(document).on('click', '.captcha-refresh a', function (e) {
    e.preventDefault();
    $('.captcha-container').trigger('click');
})
