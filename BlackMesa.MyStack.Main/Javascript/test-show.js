var frontSideArea = document.getElementById('frontside');
var backSideArea = document.getElementById('backside');
var resultButtonsArea = document.getElementById('result-buttons');

backSideArea.style.display = "none";
resultButtonsArea.style.display = "none";

var elems = document.getElementsByClassName('frontside-backside-toggle'), i;

for (i in elems) {
    elems[i].onclick = function (e) {
    
        if (frontSideArea.style.display == "none") {
            frontSideArea.style.display = "block";
            backSideArea.style.display = "none";
        } else {
            frontSideArea.style.display = "none";
            backSideArea.style.display = "block";
        }
        
        if (resultButtonsArea.style.display == "none") {
            resultButtonsArea.style.display = "block";
        }

    };
}