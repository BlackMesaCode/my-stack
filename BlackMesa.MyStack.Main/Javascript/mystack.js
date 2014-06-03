
function isTouchDevice() {
    return 'ontouchstart' in window || !!(navigator.msMaxTouchPoints);
}

$(document).ready(function () {

    if (isTouchDevice())    {
        $('body').removeClass('no-touch');
    }

});