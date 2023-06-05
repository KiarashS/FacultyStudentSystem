$(function () {
    var $switcher = $('#style_switcher'),
        $switcher_toggle = $('#style_switcher_toggle'),
        $theme_switcher = $('#theme_switcher'),
        $mini_sidebar_toggle = $('#style_sidebar_mini'),
        $slim_sidebar_toggle = $('#style_sidebar_slim'),
        $boxed_layout_toggle = $('#style_layout_boxed'),
        $accordion_mode_toggle = $('#accordion_mode_main_menu'),
        $html = $('html'),
        $body = $('body');


    $switcher_toggle.click(function (e) {
        e.preventDefault();
        $switcher.toggleClass('switcher_active');
    });

    $theme_switcher.children('li').click(function (e) {
        e.preventDefault();
        var $this = $(this),
            this_theme = $this.attr('data-app-theme');

        $theme_switcher.children('li').removeClass('active_theme');
        $(this).addClass('active_theme');
        $html
            .removeClass('app_theme_a app_theme_b app_theme_c app_theme_d app_theme_e app_theme_f app_theme_g app_theme_h app_theme_i app_theme_dark')
            .addClass(this_theme);

        if (this_theme == '') {
            localStorage.removeItem('altair_theme');
            $('#kendoCSS').attr('href', 'bower_components/kendo-ui/styles/kendo.material.min.css');
        } else {
            localStorage.setItem("altair_theme", this_theme);
            if (this_theme == 'app_theme_dark') {
                $('#kendoCSS').attr('href', 'bower_components/kendo-ui/styles/kendo.materialblack.min.css')
            } else {
                $('#kendoCSS').attr('href', 'bower_components/kendo-ui/styles/kendo.material.min.css');
            }
        }

    });

    // hide style switcher
    $document.on('click keyup', function (e) {
        if ($switcher.hasClass('switcher_active')) {
            if (
                (!$(e.target).closest($switcher).length)
                || (e.keyCode == 27)
            ) {
                $switcher.removeClass('switcher_active');
            }
        }
    });

    // get theme from local storage
    if (localStorage.getItem("altair_theme") !== null) {
        $theme_switcher.children('li[data-app-theme=' + localStorage.getItem("altair_theme") + ']').click();
    }


    // toggle mini sidebar

    // change input's state to checked if mini sidebar is active
    if ((localStorage.getItem("altair_sidebar_mini") !== null && localStorage.getItem("altair_sidebar_mini") == '1') || $body.hasClass('sidebar_mini')) {
        $mini_sidebar_toggle.iCheck('check');
    }

    $mini_sidebar_toggle
        .on('ifChecked', function (event) {
            $switcher.removeClass('switcher_active');
            localStorage.setItem("altair_sidebar_mini", '1');
            localStorage.removeItem('altair_sidebar_slim');
            location.reload(true);
        })
        .on('ifUnchecked', function (event) {
            $switcher.removeClass('switcher_active');
            localStorage.removeItem('altair_sidebar_mini');
            location.reload(true);
        });

    // toggle slim sidebar

    // change input's state to checked if mini sidebar is active
    if ((localStorage.getItem("altair_sidebar_slim") !== null && localStorage.getItem("altair_sidebar_slim") == '1') || $body.hasClass('sidebar_slim')) {
        $slim_sidebar_toggle.iCheck('check');
    }

    $slim_sidebar_toggle
        .on('ifChecked', function (event) {
            $switcher.removeClass('switcher_active');
            localStorage.setItem("altair_sidebar_slim", '1');
            localStorage.removeItem('altair_sidebar_mini');
            location.reload(true);
        })
        .on('ifUnchecked', function (event) {
            $switcher.removeClass('switcher_active');
            localStorage.removeItem('altair_sidebar_slim');
            location.reload(true);
        });

    // toggle boxed layout

    if ((localStorage.getItem("altair_layout") !== null && localStorage.getItem("altair_layout") == 'boxed') || $body.hasClass('boxed_layout')) {
        $boxed_layout_toggle.iCheck('check');
        $body.addClass('boxed_layout');
        $(window).resize();
    }

    $boxed_layout_toggle
        .on('ifChecked', function (event) {
            $switcher.removeClass('switcher_active');
            localStorage.setItem("altair_layout", 'boxed');
            location.reload(true);
        })
        .on('ifUnchecked', function (event) {
            $switcher.removeClass('switcher_active');
            localStorage.removeItem('altair_layout');
            location.reload(true);
        });

    // main menu accordion mode
    if ($sidebar_main.hasClass('accordion_mode')) {
        $accordion_mode_toggle.iCheck('check');
    }

    $accordion_mode_toggle
        .on('ifChecked', function () {
            $sidebar_main.addClass('accordion_mode');
        })
        .on('ifUnchecked', function () {
            $sidebar_main.removeClass('accordion_mode');
        });

});

$(document).ready(function () {
    // for back label to default position
    $('#notetext').focusout()

    Parsley.addMessages('fa', {
      defaultMessage: "این مقدار صحیح نمی باشد.",
      type: {
        email:        "این مقدار باید یک ایمیل معتبر باشد.",
        url:          "این مقدار باید یک آدرس لینک معتبر باشد.",
        number:       "این مقدار باید یک عدد معتبر باشد.",
        integer:      "این مقدار باید یک عدد صحیح معتبر باشد.",
        digits:       "این مقدار باید یک عدد باشد.",
        alphanum:     "این مقدار باید حروف الفبا باشد."
      },
      notblank:       "این مقدار نباید خالی باشد.",
      required:       "این مقدار باید وارد شود.",
      pattern:        "این مقدار به نظر می رسد نامعتبر است.",
      min:            "این مقدیر باید بزرگتر با مساوی %s باشد.",
      max:            "این مقدار باید کمتر و یا مساوی %s باشد.",
      range:          "این مقدار باید بین %s و %s باشد.",
      minlength:      "این مقدار بیش از حد کوتاه است. باید %s کاراکتر یا بیشتر باشد.",
      maxlength:      "این مقدار بیش از حد طولانی است. باید %s کاراکتر یا کمتر باشد.",
      length:         "این مقدار نامعتبر است و باید بین %s و %s باشد.",
      mincheck:       "شما حداقل باید %s گزینه را انتخاب کنید.",
      maxcheck:       "شما حداکثر می‌توانید %s انتخاب داشته باشید.",
      check:          "باید بین %s و %s مورد انتخاب کنید.",
      equalto:        "این مقدار باید یکسان باشد."
    });

    Parsley.setLocale('fa');

    $(document).on('submit', 'form[name="noteform"]', function (event) {
        event.preventDefault();
        event.stopPropagation();

        var $form = $(this);

        $.ajax({
            type: "POST",
            url: $form.attr('action'),
            data: $form.serialize(),
            success: function (data, textStatus, xhr) {
                UIkit.notify(data.title, { status: data.type });
            },
            error: function () {
                UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
            }
        });
    });


    $('form.dovalidate').each(function () {
        if ($(this).length <= 0)
        {
            return;
        }

        $(this).parsley({
            excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
            trigger: "change focusin blur",
            errorsWrapper: '<div class="parsley-errors-list"></div>',
            errorTemplate: "<span></span>",
            errorClass: "md-input-danger",
            successClass: "md-input-success",
            errorsContainer: function (e) {
                if (e.$element.is('input[type="file"]')) {
                    return e.$element.parents('.jtable-input-field-container');
                }
                else if (e.$element.is("input")) {
                    return e.$element.parent('.md-input-wrapper').parent();
                }
                else if (e.$element.is("select")) {
                    return e.$element.parent('.jtable-input jtable-dropdown-input').parent();
                }
            },
            classHandler: function (e) {
                var a = e.$element;
                if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
            }
        });
    });

    $('form.ajaxsubmit')
        .ajaxForm({
            beforeSubmit: function (arr, $form, options) {
                if ($form.hasClass('load-waiter') && ($('#updateindex').length > 0 && $('#updateindex').prop('checked') == true))
                {
                    $('#load-waiting').show();
                }
            },
            success: function(data, status, xhr, $form) {
                $('#load-waiting').hide();
                UIkit.notify(data.title, { status: data.type });
                if ($('.captcha-container').length > 0 && data.type != 'success') {
                    $('.captcha-container').trigger('click');
                }
                if ($form.hasClass('resetform') && data.title.indexOf('کد امنیتی') < 0 && data.title.indexOf('تصویر امنیتی') < 0) {
                    $form.resetForm();
                }
                if (data.redirectUrl && data.type == 'success') {
                    setTimeout(function () { window.location.href = data.redirectUrl; }, 2000);
                }
            },
            error: function () {
                $('#load-waiting').hide();
                UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
            }
        });

    $(document).on('click', '.editorsubmit', function () {
        var $btn = $(this),
            submitUrl = $btn.data('submitUrl'),
            editorId = $btn.data('editorId'),
            isactiveId = $btn.data('isactiveId'),
            $editor = $('#' + editorId),
            data = {};

        data[editorId] = $editor.trumbowyg('html');
        data[isactiveId] = $('#' + isactiveId).prop('checked');

        $.ajax({
            type: "POST",
            url: submitUrl,
            data: data,
            success: function (data, textStatus, xhr) {
                UIkit.notify(data.title, { status: data.type });
            },
            error: function () {
                UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
            }
        });
    });

    //function escapeRegExp(str) {
    //    return str.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
    //}
    if ($('#sections-order').length > 0){
        $('#sections-order-list').sortable({
            axis: 'y',
            cursor: 'move',
            revert: true,
            placeholder: 'sortable-placeholder',
            update: function (event, ui) {
                var order = 1;
                var $sortableList = $('#sections-order-list > li').each(function (item) {
                    var $this = $(this);
                    var itemName = $this.data('itemName');
                    $this.attr('data-sort', itemName + '_' + order);
                    order++;
                });

                //var data = $(this).sortable('serialize', { attribute: "data-sort" });
                //data = data.replace(new RegExp(escapeRegExp('[]'), 'g'), '');
                //var dataArray = data.split('&');
                var dataArray = $(this).sortable('toArray', { attribute: "data-sort" });
                //console.log(dataArray);
                $.ajax({
                    data: {orders: dataArray},
                    type: 'POST',
                    url: $('#variables').data('updateUrl'),
                    error: function () {
                        UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
                    },
                    success: function (data, textStatus, xhr) {
                        UIkit.notify(data.title, { status: data.type });
                    }
                });
            }
        });
    }

});

(function ($) {

    $.extend(true, $.hik.jtable.prototype.options.messages, {
        serverCommunicationError: 'خطا در برقراری ارتباط با سرور',
        loadingMessage: 'بارگذاری اطلاعات ...',
        noDataAvailable: 'هیچ داده ای جهت نمایش موجود نیست!',
        addNewRecord: 'رکورد جدید',
        editRecord: 'ویرایش',
        areYouSure: 'آیا اطمینان دارید ؟',
        deleteConfirmation: 'آیا از حذف این رکورد اطمینان دارید ؟',
        save: 'ذخیره',
        saving: 'در حال ذخیره ...',
        cancel: 'انصراف',
        deleteText: 'حذف',
        deleting: 'در حال حذف ...',
        error: 'خطا',
        close: 'بستن',
        cannotLoadOptionsFor: 'امکان بارگذاری   انتخاب ها نیست برای فیلد {0}',
        pagingInfo: 'نمایش  {0}-{1} از {2}',
        canNotDeletedRecords: 'نمی توان {0} از {1} رکورد را حذف کرد!',
        deleteProggress: 'حذف {0} از {1} رکورد، در حال پردازش ...',
        pageSizeChangeLabel: 'تعداد سطرها',
        gotoPageLabel: 'برو به صفحه',
    });

})(jQuery);

$(document).ready(function () {
    if($('#admin-details').length > 0)
    {
        $('#email').inputmask();

        $('input').focusout();
    }

    if($('#avatar-form').length > 0)
    {
        var drEvent = $('.dropify').dropify({
                messages: {
                    'default': 'تصویر مورد نظرتان را کشیده و در اینجا رها کنید و یا بر روی این قسمت کلیک نمائید.',
                    'replace': 'تصویر مورد نظرتان را برای جایگزینی کشیده و در اینجا رها کنید و یا بر روی این قسمت کلیک نمائید.',
                    'remove': 'حذف تصویر',
                    'error': 'خطا در برقراری ارتباط با سرور!'
                },
                error: {
                    'fileSize': 'حداکثر حجم تصویر 3 مگابایت می باشد.',
                    'imageFormat': 'تنها از تصاویر با پسوندهای {{ value }} می توان استفاده کرد.',
                    'fileExtension': 'تنها از تصاویر با پسوندهای {{ value }} می توان استفاده کرد.'
                }
        });

        if ($('.dropify-render').children('img').length > 0)
        {
            $('form').attr('action', $('#variables').data('avatarUrl') + '?hasPreview=true')
        }
        else
        {
            $('form').attr('action', $('#variables').data('avatarUrl'))
        }

        drEvent.on('dropify.afterClear', function(event, element){
            $('form').attr('action', $('#variables').data('avatarUrl'))
        });
    }

    if ($('#biotext').length > 0)
    {
        $('#IsActiveBio').iCheck({
            checkboxClass: 'icheckbox_md'
        })
        .on('ifChecked', function (event) {
            $('#IsActiveBio').val('true').prop('checked', true);
        })
        .on('ifUnchecked', function (event) {
            $('#IsActiveBio').val('false').prop('checked', false);
        })

        $('#biotext').trumbowyg({
            lang: 'fa',
            removeformatPasted: true,
            autogrow: true,
            btnsDef: {
                image: {
                    //dropdown: ['insertImage', 'base64'],
                    dropdown: ['insertImage'],
                    ico: 'insertImage'
                }
            },
            btns: [
                'btnGrp-semantic',
                ['foreColor', 'backColor'],
                ['formatting'],
                ['link'],
                'btnGrp-justify',
                'btnGrp-lists',
                ['image', 'table'],
                ['horizontalRule'],
                ['removeformat'],
                ['fullscreen']
            ]
        });

        $('#biotext').trumbowyg('html', $('#dataBioHtml').html());
    }

    if ($('#freepagetext').length > 0) {
        $('#IsActiveFreePage').iCheck({
            checkboxClass: 'icheckbox_md'
        })
        .on('ifChecked', function (event) {
            $('#IsActiveFreePage').val('true').prop('checked', true);
        })
        .on('ifUnchecked', function (event) {
            $('#IsActiveFreePage').val('false').prop('checked', false);
        })

        $('#freepagetext').trumbowyg({
            lang: 'fa',
            removeformatPasted: true,
            autogrow: true,
            btnsDef: {
                image: {
                    //dropdown: ['insertImage', 'base64'],
                    dropdown: ['insertImage'],
                    ico: 'insertImage'
                }
            },
            btns: [
                'btnGrp-semantic',
                ['foreColor', 'backColor'],
                ['formatting'],
                ['link'],
                'btnGrp-justify',
                'btnGrp-lists',
                ['image', 'table'],
                ['horizontalRule'],
                ['removeformat'],
                ['fullscreen']
            ]
        });

        $('#freepagetext').trumbowyg('html', $('#dataFreePageHtml').html());
    }

    if ($('#professor-details').length > 0)
    {
        $('#secondaryemails, #researchfields, #interests').selectize({
            plugins: {
                'remove_button': {
                    label: '',
                    title: 'حذف'
                }
            },
            delimiter: ',',
            create: function (input) {
                return {
                    value: input,
                    text: input
                }
            },
            render: {
                'option_create': function (data, escape) {
                    return '<div class="create">افزودن <strong>' + escape(data.input) + '</strong>&hellip;</div>';
                }
            },
            onDropdownOpen: function (dropdown) {
                dropdown.remove();
            }
        });

        $('#email').inputmask();
        //$('#personalwebpage, #scopusid, #googlescholarid, #pubmedid, #medlibid, #researchgateid, #orcidid, #researcherid').inputmask({ alias: "url" });
        $('#mobile').inputmask('Regex');

        $(document).ready(function () {
            var birthDate = $('#birthdaydate').pDatepicker({
                viewMode: 'year',
                format: 'YYYY/MM/DD',
                autoClose: true,
                observer: true
            });

            var $selectizedSecondaryEmails = $('#secondaryemails-selectized, #researchfields-selectized, #interests-selectized').parent('.selectize-input');
            $selectizedSecondaryEmails.css({ border: '0', borderBottom: '1px solid #d0d0d0' });
            $selectizedSecondaryEmails.parent('.selectize-control').siblings('.md-input-bar').css('bottom', '7px');

            $('input').focusout();

            if (detailsHasBirthDate) {
                try {
                    $("#birthdaydate").pDatepicker("setDate", [detailsBirthDateYear, detailsBirthDateMonth, detailsBirthDateDay, 12, 0]);
                }
                catch (err) { }
            }
            else {
                $('#birthdaydate').val('');
            }

            $('#birthdaydate').focusout();
        });
    }

    if($('#users_crud').length > 0)
    {
        $(document).on('click', 'a[data-reset-password]', function () {
            var $this = $(this),
                userId = $this.data('resetPassword');

            UIkit.modal.confirm('آیا مطمئنید که می خواهید برای کاربر مورد نظر رمز عبور جدید ارسال گردد؟', function () {
                altair_helpers.content_preloader_show('md', 'info', 'body', 48, 48);
                $.ajax({
                    type: "POST",
                    url: '/dashboard/admin/resetpassword',
                    data: { userId },
                    success: function (data, textStatus, xhr) {
                        altair_helpers.content_preloader_hide('md');
                        UIkit.modal.alert(data.title + '<br />' + data.newPassword, {labels : { Ok: 'بستن' } });
                    },
                    error: function () {
                        altair_helpers.content_preloader_hide('md');
                        UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
                    }
                });
            }, {labels: { Cancel:'انصراف', Ok: 'تائید' }});
        });

        $(document).ready(function () {
            if ($('#showpendingregistration').length > 0)
            {
                $('#filter-issoftdelete').trigger('click');
                $('#admin-users-search').trigger('click');
            }
        });
    }

    if ($('#manageradminmessages_crud').length > 0) {
        $(document).ready(function () {
            if ($('#showpendingmessages').length > 0) {
                $('#filter-pending-only').trigger('click');
                $('#admin-adminmessages-search').trigger('click');
            }
        });
    }

    if ($('#update-externalarticles-list').length > 0)
    {
        $(document).on('click', '#update-externalarticles-list', function () {
            UIkit.modal.confirm('آیا مطمئنید که می خواهید مقالات خودتان را به صورت خودکار استخراج نمائید؟', function () {
                $('#load-waiting').show();

                $.ajax({
                    type: "POST",
                    url: '/dashboard/professor/updateexternalarticles',
                    success: function (data, textStatus, xhr) {
                        $('#load-waiting').hide();
                        UIkit.notify(data.title, { status: data.type });
                        if (data.type == 'success')
                        {
                            $('#professor-externalarticles-refresh').trigger('click');
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        $('#load-waiting').hide();
                        UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
                    }
                });
            }, {labels: { Cancel: 'انصراف', Ok: 'تائید' }});
        });
    }
});
