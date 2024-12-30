function searchLogs(event) {
    event.preventDefault();
    var username = document.getElementById("searchUsername").value;

    if (!username) {
        alert("Please enter a username to search.");
        return;
    }

    $.ajax({
        type: "POST",
        url: "WebService/UserRegistration.asmx/SearchFailedLoginLogs",
        data: JSON.stringify({ username: username }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // Clear previous results
            $('#tbdlog').empty();
            if (response.d) {
               
                $("#failedLoginTable").find('#tbdlog').append(response.d.HTMLDataList);
            }
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            console.error("Response Text: " + xhr.responseText);
        }
    });
}

function LoadUserDetails(userId, _emailId, _password) {
    if (!_emailId || !_password) {
        console.error("Email or password is missing.");
        return;
    }

    //alert("Function getting called");
    $.ajax({
        type: "POST",
        url: "WebService/UserRegistration.asmx/LogInfo",
        data: JSON.stringify({
            _email: _emailId,
            _password: _password
        }),
        contentType: "application/json; charset=utf-8", // Corrected case
        dataType: "json",
        success: function (response) {
            if (response.d) {
                //console.log($("#failedLoginTable").find('tbody'));
                //const rows = $.parseHTML(response.d);
                //$("#failedLoginTable").find('tbody').append(rows);
                /*$('#tbdlog').empty();*/
                //$("#failedLoginTable").find('tbody').append(response.d);
                $("#failedLoginTable").find('#tbdlog').append(response.d.HTMLDataList);
            } else {
                console.log("No data returned from the server.");
            }
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            console.error("Response Text: " + xhr.responseText);
        }
    });
}

