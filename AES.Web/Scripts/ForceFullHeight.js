var head = document.getElementsByTagName('head')[0];
var style = document.createElement("style");
style.type = "text/css";
head.appendChild(style);

setTimeout(makeFullHeight, 300);

window.addEventListener('resize', makeFullHeight);

function makeFullHeight() {
    var navBar = document.getElementById('NavOffset');

    var winHeight = document.documentElement.clientHeight;
    if (window.innerHeight && window.innerHeight > 0) {
        winHeight = window.innerHeight;
    }

    var navHeight = navBar.clientHeight;
    style.innerHTML = "html body .full-height{min-height:" + (winHeight - navHeight) + "px!important;}";
}