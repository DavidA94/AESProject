var head = document.getElementsByTagName('head')[0];

var winHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);

var style = document.createElement("style");
style.type = "text/css";
style.innerHTML = ".container-fluid{min-height:" + winHeight + "px;}";

head.appendChild(style);


window.addEventListener('resize', function () {
    var _winHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);
    style.innerHTML = "html body .container-fluid{min-height:" + _winHeight + "px;}";
})