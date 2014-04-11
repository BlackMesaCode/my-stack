
var toggleMenuOptions = function() {
    if ($("a.checked").length > 0) {
        $("#has-any-selection-menu").show();
        $("#has-none-selection-menu").hide();
    } else {
        $("#has-any-selection-menu").hide();
        $("#has-none-selection-menu").show();
    }
};

var addCardSuccess = function (result) {
    toggleMenuOptions();
};

var removeCardSuccess = function (result) {
    toggleMenuOptions();
};

var addFolderSuccess = function (result) {
    toggleMenuOptions();
};

var removeFolderSuccess = function (result) {
    toggleMenuOptions();
};

$(document).ready(function () {

    toggleMenuOptions();

    //$("div[id$='-checkbox']").click(function () {
    //    if ($("a.checked").length > 0) {
    //        $("#has-any-selection").show();
    //        $("#has-none-selection").hide();
    //    } else {
    //        $("#has-any-selection").hide();
    //        $("#has-none-selection").show();
    //    }
    //});

});