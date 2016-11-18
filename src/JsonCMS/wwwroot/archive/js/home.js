
var picBoxes = {};
var picwid = {};
var lastBoxChanged = {};
var splashFiles = [];
var readyToSwap = true;

$(document).ready(function () {
    getSplashFiles();
    loadpics();
});

$(window).resize(function () {
    loadpics();
});


function getSplashFiles() {

    $(".regionType_Gallery img").each(function (index) {
        var image = $(this).attr("src");
        splashFiles.push(image.substring(image.lastIndexOf('/')+1));
    });
}

function rndColor() {

    var randomnumber = Math.floor(Math.random() * 3)
    randomnumber = Math.floor(Math.random() * 3)
    randomnumber = Math.floor(Math.random() * 3)
    randomnumber = Math.floor(Math.random() * 3)

    switch (randomnumber) {
        case 1: return "#1d1a2f";
        case 2: return "#27262e";
        default: return "#1b1b1b";
    }
}

function loadpics() {

    var windowWidth = $("#leftcol").width();
    var windowHeight = $(window).height();

    var picsByHeight = Math.floor($(window).height() / 250) + 1;

    picwid = Math.floor(windowHeight / picsByHeight);

    picBoxes = Math.floor(windowWidth / picwid) * (Math.floor(windowHeight / picwid));

    $('#leftcol').empty();
    $('#leftcol').append('<div id="leftcolborder"></div>');
    $('#leftcol').append('<div id="allboxes"></div>');

    for (var f = 0; f < picBoxes; f++) {
        $('#allboxes').append('<div class="boxes" id="box' + f + '"></div>');
        $('#box' + f).css('width', picwid);
        $('#box' + f).css('height', picwid);
        $('#box' + f).css('background-color', rndColor());

        $('#box' + f).append('<img style="display:none" src="" />');
    }

    var leftcolborderwidth = windowWidth - Math.floor(windowWidth / picwid) * picwid - Math.floor(windowWidth / picwid);
    $('#leftcolborder').css('width', leftcolborderwidth);

    if ($("#leftcolborder").width() == 0) {

        leftcolborderwidth = picwid;
        $('#leftcolborder').css('width', leftcolborderwidth);
    }

    $('#leftcolborder').css('height', windowHeight);

    for (var f = 0; f < 14; f++) {
        updatePic();
    }
}

function updatePic() {
    var fileCount = splashFiles.length;
    var randompicbox = Math.floor(Math.random() * picBoxes);
    var randomremovepicbox = Math.floor(Math.random() * picBoxes);
    var randomfile = splashFiles[Math.floor(Math.random() * fileCount)];

    var leftcol = document.getElementById("leftcol");
    var pageText = leftcol.innerHTML;

    if (pageText.indexOf(randomfile) <= 0) { // check not showing
        if (randompicbox != lastBoxChanged) {
            lastBoxChanged = randompicbox;
            if (pageText.indexOf(randomfile) <= 0) {
                $('#box' + randompicbox + ' img').attr("src", '/archive/images/' + randomfile).fadeIn("slow");
            }
        }
    }

}

