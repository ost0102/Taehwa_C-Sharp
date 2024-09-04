$(document).on('click', '.header-icon__list', function () {
    $('.header-icon__list').addClass('off').removeClass('on');
    $('.sub-main__cont').hide();
    $(this).removeClass('off').addClass('on');
    var index = $('.header-icon__list').index(this);
    $('.sub-main__cont').eq(index).show();

    var scrollDiv = $('.sub-header__icon');
    var item = $(this);
    var itemLeft = item.position().left + scrollDiv.scrollLeft();
    var itemWidth = item.outerWidth();
    var scrollDivWidth = scrollDiv.width();

    // 스크롤 위치를 아이템을 가운데로 오게 조정
    var scrollTo = itemLeft - (scrollDivWidth / 2) + (itemWidth / 2);

    // 최좌측 및 최우측 스크롤 제한
    var maxScroll = scrollDiv[0].scrollWidth - scrollDivWidth;
    scrollTo = Math.max(0, Math.min(scrollTo, maxScroll)); // 스크롤 범위 조정

    scrollDiv.animate({ scrollLeft: scrollTo }, 500);

    if (!$(this).hasClass('on')) {
        $('.header-icon__list').addClass('off').removeClass('on');
        $('.header-icon__list').children('.icon-img.on').hide();
        $('.header-icon__list').children('.icon-img.off').show();

        $(this).removeClass('off').addClass('on');
        $(this).children('.icon-img.off').hide();
        $(this).children('.icon-img.on').show();
    }
    $("#stx").val("");
    $("#btn_search").click();
});

$(document).on("click", "#hamburger", function () {
    $('.nav-mobile').toggleClass('show');
    $('#wrap').addClass('show');
    $('body').addClass('layer_on'); 
})
$(document).on("click", ".nav-mobile__header > p", function () {
    $('.nav-mobile').removeClass('show');
    $('#wrap').removeClass('show');
    $('body').removeClass('layer_on');
})
$(document).on('click', '#View-btn', function () {
    sessionStorage.setItem("notice", "Y");
    location.href = '/Recruitment';
});

$(document).on('click', '.service-type.sea', function () {
    sessionStorage.setItem("sea", "Y");
    location.href = '/Service';
});
$(document).on('click', '.service-type.air', function () {
    sessionStorage.setItem("air", "Y");
    location.href = '/Service';
});
$(document).on('click', '.service-type.inland', function () {
    sessionStorage.setItem("inland", "Y");
    location.href = '/Service';
});
$(document).on('click', '.service-type.house', function () {
    sessionStorage.setItem("house", "Y");
    location.href = '/Service';
});
$(document).on('click', '.service-type.chemicals', function () {
    sessionStorage.setItem("chemicals", "Y");
    location.href = '/Service';

    var scrollDiv = $('.sub-header__icon.service');
    var scrollWidth = scrollDiv[0].scrollWidth;
    scrollDiv.scrollLeft(scrollWidth);
});
$(document).on('click', '.service-type.trading', function () {
    sessionStorage.setItem("trading", "Y");
    location.href = '/Service';
});

$(document).on("click", ".lang__now", function () {    
    if ($(".select__lang").css("display") == "none") {
        $(".select__lang").slideDown(200);
        $(".lang__down").hide();
        $(".lang__up").show();
    } else {
        $(".select__lang").slideUp(200);
        $(".lang__down").show();
        $(".lang__up").hide();
    }
})


function _fnToNull(data) {
    // undifined나 null을 null string으로 변환하는 함수. 
    if (String(data) == 'undefined' || String(data) == 'null') {
        return ''
    } else {
        return data
    }
}


//setTimeout(function () {
//    window.location.href = '/Language/SetLang?strLang=KO'; // 한국어 페이지로 이동
//}, 86400000);

