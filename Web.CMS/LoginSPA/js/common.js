(function($) {
    "use strict";
    var iOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;
    var isMobile = {
        Android: function() {
            return navigator.userAgent.match(/Android/i);
        },
        BlackBerry: function() {
            return navigator.userAgent.match(/BlackBerry/i);
        },
        iOS: function() {
            return navigator.userAgent.match(/iPhone|iPad|iPod/i);
        },
        Opera: function() {
            return navigator.userAgent.match(/Opera Mini/i);
        },
        Windows: function() {
            return navigator.userAgent.match(/IEMobile/i);
        },
        any: function() {
            return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
        }
    }

    //scrollBar
    function scrollBar() {
        var scrollContainer = $(".scrollbar-inner");
        if (scrollContainer.length > 0) {
            scrollContainer.scrollbar();
        }
    }
    //resizeSite
    function resizeSite() {
        var checkPoint = 1200,
            windowWidth = $(window).innerWidth();
        // mobile
        if (checkPoint > windowWidth) {

        } else {

        }
    }
    //magnificPopup
    function magnificPopup() {
        var currentPopup;
        $('.open-popup').magnificPopup({
            type: 'inline',
            midClick: true,
            closeOnBgClick: false, // tránh đóng khi người dùng nhấn vào vùng ngoài
            mainClass: 'mfp-with-zoom',
            fixedContentPos: false,
            fixedBgPos: true,
            overflowY: 'auto',
            closeBtnInside: true,
            preloader: false,
            removalDelay: 300,
        });
        $('#popup-tao-don-hang .open-popup').click(function() {
            e.preventDefault();
            $.magnificPopup.open({
                items: {
                    src: '#popup-chi-tiet-cong-no'
                },
                type: 'inline',
                midClick: true,
                callbacks: {
                    beforeOpen: function() {
                        this.st.mainClass = currentPopup.st.mainClass;
                    }
                }
            });
        });
    }
    //fixSticky
    // function fixStickyIE() {
    // 	var stickyElements = $('.sticky');
    // 	if (stickyElements.length > 0) {
    // 		Stickyfill.add(stickyElements);
    // 	}
    // }

    //OnCLick
    function onClickTongle() {
        $('.btn_show').click(function() {
            $('body').addClass('show-menu');
        });
        $('.close-menu,.close-menu2').click(function() {
            $('body').removeClass('show-menu');
        });

        $(".all-menu-tablet").click(function() {
            $(this).toggleClass("close-menu-tablet");
        });
        $(".onclick-toggle .toggle").click(function() {
            $(this).toggleClass("active");
        });
        $(".all-menu").click(function() {
            $(".mask-content").toggle();
            $(".head-top").toggleClass("show-all-menu");
        });
        $(".mask-content").click(function() {
            $(this).hide();
            $(".all-menu").removeClass("close-menu-tablet");
            $(".head-top").removeClass("show-all-menu");
        });


        $(".onclick-togle").click(function() {
            if (!$(this).hasClass("active")) {
                $(this).addClass("active");
                $(this).next('.expand').slideDown();
            } else {
                $(this).removeClass("active");
                $(this).next('.expand').slideUp();
            }
        });

        $(".lever1 .collapsed").click(function() {
            if (!$(this).hasClass("active")) {
                $(this).addClass("active");
                $(this).next('.expand').fadeIn();
            } else {
                $(this).removeClass("active");
                $(this).next('.expand').fadeOut();
            }
        });


        $('.user-hierarchy-ul').find('.onclick').click(function() {
            if (!$(this).parent('li').hasClass("active")) {
                $(this).parent('li').addClass("active");
                $(this).parent('li').find('.lever2').slideDown('fast');
            } else {
                $(this).parent('li').removeClass("active");
            }
        });

        $(".down-up .onclick").click(function() {
            if (!$(this).hasClass("active")) {
                $(this).addClass("active");
                $(this).next('.form-down').slideDown();
                $('.form-down input').focus();

            } else {
                $(this).removeClass("active");
                $(this).next('.form-down').slideUp();
            }
        });

        $(".dropdown").find(".dropbtn").click(function() {
            $(".dropdown").find('.dropdown-content').slideUp();
            if ($(this).next().css('display') == 'none') {
                $('.dropdown-content').slideUp();
                $(this).next().stop(true, true).slideDown();
                $('.dropdown').removeClass('active');
                $(this).parent().addClass('active');
                $(".form_suggest .input_search").focus();
            } else {
                $(this).parent().find('.dropdown-content').slideUp();
                $('.dropdown').removeClass('active');
            }
        });
        $(".dropdown-content").find("li").click(function() {
            var html = $(this).html();
            $(".dropbtn").html(html);
        });
        $(document).click(function() {
            $(".onclick-togle, .dropdown .dropbtn,.down-up .onclick").removeClass('active');
            $(".dropdown").find('.dropdown-content').slideUp();
            $(".form-down").slideUp();
        });
        $(".onclick-togle, .dropdown .dropbtn,.down-up .onclick, .dropdown-content .form-default, .dropdown-content .input_search").click(function(event) {
            event.stopPropagation();
        });

        $(".fillter-wrap .btn").click(function() {
            $('.fillter-search').slideDown();
        });
        $(".fillter-search .cancel").click(function() {
            $('.fillter-search').slideUp();
        });

        $(".open-today").click(function() {
            if (!$(this).hasClass("active")) {
                $(this).addClass("active");
                $(this).next('.list-today').addClass("show");

            } else {
                $(this).removeClass("active");
                $(this).next('.list-today').removeClass("show");
            }
        });
    }
    //
    function onClickTab() {
        $(".tab-default a").click(function(event) {
            // $(".tab-default a").removeClass("active")
            // if (!$(this).hasClass("active")) {
            //     $(this).addClass("active");
            // } else {
            //     $(this).removeClass("active");
            // }
            // var tab = $(this).attr("href");
            // $(".tab-content > div").not(tab).css("display", "none");
            // $(tab).fadeIn();
            event.preventDefault();
            let tabContainer = "#" + $(this).parent().data("tab-container");
            let target = $(this).data("lbl");

            $(this).addClass("active").siblings().removeClass("active");
            $(tabContainer + " .tab-content").hide();
            $(tabContainer + " .tab-content[data-lbl='" + target + "']").show();
        });

    }

    function datepicker() {
        // $('.datepicker-input').Zebra_DatePicker({
        // 	format: 'd/m/Y'
        // });
        $('input[name="datetimes"]').daterangepicker({
            timePicker: true,
            startDate: moment().startOf('hour'),
            endDate: moment().startOf('hour').add(32, 'hour'),
            locale: {
                format: 'M/DD hh:mm A'
            }
        });
        $('input[name="birthday"]').daterangepicker({
            singleDatePicker: true,
            showDropdowns: true,
            autoUpdateInput: false,
            locale: {
                cancelLabel: 'Clear'
            }
        });

        $('input[name="birthday"]').on('apply.daterangepicker', function(ev, picker) {
            $(this).val(picker.startDate.format('DD/MM/YYYY'));
        });

        $('input[name="birthday"]').on('cancel.daterangepicker', function(ev, picker) {
            $(this).val('');
        });
        // $('input[name="birthday"]').daterangepicker({
        // 	singleDatePicker: true,
        // 	showDropdowns: true,
        // 	minYear: 1901,
        // 	maxYear: parseInt(moment().format('YYYY'), 10)
        // }, function (start, end, label) {
        // 	var years = moment().diff(start, 'years');
        // });
    }

    // function select2() {
    //     $('.select2').select2();
    //     $('.select2--nosearch').select2({
    //         minimumResultsForSearch: -1
    //     });
    // }

    $(function() {
        scrollBar();
        magnificPopup();
        // fixStickyIE();
        onClickTongle();
        onClickTab();
      //  datepicker();
//        select2();
        $('[data-toggle="tooltip"]').tooltip({
            html: true
        });
    });

    $(window).on('load resize', function() {
        resizeSite()
    });

})(jQuery);