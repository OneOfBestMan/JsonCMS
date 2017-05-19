var town = "";
var tweetpage = 1;

function startscripts(searchtown) {
    town = searchtown;

    if (searchtown != null && searchtown != '') {
        showtweets();
    }
}

/* comments */

function checkcommentsize(maxsize) {
    var id = document.getElementById("commt");
    var t = id.value;
    if (t.length > maxsize) {
        id.value = t.substr(0, maxsize);
        alert("At maximum comment length");
    }
}

/* tweets */

function showtweetpage(pno) {
    var id = document.getElementById('wrapper');
    $("#tweets").slideUp("fast");

    id.setAttribute("class", "tweetp" + pno);
    tweetpage = pno;
    $(window).scrollTop($('#tweets').offset().top - 30);
    $("#tweets").slideDown("fast");

}

function showtweetsrefresh() {
    showtweets();
    showtweetpage(1);
}

function showtweets() {

    var url = window.location.protocol + "//" + window.location.host + "?id=GetTweets&id2=" + town;
    var i = 0;
    var pno = 0;
    $('#tweets').html("");

    $.getJSON(url, function (json) {
        if (json != null) {
            $.each(json, function (x, tweet) {
                var upp1 = tweet.Text.toUpperCase();
                var tweettime = tweet.CreatedAt.substr(0, 10);
                i++;
                pno = 1 + Math.floor((i - 1) / 8);
                var name = tweet.CreatedBy.UserDTO.name;
                var symbol = tweet.CreatedBy.UserDTO.profile_image_url;
                $('#tweets').append('<div class="tweet page' + pno + '"><img class="imgTweet" width="48" height="48" title="' + tweettime + '" alt="' + tweettime + '"  src="' + symbol + '"/><a target="_blank" ref="nofollow" href="http://twitter.com/' + name + '">@' + name + '</a> says: ' + tweet.Text + '</div>');

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

