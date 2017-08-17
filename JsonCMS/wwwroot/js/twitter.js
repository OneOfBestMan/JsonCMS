var _domain;
var _searchString
var tweetpage = 1;

function getTwitterData(domain, searchString) {

    _searchString = searchString;
    _domain = domain;

    var i = 0;
    var pno = 0;
    $('#tweets').html("");

    $.getJSON("/api/TwitterApi/GetTweets" + "?d=" + domain + "&searchString=" + searchString, function (json) {
        if (json !== null) {
            $.each(json, function (x, tweet) {
                var upp1 = tweet.text.toUpperCase();
                var tweettime = tweet.createdAt.substr(0, 10);
                i++;
                pno = 1 + Math.floor((i - 1) / 8);
                var name = tweet.createdBy.userDTO.name;
                var symbol = tweet.createdBy.userDTO.profile_image_url;
                $('#tweets').append('<div class="tweet page' + pno + '"><img class="imgTweet" width="48" height="48" title="' + tweettime + '" alt="' + tweettime + '"  src="' + symbol + '"/><a target="_blank" ref="nofollow" href="http://twitter.com/' + name + '">@' + name + '</a> says: ' + tweet.text + '</div>');

            });

            if (pno > 10) {
                pno = 10;
            }

            var pages = "";
            for (var f = 1; f <= pno; f++) {
                var thispage = "";
                thispage = " class='pg" + f + "' ";
                pages = pages + '<a ' + thispage + ' href="#" onclick="showtweetpage(' + f + '); return false;">' + f + '</a> ';
            }
            pages = pages + '<a title="Refresh Tweets"  alt="Refresh Tweets"  href="#" onclick="showtweetsrefresh(); return false;"><i class="icon-refresh"></i></a> ';
            $('#tweetpage').html('<div class="tweetpaging"><p>Page : ' + pages + '</p></div>');
        }
    })
    .done(function () { })
    .fail(function (jqxhr, textStatus, error) {
        var err = textStatus + ', ' + error;
    });
}

function showtweetpage(pno) {
    var id = document.getElementById('twittercontent');
    $("#tweets").slideUp("fast");

    id.setAttribute("class", "tweetp" + pno);
    tweetpage = pno;
    $("#tweets").slideDown("fast");
}

function showtweetsrefresh() {
    getTwitterData(_domain, _searchString);
    showtweetpage(1);
    $(window).scrollTop($('#tweets').offset().top - 10);
}

