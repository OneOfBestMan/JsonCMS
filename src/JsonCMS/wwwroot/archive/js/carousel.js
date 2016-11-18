mousex = 0;
mousey = 0;

$(document).ready(function () {
    $("div#makeMeScrollable").smoothDivScroll({
        mousewheelScrolling: true,
        manualContinuousScrolling: true,
        touchScrolling: true,
        mousewheelScrolling: true,
        visibleHotSpotBackgrounds: "always"
    });

    if (!isIE) {
        try {
            var c = 0;

            var containerHeight = $(window).height();
            var scrollBoxHeight = 612; //$(this).height();
            if (c == 0) {
                c = c + 1;

                $('div.scrollingHotSpotLeft').css('height', scrollBoxHeight);
                $('div.scrollingHotSpotRight').css('height', scrollBoxHeight);

                var mtop = (containerHeight - scrollBoxHeight) / 2 - 20;
                if (mtop > 40) {
                    $("#makeMeScrollable").css("margin-top", mtop);
                }

            }
            if (scrollBoxHeight > containerHeight) {
                $(this).css("height", containerHeight);
            }
            else {
                $("#makeMeScrollable").css("height", scrollBoxHeight);
            }

        }
        catch (err) {
            alert(err);
        }
    }
    else { /* IE */
        var containerHeight = $(window).height();
        var scrollBoxHeight = 612;
        var mtop = (containerHeight - scrollBoxHeight) / 2;
        if (mtop > 40) {
            $("#makeMeScrollable").css("margin-top", mtop);
        }
        $("img").css("height", "612px");
        $("#makeMeScrollable").css("height", "612px");
    }

    setTimeout(function () {
        $("#makeMeScrollable").css("visibility", "visible");
        $("#makeMeScrollable img").show();
    }, 500);
    

});

$(function () {

    $('#navigation_horiz').naviDropDown({
        dropDownWidth: '180px'
    });

});

/* idle function */

idleTimer = null;
idleState = false;
idleWait = 2500;

(function ($) {

    $(document).ready(function () {

        $('*').bind('mousemove keydown scroll', function (e) {

            mousex = e.pageX; //- gives you X position
            mousey = e.pageY  //- gives you Y position

            clearTimeout(idleTimer);

            if (idleState == true) {

                $('#navigation_horiz').slideDown('slow', function () { });
                $('.scrollingHotSpotLeft').fadeIn();
                $('.scrollingHotSpotRight').fadeIn();
                $('#icons').fadeIn();

            }

            idleState = false;

            idleTimer = setTimeout(function () {

                if (mousey > 200) {

                    $('#navigation_horiz').slideUp('slow', function () { });
                    $('.scrollingHotSpotLeft').fadeOut();
                    $('.scrollingHotSpotRight').fadeOut();
                    $('#icons').fadeOut();



                    idleState = true;
                }
            }, idleWait);
        });

        $("body").trigger("mousemove");

    });
})(jQuery)


