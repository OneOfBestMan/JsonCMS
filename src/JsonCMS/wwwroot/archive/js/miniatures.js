
var viewModel = {

    sections: ko.observableArray(),
    currentImages: ko.observableArray(),
    currentSectionName: ko.observable(""),
    currentSectionNumber: ko.observable(),
}

ko.applyBindings(viewModel);

function Section(sectionNo, sectionName) {
    return {
        sectionNo: sectionNo,
        sectionName: sectionName,
        eventName: 'changeSection(' + sectionNo + ')'
    };
}

function Image(fullPath, title, alt) {
    return {
        fullPath: fullPath,
        title: title,
        alt: alt
    };
}

function getSectionImages(sectionName) {
    $.getJSON("/api/GalleryApi/miniatures/" + sectionName + "?d=archive", function (data) {
        viewModel.currentImages([]);
        for (var i = 0; i < data.length; i++) {
            viewModel.currentImages.push(new Image(data[i].value, data[i].alt, data[i].alt));
        }
        $("#centerCol").scrollTop(0);
    });
}

$.getJSON("/api/GalleryApi/miniatures?d=archive", function (data) {

    viewModel.sections([]);
    for (var i = 0; i < data.length; i++) {
        viewModel.sections.push(new Section(i + 1, data[i].value));
    }

    //var url = window.location.href;
    //var sp = url.split('#');
    //if (sp.length > 0) {
    //    var section = sp[1] - 1;
    //    viewModel.currentSectionNumber = section;
    //}
    //else {
        var rnd = Math.floor((Math.random() * data.length));
        viewModel.currentSectionNumber = rnd;
    //}

    var newSectionName = viewModel.sections()[viewModel.currentSectionNumber].sectionName;
    viewModel.currentSectionName(newSectionName);

    getSectionImages(newSectionName);
});

function changeSection(sectionNo) {
    viewModel.currentSectionNumber = sectionNo - 1;
    var newSectionName = viewModel.sections()[viewModel.currentSectionNumber].sectionName;
    viewModel.currentSectionName(newSectionName);

    getSectionImages(newSectionName);
}