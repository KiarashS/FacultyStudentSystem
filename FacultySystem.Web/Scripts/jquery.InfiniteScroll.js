//// <![CDATA[
//(function ($) {
//    $.fn.InfiniteScroll = function (options) {
//        var defaults = {
//            moreInfoDiv: '#MoreInfoDiv',
//            progressDiv: '#Progress',
//            loadInfoUrl: '/',
//            loginUrl: '/login',
//            errorHandler: null,
//            completeHandler: null,
//            noMoreInfoHandler: null,
//            filterFirstnameId: "#filter-firstname",
//            filterLastnameId: "#filter-lastname",
//            filterEmailId: "#filter-email",
//            filterCollegeId: "#filter-college",
//            filterEducationalGroupId: "#filter-educational-group",
//            mainNonAjaxContentDivId: "#mainNonAjaxContent",
//            paramName: "",
//            pageName: "صفحه"
//        };
//        var options = $.extend(defaults, options);

//        var showProgress = function () {
//            $(options.progressDiv).removeClass('uk-hidden');
//        }

//        var hideProgress = function () {
//            $(options.progressDiv).addClass('uk-hidden');
//        }

//        var clearArea = function () {
//            $(options.moreInfoDiv).html("");
//            $(options.mainNonAjaxContentDivId).html("");
//            window.scrollTo(0, 0);
//        }

//        return this.each(function () {
//            var moreInfoButton = $(this);
//            var searchButton = $('#users-search');
//            var refreshButton = $('#users-refresh');
//            var page = 1;
//            var title = document.title;
//            var updatePath = function () {
//                //if (!$(options.pagerSortById).val()) {
//                //    return;
//                //}

//                var path = "#/page/" + (page + 1) + "/" + $(options.filterFirstnameId).val() + "/" + $(options.filterLastnameId).val() + "/" + $(options.filterEmailId).val() + "/" + $(options.filterCollegeId).val() + "/" + $(options.filterEducationalGroupId).val();
//                try {
//                    history.pushState({}, "", path);
//                }
//                catch (ex) {
//                    window.location.hash = path;
//                }
//                document.title = title + " / " + options.pageName + " " + (page + 1);
//            }

//            var clickFn = function (moreInfoButton) {
//                moreInfoButton.parent().hide();
//                showProgress();
//                var filterFirstname = $(options.filterFirstnameId).val();
//                var filterLastname = $(options.filterLastnameId).val();
//                var filterEmail = $(options.filterEmailId).val();
//                var filterCollege = $(options.filterCollegeId).val();
//                var filterEducationalGroup = $(options.filterEducationalGroupId).val();

//                $.ajax({
//                    type: "POST",
//                    url: options.loadInfoUrl,
//                    data: JSON.stringify({ page: page, firstname: filterFirstname, lastname: filterLastname, email: filterEmail, college: filterCollege, educationalgroup: filterEducationalGroup, name: options.paramName }),
//                    contentType: "application/json; charset=utf-8",
//                    dataType: "json",
//                    complete: function (xhr, status) {
//                        var data = xhr.responseText;
//                        if (xhr.status == 403) {
//                            window.location = options.loginUrl;
//                        }
//                        else if (status === 'error' || !data) {
//                            if (options.errorHandler)
//                                options.errorHandler(this);
//                        }
//                        else {
//                            if (data == "no-more-info") {
//                                hideProgress();
//                                moreInfoButton.parent().hide();
//                                if (options.noMoreInfoHandler)
//                                    options.noMoreInfoHandler(this);
//                            }
//                            else {
//                                var $boxes = $(data);
//                                var appendEl = $(options.moreInfoDiv).append(data);
//                                updatePath();

//                                hideProgress();
//                                moreInfoButton.parent().show();
//                                if (options.completeHandler)
//                                    options.completeHandler(appendEl, $boxes);
//                            }
//                            page++;
//                        }
//                    }
//                });
//            }

//            $(document).on('click', '#users-search', function () {
//                var path = "#/page/1/" + $(options.filterFirstnameId).val() + "/" + $(options.filterLastnameId).val() + "/" + $(options.filterEmailId).val() + "/" + $(options.filterCollegeId).val() + "/" + $(options.filterEducationalGroupId).val();
//                window.location.hash = path;
//                Path.listen();
//            })

//            $(document).on('click', '#users-refresh', function () {
//                window.location.hash = '#/page/1';
//                Path.listen();
//            })

//            Path.map("#/page(/:page)(/:firstname)(/:lastname)(/:email)(/:college)(/:educationalGroup)").to(function () {
//                var firstname = this.params['firstname'] || null;
//                var lastname = this.params['lastname'] || null;
//                var email = this.params['email'] || null;
//                var college = this.params['college'] || 1;
//                var educationalGroup = this.params['educationalGroup'] || 1;
//                var urlPage = parseInt(this.params['page'], 10);
//                if (urlPage == page &&
//                    firstname == $(options.filterFirstnameId).val() &&
//                    lastname == $(options.filterLastnameId).val() &&
//                    email == $(options.filterEmailId).val() &&
//                    college == $(options.filterCollegeId).val() &&
//                    educationalGroup == $(options.filterEducationalGroupId).val()) 
//                {                    
//                    return;
//                }

//                $(options.filterFirstnameId).val(firstname);
//                $(options.filterLastnameId).val(lastname);
//                $(options.filterEmailId).val(email);
//                $(options.filterCollegeId).val(college);
//                $(options.filterEducationalGroupId).val(educationalGroup);
//                page = urlPage - 1;
//                clearArea();
//                clickFn(moreInfoButton);
//            });
//            Path.root("#/page/1");
//            //if ($(options.pagerSortById).val()) {
//            //    Path.listen();
//            //}

//            Path.listen();

//            //$(options.pagerSortById + "," + options.pagerSortOrderId).change(function () {
//            //    page = 0;
//            //    clearArea();
//            //    $(moreInfoButton).click();
//            //});

//            $(moreInfoButton).click(function (event) {
//                if (event.originalEvent === undefined) {
//                    // triggered by code
//                    page = 0;
//                }
//                clickFn(moreInfoButton);
//            });
//        });
//    };
//})(jQuery);
//// ]]>