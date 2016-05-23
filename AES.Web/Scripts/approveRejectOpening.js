// #region Initial Stuff 

// OpeningStatus enum
var OpeningStatus =
{
    APPROVED : 0,
    REJECTED : 2
}

// Stack used so we know when all AJAX things are done
var ajaxStack = [];

// Keeps track of the currently open rejectBox
var currentRejectBox = null;

// Add events to all the opening requests
addEvents();

// #endregion

// #region Add/Remove events

/************************************
** addEvents function definition    *
** Adds events to all the buttons   *
** on the page                      *
************************************/
function addEvents() {
    // Remove any current events just as a cleanup-precaution
    removeEvents();

    // Get all the buttons
    var initialRejects = document.getElementsByClassName('reject arButton col-md-4');
    var approveButtons = document.getElementsByClassName('accept arButton col-md-4');
    var rejectButtons = document.getElementsByClassName('reallyReject arButton col-md-3 pull-right');
    var cancelButtons = document.getElementsByClassName('cancelReject arButton col-md-3 pull-right');

    // Since there is one of each button in a request, there should be the same number for all of these,
    // But try just in case. No-op if an index isn't found
    for (var i = 0; i < initialRejects.length; ++i) {
        try {
            // Handle the click event for all of these
            initialRejects[i].addEventListener('click', showRejectBox);
            approveButtons[i].addEventListener('click', approveOpening);
            rejectButtons[i].addEventListener('click', rejectOpening);
            cancelButtons[i].addEventListener('click', cancelReject);
        }
        catch(ex) {}
    }
}

/************************************
** removeEvents function definition *
** Removes all of the events from   *
** the buttons on the page.         *
************************************/
function removeEvents() {
    // Get all the buttons
    var initialRejects = document.getElementsByClassName('reject arButton col-md-4');
    var approveButtons = document.getElementsByClassName('accept arButton col-md-4');
    var rejectButtons = document.getElementsByClassName('reallyReject arButton col-md-3 pull-right');
    var cancelButtons = document.getElementsByClassName('cancelReject arButton col-md-3 pull-right');

    // Since there is one of each button in a request, there should be the same number for all of these,
    // But try just in case. No-op if an index isn't found
    for (var i = 0; i < initialRejects.length; ++i) {
        try {
            // Remove the click listener from all of these
            initialRejects[i].removeEventListener('click', showRejectBox);
            approveButtons[i].removeEventListener('click', approveOpening);
            rejectButtons[i].removeEventListener('click', rejectOpening);
            cancelButtons[i].removeEventListener('click', cancelReject);
        }
        catch (ex) { }
    }
}

// #endregion

// #region Event Handlers

/************************************************
** approveOpening function definition           *
** Fires when an approve button is clicked, and *
** causes job linked to it to be approved in    *
** the system.                                  *
************************************************/
function approveOpening(event) {
    // Get the opening that the button that was clicked is linked to. If it's not found, return.
    var opening = findOpening(event.currentTarget);
    if (opening == null) {
        return;
    }

    // Get the request ID and set the status
    var requestID = opening.getElementsByClassName("requestID")[0].innerHTML;
    var status = OpeningStatus.APPROVED;

    // Create the formDAta
    var formData = new FormData();
    formData.append("requestID", requestID);
    formData.append("status", status);

    // Send the decision to the server
    sendDecision(formData, document.getElementById("SetRequestURL").innerHTML);
}

/****************************************
** cancelReject function definition     *
** Cancels the reject box if it is open *
****************************************/
function cancelReject(event) {
    // If there is no reject box open, do nothing
    if (currentRejectBox == null) {
        return;
    }

    // Get the reject box from the parent, and remove the style (what makes it visible)
    currentRejectBox.getElementsByClassName('rejectBox')[0].removeAttribute('style');

    // Nullify the current box since it's not open anymore
    currentRejectBox = null;

    // Remove the cover from the body
    document.body.removeChild(document.getElementById("cancelCover"));
}

/************************************************
** rejectOpening function definition            *
** Fires when a reject button is clicked, and   *
** causes job linked to it to be rejected in    *
** the system.                                  *
************************************************/
function rejectOpening(event) {
    // Get the opening that the button that was clicked is linked to. If it's not found, return.
    var opening = findOpening(event.currentTarget);
    if (opening == null) {
        return;
    }

    // Get the information that we need to send and set the status to reject
    var requestID = opening.getElementsByClassName("requestID")[0].innerHTML;
    var notes = opening.getElementsByTagName("textarea")[0].value;
    var status = OpeningStatus.REJECTED;

    // Create the FormData object
    var formData = new FormData();
    formData.append("requestID", requestID);
    formData.append("status", status);
    formData.append("notes", notes)

    // And send the decision
    sendDecision(formData, document.getElementById("SetRequestURL").innerHTML);
}

/****************************************
** showRejectBox function definition    *
** Shows the reject confirmation/notes  *
** box for a given opening.             *
****************************************/
function showRejectBox(event) {
    // Get the opening that the button that was clicked is linked to. If it's not found, return.
    var elem = findOpening(event.currentTarget);
    if (elem === null) {
        return;
    }

    // Set the current reject box to this opening
    currentRejectBox = elem;

    // Show the reject box
    var rejectBox = elem.getElementsByClassName('rejectBox')[0];
    rejectBox.style.display = "block";

    // Create a DIV to make all other elements on the page disabled until this is closed
    var div = document.createElement("div");
    div.id = "cancelCover";
    document.body.appendChild(div);
    div.setAttribute("onclick", "cancelReject(event)");  // Using onclick so cleaning up the event isn't needed
}

// #endregion

// #region Helpers

/****************************************
** findOpening function definition      *
** Finds the opening DIV for the        *
** given elemenet, and returns it       *  
**                                      *
** Parameters                           *
**  elem: The element who's parent      *
**        opening DIV we want to get.   *
**                                      *
** Returns                              *
**  The .opening DIV, or null if one    *
**  wasn't found.                       *
****************************************/
function findOpening(elem) {
    // While it's not null, and it's not the right element, get the parent node.
    while (elem != null && elem.className != "opening") { elem = elem.parentNode; }

    // Return whatever we got
    return elem;
}

/****************************************
** sendDecision function definition     *
** Sends the decision for an opening to *
** the server.                          *
**                                      *
** Parameters                           *
**  formData: A FormData object that    *
**            has the needed POST data  *
**            to send to the server to  *
**            change the status of a    *
**            job opening.              *
**                                      *
**  url: The URL that we want to send   *
**       The decision request to.       *
****************************************/
function sendDecision(formData, url) {

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what will happen when the request is done
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // If we got success back, reload the lists
            if (xmlhttp.responseText == "success") {
                removeEvents();

                reloadPending();
                reloadApproved();
                reloadDenied();
            }
            // Otherwise tell the user what went wrong
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Open a new POST request to the URL and send it.
    xmlhttp.open("POST", url, true);
    xmlhttp.send(formData);
}

// #endregion

// #region Reloaders

/****************************************
** reloadApproved function definition   *
** Reloads the approved jobs list       *
****************************************/
function reloadApproved() {
    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the request is done
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // Update the list with whatever we got back.
            document.getElementById("approvedRequests").innerHTML = xmlhttp.responseText;
        });
    };

    // Open a request to the right URL and send it
    xmlhttp.open("GET", document.getElementById("ApprovedRequestsURL").innerHTML, true);
    xmlhttp.send();
}

/****************************************
** reloadDenied function definition     *
** Reloads the denied jobs list         *
****************************************/
function reloadDenied() {

    /// Ditto as reloadApproved, but for the denied list

    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            document.getElementById("rejectedRequests").innerHTML = xmlhttp.responseText;
        });
    };

    xmlhttp.open("GET", document.getElementById("DeniedRequestsURL").innerHTML, true);
    xmlhttp.send();
}

/****************************************
** reloadPending function definition    *
** Reloads the pending jobs list        *
****************************************/
function reloadPending() {

    /// Ditto as reloadApproved, but for the pending list

    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function(event){
        AjaxHelper(event, function(xmlhttp){
            document.getElementById("pendingRequests").innerHTML = xmlhttp.responseText;
        });
    };

    xmlhttp.open("GET", document.getElementById("PendingRequestsURL").innerHTML, true);
    xmlhttp.send();
}

// #endregion
