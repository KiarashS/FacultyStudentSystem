function SetPersianNumber() {
    $('.persian-number').each(function() {
        if (!$(this).text() || $(this).text() === '')
        {
            return;
        }
        $(this).text(persianJs($(this).text()).englishNumber().toString());
    });
}

$(document).ajaxComplete(function( event, xhr, settings ) {
    if (xhr.status == 401 || xhr.status == 403) {
        document.location = '/login';
    }
});

function logout() {window.location.href = '/login/logout';}

function loadImage(path, logintarget, forgottarget) {
    //console.log(path);
    //console.log(logintarget);
    //console.log(forgottarget);
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
    if ($('#material-scrolltop-btn').length > 0) {
        $('body').materialScrollTop();
    }

    //window.Parsley.on('form:submit', function (instance) {
    //    instance.submitEvent.preventDefault();
    //    return false;
    //});

    SetPersianNumber();
    
    // index page
    if ($('#users_index').length > 0)
    {
        //$("#moreInfoButton").InfiniteScroll({
        //    moreInfoDiv: '#MoreInfoDiv',
        //    progressDiv: '#ProgressDiv',
        //    loadInfoUrl: $('#variables').data('pagedIndexUrl'),
        //    loginUrl: '/login',
        //    errorHandler: function () {
        //        UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
        //    },
        //    completeHandler: function () {
                
        //    },
        //    noMoreInfoHandler: function () {
        //        UIkit.notify("اطلاعات بيشتري يافت نشد.", { status: 'warning' });
        //    }
        //});
        title = document.title;
        $('#mainNonAjaxContent').find('.md-list').matchHeight({byRow: false});
        // fix footer margin-top in index page
        $('footer').css({ marginTop: '15px' });

        $(document).on('click', '#moreInfoButton', function () {
            var $moreInfoButton = $(this),
                $progress = $('#ProgressDiv');

            $progress.show();
            $moreInfoButton.parent().hide();

            $.ajax({
                type: "POST",
                url: $('#variables').data('pagedIndexUrl'),
                data: JSON.stringify({ page: parseInt(currentPage, 10) + 1, firstname: filterFirstname, lastname: filterLastname, email: filterEmail, college: filterCollege, educationalgroup: filterEducationalGroup }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                complete: function (xhr, status) {
                    var data = xhr.responseText;
                    if (xhr.status == 403) {
                        window.location = '/login';
                    }
                    else if (status === 'error' || !data) {
                        $progress.hide();
                        $moreInfoButton.parent().show();
                        UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
                    }
                    else {
                        if (data == "no-more-info") {
                            $moreInfoButton.parent().hide();
                            $progress.hide();
                            UIkit.notify("اطلاعات بيشتري يافت نشد.", { status: 'warning' });
                            if ($('.card-number').length == 0) {
                                $('#no-item').show();
                            }
                        }
                        else {
                            var $boxes = $(data);
                            var $moreInfoDiv = $('#MoreInfoDiv');
                            var cardsNumber = $boxes.filter('.card-number').length;
                            var appendEl = $moreInfoDiv.append(data);
                            $moreInfoDiv.find('.md-list').slice(cardsNumber * -1).matchHeight({byRow: false});
                            SetPersianNumber();
                            $('#MoreInfoDiv').find('.card-number').slice(cardsNumber * -1).each(function (i) {
                                $(this).delay((i++) * 200).fadeTo(1000, 1);
                            });
                            $moreInfoDiv.trigger('display.uk.check');
                            document.title = title + ' / صفحه ' + (parseInt(currentPage, 10) + 1);
                            $("html, body").animate({ scrollTop: $moreInfoDiv.find('.card-number').slice(cardsNumber * -1).offset().top - 20 }, 1500);
                            $('#no-item').hide();

                            $progress.hide();
                            $moreInfoButton.parent().show();
                        }
                    }
                }
            });
        });

        $(document).on('click', '#users-search', function () {
            var firstname = $('#filter-firstname').val();
            var lastname = $('#filter-lastname').val();
            var email = $('#filter-email').val();
            var college = $('#filter-college').val();
            var educationalGroup = $('#filter-educational-group').val();

            if (firstname.trim().length == 0 &&
                lastname.trim().length == 0 &&
                email.trim().length == 0 &&
                college.trim() == '1' &&
                educationalGroup.trim() == '1')
            {
                UIkit.notify("جهت جستجو حداقل یکی از فیلد ها را تکمیل نمائید.", { status: 'warning' });
                return;
            }

            altair_helpers.content_preloader_show('md');

            $.ajax({
                type: "POST",
                url: $('#variables').data('pagedIndexUrl'),
                data: JSON.stringify({ page: 0, firstname: firstname, lastname: lastname, email: email, college: college, educationalgroup: educationalGroup }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                complete: function (xhr, status) {
                    var data = xhr.responseText;
                    if (xhr.status == 403) {
                        window.location = '/login';
                    }
                    else if (status === 'error' || !data) {
                        altair_helpers.content_preloader_hide();
                        UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
                    }
                    else {
                        if (data == "no-more-info") {
                            UIkit.notify("جستجوی شما نتیجه ای نداشت!", { status: 'warning' });
                            $('#ProgressDiv').hide();
                            if ($('.card-number').length == 0) {
                                $('#no-item').show();
                            }
                        }
                        else {
                            $('#MoreInfoDiv').html('');
                            $('#mainNonAjaxContent').html('').hide();
                            window.scrollTo(0, 0);

                            var $boxes = $(data);
                            var $moreInfoDiv = $('#MoreInfoDiv');
                            var cardsNumber = $boxes.filter('.card-number').length;
                            var appendEl = $moreInfoDiv.append(data);
                            $moreInfoDiv.find('.md-list').slice(cardsNumber * -1).matchHeight({byRow: false});
                            SetPersianNumber();
                            $('#MoreInfoDiv').find('.card-number').slice(cardsNumber * -1).each(function (i) {
                                $(this).delay((i++) * 200).fadeTo(1000, 1);
                            });
                            $moreInfoDiv.trigger('display.uk.check');
                            document.title = title;
                            window.scrollTo(0, 0);
                            $('#no-item').hide();

                            $('#ProgressDiv').hide();
                            $('#moreInfoButton').parent().show();
                        }
                        altair_helpers.content_preloader_hide();
                    }
                }
            });
        });

        $(document).on('click', '#users-refresh', function () {
            altair_helpers.content_preloader_show('md');

            $.ajax({
                type: "POST",
                url: $('#variables').data('pagedIndexUrl'),
                data: JSON.stringify({ page: 0 }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                complete: function (xhr, status) {
                    var data = xhr.responseText;
                    if (xhr.status == 403) {
                        window.location = '/login';
                    }
                    else if (status === 'error' || !data) {
                        altair_helpers.content_preloader_hide();
                        UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
                    }
                    else {
                        if (data == "no-more-info") {
                            UIkit.notify("متاسفانه هیچ اطلاعاتی يافت نشد.", { status: 'warning' });
                            $('#ProgressDiv').hide();
                            $('#moreInfoButton').parent().hide();
                            if ($('.card-number').length == 0) {
                                $('#no-item').show();
                            }
                        }
                        else {
                            $('#MoreInfoDiv').html('');
                            $('#mainNonAjaxContent').html('').hide();
                            window.scrollTo(0, 0);

                            var $boxes = $(data);
                            var $moreInfoDiv = $('#MoreInfoDiv');
                            var cardsNumber = $boxes.filter('.card-number').length;
                            var appendEl = $moreInfoDiv.append(data);
                            $moreInfoDiv.find('.md-list').slice(cardsNumber * -1).matchHeight({byRow: false});
                            SetPersianNumber();
                            $('#MoreInfoDiv').find('.card-number').slice(cardsNumber * -1).each(function (i) {
                                $(this).delay((i++) * 200).fadeTo(1000, 1);
                            });
                            $moreInfoDiv.trigger('display.uk.check');
                            document.title = title;
                            window.scrollTo(0, 0);
                            $('#no-item').hide();

                            $('#ProgressDiv').hide();
                            $('#moreInfoButton').parent().show();
                        }
                        altair_helpers.content_preloader_hide();
                    }
                }
            });
        });
    }

    $(document).on('click', 'a[data-getinfo-fullurl]', function () {
        var $btn = $(this),
            $ajaxContainer = $('#ajax-container'),
            $ajaxResult = $('#ajax-result');

        altair_helpers.content_preloader_show('md', 'info', 'body', 48, 48);
        $ajaxContainer.hide()
        $ajaxResult.html('');

        $.ajax({
            type: "POST",
            url: $btn.data('getinfoFullurl'),
            // controller is returning the json data
            complete: function (xhr, status) {
                var data = xhr.responseText;
                if (status === 'error')
                {
                    UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
                }
                else if (!data)
                {
                    UIkit.notify("هیچ داده ای جهت نمایش موجود نیست!", { status: 'info' });
                }
                else
                {
                    $ajaxResult.html(data);
                    SetPersianNumber();
                    $ajaxContainer.trigger('display.uk.check');
                    $ajaxContainer.slideDown()
                    $('html, body').animate({
                        scrollTop: $("#ajax-container").offset().top - 77
                    }, 1000);
                }
                altair_helpers.content_preloader_hide();
            }
        });
    });


    $(document).on('click', 'a[data-loadmore-button]', function () {
        var $btn = $(this),
            $ajaxContainer = $btn.parents('[data-loadmore-container]'),
            $ajaxResult = $ajaxContainer.children('[data-loadmore-result]'),
            $ajaxLoading = $ajaxContainer.children('[data-loadmore-loading]'),
            currentPage = $btn.data('currentPage'),
            nextPage = parseInt(currentPage, 10) + 1;

        //altair_helpers.content_preloader_show('md', 'info', 'body', 48, 48);
        $btn.hide()
        $ajaxLoading.show();

        $.ajax({
            type: "POST",
            url: $btn.data('listUrl') + '/' + nextPage,
            // controller is returning the json data
            complete: function (xhr, status) {
                var data = xhr.responseText;
                if (status === 'error') {
                    UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
                    $btn.show()
                }
                else if (!data || data == 'no-more-info') {
                    UIkit.notify("هیچ داده ای جهت نمایش موجود نیست!", { status: 'warning' });
                    $btn.remove();
                }
                else {
                    $ajaxResult.append(data);
                    SetPersianNumber();
                    $ajaxContainer.trigger('display.uk.check');
                    $btn.show()
                    $btn.data('currentPage', nextPage);
                    //$ajaxContainer.slideDown()
                    //$('html, body').animate({
                    //    scrollTop: $("#ajax-container").offset().top - 77
                    //}, 1000);
                }
                $ajaxLoading.hide();
                //altair_helpers.content_preloader_hide();
            }
        });
    });
});

//(function () {
//    UIkit.on('beforeready.uk.dom', function () {

//        UIkit.plugin("lightbox", "video", {

//            init: function (lightbox) {

//                lightbox.on("showitem.uk.lightbox", function (e, data) {

//                    var resolve = function (source, width, height) {

//                        data.meta = {
//                            'content': '<video class="uk-responsive-width" src="' + source + '" width="' + width + '" height="' + height + '" controls autoplay></video>',
//                            'width': width,
//                            'height': height
//                        };

//                        data.type = 'video';

//                        data.promise.resolve();
//                    };

//                    if (data.type == 'video' || data.source.match(/\.(mp4|webm|ogv)$/i)) {
//                        resolve(data.source, (lightbox.options.width || 800), (lightbox.options.height || 600));
//                    }
//                });

//            }
//        });
//    });
//}());

function drawScopusChart()
{
    if ($('#scopuschart').length == 0)
    {
        return;
    }

    var waypoint = $('#scopuschart').waypoint(function (direction) {
        var $chartContainer = $('#scopuschart');

        $.post($chartContainer.data('url'), function (data, textStatus) {
            if (textStatus == 'error') {
                UIkit.notify("در حال حاضر سرور قادر به پاسخگویی جهت بارگذاری نمودار نمی باشد ...", { status: 'danger' });
            }
            $chartContainer.find('.chart-loading').hide();

            Highcharts.chart('scopuschart', {
                chart: {
                    zoomType: 'xy',
                    resetZoomButton: {
                        position: {
                            align: 'left',
                            x: 100
                        }
                    },
                    style: {
                        "fontFamily": "WRoya, Tahoma, Arial, Helvetica, sans-serif", "fontSize": "12px"
                    }
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false,
                    allowHTML: true,
                    filename: 'Citations and Documents Chart in Scopus',
                },
                navigation: { 
                    menuItemStyle: { 'font-size': '14px'}
                },
                title: {
                    text: 'تعداد ارجاعات و مقالات در اسکاپوس'
                },
                xAxis: [{
                    categories: data.years,
                    crosshair: true,
                    crosshair: {
                        label: {
                            enabled: true
                        }
                    }
                }],
                yAxis: [{ // Primary yAxis
                    labels: {
                        format: '{value}',
                    },
                    title: {
                        text: 'ارجاعات'
                    },
                    crosshair: {
                        label: {
                            enabled: true
                        }
                    },
                    crosshair: true,
                    opposite: true,
                    offset: 5
                }, { // Secondary yAxis
                    labels: {
                        format: '{value}',
                    },
                    title: {
                        text: 'مقالات'
                    },
                    crosshair: {
                        label: {
                            enabled: true
                        }
                    },
                    crosshair: true,
                    offset: 5
                }],
                tooltip: {
                    useHTML: true,
                    shared: true,
                },
                legend: {
                    rtl: true,
                    floating: true,
                    y: 25,
                    x: 60,
                    symbolPadding: 40,
                    backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
                },
                series: [{
                    name: 'مقالات',
                    type: 'column',
                    yAxis: 1,
                    data: data.documents,
                    tooltip: {
                        valueSuffix: ' عدد'
                    },
                }, {
                    name: 'ارجاعات',
                    type: 'line',
                    data: data.citations,
                    tooltip: {
                        valueSuffix: ' عدد'
                    }
                }]
            });
        }, 'json');
        
        this.destroy();
    }, { offset: '55%' });
}

function drawGoogleChart()
{
    if ($('#googlechart').length == 0)
    {
        return;
    }

    var waypoint = $('#googlechart').waypoint(function (direction) {
        var $chartContainer = $('#googlechart');

        $.post($chartContainer.data('url'), function (data, textStatus) {
            if (textStatus == 'error') {
                UIkit.notify("در حال حاضر سرور قادر به پاسخگویی جهت بارگذاری نمودار نمی باشد ...", { status: 'danger' });
            }
            $chartContainer.find('.chart-loading').hide();
            
            Highcharts.chart('googlechart', {
                chart: {
                    zoomType: 'xy',
                    resetZoomButton: {
                        position: {
                            align: 'left',
                            x: 100
                        }
                    },
                    style: {
                        "fontFamily": "WRoya, Tahoma, Arial, Helvetica, sans-serif", "fontSize": "12px"
                    }
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false,
                    allowHTML: true,
                    filename: 'Citations and Documents Chart in Scopus',
                },
                navigation: { 
                    menuItemStyle: { 'font-size': '14px'}
                },
                title: {
                    text: 'تعداد ارجاعات در گوگل اسکولار'
                },
                xAxis: [{
                    categories: data.years,
                    crosshair: true,
                    crosshair: {
                        label: {
                            enabled: true
                        }
                    }
                }],
                yAxis: [{
                    labels: {
                        format: '{value}',
                    },
                    title: {
                        text: 'ارجاعات'
                    },
                    crosshair: {
                        label: {
                            enabled: true
                        }
                    },
                    opposite: true,
                    crosshair: true,
                    offset: 5
                }],
                tooltip: {
                    useHTML: true,
                    shared: true,
                },
                legend: {
                    rtl: true,
                    floating: true,
                    y: 25,
                    x: 60,
                    symbolPadding: 40,
                    backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
                },
                series: [{
                    name: 'ارجاعات',
                    type: 'column',
                    className: 'google-series',
                    data: data.citations,
                    tooltip: {
                        valueSuffix: ' عدد'
                    }
                }]
            });
        }, 'json');
        
        this.destroy();
    }, { offset: '55%' });
}

$(document).ready(function () {
    if ($('#scopuschart').length != 0 || $('#googlechart').length != 0) {
        Highcharts.setOptions({
            lang: {
                contextButtonTitle: 'منوی نمودار',
                downloadJPEG: 'دریافت تصویر JPEG',
                downloadPDF: 'دریافت به صورت PDF',
                downloadPNG: 'دریافت تصویر PNG',
                downloadSVG: 'دریافت تصویر SVG',
                drillUpText: 'بازگشت به {series.name}',
                loading: 'در حال بارگذاری ...',
                noData: 'هیچ داده ای جهت نمایش موجود نیست!',
                printChart: 'پرینت نمودار',
                resetZoom: 'تنظیم مجدد بزرگنمایی',
                resetZoomTitle: "تنظیم مجدد بزرگنمایی به سطح 1:1"
            }
        });
        drawScopusChart();
        drawGoogleChart();
    }
});

$(document).on('click', 'a[data-call-ajax=true]', function (e) {
    e.preventDefault();
    var $this = $(this);
    altair_helpers.content_preloader_show('md');

    $.get($this.data('url'), function (data, textStatus, jqXHR) {
        altair_helpers.content_preloader_hide();
        if (jqXHR.status !== 200)
        {
            UIkit.notify("در حال حاضر سرور قادر به پاسخگویی نمی باشد ...", { status: 'danger' });
            return;
        }

        $($this.data('containerId')).html(data);
    });

    return false;
});


if ($('#registration-form').length > 0) {
    $(document).ready(function () {
        loadImage($('#captcha-url').data('captchaUrl'), '[data-registration-captcha-container]');
    });

    $(document).on('click', '.captcha-container', function () {
        var captchaUrl = $('#captcha-url').data('captchaUrl').split('=')[0] + '=';
        var date = new Date();
        var ticks = ((date.getTime() * 10000) + 621355968000000000);

        loadImage(captchaUrl + ticks, '[data-registration-captcha-container]');
    });

    $(document).on('click', '.captcha-refresh a', function (e) {
        e.preventDefault();
        $('.captcha-container:eq(0)').trigger('click');
    });
}

if ($('#login-form').length > 0) {
    $(document).ready(function () {
        loadImage($('#captcha-url').data('captchaUrl'), '[data-login-captcha-container]');
    });

    $(document).on('click', '.captcha-container', function () {
        var captchaUrl = $('#captcha-url').data('captchaUrl').split('=')[0] + '=';
        var date = new Date();
        var ticks = ((date.getTime() * 10000) + 621355968000000000);

        loadImage(captchaUrl + ticks, '[data-login-captcha-container]');
    });

    $(document).on('click', '.captcha-refresh a', function (e) {
        e.preventDefault();
        $('.captcha-container:eq(0)').trigger('click');
    });
}

if ($('#forgottenpassword-form').length > 0) {
    $(document).ready(function () {
        loadImage($('#captcha-url').data('captchaUrl'), '[data-forgottenpassword-captcha-container]');
    });

    $(document).on('click', '.captcha-container', function () {
        var captchaUrl = $('#captcha-url').data('captchaUrl').split('=')[0] + '=';
        var date = new Date();
        var ticks = ((date.getTime() * 10000) + 621355968000000000);

        loadImage(captchaUrl + ticks, '[data-forgottenpassword-captcha-container]');
    });

    $(document).on('click', '.captcha-refresh a', function (e) {
        e.preventDefault();
        $('.captcha-container:eq(0)').trigger('click');
    });
}
