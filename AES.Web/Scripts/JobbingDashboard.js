// #region Initial Setup

var editBox = document.getElementById("editBox");
var noJobs = document.getElementById("noJobSelected");

document.getElementById("addJob").addEventListener('click', getNewJobTemplate);
document.getElementById("addNewQ").addEventListener('click', getNewQuestionTemplate);
document.getElementById("addExistingQ").addEventListener('click', getQuestionsList);

addJobListEvents()

// #endregion

// #region Helper Methods

/********************************************
** addFormSubmitEvent function definition   *
** Makes the given function be called when  *
** the form in the editBox is submitted.    *
**                                          *
** Parameters                               *
** eventHandler: The name of the function   *
**               to be called when the form * 
**               is submitted               *
********************************************/
function addFormSubmitEvent(eventHandler) {
    // If the editBox has a form
    if (editBox.getElementsByTagName('form')[0]) {
        // Add the onsubmit event.
        // We use onsubmit here because this is only used for new jobs/questions.
        // Since there isn't a good way to know when a new j/q is in the editBox
        // as opposed to an edit j/q, using this method allows the event to be
        // destroyed with the DOM
        editBox.getElementsByTagName('form')[0].setAttribute("onsubmit", eventHandler + "(event)");
    }
}

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
            try{
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

/********************************************
** showEdit function definition             *
** Shows the given object in the #editBox   *
**                                          *
** Parameters                               *
** editObject: The DOM object that is to be *
**             moved to the #editBox.       *
********************************************/
function showEdit(editObject) {
    // Restore anything that is in the editBox presently
    restoreEdit();

    // Create the placeholder DIV
    var newPlaceholder = document.createElement("div");
    newPlaceholder.id = "placeholder";

    // Insert the palaceholder before the object we want to move
    editObject.parentNode.insertBefore(newPlaceholder, editObject);

    // and move the editObject to the editBox
    editBox.appendChild(editObject);
}

// #endregion

// #region Job Methods

/****************************************
** addJobListEvents function definition *
** Adds the cilck events to the job     *
** titles and job edit buttons          *
****************************************/
function addJobListEvents() {
    // Get the elements that hold the click events
    var editJobs = document.getElementsByClassName("EditJob");
    var jobForms = document.getElementsByClassName("form-horizontal jobForm");
    var jobs = document.getElementsByClassName("jobTitleText");

    // Add the events -- there should be the same amount of both, so only one loop is needed
    for (var i = 0; i < editJobs.length; ++i) {
        try{
            editJobs[i].addEventListener('click', showJobEditBox);
            jobForms[i].addEventListener('submit', submitJob);
            jobs[i].addEventListener('click', getJobQuestions);
        }
        catch (ex) {
            console.debug(ex.message);
            continue;
        }
    }
}

/****************************************
** getJobQuestions function definition  *
** Gets the questions for the given job *
** and displays them in the right box.  *
****************************************/
function getJobQuestions(event) {
    // Get the job that we want the questions for.
    var job = event.currentTarget;
    while (job && job.className != "job") { job = job.parentNode; }

    // If we don't find a job, do nothing
    if (!job) {
        return;
    }
    
    // Get all the questions for the job, by using the ID of the Job
    getJobQuestionsById(job.getElementsByClassName('jobID')[0].innerHTML, true);

    // Get the current job that was being edited, and if it exists, remove that it's being edited.
    var editing = document.getElementById('editing');
    if (editing) {
        editing.removeAttribute('id');
    }

    // Set this job as being edited.
    event.currentTarget.id = "editing";
}

/********************************************
** getJobQuestionsById function definition  *
** Gets the questions for the given job ID  *
** and puts displays them in the proper box *
**                                          *
** Parameters                               *
**  jobID: The integer ID of the job to get *
**         the questions for.               *
**                                          *
**  restoreEditBox: Indicates if whatever   *
**               is in the editBox should   *
**               be restored or not.        *
********************************************/
function getJobQuestionsById(jobID, restoreEditBox) {
    // The URL that we want to call to to get the jobs
    var questionListURL = document.getElementById("QuestionListURL").innerHTML;
    var questionsBox = document.getElementById("QuestionList"); // The box where the questions go

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the request is finished
    xmlhttp.onreadystatechange = function(event) { 
        AjaxHelper(event, function (xmlhttp) {
            // Clear any current questions
            clearQuestions();

            // Restore if necessary
            if (restoreEditBox === true) {
                restoreEdit();
            }

            // Put what came back into the questions box
            questionsBox.innerHTML = xmlhttp.responseText;

            // If nothing besides the job ID comes back
            if (xmlhttp.responseText.replace(/<div class="hidden" id="currentJob">\d+<\/div>\r?\n?/, "") == "") {
                // Set noJobs to tell the user that there are no questions for this job, and ensure it's visible
                noJobs.innerHTML = "No Questions Exist for this Job";
                noJobs.className = "";
            }
                // Otherwise
            else {
                // Ensure that noJobs is hidden
                noJobs.className = "hidden";

                // Add the event handlers for the questions
                addQuestionEventHandlers();
            }    
        });
    } 

    // Open the request to the server ASYNC, and send it
    xmlhttp.open("GET", questionListURL + "?jobID=" + jobID, true);
    xmlhttp.send();
}

/********************************************
** getNewJobTemplate function definition    *
** Gets the HTML for a new job so that a    *
** job can be added to the system           *
********************************************/
function getNewJobTemplate(event) {
    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the request is finished.
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // Clear out any open jobs, and restore anything that is being edited
            clearQuestions();
            restoreEdit();

            // Set the editBox to have the new job template
            editBox.innerHTML = xmlhttp.responseText;

            // Add the submit event
            addFormSubmitEvent("submitJob");
        });
    };

    // Open the request and send it
    xmlhttp.open("GET", document.getElementById("GetJobTemplateURL").innerHTML, true);
    xmlhttp.send();
}

/********************************************
** removeJobListEvents function definition  *
** Removes the cilck events from the job    *
** titles and job edit buttons              *
********************************************/
function removeJobListEvents() {
    // Get the elements that hold the click events
    var editJobs = document.getElementsByClassName("EditJob");
    var jobForms = document.getElementsByClassName("form-horizontal jobForm");
    var jobs = document.getElementsByClassName("jobTitleText");

    // Remove the events -- there should be the same amount of both, so only one loop is needed
    for (var i = 0; i < editJobs.length; ++i) {
        try{
            editJobs[i].removeEventListener('click', showJobEditBox);
            jobForms[i].removeEventListener('submit', submitJob);
            jobs[i].removeEventListener('click', getJobQuestions);
        }
        catch (ex) {
            console.debug(ex.message);
            continue;
        }
    }
}

/********************************************
** showJobEditBox function definition       *
** Shows the edit box for the given job     *
********************************************/
function showJobEditBox(event) {
    // Get the job that we want to edit.
    var job = event.currentTarget;
    while (job && job.className != "job") { job = job.parentNode; }

    // If we don't find a job, do nothing
    if (!job) {
        return;
    }

    // Clear out any questions that are currently being displayed for a job,
    // that way there is no diconnect between what is in that list, and what
    // is being edited.
    clearQuestions();

    // Show the edit box for this job
    showEdit(job.getElementsByClassName("editBox")[0]);
}

/************************************
** submitJob function definition    *
** Submits a job to the server, and *
** updates the list of jobs so that *
** they are up to date with the new *
** information.                     *
************************************/
function submitJob(event) {
    // Prevent the form from actually submitting so we can use AJAX
    event.preventDefault();

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the request is finished
    xmlhttp.onreadystatechange = function(event){
        AjaxHelper(event, function(xmlhttp){
            // If there is a DIV tag in our response, assume that the requst completed successfully
            if (xmlhttp.responseText.indexOf("<div") >= 0) {

                // Restore the edit box (to clear out the job that was being edited / added)
                restoreEdit();

                // Remove all the events from the job list
                removeJobListEvents();

                // Update the job list with what we got back from the server
                document.getElementById('JobList').innerHTML = xmlhttp.responseText;

                // Add the events to the job list
                addJobListEvents();
            }
            // If something goes wrong, alert what the server responded with
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Create the form data to be posted to the server
    var formData = new FormData(event.currentTarget);

    // Open the request and send it -- This one just uses the action from the <form> tag.
    xmlhttp.open("POST", event.currentTarget.action, true);
    xmlhttp.send(formData);
}

// #endregion

// #region Question Methods

/************************************************
** addQuestionEventHandlers function definition *
** Adds the event handlers for the different    *
** parts of a question element.                 *
************************************************/
function addQuestionEventHandlers() {
    var formQs = document.getElementsByClassName("form-horizontal qForm");   // The question forms
    var questions = document.getElementsByClassName("questionText");         // The question text (click to edit)
    var removeQuestions = document.getElementsByClassName('deleteQuestion'); // The question X (click to remove)
    var typeDD = document.getElementsByClassName("questionTypeDD");          // The question's type DD

    // We should have the same amount of all of these, so only one loop is needed
    for (var i = 0; i < questions.length; ++i) {
        try{
            formQs[i].addEventListener('submit', submitQuestion);
            questions[i].addEventListener('click', showQuestionEditBox);
            removeQuestions[i].addEventListener('click', removeQuestionFromJob);
            typeDD[i].addEventListener("change", updateMultipleChoice);

            // Update the dropdown when it comes in
            changeMultipleChoiceVisibility(typeDD[i]);
        }
        catch (ex) {
            console.debug(ex.message);
            continue;
        }
    }
}

/*******************************************************
** changeMultipleChoiceVisibility function definition  *
** Enables/disables and shows/hides the radio buttons  *
** or checkboxes based on whether the question type    *
** dropdown is SHORT, RADIO, or CHECKBOX.              *
**                                                     *
** Parameters                                          *
**  dropdown: The <select> object that holds the       *
**            options the user can select.             *
*******************************************************/
function changeMultipleChoiceVisibility(dropdown) {
    // If we weren't given a dropdown, do nothing
    if (!dropdown) {
        return;
    }

    // Find the multipleChoiceDiv by traversing up to the .editBox, and then searching for it by class name.
    var multipleChoiceDIV = dropdown;
    while (multipleChoiceDIV && multipleChoiceDIV.className != "editBox") { multipleChoiceDIV = multipleChoiceDIV.parentNode; }
    multipleChoiceDIV = multipleChoiceDIV ? multipleChoiceDIV.getElementsByClassName("multipleChoice")[0] : null;

    // If we didn't get the DIV, do nothing
    if (!multipleChoiceDIV) {
        return;
    }

    // Get all the radio buttons and checkboxes
    var radios = multipleChoiceDIV.getElementsByClassName("multipleChoiceRadio");
    var checks = multipleChoiceDIV.getElementsByClassName("multipleChoiceCheckbox");

    // Set all radio buttons and checkboxes to enabled
    for (var i = 0; i < radios.length; ++i) { radios[i].removeAttribute("disabled"); }
    for (var i = 0; i < checks.length; ++i) { checks[i].removeAttribute("disabled"); }

    // Figure out which optoin was selected
    if (dropdown.value == "0") { // SHORT
        // Ensure none of the multiple choice stuff is showing
        multipleChoiceDIV.className = "multipleChoice";
    }
    else {

        if (dropdown.value == 1) { // RADIO
            // Ensure only the radio buttons are showing, and clear that list so they don't get put to disabled
            multipleChoiceDIV.className = "multipleChoice typeRadio";
            radios = [];
        }
        else { // CHECKBOX
            // Ditto, but for checkboxes
            multipleChoiceDIV.className = "multipleChoice typeCheckbox";
            checks = []
        }
    }

    // Disable anything that remains
    for (var i = 0; i < radios.length; ++i) { radios[i].disabled = "disabled"; }
    for (var i = 0; i < checks.length; ++i) { checks[i].disabled = "disabled"; }
}

/********************************************
** clearQuestions function definition       *
** Clears out any questions that are in the *
** box that holds what questions are linked *
** to a given job.                          *
********************************************/
function clearQuestions() {
    // Get the box that holds the questions.
    var questionsBox = document.getElementById("QuestionList");

    // Clear out any event handlers for the questions
    removeQuestionEventHandlers();

    // Remove all children from the questionsBox
    while (questionsBox.firstChild) {
        questionsBox.removeChild(questionsBox.firstChild);
    }

    // Update noJobs to tell the user they must select a job to see questions for it, and ensure it's visible
    noJobs.innerHTML = "Select job to see questions";
    noJobs.removeAttribute('class');
    noJobs.style.display = "block";
}

/**********************************************
** getNewQuestionTemplate function definition *
** Gets the HTML for a new question so that   *
** a questoin can be added to the system      *
**********************************************/
function getNewQuestionTemplate(event) {
    // Get the URL we need to send the request to
    var getQuestionTemplateURL = document.getElementById("GetQuestionTemplateURL").innerHTML;

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the reqeust is complete
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // Restore whatever is in the #editBox
            restoreEdit();

            // Set the #editBox to have what we got backfrom the server
            editBox.innerHTML = xmlhttp.responseText;

            // Find the questionType dropdown
            var dd = editBox.getElementsByClassName("questionTypeDD")[0];
            // If we found one
            if(dd){
                // Set its onchange event so it will update when the dropdown is changed.
                // We do it this way instead of using addEventListener so that the event
                // is destroyed with the DOM.
                dd.setAttribute("onchange", "updateMultipleChoice(event)");
                
                // Make sure the dropdown visibility is correct
                changeMultipleChoiceVisibility(dd);

                // And addthe submit event to the submit button
                addFormSubmitEvent("submitQuestion");

            }
        });
    };

    // Open the request and send it
    xmlhttp.open("GET", getQuestionTemplateURL, true);
    xmlhttp.send();
}

/**********************************************
** getQuestionsList function definition       *
** Gets all questions in the database so they *
** can be added to the currently active job.  *
**********************************************/
function getQuestionsList(event) {
    // Create the AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the request has finished
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // Restore whatever is in the #editBox
            restoreEdit();

            // Put what we got in the #editBox
            editBox.innerHTML = xmlhttp.responseText;

            // Get all the questions that there were returned, and add events to them
            var questions = editBox.getElementsByClassName("questionTitle");
            for (var i = 0; i < questions.length; ++i) {
                // Use onclick so when this list is destoryed, all the events are destoryed with it.
                questions[i].setAttribute('onclick', 'moveQuestionToJob(event)');
            }
        });
    };

    // Open the request and send it
    xmlhttp.open("GET", document.getElementById("GetListOfQuestionsURL").innerHTML, true);
    xmlhttp.send();
}

/********************************************
** moveQuestionToJob function definition    *
** Moves a given question to the currently  *
** selected job.                            *
********************************************/
function moveQuestionToJob(event) {
    // Get the ID of the job that was clicked, and move it to the job
    moveQuestionToJobById(event.currentTarget.getElementsByClassName('qID')[0].innerHTML, event.currentTarget);
}

/************************************************
** moveQuestionToJobById function definition    *
** Takes the given questionID and moves it to   *
** the currently selected job. Also removes     *
** the question element if it is not null       *
**                                              *
** Parameters                                   *
**  questionID: The ID of the question to move  *
**              to the currently selected job.  *
**                                              *
**  question: The question object to be removed *
**            from its parent (if not null)     *
************************************************/
function moveQuestionToJobById(questionID, question) {
    // get the current job. If it doesn't exist, no-op
    var currentJob = document.getElementById('currentJob');
    if (!currentJob) {
        return;
    }

    // Get the ID of the current job
    var currentJobID = currentJob.innerHTML;

    // Create the AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the request has completed
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // If the response had successs in it
            if (xmlhttp.responseText == "SUCCESS") {
                // If we were passed a question, remove it from its parent
                if (question) {
                    question.parentNode.removeChild(question);
                }

                getJobQuestionsById(currentJobID);
            }
            // Otherwise show the user what the response was.
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Open a request to the server with the jobID and questionID, and send it.
    xmlhttp.open("GET", document.getElementById("AddJobQuestionLinkURL").innerHTML + "?jobID=" + currentJobID + "&questionID=" + questionID, true);
    xmlhttp.send();
}

/********************************************************
** removeQuestionEventHandlers function definition      *
** Removes all of the events from the list of questions *
********************************************************/
function removeQuestionEventHandlers() {
    var formQs = document.getElementsByClassName('form-horizontal qForm');   // The forms tha are used to edit the question
    var questions = document.getElementsByClassName("questionText");         // The questions that cause the edit form to show
    var removeQuestions = document.getElementsByClassName('deleteQuestion'); // The X's that remove a question from a job
    var typeDD = document.getElementsByClassName("questionTypeDD");          // The type dropdowns that change what is visible

    // There should be an equal amount of objects, so only one loop is needed
    for (var i = 0; i < questions.length; ++i) {
        formQs[i].removeEventListener('submit', submitQuestion);
        questions[i].removeEventListener('click', showQuestionEditBox);
        removeQuestions[i].removeEventListener('click', removeQuestionFromJob);
        typeDD[i].removeEventListener('change', updateMultipleChoice)
    }
}

/************************************************
** removeQuestionFromJob function definition    *
** Removes a question from a job, and refreshes *
** the list of questions for a given job.       *
************************************************/
function removeQuestionFromJob(event) {
    
    // Get the question that was clicked. If there is no question, no-op
    var question = event.currentTarget;
    while (question && question.className != "question") { question = question.parentNode; }
    if (!question) {
        return;
    }

    // Try to get the current job and question ID
    try{
        var questionID = question.getElementsByClassName('questionID')[0].innerHTML;
        var currentJobID = document.getElementById('currentJob').innerHTML;
    }
    catch (ex) {
        console.debug(ex.message);
        return;
    }

    // Create an AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the request has finished
    xmlhttp.onreadystatechange = function(event){
        AjaxHelper(event, function(xmlhttp){
            // If the response is SUCCESS
            if (xmlhttp.responseText == "SUCCESS") {
                // Restore whatever is in the editBox
                restoreEdit();

                // hide the question so it is gone immediately
                question.style.display = "none";

                // Refresh the list of questions for the current job.
                getJobQuestionsById(currentJobID);
            }
            // Otherwise, just alert the response
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Open a request with the jobID and questionID, and send it
    xmlhttp.open("GET", document.getElementById("RemoveJobQuestionLinkURL").innerHTML + "?jobID=" + currentJobID + "&questionID=" + questionID, true);
    xmlhttp.send();
}

/********************************************
** showQuestionEditBox function definition  *
** Shows the .editBox for a given question  *
********************************************/
function showQuestionEditBox(event) {
    
    // Get the question that was clicked. If there is no question, no-op
    var question = event.currentTarget;
    while (question && question.className != "question") { question = question.parentNode; }
    if (!question) {
        return;
    }

    // Show the editBox for the question
    showEdit(question.getElementsByClassName("editBox")[0]);
}

/****************************************
** submitQuestion function definition   *
** Submits a question so that it is     *
** created/updated, and then refreshes  *
** the list.                            *
****************************************/
function submitQuestion(event) {
    // Prevent the form from submitting so we can use AJAX
    event.preventDefault();

    // Create a new AJAX request
    var xmlhttp = new XMLHttpRequest();

    // Set what happens when the request is finished
    xmlhttp.onreadystatechange = function (event) {
        AjaxHelper(event, function (xmlhttp) {
            // If there was success in the response
            if (xmlhttp.responseText.indexOf("Success") >= 0) {
                // Restore whatever was in the box
                restoreEdit();

                // Try to get the current job, and if it exists
                var currentJob = document.getElementById('currentJob');
                if (currentJob) {
                    // Get the ID of the question from the response (Formatted "Success:[ID]") 
                    // and add the question to the current job.
                    // This will also clear the current questions and refresh the list
                    moveQuestionToJobById(xmlhttp.responseText.split(':')[1], null);
                }
                // If there is no job selected, then just let the user know all is well
                else {
                    alert("Question added successfully");
                }
            }
            // If we didn't get success, alert what we did get.
            else {
                alert(xmlhttp.responseText);
            }
        });
    };

    // Get the form, and create a new FormData object
    var form = event.currentTarget;
    var formData = new FormData(form);

    // Open a request to the form's action link, and send the request
    xmlhttp.open("POST", form.action, true);
    xmlhttp.send(formData);
}

/********************************************
** updateMultipleChoice function definition *
** Changes what is visibile with regards to *
** multiple choice when the dropdown is     *
** changed.                                 *
********************************************/
function updateMultipleChoice(event) {
    // Use the dropdown that was changed to update the visibility
    changeMultipleChoiceVisibility(event.currentTarget);
}

// #endregion