// TextArea AutoSize 

var observe;
if (window.attachEvent) {
    observe = function (element, event, handler) {
        element.attachEvent('on' + event, handler);
    };
}
else {
    observe = function (element, event, handler) {
        element.addEventListener(event, handler, false);
    };
}

function AutoSize(elementId) {
    var text = document.getElementById(elementId);
    function resize() {
        text.style.height = 'auto';
        text.style.height = text.scrollHeight + 'px';
    }
    /* 0-timeout to get the already changed text */
    function delayedResize() {
        window.setTimeout(resize, 0);
    }
    observe(text, 'change', resize);
    observe(text, 'cut', delayedResize);
    observe(text, 'paste', delayedResize);
    observe(text, 'drop', delayedResize);
    observe(text, 'keydown', delayedResize);

    //text.focus();
    //text.select();
    resize();
}


AutoSize('FrontSide');
AutoSize('BackSide');



// Adds Tab-Functionality to TextAreas

//document.testSelector("textarea").addEventListener('keydown', function (e) {
//    if (e.keyCode === 9) { // tab was pressed
//        // get caret position/selection
//        var start = this.selectionStart;
//        var end = this.selectionEnd;

//        var target = e.target;
//        var value = target.value;

//        // set textarea value to: text before caret + tab + text after caret
//        target.value = value.substring(0, start)
//            + "\t"
//            + value.substring(end);

//        // put caret at right position again (add one for the tab)
//        this.selectionStart = this.selectionEnd = start + 1;

//        // prevent the focus lose
//        e.preventDefault();
//    }
//}, false);