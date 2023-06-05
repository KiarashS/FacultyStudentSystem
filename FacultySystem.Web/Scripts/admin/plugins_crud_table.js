function persianNumberToEnglishNumber(value) {
    if (!value) {
        return;
    }
    var englishNumbers = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "0"],
        persianNumbers = ["۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹", "۰"];

    for (var i = 0, numbersLen = persianNumbers.length; i < numbersLen; i++) {
        value = value.replace(new RegExp(persianNumbers[i], "g"), englishNumbers[i]);
    }

    return value;
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}

function replaceBreaksForTextarea(string_to_replace) {
    return $("<div>").append(string_to_replace.replace(/&nbsp;/g, ' ').replace(/<br.*?>/g, '&#13;&#10;')).text();
}

$(function () {
    // crud table
    crud_table.initAddresses();
    crud_table.initUsers();
    crud_table.initEducationalDegree();
    crud_table.initEducationalGroup();
    crud_table.initCollege();
    crud_table.initAcademicRank();
    crud_table.initFreeFields();
    crud_table.initAdministrations();
    crud_table.initResearchs();
    crud_table.initTrainings();
    crud_table.initStudings();
    crud_table.initHonors();
    crud_table.initTheses();
    crud_table.initWorkshops();
    crud_table.initPublications();
    crud_table.initLanguages();
    crud_table.initDefaultFreeFields();
    crud_table.initLessons();
    crud_table.initActivityLogs();
    crud_table.initMemberships();
    crud_table.initGalleries();
    crud_table.initAdminMessages();
    crud_table.initManagerAdminMessages();
    crud_table.initCitationManagement();
    crud_table.initWeeklyPrograms();
    crud_table.initExternalArticles();
    crud_table.initExternalSeminars();
    crud_table.initInternalArticles();
    crud_table.initInternalSeminars();
    crud_table.initNews();
});


crud_table = {
    // #region Address
    initAddresses: function () {

        $('#addresses_crud').jtable({
            title: 'لیست آدرس ها',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'آدرس مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="PostalAddress"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });

            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                AddressId: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                PostalAddress: {
                    title: 'آدرس',
                    width: '50%'

                },
                PostalCode: {
                    title: 'کد پستی',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.PostalCode) {
                            return '<span class="persian-number">' + data.record.PostalCode + '</a>';
                        }
                    }
                },
                Tel: {
                    title: 'تلفن',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Tel) {
                            return '<span class="persian-number">' + data.record.Tel + '</a>';
                        }
                    }
                },
                Fax: {
                    title: 'فکس',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Fax) {
                            return '<span class="persian-number">' + data.record.Fax + '</a>';
                        }
                    }
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    inputTitle: 'لینک (به عنوان مثال لینک آدرس در Google Map)',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Admin Users Management
    initUsers: function () {
        $('#users_crud').jtable({
            tableId: 'users-admin-management',
            title: 'لیست کاربران',
            paging: true, //Enable paging
            pageSize: 10, //Set page size (default: 10)
            pageSizes: [10, 20, 30],
            columnSelectable: true,
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'کاربر مورد نظر حذف شود؟ (توجه فرمائید، در این صورت کاربر قابل بازگشت نمی باشد)';
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });

                //create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);

                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('بله');
                            $this.prop('checked', true);
                            if ($this.attr('name') != 'UserRoles') {
                                $this.attr('value', true);
                            }

                            if ($this.data('professorRole')) {
                                $('input.pageid-input').removeClass('exclude_validation');
                            }
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('خیر');
                            $this.prop('checked', false);
                            if ($this.attr('name') != 'UserRoles') {
                                $this.attr('value', false);
                            }

                            if ($this.data('professorRole')) {
                                $('input.pageid-input').addClass('exclude_validation');
                            }
                        })
                    });

                if (!$('[data-professor-role="true"]').prop('checked')) {
                    $('input.pageid-input').addClass('exclude_validation');
                }

                //reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Firstname"]').attr('data-parsley-required', 'true');
                $('input[name="Lastname"]').attr('data-parsley-required', 'true');
                $('input[name="Email"]').attr('data-parsley-required', 'true');
                $('input[name="Email"]').attr('data-parsley-type', 'email');
                $('input[name="Email"]').attr('data-parsley-remote', $('#variables').data('checkEmailUrl'));
                $('input[name="Email"]').attr('data-parsley-remote-message', 'کاربری با این آدرس ایمل در سامانه موجود می باشد.');
                $('input[name="PageId"]').attr('data-parsley-required', 'true');
                $('input[name="PageId"]').attr('data-parsley-minlength', '5');
                $('input[name="PageId"]').attr('data-parsley-remote-message', 'کاربری با این شناسه در سامانه موجود می باشد.');
                $('input[name="PageId"]').attr('data-parsley-remote', $('#variables').data('checkPageidUrl'));

                if (data.formType == 'create') {
                    $('input[name="Email"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "userid": "0" } }');
                    $('input[name="PageId"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "userid": "0" } }');
                }
                else if (data.formType == 'edit') {
                    $('input[name="Email"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "userid": "' + data.record.UserId + '", "oldEmail": "' + data.record.Email + '"  } }');
                    $('input[name="PageId"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "userid": "' + data.record.UserId + '", "oldPageId": "' + data.record.PageId + '" } }');
                }

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordUpdated: function (event, data) {
                $('#users_crud').jtable('reload');
            },
            //recordAdded: function (event, data) {
            //    $('#users_crud').jtable('reload');
            //},
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            recordsLoaded: function (event, data) {
                //$('.jtable-bottom-panel').addClass('persian-number');
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                var isValid = true;

                $form.parsley().validate();
                if (!$form.parsley().isValid()) {
                    isValid = false;
                }

                var $rolesError = $('#roles-errors');
                if ($rolesError.length) {
                    $rolesError.remove();
                }

                if ($form.find('input[type="checkbox"][name="UserRoles"]:checked').length == 0) {
                    $form.find('.checkboxList').parent('div').parent('div').prepend('<div class="parsley-errors-list filled" id="roles-errors"><span class="parsley-required">انتخاب حداقل یک نقش الزامی می باشد.</span></div>');
                    isValid = false;
                }

                if (!isValid) {
                    return false;
                }

                $rolesError.remove();
            },
            fields: {
                UserId: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Firstname: {
                    title: 'نام',
                    width: '11%',
                    visibility: 'fixed'
                },
                Lastname: {
                    title: 'نام خانوادگی',
                    width: '13%',
                    visibility: 'fixed'
                },
                Fullname: {
                    title: 'نام کامل',
                    create: false,
                    edit: false,
                    list: false
                },
                Email: {
                    title: 'ایمیل',
                    width: '16%',
                    visibility: 'fixed',
                    inputClass: 'english-input',
                    display: function (data) {
                        //console.log(data);
                        return '<div class="font-arial" style="width:100%; height:100%; text-align:center;direction:ltr;"><a href="mailto:' + data.record.Email + '" class="font-arial" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" title="ارسال ایمیل به ' + data.record.Fullname + '">' + data.record.Email + '</a></div>';
                    }
                },
                PageId: {
                    title: 'شناسه پروفایل',
                    width: '10%',
                    inputTitle: 'شناسه پروفایل',
                    inputClass: 'english-input pageid-input',
                    visibility: 'fixed',
                    display: function (data) {
                        if (!data.record.PageId) {
                            return '<div style="width:100%; height:100%; text-align:center;">-</div>';
                        }
                        return '<div class="font-arial" style="width:100%; height:100%; text-align:center;"><a href="/' + data.record.PageId + '" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" title="ورود به صفحه شخصی" target="_blank"><i class="material-icons">account_box</i>' + data.record.PageId + '</a></div>';
                    }
                },
                BannedDate: {
                    create: false,
                    edit: false,
                    list: false
                },
                IsBanned: {
                    defaultValue: false,
                    title: 'مسدود شده؟',
                    width: '4%',
                    //create: false,
                    inputTitle: 'کابر فعلی مسدود شود؟',
                    type: 'checkbox',
                    values: { false: 'خیر', true: 'بله' },
                    display: function (data) {
                        if (!data.record.PageId) {
                            return '<div style="width:100%; height:100%; text-align:center;">-</div>';
                        }

                        if (data.record.IsBanned == true) {
                            return '<div style="width:100%; height:100%; text-align:center;"><i class="material-icons green">done</i></div>';
                        }
                        return '<div style="width:100%; height:100%; text-align:center;"><i class="material-icons red">highlight_off</i></div>';
                    }
                },
                PersianBannedDate: {
                    create: false,
                    edit: false,
                    title: 'زمان مسدود شدن',
                    width: '8%',
                    display: function (data) {
                        if (!data.record.PageId) {
                            return '<div style="width:100%; height:100%; text-align:center;">-</div>';
                        }

                        return '<div class="persian-number" style="width:100%; height:100%; text-align:center;">' + data.record.PersianBannedDate + '</div>';
                    }
                    //listClass: 'persian-number'
                },
                BannedReason: {
                    title: 'دلیل مسدود شدن',
                    width: '14%',
                    //create: false,
                    inputTitle: 'دلیل مسدود شدن کاربر فعلی چیست؟ (اختیاری، جهت نمایش به کاربر)',
                    display: function (data) {
                        if (!data.record.PageId) {
                            return '-';
                        }

                        if (!data.record.BannedReason) {
                            return '<div style="width:100%; height:100%;">-</div>';
                        }
                        return data.record.BannedReason;
                    }
                },
                IsSoftDelete: {
                    defaultValue: false,
                    title: 'حذف منطقی؟',
                    create: false,
                    width: '4%',
                    type: 'checkbox',
                    values: { false: 'خیر', true: 'بله' },
                    inputTitle: 'کابر فعلی حذف منطقی شود؟ (در صورت حذف منطقی قابلیت بازگشت وجود دارد)',
                    visibility: 'hidden',
                    display: function (data) {
                        if (!data.record.PageId) {
                            return '<div style="width:100%; height:100%; text-align:center;">-</div>';
                        }

                        if (data.record.IsSoftDelete == true) {
                            return '<div style="width:100%; height:100%; text-align:center;"><i class="material-icons green">done</i></div>';
                        }
                        return '<div style="width:100%; height:100%; text-align:center;"><i class="material-icons red">highlight_off</i></div>';
                    }
                },
                Roles: {
                    title: 'نقش های کاربر',
                    width: '15%',
                    input: function (data) {
                        var values = [{ displayText: 'Professor', value: '1', isSelected: false, cls: 'uk-text-warning' }, { displayText: 'Admin', value: '2', isSelected: false, cls: 'uk-text-danger' }];
                        if (data.record) {
                            for (item in values) {
                                for (role in data.record.Roles) {
                                    if (values[item].displayText.toLowerCase() == data.record.Roles[role]) {
                                        values[item].isSelected = true;
                                    }
                                }
                            }
                        }
                        //else{ // default value for create form
                        //    values[0].isSelected = true;
                        //}

                        var rolesContainer = '<ul class="checkboxList">';
                        for (item in values) {
                            if (values[item].isSelected === true) {
                                if (values[item].displayText == 'Professor') {
                                    rolesContainer += '<li><label class="font-arial ' + values[item].cls + '"><input type="checkbox" data-professor-role="true" name="UserRoles" value="' + values[item].value + ':' + values[item].displayText.toLowerCase() + '" checked />' + values[item].displayText + '</label></li>';
                                }
                                else {
                                    rolesContainer += '<li><label class="font-arial ' + values[item].cls + '"><input type="checkbox" name="UserRoles" value="' + values[item].value + ':' + values[item].displayText.toLowerCase() + '" checked />' + values[item].displayText + '</label></li>';
                                }
                            }
                            else {
                                if (values[item].displayText == 'Professor') {
                                    rolesContainer += '<li><label class="font-arial ' + values[item].cls + '"><input type="checkbox" data-professor-role="true" name="UserRoles" value="' + values[item].value + ':' + values[item].displayText.toLowerCase() + '" />' + values[item].displayText + '</label></li>';
                                }
                                else {
                                    rolesContainer += '<li><label class="font-arial ' + values[item].cls + '"><input type="checkbox" name="UserRoles" value="' + values[item].value + ':' + values[item].displayText.toLowerCase() + '" />' + values[item].displayText + '</label></li>';
                                }
                            }
                        }
                        rolesContainer += '</ul>';
                        return rolesContainer;

                    },
                    display: function (data) {
                        var rolesContainer = '<div class="tags">';
                        for (role in data.record.Roles) {
                            if (data.record.Roles[role] == 'professor') {
                                rolesContainer += '<a href="javascript:void(0)" "><span class="uk-badge uk-badge-warning" style="padding:5px 9px;font-size:14px;">' + data.record.Roles[role] + '</span></a>';
                            }
                            else {
                                rolesContainer += '<a href="javascript:void(0)" "><span class="uk-badge uk-badge-danger" style="padding:5px 9px;font-size:14px;">' + data.record.Roles[role] + '</span></a>';
                            }
                        }
                        rolesContainer += '</div>';
                        return rolesContainer;
                    }
                },
                ResetPassword: {
                    title: 'ارسال رمز عبور',
                    create: false,
                    edit: false,
                    width: '5%',
                    display: function (data) {
                        return '<a class="md-btn md-btn-primary waves-effect waves-button waves-light" data-reset-password="' + data.record.UserId + '"><i class="material-icons">refresh</i></a>';
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Educational Degree
    initEducationalDegree: function () {

        $('#educationaldegree_crud').jtable({
            title: 'لیست درجه های تحصیلی',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'درجه تحصیلی مورد نظر حذف شود؟';
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Name"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Name"]').attr('data-parsley-remote', $('#variables').data('checkNameUrl'));
                $('input[name="Name"]').attr('data-parsley-remote-message', 'این درجه موجود است.');

                if (data.formType == 'create') {
                    $('input[name="Name"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "id": "0" } }');
                }
                else if (data.formType == 'edit') {
                    $('input[name="Name"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "id": "' + data.record.Id + '" } }');
                }

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Name: {
                    title: 'نام',
                    width: '70%'
                },
                ProfessorCount: {
                    create: false,
                    edit: false,
                    list: true,
                    title: 'تعداد کاربران',
                    width: '10%',
                    display: function (data) {
                        return '<span class="persian-number uk-badge uk-badge-primary uk-badge-notification">' + data.record.ProfessorCount + '</span>';
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '20%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Educational Group
    initEducationalGroup: function () {

        $('#educationalgroup_crud').jtable({
            title: 'لیست گروه های آموزشی',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'گروه آموزشی مورد نظر حذف شود؟';
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Name"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Name"]').attr('data-parsley-remote', $('#variables').data('checkNameUrl'));
                $('input[name="Name"]').attr('data-parsley-remote-message', 'این گروه موجود است.');

                if (data.formType == 'create') {
                    $('input[name="Name"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "id": "0" } }');
                }
                else if (data.formType == 'edit') {
                    $('input[name="Name"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "id": "' + data.record.Id + '" } }');
                }

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Name: {
                    title: 'نام',
                    width: '70%'
                },
                ProfessorCount: {
                    create: false,
                    edit: false,
                    list: true,
                    title: 'تعداد کاربران',
                    width: '10%',
                    display: function (data) {
                        return '<span class="persian-number uk-badge uk-badge-primary uk-badge-notification">' + data.record.ProfessorCount + '</span>';
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '20%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region College
    initCollege: function () {

        $('#college_crud').jtable({
            title: 'لیست دانشکده ها',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'دانشکده مورد نظر حذف شود؟';
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Name"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Name"]').attr('data-parsley-remote', $('#variables').data('checkNameUrl'));
                $('input[name="Name"]').attr('data-parsley-remote-message', 'این دانشکده موجود است.');

                if (data.formType == 'create') {
                    $('input[name="Name"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "id": "0" } }');
                }
                else if (data.formType == 'edit') {
                    $('input[name="Name"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "id": "' + data.record.Id + '" } }');
                }

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Name: {
                    title: 'نام',
                    width: '70%'
                },
                ProfessorCount: {
                    create: false,
                    edit: false,
                    list: true,
                    title: 'تعداد کاربران',
                    width: '10%',
                    display: function (data) {
                        return '<span class="persian-number uk-badge uk-badge-primary uk-badge-notification">' + data.record.ProfessorCount + '</span>';
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '20%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Academic Rank
    initAcademicRank: function () {

        $('#academicrank_crud').jtable({
            title: 'لیست مرتبه های علمی',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'مرتبه علمی مورد نظر حذف شود؟';
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Name"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Name"]').attr('data-parsley-remote', $('#variables').data('checkNameUrl'));
                $('input[name="Name"]').attr('data-parsley-remote-message', 'این مرتبه موجود است.');

                if (data.formType == 'create') {
                    $('input[name="Name"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "id": "0" } }');
                }
                else if (data.formType == 'edit') {
                    $('input[name="Name"]').attr('data-parsley-remote-options', '{ "type": "POST", "data": { "id": "' + data.record.Id + '" } }');
                }

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Name: {
                    title: 'نام',
                    width: '70%'
                },
                ProfessorCount: {
                    create: false,
                    edit: false,
                    list: true,
                    title: 'تعداد کاربران',
                    width: '10%',
                    display: function (data) {
                        return '<span class="persian-number uk-badge uk-badge-primary uk-badge-notification">' + data.record.ProfessorCount + '</span>';
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '20%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Free Fields
    initFreeFields: function () {

        $('#freefields_crud').jtable({
            title: 'لیست فیلدهای آزاد',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'فیلد مورد نظر حذف شود؟';
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Name"]').attr('data-parsley-required', 'true');
                $('input[name="Value"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Name: {
                    title: 'نام',
                    width: '40%'
                },
                Value: {
                    title: 'مقدار',
                    width: '40%'
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '20%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Administration
    initAdministrations: function () {

        $('#administrations_crud').jtable({
            title: 'لیست سوابق اجرایی و مدیریتی',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                if (data.formType == 'edit') {
                    var startTime = persianNumberToEnglishNumber(data.record.StartTime);
                    var endTime = persianNumberToEnglishNumber(data.record.EndTime);
                    $('input[name="StartTime"]').val(startTime);
                    $('input[name="EndTime"]').val(endTime);
                }

                $('input[name="StartTime"]').pDatepicker({
                    format: 'YYYY/MM/DD',
                    autoClose: true,
                });

                $('input[name="EndTime"]').pDatepicker({
                    format: 'YYYY/MM/DD',
                    autoClose: true,
                });

                if (data.formType == 'edit') {
                    var startTime = persianNumberToEnglishNumber(data.record.StartTime);
                    var endTime = persianNumberToEnglishNumber(data.record.EndTime);

                    try {
                        $('input[name="StartTime"]').pDatepicker("setDate", [parseInt(startTime.split('/')[0]), parseInt(startTime.split('/')[1]), parseInt(startTime.split('/')[2]), 12, 0]);
                    }
                    catch (err) { }

                    try {
                        $('input[name="EndTime"]').pDatepicker("setDate", [parseInt(endTime.split('/')[0]), parseInt(endTime.split('/')[1]), parseInt(endTime.split('/')[2]), 12, 0]);
                    }
                    catch (err) { }

                    if (!data.record.StartTime) {
                        $('input[name="StartTime"]').val('');
                    }
                    else if (data.record.StartTime.indexOf('/') == -1) {
                        $('input[name="StartTime"]').val(startTime);
                    }

                    if (!data.record.EndTime) {
                        $('input[name="EndTime"]').val('');
                    }
                    else if (data.record.EndTime.indexOf('/') == -1) {
                        $('input[name="EndTime"]').val(endTime);
                    }
                }
                else {
                    $('input[name="StartTime"]').val('');
                    $('input[name="EndTime"]').val('');
                }

                $('input[name="StartTime"], input[name="EndTime"]').focusout();

                $('input[name="Post"]').attr('data-parsley-required', 'true');
                $('input[name="Place"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Post: {
                    title: 'سمت',
                    width: '30%'

                },
                Place: {
                    title: 'محل',
                    width: '15%'
                },
                StartTime: {
                    title: 'زمان شروع',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.StartTime) {
                            return '<span class="persian-number">' + data.record.StartTime + '</a>';
                        }
                    }
                },
                EndTime: {
                    title: 'زمان پایان',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.EndTime) {
                            return '<span class="persian-number">' + data.record.EndTime + '</a>';
                        }
                    }
                },
                Description: {
                    title: 'توضیحات',
                    width: '16%'
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Researchs
    initResearchs: function () {
        $('#researchs_crud').jtable({
            title: 'لیست سوابق پژوهشی',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                if (data.formType == 'edit') {
                    var startTime = persianNumberToEnglishNumber(data.record.StartTime);
                    var endTime = persianNumberToEnglishNumber(data.record.EndTime);
                    $('input[name="StartTime"]').val(startTime);
                    $('input[name="EndTime"]').val(endTime);
                }

                $('input[name="StartTime"]').pDatepicker({
                    format: 'YYYY/MM/DD',
                    autoClose: true,
                });

                $('input[name="EndTime"]').pDatepicker({
                    format: 'YYYY/MM/DD',
                    autoClose: true,
                });

                if (data.formType == 'edit') {
                    var startTime = persianNumberToEnglishNumber(data.record.StartTime);
                    var endTime = persianNumberToEnglishNumber(data.record.EndTime);

                    try {
                        $('input[name="StartTime"]').pDatepicker("setDate", [parseInt(startTime.split('/')[0]), parseInt(startTime.split('/')[1]), parseInt(startTime.split('/')[2]), 12, 0]);
                    }
                    catch (err) { }

                    try {
                        $('input[name="EndTime"]').pDatepicker("setDate", [parseInt(endTime.split('/')[0]), parseInt(endTime.split('/')[1]), parseInt(endTime.split('/')[2]), 12, 0]);
                    }
                    catch (err) { }

                    if (!data.record.StartTime) {
                        $('input[name="StartTime"]').val('');
                    }
                    else if (data.record.StartTime.indexOf('/') == -1) {
                        $('input[name="StartTime"]').val(startTime);
                    }

                    if (!data.record.EndTime) {
                        $('input[name="EndTime"]').val('');
                    }
                    else if (data.record.EndTime.indexOf('/') == -1) {
                        $('input[name="EndTime"]').val(endTime);
                    }
                }
                else {
                    $('input[name="StartTime"]').val('');
                    $('input[name="EndTime"]').val('');
                }

                $('input[name="StartTime"], input[name="EndTime"]').focusout();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                $('input[name="Place"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان',
                    width: '30%'

                },
                Place: {
                    title: 'محل',
                    width: '15%'
                },
                StartTime: {
                    title: 'زمان شروع',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.StartTime) {
                            return '<span class="persian-number">' + data.record.StartTime + '</a>';
                        }
                    }
                },
                EndTime: {
                    title: 'زمان پایان',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.EndTime) {
                            return '<span class="persian-number">' + data.record.EndTime + '</a>';
                        }
                    }
                },
                Description: {
                    title: 'توضیحات',
                    width: '16%'
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Trainings
    initTrainings: function () {

        $('#trainings_crud').jtable({
            title: 'لیست سوابق آموزشی',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                $('input[name="Place"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Time"]').attr('data-parsley-type', 'number');
                $('input[name="FromTime"]').attr('data-parsley-type', 'number');
                $('input[name="ToTime"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان',
                    width: '35%'

                },
                Place: {
                    title: 'محل',
                    width: '10%'
                },
                Time: {
                    title: 'سال',
                    width: '5%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Time) {
                            return '<span class="persian-number">' + data.record.Time + '</a>';
                        }
                    }
                },
                FromTime: {
                    title: 'از سال',
                    width: '5%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.FromTime) {
                            return '<span class="persian-number">' + data.record.FromTime + '</a>';
                        }
                    }
                },
                ToTime: {
                    title: 'تا سال',
                    width: '5%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.ToTime) {
                            return '<span class="persian-number">' + data.record.ToTime + '</a>';
                        }
                    }
                },
                Teacher: {
                    title: 'مدرس',
                    width: '10%'
                },
                Participant: {
                    title: 'شرکت کننده',
                    width: '10%'
                },
                Secretary: {
                    title: 'دبیر',
                    width: '10%'
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '4%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Studings
    initStudings: function () {

        $('#studings_crud').jtable({
            title: 'لیست سوابق تحصیلی',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Grade"]').attr('data-parsley-required', 'true');
                $('input[name="Field"]').attr('data-parsley-required', 'true');
                //$('input[name="Trend"]').attr('data-parsley-required', 'true');
                $('input[name="University"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="StartTime"]').attr('data-parsley-type', 'number');
                $('input[name="EndTime"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Grade: {
                    title: 'مقطع',
                    width: '5%'
                },
                Field: {
                    title: 'رشته',
                    width: '5%'
                },
                Trend: {
                    title: 'گرایش',
                    width: '5%'
                },
                University: {
                    title: 'دانشگاه',
                    width: '10%'
                },
                ThesisTitle: {
                    title: 'عنوان تز',
                    width: '16%'
                },
                ThesisSupervisors: {
                    title: 'استادان راهنما',
                    width: '10%'
                },
                ThesisAdvisors: {
                    title: 'استادان مشاور',
                    width: '10%'
                },
                StartTime: {
                    title: 'از سال',
                    width: '11%',
                    inputClass: 'english-input',
                    inputTitle: 'سال شروع',
                    display: function (data) {
                        if (data.record.StartTime) {
                            return '<span class="persian-number">' + data.record.StartTime + '</a>';
                        }
                    }
                },
                EndTime: {
                    title: 'تا سال',
                    width: '11%',
                    inputClass: 'english-input',
                    inputTitle: 'سال پایان',
                    display: function (data) {
                        if (data.record.EndTime) {
                            return '<span class="persian-number">' + data.record.EndTime + '</a>';
                        }
                    }
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Honors
    initHonors: function () {

        $('#honors_crud').jtable({
            title: 'لیست جوایز و افتخارات',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                //$('input[name="Place"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Time"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان',
                    width: '61%'

                },
                //Place: {
                //    title: 'محل',
                //    width: '21%'
                //},
                Time: {
                    title: 'سال',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Time) {
                            return '<span class="persian-number">' + data.record.Time + '</a>';
                        }
                    }
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Theses
    initTheses: function () {
        $('#theses_crud').jtable({
            title: 'لیست پایان نامه ها',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                //$('input[name="ThesisPost"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Time"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان',
                    width: '20%'
                },
                ThesisPost: {
                    title: 'نقش شما',
                    width: '10%',
                    options: { '1': '--', '2': 'استاد راهنما', '3': 'استاد مشاور' }
                },
                ThesisGrade: {
                    title: 'دوره',
                    width: '5%',
                    options: {
                        '1': '--', '2': 'کاردانی',
                        '3': 'کارشناسی', '4': 'پزشکی عمومی',
                        '5': 'کارشناسی ارشد', '6': 'دکترا', 
                        '7': 'دکترای تخصصی پزشکی', '8': 'دکترای حرفه ای',
                        '9': 'دکترای فوق تخصصی بالینی', '10': 'دکترای تکمیلی تخصصی (فلوشیپ)', 
                        '11': 'دکترای تخصصی دندانپزشکی', '12': 'دکترای تخصصی (PhD) داروسازی', 
                        '13': 'دکترای تخصصی (PhD)', '14': 'دستیاری تخصصی (علوم پایه پزشکی، داروسازی و دندانپزشکی)',
                        '15': 'دستیاری تخصصی بالینی', '16': 'پسادکترا',
                        '17': 'دوره MPH', '18': 'دانشوری',
                    }
                },
                ThesisType: {
                    title: 'نوع',
                    width: '5%',
                    options: { '1': '--', '2': 'تحقیقاتی', '3': 'کاربردی', '4': 'تحقیقاتی-کاربردی', '5': 'بنیادی', '6': 'توسعه ای' }
                },
                ThesisState: {
                    title: 'وضعیت',
                    width: '5%',
                    options: { '1': 'در حال انجام', '2': 'پایان یافته' }
                },
                Executers: {
                    title: 'مجریان',
                    width: '5%'
                },
                University: {
                    title: 'دانشگاه',
                    width: '11%'
                },
                Time: {
                    title: 'سال',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Time) {
                            return '<span class="persian-number">' + data.record.Time + '</a>';
                        }
                    }
                },
                Description: {
                    title: 'توضیحات',
                    width: '5%'
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Workshops
    initWorkshops: function () {

        $('#workshops_crud').jtable({
            title: 'لیست دوره های آموزشی و کارگاه‌ها',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                if (data.formType == 'edit') {
                    var startTime = persianNumberToEnglishNumber(data.record.StartTime);
                    var endTime = persianNumberToEnglishNumber(data.record.EndTime);
                    $('input[name="StartTime"]').val(startTime);
                    $('input[name="EndTime"]').val(endTime);
                }

                $('input[name="StartTime"]').pDatepicker({
                    format: 'YYYY/MM/DD',
                    autoClose: true,
                });

                $('input[name="EndTime"]').pDatepicker({
                    format: 'YYYY/MM/DD',
                    autoClose: true,
                });

                if (data.formType == 'edit') {
                    var startTime = persianNumberToEnglishNumber(data.record.StartTime);
                    var endTime = persianNumberToEnglishNumber(data.record.EndTime);

                    try {
                        $('input[name="StartTime"]').pDatepicker("setDate", [parseInt(startTime.split('/')[0]), parseInt(startTime.split('/')[1]), parseInt(startTime.split('/')[2]), 12, 0]);
                    }
                    catch (err) { }

                    try {
                        $('input[name="EndTime"]').pDatepicker("setDate", [parseInt(endTime.split('/')[0]), parseInt(endTime.split('/')[1]), parseInt(endTime.split('/')[2]), 12, 0]);
                    }
                    catch (err) { }

                    if (!data.record.StartTime) {
                        $('input[name="StartTime"]').val('');
                    }
                    else if (data.record.StartTime.indexOf('/') == -1) {
                        $('input[name="StartTime"]').val(startTime);
                    }

                    if (!data.record.EndTime) {
                        $('input[name="EndTime"]').val('');
                    }
                    else if (data.record.EndTime.indexOf('/') == -1) {
                        $('input[name="EndTime"]').val(endTime);
                    }
                }
                else {
                    $('input[name="StartTime"]').val('');
                    $('input[name="EndTime"]').val('');
                }

                $('input[name="StartTime"], input[name="EndTime"]').focusout();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                //$('input[name="Host"]').attr('data-parsley-required', 'true');
                //$('input[name="Place"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان دوره',
                    width: '22%'

                },
                Host: {
                    title: 'میزبان',
                    width: '10%'
                },
                Place: {
                    title: 'مکان',
                    width: '10%'
                },
                StartTime: {
                    title: 'زمان شروع',
                    width: '10%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.StartTime) {
                            return '<span class="persian-number">' + data.record.StartTime + '</a>';
                        }
                    }
                },
                EndTime: {
                    title: 'زمان پایان',
                    width: '10%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.EndTime) {
                            return '<span class="persian-number">' + data.record.EndTime + '</a>';
                        }
                    }
                },
                Description: {
                    title: 'توضیحات',
                    width: '10%'
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Publications
    initPublications: function () {

        $('#publications_crud').jtable({
            title: 'لیست تدوین ها و تالیفات',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                $('input[name="Publisher"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Time"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان',
                    width: '30%'

                },
                Publisher: {
                    title: 'انتشارات',
                    width: '15%'
                },
                Time: {
                    title: 'سال',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Time) {
                            return '<span class="persian-number">' + data.record.Time + '</a>';
                        }
                    }
                },
                Description: {
                    title: 'توضیحات',
                    width: '16%'
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Languages
    initLanguages: function () {
        $('#languages_crud').jtable({
            title: 'لیست زبان ها',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'زبان مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        //onChange: function (value) {
                        //    $('select[name="Name"]').val(value);
                        //    $(data.form).parsley().validate();
                        //}
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('select[name="Name"]').attr('data-parsley-min', '2');
                $('select[name="Name"]').attr('data-parsley-min-message', 'لطفاً زبان را انتخاب نمائید.');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input")) {
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
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Name: {
                    title: 'زبان',
                    width: '20%',
                    options: {
                        '1': '--', '2': 'فارسی', '3': 'انگلیسی', '4': 'عربی',
                        '5': 'ترکی', '6': 'هندی', '7': 'ژاپنی', '8': 'کره ای',
                        '9': 'چینی', '10': 'آلمانی', '11': 'فرانسوی', '12': 'ایتالیایی',
                        '13': 'اسپانیایی', '14': 'روسی', '15': 'لهستانی', '16': 'اردو',
                        '17': 'ارمنی', '18': 'آذربایجانی', '19': 'هلندی'
                    }
                },
                Level: {
                    title: 'سطح',
                    width: '20%',
                    options: {
                        '1': '--', '2': 'اصلی', '3': 'مبتدی', '4': 'متوسط',
                        '5': 'خوب', '6': 'عالی'
                    }
                },
                Description: {
                    title: 'توضیحات',
                    width: '43%'
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Default Free Fields
    initDefaultFreeFields: function () {

        $('#defaultfreefields_crud').jtable({
            title: 'فیلدهای آزاد پیش فرض',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'فیلد مورد نظر حذف شود؟';
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('بله');
                            $this.prop('checked', true);
                            $this.attr('value', true);
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('خیر');
                            $this.prop('checked', false);
                            $this.attr('value', false);
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Name"]').attr('data-parsley-required', 'true');
                //$('input[name="Value"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Name: {
                    title: 'نام',
                    width: '40%'
                },
                Value: {
                    title: 'مقدار',
                    width: '40%'
                },
                AddToUsers: {
                    title: 'این فیلد به کاربران فعلی هم اضافه شود؟',
                    width: '10%',
                    list: false,
                    edit: false,
                    defaultValue: false,
                    type: 'checkbox',
                    values: { false: 'خیر', true: 'بله' },
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '10%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Lessons
    initLessons: function () {
        $('#lessons_crud').jtable({
            title: 'لیست دروس ارائه شده',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            //openChildAsAccordion: true,
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'درس مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        //onChange: function (value) {
                        //    $('select[name="Name"]').val(value);
                        //    $(data.form).parsley().validate();
                        //}
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                //$('select[name="Name"]').attr('data-parsley-min', '2');
                //$('select[name="Name"]').attr('data-parsley-min-message', 'لطفاً زبان را انتخاب نمائید.');
                $('input[name="LessonName"]').attr('data-parsley-required', 'true');
                $('input[name="GroupNumber"]').attr('data-parsley-required', 'true');
                $('input[name="GroupNumber"]').attr('data-parsley-type', 'number');
                $('input[name="Field"]').attr('data-parsley-required', 'true');
                $('input[name="Trend"]').attr('data-parsley-required', 'true');
                $('input[name="AcademicYear"]').attr('data-parsley-required', 'true');
                //$('input[name="AcademicYear"]').attr('data-parsley-type', 'number');
                $('input[name="Semester"]').attr('data-parsley-required', 'true');
                $('select[name="LessonGrade"]').attr('data-parsley-min', '2');
                $('select[name="LessonGrade"]').attr('data-parsley-min-message', 'لطفاً دوره درس را انتخاب نمائید.');
                //$('input[name="LessonType"]').attr('data-parsley-min', '2');
                //$('input[name="LessonState"]').attr('data-parsley-min', '2');
                //$('input[name="UnitState"]').attr('data-parsley-min', '2');
                $('input[name="UnitNumber"]').attr('data-parsley-required', 'true');
                $('input[name="UnitNumber"]').attr('data-parsley-type', 'number');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input")) {
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
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                LessonClass: {
                    title: '',
                    width: '4%',
                    edit: false,
                    create: false,
                    listClass: 'child-opener-image-column',
                    display: function (lessonData) {
                        var $img = $('<img class="child-opener-image" src="/build/images/class.png" title="اطلاعات تشکیل کلاس" width="25" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" />');
                        $img.click(function () {
                            $('#lessons_crud').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: lessonData.record.LessonName + ' - اطلاعات تشکیل کلاس',
                                    actions: {
                                        listAction: $('#variables').data('listClassUrl') + '?LessonId=' + lessonData.record.Id,
                                        createAction: $('#variables').data('createClassUrl') + '?LessonId=' + lessonData.record.Id,
                                        updateAction: $('#variables').data('updateClassUrl') + '?LessonId=' + lessonData.record.Id,
                                        deleteAction: $('#variables').data('deleteClassUrl') + '?LessonId=' + lessonData.record.Id
                                    },
                                    fields: {
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        Place: {
                                            title: 'مکان',
                                            width: '20%',
                                            inputTitle: 'مکان (به عنوان مثال: کلاس 203)',
                                            display: function (data) {
                                                if (data.record.Place) {
                                                    return '<span class="persian-number">' + data.record.Place + '</span>';
                                                }
                                            }
                                        },
                                        ClassDay: {
                                            title: 'روز کلاس',
                                            width: '15%',
                                            options: { '0': '--', '1': 'شنبه', '2': 'یکشنبه', '3': 'دوشنبه', '4': 'سه شنبه', '5': 'چهارشنبه', '6': 'پنجشنبه', '7': 'جمعه' }
                                        },
                                        StartHour: {
                                            title: 'ساعت شروع',
                                            width: '10%',
                                            display: function (data) {
                                                if (data.record.StartHour) {
                                                    return '<span class="persian-number">' + data.record.StartHour + '</span>';
                                                }
                                            },
                                            input: function (data) {
                                                if (!data.record || !data.record.StartHour) {
                                                    return '<input class="md-input" type="text" name="StartHour" data-uk-timepicker="" autocomplete="off">';
                                                }
                                                else {
                                                    return '<input class="md-input" type="text" name="StartHour" data-uk-timepicker="" autocomplete="off" value="' + data.record.StartHour + '">';
                                                }
                                            }
                                        },
                                        EndHour: {
                                            title: 'ساعت پایان',
                                            width: '10%',
                                            display: function (data) {
                                                if (data.record.EndHour) {
                                                    return '<span class="persian-number">' + data.record.EndHour + '</span>';
                                                }
                                            },
                                            input: function (data) {
                                                if (!data.record || !data.record.EndHour) {
                                                    return '<input class="md-input" type="text" name="EndHour" data-uk-timepicker="" autocomplete="off">';
                                                }
                                                else {
                                                    return '<input class="md-input" type="text" name="EndHour" data-uk-timepicker="" autocomplete="off" value="' + data.record.EndHour + '">';
                                                }
                                            }
                                        },
                                        Description: {
                                            title: 'توضیحات',
                                            width: '25%',
                                        },
                                        CreateDateText: {
                                            title: 'تاریخ ایجاد',
                                            width: '10%',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.CreateDateText) {
                                                    return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                                                }
                                            }
                                        },
                                        Link: {
                                            title: 'لینک',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Link) {
                                                    return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                                                }
                                            }
                                        },
                                        Order: {
                                            title: 'اولویت قرارگیری',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Order) {
                                                    return '<span class="persian-number">' + data.record.Order + '</span>';
                                                }
                                            }
                                        }
                                    },
                                    formCreated: function (event, data) {
                                        data.form.find('.jtable-option-text-clickable').each(function () {
                                            var $thisTarget = $(this).prev().attr('id');
                                            $(this)
                                                .attr('data-click-target', $thisTarget)
                                                .off('click')
                                                .on('click', function (e) {
                                                    e.preventDefault();
                                                    $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                                                })
                                        });
                                        // create selectize
                                        data.form.find('select').each(function () {
                                            var $this = $(this);
                                            $this.after('<div class="selectize_fix"></div>')
                                            .selectize({
                                                dropdownParent: 'body',
                                                placeholder: 'Click here to select ...',
                                                onDropdownOpen: function ($dropdown) {
                                                    $dropdown
                                                        .hide()
                                                        .velocity('slideDown', {
                                                            begin: function () {
                                                                $dropdown.css({ 'margin-top': '0' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                onDropdownClose: function ($dropdown) {
                                                    $dropdown
                                                        .show()
                                                        .velocity('slideUp', {
                                                            complete: function () {
                                                                $dropdown.css({ 'margin-top': '' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                //onChange: function (value) {
                                                //    $('select[name="Name"]').val(value);
                                                //    $(data.form).parsley().validate();
                                                //}
                                            });
                                        });
                                        // create icheck
                                        data.form
                                            .find('input[type="checkbox"],input[type="radio"]')
                                            .each(function () {
                                                var $this = $(this);
                                                $this.iCheck({
                                                    checkboxClass: 'icheckbox_md',
                                                    radioClass: 'iradio_md',
                                                    increaseArea: '20%'
                                                })
                                                .on('ifChecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('Active');
                                                })
                                                .on('ifUnchecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('Passive');
                                                })
                                            });
                                        // reinitialize inputs
                                        data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                                            $(this).addClass('md-input');
                                            altair_forms.textarea_autosize();
                                        });
                                        altair_md.inputs();

                                        $('.ui-dialog-buttonset')
                                            .children('button')
                                            .attr('class', '')
                                            .addClass('md-btn md-btn-flat')
                                            .off('mouseenter focus');
                                        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');

                                        $('input[name="Place"]').attr('data-parsley-required', 'true');
                                        $('input[name="StartHour"]').attr('data-parsley-required', 'true');
                                        $('input[name="EndHour"]').attr('data-parsley-required', 'true');
                                        $('select[name="ClassDay"]').attr('data-parsley-min', '1');
                                        $('select[name="ClassDay"]').attr('data-parsley-min-message', 'لطفاً روز تشکیل کلاس را انتخاب نمائید.');
                                        $('input[name="Order"]').attr('data-parsley-type', 'number');
                                        $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                                        $(data.form).parsley({
                                            excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                                            trigger: "change focusin blur",
                                            errorsWrapper: '<div class="parsley-errors-list"></div>',
                                            errorTemplate: "<span></span>",
                                            errorClass: "md-input-danger",
                                            successClass: "md-input-success",
                                            errorsContainer: function (e) {
                                                if (e.$element.is("input")) {
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

                                    },
                                    formSubmitting: function (event, data) {
                                        var $form = $(data.form);
                                        $form.parsley().validate();

                                        if (!$form.parsley().isValid()) {
                                            return false;
                                        }
                                    },
                                    //formClosed: function (event, data) {
                                    //},
                                    rowInserted: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    },
                                    rowUpdated: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                    $('.jtable-toolbar').css({ 'backgroundColor': '#7cb342', 'padding': '4px', 'color': '#fff' });
                                    $('.jtable-toolbar-item-icon').addClass('material-icons white').text('add');
                                    $('.jtable-toolbar-item.jtable-toolbar-item-add-record').addClass('mb-n4');
                                });
                        });

                        return $img;
                    }
                },
                LessonPracticeClass: {
                    title: '',
                    width: '4%',
                    edit: false,
                    create: false,
                    listClass: 'child-opener-image-column',
                    display: function (lessonData) {
                        var $img = $('<img class="child-opener-image" src="/build/images/practiceclass.png" title="اطلاعات کلاس حل تمرین" width="25" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" />');
                        $img.click(function () {
                            $('#lessons_crud').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: lessonData.record.LessonName + ' - اطلاعات کلاس حل تمرین',
                                    actions: {
                                        listAction: $('#variables').data('listPracticeclassUrl') + '?LessonId=' + lessonData.record.Id,
                                        createAction: $('#variables').data('createPracticeclassUrl') + '?LessonId=' + lessonData.record.Id,
                                        updateAction: $('#variables').data('updatePracticeclassUrl') + '?LessonId=' + lessonData.record.Id,
                                        deleteAction: $('#variables').data('deletePracticeclassUrl') + '?LessonId=' + lessonData.record.Id
                                    },
                                    fields: {
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        Place: {
                                            title: 'مکان',
                                            width: '20%',
                                            inputTitle: 'مکان (به عنوان مثال: کلاس 203)',
                                            display: function (data) {
                                                if (data.record.Place) {
                                                    return '<span class="persian-number">' + data.record.Place + '</span>';
                                                }
                                            }
                                        },
                                        PracticeClassDay: {
                                            title: 'روز کلاس',
                                            width: '15%',
                                            options: { '0': '--', '1': 'شنبه', '2': 'یکشنبه', '3': 'دوشنبه', '4': 'سه شنبه', '5': 'چهارشنبه', '6': 'پنجشنبه', '7': 'جمعه' }
                                        },
                                        StartHour: {
                                            title: 'ساعت شروع',
                                            width: '7%',
                                            display: function (data) {
                                                if (data.record.StartHour) {
                                                    return '<span class="persian-number">' + data.record.StartHour + '</span>';
                                                }
                                            },
                                            input: function (data) {
                                                if (!data.record || !data.record.StartHour) {
                                                    return '<input class="md-input" type="text" name="StartHour" data-uk-timepicker="" autocomplete="off">';
                                                }
                                                else {
                                                    return '<input class="md-input" type="text" name="StartHour" data-uk-timepicker="" autocomplete="off" value="' + data.record.StartHour + '">';
                                                }
                                            }
                                        },
                                        EndHour: {
                                            title: 'ساعت پایان',
                                            width: '7%',
                                            display: function (data) {
                                                if (data.record.EndHour) {
                                                    return '<span class="persian-number">' + data.record.EndHour + '</span>';
                                                }
                                            },
                                            input: function (data) {
                                                if (!data.record || !data.record.EndHour) {
                                                    return '<input class="md-input" type="text" name="EndHour" data-uk-timepicker="" autocomplete="off">';
                                                }
                                                else {
                                                    return '<input class="md-input" type="text" name="EndHour" data-uk-timepicker="" autocomplete="off" value="' + data.record.EndHour + '">';
                                                }
                                            }
                                        },
                                        TeacherName: {
                                            title: 'مدرس',
                                            width: '11%'
                                        },
                                        Description: {
                                            title: 'توضیحات',
                                            width: '20%',
                                        },
                                        CreateDateText: {
                                            title: 'تاریخ ایجاد',
                                            width: '10%',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.CreateDateText) {
                                                    return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                                                }
                                            }
                                        },
                                        Link: {
                                            title: 'لینک',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Link) {
                                                    return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                                                }
                                            }
                                        },
                                        Order: {
                                            title: 'اولویت قرارگیری',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Order) {
                                                    return '<span class="persian-number">' + data.record.Order + '</span>';
                                                }
                                            }
                                        }
                                    },
                                    formCreated: function (event, data) {
                                        data.form.find('.jtable-option-text-clickable').each(function () {
                                            var $thisTarget = $(this).prev().attr('id');
                                            $(this)
                                                .attr('data-click-target', $thisTarget)
                                                .off('click')
                                                .on('click', function (e) {
                                                    e.preventDefault();
                                                    $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                                                })
                                        });
                                        // create selectize
                                        data.form.find('select').each(function () {
                                            var $this = $(this);
                                            $this.after('<div class="selectize_fix"></div>')
                                            .selectize({
                                                dropdownParent: 'body',
                                                placeholder: 'Click here to select ...',
                                                onDropdownOpen: function ($dropdown) {
                                                    $dropdown
                                                        .hide()
                                                        .velocity('slideDown', {
                                                            begin: function () {
                                                                $dropdown.css({ 'margin-top': '0' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                onDropdownClose: function ($dropdown) {
                                                    $dropdown
                                                        .show()
                                                        .velocity('slideUp', {
                                                            complete: function () {
                                                                $dropdown.css({ 'margin-top': '' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                //onChange: function (value) {
                                                //    $('select[name="Name"]').val(value);
                                                //    $(data.form).parsley().validate();
                                                //}
                                            });
                                        });
                                        // create icheck
                                        data.form
                                            .find('input[type="checkbox"],input[type="radio"]')
                                            .each(function () {
                                                var $this = $(this);
                                                $this.iCheck({
                                                    checkboxClass: 'icheckbox_md',
                                                    radioClass: 'iradio_md',
                                                    increaseArea: '20%'
                                                })
                                                .on('ifChecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('Active');
                                                })
                                                .on('ifUnchecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('Passive');
                                                })
                                            });
                                        // reinitialize inputs
                                        data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                                            $(this).addClass('md-input');
                                            altair_forms.textarea_autosize();
                                        });
                                        altair_md.inputs();

                                        $('.ui-dialog-buttonset')
                                            .children('button')
                                            .attr('class', '')
                                            .addClass('md-btn md-btn-flat')
                                            .off('mouseenter focus');
                                        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');

                                        $('input[name="Place"]').attr('data-parsley-required', 'true');
                                        $('input[name="StartHour"]').attr('data-parsley-required', 'true');
                                        $('input[name="EndHour"]').attr('data-parsley-required', 'true');
                                        $('select[name="PracticeClassDay"]').attr('data-parsley-min', '1');
                                        $('select[name="PracticeClassDay"]').attr('data-parsley-min-message', 'لطفاً روز تشکیل کلاس حل تمرین را انتخاب نمائید.');
                                        $('input[name="Order"]').attr('data-parsley-type', 'number');
                                        $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                                        $(data.form).parsley({
                                            excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                                            trigger: "change focusin blur",
                                            errorsWrapper: '<div class="parsley-errors-list"></div>',
                                            errorTemplate: "<span></span>",
                                            errorClass: "md-input-danger",
                                            successClass: "md-input-success",
                                            errorsContainer: function (e) {
                                                if (e.$element.is("input")) {
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

                                    },
                                    formSubmitting: function (event, data) {
                                        var $form = $(data.form);
                                        $form.parsley().validate();

                                        if (!$form.parsley().isValid()) {
                                            return false;
                                        }
                                    },
                                    //formClosed: function (event, data) {
                                    //},
                                    rowInserted: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    },
                                    rowUpdated: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                    $('.jtable-toolbar').css({ 'backgroundColor': '#7cb342', 'padding': '4px', 'color': '#fff' });
                                    $('.jtable-toolbar-item-icon').addClass('material-icons white').text('add');
                                    $('.jtable-toolbar-item.jtable-toolbar-item-add-record').addClass('mb-n4');
                                });
                        });

                        return $img;
                    }
                },
                ImportantDate: {
                    title: '',
                    width: '4%',
                    edit: false,
                    create: false,
                    listClass: 'child-opener-image-column',
                    display: function (lessonData) {
                        var $img = $('<img class="child-opener-image" src="/build/images/importantdate.png" title="تاریخ های مهم" width="25" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" />');
                        $img.click(function () {
                            $('#lessons_crud').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: lessonData.record.LessonName + ' - تاریخ های مهم',
                                    actions: {
                                        listAction: $('#variables').data('listDateUrl') + '?LessonId=' + lessonData.record.Id,
                                        createAction: $('#variables').data('createDateUrl') + '?LessonId=' + lessonData.record.Id,
                                        updateAction: $('#variables').data('updateDateUrl') + '?LessonId=' + lessonData.record.Id,
                                        deleteAction: $('#variables').data('deleteDateUrl') + '?LessonId=' + lessonData.record.Id
                                    },
                                    fields: {
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        DateDay: {
                                            title: 'روز',
                                            width: '20%',
                                            options: { '0': '--', '1': 'شنبه', '2': 'یکشنبه', '3': 'دوشنبه', '4': 'سه شنبه', '5': 'چهارشنبه', '6': 'پنجشنبه', '7': 'جمعه' }
                                        },
                                        Date: {
                                            title: 'تاریخ',
                                            width: '20%',
                                            display: function (data) {
                                                if (data.record.Date) {
                                                    return '<span class="persian-number">' + data.record.Date + '</span>';
                                                }
                                            },
                                            input: function (data) {
                                                if (!data.record || !data.record.Date) {
                                                    return '<input class="md-input" type="text" readonly="readonly" name="Date" autocomplete="off">';
                                                }
                                                else {
                                                    return '<input class="md-input" type="text" readonly="readonly" name="Date" autocomplete="off" value="' + persianNumberToEnglishNumber(data.record.Date) + '">';
                                                }
                                            }
                                        },
                                        Time: {
                                            title: 'ساعت',
                                            width: '10%',
                                            display: function (data) {
                                                if (data.record.Time) {
                                                    return '<span class="persian-number">' + data.record.Time + '</span>';
                                                }
                                            },
                                            input: function (data) {
                                                if (!data.record || !data.record.Time) {
                                                    return '<input class="md-input" type="text" name="Time" data-uk-timepicker="" autocomplete="off">';
                                                }
                                                else {
                                                    return '<input class="md-input" type="text" name="Time" data-uk-timepicker="" autocomplete="off" value="' + data.record.Time + '">';
                                                }
                                            }
                                        },
                                        Description: {
                                            title: 'توضیحات',
                                            width: '35%',
                                        },
                                        CreateDateText: {
                                            title: 'تاریخ ایجاد',
                                            width: '10%',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.CreateDateText) {
                                                    return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                                                }
                                            }
                                        },
                                        Link: {
                                            title: 'لینک',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Link) {
                                                    return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                                                }
                                            }
                                        },
                                        Order: {
                                            title: 'اولویت قرارگیری',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Order) {
                                                    return '<span class="persian-number">' + data.record.Order + '</span>';
                                                }
                                            }
                                        }
                                    },
                                    formCreated: function (event, data) {
                                        data.form.find('.jtable-option-text-clickable').each(function () {
                                            var $thisTarget = $(this).prev().attr('id');
                                            $(this)
                                                .attr('data-click-target', $thisTarget)
                                                .off('click')
                                                .on('click', function (e) {
                                                    e.preventDefault();
                                                    $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                                                })
                                        });
                                        // create selectize
                                        data.form.find('select').each(function () {
                                            var $this = $(this);
                                            $this.after('<div class="selectize_fix"></div>')
                                            .selectize({
                                                dropdownParent: 'body',
                                                placeholder: 'Click here to select ...',
                                                onDropdownOpen: function ($dropdown) {
                                                    $dropdown
                                                        .hide()
                                                        .velocity('slideDown', {
                                                            begin: function () {
                                                                $dropdown.css({ 'margin-top': '0' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                onDropdownClose: function ($dropdown) {
                                                    $dropdown
                                                        .show()
                                                        .velocity('slideUp', {
                                                            complete: function () {
                                                                $dropdown.css({ 'margin-top': '' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                //onChange: function (value) {
                                                //    $('select[name="Name"]').val(value);
                                                //    $(data.form).parsley().validate();
                                                //}
                                            });
                                        });
                                        // create icheck
                                        data.form
                                            .find('input[type="checkbox"],input[type="radio"]')
                                            .each(function () {
                                                var $this = $(this);
                                                $this.iCheck({
                                                    checkboxClass: 'icheckbox_md',
                                                    radioClass: 'iradio_md',
                                                    increaseArea: '20%'
                                                })
                                                .on('ifChecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('Active');
                                                })
                                                .on('ifUnchecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('Passive');
                                                })
                                            });
                                        // reinitialize inputs
                                        data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                                            $(this).addClass('md-input');
                                            altair_forms.textarea_autosize();
                                        });
                                        altair_md.inputs();

                                        $('.ui-dialog-buttonset')
                                            .children('button')
                                            .attr('class', '')
                                            .addClass('md-btn md-btn-flat')
                                            .off('mouseenter focus');
                                        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');

                                        if (data.formType == 'edit') {
                                            var date = persianNumberToEnglishNumber(data.record.Date);
                                            $('input[name="Date"]').val(date);
                                        }

                                        $('input[name="Date"]').pDatepicker({
                                            format: 'YYYY/MM/DD',
                                            autoClose: true,
                                        });

                                        if (data.formType == 'edit') {
                                            var date = persianNumberToEnglishNumber(data.record.Date);

                                            try {
                                                $('input[name="Date"]').pDatepicker("setDate", [parseInt(date.split('/')[0]), parseInt(date.split('/')[1]), parseInt(date.split('/')[2]), 12, 0]);
                                            }
                                            catch (err) { }

                                            if (!data.record.Date) {
                                                $('input[name="Date"]').val('');
                                            }
                                            else if (data.record.Date.indexOf('/') == -1) {
                                                $('input[name="Date"]').val(date);
                                            }
                                        }
                                        else {
                                            $('input[name="Date"]').val('');
                                        }

                                        $('input[name="Date"]').focusout();

                                        $('input[name="Date"]').attr('data-parsley-required', 'true');
                                        $('input[name="Time"]').attr('data-parsley-required', 'true');
                                        $('select[name="DateDay"]').attr('data-parsley-min', '1');
                                        $('select[name="DateDay"]').attr('data-parsley-min-message', 'لطفاً روز را انتخاب نمائید.');
                                        $('input[name="Order"]').attr('data-parsley-type', 'number');
                                        $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                                        $(data.form).parsley({
                                            excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                                            trigger: "change focusin blur",
                                            errorsWrapper: '<div class="parsley-errors-list"></div>',
                                            errorTemplate: "<span></span>",
                                            errorClass: "md-input-danger",
                                            successClass: "md-input-success",
                                            errorsContainer: function (e) {
                                                if (e.$element.is("input")) {
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

                                    },
                                    formSubmitting: function (event, data) {
                                        var $form = $(data.form);
                                        $form.parsley().validate();

                                        if (!$form.parsley().isValid()) {
                                            return false;
                                        }
                                    },
                                    //formClosed: function (event, data) {
                                    //},
                                    rowInserted: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    },
                                    rowUpdated: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                    $('.jtable-toolbar').css({ 'backgroundColor': '#7cb342', 'padding': '4px', 'color': '#fff' });
                                    $('.jtable-toolbar-item-icon').addClass('material-icons white').text('add');
                                    $('.jtable-toolbar-item.jtable-toolbar-item-add-record').addClass('mb-n4');
                                });
                        });

                        return $img;
                    }
                },
                News: {
                    title: '',
                    width: '4%',
                    edit: false,
                    create: false,
                    listClass: 'child-opener-image-column',
                    display: function (lessonData) {
                        var $img = $('<img class="child-opener-image" src="/build/images/news.png" title="اخبار" width="25" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" />');
                        $img.click(function () {
                            $('#lessons_crud').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: lessonData.record.LessonName + ' - اخبار',
                                    actions: {
                                        listAction: $('#variables').data('listNewsUrl') + '?LessonId=' + lessonData.record.Id,
                                        createAction: $('#variables').data('createNewsUrl') + '?LessonId=' + lessonData.record.Id,
                                        updateAction: $('#variables').data('updateNewsUrl') + '?LessonId=' + lessonData.record.Id,
                                        deleteAction: $('#variables').data('deleteNewsUrl') + '?LessonId=' + lessonData.record.Id
                                    },
                                    fields: {
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        Title: {
                                            title: 'عنوان خبر',
                                            width: '20%',
                                        },
                                        Content: {
                                            title: 'متن خبر',
                                            width: '55%',
                                            input: function (data) {
                                                if (!data.record || !data.record.Content) {
                                                    return '<textarea rows="3" name="Content"></textarea>';
                                                } else if (data.record.Content) {
                                                    return '<textarea rows="3" name="Content">' + data.record.Content + '</textarea>';
                                                }
                                            }
                                        },
                                        NewsColor: {
                                            title: 'رنگ',
                                            inputTitle: 'رنگ (جهت ایجاد تمایز در خبر ها، به عنوان مثال آبی برای اخبار معمولی، قرمز برای اخبار خاص و ضروری)',
                                            width: '5%',
                                            options: { '1': 'آبی', '2': 'سبز', '3': 'طوسی', '4': 'نارنجی', '5': 'قرمز' }
                                        },
                                        CreateDateText: {
                                            title: 'تاریخ ایجاد',
                                            width: '10%',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.CreateDateText) {
                                                    return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                                                }
                                            }
                                        },
                                        Link: {
                                            title: 'لینک',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Link) {
                                                    return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                                                }
                                            }
                                        },
                                        Order: {
                                            title: 'اولویت قرارگیری',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Order) {
                                                    return '<span class="persian-number">' + data.record.Order + '</span>';
                                                }
                                            }
                                        }
                                    },
                                    formCreated: function (event, data) {
                                        data.form.find('.jtable-option-text-clickable').each(function () {
                                            var $thisTarget = $(this).prev().attr('id');
                                            $(this)
                                                .attr('data-click-target', $thisTarget)
                                                .off('click')
                                                .on('click', function (e) {
                                                    e.preventDefault();
                                                    $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                                                })
                                        });
                                        // create selectize
                                        data.form.find('select').each(function () {
                                            var $this = $(this);
                                            $this.after('<div class="selectize_fix"></div>')
                                            .selectize({
                                                dropdownParent: 'body',
                                                placeholder: 'Click here to select ...',
                                                onDropdownOpen: function ($dropdown) {
                                                    $dropdown
                                                        .hide()
                                                        .velocity('slideDown', {
                                                            begin: function () {
                                                                $dropdown.css({ 'margin-top': '0' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                onDropdownClose: function ($dropdown) {
                                                    $dropdown
                                                        .show()
                                                        .velocity('slideUp', {
                                                            complete: function () {
                                                                $dropdown.css({ 'margin-top': '' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                //onChange: function (value) {
                                                //    $('select[name="Name"]').val(value);
                                                //    $(data.form).parsley().validate();
                                                //}
                                            });
                                        });
                                        // create icheck
                                        data.form
                                            .find('input[type="checkbox"],input[type="radio"]')
                                            .each(function () {
                                                var $this = $(this);
                                                $this.iCheck({
                                                    checkboxClass: 'icheckbox_md',
                                                    radioClass: 'iradio_md',
                                                    increaseArea: '20%'
                                                })
                                                .on('ifChecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('Active');
                                                })
                                                .on('ifUnchecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('Passive');
                                                })
                                            });
                                        // reinitialize inputs
                                        data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                                            $(this).addClass('md-input');
                                            altair_forms.textarea_autosize();
                                        });
                                        altair_md.inputs();

                                        $('.ui-dialog-buttonset')
                                            .children('button')
                                            .attr('class', '')
                                            .addClass('md-btn md-btn-flat')
                                            .off('mouseenter focus');
                                        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');

                                        $('input[name="Title"]').attr('data-parsley-required', 'true');
                                        $('input[name="Order"]').attr('data-parsley-type', 'number');
                                        $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                                        $(data.form).parsley({
                                            excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                                            trigger: "change focusin blur",
                                            errorsWrapper: '<div class="parsley-errors-list"></div>',
                                            errorTemplate: "<span></span>",
                                            errorClass: "md-input-danger",
                                            successClass: "md-input-success",
                                            errorsContainer: function (e) {
                                                if (e.$element.is("input")) {
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

                                    },
                                    formSubmitting: function (event, data) {
                                        var $form = $(data.form);
                                        $form.parsley().validate();

                                        if (!$form.parsley().isValid()) {
                                            return false;
                                        }
                                    },
                                    //formClosed: function (event, data) {
                                    //},
                                    rowInserted: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    },
                                    rowUpdated: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                    $('.jtable-toolbar').css({ 'backgroundColor': '#7cb342', 'padding': '4px', 'color': '#fff' });
                                    $('.jtable-toolbar-item-icon').addClass('material-icons white').text('add');
                                    $('.jtable-toolbar-item.jtable-toolbar-item-add-record').addClass('mb-n4');
                                });
                        });

                        return $img;
                    }
                },
                Files: {
                    title: '',
                    width: '4%',
                    edit: false,
                    create: false,
                    listClass: 'child-opener-image-column',
                    display: function (lessonData) {
                        var $img = $('<img class="child-opener-image" src="/build/images/file.png" title="فایل ها" width="25" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" />');
                        $img.click(function () {
                            $('#lessons_crud').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: lessonData.record.LessonName + ' - فایل ها',
                                    actions: {
                                        listAction: $('#variables').data('listFileUrl') + '?LessonId=' + lessonData.record.Id,
                                        createAction: function (data) {
                                            var deferred = $.Deferred();

                                            // Capture form submit result from the hidden iframe
                                            $("#postiframe").load(function () {
                                                iframeContents = $("#postiframe").contents().find("body").text();
                                                var result = $.parseJSON(iframeContents);
                                                deferred.resolve(result);
                                            });

                                            // Submit form with file upload settings
                                            var form = $('#jtable-create-form');
                                            form.unbind('submit');
                                            form.attr('action', $('#variables').data('createFileUrl') + '?LessonId=' + lessonData.record.Id);
                                            form.attr('method', 'post');
                                            form.attr('enctype', 'multipart/form-data');
                                            form.attr('encoding', 'multipart/form-data');
                                            form.attr('target', 'postiframe');
                                            form.submit();

                                            return deferred;
                                        },
                                        updateAction: function () {
                                            var deferred = $.Deferred();

                                            // Capture form submit result from the hidden iframe
                                            $("#postiframe").load(function () {
                                                iframeContents = $("#postiframe").contents().find("body").text();
                                                var result = $.parseJSON(iframeContents);
                                                deferred.resolve(result);
                                            });

                                            // Submit form with file upload settings
                                            var form = $('#jtable-edit-form');
                                            form.unbind('submit');
                                            form.attr('action', $('#variables').data('updateFileUrl') + '?LessonId=' + lessonData.record.Id);
                                            form.attr('method', 'post');
                                            form.attr('enctype', 'multipart/form-data');
                                            form.attr('encoding', 'multipart/form-data');
                                            form.attr('target', 'postiframe');
                                            form.submit();

                                            return deferred;
                                        },
                                        deleteAction: $('#variables').data('deleteFileUrl') + '?LessonId=' + lessonData.record.Id
                                    },
                                    fields: {
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        Title: {
                                            title: 'عنوان',
                                            width: '20%',
                                        },
                                        Description: {
                                            title: 'توضیحات',
                                            width: '25%',
                                        },
                                        FileType: {
                                            title: 'نوع',
                                            width: '15%',
                                            options: {
                                                '1': 'اسلاید', '2': 'کتاب',
                                                '3': 'نمونه سوال', '4': 'نرم افزار',
                                                '5': 'نرم افزار کمک آموزشی', '6': 'ویدئو', 
                                                '7': 'فایل صوتی', '8': 'تصویر',
                                                '9': 'مقاله', '10': 'پایان نامه',
                                                '11': 'استاندارد', '12': 'لینک/سایت',
                                                '13': 'فایل کمکی', '14': 'فایل متنی',
                                                '15': 'فایل راهنما', '16': 'غیره'
                                            }
                                        },
                                        FileInput: {
                                            title: 'فایل',
                                            list: false,
                                            input: function (data) {
                                                return '<div class="uk-form-file md-btn md-btn-primary" style="margin-right:5px;"> انتخاب فایل <input id="file" name="file" type="file"/><iframe name="postiframe" id="postiframe" style="display: none" /> </div>';
                                            }
                                        },
                                        CoverText: {
                                            title: 'کاور',
                                            width: '5%',
                                            listClass: 'uk-text-center',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.CoverText) {
                                                    return '<a href="' + $('#variables').data('getfileUrl') + '?Type=1&FileText=' + data.record.CoverText + '" data-lightbox-type="image" data-uk-lightbox title="کاور"><img data-uk-tooltip title="تصویر کاور" width="50" src="' + $('#variables').data('getfileUrl') + '?Type=1&FileText=' + data.record.CoverText + '" /></a>';
                                                }
                                            }
                                        },
                                        FileText: {
                                            title: 'فایل',
                                            width: '5%',
                                            listClass: 'uk-text-center',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.FileText) {
                                                    return '<a href="' + $('#variables').data('getfileUrl') + '?Type=1&FileText=' + data.record.FileText + '"><i style="cursor:pointer;font-size:22px;" class="material-icons">file_download</i></a>';
                                                }
                                            }
                                        },
                                        DeleteFile: {
                                            title: 'فایل حذف شود؟ (در صورت موجود بودن)',
                                            list: false,
                                            create: false,
                                            defaultValue: false,
                                            type: 'checkbox',
                                            values: { false: 'خیر', true: 'بله' },
                                        },
                                        FileLink: {
                                            title: 'لینک فایل',
                                            inputTitle: 'لینک فایل (در صورت نیاز) [در صورت نیاز به چندین لینک آن ها را با , از هم جدا کنید.]',
                                            width: '10%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.FileLink) {
                                                    var fileLinks = data.record.FileLink.split(','),
                                                        links = '';

                                                    for (var link in fileLinks)
                                                    {
                                                        var validLink = fileLinks[link];
                                                        if (validLink.indexOf('http://') == -1 && validLink.indexOf('https://') == -1 && validLink.indexOf('ftp://') == -1)
                                                        {
                                                            validLink = 'http://' + validLink;
                                                        }
                                                        links +=  '<a href="' + validLink + '" target="_blank" rel="noreferrer"><span class="persian-number">[لینک ' + (parseInt(link, 10) + 1) + ']</span></a>&nbsp;';
                                                    }
                                                    return links;
                                                }
                                            }
                                        },
                                        CoverInput: {
                                            title: 'تصویر کاور فایل',
                                            list: false,
                                            input: function (data) {
                                                return '<div class="uk-form-file md-btn md-btn-primary" style="margin-right:5px;"> انتخاب تصویر کاور <input id="coverfile" name="coverfile" type="file"/> </div>';
                                            }
                                        },
                                        DeleteCover: {
                                            title: 'تصویر کاور فایل حذف شود؟ (در صورت موجود بودن)',
                                            list: false,
                                            create: false,
                                            defaultValue: false,
                                            type: 'checkbox',
                                            values: { false: 'خیر', true: 'بله' },
                                        },
                                        CreateDateText: {
                                            title: 'تاریخ ایجاد',
                                            width: '10%',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.CreateDateText) {
                                                    return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                                                }
                                            }
                                        },
                                        Link: {
                                            title: 'لینک',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Link) {
                                                    return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                                                }
                                            }
                                        },
                                        Order: {
                                            title: 'اولویت قرارگیری',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Order) {
                                                    return '<span class="persian-number">' + data.record.Order + '</span>';
                                                }
                                            }
                                        }
                                    },
                                    recordAdded: function (event, data) {
                                        $img.parents('tr').next('tr.jtable-child-row').find('.jtable-child-table-container').jtable('reload');
                                    },
                                    recordUpdated: function (event, data) {
                                        $img.parents('tr').next('tr.jtable-child-row').find('.jtable-child-table-container').jtable('reload');
                                    },
                                    formCreated: function (event, data) {
                                        //data.form.attr('enctype', 'multipart/form-data');
                                        data.form.find('.jtable-option-text-clickable').each(function () {
                                            var $thisTarget = $(this).prev().attr('id');
                                            $(this)
                                                .attr('data-click-target', $thisTarget)
                                                .off('click')
                                                .on('click', function (e) {
                                                    e.preventDefault();
                                                    $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                                                })
                                        });
                                        // create selectize
                                        data.form.find('select').each(function () {
                                            var $this = $(this);
                                            $this.after('<div class="selectize_fix"></div>')
                                            .selectize({
                                                dropdownParent: 'body',
                                                placeholder: 'Click here to select ...',
                                                onDropdownOpen: function ($dropdown) {
                                                    $dropdown
                                                        .hide()
                                                        .velocity('slideDown', {
                                                            begin: function () {
                                                                $dropdown.css({ 'margin-top': '0' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                onDropdownClose: function ($dropdown) {
                                                    $dropdown
                                                        .show()
                                                        .velocity('slideUp', {
                                                            complete: function () {
                                                                $dropdown.css({ 'margin-top': '' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                //onChange: function (value) {
                                                //    $('select[name="Name"]').val(value);
                                                //    $(data.form).parsley().validate();
                                                //}
                                            });
                                        });
                                        // create icheck
                                        data.form
                                            .find('input[type="checkbox"],input[type="radio"]')
                                            .each(function () {
                                                var $this = $(this);
                                                $this.iCheck({
                                                    checkboxClass: 'icheckbox_md',
                                                    radioClass: 'iradio_md',
                                                    increaseArea: '20%'
                                                })
                                                .on('ifChecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('بله');
                                                    $this.prop('checked', true);
                                                    $this.attr('value', true);
                                                })
                                                .on('ifUnchecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('خیر');
                                                    $this.prop('checked', false);
                                                    $this.attr('value', false);
                                                })
                                            });
                                        // reinitialize inputs
                                        data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                                            $(this).addClass('md-input');
                                            altair_forms.textarea_autosize();
                                        });
                                        altair_md.inputs();

                                        $('.ui-dialog-buttonset')
                                            .children('button')
                                            .attr('class', '')
                                            .addClass('md-btn md-btn-flat')
                                            .off('mouseenter focus');
                                        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');

                                        $('input[name="Title"]').attr('data-parsley-required', 'true');
                                        //$('select[name="FileType"]').attr('data-parsley-min', '1');
                                        //$('select[name="FileType"]').attr('data-parsley-min-message', 'لطفاً نوع فایل را انتخاب نمائید.');
                                        $('input[name="FileLink"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');
                                        $('input[name="Order"]').attr('data-parsley-type', 'number');
                                        $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                                        $(data.form).parsley({
                                            excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                                            trigger: "change focusin blur",
                                            errorsWrapper: '<div class="parsley-errors-list"></div>',
                                            errorTemplate: "<span></span>",
                                            errorClass: "md-input-danger",
                                            successClass: "md-input-success",
                                            errorsContainer: function (e) {
                                                if (e.$element.is("input")) {
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

                                    },
                                    formSubmitting: function (event, data) {
                                        var $form = $(data.form);
                                        $form.parsley().validate();

                                        if (!$form.parsley().isValid()) {
                                            return false;
                                        }

                                        //var filename = $('input[type="file"]').val().split('\\').pop();
                                        //($("#" + data.form.attr("id")).find('input[name="File"]').val(filename));
                                    },
                                    //formClosed: function (event, data) {
                                    //},
                                    rowInserted: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    },
                                    rowUpdated: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                    $('.jtable-toolbar').css({ 'backgroundColor': '#7cb342', 'padding': '4px', 'color': '#fff' });
                                    $('.jtable-toolbar-item-icon').addClass('material-icons white').text('add');
                                    $('.jtable-toolbar-item.jtable-toolbar-item-add-record').addClass('mb-n4');
                                });
                        });

                        return $img;
                    }
                },
                Practices: {
                    title: '',
                    width: '4%',
                    edit: false,
                    create: false,
                    listClass: 'child-opener-image-column',
                    display: function (lessonData) {
                        var $img = $('<img class="child-opener-image" src="/build/images/practice.png" title="تمرین ها" width="25" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" />');
                        $img.click(function () {
                            $('#lessons_crud').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: lessonData.record.LessonName + ' - تمرین ها',
                                    actions: {
                                        listAction: $('#variables').data('listPracticeUrl') + '?LessonId=' + lessonData.record.Id,
                                        createAction: function (data) {
                                            var deferred = $.Deferred();

                                            // Capture form submit result from the hidden iframe
                                            $("#postiframe").load(function () {
                                                iframeContents = $("#postiframe").contents().find("body").text();
                                                var result = $.parseJSON(iframeContents);
                                                deferred.resolve(result);
                                            });

                                            // Submit form with file upload settings
                                            var form = $('#jtable-create-form');
                                            form.unbind('submit');
                                            form.attr('action', $('#variables').data('createPracticeUrl') + '?LessonId=' + lessonData.record.Id);
                                            form.attr('method', 'post');
                                            form.attr('enctype', 'multipart/form-data');
                                            form.attr('encoding', 'multipart/form-data');
                                            form.attr('target', 'postiframe');
                                            form.submit();

                                            return deferred;
                                        },
                                        updateAction: function () {
                                            var deferred = $.Deferred();

                                            // Capture form submit result from the hidden iframe
                                            $("#postiframe").load(function () {
                                                iframeContents = $("#postiframe").contents().find("body").text();
                                                var result = $.parseJSON(iframeContents);
                                                deferred.resolve(result);
                                            });

                                            // Submit form with file upload settings
                                            var form = $('#jtable-edit-form');
                                            form.unbind('submit');
                                            form.attr('action', $('#variables').data('updatePracticeUrl') + '?LessonId=' + lessonData.record.Id);
                                            form.attr('method', 'post');
                                            form.attr('enctype', 'multipart/form-data');
                                            form.attr('encoding', 'multipart/form-data');
                                            form.attr('target', 'postiframe');
                                            form.submit();

                                            return deferred;
                                        },
                                        deleteAction: $('#variables').data('deletePracticeUrl') + '?LessonId=' + lessonData.record.Id
                                    },
                                    fields: {
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        Title: {
                                            title: 'عنوان',
                                            width: '25%',
                                        },
                                        Description: {
                                            title: 'توضیحات',
                                            width: '25%',
                                        },
                                        FileInput: {
                                            title: 'فایل تمرین',
                                            list: false,
                                            input: function (data) {
                                                return '<div class="uk-form-file md-btn md-btn-primary" style="margin-right:5px;"> انتخاب فایل <input id="file" name="file" type="file"/><iframe name="postiframe" id="postiframe" style="display: none" /> </div>';
                                            }
                                        },
                                        FileText: {
                                            title: 'فایل',
                                            width: '5%',
                                            listClass: 'uk-text-center',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.FileText) {
                                                    return '<a href="' + $('#variables').data('getfileUrl') + '?Type=2&FileText=' + data.record.FileText + '"><i style="cursor:pointer;font-size:22px;" class="material-icons">file_download</i></a>';
                                                }
                                            }
                                        },
                                        DeleteFile: {
                                            title: 'فایل حذف شود؟ (در صورت موجود بودن)',
                                            list: false,
                                            create: false,
                                            defaultValue: false,
                                            type: 'checkbox',
                                            values: { false: 'خیر', true: 'بله' },
                                        },
                                        FileLink: {
                                            title: 'لینک فایل',
                                            inputTitle: 'لینک فایل (در صورت نیاز) [در صورت نیاز به چندین لینک آن ها را با , از هم جدا کنید.]',
                                            width: '10%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.FileLink) {
                                                    var fileLinks = data.record.FileLink.split(','),
                                                        links = '';

                                                    for (var link in fileLinks)
                                                    {
                                                        var validLink = fileLinks[link];
                                                        if (validLink.indexOf('http://') == -1 && validLink.indexOf('https://') == -1 && validLink.indexOf('ftp://') == -1)
                                                        {
                                                            validLink = 'http://' + validLink;
                                                        }
                                                        links +=  '<a href="' + validLink + '" target="_blank" rel="noreferrer"><span class="persian-number">[لینک ' + (parseInt(link, 10) + 1) + ']</span></a>&nbsp;';
                                                    }
                                                    return links;
                                                }
                                            }
                                        },
                                        DeliverDate: {
                                            title: 'تاریخ تحویل',
                                            width: '10%',
                                            display: function (data) {
                                                if (data.record.DeliverDate) {
                                                    return '<span class="persian-number">' + data.record.DeliverDate + '</span>';
                                                }
                                            },
                                            input: function (data) {
                                                if (!data.record || !data.record.DeliverDate) {
                                                    return '<input class="md-input" type="text" readonly="readonly" name="DeliverDate" autocomplete="off">';
                                                }
                                                else {
                                                    return '<input class="md-input" type="text" readonly="readonly" name="DeliverDate" autocomplete="off" value="' + persianNumberToEnglishNumber(data.record.c) + '">';
                                                }
                                            }
                                        },
                                        CreateDateText: {
                                            title: 'تاریخ ایجاد',
                                            width: '10%',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.CreateDateText) {
                                                    return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                                                }
                                            }
                                        },
                                        Link: {
                                            title: 'لینک',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Link) {
                                                    return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                                                }
                                            }
                                        },
                                        Order: {
                                            title: 'اولویت قرارگیری',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Order) {
                                                    return '<span class="persian-number">' + data.record.Order + '</span>';
                                                }
                                            }
                                        }
                                    },
                                    recordAdded: function (event, data) {
                                        $img.parents('tr').next('tr.jtable-child-row').find('.jtable-child-table-container').jtable('reload');
                                    },
                                    recordUpdated: function (event, data) {
                                        $img.parents('tr').next('tr.jtable-child-row').find('.jtable-child-table-container').jtable('reload');
                                    },
                                    formCreated: function (event, data) {
                                        //data.form.attr('enctype', 'multipart/form-data');
                                        data.form.find('.jtable-option-text-clickable').each(function () {
                                            var $thisTarget = $(this).prev().attr('id');
                                            $(this)
                                                .attr('data-click-target', $thisTarget)
                                                .off('click')
                                                .on('click', function (e) {
                                                    e.preventDefault();
                                                    $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                                                })
                                        });
                                        // create selectize
                                        data.form.find('select').each(function () {
                                            var $this = $(this);
                                            $this.after('<div class="selectize_fix"></div>')
                                            .selectize({
                                                dropdownParent: 'body',
                                                placeholder: 'Click here to select ...',
                                                onDropdownOpen: function ($dropdown) {
                                                    $dropdown
                                                        .hide()
                                                        .velocity('slideDown', {
                                                            begin: function () {
                                                                $dropdown.css({ 'margin-top': '0' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                onDropdownClose: function ($dropdown) {
                                                    $dropdown
                                                        .show()
                                                        .velocity('slideUp', {
                                                            complete: function () {
                                                                $dropdown.css({ 'margin-top': '' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                //onChange: function (value) {
                                                //    $('select[name="Name"]').val(value);
                                                //    $(data.form).parsley().validate();
                                                //}
                                            });
                                        });
                                        // create icheck
                                        data.form
                                            .find('input[type="checkbox"],input[type="radio"]')
                                            .each(function () {
                                                var $this = $(this);
                                                $this.iCheck({
                                                    checkboxClass: 'icheckbox_md',
                                                    radioClass: 'iradio_md',
                                                    increaseArea: '20%'
                                                })
                                                .on('ifChecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('بله');
                                                    $this.prop('checked', true);
                                                    $this.attr('value', true);
                                                })
                                                .on('ifUnchecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('خیر');
                                                    $this.prop('checked', false);
                                                    $this.attr('value', false);
                                                })
                                            });
                                        // reinitialize inputs
                                        data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                                            $(this).addClass('md-input');
                                            altair_forms.textarea_autosize();
                                        });
                                        altair_md.inputs();

                                        $('.ui-dialog-buttonset')
                                            .children('button')
                                            .attr('class', '')
                                            .addClass('md-btn md-btn-flat')
                                            .off('mouseenter focus');
                                        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');

                                        if (data.formType == 'edit') {
                                            var date = persianNumberToEnglishNumber(data.record.DeliverDate);
                                            $('input[name="DeliverDate"]').val(date);
                                        }

                                        $('input[name="DeliverDate"]').pDatepicker({
                                            format: 'YYYY/MM/DD',
                                            autoClose: true,
                                        });

                                        if (data.formType == 'edit') {
                                            var date = persianNumberToEnglishNumber(data.record.DeliverDate);

                                            try {
                                                $('input[name="DeliverDate"]').pDatepicker("setDate", [parseInt(date.split('/')[0]), parseInt(date.split('/')[1]), parseInt(date.split('/')[2]), 12, 0]);
                                            }
                                            catch (err) { }

                                            if (!data.record.DeliverDate) {
                                                $('input[name="DeliverDate"]').val('');
                                            }
                                            else if (data.record.DeliverDate.indexOf('/') == -1) {
                                                $('input[name="DeliverDate"]').val(date);
                                            }
                                        }
                                        else {
                                            $('input[name="DeliverDate"]').val('');
                                        }

                                        $('input[name="DeliverDate"]').focusout();

                                        $('input[name="Title"]').attr('data-parsley-required', 'true');
                                        //$('select[name="FileType"]').attr('data-parsley-min', '1');
                                        //$('select[name="FileType"]').attr('data-parsley-min-message', 'لطفاً نوع فایل را انتخاب نمائید.');
                                        $('input[name="FileLink"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');
                                        $('input[name="Order"]').attr('data-parsley-type', 'number');
                                        $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                                        $(data.form).parsley({
                                            excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                                            trigger: "change focusin blur",
                                            errorsWrapper: '<div class="parsley-errors-list"></div>',
                                            errorTemplate: "<span></span>",
                                            errorClass: "md-input-danger",
                                            successClass: "md-input-success",
                                            errorsContainer: function (e) {
                                                if (e.$element.is("input")) {
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

                                    },
                                    formSubmitting: function (event, data) {
                                        var $form = $(data.form);
                                        $form.parsley().validate();

                                        if (!$form.parsley().isValid()) {
                                            return false;
                                        }
                                    },
                                    rowInserted: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    },
                                    rowUpdated: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                    $('.jtable-toolbar').css({ 'backgroundColor': '#7cb342', 'padding': '4px', 'color': '#fff' });
                                    $('.jtable-toolbar-item-icon').addClass('material-icons white').text('add');
                                    $('.jtable-toolbar-item.jtable-toolbar-item-add-record').addClass('mb-n4');
                                });
                        });

                        return $img;
                    }
                },
                Scores: {
                    title: '',
                    width: '4%',
                    edit: false,
                    create: false,
                    listClass: 'child-opener-image-column',
                    display: function (lessonData) {
                        var $img = $('<img class="child-opener-image" src="/build/images/score.png" title="نمرات" width="25" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" />');
                        $img.click(function () {
                            $('#lessons_crud').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: lessonData.record.LessonName + ' - نمرات',
                                    actions: {
                                        listAction: $('#variables').data('listScoreUrl') + '?LessonId=' + lessonData.record.Id,
                                        createAction: function (data) {
                                            var deferred = $.Deferred();

                                            // Capture form submit result from the hidden iframe
                                            $("#postiframe").load(function () {
                                                iframeContents = $("#postiframe").contents().find("body").text();
                                                var result = $.parseJSON(iframeContents);
                                                deferred.resolve(result);
                                            });

                                            // Submit form with file upload settings
                                            var form = $('#jtable-create-form');
                                            form.unbind('submit');
                                            form.attr('action', $('#variables').data('createScoreUrl') + '?LessonId=' + lessonData.record.Id);
                                            form.attr('method', 'post');
                                            form.attr('enctype', 'multipart/form-data');
                                            form.attr('encoding', 'multipart/form-data');
                                            form.attr('target', 'postiframe');
                                            form.submit();

                                            return deferred;
                                        },
                                        updateAction: function () {
                                            var deferred = $.Deferred();

                                            // Capture form submit result from the hidden iframe
                                            $("#postiframe").load(function () {
                                                iframeContents = $("#postiframe").contents().find("body").text();
                                                var result = $.parseJSON(iframeContents);
                                                deferred.resolve(result);
                                            });

                                            // Submit form with file upload settings
                                            var form = $('#jtable-edit-form');
                                            form.unbind('submit');
                                            form.attr('action', $('#variables').data('updateScoreUrl') + '?LessonId=' + lessonData.record.Id);
                                            form.attr('method', 'post');
                                            form.attr('enctype', 'multipart/form-data');
                                            form.attr('encoding', 'multipart/form-data');
                                            form.attr('target', 'postiframe');
                                            form.submit();

                                            return deferred;
                                        },
                                        deleteAction: $('#variables').data('deleteScoreUrl') + '?LessonId=' + lessonData.record.Id
                                    },
                                    fields: {
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        Title: {
                                            title: 'عنوان',
                                            width: '25%',
                                        },
                                        Description: {
                                            title: 'توضیحات',
                                            width: '25%',
                                        },
                                        FileInput: {
                                            title: 'فایل نمره ها',
                                            list: false,
                                            input: function (data) {
                                                return '<div class="uk-form-file md-btn md-btn-primary" style="margin-right:5px;"> انتخاب فایل <input id="file" name="file" type="file"/><iframe name="postiframe" id="postiframe" style="display: none" /> </div>';
                                            }
                                        },
                                        FileText: {
                                            title: 'فایل',
                                            width: '5%',
                                            listClass: 'uk-text-center',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.FileText) {
                                                    return '<a href="' + $('#variables').data('getfileUrl') + '?Type=3&FileText=' + data.record.FileText + '"><i style="cursor:pointer;font-size:22px;" class="material-icons">file_download</i></a>';
                                                }
                                            }
                                        },
                                        DeleteFile: {
                                            title: 'فایل حذف شود؟ (در صورت موجود بودن)',
                                            list: false,
                                            create: false,
                                            defaultValue: false,
                                            type: 'checkbox',
                                            values: { false: 'خیر', true: 'بله' },
                                        },
                                        FileLink: {
                                            title: 'لینک فایل',
                                            inputTitle: 'لینک فایل (در صورت نیاز) [در صورت نیاز به چندین لینک آن ها را با , از هم جدا کنید.]',
                                            width: '10%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.FileLink) {
                                                    var fileLinks = data.record.FileLink.split(','),
                                                        links = '';

                                                    for (var link in fileLinks)
                                                    {
                                                        var validLink = fileLinks[link];
                                                        if (validLink.indexOf('http://') == -1 && validLink.indexOf('https://') == -1 && validLink.indexOf('ftp://') == -1)
                                                        {
                                                            validLink = 'http://' + validLink;
                                                        }
                                                        links +=  '<a href="' + validLink + '" target="_blank" rel="noreferrer"><span class="persian-number">[لینک ' + (parseInt(link, 10) + 1) + ']</span></a>&nbsp;';
                                                    }
                                                    return links;
                                                }
                                            }
                                        },
                                        CreateDateText: {
                                            title: 'تاریخ ایجاد',
                                            width: '10%',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.CreateDateText) {
                                                    return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                                                }
                                            }
                                        },
                                        Link: {
                                            title: 'لینک',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Link) {
                                                    return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                                                }
                                            }
                                        },
                                        Order: {
                                            title: 'اولویت قرارگیری',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Order) {
                                                    return '<span class="persian-number">' + data.record.Order + '</span>';
                                                }
                                            }
                                        }
                                    },
                                    recordAdded: function (event, data) {
                                        $img.parents('tr').next('tr.jtable-child-row').find('.jtable-child-table-container').jtable('reload');
                                    },
                                    recordUpdated: function (event, data) {
                                        $img.parents('tr').next('tr.jtable-child-row').find('.jtable-child-table-container').jtable('reload');
                                    },
                                    formCreated: function (event, data) {
                                        //data.form.attr('enctype', 'multipart/form-data');
                                        data.form.find('.jtable-option-text-clickable').each(function () {
                                            var $thisTarget = $(this).prev().attr('id');
                                            $(this)
                                                .attr('data-click-target', $thisTarget)
                                                .off('click')
                                                .on('click', function (e) {
                                                    e.preventDefault();
                                                    $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                                                })
                                        });
                                        // create selectize
                                        data.form.find('select').each(function () {
                                            var $this = $(this);
                                            $this.after('<div class="selectize_fix"></div>')
                                            .selectize({
                                                dropdownParent: 'body',
                                                placeholder: 'Click here to select ...',
                                                onDropdownOpen: function ($dropdown) {
                                                    $dropdown
                                                        .hide()
                                                        .velocity('slideDown', {
                                                            begin: function () {
                                                                $dropdown.css({ 'margin-top': '0' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                onDropdownClose: function ($dropdown) {
                                                    $dropdown
                                                        .show()
                                                        .velocity('slideUp', {
                                                            complete: function () {
                                                                $dropdown.css({ 'margin-top': '' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                //onChange: function (value) {
                                                //    $('select[name="Name"]').val(value);
                                                //    $(data.form).parsley().validate();
                                                //}
                                            });
                                        });
                                        // create icheck
                                        data.form
                                            .find('input[type="checkbox"],input[type="radio"]')
                                            .each(function () {
                                                var $this = $(this);
                                                $this.iCheck({
                                                    checkboxClass: 'icheckbox_md',
                                                    radioClass: 'iradio_md',
                                                    increaseArea: '20%'
                                                })
                                                .on('ifChecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('بله');
                                                    $this.prop('checked', true);
                                                    $this.attr('value', true);
                                                })
                                                .on('ifUnchecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('خیر');
                                                    $this.prop('checked', false);
                                                    $this.attr('value', false);
                                                })
                                            });
                                        // reinitialize inputs
                                        data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                                            $(this).addClass('md-input');
                                            altair_forms.textarea_autosize();
                                        });
                                        altair_md.inputs();

                                        $('.ui-dialog-buttonset')
                                            .children('button')
                                            .attr('class', '')
                                            .addClass('md-btn md-btn-flat')
                                            .off('mouseenter focus');
                                        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');

                                        $('input[name="Title"]').attr('data-parsley-required', 'true');
                                        //$('select[name="FileType"]').attr('data-parsley-min', '1');
                                        //$('select[name="FileType"]').attr('data-parsley-min-message', 'لطفاً نوع فایل را انتخاب نمائید.');
                                        $('input[name="FileLink"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');
                                        $('input[name="Order"]').attr('data-parsley-type', 'number');
                                        $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                                        $(data.form).parsley({
                                            excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                                            trigger: "change focusin blur",
                                            errorsWrapper: '<div class="parsley-errors-list"></div>',
                                            errorTemplate: "<span></span>",
                                            errorClass: "md-input-danger",
                                            successClass: "md-input-success",
                                            errorsContainer: function (e) {
                                                if (e.$element.is("input")) {
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

                                    },
                                    formSubmitting: function (event, data) {
                                        var $form = $(data.form);
                                        $form.parsley().validate();

                                        if (!$form.parsley().isValid()) {
                                            return false;
                                        }
                                    },
                                    rowInserted: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    },
                                    rowUpdated: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                    $('.jtable-toolbar').css({ 'backgroundColor': '#7cb342', 'padding': '4px', 'color': '#fff' });
                                    $('.jtable-toolbar-item-icon').addClass('material-icons white').text('add');
                                    $('.jtable-toolbar-item.jtable-toolbar-item-add-record').addClass('mb-n4');
                                });
                        });

                        return $img;
                    }
                },
                LessonName: {
                    title: 'نام درس',
                    width: '8%'
                },
                LessonCode: {
                    title: 'شماره درس',
                    width: '5%',
                    display: function (data) {
                        if (data.record.LessonCode) {
                            return '<span class="persian-number">' + data.record.LessonCode + '</span>';
                        }
                    }
                },
                GroupNumber: {
                    title: 'شماره گروه',
                    width: '2%',
                    display: function (data) {
                        if (data.record.GroupNumber) {
                            return '<span class="persian-number">' + data.record.GroupNumber + '</span>';
                        }
                    }
                },
                Description: {
                    title: 'توضیحات',
                    list: false,
                    input: function (data) {
                        if (!data.record || !data.record.Description) {
                            return '<textarea rows="2" name="Description"></textarea>';
                        } else if (data.record.Description) {
                            return '<textarea rows="2" name="Description">' + data.record.Description + '</textarea>';
                        }
                    }
                },
                Reference: {
                    title: 'مراجع',
                    list: false,
                    input: function (data) {
                        if (!data.record || !data.record.Reference) {
                            return '<textarea rows="2" name="Reference"></textarea>';
                        } else if (data.record.Reference) {
                            return '<textarea rows="2" name="Reference">' + data.record.Reference + '</textarea>';
                        }
                    }
                },
                ScoringDescription: {
                    title: 'توضیحات شیوه نمره دهی',
                    list: false,
                    input: function (data) {
                        if (!data.record || !data.record.ScoringDescription) {
                            return '<textarea rows="2" name="ScoringDescription"></textarea>';
                        } else if (data.record.ScoringDescription) {
                            return '<textarea rows="2" name="ScoringDescription">' + data.record.ScoringDescription + '</textarea>';
                        }
                    }
                },
                ProjectDescription: {
                    title: 'توضیحات پروژه',
                    list: false,
                    input: function (data) {
                        if (!data.record || !data.record.ProjectDescription) {
                            return '<textarea rows="2" name="ProjectDescription"></textarea>';
                        } else if (data.record.ProjectDescription) {
                            return '<textarea rows="2" name="ProjectDescription">' + data.record.ProjectDescription + '</textarea>';
                        }
                    }
                },
                LessonGrade: {
                    title: 'دوره',
                    width: '5%',
                    options: {
                        '1': '--', '2': 'کاردانی',
                        '3': 'کارشناسی', '4': 'پزشکی عمومی',
                        '5': 'کارشناسی ارشد', '6': 'دکترا',
                        '7': 'دکترای تخصصی پزشکی', '8': 'دکترای حرفه ای',
                        '9': 'دکترای فوق تخصصی بالینی', '10': 'دکترای تکمیلی تخصصی (فلوشیپ)',
                        '11': 'دکترای تخصصی دندانپزشکی', '12': 'دکترای تخصصی (PhD) داروسازی',
                        '13': 'دکترای تخصصی (PhD)', '14': 'دستیاری تخصصی (علوم پایه پزشکی، داروسازی و دندانپزشکی)',
                        '15': 'دستیاری تخصصی بالینی', '16': 'پسادکترا',
                        '17': 'دوره MPH', '18': 'دانشوری',
                    }
                },
                Field: {
                    title: 'رشته',
                    width: '5%',
                },
                Trend: {
                    title: 'گرایش',
                    width: '5%',
                },
                AcademicYear: {
                    title: 'سال تحصیلی',
                    inputTitle: 'سال تحصیلی (مثال: 97-96)',
                    width: '5%',
                    display: function (data) {
                        if (data.record.AcademicYear) {
                            return '<span class="persian-number">' + data.record.AcademicYear + '</span>';
                        }
                    }
                },
                Semester: {
                    title: 'ترم تحصیلی',
                    width: '5%',
                },
                LessonType: {
                    title: 'نوع درس',
                    width: '5%',
                    list: false,
                    options: {
                        '1': '--', '2': 'تخصصی', '3': 'عمومی'
                    }
                },
                LessonState: {
                    title: 'وضعیت درس',
                    width: '5%',
                    list: false,
                    options: {
                        '1': '--', '2': 'اجباری', '3': 'اختیاری'
                    }
                },
                UnitState: {
                    title: 'وضعیت واحد',
                    width: '5%',
                    list: false,
                    options: {
                        '1': '--', '2': 'نظری', '3': 'عملی'
                    }
                },
                UnitNumber: {
                    title: 'تعداد واحد',
                    width: '5%',
                    list: false,
                    display: function (data) {
                        if (data.record.UnitNumber) {
                            return '<span class="persian-number">' + data.record.UnitNumber + '</span>';
                        }
                    }
                },
                CreateDateText: {
                    title: 'تاریخ ایجاد',
                    width: '5%',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (data.record.CreateDateText) {
                            return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                        }
                    }
                },
                Link: {
                    title: 'لینک',
                    width: '5%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '5%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Activity Logs
    initActivityLogs: function () {

        $('#activitylogs_crud').jtable({
            title: 'لاگ فعالیت ها',
            paging: true, //Enable paging
            pageSize: 50, //Set page size (default: 10)
            addRecordButton: null,
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'فیلد مورد نظر حذف شود؟';
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                //createAction: $('#variables').data('createUrl'),
                //updateAction: $('#variables').data('updateUrl'),
                //deleteAction: $('#variables').data('deleteUrl')
            },
            rowInserted: function (event, data) {
                data.row.find('.jtable-edit-command-button').hide();
                data.row.find('.jtable-delete-command-button').hide();
            },
            //recordsLoaded: function (event, data) {
            //    $('body').trigger('display.uk.check');
            //},
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                ActionType: {
                    title: 'نوع',
                    width: '10%'
                },
                ActionBy: {
                    title: 'توسط',
                    width: '15%',
                    //display: function (data) {
                    //    if (!data.record.ActionBy)
                    //    {
                    //        return '<span>کاربر مهمان</span>';
                    //    }
                    //    else
                    //    {
                    //        return '<span>' + data.record.ActionBy + '</span>';
                    //    }
                    //}
                },
                SourceAddress: {
                    title: 'آدرس IP کاربر',
                    width: '10%',
                    display: function (data) {
                        return '<a href="http://www.ip-tracker.org/locator/ip-lookup.php?ip=' + data.record.SourceAddress + '" target="_blank" rel="noreferrer" title="جهت مشاهده جزئیات آی پی کلیک کنید">' + data.record.SourceAddress + '</a>';
                    }
                },
                Message: {
                    title: 'پیام',
                    width: '30%',
                },
                ActionDateText: {
                    title: 'زمان',
                    width: '10%',
                },
                Url: {
                    title: 'آدرس صفحه',
                    width: '25%',
                    listClass: 'uk-text-right',
                    display: function (data) {
                        return '<span class=\"font-arial\">' + data.record.Url + '</span>';
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Memberships
    initMemberships: function () {
        $('#memberships_crud').jtable({
            title: 'عضویت ها',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                if (data.formType == 'edit') {
                    var startTime = persianNumberToEnglishNumber(data.record.StartTime);
                    var endTime = persianNumberToEnglishNumber(data.record.EndTime);
                    $('input[name="StartTime"]').val(startTime);
                    $('input[name="EndTime"]').val(endTime);
                }

                $('input[name="StartTime"]').pDatepicker({
                    format: 'YYYY/MM/DD',
                    autoClose: true,
                });

                $('input[name="EndTime"]').pDatepicker({
                    format: 'YYYY/MM/DD',
                    autoClose: true,
                });

                if (data.formType == 'edit') {
                    var startTime = persianNumberToEnglishNumber(data.record.StartTime);
                    var endTime = persianNumberToEnglishNumber(data.record.EndTime);

                    try {
                        $('input[name="StartTime"]').pDatepicker("setDate", [parseInt(startTime.split('/')[0]), parseInt(startTime.split('/')[1]), parseInt(startTime.split('/')[2]), 12, 0]);
                    }
                    catch (err) { }

                    try {
                        $('input[name="EndTime"]').pDatepicker("setDate", [parseInt(endTime.split('/')[0]), parseInt(endTime.split('/')[1]), parseInt(endTime.split('/')[2]), 12, 0]);
                    }
                    catch (err) { }

                    if (!data.record.StartTime) {
                        $('input[name="StartTime"]').val('');
                    }
                    else if (data.record.StartTime.indexOf('/') == -1) {
                        $('input[name="StartTime"]').val(startTime);
                    }

                    if (!data.record.EndTime) {
                        $('input[name="EndTime"]').val('');
                    }
                    else if (data.record.EndTime.indexOf('/') == -1) {
                        $('input[name="EndTime"]').val(endTime);
                    }
                }
                else {
                    $('input[name="StartTime"]').val('');
                    $('input[name="EndTime"]').val('');
                }

                $('input[name="StartTime"], input[name="EndTime"]').focusout();

                $('input[name="Post"]').attr('data-parsley-required', 'true');
                $('input[name="CommitteeTitle"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        return e.$element.parent('.md-input-wrapper').parent();
                    },
                    classHandler: function (e) {
                        var a = e.$element;
                        if (a.is(":checkbox") || a.is(":radio") || a.parent().is("label") || $(a).is("[data-md-selectize]")) return a.closest(".parsley-row")
                    }
                });
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                CommitteeTitle: {
                    title: 'عنوان کمیته',
                    width: '25%'
                },
                Post: {
                    title: 'سمت',
                    width: '15%'

                },
                StartTime: {
                    title: 'زمان شروع',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.StartTime) {
                            return '<span class="persian-number">' + data.record.StartTime + '</a>';
                        }
                    }
                },
                EndTime: {
                    title: 'زمان پایان',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.EndTime) {
                            return '<span class="persian-number">' + data.record.EndTime + '</a>';
                        }
                    }
                },
                Description: {
                    title: 'توضیحات',
                    width: '21%'
                },
                Link: {
                    title: 'لینک',
                    width: '6%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '11%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Galleries
    initGalleries: function () {
        $('#galleries_crud').jtable({
            title: 'لیست گالری ها',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            //openChildAsAccordion: true,
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'گالری مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        //onChange: function (value) {
                        //    $('select[name="Name"]').val(value);
                        //    $(data.form).parsley().validate();
                        //}
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('بله');
                            //$this.prop('checked', true);
                            $this.attr('value', true);
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('خیر');
                            //$this.prop('checked', false);
                            $this.attr('value', false);
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                var $isActiveElement = $('input[name="IsActive"]');
                if (data.formType == 'create')
                {
                    $isActiveElement.iCheck('check');
                }
                //else if (data.formType == 'edit')
                //{
                //    if (data.record.IsActive == 'true')
                //    {
                //        $isActiveElement.prop('checked', true).attr('value', 'true');
                //        $isActiveElement.iCheck('check');
                //    }
                //    else
                //    {
                //        $isActiveElement.prop('checked', false).attr('value', 'false');
                //        $isActiveElement.iCheck('uncheck');
                //    }
                //}

                //$('select[name="Name"]').attr('data-parsley-min', '2');
                //$('select[name="Name"]').attr('data-parsley-min-message', 'لطفاً زبان را انتخاب نمائید.');
                $('input[name="Title"]').attr('data-parsley-required', 'true');
                //$('input[name="LessonType"]').attr('data-parsley-min', '2');
                //$('input[name="LessonState"]').attr('data-parsley-min', '2');
                //$('input[name="UnitState"]').attr('data-parsley-min', '2');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input")) {
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
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                GalleryItems: {
                    title: '',
                    width: '5%',
                    edit: false,
                    create: false,
                    listClass: 'child-opener-image-column',
                    display: function (galleryData) {
                        var $img = $('<img class="child-opener-image" src="/build/images/galleryitem.png" title="آیتم های گالری" width="30" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" />');
                        $img.click(function () {
                            $('#galleries_crud').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: galleryData.record.Title + ' - آیتم ها',
                                    actions: {
                                        listAction: $('#variables').data('listGalleryitemUrl') + '?GalleryId=' + galleryData.record.Id,
                                        createAction: function (data) {
                                            var deferred = $.Deferred();

                                            // Capture form submit result from the hidden iframe
                                            $("#postiframe").load(function () {
                                                iframeContents = $("#postiframe").contents().find("body").text();
                                                var result = $.parseJSON(iframeContents);
                                                deferred.resolve(result);
                                            });

                                            // Submit form with file upload settings
                                            var form = $('#jtable-create-form');
                                            form.unbind('submit');
                                            form.attr('action', $('#variables').data('createGalleryitemUrl') + '?GalleryId=' + galleryData.record.Id);
                                            form.attr('method', 'post');
                                            form.attr('enctype', 'multipart/form-data');
                                            form.attr('encoding', 'multipart/form-data');
                                            form.attr('target', 'postiframe');
                                            form.submit();

                                            return deferred;
                                        },
                                        updateAction: function () {
                                            var deferred = $.Deferred();

                                            // Capture form submit result from the hidden iframe
                                            $("#postiframe").load(function () {
                                                iframeContents = $("#postiframe").contents().find("body").text();
                                                var result = $.parseJSON(iframeContents);
                                                deferred.resolve(result);
                                            });

                                            // Submit form with file upload settings
                                            var form = $('#jtable-edit-form');
                                            form.unbind('submit');
                                            form.attr('action', $('#variables').data('updateGalleryitemUrl') + '?GalleryId=' + galleryData.record.Id);
                                            form.attr('method', 'post');
                                            form.attr('enctype', 'multipart/form-data');
                                            form.attr('encoding', 'multipart/form-data');
                                            form.attr('target', 'postiframe');
                                            form.submit();

                                            return deferred;
                                        },
                                        deleteAction: $('#variables').data('deleteGalleryitemUrl') + '?GalleryId=' + galleryData.record.Id
                                    },
                                    fields: {
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        Title: {
                                            title: 'عنوان',
                                            width: '20%',
                                        },
                                        Description: {
                                            title: 'توضیحات',
                                            width: '40%',
                                            input: function (data) {
                                                if (!data.record || !data.record.Description) {
                                                    return '<textarea rows="2" name="Description"></textarea>';
                                                } else if (data.record.Description) {
                                                    return '<textarea rows="2" name="Description">' + data.record.Description + '</textarea>';
                                                }
                                            }
                                        },
                                        MediaType: {
                                            title: 'نوع',
                                            width: '7%',
                                            options: {
                                                '1': '--', '2': 'تصویر',
                                                '3': 'ویدئو', '4': 'فایل صوتی',
                                                '5': 'غیره',
                                            }
                                        },
                                        FileInput: {
                                            title: 'فایل',
                                            list: false,
                                            input: function (data) {
                                                return '<div class="uk-form-file md-btn md-btn-primary" style="margin-right:5px;"> انتخاب فایل <input id="file" name="file" type="file"/><iframe name="postiframe" id="postiframe" style="display: none" /> </div>';
                                            }
                                        },
                                        FileText: {
                                            title: 'فایل',
                                            width: '5%',
                                            listClass: 'uk-text-center',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.FileText) {
                                                    return '<a href="' + $('#variables').data('getfileUrl') + '?Type=5&FileText=' + data.record.FileText + '"><i style="cursor:pointer;font-size:22px;" class="material-icons">file_download</i></a>';
                                                }
                                            }
                                        },
                                        //DeleteFile: {
                                        //    title: 'فایل حذف شود؟ (در صورت موجود بودن)',
                                        //    list: false,
                                        //    create: false,
                                        //    defaultValue: false,
                                        //    type: 'checkbox',
                                        //    values: { false: 'خیر', true: 'بله' },
                                        //},
                                        CreateDateText: {
                                            title: 'تاریخ ایجاد',
                                            width: '10%',
                                            create: false,
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.CreateDateText) {
                                                    return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                                                }
                                            }
                                        },
                                        Link: {
                                            title: 'لینک',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Link) {
                                                    return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                                                }
                                            }
                                        },
                                        Order: {
                                            title: 'اولویت قرارگیری',
                                            width: '5%',
                                            inputClass: 'english-input',
                                            display: function (data) {
                                                if (data.record.Order) {
                                                    return '<span class="persian-number">' + data.record.Order + '</span>';
                                                }
                                            }
                                        }
                                    },
                                    recordAdded: function (event, data) {
                                        $img.parents('tr').next('tr.jtable-child-row').find('.jtable-child-table-container').jtable('reload');
                                    },
                                    recordUpdated: function (event, data) {
                                        $img.parents('tr').next('tr.jtable-child-row').find('.jtable-child-table-container').jtable('reload');
                                    },
                                    formCreated: function (event, data) {
                                        //data.form.attr('enctype', 'multipart/form-data');
                                        data.form.find('.jtable-option-text-clickable').each(function () {
                                            var $thisTarget = $(this).prev().attr('id');
                                            $(this)
                                                .attr('data-click-target', $thisTarget)
                                                .off('click')
                                                .on('click', function (e) {
                                                    e.preventDefault();
                                                    $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                                                })
                                        });
                                        // create selectize
                                        data.form.find('select').each(function () {
                                            var $this = $(this);
                                            $this.after('<div class="selectize_fix"></div>')
                                            .selectize({
                                                dropdownParent: 'body',
                                                placeholder: 'Click here to select ...',
                                                onDropdownOpen: function ($dropdown) {
                                                    $dropdown
                                                        .hide()
                                                        .velocity('slideDown', {
                                                            begin: function () {
                                                                $dropdown.css({ 'margin-top': '0' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                onDropdownClose: function ($dropdown) {
                                                    $dropdown
                                                        .show()
                                                        .velocity('slideUp', {
                                                            complete: function () {
                                                                $dropdown.css({ 'margin-top': '' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                //onChange: function (value) {
                                                //    $('select[name="Name"]').val(value);
                                                //    $(data.form).parsley().validate();
                                                //}
                                            });
                                        });
                                        // create icheck
                                        data.form
                                            .find('input[type="checkbox"],input[type="radio"]')
                                            .each(function () {
                                                var $this = $(this);
                                                $this.iCheck({
                                                    checkboxClass: 'icheckbox_md',
                                                    radioClass: 'iradio_md',
                                                    increaseArea: '20%'
                                                })
                                                .on('ifChecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('بله');
                                                    $this.prop('checked', true);
                                                    $this.attr('value', true);
                                                })
                                                .on('ifUnchecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('خیر');
                                                    $this.prop('checked', false);
                                                    $this.attr('value', false);
                                                })
                                            });
                                        // reinitialize inputs
                                        data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                                            $(this).addClass('md-input');
                                            altair_forms.textarea_autosize();
                                        });
                                        altair_md.inputs();

                                        $('.ui-dialog-buttonset')
                                            .children('button')
                                            .attr('class', '')
                                            .addClass('md-btn md-btn-flat')
                                            .off('mouseenter focus');
                                        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');

                                        $('input[name="Title"]').attr('data-parsley-required', 'true');
                                        if (data.formType == 'create')
                                        {
                                            $('input[name="file"]').attr('data-parsley-required', 'true');
                                            $('input[name="file"]').attr('data-parsley-required-message', 'لطفاً فایل را انتخاب نمائید.');
                                        }
                                        $('select[name="MediaType"]').attr('data-parsley-min', '2');
                                        $('select[name="MediaType"]').attr('data-parsley-min-message', 'لطفاً نوع فایل را انتخاب نمائید.');
                                        $('input[name="Order"]').attr('data-parsley-type', 'number');
                                        $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                                        $(data.form).parsley({
                                            excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                                            trigger: "change focusin blur",
                                            errorsWrapper: '<div class="parsley-errors-list"></div>',
                                            errorTemplate: "<span></span>",
                                            errorClass: "md-input-danger",
                                            successClass: "md-input-success",
                                            errorsContainer: function (e) {
                                                if (e.$element.is('input[type="file"]'))
                                                {
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

                                    },
                                    formSubmitting: function (event, data) {
                                        var $form = $(data.form);
                                        $form.parsley().validate();

                                        if (!$form.parsley().isValid()) {
                                            return false;
                                        }

                                        //var filename = $('input[type="file"]').val().split('\\').pop();
                                        //($("#" + data.form.attr("id")).find('input[name="File"]').val(filename));
                                    },
                                    //formClosed: function (event, data) {
                                    //},
                                    rowInserted: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    },
                                    rowUpdated: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                    $('.jtable-toolbar').css({ 'backgroundColor': '#7cb342', 'padding': '4px', 'color': '#fff' });
                                    $('.jtable-toolbar-item-icon').addClass('material-icons white').text('add');
                                    $('.jtable-toolbar-item.jtable-toolbar-item-add-record').addClass('mb-n4');
                                });
                        });

                        return $img;
                    }
                },
                Title: {
                    title: 'عنوان گالری',
                    width: '20%'
                },
                Description: {
                    title: 'توضیحات',
                    list: true,
                    width: '45%',
                    input: function (data) {
                        if (!data.record || !data.record.Description) {
                            return '<textarea rows="2" name="Description"></textarea>';
                        } else if (data.record.Description) {
                            return '<textarea rows="2" name="Description">' + data.record.Description + '</textarea>';
                        }
                    }
                },
                IsActive: {
                    title: 'فعال؟',
                    inputTitle: 'گالری فعال باشد؟',
                    //defaultValue: true,
                    width: '5%',
                    type: 'checkbox',
                    values: { false: 'خیر', true: 'بله' },
                    //display: function (data) {
                    //    var color = 'red';
                    //    var icon = 'highlight_off';
                    //    if (data.record.IsActive == true) 
                    //    {
                    //        color = 'green';
                    //        icon = 'done';
                    //    }
                    //    if (data.record.IsActive == false)
                    //    {
                    //        color = 'red';
                    //        icon = 'highlight_off';
                    //    }
                    //    return '<div style="width:100%; height:100%; text-align:center;"><i class="material-icons ' + color + '">' + icon + '</i></div>';
                    //},
                    //input: function (data) {
                    //    if (data.record.IsActive == 'true') {
                    //        return '<input type="checkbox" name="IsActive" value="true" checked />';
                    //    }
                    //    return '<input type="checkbox" name="IsActive" value="false" />';
                    //}
                },
                CreateDateText: {
                    title: 'تاریخ ایجاد',
                    width: '10%',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (data.record.CreateDateText) {
                            return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                        }
                    }
                },
                Link: {
                    title: 'لینک',
                    width: '5%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '10%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Admin Messages
    initAdminMessages: function () {
        $('#adminmessages_crud').jtable({
            title: 'لیست پیام ها',
            paging: true, //Enable paging
            pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'پیام مورد نظر حذف شود؟';
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: null,
                deleteAction: $('#variables').data('deleteUrl')
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        //onChange: function (value) {
                        //    $('select[name="Name"]').val(value);
                        //    $(data.form).parsley().validate();
                        //}
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('بله');
                            //$this.prop('checked', true);
                            $this.attr('value', true);
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('خیر');
                            //$this.prop('checked', false);
                            $this.attr('value', false);
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                $('textarea[name="Content"]').attr('data-parsley-required', 'true');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input") || e.$element.is("textarea")) {
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

                if (data.formType == 'create') {
                    $('span.ui-dialog-title').text('پیام جدید');
                    $('#AddRecordDialogSaveButton').text('ارسال');
                }
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function (event, data) {
                var $editColumn = data.row.find('.jtable-edit-command-button').parent('td.jtable-command-column');
                $editColumn.remove();

                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                StateText: {
                    title: 'وضعیت',
                    create: false,
                    edit: false,
                    width: '10%',
                    display: function (data) {
                        if (data.record.State == '1') {
                            return '<span class="uk-badge uk-badge-warning fs-16p">' + data.record.StateText + '</span>';
                        }
                        else if (data.record.State == '2') {
                            return '<span class="uk-badge uk-badge-success fs-16p">' + data.record.StateText + '</span>';
                        }
                        else {
                            return '<span class="uk-badge fs-16p">' + data.record.StateText + '</span>';
                        }
                    }
                },
                Title: {
                    title: 'موضوع پیام',
                    width: '20%'
                },
                Content: {
                    title: 'متن پیام',
                    width: '30%',
                    type: 'textarea',
                    inputClass: 'full-width'
                },
                ReplyContent: {
                    title: 'پاسخ دریافتی',
                    type: 'textarea',
                    create: false,
                    edit: false,
                    width: '30%',
                },
                CreateDateText: {
                    title: 'تاریخ ارسال',
                    width: '10%',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (data.record.CreateDateText) {
                            return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Manager Admin Messages
    initManagerAdminMessages: function () {
        $('#manageradminmessages_crud').jtable({
            title: 'لیست پیام ها',
            paging: true, //Enable paging
            pageSize: 20, //Set page size (default: 10)
            addRecordButton: null,
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'پیام مورد نظر حذف شود؟';
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: null,
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        //onChange: function (value) {
                        //    $('select[name="Name"]').val(value);
                        //    $(data.form).parsley().validate();
                        //}
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('بله');
                            //$this.prop('checked', true);
                            $this.attr('value', true);
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('خیر');
                            //$this.prop('checked', false);
                            $this.attr('value', false);
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                $('textarea[name="Content"]').attr('data-parsley-required', 'true');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input") || e.$element.is("textarea")) {
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

                if (data.formType == 'edit')
                {
                    $('span.ui-dialog-title').text('ارسال پاسخ');
                    $('#EditDialogSaveButton').text('ارسال');
                }
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            recordsLoaded: function (event, data) {
                $('body').trigger('display.uk.check');
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function (event, data) {
                var $editBtn = data.row.find('.jtable-edit-command-button');
                $editBtn.css('background-image', 'none').attr('title', 'ارسال پاسخ').html('<i class="material-icons md-24 green">reply</i>');

                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                StateText: {
                    title: 'وضعیت',
                    create: false,
                    edit: false,
                    width: '10%',
                    display: function (data) {
                        if (data.record.State == '1')
                        {
                            return '<span class="uk-badge uk-badge-warning fs-16p">معلق</span>';
                        }
                        else if (data.record.State == '2')
                        {
                            return '<span class="uk-badge uk-badge-success fs-16p">' + data.record.StateText + '</span>';
                        }
                        else
                        {
                            return '<span class="uk-badge fs-16p">' + data.record.StateText + '</span>';
                        }
                    }
                },
                Title: {
                    title: 'موضوع پیام',
                    create: false,
                    edit: false,
                    width: '20%'
                },
                Content: {
                    title: 'متن پیام',
                    width: '20%',
                    create: false,
                    edit: false,
                },
                Fullname:{
                    title: 'ارسال کننده',
                    width: '5%',
                    create: false,
                    edit: false
                },
                Email: {
                    title: 'ایمیل',
                    width: '10%',
                    create: false,
                    edit: false,
                    display: function (data) {
                        //console.log(data);
                        return '<div class="font-arial" style="width:100%; height:100%; text-align:center;direction:ltr;"><a href="mailto:' + data.record.Email + '" class="font-arial" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" title="ارسال ایمیل به ' + data.record.Fullname + '">' + data.record.Email + '</a></div>';
                    }
                },
                PageId: {
                    title: 'شناسه',
                    width: '5%',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (!data.record.PageId) {
                            return '<div style="width:100%; height:100%; text-align:center;">-</div>';
                        }
                        return '<div class="font-arial" style="width:100%; height:100%; text-align:center;"><a href="/' + data.record.PageId + '" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" title="ورود به صفحه شخصی" target="_blank"><i class="material-icons">account_box</i>' + data.record.PageId + '</a></div>';
                    }
                },
                ReplyContent: {
                    title: 'پاسخ ارسالی',
                    inputTitle: 'پاسخ ارسالی (وارد کردن این فیلد اختیاری است. در صورتی که نیاز به ارسال هیچ پیامی به کاربر ندارید، در این فیلد چیزی وارد نکنید اما حتماً بر روی دکمه ارسال کلیک کنید.)',
                    create: false,
                    width: '20%',
                    inputClass: 'full-width',
                    type: 'textarea'
                },
                CreateDateText: {
                    title: 'تاریخ درخواست',
                    width: '10%',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (data.record.CreateDateText) {
                            return '<span class="persian-number">' + data.record.CreateDateText + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Citations Management
    initCitationManagement: function () {
        $('#citations_crud').jtable({
            title: 'لیست مقالات و ارجاعات',
            paging: true, //Enable paging
            pageSize: 20, //Set page size (default: 10)
            //openChildAsAccordion: true,
            addRecordButton: null,
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        //onChange: function (value) {
                        //    $('select[name="Name"]').val(value);
                        //    $(data.form).parsley().validate();
                        //}
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('بله');
                            //$this.prop('checked', true);
                            $this.attr('value', true);
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('خیر');
                            //$this.prop('checked', false);
                            $this.attr('value', false);
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                var $isActiveElement = $('input[name="IsActive"]');
                if (data.formType == 'create') {
                    $isActiveElement.iCheck('check');
                }

                $('input[name="ScopusHIndex"]').attr('data-parsley-type', 'number');
                $('input[name="GoogleHIndex"]').attr('data-parsley-type', 'number');
                $('input[name="ScopusCitations"]').attr('data-parsley-type', 'number');
                $('input[name="GoogleCitations"]').attr('data-parsley-type', 'number');
                $('input[name="ScopusDocuments"]').attr('data-parsley-type', 'number');
                $('input[name="ScopusTotalDocumentsCited"]').attr('data-parsley-type', 'number');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input")) {
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
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: null,
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: null
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function (event, data) {
                var $deleteColumn = data.row.find('.jtable-delete-command-button').parent('td.jtable-command-column');
                $deleteColumn.remove();

                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                UserId: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Citations: {
                    title: '',
                    width: '3%',
                    edit: false,
                    create: false,
                    listClass: 'child-opener-image-column',
                    display: function (userData) {
                        var $img = $('<img class="child-opener-image" src="/build/images/chart.png" title="مقالات و ارجاع ها در سال های مختلف" width="100%" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" />');
                        $img.click(function () {
                            $('#citations_crud').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: userData.record.Fullname + ' - مقالات و ارجاع ها در سال های مختلف',
                                    paging: true,
                                    pageSize: 10,
                                    actions: {
                                        listAction: $('#variables').data('listCitationUrl') + '?UserId=' + userData.record.UserId,
                                        createAction: $('#variables').data('createCitationUrl') + '?UserId=' + userData.record.UserId,
                                        updateAction: $('#variables').data('updateCitationUrl') + '?UserId=' + userData.record.UserId,
                                        deleteAction: $('#variables').data('deleteCitationUrl') + '?UserId=' + userData.record.UserId
                                    },
                                    fields: {
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        Year: {
                                            title: 'سال',
                                            width: '25%',
                                            display: function (data) {
                                                if (data.record.Year) {
                                                    return '<span class="uk-text-center persian-number">' + data.record.Year + '</span>';
                                                }
                                            }
                                        },
                                        Source: {
                                            title: 'منبع',
                                            width: '25%',
                                            options: {
                                                '0': '--', '1': 'اسکاپوس', '2': 'گوگل اسکولار',
                                            },
                                        },
                                        Citation: {
                                            title: 'تعداد ارجاع ها',
                                            width: '25%',
                                            display: function (data) {
                                                if (data.record.Citation) {
                                                    return '<span class="uk-text-center persian-number">' + data.record.Citation + '</span>';
                                                }
                                            }
                                        },
                                        Document: {
                                            title: 'تعداد مقالات',
                                            width: '25%',
                                            display: function (data) {
                                                if (data.record.Document) {
                                                    return '<span class="uk-text-center persian-number">' + data.record.Document + '</span>';
                                                }
                                            }
                                        }
                                    },
                                    recordAdded: function (event, data) {
                                        $img.parents('tr').next('tr.jtable-child-row').find('.jtable-child-table-container').jtable('reload');
                                    },
                                    recordUpdated: function (event, data) {
                                        $img.parents('tr').next('tr.jtable-child-row').find('.jtable-child-table-container').jtable('reload');
                                    },
                                    formCreated: function (event, data) {
                                        //data.form.attr('enctype', 'multipart/form-data');
                                        data.form.find('.jtable-option-text-clickable').each(function () {
                                            var $thisTarget = $(this).prev().attr('id');
                                            $(this)
                                                .attr('data-click-target', $thisTarget)
                                                .off('click')
                                                .on('click', function (e) {
                                                    e.preventDefault();
                                                    $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                                                })
                                        });
                                        // create selectize
                                        data.form.find('select').each(function () {
                                            var $this = $(this);
                                            $this.after('<div class="selectize_fix"></div>')
                                            .selectize({
                                                dropdownParent: 'body',
                                                placeholder: 'Click here to select ...',
                                                onDropdownOpen: function ($dropdown) {
                                                    $dropdown
                                                        .hide()
                                                        .velocity('slideDown', {
                                                            begin: function () {
                                                                $dropdown.css({ 'margin-top': '0' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                onDropdownClose: function ($dropdown) {
                                                    $dropdown
                                                        .show()
                                                        .velocity('slideUp', {
                                                            complete: function () {
                                                                $dropdown.css({ 'margin-top': '' })
                                                            },
                                                            duration: 200,
                                                            easing: easing_swiftOut
                                                        })
                                                },
                                                //onChange: function (value) {
                                                //    $('select[name="Name"]').val(value);
                                                //    $(data.form).parsley().validate();
                                                //}
                                            });
                                        });
                                        // create icheck
                                        data.form
                                            .find('input[type="checkbox"],input[type="radio"]')
                                            .each(function () {
                                                var $this = $(this);
                                                $this.iCheck({
                                                    checkboxClass: 'icheckbox_md',
                                                    radioClass: 'iradio_md',
                                                    increaseArea: '20%'
                                                })
                                                .on('ifChecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('بله');
                                                    $this.prop('checked', true);
                                                    $this.attr('value', true);
                                                })
                                                .on('ifUnchecked', function (event) {
                                                    $this.parent('div.icheckbox_md').next('span').text('خیر');
                                                    $this.prop('checked', false);
                                                    $this.attr('value', false);
                                                })
                                            });
                                        // reinitialize inputs
                                        data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                                            $(this).addClass('md-input');
                                            altair_forms.textarea_autosize();
                                        });
                                        altair_md.inputs();

                                        $('.ui-dialog-buttonset')
                                            .children('button')
                                            .attr('class', '')
                                            .addClass('md-btn md-btn-flat')
                                            .off('mouseenter focus');
                                        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');

                                        $('input[name="Year"]').attr('data-parsley-required', 'true');
                                        $('input[name="Citation"]').attr('data-parsley-required', 'true');
                                        $('select[name="Source"]').attr('data-parsley-min', '1');
                                        $('select[name="Source"]').attr('data-parsley-min-message', 'لطفاً منبع را انتخاب نمائید.');
                                        $('input[name="Year"]').attr('data-parsley-type', 'number');
                                        $('input[name="Citation"]').attr('data-parsley-type', 'number');
                                        $('input[name="Document"]').attr('data-parsley-type', 'number');

                                        $(data.form).parsley({
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

                                        $(document).on('change', 'select[name="Source"]', function () {
                                            var $input = $('input[name="Document"]');
                                            if ($('select[name="Source"]').val() == '1') {
                                                $input.attr('data-parsley-required', 'true');
                                                $input.prop('disabled', false);
                                                $input.parent('.md-input-wrapper').removeClass('md-input-wrapper-disabled');;
                                            }
                                            else
                                            {
                                                $input.removeAttr('data-parsley-required');
                                                $input.prop('disabled', true);
                                                $input.parent('.md-input-wrapper').addClass('md-input-wrapper-disabled');
                                            }
                                        });
                                    },
                                    formSubmitting: function (event, data) {
                                        var $form = $(data.form);
                                        $form.parsley().validate();

                                        if (!$form.parsley().isValid()) {
                                            return false;
                                        }

                                        //var filename = $('input[type="file"]').val().split('\\').pop();
                                        //($("#" + data.form.attr("id")).find('input[name="File"]').val(filename));
                                    },
                                    //formClosed: function (event, data) {
                                    //},
                                    rowInserted: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    },
                                    rowUpdated: function () {
                                        $('.persian-number').each(function () {
                                            $(this).text(persianJs($(this).text()).englishNumber().toString());
                                        });
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                    $('.jtable-toolbar').css({ 'backgroundColor': '#7cb342', 'padding': '4px', 'color': '#fff' });
                                    $('.jtable-toolbar-item-icon').addClass('material-icons white').text('add');
                                    $('.jtable-toolbar-item.jtable-toolbar-item-add-record').addClass('mb-n4');
                                });
                        });

                        return $img;
                    }
                },
                Fullname: {
                    title: 'ارسال کننده',
                    width: '8%',
                    create: false,
                    edit: false
                },
                Email: {
                    title: 'ایمیل',
                    width: '9%',
                    create: false,
                    edit: false,
                    display: function (data) {
                        //console.log(data);
                        return '<div class="font-arial" style="width:100%; height:100%; text-align:center;direction:ltr;"><a href="mailto:' + data.record.Email + '" class="font-arial" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" title="ارسال ایمیل به ' + data.record.Fullname + '">' + data.record.Email + '</a></div>';
                    }
                },
                PageId: {
                    title: 'شناسه',
                    width: '5%',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (!data.record.PageId) {
                            return '<div style="width:100%; height:100%; text-align:center;">-</div>';
                        }
                        return '<div class="font-arial" style="width:100%; height:100%; text-align:center;"><a href="/' + data.record.PageId + '" data-uk-tooltip="{pos:\'top\', cls:\'fs-16p\'}" title="ورود به صفحه شخصی" target="_blank"><i class="material-icons">account_box</i>' + data.record.PageId + '</a></div>';
                    }
                },
                ScopusHIndex: {
                    title: 'H-Index اسکاپوس',
                    width: '7%',
                    display: function (data) {
                        if (data.record.ScopusHIndex) {
                            return '<span class="uk-text-center persian-number">' + data.record.ScopusHIndex + '</span>';
                        }
                    }
                },
                GoogleHIndex: {
                    title: 'H-Index گوگل',
                    width: '7%',
                    display: function (data) {
                        if (data.record.GoogleHIndex) {
                            return '<span class="uk-text-center persian-number">' + data.record.GoogleHIndex + '</span>';
                        }
                    }
                },
                //ScopusCitations: {
                //    title: 'تعداد ارجاع های اسکاپوس',
                //    width: '9%',
                //    display: function (data) {
                //        if (data.record.ScopusCitations) {
                //            return '<span class="uk-text-center persian-number">' + data.record.ScopusCitations + '</span>';
                //        }
                //    }
                //},
                GoogleCitations: {
                    title: 'تعداد ارجاع های گوگل',
                    width: '9%',
                    display: function (data) {
                        if (data.record.GoogleCitations) {
                            return '<span class="uk-text-center persian-number">' + data.record.GoogleCitations + '</span>';
                        }
                    }
                },
                ScopusDocuments: {
                    title: 'تعداد مقالات اسکاپوس',
                    width: '9%',
                    display: function (data) {
                        if (data.record.ScopusDocuments) {
                            return '<span class="uk-text-center persian-number">' + data.record.ScopusDocuments + '</span>';
                        }
                    }
                },
                //ScopusTotalDocumentsCited: {
                //    title: 'تعداد مقالاتی که این نویسنده در آن ها ارجاع شده',
                //    width: '13%',
                //    display: function (data) {
                //        if (data.record.ScopusTotalDocumentsCited)
                //        {
                //            return '<span class="uk-text-center persian-number">' + data.record.ScopusTotalDocumentsCited + '</span>';
                //        }
                //    }
                //},
                OtherNamesFormat: {
                    title: 'اسامی دیگر',
                    width: '10%',
                },
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Weekly Programs
    initWeeklyPrograms: function () {
        $('#weeklyprograms_crud').jtable({
            title: 'لیست روزها و ساعت های برنامه هفتگی',
            //paging: true, //Enable paging
            //pageSize: 10, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'برنامه مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Active');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('Passive');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="StartTime"]').attr('data-parsley-required', 'true');
                $('input[name="EndTime"]').attr('data-parsley-required', 'true');
                $('select[name="DayOfProgram"]').attr('data-parsley-min', '1');
                $('select[name="DayOfProgram"]').attr('data-parsley-min-message', 'لطفاً روز را انتخاب نمائید.');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input")) {
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
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                DayOfProgram: {
                    title: 'روز',
                    width: '15%',
                    options: { '0': '--', '1': 'شنبه', '2': 'یکشنبه', '3': 'دوشنبه', '4': 'سه شنبه', '5': 'چهارشنبه', '6': 'پنجشنبه', '7': 'جمعه', }
                },
                StartTime: {
                    title: 'زمان شروع',
                    width: '15%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.StartTime) {
                            return '<span class="persian-number">' + data.record.StartTime + '</a>';
                        }
                    },
                    input: function (data) {
                        if (!data.record || !data.record.StartTime) {
                            return '<input class="md-input" type="text" name="StartTime" data-uk-timepicker="" autocomplete="off">';
                        }
                        else {
                            return '<input class="md-input" type="text" name="StartTime" data-uk-timepicker="" autocomplete="off" value="' + data.record.StartTime + '">';
                        }
                    }
                },
                EndTime: {
                    title: 'زمان پایان',
                    width: '15%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.EndTime) {
                            return '<span class="persian-number">' + data.record.EndTime + '</a>';
                        }
                    },
                    input: function (data) {
                        if (!data.record || !data.record.EndTime) {
                            return '<input class="md-input" type="text" name="EndTime" data-uk-timepicker="" autocomplete="off">';
                        }
                        else {
                            return '<input class="md-input" type="text" name="EndTime" data-uk-timepicker="" autocomplete="off" value="' + data.record.EndTime + '">';
                        }
                    }
                },
                Description: {
                    title: 'توضیحات',
                    width: '55%',
                    type: 'textarea'
                },
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    initExternalArticles: function () {
        $('#externalarticles_crud').jtable({
            title: 'لیست مقالات خارجی',
            paging: true, //Enable paging
            pageSize: 20, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'مقاله مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('بله');
                            $this.prop('checked', true).attr('value', 'true');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('خیر');
                            $this.prop('checked', false).attr('value', 'false');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                $('input[name="TimesCited"]').attr('data-parsley-type', 'number');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input")) {
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
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: function (data) {
                    var deferred = $.Deferred();

                    // Capture form submit result from the hidden iframe
                    $("#postiframe").load(function () {
                        iframeContents = $("#postiframe").contents().find("body").text();
                        var result = $.parseJSON(iframeContents);
                        deferred.resolve(result);
                    });

                    // Submit form with file upload settings
                    var form = $('#jtable-create-form');
                    form.unbind('submit');
                    form.attr('action', $('#variables').data('createUrl'));
                    form.attr('method', 'post');
                    form.attr('enctype', 'multipart/form-data');
                    form.attr('encoding', 'multipart/form-data');
                    form.attr('target', 'postiframe');
                    form.submit();

                    return deferred;
                },
                updateAction: function (data) {
                    var deferred = $.Deferred();

                    // Capture form submit result from the hidden iframe
                    $("#postiframe").load(function () {
                        iframeContents = $("#postiframe").contents().find("body").text();
                        var result = $.parseJSON(iframeContents);
                        deferred.resolve(result);
                    });

                    // Submit form with file upload settings
                    var form = $('#jtable-edit-form');
                    form.unbind('submit');
                    form.attr('action', $('#variables').data('updateUrl'));
                    form.attr('method', 'post');
                    form.attr('enctype', 'multipart/form-data');
                    form.attr('encoding', 'multipart/form-data');
                    form.attr('target', 'postiframe');
                    form.submit();

                    return deferred;
                },
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('#externalarticles_crud').jtable('reload');
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان',
                    width: '16%',
                },
                Doi: {
                    title: 'شناسه دیجیتال',
                    inputClass: 'english-input',
                    width: '10%',
                    display: function (data) {
                        if (data.record.Doi) {
                            return '<div class="direction-ltr"><a href="http://dx.doi.org/' + data.record.Doi + '" target="_blank" rel="noreferrer">' + data.record.Doi + '</a></div>';
                        }
                    }
                },
                Authors: {
                    title: 'نویسندگان',
                    width: '15%',
                },
                Journal: {
                    title: 'نام ژورنال',
                    width: '10%',
                },
                Volume: {
                    title: 'Volume',
                    width: '5%',
                },
                Issue: {
                    title: 'Issue',
                    width: '5%',
                },
                Pages: {
                    title: 'صفحات',
                    width: '3%',
                },
                Year: {
                    title: 'سال',
                    width: '7%',
                },
                TimesCited: {
                    title: 'تعداد دفعات ارجاع',
                    width: '3%',
                },
                Abstract: {
                    title: 'چکیده',
                    width: '5%',
                    type: 'textarea',
                    display: function (data) {
                        if (data.record.Abstract)
                        {
                            var newId = guid();
                            return '<a href="javascript:void(0)" data-uk-modal="{target:\'#' + newId + '\'}">[چکیده]</a><div data-abstract-container class="uk-modal" id="' + newId + '" aria-hidden="true"> <div class="uk-modal-dialog"> <div class="uk-modal-header"> <h3 class="uk-modal-title">چکیده</span></h3> </div> <p class="uk-text-left" style="white-space: pre-wrap;" data-abstract>' + data.record.Abstract.replace(/(?:\r\n|\r|\n)/g, '<br />') + '</p> <div class="uk-modal-footer uk-text-left"> <button type="button" class="md-btn md-btn-flat uk-modal-close">بستن</button> </div> </div> </div>';
                        }
                    }
                },
                FileInput: {
                    title: 'فایل مقاله (اختیاری)',
                    list: false,
                    input: function (data) {
                        return '<div class="uk-form-file md-btn md-btn-primary" style="margin-right:5px;"> انتخاب فایل <input id="file" name="file" type="file"/><iframe name="postiframe" id="postiframe" style="display: none" /> </div>';
                    }
                },
                FileText: {
                    title: 'فایل',
                    width: '3%',
                    listClass: 'uk-text-center',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (data.record.FileText) {
                            return '<a href="' + $('#variables').data('getfileUrl') + '?Type=6&FileText=' + data.record.FileText + '"><i style="cursor:pointer;font-size:22px;" class="material-icons">file_download</i></a>';
                        }
                    }
                },
                DeleteFile: {
                    title: 'فایل حذف شود؟ (در صورت موجود بودن)',
                    list: false,
                    create: false,
                    defaultValue: false,
                    type: 'checkbox',
                    values: { false: 'خیر', true: 'بله' },
                },
                Description: {
                    title: 'توضیحات',
                    width: '10%',
                    type: 'textarea'
                },
                Link: {
                    title: 'لینک',
                    width: '4%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '4%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #region External Seminars
    initExternalSeminars: function () {
        $('#externalseminars_crud').jtable({
            title: 'لیست سمینارها و کنگره های خارجی',
            paging: true, //Enable paging
            pageSize: 20, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('بله');
                            $this.prop('checked', true).attr('value', 'true');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('خیر');
                            $this.prop('checked', false).attr('value', 'false');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input")) {
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
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: function (data) {
                    var deferred = $.Deferred();

                    // Capture form submit result from the hidden iframe
                    $("#postiframe").load(function () {
                        iframeContents = $("#postiframe").contents().find("body").text();
                        var result = $.parseJSON(iframeContents);
                        deferred.resolve(result);
                    });

                    // Submit form with file upload settings
                    var form = $('#jtable-create-form');
                    form.unbind('submit');
                    form.attr('action', $('#variables').data('createUrl'));
                    form.attr('method', 'post');
                    form.attr('enctype', 'multipart/form-data');
                    form.attr('encoding', 'multipart/form-data');
                    form.attr('target', 'postiframe');
                    form.submit();

                    return deferred;
                },
                updateAction: function (data) {
                    var deferred = $.Deferred();

                    // Capture form submit result from the hidden iframe
                    $("#postiframe").load(function () {
                        iframeContents = $("#postiframe").contents().find("body").text();
                        var result = $.parseJSON(iframeContents);
                        deferred.resolve(result);
                    });

                    // Submit form with file upload settings
                    var form = $('#jtable-edit-form');
                    form.unbind('submit');
                    form.attr('action', $('#variables').data('updateUrl'));
                    form.attr('method', 'post');
                    form.attr('enctype', 'multipart/form-data');
                    form.attr('encoding', 'multipart/form-data');
                    form.attr('target', 'postiframe');
                    form.submit();

                    return deferred;
                },
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('#externalarticles_crud').jtable('reload');
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان',
                    width: '28%',
                },
                Doi: {
                    title: 'شناسه دیجیتال',
                    inputClass: 'english-input',
                    width: '10%',
                },
                Authors: {
                    title: 'نویسندگان',
                    width: '15%',
                },
                Conference: {
                    title: 'نام کنفرانس',
                    width: '10%',
                },
                Date: {
                    title: 'زمان',
                    width: '6%',
                },
                Abstract: {
                    title: 'چکیده',
                    width: '5%',
                    type: 'textarea',
                    display: function (data) {
                        if (data.record.Abstract) {
                            var newId = guid();
                            return '<a href="javascript:void(0)" data-uk-modal="{target:\'#' + newId + '\'}">[چکیده]</a><div data-abstract-container class="uk-modal" id="' + newId + '" aria-hidden="true"> <div class="uk-modal-dialog"> <div class="uk-modal-header"> <h3 class="uk-modal-title">چکیده</span></h3> </div> <p class="uk-text-left" style="white-space: pre-wrap;" data-abstract>' + data.record.Abstract.replace(/(?:\r\n|\r|\n)/g, '<br />') + '</p> <div class="uk-modal-footer uk-text-left"> <button type="button" class="md-btn md-btn-flat uk-modal-close">بستن</button> </div> </div> </div>';
                        }
                    }
                },
                FileInput: {
                    title: 'فایل (اختیاری)',
                    list: false,
                    input: function (data) {
                        return '<div class="uk-form-file md-btn md-btn-primary" style="margin-right:5px;"> انتخاب فایل <input id="file" name="file" type="file"/><iframe name="postiframe" id="postiframe" style="display: none" /> </div>';
                    }
                },
                FileText: {
                    title: 'فایل',
                    width: '3%',
                    listClass: 'uk-text-center',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (data.record.FileText) {
                            return '<a href="' + $('#variables').data('getfileUrl') + '?Type=6&FileText=' + data.record.FileText + '"><i style="cursor:pointer;font-size:22px;" class="material-icons">file_download</i></a>';
                        }
                    }
                },
                DeleteFile: {
                    title: 'فایل حذف شود؟ (در صورت موجود بودن)',
                    list: false,
                    create: false,
                    defaultValue: false,
                    type: 'checkbox',
                    values: { false: 'خیر', true: 'بله' },
                },
                Description: {
                    title: 'توضیحات',
                    width: '15%',
                    type: 'textarea'
                },
                Link: {
                    title: 'لینک',
                    width: '4%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '4%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Internal Articles
    initInternalArticles: function () {
        $('#internalarticles_crud').jtable({
            title: 'لیست مقالات داخلی',
            paging: true, //Enable paging
            pageSize: 20, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'مقاله مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('بله');
                            $this.prop('checked', true).attr('value', 'true');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('خیر');
                            $this.prop('checked', false).attr('value', 'false');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input")) {
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
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: function (data) {
                    var deferred = $.Deferred();

                    // Capture form submit result from the hidden iframe
                    $("#postiframe").load(function () {
                        iframeContents = $("#postiframe").contents().find("body").text();
                        var result = $.parseJSON(iframeContents);
                        deferred.resolve(result);
                    });

                    // Submit form with file upload settings
                    var form = $('#jtable-create-form');
                    form.unbind('submit');
                    form.attr('action', $('#variables').data('createUrl'));
                    form.attr('method', 'post');
                    form.attr('enctype', 'multipart/form-data');
                    form.attr('encoding', 'multipart/form-data');
                    form.attr('target', 'postiframe');
                    form.submit();

                    return deferred;
                },
                updateAction: function (data) {
                    var deferred = $.Deferred();

                    // Capture form submit result from the hidden iframe
                    $("#postiframe").load(function () {
                        iframeContents = $("#postiframe").contents().find("body").text();
                        var result = $.parseJSON(iframeContents);
                        deferred.resolve(result);
                    });

                    // Submit form with file upload settings
                    var form = $('#jtable-edit-form');
                    form.unbind('submit');
                    form.attr('action', $('#variables').data('updateUrl'));
                    form.attr('method', 'post');
                    form.attr('enctype', 'multipart/form-data');
                    form.attr('encoding', 'multipart/form-data');
                    form.attr('target', 'postiframe');
                    form.submit();

                    return deferred;
                },
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('#externalarticles_crud').jtable('reload');
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان',
                    width: '28%',
                },
                Authors: {
                    title: 'نویسندگان',
                    width: '15%',
                },
                Journal: {
                    title: 'نام ژورنال',
                    width: '10%',
                },
                Year: {
                    title: 'سال',
                    width: '6%',
                },
                Abstract: {
                    title: 'چکیده',
                    width: '5%',
                    type: 'textarea',
                    display: function (data) {
                        if (data.record.Abstract) {
                            var newId = guid();
                            return '<a href="javascript:void(0)" data-uk-modal="{target:\'#' + newId + '\'}">[چکیده]</a><div data-abstract-container class="uk-modal" id="' + newId + '" aria-hidden="true"> <div class="uk-modal-dialog"> <div class="uk-modal-header"> <h3 class="uk-modal-title">چکیده</span></h3> </div> <p class="uk-text-left" style="white-space: pre-wrap;" data-abstract>' + data.record.Abstract.replace(/(?:\r\n|\r|\n)/g, '<br />') + '</p> <div class="uk-modal-footer uk-text-left"> <button type="button" class="md-btn md-btn-flat uk-modal-close">بستن</button> </div> </div> </div>';
                        }
                    }
                },
                FileInput: {
                    title: 'فایل مقاله (اختیاری)',
                    list: false,
                    input: function (data) {
                        return '<div class="uk-form-file md-btn md-btn-primary" style="margin-right:5px;"> انتخاب فایل <input id="file" name="file" type="file"/><iframe name="postiframe" id="postiframe" style="display: none" /> </div>';
                    }
                },
                FileText: {
                    title: 'فایل',
                    width: '3%',
                    listClass: 'uk-text-center',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (data.record.FileText) {
                            return '<a href="' + $('#variables').data('getfileUrl') + '?Type=6&FileText=' + data.record.FileText + '"><i style="cursor:pointer;font-size:22px;" class="material-icons">file_download</i></a>';
                        }
                    }
                },
                DeleteFile: {
                    title: 'فایل حذف شود؟ (در صورت موجود بودن)',
                    list: false,
                    create: false,
                    defaultValue: false,
                    type: 'checkbox',
                    values: { false: 'خیر', true: 'بله' },
                },
                Description: {
                    title: 'توضیحات',
                    width: '15%',
                    type: 'textarea'
                },
                Link: {
                    title: 'لینک',
                    width: '4%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '4%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region Internal Seminars
    initInternalSeminars: function () {
        $('#internalseminars_crud').jtable({
            title: 'لیست سمینارها و کنگره های داخلی',
            paging: true, //Enable paging
            pageSize: 20, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'رکورد مورد نظر حذف شود؟';
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                    .selectize({
                        dropdownParent: 'body',
                        placeholder: 'Click here to select ...',
                        onDropdownOpen: function ($dropdown) {
                            $dropdown
                                .hide()
                                .velocity('slideDown', {
                                    begin: function () {
                                        $dropdown.css({ 'margin-top': '0' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        },
                        onDropdownClose: function ($dropdown) {
                            $dropdown
                                .show()
                                .velocity('slideUp', {
                                    complete: function () {
                                        $dropdown.css({ 'margin-top': '' })
                                    },
                                    duration: 200,
                                    easing: easing_swiftOut
                                })
                        }
                    });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                        .on('ifChecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('بله');
                            $this.prop('checked', true).attr('value', 'true');
                        })
                        .on('ifUnchecked', function (event) {
                            $this.parent('div.icheckbox_md').next('span').text('خیر');
                            $this.prop('checked', false).attr('value', 'false');
                        })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                $('input[name="Order"]').attr('data-parsley-type', 'number');
                $('input[name="Link"]').attr('data-parsley-pattern', '((http|ftp|https):\/{2})+[^]*');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input")) {
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
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: function (data) {
                    var deferred = $.Deferred();

                    // Capture form submit result from the hidden iframe
                    $("#postiframe").load(function () {
                        iframeContents = $("#postiframe").contents().find("body").text();
                        var result = $.parseJSON(iframeContents);
                        deferred.resolve(result);
                    });

                    // Submit form with file upload settings
                    var form = $('#jtable-create-form');
                    form.unbind('submit');
                    form.attr('action', $('#variables').data('createUrl'));
                    form.attr('method', 'post');
                    form.attr('enctype', 'multipart/form-data');
                    form.attr('encoding', 'multipart/form-data');
                    form.attr('target', 'postiframe');
                    form.submit();

                    return deferred;
                },
                updateAction: function (data) {
                    var deferred = $.Deferred();

                    // Capture form submit result from the hidden iframe
                    $("#postiframe").load(function () {
                        iframeContents = $("#postiframe").contents().find("body").text();
                        var result = $.parseJSON(iframeContents);
                        deferred.resolve(result);
                    });

                    // Submit form with file upload settings
                    var form = $('#jtable-edit-form');
                    form.unbind('submit');
                    form.attr('action', $('#variables').data('updateUrl'));
                    form.attr('method', 'post');
                    form.attr('enctype', 'multipart/form-data');
                    form.attr('encoding', 'multipart/form-data');
                    form.attr('target', 'postiframe');
                    form.submit();

                    return deferred;
                },
                deleteAction: $('#variables').data('deleteUrl')
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function () {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('#externalarticles_crud').jtable('reload');
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان',
                    width: '28%',
                },
                Authors: {
                    title: 'نویسندگان',
                    width: '15%',
                },
                Conference: {
                    title: 'نام کنفرانس',
                    width: '10%',
                },
                Date: {
                    title: 'زمان',
                    width: '6%',
                },
                Abstract: {
                    title: 'چکیده',
                    width: '5%',
                    type: 'textarea',
                    display: function (data) {
                        if (data.record.Abstract) {
                            var newId = guid();
                            return '<a href="javascript:void(0)" data-uk-modal="{target:\'#' + newId + '\'}">[چکیده]</a><div data-abstract-container class="uk-modal" id="' + newId + '" aria-hidden="true"> <div class="uk-modal-dialog"> <div class="uk-modal-header"> <h3 class="uk-modal-title">چکیده</span></h3> </div> <p class="uk-text-left" style="white-space: pre-wrap;" data-abstract>' + data.record.Abstract.replace(/(?:\r\n|\r|\n)/g, '<br />') + '</p> <div class="uk-modal-footer uk-text-left"> <button type="button" class="md-btn md-btn-flat uk-modal-close">بستن</button> </div> </div> </div>';
                        }
                    }
                },
                FileInput: {
                    title: 'فایل (اختیاری)',
                    list: false,
                    input: function (data) {
                        return '<div class="uk-form-file md-btn md-btn-primary" style="margin-right:5px;"> انتخاب فایل <input id="file" name="file" type="file"/><iframe name="postiframe" id="postiframe" style="display: none" /> </div>';
                    }
                },
                FileText: {
                    title: 'فایل',
                    width: '3%',
                    listClass: 'uk-text-center',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (data.record.FileText) {
                            return '<a href="' + $('#variables').data('getfileUrl') + '?Type=6&FileText=' + data.record.FileText + '"><i style="cursor:pointer;font-size:22px;" class="material-icons">file_download</i></a>';
                        }
                    }
                },
                DeleteFile: {
                    title: 'فایل حذف شود؟ (در صورت موجود بودن)',
                    list: false,
                    create: false,
                    defaultValue: false,
                    type: 'checkbox',
                    values: { false: 'خیر', true: 'بله' },
                },
                Description: {
                    title: 'توضیحات',
                    width: '15%',
                    type: 'textarea'
                },
                Link: {
                    title: 'لینک',
                    width: '4%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Link) {
                            return '<a href="' + data.record.Link + '" target="_blank" rel="noreferrer">[لینک]</a>';
                        }
                    }
                },
                Order: {
                    title: 'اولویت قرارگیری',
                    width: '4%',
                    inputClass: 'english-input',
                    display: function (data) {
                        if (data.record.Order) {
                            return '<span class="persian-number">' + data.record.Order + '</span>';
                        }
                    }
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
    // #region News
    initNews: function () {
        $('#news_crud').jtable({
            title: 'لیست اخبار',
            paging: true, //Enable paging
            pageSize: 20, //Set page size (default: 10)
            addRecordButton: $('#recordAdd'),
            deleteConfirmation: function (data) {
                data.deleteConfirmMessage = 'خبر مورد نظر حذف شود؟';
            },
            actions: {
                listAction: $('#variables').data('listUrl'),
                createAction: $('#variables').data('createUrl'),
                updateAction: $('#variables').data('updateUrl'),
                deleteAction: $('#variables').data('deleteUrl')
            },
            formCreated: function (event, data) {
                // replace click event on some clickable elements
                // to make icheck label works
                data.form.find('.jtable-option-text-clickable').each(function () {
                    var $thisTarget = $(this).prev().attr('id');
                    $(this)
                        .attr('data-click-target', $thisTarget)
                        .off('click')
                        .on('click', function (e) {
                            e.preventDefault();
                            $('#' + $(this).attr('data-click-target')).iCheck('toggle');
                        })
                });
                // create selectize
                data.form.find('select').each(function () {
                    var $this = $(this);
                    $this.after('<div class="selectize_fix"></div>')
                        .selectize({
                            dropdownParent: 'body',
                            placeholder: 'Click here to select ...',
                            onDropdownOpen: function ($dropdown) {
                                $dropdown
                                    .hide()
                                    .velocity('slideDown', {
                                        begin: function () {
                                            $dropdown.css({ 'margin-top': '0' })
                                        },
                                        duration: 200,
                                        easing: easing_swiftOut
                                    })
                            },
                            onDropdownClose: function ($dropdown) {
                                $dropdown
                                    .show()
                                    .velocity('slideUp', {
                                        complete: function () {
                                            $dropdown.css({ 'margin-top': '' })
                                        },
                                        duration: 200,
                                        easing: easing_swiftOut
                                    })
                            },
                            //onChange: function (value) {
                            //    $('select[name="Name"]').val(value);
                            //    $(data.form).parsley().validate();
                            //}
                        });
                });
                // create icheck
                data.form
                    .find('input[type="checkbox"],input[type="radio"]')
                    .each(function () {
                        var $this = $(this);
                        $this.iCheck({
                            checkboxClass: 'icheckbox_md',
                            radioClass: 'iradio_md',
                            increaseArea: '20%'
                        })
                            .on('ifChecked', function (event) {
                                $this.parent('div.icheckbox_md').next('span').text('بله');
                                //$this.prop('checked', true);
                                $this.attr('value', true);
                            })
                            .on('ifUnchecked', function (event) {
                                $this.parent('div.icheckbox_md').next('span').text('خیر');
                                //$this.prop('checked', false);
                                $this.attr('value', false);
                            })
                    });
                // reinitialize inputs
                data.form.find('.jtable-input').children('input[type="text"],input[type="password"],textarea').not('.md-input').each(function () {
                    $(this).addClass('md-input');
                    altair_forms.textarea_autosize();
                });
                altair_md.inputs();

                $('input[name="Title"]').attr('data-parsley-required', 'true');
                $('textarea[name="Content"]').attr('data-parsley-required', 'true');

                $(data.form).parsley({
                    excluded: "input[type=button], input[type=submit], input[type=reset], input[type=hidden], input.exclude_validation",
                    trigger: "change focusin blur",
                    errorsWrapper: '<div class="parsley-errors-list"></div>',
                    errorTemplate: "<span></span>",
                    errorClass: "md-input-danger",
                    successClass: "md-input-success",
                    errorsContainer: function (e) {
                        if (e.$element.is("input") || e.$element.is("textarea")) {
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
            },
            formSubmitting: function (event, data) {
                var $form = $(data.form);
                $form.parsley().validate();

                if (!$form.parsley().isValid()) {
                    return false;
                }
            },
            recordsLoaded: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowInserted: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            rowUpdated: function (event, data) {
                $('.persian-number').each(function () {
                    $(this).text(persianJs($(this).text()).englishNumber().toString());
                });
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Title: {
                    title: 'عنوان خبر',
                    width: '20%'
                },
                Content: {
                    title: 'متن خبر',
                    width: '70%',
                    type: 'textarea',
                    inputClass: 'full-width'
                },
                CreateDateText: {
                    title: 'تاریخ درج',
                    width: '10%',
                    create: false,
                    edit: false
                }
            }
        }).jtable('load');

        // change buttons visual style in ui-dialog
        $('.ui-dialog-buttonset')
            .children('button')
            .attr('class', '')
            .addClass('md-btn md-btn-flat')
            .off('mouseenter focus');
        $('#AddRecordDialogSaveButton,#EditDialogSaveButton,#DeleteDialogButton').addClass('md-btn-flat-primary');
    },
    // #endregion
};

if ($('#users_crud').length > 0) {
    $('#admin-users-search').click(function (e) {
        e.preventDefault();
        $('#users_crud').jtable('load', {
            filterfirstname: $('#filter-firstname').val(),
            filterlastname: $('#filter-lastname').val(),
            filteremail: $('#filter-email').val(),
            filterpageid: $('#filter-pageid').val(),
            filterrole: $('#filter-role option:selected').val(),
            filterisbanned: $('#filter-isbanned').prop('checked'),
            filterissoftdelete: $('#filter-issoftdelete').prop('checked')
        });
    });

    $('#admin-users-refresh').click(function (e) {
        e.preventDefault();
        $('#users_crud').jtable('load');
    });
}

if ($('#activitylogs_crud').length > 0) {
    $('#admin-logs-search').click(function (e) {
        e.preventDefault();
        $('#activitylogs_crud').jtable('load', {
            filteremail: $('#filter-email').val(),
        });
    });

    $('#admin-logs-refresh').click(function (e) {
        e.preventDefault();
        $('#activitylogs_crud').jtable('load');
    });
}

if ($('#manageradminmessages_crud').length > 0) {
    $('#admin-adminmessages-search').click(function (e) {
        e.preventDefault();
        $('#manageradminmessages_crud').jtable('load', {
            filterlastname: $('#filter-lastname').val(),
            filterpageid: $('#filter-pageid').val(),
            filteremail: $('#filter-email').val(),
            filterpendingonly: $('#filter-pending-only').prop('checked')
        });
    });

    $('#admin-adminmessages-refresh').click(function (e) {
        e.preventDefault();
        $('#manageradminmessages_crud').jtable('load');
    });
}

if ($('#citations_crud').length > 0) {
    $('#admin-citations-search').click(function (e) {
        e.preventDefault();
        $('#citations_crud').jtable('load', {
            filterlastname: $('#filter-lastname').val(),
            filterpageid: $('#filter-pageid').val(),
            filteremail: $('#filter-email').val(),
        });
    });

    $('#admin-citations-refresh').click(function (e) {
        e.preventDefault();
        $('#citations_crud').jtable('load');
    });
}

if ($('#weeklyprograms_crud').length > 0) {
    $(document).on('change', '#DayOfProgram', function (e) {
        e.preventDefault();
        $('#weeklyprograms_crud').jtable('load', {
            filterDayOfWeek: $(this).val()
        });
    });

    $(document).ready(function () {
        $('#DayOfProgram-selectized').parents('.selectize-control').css('display', 'inline-block');

        try
        {
            $('input[name="startdate"]').pDatepicker({
                format: 'YYYY/MM/DD',
                autoClose: true
                //observer: true
            });
        }
        catch(e){}

        try
        {
            $('input[name="enddate"]').pDatepicker({
                format: 'YYYY/MM/DD',
                autoClose: true
                //observer: true
            });
        }
        catch(e){}
        
        if (startDate.trim().length > 0) {
            var date = persianNumberToEnglishNumber(startDate.trim());

            try {
                $('input[name="startdate"]').pDatepicker("setDate", [parseInt(date.split('/')[0]), parseInt(date.split('/')[1]), parseInt(date.split('/')[2]), 12, 0]);
            }
            catch (err) { }

            $('input[name="startdate"]').val(date);
        }
        else {
            $('input[name="startdate"]').val('');
        }

        if (endDate.trim().length > 0) {
            var date = persianNumberToEnglishNumber(endDate.trim());

            try {
                $('input[name="enddate"]').pDatepicker("setDate", [parseInt(date.split('/')[0]), parseInt(date.split('/')[1]), parseInt(date.split('/')[2]), 12, 0]);
            }
            catch (err) { }

            $('input[name="enddate"]').val(date);
        }
        else {
            $('input[name="enddate"]').val('');
        }

        $('.md-input').focusin().focusout();
    });
}

if ($('#externalarticles_crud').length > 0) {
    $('#professor-externalarticles-search').click(function (e) {
        e.preventDefault();
        $('#externalarticles_crud').jtable('load', {
            filtertitle: $('#filter-title').val(),
        });
    });

    $('#professor-externalarticles-refresh').click(function (e) {
        e.preventDefault();
        $('#externalarticles_crud').jtable('load');
    });
}

if ($('#externalseminars_crud').length > 0) {
    $('#professor-externalseminars-search').click(function (e) {
        e.preventDefault();
        $('#externalseminars_crud').jtable('load', {
            filtertitle: $('#filter-title').val(),
        });
    });

    $('#professor-externalseminars-refresh').click(function (e) {
        e.preventDefault();
        $('#externalseminars_crud').jtable('load');
    });
}

if ($('#internalarticles_crud').length > 0) {
    $('#professor-internalarticles-search').click(function (e) {
        e.preventDefault();
        $('#internalarticles_crud').jtable('load', {
            filtertitle: $('#filter-title').val(),
        });
    });

    $('#professor-internalarticles-refresh').click(function (e) {
        e.preventDefault();
        $('#internalarticles_crud').jtable('load');
    });
}

if ($('#internalseminars_crud').length > 0) {
    $('#professor-internalseminars-search').click(function (e) {
        e.preventDefault();
        $('#internalseminars_crud').jtable('load', {
            filtertitle: $('#filter-title').val(),
        });
    });

    $('#professor-internalseminars-refresh').click(function (e) {
        e.preventDefault();
        $('#internalseminars_crud').jtable('load');
    });
}

//$(document).ready(function () {
//    $('div[data-abstract-container]').on({
//        'show.uk.modal': function () {
//            alert('asdasd');
//            var $container = $(this).find('p[data-abstract]');
//            //var content = replaceBreaksForTextarea($container.html());
//            $container.html('').html(content);
//        }
//    });
//});
