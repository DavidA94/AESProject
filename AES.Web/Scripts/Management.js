// #region Initial Setup

var editBox = document.getElementById("editBox");
var newUser = document.getElementById('newTemplate');

try{
    document.getElementById('addNewUser').addEventListener('click', showCreateNewUser);
}
catch(e){}
try{
    document.getElementById('addNewStore').addEventListener('click', showCreateNewStore);
}
catch(e){}

addEditEvents();
addDeactivateEvents();
addFormSubmitEvents();

// #endregion

// #region Add/Remove Events

/*******************************************************
** addEditEvents function definition                   *
** Adds the click event to all of the "Edit" buttons   *
*******************************************************/
function addEditEvents() {
    var editButtons = document.getElementsByClassName("edit");
    for (var i = 0; i < editButtons.length; ++i) {
        editButtons[i].addEventListener('click', showEdit);
    }
}

/********************************************************
** removeEditEvents function definition                 *
** Removes the click event from all of the Edit buttons *
********************************************************/
function removeEditEvents() {
    var editButtons = document.getElementsByClassName("edit");
    for (var i = 0; i < editButtons.length; ++i) {
        editButtons[i].removeEventListener('click', showEdit);
    }
}

/********************************************************
** addDeactivateEvents function definition              *
** Adds the click events to all the Deactivate buttons  *
********************************************************/
function addDeactivateEvents() {
    var deactivateButtons = document.getElementsByClassName("deactivate");

    if (location.href.indexOf("Users") > 0) {
        for (var i = 0; i < deactivateButtons.length; ++i) {
            deactivateButtons[i].addEventListener('click', deactivateUser);
        }
    }
    else if (location.href.indexOf("Stores") > 0) {
        for (var i = 0; i < deactivateButtons.length; ++i) {
            deactivateButtons[i].addEventListener('click', deactivateStore);
        }
    }
}

/************************************************************
** removeDeactivateEvents function definition               *
** Removes the click eents from all the Deactivate buttons  *
************************************************************/
function removeDeactivateEvents() {
    var deactivateButtons = document.getElementsByClassName("deactivate");


    if (location.href.indexOf("Users") > 0) {
        for (var i = 0; i < deactivateButtons.length; ++i) {
            deactivateButtons[i].removeEventListener('click', deactivateUser);
        }
    }
    else if (location.href.indexOf("Stores") > 0) {
        for (var i = 0; i < deactivateButtons.length; ++i) {
            deactivateButtons[i].removeEventListener('click', deactivateStore);
        }
    }
}

/********************************************************
** addFormSubmitEvents function definition              *
** Adds the submit event to each form based on what the *
** current page is.                                     *
********************************************************/
function addFormSubmitEvents() {
    var forms = document.getElementsByTagName('form');

    // Users Page
    if (location.href.indexOf("Users") > 0) {
        for (var i = 0; i < forms.length; ++i) {
            forms[i].addEventListener('submit', updateUser);
        }
    }
    // Stores Page
    else if (location.href.indexOf("Stores") > 0) {
        for (var i = 0; i < forms.length; ++i) {
            forms[i].addEventListener('submit', updateStore);
        }
    }
}

/********************************************************
** removeFormSubmitEevnts function definition           *
** Removes the submit event to each form based on what  *
** the current page is.                                 *
********************************************************/
function removeFormSubmitEevnts() {
    var forms = document.getElementsByTagName('form');
    
    // Users Page
    if (location.href.indexOf("Users") > 0) {
        for (var i = 0; i < forms.length; ++i) {
            forms[i].removeEventListener('submit', updateUser);
            forms[i].removeEventListener('submit', addUser);
        }
    }
    // Stores Page
    else if (location.href.indexOf("Stores") > 0) {
        for (var i = 0; i < forms.length; ++i) {
            forms[i].removeEventListener('submit', updateStore);
        }
    }
}

// #endregion

// #region Store Methods

/****************************************************************
** addStore function definition                                 *
** Sends a request to make a new store, and if it is successful *
** refreshes the stoers list                                    *
****************************************************************/
function addStore(event) {
    // Don't submit the form
    event.preventDefault();

    // Get the form
    var form = event.currentTarget;

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the AJAX requst is complete
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // If there's success in the returned message,
            if (xmlhttp.responseText.indexOf("Success") >= 0) {
                // Update the list of users
                updateList(document.getElementById("GetStoreListURL").innerHTML)
            }
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Open a new request and send it
    xmlhttp.open("POST", form.action, true);
    xmlhttp.send(new FormData(form));

    return false;
}

/****************************************
** deactivateUser function definition   *
** Deactives a user from the system.    *
fun****************************************/
function deactivateStore(event) {
    var deactivate = confirm("Are you sure you want to deactivate this store? This cannot be undone.");

    // If they didn't want to deactive, then don't do anything
    if (!deactivate) {
        return;
    }

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the AJAX requst is complete
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // If there's success in the returned message,
            if (xmlhttp.responseText == "Success") {
                // Update the list of users
                updateList(document.getElementById("GetStoreListURL").innerHTML)
            }
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Get the user form and email address
    var item = findParentClass(event.currentTarget, "item");
    var form = item.getElementsByTagName('form')[0];

    // Open a new request and send it
    xmlhttp.open("POST", document.getElementById("DeactivateStoreURL").innerHTML, true);
    xmlhttp.send(new FormData(form));
}

/****************************************
** showCreateNewStore                   *
** Shows the form to create a new store *
****************************************/
function showCreateNewStore(event){
    // Restore whatever is being edited
    restoreEdit();

    // Fill the edit box with the newUser content
    editBox.innerHTML = newUser.innerHTML;

    // Get the form and ensure it's reset
    var form = editBox.getElementsByTagName('form')[0];
    form.reset();

    // Set the form action for creating a new user, and set it's class show it shows the Name fields
    form.action = document.getElementById("NewStoreURL").innerHTML;

    // Set the submit button's text
    form.submitBtn.value = "Add Store";
    form.Zip.value = ""; // Ensure there's no zero shown

    // Remove the updateUser event, and add the AddUser event
    form.removeEventListener('submit', updateStore);
    form.addEventListener('submit', addStore)
}

/************************************
** updateStore function definition  *
** Updates a store in the system    *
************************************/
function updateStore(event) {
    // Don't submit the form
    event.preventDefault();

    // Get the form for the store being edited
    var item = findParentClass(document.getElementById("placeholder"), "item");
    var form = event.currentTarget;

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what hapens when the request is complete
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // If successful, update the store list, otherwise let the user know that something went wrong.
            if (xmlhttp.responseText == "Success") {
                updateList(document.getElementById("GetStoreListURL").innerHTML)
            }
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Open the request and send it
    xmlhttp.open("POST", form.action, true);
    xmlhttp.send(new FormData(form));

    return false;
}

// #endregion

// #region Users Methods

/****************************************************************
** addUser function definition                                  *
** Sends a request to make a new user, and if it is successful, *
** refreshes the user list and shows the new user's temporary   *
** password.                                                    *
****************************************************************/
function addUser(event) {
    // Don't submit the form
    event.preventDefault();

    // Get the form
    var form = event.currentTarget;

    // Create the form data and add a dummy email for sake of the model
    var formData = new FormData(form);
    formData.append("Email", "dummy@dummy.com");

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the AJAX requst is complete
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // If there's success in the returned message,
            if (xmlhttp.responseText.indexOf("Success") >= 0) {
                // Update the list of users
                updateList(document.getElementById("GetUserListURL").innerHTML)

                // And then get the temporary passowrd and show it (Returned value is "Success:[tempPassword]
                var tempPass = xmlhttp.responseText.split(":")[1];
                alert("The new user's temporary password is: " + tempPass);
            }
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Open a new request and send it
    xmlhttp.open("POST", form.action, true);
    xmlhttp.send(formData);

    return false;
}

/****************************************
** deactivateUser function definition   *
** Deactives a user from the system.    *
****************************************/
function deactivateUser(event) {
    var deactivate = confirm("Are you sure you want to deactivate this user? This cannot be undone.");

    // If they didn't want to deactive, then don't do anything
    if (!deactivate) {
        return;
    }

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the AJAX requst is complete
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // If there's success in the returned message,
            if (xmlhttp.responseText == "Success") {
                // Update the list of users
                updateList(document.getElementById("GetUserListURL").innerHTML)
            }
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Get the user form and email address
    var item = findParentClass(event.currentTarget, "item");
    var form = item.getElementsByTagName('form')[0];
    var email = item.getElementsByClassName("email")[0].innerHTML;

    // Get the FromData object, and add the email to it
    var formData = new FormData(form);
    formData.append("Email", email);

    // Open a new request and send it
    xmlhttp.open("POST", document.getElementById("DeactivateUserURL").innerHTML, true);
    xmlhttp.send(formData);
}

/****************************************
** showCreateNewUser                    *
** Shows the form to create a new user  *
****************************************/
function showCreateNewUser(event) {
    // Restore whatever is being edited
    restoreEdit();

    // Fill the edit box with the newUser content
    editBox.innerHTML = newUser.innerHTML;

    // Get the form and ensure it's reset
    var form = editBox.getElementsByTagName('form')[0];
    form.reset();

    // Set the form action for creating a new user, and set it's class show it shows the Name fields
    form.action = document.getElementById("newUserURL").innerHTML;
    form.className += " newUser";

    // Make it so the store isn't auto selected
    form.StoreID.value = -1;

    // Set the submit button's text
    form.submitBtn.value = "Add User";

    // Remove the updateUser event, and add the AddUser event
    form.removeEventListener('submit', updateUser);
    form.addEventListener('submit', addUser)
}

/************************************
** updateUser function definition   *
** Updates a user in the system     *
************************************/
function updateUser(event) {
    // Don't submit the form
    event.preventDefault();

    // Get the email address for the user being edited
    var item = findParentClass(document.getElementById("placeholder"), "item");
    var email = item.getElementsByClassName("email")[0].innerHTML;

    // Create the FormData and add the email to it
    var form = event.currentTarget;
    var formData = new FormData(form);
    formData.append("Email", email);

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what hapens when the request is complete
    xmlhttp.onreadystatechange = function(event){
        AjaxHelper(event, function (xmlhttp) {
            // If successful, update the user list, otherwise let the user know that something went wrong.
            if (xmlhttp.responseText == "Success") {
                updateList(document.getElementById("GetUserListURL").innerHTML)
            }
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Open the request and send it
    xmlhttp.open("POST", form.action, true);
    xmlhttp.send(formData);

    return false;
}

// #endregion

// #region Helpers

/********************************************************
** findParentClass function definition                  *
** Finds the parent of the given element that has the   *
** given class name.                                    *
**                                                      *
** Paramters                                            *
**  elem: The element who is the child                  *
**                                                      *
**  parentClassName: The class name of the parent       *
**                   element that you wish to find.     *
********************************************************/
function findParentClass(elem, parentClassName) {
    while (elem && elem.className != parentClassName) { elem = elem.parentNode; }
    return elem;
}

/********************************************
** restoreEdit function definition          *
** Restores whatever is in the editBox back *
** to its original location, and removes    *
** any events that would be left behind.    *
********************************************/
function restoreEdit() {
    // Get the placeholder (where the element in the edit box will be moved to)
    var placeholder = document.getElementById("placeholder");

    // If there is no placeholder
    if (!placeholder) {
        // If there is an .editBox inside #editBox, then remove it, otherwise, just no-op
        if (editBox.getElementsByClassName('editBox').length > 0) {
            editBox.removeChild(editBox.getElementsByClassName('editBox')[0]);
        }

        return;
    }

    // If there is an .editBox inside #editBox
    if (editBox.getElementsByClassName("editBox").length > 0) {
        // Get the object that needs to be moved
        var objectToMove = editBox.getElementsByClassName("editBox")[0];

        // Place the object that is being moved before the placeholder
        placeholder.parentNode.insertBefore(objectToMove, placeholder);

        // And remove the placeholder
        placeholder.parentNode.removeChild(placeholder);
    }
}

/********************************************************
** showEdit function definition                         *
** Shows the .editBox for a given item in the #editBox  *
********************************************************/
function showEdit(event) {
    // Get the item that we want to edit
    var item = event.currentTarget;
    while (item && item.className != "item") { item = item.parentNode; }

    // If we don't find an item, do nothing
    if (!item || !item.getElementsByClassName("editBox")[0]) {
        return;
    }

    // Get the actual element
    var editObject = item.getElementsByClassName("editBox")[0];

    // Clear out anything that was being edited prior
    restoreEdit();

    // Create the placeholder DIV
    var newPlaceholder = document.createElement("div");
    newPlaceholder.id = "placeholder";

    // Insert the palaceholder before the object we want to move
    editObject.parentNode.insertBefore(newPlaceholder, editObject);

    // and move the editObject to the editObject
    editBox.appendChild(editObject);
}

/********************************************************
** updateList function definition                       *
** Updates the #List container with the response from   *
** the given URL.                                       *
**                                                      *
** Parameters                                           *
**  url: The URL to GET the new content from            *
********************************************************/
function updateList(url) {
    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the request is done
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // If it has a DIV element
            if (xmlhttp.responseText.indexOf("<div") >= 0) {

                // Restore whatever is in the editbox, and remove all events
                restoreEdit();
                removeEditEvents();
                removeDeactivateEvents();
                removeFormSubmitEevnts();

                // Fill the #List box with the new content
                document.getElementById("List").innerHTML = xmlhttp.responseText;
                
                // Add the events back
                addEditEvents();
                addDeactivateEvents();
                addFormSubmitEvents();
            }
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Open the request and send it
    xmlhttp.open("GET", url, true);
    xmlhttp.send();
}

// #endregion