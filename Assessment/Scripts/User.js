document.addEventListener("DOMContentLoaded", function () {
function ValidateUser(event) {
    event.preventDefault(); // Prevent default form submission

    let isValid = true;

    // Get form inputs
    const city = document.getElementById("txtCity").value.trim();
    const state = document.getElementById("txtState").value.trim();
    const country = document.getElementById("ddlCountry").value;
    const gender = document.querySelector('input[name="gender"]:checked');
    const dob = document.getElementById("txtDob").value;
    const profileImage = document.getElementById("txtImage").value;

    // Error elements
    const cityError = document.getElementById("cityError");
    const stateError = document.getElementById("StateError");
    const countryError = document.getElementById("CountryError");
    const genderError = document.getElementById("GenderError");
    const dobError = document.getElementById("DobError");
    const profileError = document.getElementById("ProfileError");

    // Reset error messages
    cityError.textContent = "";
    stateError.textContent = "";
    countryError.textContent = "";
    genderError.textContent = "";
    dobError.textContent = "";
    profileError.textContent = "";

    // Validate City
    if (!city) {
        cityError.textContent = "City is required.";
        isValid = false;
    }

    // Validate State
    if (!state) {
        stateError.textContent = "State is required.";
        isValid = false;
    }

    // Validate Country
    if (country === "-1") {
        countryError.textContent = "Please select a country.";
        isValid = false;
    }

    // Validate Gender
    if (!gender) {
        genderError.textContent = "Gender is required.";
        isValid = false;
    }

    // Validate Date of Birth
    if (!dob) {
        dobError.textContent = "Date of Birth is required.";
        isValid = false;
    } else {
        const dobDate = new Date(dob);
        const today = new Date();
        if (dobDate >= today) {
            dobError.textContent = "Date of Birth cannot be today or a future date.";
            isValid = false;
        }
    }

    // Validate Profile Image
    if (!profileImage) {
        profileError.textContent = "Please upload a profile image.";
        isValid = false;
    }

    // Submit form if valid
    //if (isValid) {
    //    alert("Form submitted successfully!");
    //    document.getElementById("userForm").submit(); // Proceed with form submission
    //}
    return isValid;
}

function getQueryParam(name) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}

function fetchUserId() {
    // Fetch the userId from the query string
    const userId = getQueryParam("userId");

    if (userId) {
        console.log(`Fetched UserId: ${userId}`);
        return userId;
    } else {
        alert("UserId not found in the URL.");
        return null; // Return null if userId is not found
    }
}

const userId = fetchUserId();

function UserData(event) {
    if (!ValidateUser(event)) {
        alert("Validation not successful");
        return;
    }

    const _city = document.getElementById("txtCity").value.trim();
    const _state = document.getElementById("txtState").value.trim();
    const _country = document.getElementById("ddlCountry").value;
    const _genderElement = document.querySelector('input[name="gender"]:checked');
    const _gender = _genderElement ? _genderElement.value : null;
    const _dob = document.getElementById("txtDob").value;
    const _imageInput = document.getElementById("txtImage");
    const _profileImageFile = _imageInput.files[0];

    if (!_gender) {
        alert("Please select a gender.");
        return;
    }

    if (!_profileImageFile) {
        alert("Please upload a profile image.");
        return;
    }

    // Convert image to Base64 and submit data
    const reader = new FileReader();
    reader.onload = function () {
        const base64String = reader.result.split(',')[1];

        $.ajax({
            type: "POST",
            url: "WebService/UserRegistration.asmx/UserDetails",
            data: JSON.stringify({
                userId: userId,
                city: _city,
                state: _state,
                country: _country,
                gender: _gender,
                dob: _dob,
                profileImage: base64String
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log("Response:", response);
                if (response.d > 0) {
                    alert("User Data Saved Successfully");
                    
                } else {
                    alert("Failed to Save User Data");
                }
            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
                console.error("Response Text: " + xhr.responseText);
            }
        });
    };

    reader.readAsDataURL(_profileImageFile);
}

// Event listener for form submission
    document.getElementById("userForm").addEventListener("submit", function (event) {
        event.preventDefault(); // Prevent the default form reload
        UserData(event);
    });
});