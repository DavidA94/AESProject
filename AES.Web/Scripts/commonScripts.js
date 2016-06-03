/********************************************
** AjaxHelper function definition           *
** Handles the changing readystate's and    *
** status codes that come back from an AJAX *
** request.                                 *
**                                          *
** Parameters                               *
**  event: The AJAX event that holds the    *
**         xmlhttp context                  *
**                                          *
**  OKFunction: Called when a 200 code is   *
**              returned by the server.     *
********************************************/
function AjaxHelper(event, OKFunction) {
    // If the event, or the xmlhttp context, or the OK function are not present, do nothing.
    if (!event || !event.currentTarget || !OKFunction) {
        return;
    }

    // Get the xmlhttp context from the event
    var xmlhttp = event.currentTarget;

    // If the request is starting
    if (xmlhttp.readyState == 1) {
        // Show a spinner
        var spinner = document.createElement("div");
        spinner.id = "ajaxSpinner";
        document.documentElement.appendChild(spinner);
    }

        // If the request is finished
    else if (xmlhttp.readyState == 4) {
        // Call the OK function and remove the spinner
        if (xmlhttp.status == 200) { // OK
            try {
                OKFunction(xmlhttp);
            }
            catch (ex) {
                console.debug(ex.message);
            }
        }
            // Tell the user something is up, and log the error response.
        else { // ERROR
            alert("Unknown error occurred.\nPlease try again, or contact technical support.");
            console.debug(xmlhttp.responseText);
        }

        var spinner = document.getElementById("ajaxSpinner");
        spinner.parentNode.removeChild(spinner);
    }
}

// Dropdown remembers the choice of slected item
$(".dropdown-menu li a").click(function () {

    $(".btn:first-child").html($(this).text() + ' <span class="caret"></span>');

});
