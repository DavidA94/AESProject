$(document).ready(function () {
    // Add smooth scrolling to all links in navbar + footer link
    $(".navbar a[data-smooth], .container-fluid a[data-smooth]").on('click', function (event) {

        // Prevent default anchor click behavior
        event.preventDefault();

        // Store hash
        var hash = this.hash;

        // Using jQuery's animate() method to add smooth page scroll
        // The optional number (900) specifies the number of milliseconds it takes to scroll to the specified area
        $('html, body').animate({
            scrollTop: $(hash).offset().top
        }, 1400, function () {

            // Add hash (#) to URL when done scrolling (default click behavior)
            window.location.hash = hash;
        });
    });
})

$('#nav-wrapper').height($("#nav").height());

$('#nav').affix({
    offset: $('#nav').position()
});